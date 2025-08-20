using ShiftSolutions.web.Data;

namespace ShiftSolutions.web.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly AppDbContext _db;

        public PropertyService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<string>> GetAllPropertiesAsync()
        {
            // TODO: Replace with real query
            return new List<string> { "Property 1", "Property 2" };
        }
    }
}
