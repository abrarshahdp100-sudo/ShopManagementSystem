namespace Shop.Models
{
    public class CashClosing
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public decimal OpeningCash { get; set; }
        public decimal TotalCashSales { get; set; }
        public decimal ClosingCashExpected { get; set; }
        public decimal ClosingCashActual { get; set; }
        public decimal Difference { get; set; } // Shortage or Excess
        public string Notes { get; set; }
    }
}