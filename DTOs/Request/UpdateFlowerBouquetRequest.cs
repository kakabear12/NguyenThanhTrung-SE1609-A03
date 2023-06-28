using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class UpdateFlowerBouquetRequest
    {
        [Required]
        public int FlowerBouquetId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string FlowerBouquetName { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int UnitsInStock { get; set; }
        [Required]
        public byte FlowerBouquetStatus { get; set; }
        [Required]
        public int SupplierId { get; set; }
    }
}
