using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class BlogDto
    {
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }
        public string Title { get; set; }
        public string BlogContent { get; set; }
        public string Picture { get; set; }
        public string Username { get; set; }


        [DataType(DataType.Date)]
        public DateTime PublishedOn { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdatedOn { get; set; }
    }
}