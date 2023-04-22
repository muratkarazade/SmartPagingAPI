using Enoca.Task.Pagination.Data;
using Enoca.Task.Pagination.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enoca.Task.Pagination.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        // "pageSize" parametresi eklenerek kullanıcının istediği ürün sayısını belirlemesi sağlandı
        [HttpGet("{page}/{pageSize}")]
        public async Task<ActionResult<List<Product>>> GetProducts(int page, int pageSize)
        {
            if (_context.Products == null)
                return NotFound();

            // Toplam sayfa sayısını hesaplar
            var pageCount = Math.Ceiling(_context.Products.Count() / (float)pageSize);

            // Veritabanından ürünleri alarak sayfalama işlemi gerçekleştirir
            var products = await _context.Products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new ProductResponse
            {
                Products = products,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return Ok(response);
        }
    }
}
