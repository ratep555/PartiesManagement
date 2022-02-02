using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly PartiesContext _context;
        public ItemRepository(PartiesContext context)
        {
            _context = context;
        }

        public void AddItem(Item item)
        {
            _context.Items.Add(item);
        }

        public async Task<List<Item>> GetAllItems(QueryParameters queryParameters)
        {
            IQueryable<Item> items = _context.Items.AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                items = items.Where(t => t.Name.Contains(queryParameters.Query));
            }

            items = items.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await items.ToListAsync();
        }

        public async Task<int> GetCountForItems()
        {
            return await _context.Items.CountAsync();
        }

        public async Task<Item> GetItemById(int id)
        {
            return await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
        }


    }
}