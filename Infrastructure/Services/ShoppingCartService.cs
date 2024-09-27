using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class ShoppingCartService(IConnectionMultiplexer redis) : IShoppingCartService
    {
        private readonly IDatabase db = redis.GetDatabase();

        public async Task<bool> DeleteCartAsync(string key)
        {
            return await db.KeyDeleteAsync(key);
        }

        public async Task<ShoppingCart?> GetCartAsync(string key)
        {
            try {
                 var cart = await db.StringGetAsync(key);
                 return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(cart!);
            } catch( Exception e) {
                throw new Exception(e.Message);
            }
        }

        public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
        {

                var created = await db.StringSetAsync(
                    cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
                if (!created) return null;
                return await  GetCartAsync(cart.Id);
            
        }
    }
}