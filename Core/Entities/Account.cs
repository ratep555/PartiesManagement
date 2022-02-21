namespace Core.Entities
{
    public class Account : BaseEntity
    {
        public string BankName { get; set; }
        public string IBAN { get; set; }
    }
}