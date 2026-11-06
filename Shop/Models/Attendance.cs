namespace Shop.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public DateTime CheckInTime { get; set; } = DateTime.Now; // Jab staff ne submit kiya
        public DateTime? CheckOutTime { get; set; } // Jab duty khatam karke gaya
        public string Status { get; set; } = "Present";
    }
}