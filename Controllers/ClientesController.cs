using AvicolaApp.Models;
using AvicolaApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AvicolaApp.Controllers
{
    [Authorize(Roles = "Administrador, Operario")]
    public class ClientesController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index()
        {
            var listaClientes = await _clienteService.ObtenerTodosAsync();
            return View(listaClientes);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarCliente(
            string NombreRazonSocial,
            string Cuit,
            string? Domicilio,
            string? Email,
            string? Telefono,
            string? Celular,
            string? Fax,
            string? Localidad,
            string TipoCliente)
        {
            // Validar que el CUIT no exista
            var clienteExistente = await _clienteService.ObtenerPorCuitAsync(Cuit);
            if (clienteExistente != null)
            {
                TempData["Error"] = "El CUIT ya existe en el sistema";
                return RedirectToAction("Index");
            }

            var nuevoCliente = new Cliente
            {
                NombreRazonSocial = NombreRazonSocial,
                Cuit = Cuit,
                Domicilio = Domicilio,
                Email = Email,
                Telefono = Telefono,
                Celular = Celular,
                Fax = Fax,
                Localidad = Localidad,
                TipoCliente = TipoCliente
            };

            await _clienteService.GuardarAsync(nuevoCliente);
            TempData["Exito"] = "Cliente registrado exitosamente";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditarCliente(
            int Id,
            string NombreRazonSocial,
            string Cuit,
            string? Domicilio,
            string? Email,
            string? Telefono,
            string? Celular,
            string? Fax,
            string? Localidad,
            string TipoCliente)
        {
            var clienteExistente = await _clienteService.ObtenerPorIdAsync(Id);

            if (clienteExistente == null)
            {
                TempData["Error"] = "Cliente no encontrado";
                return RedirectToAction("Index");
            }

            // Validar que el CUIT no esté en uso por otro cliente
            if (clienteExistente.Cuit != Cuit)
            {
                var otroCuitEnUso = await _clienteService.ObtenerPorCuitAsync(Cuit);
                if (otroCuitEnUso != null)
                {
                    TempData["Error"] = "El CUIT ya está en uso por otro cliente";
                    return RedirectToAction("Index");
                }
            }

            clienteExistente.NombreRazonSocial = NombreRazonSocial;
            clienteExistente.Cuit = Cuit;
            clienteExistente.Domicilio = Domicilio;
            clienteExistente.Email = Email;
            clienteExistente.Telefono = Telefono;
            clienteExistente.Celular = Celular;
            clienteExistente.Fax = Fax;
            clienteExistente.Localidad = Localidad;
            clienteExistente.TipoCliente = TipoCliente;

            await _clienteService.ActualizarAsync(clienteExistente);
            TempData["Exito"] = "Cliente actualizado exitosamente";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var cliente = await _clienteService.ObtenerPorIdAsync(id);

            if (cliente == null)
            {
                TempData["Error"] = "Cliente no encontrado";
                return RedirectToAction("Index");
            }

            await _clienteService.EliminarAsync(id);
            TempData["Exito"] = "Cliente eliminado exitosamente";

            return RedirectToAction("Index");
        }
    }
}