using AvicolaApp.Models;
using AvicolaApp.Services;
using AvicolaApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AvicolaApp.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;
        private readonly IClienteService _clienteService;
        private readonly IAutenticacionService _autenticacionService;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(
            IUsuarioService usuarioService,
            IRolService rolService,
            IClienteService clienteService,
            IAutenticacionService autenticacionService,
            ILogger<UsuariosController> logger)
        {
            _usuarioService = usuarioService;
            _rolService = rolService;
            _clienteService = clienteService;
            _autenticacionService = autenticacionService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var listaUsuarios = await _usuarioService.ObtenerTodosAsync();
            ViewBag.TotalUsuarios = await _usuarioService.ObtenerTotalAsync();
            ViewBag.TotalClientes = await _clienteService.ObtenerTotalAsync();
            ViewBag.Roles = await _rolService.ObtenerTodosAsync();

            return View(listaUsuarios);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarUsuario(string UserName, string UserEmail, string Password, int RolId)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(UserEmail) ||
                string.IsNullOrWhiteSpace(Password) || RolId <= 0)
            {
                TempData["Error"] = "Todos los campos son obligatorios";
                return RedirectToAction("Index");
            }

            try
            {
                // Verificar que no exista otro usuario con el mismo nombre o email
                var usuarioExistente = await _autenticacionService.ObtenerUsuarioPorNombreOEmailAsync(UserName);
                if (usuarioExistente != null)
                {
                    TempData["Error"] = "El usuario o email ya existe";
                    return RedirectToAction("Index");
                }

                var nuevoUsuario = new Usuario
                {
                    UserName = UserName,
                    UserEmail = UserEmail,
                    Password = _autenticacionService.HashearPassword(Password),
                    RolId = RolId
                };

                await _usuarioService.GuardarAsync(nuevoUsuario);
                TempData["Exito"] = "Usuario creado exitosamente";
                _logger.LogInformation($"Usuario {UserName} creado por {User.Identity?.Name}");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                TempData["Error"] = "Error al crear el usuario";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GenerarHash(string password)
        {
            if (string.IsNullOrEmpty(password))
                return BadRequest("Password requerido");

            var hash = _autenticacionService.HashearPassword(password);
            return Ok(new { password = password, hash = hash });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarUsuario(int Id, string UserName, string UserEmail, int RolId, string Password)
        {
            var usuarioExistente = await _usuarioService.ObtenerPorIdAsync(Id);

            if (usuarioExistente == null)
            {
                TempData["Error"] = "Usuario no encontrado";
                return RedirectToAction("Index");
            }

            try
            {
                usuarioExistente.UserName = UserName;
                usuarioExistente.UserEmail = UserEmail;
                usuarioExistente.RolId = RolId;

                // Si se proporciona contraseña, hashearla
                if (!string.IsNullOrWhiteSpace(Password))
                {
                    usuarioExistente.Password = _autenticacionService.HashearPassword(Password);
                }

                await _usuarioService.ActualizarAsync(usuarioExistente);
                TempData["Exito"] = "Usuario actualizado exitosamente";
                _logger.LogInformation($"Usuario {UserName} actualizado por {User.Identity?.Name}");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar usuario");
                TempData["Error"] = "Error al actualizar el usuario";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            try
            {
                var usuario = await _usuarioService.ObtenerPorIdAsync(id);

                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction("Index");
                }

                await _usuarioService.EliminarLogicamenteAsync(id);
                TempData["Exito"] = "Usuario eliminado exitosamente";
                _logger.LogInformation($"Usuario {usuario.UserName} eliminado lógicamente por {User.Identity?.Name}");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario");
                TempData["Error"] = "Error al eliminar el usuario";
                return RedirectToAction("Index");
            }
        }
    }
}