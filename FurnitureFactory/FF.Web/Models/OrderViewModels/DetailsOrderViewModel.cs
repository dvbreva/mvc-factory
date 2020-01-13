using FF.Data.Entities;

namespace FF.Web.Models.OrderViewModels
{
    public class DetailsOrderViewModel : CreateOrderViewModel
    {
        public int Id { get; set; }

        public Client Client { get; set; }

    }
}
