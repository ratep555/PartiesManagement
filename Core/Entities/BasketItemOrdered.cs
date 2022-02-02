namespace Core.Entities
{
    public class BasketItemOrdered
    {
        public BasketItemOrdered()
        {
            
        }
        public BasketItemOrdered(int basketItemOdreredId, string basketItemOrderedName)
        {
            BasketItemOrderedId = basketItemOdreredId;
            BasketItemOrderedName = basketItemOrderedName;
        }
        public int BasketItemOrderedId { get; set; }
        public string BasketItemOrderedName { get; set; }

    }
}