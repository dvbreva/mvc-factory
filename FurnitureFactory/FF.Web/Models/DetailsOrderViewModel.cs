using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FF.Data.Entities;

namespace FF.Web.Models
{
    public class DetailsOrderViewModel : CreateOrderViewModel
    {
        public int Id { get; set; }

        public Client Client { get; set; }

    }
}
