namespace ShiftSolutions.web.Services
{
    public interface IPropertyService
    {
        Task<IEnumerable<string>> GetAllPropertiesAsync();
    }
}
