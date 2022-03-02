using Microsoft.AspNetCore.Http;

namespace Core.Dtos.Birthday
{
    public class ServiceIncludedCreateEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Picture { get; set; }
        public string VideoClip { get; set; }
    }
}