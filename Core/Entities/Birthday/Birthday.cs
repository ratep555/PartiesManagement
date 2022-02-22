using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Birthday : BaseEntity
    {
        public int LocationId { get; set; }
        public Location Location { get; set; }

        public int BirthdayPackageId { get; set; }
        public BirthdayPackage BirthdayPackage { get; set; }

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
        public OrderStatus1 OrderStatus1 { get; set; }

    }
}
