﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FF.Data.Entities
{
    public class OrderProduct
    {
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
