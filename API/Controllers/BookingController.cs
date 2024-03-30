using BookingApp.DTO;
using BookingApp.DTO.Filter;
using BookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.IO;
using System;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Data;
namespace BookingApp.Controllers
{
    public class BookingController : ApiControllerBase
    {
        private readonly IBookingService _service;
        public BookingController(IBookingService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetByIDAsync(int id)
        {
            return Ok(await _service.GetByIDAsync(id));
        }
        [HttpGet]
        public async Task<ActionResult> SearchBooking([FromQuery] BookingFilter bookingFilter)
        {
            return Ok(await _service.SearchBooking(bookingFilter));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] BookingDto model)
        {
            return Ok(await _service.AddAsync(model));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] BookingDto model)
        {
            return Ok(await _service.UpdateAsync(model));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateStatus(int id, int status)
        {
            return Ok(await _service.UpdateStatus(id, status));
        }


        [HttpGet]
        public IActionResult ExportToExcel() // sua lai dung epplus
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var data = _service.GetBookingData(DateTime.Now);
            using (ExcelPackage package = new ExcelPackage())
            {
                package.Workbook.Worksheets.Add("Booking");
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells["A1"].LoadFromDataTable(data, true);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    package.SaveAs(memoryStream);
                    var name = $"BKA-Booking_Report_{DateTime.Now.ToString("yyyy-MM-dd")}.xlsx";
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", name);
                }
            }
        }
        [HttpGet]
        public IActionResult ExportToCSV()
        {
            var data = _service.GetBookingData(DateTime.Now);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                    {
                        streamWriter.WriteLine("BookingID,BookingDate,BookingTimeS,BookingTimeE,RoomGuid,FloorGuid,BuildingGuid,CampusGuid,UserGuid,BookingStatus");
                        foreach (DataRow row in data.Rows)
                        {
                            streamWriter.WriteLine(
                                $"{row["BookingID"]}," +
                                $"{row["BookingDate"]}," +
                                $"{row["BookingTimeS"]}," +
                                $"{row["BookingTimeE"]}," +
                                $"{row["RoomGuid"]}," +
                                $"{row["FloorGuid"]}," +
                                $"{row["BuildingGuid"]}," +
                                $"{row["CampusGuid"]}," +
                                $"{row["UserGuid"]}," +
                                $"{row["BookingStatus"]} "
                                );
                        }
                    }
                    var name = $"BKA-Booking_Report_{DateTime.Now.ToString("yyyy-MM-dd")}.csv";
                    return File(memoryStream.ToArray(), "text/csv", name);
                }
            
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return Ok(await _service.DeleteAsync(id));
        }
    }
}
