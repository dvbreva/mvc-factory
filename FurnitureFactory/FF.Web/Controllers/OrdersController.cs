using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FF.Data.Entities;
using FF.Services.Contracts;
using FF.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FF.Web.Controllers
{
    [Authorize(Roles = "Admin,Regular")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IClientService _clientService;

        public OrdersController(
            IOrderService orderService, 
            IProductService productService, 
            IClientService clientService)
        {
            _orderService = orderService;
            _productService = productService;
            _clientService = clientService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetOrders();

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var orderExists = await _orderService.Exists(id);

            if (!orderExists)
            {
                return NotFound();
            }

            var order = await _orderService.GetById(id);

            
            var model = new DetailsOrderViewModel
            {
                Id = order.Id,
                Date = order.OrderedDate,
                InvoiceNumber = order.InvoiceNumber,
                Client = order.Client,
                Products = await GetProductList()
            };
            
            return View(model);
        }

        // add 
        public async Task<IActionResult> Create()
        {
            var model = new CreateOrderViewModel
            {
                Clients = await GetClientList(),
                Products = await GetProductList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = new Order
                {
                    ClientId = model.ClientId,
                    OrderedDate = model.Date,
                    InvoiceNumber = model.InvoiceNumber
                };

                var result = await _orderService.Insert(order, model.SelectedProductIds);

                if (result.Succeeded)
                {
                    TempData["OrderCreated"] = true;
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            model.Products = await GetProductList();
            model.Clients = await GetClientList();

            return View(model);
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetById(id);

            var model = new UpdateOrderViewModel
            {
                Id = order.Id,
                Date = order.OrderedDate,
                InvoiceNumber = order.InvoiceNumber,
                ClientId = order.ClientId,
                Clients = await GetClientList(),
                Products = await GetProductList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = await _orderService.GetById(id);

                var result = await _orderService.Update(id, order, model.SelectedProductIds);

                if (result.Succeeded)
                {
                    TempData["OrderCreated"] = true;
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

            }

            model.Products = await GetProductList();
            model.Clients = await GetClientList();

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderExists = await _orderService.Exists(id);

            if (!orderExists)
            {
                return NotFound();
            }

            var order = await _orderService.GetById(id);

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _orderService.Delete(id);

                if (result.Succeeded)
                {
                    TempData["OrderDeleted"] = true;
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetProductList()
        {
            var products = await _productService.GetProducts();
            var result = products.Select(p => new SelectListItem(p.Name, p.Id.ToString()))
                .ToList();

            return result;
        }

        private async Task<IEnumerable<SelectListItem>> GetClientList()
        {
            var clients = await _clientService.GetClients();
            var result = clients.Select(p => new SelectListItem(p.Name, p.Id.ToString()))
                .ToList();

            return result;
        }
    }
}
