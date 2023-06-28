using BusinessObjectLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SupplierDAO
    {
        public static async Task<List<Supplier>> GetSuppliers()
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var listSuppliers = await context.Suppliers.ToListAsync();
                    return listSuppliers;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        public static async Task AddSupplier(Supplier supplier)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    await context.Suppliers.AddAsync(supplier);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task RemoveSupplier(Supplier supplier)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    context.Suppliers.Remove(supplier);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<Supplier> GetSupplierById(int supplierId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    return await context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == supplierId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
