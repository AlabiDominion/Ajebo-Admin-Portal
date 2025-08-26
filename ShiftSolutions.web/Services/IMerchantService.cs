// Application/Merchants/IMerchantService.cs
using ShiftSolutions.web.Application.Merchants;
using System.Threading;

namespace ShiftSolutions.web.Services
{
    public interface IMerchantService
    {
        // MERCHANT LIST (grouped by AgentId from ApartmentsOnLine)
        Task<PagedResult<MerchantListItemDto>> GetMerchantsAsync(
            MerchantFilter filter, CancellationToken ct = default);

        // MERCHANT PROFILE (aggregated from that agent's apartments)
        Task<MerchantDto?> GetMerchantAsync(
            string agentId, CancellationToken ct = default);

        // Agent-level decision: updates ALL apartments for that AgentId
        Task ApproveMerchantAsync(string agentId, string approvedByUserId, int assignedStaffId, CancellationToken ct = default);
        Task DeclineMerchantAsync(string agentId, string reason, string declinedByUserId, CancellationToken ct = default);

        // (Optional) Per-apartment moderation if you need it
        Task ApproveApartmentAsync(int apartmentId, string approvedByUserId, CancellationToken ct = default);
        Task DeclineApartmentAsync(int apartmentId, string reason, string declinedByUserId, CancellationToken ct = default);

        // 🔹 NEW: list merchants assigned to a specific staff member
        Task<PagedResult<MerchantListItemDto>> GetMerchantsForStaffAsync(
            int staffId, MerchantFilter filter, CancellationToken ct = default);
    }
}
