using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class AddToCartRequest
    {
        [Required]
        public int FlowerBouquetId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
