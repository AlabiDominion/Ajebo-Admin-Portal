// Services/MerchantService.cs
using Microsoft.EntityFrameworkCore;
using ShiftSolutions.web.Application.Merchants; // DTOs + PagedResult + MerchantFilter
using ShiftSolutions.web.Data;
using ShiftSolutions.web.Models;
using System.IO;

namespace ShiftSolutions.web.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly AppDbContext _db;
        private const string AVATAR_BASE_URL = "https://merchants.shifts.com.ng/SharedImages/apartments/";

        public MerchantService(AppDbContext db) => _db = db;

        // ------------- helpers -------------
        private static string? PickImage(params string?[] candidates)
        {
            foreach (var v in candidates)
            {
                var s = (v ?? "").Trim();
                if (string.IsNullOrEmpty(s)) continue;
                if (string.Equals(s, "NA", StringComparison.OrdinalIgnoreCase)) continue;
                return s;
            }
            return null;
        }

        private static string BuildAvatarUrl(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "/images/users/default.jpg";

            // Already absolute?
            if (raw.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                raw.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return raw;

            var file = Path.GetFileName(raw);
            return string.IsNullOrWhiteSpace(file)
                ? "/images/users/default.jpg"
                : AVATAR_BASE_URL + file;
        }

        // =============== LIST (from ApartmentsOnLine) ===============
        public async Task<PagedResult<MerchantListItemDto>> GetMerchantsAsync(
            MerchantFilter filter, CancellationToken ct = default)
        {
            var q = _db.Apartments.AsNoTracking();

            // --- Filters ---
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var s = filter.Search.Trim().ToLower();
                q = q.Where(a =>
                    (a.Agent ?? "").ToLower().Contains(s) ||
                    (a.Name ?? "").ToLower().Contains(s) ||
                    (a.City ?? "").ToLower().Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
                q = q.Where(a => a.Status == filter.Status);

            if (!string.IsNullOrWhiteSpace(filter.City))
                q = q.Where(a => a.City == filter.City);

            if (filter.From.HasValue) q = q.Where(a => a.CreatedAt >= filter.From.Value);
            if (filter.To.HasValue) q = q.Where(a => a.CreatedAt <= filter.To.Value);

            // --- Group by merchant (AgentId) ---
            var merch = q.GroupBy(a => new { a.AgentId, a.Agent });

            // --- Projection (derive avatar from newest apartment images) ---
            var projected = merch.Select(g => new
            {
                g.Key.AgentId,
                DisplayName = g.Key.Agent ?? "(Unknown)",
                Contact = g.Key.Agent ?? "(Unknown)",   // for Contact column
                City = g.Max(x => x.City),
                CreatedAt = g.Min(x => x.CreatedAt),
                ApartmentsCount = g.Count(),
                ApprovalStatus =
                    g.Any(x => x.Status == "Pending") ? "Pending" :
                    g.Any(x => !x.IsApproved) ? "Declined" :
                                                         "Approved",
                RawImage = g.OrderByDescending(x => x.CreatedAt)
                            .Select(x => PickImage(
                                x.ImageUrl, x.ImageName,
                                x.SupportImage1, x.SupportImage2, x.SupportImage3, x.SupportImage4))
                            .FirstOrDefault()
            });

            // --- Sorting ---
            projected = filter.Sort switch
            {
                "name_asc" => projected.OrderBy(m => m.DisplayName),
                "name_desc" => projected.OrderByDescending(m => m.DisplayName),
                "created_asc" => projected.OrderBy(m => m.CreatedAt),
                "created_desc" => projected.OrderByDescending(m => m.CreatedAt),
                "status_asc" => projected.OrderBy(m => m.ApprovalStatus).ThenBy(m => m.DisplayName),
                "status_desc" => projected.OrderByDescending(m => m.ApprovalStatus).ThenBy(m => m.DisplayName),
                _ => projected.OrderByDescending(m => m.CreatedAt)
            };

            // --- Paging ---
            var total = await projected.CountAsync(ct);
            var page = Math.Max(1, filter.Page);
            var size = Math.Clamp(filter.PageSize, 5, 200);
            var skip = (page - 1) * size;

            var rows = await projected.Skip(skip).Take(size).ToListAsync(ct);

            // --- Map to DTOs ---
            var items = rows.Select(m => new MerchantListItemDto
            {
                AgentId = m.AgentId ?? m.DisplayName, // fallback if AgentId is null
                DisplayName = m.DisplayName,
                CompanyName = null,
                Email = null,
                Phone = null,
                City = m.City,
                ApartmentsCount = m.ApartmentsCount,
                ApprovalStatus = m.ApprovalStatus,
                CreatedAt = m.CreatedAt,
                Avatar = BuildAvatarUrl(m.RawImage),
                Contact = m.Contact
            }).ToList();

            return new PagedResult<MerchantListItemDto>
            {
                Items = items,
                Page = page,
                PageSize = size,
                TotalItems = total
            };
        }

        // =============== PROFILE (aggregate apartments by AgentId) ===============
        public async Task<MerchantDto?> GetMerchantAsync(string agentId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(agentId)) return null;

            var apts = await _db.Apartments
                .AsNoTracking()
                .Where(a => a.AgentId == agentId)
                .Select(a => new
                {
                    a.Id,
                    a.AgentId,
                    a.Agent,
                    a.Name,
                    a.City,
                    a.Address,
                    a.Status,
                    a.IsApproved,
                    a.CreatedAt,
                    a.ImageUrl,
                    a.SupportImage1,
                    a.SupportImage2,
                    a.SupportImage3,
                    a.SupportImage4
                })
                .ToListAsync(ct);

            if (apts.Count == 0) return null;

            var approvalStatus =
                apts.Any(x => x.Status == "Pending") ? "Pending" :
                apts.Any(x => !x.IsApproved) ? "Declined" :
                                                        "Approved";

            // Build photos as DTOs first
            var photos = new List<MerchantPhotoDto>();
            void add(string? url, string caption)
            {
                if (!string.IsNullOrWhiteSpace(url) &&
                    !string.Equals(url, "NA", StringComparison.OrdinalIgnoreCase))
                {
                    photos.Add(new MerchantPhotoDto { Url = url!, Caption = caption });
                }
            }
            foreach (var ap in apts)
            {
                add(ap.ImageUrl, ap.Name);
                add(ap.SupportImage1, ap.Name);
                add(ap.SupportImage2, ap.Name);
                add(ap.SupportImage3, ap.Name);
                add(ap.SupportImage4, ap.Name);
            }

            // Convert photos -> plain URL list for MerchantDto.PhotoUrls
            var photoUrls = photos
                .Select(p => p.Url)
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .Distinct()
                .ToList();

            // Build docs (empty for now; fill from DB if/when you have a source)
            var docs = new List<MerchantDocDto>();
            // Example when you add a table for merchant documents:
            // docs = await _db.MerchantDocuments
            //     .Where(d => d.AgentId == agentId)
            //     .OrderByDescending(d => d.UploadedAt)
            //     .Select(d => new MerchantDocDto
            //     {
            //         Name = d.FileName,
            //         Url = d.Url,
            //         Kind = d.Kind,     // "pdf", "image", ...
            //         UploadedAt = d.UploadedAt
            //     })
            //     .ToListAsync(ct);

            // Pick an avatar from the newest apartment images (optional)
            string? newestImage = apts
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => PickImage(x.ImageUrl, x.SupportImage1, x.SupportImage2, x.SupportImage3, x.SupportImage4))
                .FirstOrDefault();
            var avatarUrl = BuildAvatarUrl(newestImage);

            return new MerchantDto
            {
                AgentId = agentId,
                DisplayName = apts[0].Agent ?? "(Unknown)",
                CompanyName = "",
                Email = "",
                Phone = "",
                Address = "",
                City = apts[0].City ?? "",
                ApprovalStatus = approvalStatus,
                DeclineReason = "",
                CreatedAt = apts.Min(x => x.CreatedAt),
                AvatarUrl = avatarUrl,
                ApartmentsCount = apts.Count,

                Documents = docs,       // List<MerchantDocDto>
                PhotoUrls = photoUrls    // List<string>
            };
        }


        // =============== AGENT-LEVEL APPROVAL (updates all rows for AgentId) ===============
        // Services/MerchantService.cs (inside class)
        public async Task ApproveMerchantAsync(
            string agentId,
            string approvedByUserId,
            int assignedStaffId,
            CancellationToken ct = default)
        {
            // 1) Approve all apartments for this merchant
            var aps = await _db.Apartments.Where(a => a.AgentId == agentId).ToListAsync(ct);
            if (aps.Count == 0) throw new KeyNotFoundException("Merchant (AgentId) not found.");

            foreach (var a in aps)
            {
                a.Status = "Approved";
                a.IsApproved = true;
                a.UpdatedAt = DateTime.UtcNow;
            }

            // 2) Ensure staff exists
            var staffExists = await _db.Staff.AnyAsync(s => s.Id == assignedStaffId, ct);
            if (!staffExists) throw new KeyNotFoundException("Staff not found.");

            // 3) Link staff ↔ merchant (if not already linked)
            var linkExists = await _db.MerchantStaff
                .AnyAsync(x => x.StaffId == assignedStaffId && x.AgentId == agentId, ct);

            if (!linkExists)
            {
                _db.MerchantStaff.Add(new MerchantStaff
                {
                    StaffId = assignedStaffId,
                    AgentId = agentId,
                    AssignedAtUtc = DateTime.UtcNow,
                    AssignedByUserId = approvedByUserId
                });
            }

            await _db.SaveChangesAsync(ct);

            // 4) Audit (optional)
            _db.MerchantDecisions.Add(new MerchantDecision
            {
                AgentId = agentId,
                Action = MerchantDecisionType.Approved,
                Reason = null,
                ByUserId = approvedByUserId,
                AtUtc = DateTime.UtcNow,
                AffectedApartments = aps.Count,
                AssignedStaffId = assignedStaffId
            });
            await _db.SaveChangesAsync(ct);
        }
        public async Task DetachStaffFromMerchantAsync(string agentId, int staffId, CancellationToken ct = default)
        {
            var link = await _db.MerchantStaff
                .FirstOrDefaultAsync(x => x.AgentId == agentId && x.StaffId == staffId, ct);
            if (link != null)
            {
                _db.MerchantStaff.Remove(link);
                await _db.SaveChangesAsync(ct);
            }
        }


        public async Task DeclineMerchantAsync(string agentId, string reason, string declinedByUserId, CancellationToken ct = default)
        {
            var rows = await _db.Apartments.Where(a => a.AgentId == agentId).ToListAsync(ct);
            if (rows.Count == 0) throw new KeyNotFoundException("Merchant (by AgentId) not found.");

            foreach (var a in rows)
            {
                a.Status = "Declined";
                a.IsApproved = false;
                a.UpdatedAt = DateTime.UtcNow;
            }
            await _db.SaveChangesAsync(ct);

            _db.MerchantDecisions.Add(new MerchantDecision
            {
                AgentId = agentId,
                Action = MerchantDecisionType.Declined,
                Reason = reason,
                ByUserId = declinedByUserId,
                AtUtc = DateTime.UtcNow,
                AffectedApartments = rows.Count
            });
            await _db.SaveChangesAsync(ct);
        }

        // =============== PER-APARTMENT MODERATION (optional) ===============
        public async Task ApproveApartmentAsync(int apartmentId, string approvedByUserId, CancellationToken ct = default)
        {
            var ap = await _db.Apartments.FirstOrDefaultAsync(x => x.Id == apartmentId, ct)
                     ?? throw new KeyNotFoundException("Apartment not found.");
            ap.Status = "Approved";
            ap.IsApproved = true;
            ap.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeclineApartmentAsync(int apartmentId, string reason, string declinedByUserId, CancellationToken ct = default)
        {
            var ap = await _db.Apartments.FirstOrDefaultAsync(x => x.Id == apartmentId, ct)
                     ?? throw new KeyNotFoundException("Apartment not found.");
            ap.Status = "Declined";
            ap.IsApproved = false;
            ap.UpdatedAt = DateTime.UtcNow;
            // Optionally store reason in a free text field if you have one (e.g., InternalService)
            await _db.SaveChangesAsync(ct);
        }
    }
}
