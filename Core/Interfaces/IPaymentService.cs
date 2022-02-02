using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
     public interface IPaymentService
    {
        Task<ClientBasket> CreatingOrUpdatingPaymentIntent(string basketId);
        Task<CustomerOrder> UpdatingOrderPaymentSucceeded(string paymentIntentId);
        Task<CustomerOrder> UpdatingOrderPaymentFailed(string paymentIntentId);    }
}