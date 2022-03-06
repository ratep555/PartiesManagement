using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Blogs
{
    public class BlogComment : BaseEntity
    {
        public int? ParentBlogCommentId { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string CommentContent { get; set; }
        
        
        [DataType(DataType.Date)]
        public DateTime PublishedOn { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdatedOn { get; set; }



    }
}