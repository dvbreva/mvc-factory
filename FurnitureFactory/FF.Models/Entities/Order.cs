using System;
using System.Collections.Generic;
using System.Text;

namespace FF.Data.Entities
{
    public class Order : Auditable
    { 
        public int Id { get; set; }

        public DateTime OrderedDate { get; set; }

        public string InvoiceNumber { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }

        public List<OrderProduct> OrderedProducts { get; set; }
    }
}
