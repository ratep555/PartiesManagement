using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAllItems(QueryParameters queryParameters);
        Task<int> GetCountForItems();
        Task<Item> GetItemById(int id);
        void AddItem(Item item);

    }
}