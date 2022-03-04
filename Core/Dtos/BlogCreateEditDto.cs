using Microsoft.AspNetCore.Http;

namespace Core.Dtos
{
    public class BlogCreateEditDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BlogContent { get; set; }
        public IFormFile Picture { get; set; }
    }
}