using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Birthday
{
    public class BirthdayDto
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string BirthdayPackage { get; set; }
        public string OrderStatus { get; set; }
        public string ClientName { get; set; }
        public string BirthdayGirlBoyName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public int NumberOfGuests { get; set; }
        public string Remarks { get; set; }
        public int BirthdayNo { get; set; }
        public decimal Price { get; set; }


        [DataType(DataType.Date)]
        public DateTime StartDateAndTime { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime EndDateAndTime { get; set; }

        public int? OrderStatus1Id { get; set; }
        public int Location1Id { get; set; }
        public int BirthdayPackageId { get; set; }
    }
}