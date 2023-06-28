using BusinessObjectLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CategoryDAO
    {
        public static async Task<List<Category>> GetCategories()
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var listCategories = await context.Categories.ToListAsync();
                    return listCategories;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
