using BusinessObjectLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class OrderDetailDAO
    {
        public static async Task<List<OrderDetail>> GetOrderDetails()
        {
            
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                   return await context.OrderDetails.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        public static async Task CreateOrderDetail(OrderDetail od)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = od.OrderId,
                        FlowerBouquetId = od.FlowerBouquetId,
                        UnitPrice = od.UnitPrice,
                        Quantity = od.Quantity,
                        Discount = od.Discount
                    };
                   
                    await context.OrderDetails.AddAsync(orderDetail);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<OrderDetail> UpdateOrderDetail(OrderDetail od)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var orderDetail = await context.OrderDetails.FirstOrDefaultAsync(od => od.OrderId == od.OrderId);

                    if (orderDetail != null)
                    {
                        orderDetail.FlowerBouquetId = od.FlowerBouquetId;
                        orderDetail.UnitPrice = od.UnitPrice;
                        orderDetail.Quantity = od.Quantity;
                        orderDetail.Discount = od.Discount;

                        await context.SaveChangesAsync();
                        return orderDetail;
                    }
                    else
                    {
                        throw new Exception("Order detail not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task RemoveOrderDetail(int orderId, int flowerId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var od = await context.OrderDetails.SingleOrDefaultAsync(c => c.OrderId == orderId && c.FlowerBouquetId == flowerId); ;
                    context.OrderDetails.Remove(od);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<OrderDetail> GetOrderDetailByOrderId(int orderId, int flowerBouquetId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    return await context.OrderDetails.FirstOrDefaultAsync(od => od.OrderId == orderId && od.FlowerBouquetId == flowerBouquetId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static Task<List<OrderDetail>> GetOrderDetailsByOrderId(int orderId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    return context.OrderDetails
                        .Where(od => od.OrderId == orderId)
                        .Include(od => od.FlowerBouquet)
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
