// ShiftSolutions.web/Controllers/OpTransactionController.cs
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftSolutions.web.Data;
using ShiftSolutions.web.Models;

namespace ShiftSolutions.web.Controllers
{
    public class OpTransactionController : Controller
    {
        private readonly AppDbContext _db;
        public OpTransactionController(AppDbContext db) => _db = db;

        // LIST + FILTERS
        public async Task<IActionResult> OrdersBooking(string? q, string? type, string? status, DateTime? from, DateTime? to)
        {
            var query = ApplyFilters(_db.Bookings.AsQueryable(), q, type, status, from, to);

            var bookings = await query
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            // keep selected values for the view
            ViewBag.Q = q;
            ViewBag.Type = type;
            ViewBag.Status = status;
            ViewBag.From = from?.ToString("yyyy-MM-dd");
            ViewBag.To = to?.ToString("yyyy-MM-dd");

            return View("BookingsList", bookings); 
        }

        // DETAILS
        public async Task<IActionResult> OrderDetails(int id)
        {
            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return NotFound();
            return View("OrdersDetailes", booking); 
        }

        // EXCEL EXPORT (respects same filters)
        [HttpGet]
        public async Task<FileResult> ExportExcel(string? q, string? type, string? status, DateTime? from, DateTime? to)
        {
            var query = ApplyFilters(_db.Bookings.AsNoTracking().AsQueryable(), q, type, status, from, to);

            var bookings = await query
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Bookings");

            // Header
            string[] headers = new[]
            {
                "Id","BookingId","BookingDate","ClientName","ClientEmail","ApartmentId","ApartmentName",
                "ApartmentAddress","ApartmentDescription","ApartmentCity","PricePerNight","StarRatings",
                "StayDuration","TransactionId","TotalAmount","NumberOfNights","PaymentMethod",
                "TransactionReferenceId","PaymentStatus","VatPercentage","Vat","SubTotal","CreatedAt",
                "UpdatedAt","IsActive"
            };
            for (int c = 0; c < headers.Length; c++)
                worksheet.Cell(1, c + 1).Value = headers[c];

            // Rows
            for (int i = 0; i < bookings.Count; i++)
            {
                var b = bookings[i];
                int r = i + 2;
                worksheet.Cell(r, 1).Value = b.Id;
                worksheet.Cell(r, 2).Value = b.BookingId;
                worksheet.Cell(r, 3).Value = b.BookingDate;
                worksheet.Cell(r, 4).Value = b.ClientName;
                worksheet.Cell(r, 5).Value = b.ClientEmailAddress;
                worksheet.Cell(r, 6).Value = b.ApartmentId;
                worksheet.Cell(r, 7).Value = b.ApartmentName;
                worksheet.Cell(r, 8).Value = b.ApartmentAddress;
                worksheet.Cell(r, 9).Value = b.ApartmentDescription;
                worksheet.Cell(r, 10).Value = b.ApartmentCity;
                worksheet.Cell(r, 11).Value = b.ApartmentPricePerNight;
                worksheet.Cell(r, 12).Value = b.ApartmentStarRatings;
                worksheet.Cell(r, 13).Value = b.ApartmentStayDuration;
                worksheet.Cell(r, 14).Value = b.TransactionId;
                worksheet.Cell(r, 15).Value = b.TotalAmount;
                worksheet.Cell(r, 16).Value = b.NumberOfNights;
                worksheet.Cell(r, 17).Value = b.PaymentMethod;
                worksheet.Cell(r, 18).Value = b.TransactionReferenceId;
                worksheet.Cell(r, 19).Value = b.PaymentStatus;
                worksheet.Cell(r, 20).Value = b.VatPercentage;
                worksheet.Cell(r, 21).Value = b.Vat;
                worksheet.Cell(r, 22).Value = b.SubTotal;
                worksheet.Cell(r, 23).Value = b.CreatedAt;
                worksheet.Cell(r, 24).Value = b.UpdatedAt;
                worksheet.Cell(r, 25).Value = b.IsActive ? "Yes" : "No";
            }

            worksheet.Column(3).Style.DateFormat.Format = "yyyy-mm-dd";
            worksheet.Column(24).Style.DateFormat.Format = "yyyy-mm-dd";
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Bookings_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx"
            );
        }

        // Other pages (if you still use them)
        public IActionResult OrdersDetailes() => View();
        public IActionResult Settlements() => View();
        public IActionResult BatchDetailes() => View();

        // ---------- helpers ----------
        private static IQueryable<Booking> ApplyFilters(
            IQueryable<Booking> query,
            string? q, string? type, string? status, DateTime? from, DateTime? to)
        {
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(b =>
                    (b.BookingId ?? "").Contains(q) ||
                    (b.ClientName ?? "").Contains(q) ||
                    (b.ClientEmailAddress ?? "").Contains(q) ||
                    (b.ApartmentName ?? "").Contains(q) ||
                    (b.ApartmentCity ?? "").Contains(q) ||
                    (b.TransactionId ?? "").Contains(q) ||
                    (b.TransactionReferenceId ?? "").Contains(q));
            }

            // reserved for future use (orders vs bookings)
            if (!string.IsNullOrWhiteSpace(type) && type != "All")
            {
                // e.g., query = query.Where(b => type == "Booking");
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
                query = query.Where(b => b.PaymentStatus == status);

            if (from.HasValue)
                query = query.Where(b => b.CreatedAt >= from.Value.Date);

            if (to.HasValue)
                query = query.Where(b => b.CreatedAt < to.Value.Date.AddDays(1));

            return query;
        }
    }
}
