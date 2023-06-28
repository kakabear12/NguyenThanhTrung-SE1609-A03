using BusinessObjectLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IFlowerBouquetRepo
    {
        Task<IEnumerable<FlowerBouquet>> GetFlowerBouquets();
        Task<IEnumerable<Supplier>> GetSuppliers();
        Task CreateFlowerBouquet(FlowerBouquet flowerBouquet);
        Task<FlowerBouquet> UpdateFlowerBouquet(FlowerBouquet flower);
        Task RemoveFlowerBouquet(int flowId);
        Task<FlowerBouquet> GetFlowerBouquetById(int flowerBouquetId);
        Task<IEnumerable<FlowerBouquet>> SearchFlowerBouquetByName(string searchTerm);
    }
}
