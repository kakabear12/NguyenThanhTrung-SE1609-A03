using BusinessObjectLayer.Models;
using DataAccessLayer;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CardRepo : ICardRepo
    {
        public async Task AddFlowerBouquetToCart(int customerId, int flowerId, int quantity)
        {
            await FlowerBouquetCart.AddFlowerBouquetToCart(customerId, flowerId, quantity);
        }

        public async Task<Order> CheckoutCart(int customerId)
        {
            return await FlowerBouquetCart.CheckoutCart(customerId);
        }

        public async Task<Order> GetCartItems(int customerId)
        {
            return await FlowerBouquetCart.GetCartItems(customerId);
        }

        public async Task RemoveFlowerBouquetFromCart(int customerId, int flowerBouquetId)
        {
           await FlowerBouquetCart.RemoveFlowerBouquetFromCart(customerId, flowerBouquetId);
        }

        public async Task UpdateFlowerBouquetQuantity(int customerId, int flowerId, int quantity)
        {
            await FlowerBouquetCart.UpdateFlowerBouquetQuantity(customerId, flowerId, quantity);
        }
    }
}
