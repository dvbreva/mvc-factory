using System;
using System.Collections.Generic;
using System.Text;
using FF.Data.Entities;

namespace FF.Data
{
    public class Category : Auditable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
