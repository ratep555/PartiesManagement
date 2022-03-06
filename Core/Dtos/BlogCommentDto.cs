using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class BlogCommentDto
    {
        public int Id { get; set; }
        public int? ParentBlogCommentId { get; set; }
        public int BlogId { get; set; }
        public string CommentContent { get; set; }  
        public int ApplicationUserId { get; set; }
        public string Username { get; set; }


        [DataType(DataType.Date)]
        public DateTime PublishedOn { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdatedOn { get; set; }
    }
}