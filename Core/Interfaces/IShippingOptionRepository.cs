using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IShippingOptionRepository
    {
        Task<ShippingOption> GetShippingOptionById(int id);
        Task<PaymentOption> GetPaymentOptionById(int id);

    }
}