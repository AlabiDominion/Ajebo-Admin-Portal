using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShiftSolutions.web.Data;
using System.Data.Entity;

namespace ShiftSolutions.web.Controllers
{
    public class MerchantController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        public MerchantController(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }
        public async  Task<IActionResult> MerchantList()
        {
            GetMerchantsData();
            return View();
          
        }        
        public async Task<IActionResult> GetMerchantsData()
        {
            var merchants = new List<object>();

            string connectionString = _configuration.GetConnectionString("Ajebos");
            string sql = @"                            
                        SELECT [Name],
                        [City],
                        [Agent],
                        [Statetext],
                        [imagename],
                        CONVERT(DATE, CreatedAt) as created,
                        [status]
                        FROM [dbo].[Apartments] 
                        WHERE status='Pending'";

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            merchants.Add(new
                            {
                                id = reader["Name"].ToString(),
                                name = reader["City"].ToString(),
                                email = reader["Agent"].ToString(),
                                phone = reader["Statetext"].ToString(),
                                city = reader["CreatedAt"].ToString(),
                                status = reader["status"].ToString(),
                                created = reader["created"].ToString(),
                                avatar = "https://merchants.shifts.com.ng/SharedImages/apartments/"+"default2.jpg",
                            });
                        }
                    }
                }
            }

            return Json(merchants);
        }
        public IActionResult MerchantProfile()
        {
            return View();
        }
    }
}
