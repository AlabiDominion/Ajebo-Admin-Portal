namespace ShiftSolutions.web.Application.Merchants;

public class MerchantListItemDto
{
    public string AgentId { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? CompanyName { get; set; }
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? City { get; set; }
    public string ApprovalStatus { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public int ApartmentsCount { get; set; }
    public string? Avatar { get; set; }
    public string? Contact { get; set; }
}

public class MerchantDocDto
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string Kind { get; set; } // "pdf", "image", ...
    public DateTime UploadedAt { get; set; }
}

public class MerchantPhotoDto
{
    public string Url { get; set; }
    public string Caption { get; set; }
}

public class MerchantDto
{
    public string AgentId { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string CompanyName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string City { get; set; } = "";
    public string ApprovalStatus { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
    public string ContactName { get; set; } = "";
    public string ContactPhone { get; set; } = "";
    public string Address { get; set; } = "";
    public string BankName { get; set; } = "";
    public string BankAccountNumber { get; set; } = "";
    public string BankAccountName { get; set; } = "";
    public int ApartmentsCount { get; set; }
    public string? Notes { get; set; }
    public string AvatarUrl { get; set; } = "";
    public string DeclineReason { get; set; } = "";

    public List<string> PhotoUrls { get; set; } = new();
    public List<MerchantDocDto> Documents { get; set; } = new();
}


// simple page envelope
public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / Math.Max(PageSize, 1));
}

public class MerchantFilter
{
    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? City { get; set; }          // <-- add this
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Sort { get; set; } = "created-desc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}