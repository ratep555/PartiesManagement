using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IItemRepository ItemRepository {get; }
        IShippingOptionRepository ShippingOptionRepository {get; }
        IOrderRepository OrderRepository {get; }
        IBirthdayRepository BirthdayRepository {get; }
        Task<bool> SaveAsync();


    }
}