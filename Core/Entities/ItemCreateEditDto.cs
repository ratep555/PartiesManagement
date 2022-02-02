using Microsoft.AspNetCore.Http;

namespace Core.Entities
{
    public class ItemCreateEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile Picture { get; set; }
    }
}