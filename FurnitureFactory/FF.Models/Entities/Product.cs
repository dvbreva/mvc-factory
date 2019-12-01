using System;
using System.Collections.Generic;
using System.Text;

namespace FF.Data.Entities
{
    public class Product : Auditable
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
