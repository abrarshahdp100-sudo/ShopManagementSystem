using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StaffController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Staff Check-In (Jab banda dukaan par aakar submit karega)
        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn([FromBody] AttendanceDto dto)
        {
            // Check karein ke aaj ki entry already toh nahi hui
            var existingAttendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.StaffId == dto.StaffId && a.Date == DateTime.Today);

            if (existingAttendance != null)
            {
                return BadRequest("Aap aaj ki attendance pehle hi mark kar chuke hain!");
            }

            var attendance = new Attendance
            {
                StaffId = dto.StaffId,
                Date = DateTime.Today,
                CheckInTime = DateTime.Now,
                Status = "Present"
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Check-in successful! Attendance recorded.", attendance.CheckInTime });
        }

        // Staff Check-Out (Jab 12 ghante ki duty khatam karke jaye)
        [HttpPost("check-out/{staffId}")]
        public async Task<IActionResult> CheckOut(int staffId)
        {
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.StaffId == staffId && a.Date == DateTime.Today && a.CheckOutTime == null);

            if (attendance == null)
                return NotFound("Koi active check-in nahi mila aaj ke liye!");

            attendance.CheckOutTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Check-out successful!", attendance.CheckOutTime });
        }

        // Staff Register / Add
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Staff staff)
        {
            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Staff registered successfully!", staff });
        }

        // Staff Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var staff = await _context.Staffs
                .FirstOrDefaultAsync(s => s.Email == loginDto.Email && s.Password == loginDto.Password);

            if (staff == null)
                return Unauthorized("Invalid email or password!");

            return Ok(new { Message = "Login successful!", StaffName = staff.Name, Role = staff.Role });
        }

        // Simple Dashboard Data
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var totalShops = await _context.Shops.CountAsync();
            var totalProducts = await _context.Products.CountAsync();
            var totalSales = await _context.Sales.SumAsync(s => (decimal?)s.TotalPrice) ?? 0;

            return Ok(new
            {
                TotalShops = totalShops,
                TotalProducts = totalProducts,
                TotalRevenue = totalSales
            });
        }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AttendanceDto
    {
        public int StaffId { get; set; }
    }
}