using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ShippingOptionRepository : IShippingOptionRepository
    {
        private readonly PartiesContext _context;
        public ShippingOptionRepository(PartiesContext context)
        {
            _context = context;
        }

        public async Task<PaymentOption> GetPaymentOptionById(int id)
        {
            return await _context.PaymentOptions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ShippingOption> GetShippingOptionById(int id)
        {
            return await _context.ShippingOptions.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}





