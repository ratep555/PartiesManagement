using System.Threading.Tasks;
using Core.Interfaces;

namespace Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PartiesContext _context;

        public UnitOfWork(PartiesContext context)
        {
            _context = context;
        }
        public IItemRepository ItemRepository => new ItemRepository(_context);
        public IShippingOptionRepository ShippingOptionRepository => new ShippingOptionRepository(_context);
        public IOrderRepository OrderRepository => new OrderRepository(_context);
        public IBirthdayRepository BirthdayRepository => new BirthdayRepository(_context);

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}