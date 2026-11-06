namespace Shop.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;

        // Foreign Key
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}