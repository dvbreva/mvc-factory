using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FF.Web.Models.CategoryViewModels
{
    public class CreateOrderViewModel
    {
        public int ClientId { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public IEnumerable<int> SelectedProductIds { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }

        public IEnumerable<SelectListItem> Clients { get; set; }
    }
}
