// Application/Org/StaffListItemVm.cs
namespace ShiftSolutions.web.Application.Org
{
    public class StaffListItemVm
    {
        public string Id { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string RoleName { get; set; } = "";
        public string DepartmentName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Status { get; set; } = "Active";
        public string AvatarUrl { get; set; } = "/images/users/avatar-3.jpg";
    }

    public class StaffListPageVm
    {
        public string? Q { get; set; }
        public IList<StaffListItemVm> Items { get; set; } = new List<StaffListItemVm>();
    }
}
