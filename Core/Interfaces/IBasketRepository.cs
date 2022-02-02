using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<ClientBasket> GetBasket(string basketId);
        Task<ClientBasket> EditBasket(ClientBasket basket);
        Task<bool> DeleteBasket(string basketId);
    }
}