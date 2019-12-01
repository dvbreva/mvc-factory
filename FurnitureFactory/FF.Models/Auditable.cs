using System;

namespace FF.Data.Entities
{
    public abstract class Auditable
    {
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}

