using BusinessObjectLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class OrderDAO
    {
        public static async Task<List<Order>> GetOrders()
        {
            
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                  var listOrders = await context.Orders.ToListAsync();
                   return listOrders;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public static async Task<List<Order>> GetOrderHistory(int customerId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var orderHistory = await context.Orders.Where(o => o.CustomerId == customerId && (o.OrderStatus == "Done" || o.OrderStatus == "Cancel")).ToListAsync();
                    return orderHistory;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task CreateOrder(int customerId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var order = new Order
                    {
                        CustomerId = customerId,
                        OrderDate = DateTime.Now,
                        ShippedDate = DateTime.Now.AddMinutes(30), // Thêm 30 phút vào ShippedDate
                        OrderStatus = "Pending"
                    };
                    while (context.Orders.Any(c => c.OrderId == order.OrderId) == true)
                    {
                        order.OrderId += 1;
                    }
                    // Thêm đơn hàng vào cơ sở dữ liệu
                    await context.Orders.AddAsync(order);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<Order> UpdateOrder(int orderId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var order = await context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);

                    if (order == null)
                    {
                        throw new Exception("Order not found");
                    }

                    var orderDetails = await context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();
                    decimal total = 0;

                    foreach (var orderDetail in orderDetails)
                    {
                        total += orderDetail.UnitPrice * orderDetail.Quantity - orderDetail.UnitPrice * orderDetail.Quantity * (decimal)orderDetail.Discount;

                        var flowerBouquet = context.FlowerBouquets.FirstOrDefault(fb => fb.FlowerBouquetId == orderDetail.FlowerBouquetId);
                        if (flowerBouquet != null)
                        {
                            flowerBouquet.UnitsInStock -= orderDetail.Quantity;
                        }
                    }

                    order.OrderStatus = "Done";
                    order.OrderDate = DateTime.Now;
                    order.ShippedDate = order.OrderDate.AddHours(1);
                    order.Total = total;

                    await context.SaveChangesAsync();
                    return order;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task CancelOrder(int orderId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var order = await context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);

                    if (order != null)
                    {
                        order.OrderStatus = "Cancel";

                        var orderDetails = await context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();
                        foreach (var orderDetail in orderDetails)
                        {
                            var flowerBouquet = await context.FlowerBouquets.FirstOrDefaultAsync(fb => fb.FlowerBouquetId == orderDetail.FlowerBouquetId);
                            if (flowerBouquet != null)
                            {
                                flowerBouquet.UnitsInStock += orderDetail.Quantity;
                            }
                        }

                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Order not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static async Task RemoveOrder(int orderId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var order = await context.Orders.SingleOrDefaultAsync(c => c.OrderId == orderId);
                    context.Orders.Remove(order);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static async Task<Order> GetOrderById(int orderId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    return await context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<string> GetLatestOrderStatus(int customerId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var latestOrder = await context.Orders.Include(c=> c.OrderDetails)
                        .Where(o => o.CustomerId == customerId)
                        .OrderByDescending(o => o.OrderDate)
                        .FirstOrDefaultAsync();

                    if (latestOrder != null)
                    {
                        switch (latestOrder.OrderStatus)
                        {
                            case "Pending   ":
                                return "PENDING";
                            case "Done   ":
                                return "DONE";
                            case "Cancel   ":
                                return "CANCEL";
                            default:
                                return ""; // Trạng thái không hợp lệ
                        }
                    }
                    else
                    {
                        return ""; // Không có đơn hàng nào của khách hàng trong cơ sở dữ liệu
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<Order> GetLatestOrder(int customerId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    return await context.Orders.Include(c=> c.OrderDetails)
                        .Where(o => o.CustomerId == customerId)
                        .OrderByDescending(o => o.OrderDate)
                        .FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
