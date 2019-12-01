using System.Threading.Tasks;
using FF.Data.Entities;
using FF.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FF.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetClients();

            return View(clients);
        }

        // details 
        public async Task<IActionResult> Details(int id)
        {
            var clientExists = await _clientService.Exists(id);

            if (!clientExists)
            {
                return NotFound();
            }

            var client = await _clientService.GetById(id);

            return View(client);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                var result = await _clientService.Insert(client);

                if (result.Succeeded)
                {
                    TempData["ClientCreated"] = true;
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return RedirectToAction("Index");
        }

        // edit
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clientService.GetById(id);

            return View(new Client { Name = client.Name, Address = client.Address, Bulstat = client.Bulstat, Vat = client.Vat, ResponsiblePerson = client.ResponsiblePerson });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (ModelState.IsValid)
            {
                var clientExists = await _clientService.Exists(id);

                if (!clientExists)
                {
                    return NotFound();
                }

                var result = await _clientService.Update(id, client);

                if (result.Succeeded)
                {
                    TempData["ClientEdited"] = true;
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            return RedirectToAction("Details", new { id });
        }

        // delete 
        public async Task<IActionResult> Delete(int id)
        {
            var clientExists = await _clientService.Exists(id);

            if (!clientExists)
            {
                return NotFound();
            }

            var client = await _clientService.GetById(id);

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _clientService.Delete(id);

            if (result.Succeeded)
            {
                TempData["ClientDeleted"] = true;
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return RedirectToAction("Index");
        }
    }
}
