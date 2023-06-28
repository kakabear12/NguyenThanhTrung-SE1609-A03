using BusinessObjectLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class FlowerBouquetCart
    {
        public static async Task AddFlowerBouquetToCart(int customerId, int flowId, int quantity)
        {
            try
            {
                var latestOrderStatus = await OrderDAO.GetLatestOrderStatus(customerId);

                if (latestOrderStatus == "PENDING")
                {
                    var latestOrder = await OrderDAO.GetLatestOrder(customerId);
                    var existingOrderDetail = await OrderDetailDAO.GetOrderDetailByOrderId(latestOrder.OrderId, flowId);

                    if (existingOrderDetail != null)
                    {
                        var updatedOrderDetail = new OrderDetail
                        {
                            FlowerBouquetId = flowId,
                            UnitPrice = existingOrderDetail.UnitPrice,
                            Quantity = existingOrderDetail.Quantity + quantity,
                            Discount = existingOrderDetail.Discount
                        };
                        if (await IsQuantityValid(updatedOrderDetail))
                        {
                            await OrderDetailDAO.UpdateOrderDetail(updatedOrderDetail);
                        }
                        else
                        {
                            throw new Exception("Số lượng sản phẩm vượt quá số lượng tồn kho.");
                        }
                    }
                    else
                    {
                        var fl = await FlowerBouquetDAO.GetFlowerBouquetById(flowId);

                        var newOrderDetail = new OrderDetail
                        {
                            OrderId = latestOrder.OrderId,
                            FlowerBouquetId = flowId,
                            UnitPrice = fl.UnitPrice,
                            Quantity = quantity,
                            Discount = CalculateDiscount(quantity)
                        };

                        if (await IsQuantityValid(newOrderDetail))
                        {
                            await OrderDetailDAO.CreateOrderDetail(newOrderDetail);
                        }
                        else
                        {
                            throw new Exception("Số lượng sản phẩm vượt quá số lượng tồn kho.");
                        }
                    }
                }
                else
                {
                    await OrderDAO.CreateOrder(customerId);
                    var latestOrder = await OrderDAO.GetLatestOrder(customerId);
                    var fl = await FlowerBouquetDAO.GetFlowerBouquetById(flowId);
                    var newOrderDetail = new OrderDetail
                    {
                        OrderId = latestOrder.OrderId,
                        FlowerBouquetId = flowId,
                        UnitPrice = fl.UnitPrice,
                        Quantity = quantity,
                        Discount = CalculateDiscount(quantity)
                    };

                    if (await IsQuantityValid(newOrderDetail))
                    {
                        await OrderDetailDAO.CreateOrderDetail(newOrderDetail);
                    }
                    else
                    {
                        throw new Exception("Số lượng sản phẩm vượt quá số lượng tồn kho.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        private static async Task<bool> IsQuantityValid(OrderDetail orderDetail)
        {
            var availableQuantity = await FlowerBouquetDAO.GetFlowerBouquetById(orderDetail.FlowerBouquetId);
            return orderDetail.Quantity <= availableQuantity.UnitsInStock;
        }

        public static async Task RemoveFlowerBouquetFromCart(int customerId, int flowerBouquetId)
        {
            try
            {
                var latestOrderStatus = await OrderDAO.GetLatestOrderStatus(customerId);

                if (latestOrderStatus == "PENDING")
                {
                    var latestOrder = await OrderDAO.GetLatestOrder(customerId);
                    var existingOrderDetail = await OrderDetailDAO.GetOrderDetailByOrderId(latestOrder.OrderId, flowerBouquetId);

                    if (existingOrderDetail != null)
                    {
                        await OrderDetailDAO.RemoveOrderDetail(existingOrderDetail.OrderId, existingOrderDetail.FlowerBouquetId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public static async Task<Order> GetCartItems(int customerId)
        {
            try
            {
                var latestOrderStatus = await OrderDAO.GetLatestOrderStatus(customerId);

                if (latestOrderStatus == "PENDING")
                {
                    var latestOrder = await OrderDAO.GetLatestOrder(customerId);
                    /*var cartItems = await OrderDetailDAO.GetOrderDetailsByOrderId(latestOrder.OrderId);
                    var flowerBouquets = new List<FlowerBouquet>();

                    foreach (var cartItem in cartItems)
                    {
                        var flowerBouquet = await FlowerBouquetDAO.GetFlowerBouquetById(cartItem.FlowerBouquetId);
                        flowerBouquets.Add(flowerBouquet);
                    }*/

                    return latestOrder;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }

           // return new List<FlowerBouquet>();
        }

        public static async Task<Order> CheckoutCart(int customerId)
        {
            try
            {
                var latestOrderStatus = await OrderDAO.GetLatestOrderStatus(customerId);

                if (latestOrderStatus == "PENDING")
                {
                    var latestOrder = await OrderDAO.GetLatestOrder(customerId);
                    await OrderDAO.UpdateOrder(latestOrder.OrderId);
                }
                var order = await OrderDAO.GetLatestOrder(customerId);
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        private static double CalculateDiscount(int quantity)
        {
            if (quantity >= 10 && quantity < 20)
            {
                return 0.1;
            }
            else if (quantity >= 20)
            {
                return 0.2;
            }
            else
            {
                return 0;
            }
        }
        public static async Task UpdateFlowerBouquetQuantity(int customerId, int flowerId, int quantity)
        {
            try
            {
                var latestOrderStatus = await OrderDAO.GetLatestOrderStatus(customerId);

                if (latestOrderStatus == "PENDING")
                {
                    var latestOrder = await OrderDAO.GetLatestOrder(customerId);
                    var existingOrderDetail = await OrderDetailDAO.GetOrderDetailByOrderId(latestOrder.OrderId, flowerId);

                    if (existingOrderDetail != null)
                    {
                        var updatedOrderDetail = new OrderDetail
                        {
                            FlowerBouquetId = flowerId,
                            UnitPrice = existingOrderDetail.UnitPrice,
                            Quantity = quantity,
                            Discount = existingOrderDetail.Discount
                        };

                        if (await IsQuantityValid(updatedOrderDetail))
                        {
                            await OrderDetailDAO.UpdateOrderDetail(updatedOrderDetail);
                        }
                        else
                        {
                            throw new Exception("Số lượng sản phẩm vượt quá số lượng tồn kho.");
                        }
                    }
                    else
                    {
                        throw new Exception("Bó hoa không tồn tại trong giỏ hàng.");
                    }
                }
                else
                {
                    throw new Exception("Không có đơn hàng đang chờ xử lý.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}
