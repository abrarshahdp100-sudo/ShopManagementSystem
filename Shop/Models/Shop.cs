namespace Shop.Models
{
    public class ShopModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Relationship: Ek shop ke paas bohat se products ho sakte hain
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}