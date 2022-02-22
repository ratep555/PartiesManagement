using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Birthday
{
    public class BirthdayEditDto
    {
         public int Id { get; set; }  
        public int LocationId { get; set; }
        public int BirthdayPackageId { get; set; }
        public string ClientName { get; set; }
        public string BirthdayGirlBoyName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public int NumberOfGuests { get; set; }
        public string Remarks { get; set; }
        public int BirthdayNo { get; set; }
        public decimal Price { get; set; }
        public int? OrderStatus1Id { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime StartDateAndTime { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime EndDateAndTime { get; set; }
    }
}