using BusinessObjectLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class FlowerBouquetDAO
    {
        public static async Task<List<FlowerBouquet>> GetFlowerBouquets()
        {
            try
            {
               using (var context =  new FUFlowerBouquetManagementContext())
                {
                    return await context.FlowerBouquets.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task CreateFlowerBouquet(FlowerBouquet flower)
        {
            try
            {
               using(var context = new FUFlowerBouquetManagementContext())
                {
                    while (context.FlowerBouquets.Any(c => c.FlowerBouquetId == flower.FlowerBouquetId) == true)
                    {
                        flower.FlowerBouquetId += 1;
                    }
                    await context.FlowerBouquets.AddAsync(flower);
                   await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<FlowerBouquet> UpdateFlowerBouquet(FlowerBouquet flower)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {

                    var flowerBouquet = await context.FlowerBouquets.FirstOrDefaultAsync(fb => fb.FlowerBouquetId == flower.FlowerBouquetId);

                    if (flowerBouquet != null)
                    {
                        flowerBouquet.CategoryId = flower.CategoryId;
                        flowerBouquet.FlowerBouquetName = flower.FlowerBouquetName;
                        flowerBouquet.Description = flower.Description;
                        flowerBouquet.UnitPrice = flower.UnitPrice;
                        flowerBouquet.UnitsInStock = flower.UnitsInStock;
                        flowerBouquet.FlowerBouquetStatus = flower.FlowerBouquetStatus;
                        flowerBouquet.SupplierId = flower.SupplierId;
                        await context.SaveChangesAsync();
                        return flowerBouquet;
                    }
                    else
                    {
                        throw new Exception("Flower bouquet not found.");
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task RemoveFlowerBouquet(int flowerId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var flow = await context.FlowerBouquets.SingleOrDefaultAsync(c => c.FlowerBouquetId == flowerId);
                    if(flow == null)
                    {
                        throw new Exception("FlowerBouquet not found");
                    }
                    context.FlowerBouquets.Remove(flow);
                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<FlowerBouquet> GetFlowerBouquetById(int flowerBouquetId)
        {
            try
            {
                using(var context = new FUFlowerBouquetManagementContext())
                {
                    return await context.FlowerBouquets.FirstOrDefaultAsync(fb => fb.FlowerBouquetId == flowerBouquetId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<FlowerBouquet>> SearchFlowerBouquetByName(string searchTerm)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    int searchTermNumber;
                    var isNumeric = int.TryParse(searchTerm, out searchTermNumber);

                    var query = await context.FlowerBouquets
                        .Where(fb => fb.FlowerBouquetName.Contains(searchTerm) || (isNumeric && fb.UnitPrice == searchTermNumber))
                        .ToListAsync();

                    return query;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
