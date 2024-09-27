using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IShoppingCartService
    {
        Task<bool> DeleteCartAsync (string key);
        Task<ShoppingCart?> GetCartAsync (string key);
        Task<ShoppingCart?> SetCartAsync (ShoppingCart cart);
    }
}