using BusinessObjectLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ICardRepo
    {
        Task AddFlowerBouquetToCart(int customerId, int flowerId, int quantity);
        Task RemoveFlowerBouquetFromCart(int customerId, int flowerBouquetId);
        Task<Order> CheckoutCart(int customerId);
        Task<Order> GetCartItems(int customerId);
        Task UpdateFlowerBouquetQuantity(int customerId, int flowerId, int quantity);
    }
}
