using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Blogs
{
    public class Blog : BaseEntity
    {
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Title { get; set; }
        public string BlogContent { get; set; }
        public string Picture { get; set; }


        [DataType(DataType.Date)]
        public DateTime PublishedOn { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdatedOn { get; set; }
    }
}