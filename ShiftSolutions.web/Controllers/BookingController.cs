using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftSolutions.web.Data;
using ShiftSolutions.web.Models;
using System.Data;

namespace ShiftSolutions.web.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _db;
        public BookingController(AppDbContext db) => _db = db;

        public async Task<IActionResult> List()
        {
            var bookings = await _db.Bookings
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        [HttpGet]
        public async Task<FileResult> ExportExcel()
        {
            var bookings = await _db.Bookings.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Bookings");

            // Header
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "BookingId";
            worksheet.Cell(1, 3).Value = "BookingDate";
            worksheet.Cell(1, 4).Value = "ClientName";
            worksheet.Cell(1, 5).Value = "ClientEmail";
            worksheet.Cell(1, 6).Value = "ApartmentId";
            worksheet.Cell(1, 7).Value = "ApartmentName";
            worksheet.Cell(1, 8).Value = "ApartmentAddress";
            worksheet.Cell(1, 9).Value = "ApartmentDescription";
            worksheet.Cell(1, 10).Value = "ApartmentCity";
            worksheet.Cell(1, 11).Value = "PricePerNight";
            worksheet.Cell(1, 12).Value = "StarRatings";
            worksheet.Cell(1, 13).Value = "StayDuration";
            worksheet.Cell(1, 14).Value = "TransactionId";
            worksheet.Cell(1, 15).Value = "TotalAmount";
            worksheet.Cell(1, 16).Value = "NumberOfNights";
            worksheet.Cell(1, 17).Value = "PaymentMethod";
            worksheet.Cell(1, 18).Value = "TransactionReferenceId";
            worksheet.Cell(1, 19).Value = "PaymentStatus";
            worksheet.Cell(1, 20).Value = "VatPercentage";
            worksheet.Cell(1, 21).Value = "Vat";
            worksheet.Cell(1, 22).Value = "SubTotal";
            worksheet.Cell(1, 23).Value = "CreatedAt";
            worksheet.Cell(1, 24).Value = "UpdatedAt";
            worksheet.Cell(1, 25).Value = "IsActive";

            // Rows
            for (int i = 0; i < bookings.Count; i++)
            {
                var b = bookings[i];
                worksheet.Cell(i + 2, 1).Value = b.Id;
                worksheet.Cell(i + 2, 2).Value = b.BookingId;
                worksheet.Cell(i + 2, 3).Value = b.BookingDate;
                worksheet.Cell(i + 2, 4).Value = b.ClientName;
                worksheet.Cell(i + 2, 5).Value = b.ClientEmailAddress;
                worksheet.Cell(i + 2, 6).Value = b.ApartmentId;
                worksheet.Cell(i + 2, 7).Value = b.ApartmentName;
                worksheet.Cell(i + 2, 8).Value = b.ApartmentAddress;
                worksheet.Cell(i + 2, 9).Value = b.ApartmentDescription;
                worksheet.Cell(i + 2, 10).Value = b.ApartmentCity;
                worksheet.Cell(i + 2, 11).Value = b.ApartmentPricePerNight;
                worksheet.Cell(i + 2, 12).Value = b.ApartmentStarRatings;
                worksheet.Cell(i + 2, 13).Value = b.ApartmentStayDuration;
                worksheet.Cell(i + 2, 14).Value = b.TransactionId;
                worksheet.Cell(i + 2, 15).Value = b.TotalAmount;
                worksheet.Cell(i + 2, 16).Value = b.NumberOfNights;
                worksheet.Cell(i + 2, 17).Value = b.PaymentMethod;
                worksheet.Cell(i + 2, 18).Value = b.TransactionReferenceId;
                worksheet.Cell(i + 2, 19).Value = b.PaymentStatus;
                worksheet.Cell(i + 2, 20).Value = b.VatPercentage;
                worksheet.Cell(i + 2, 21).Value = b.Vat;
                worksheet.Cell(i + 2, 22).Value = b.SubTotal;
                worksheet.Cell(i + 2, 23).Value = b.CreatedAt;
                worksheet.Cell(i + 2, 24).Value = b.UpdatedAt?.ToString("yyyy-MM-dd");
                worksheet.Cell(i + 2, 25).Value = b.IsActive ? "Yes" : "No";
            }

            worksheet.Columns().AdjustToContents(); // Auto fit columns

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Bookings_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx");
        }
    }
}
