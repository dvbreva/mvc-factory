using System;
using System.Collections.Generic;
using System.Text;

namespace FF.Data.Entities
{
    public class Client : Auditable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Bulstat { get; set; }

        public bool Vat { get; set; }

        public string ResponsiblePerson { get; set; }
    }
}
