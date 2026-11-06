namespace Shop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        // Foreign Key
        public int ShopModelId { get; set; }
        public ShopModel? Shop { get; set; }

        // Relationship: Ek product ki bohat si sales ho sakti hain
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}