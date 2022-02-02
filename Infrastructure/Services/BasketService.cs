using System;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    // trebao bi ovo nazvati repository obzirom da je redis inmemory database
    public class BasketService : IBasketRepository
    {
        private readonly IDatabase _db;
        public BasketService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasket(string basketId)
        {
            return await _db.KeyDeleteAsync(basketId);
        }

        public async Task<ClientBasket> EditBasket(ClientBasket basket)
        {
            var updated = await _db.StringSetAsync(basket.Id, 
                JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (!updated) return null;

            return await GetBasket(basket.Id);       
        }

        public async Task<ClientBasket> GetBasket(string basketId)
        {
            var basket = await _db.StringGetAsync(basketId);

            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ClientBasket>(basket);
        }
    }
}







