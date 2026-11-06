using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShopsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Saari Shops aur unke Products/Stock dekhne ke liye
        [HttpGet]
        public async Task<IActionResult> GetShops()
        {
            var shops = await _context.Shops
                .Include(s => s.Products)
                .ToListAsync();
            return Ok(shops);
        }

        // 2. Nayi Shop add karne ke liye
        [HttpPost("add-shop")]
        public async Task<IActionResult> AddShop([FromBody] ShopModel shop)
        {
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();
            return Ok(shop);
        }

        // 3. Kisi Shop mein Product, Price aur Stock add karne ke liye
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            var shop = await _context.Shops.FindAsync(product.ShopModelId);
            if (shop == null) return NotFound("Shop nahi mili!");

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        // 4. Sale record karne aur Stock update karne ke liye
        [HttpPost("record-sale")]
        public async Task<IActionResult> RecordSale([FromBody] Sale sale)
        {
            var product = await _context.Products.FindAsync(sale.ProductId);
            if (product == null) return NotFound("Product nahi mila!");

            // Check karo ke stock poora hai ya nahi
            if (product.StockQuantity < sale.QuantitySold)
            {
                return BadRequest("Stock khatam hai ya quantity zyada hai!");
            }

            // Stock kam karo
            product.StockQuantity -= sale.QuantitySold;

            // Total price calculate karo (Price * Quantity)
            sale.TotalPrice = product.Price * sale.QuantitySold;
            sale.SaleDate = DateTime.Now;

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Sale successfully record ho gayi!", Sale = sale, RemainingStock = product.StockQuantity });
        }
    }
}