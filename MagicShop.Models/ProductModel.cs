using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ProductCharacteristicsModel Characteristics { get; set; } = new ProductCharacteristicsModel();

        public int Stock { get; set; }

        public decimal Price { get; set; }

        public AccountModel Vendor { get; set; }

        public DateTime Created { get; set; }
    }

    public class ProductCharacteristicsModel
    {
        public double Weight { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }

        public double Long { get; set; }
    }
}
