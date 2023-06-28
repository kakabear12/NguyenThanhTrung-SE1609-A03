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
    public class OrderRepo : IOrderRepo
    {
        public async Task<List<Order>> GetOrderHistory(int customerId)
        {
            return await OrderDAO.GetOrderHistory(customerId);
        }
    }
}
