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
    public class FlowerBouquetRepo : IFlowerBouquetRepo
    {
        public async Task CreateFlowerBouquet(FlowerBouquet flowerBouquet)
        {
            await FlowerBouquetDAO.CreateFlowerBouquet(flowerBouquet);
        }

        public async Task<FlowerBouquet> GetFlowerBouquetById(int flowerBouquetId)
        {
            return await FlowerBouquetDAO.GetFlowerBouquetById(flowerBouquetId);
        }

        public async Task<IEnumerable<FlowerBouquet>> GetFlowerBouquets()
        {
            return await FlowerBouquetDAO.GetFlowerBouquets();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliers()
        {
            return await SupplierDAO.GetSuppliers();
        }

        public async Task RemoveFlowerBouquet(int id)
        {
            await FlowerBouquetDAO.RemoveFlowerBouquet(id);
        }

        public async Task<IEnumerable<FlowerBouquet>> SearchFlowerBouquetByName(string searchTerm)
        {
            return await FlowerBouquetDAO.SearchFlowerBouquetByName(searchTerm);
        }

        public async Task<FlowerBouquet> UpdateFlowerBouquet(FlowerBouquet flower)
        {
            return await FlowerBouquetDAO.UpdateFlowerBouquet(flower);
        }
    }
}
