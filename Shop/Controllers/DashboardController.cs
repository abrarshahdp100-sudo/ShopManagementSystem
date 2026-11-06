using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Owner ke liye live attendance list (Kaun kab aaya)
        [HttpGet("today-attendance")]
        public async Task<IActionResult> GetTodayAttendance()
        {
            var today = DateTime.Today;
            var attendanceList = await _context.Attendances
                .Where(a => a.Date == today)
                .Join(_context.Staffs,
                    att => att.StaffId,
                    staff => staff.Id,
                    (att, staff) => new
                    {
                        StaffName = staff.Name,
                        Email = staff.Email,
                        CheckInTime = att.CheckInTime.ToString("hh:mm tt"),
                        CheckOutTime = att.CheckOutTime != null ? att.CheckOutTime.Value.ToString("hh:mm tt") : "On Duty",
                        Status = att.Status
                    })
                .ToListAsync();

            return Ok(attendanceList);
        }
    }
}