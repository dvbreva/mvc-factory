using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FF.Data;
using FF.Data.Entities;

namespace FF.Web.Models
{
    public class ProductCreateViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double WeightInKilos { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public List<OrderProduct> OrderedProducts { get; set; }
    }
}
