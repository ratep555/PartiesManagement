using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Message : BaseEntity
    {
        public int ApplicationUserId { get; set; }
        public ApplicationUser Receiver { get; set; }
        public string FirstLastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MessageContent { get; set; }

        [DataType(DataType.Date)]
        public DateTime SendingDate { get; set; } = DateTime.UtcNow;

    }
}