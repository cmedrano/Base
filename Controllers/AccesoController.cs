using AvicolaApp.Models;
using AvicolaApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AvicolaApp.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IAutenticacionService _autenticacionService;
        private readonly ILogger<AccesoController> _logger;

        public AccesoController(
            IAutenticacionService autenticacionService,
            ILogger<AccesoController> logger)
        {
            _autenticacionService = autenticacionService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string usuario, string password)
        {
            // Validar entrada
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Usuario y contraseña son requeridos");
                return View();
            }

            try
            {
                var usuarioBD = await _autenticacionService.ObtenerUsuarioPorNombreOEmailAsync(usuario);

                if (usuarioBD == null || !_autenticacionService.VerificarPassword(password, usuarioBD.Password))
                {
                    _logger.LogWarning($"Intento de login fallido para: {usuario}");
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos");
                    return View();
                }

                // Login exitoso
                await SignInUsuario(usuarioBD);
                _logger.LogInformation($"Usuario {usuarioBD.UserName} ha iniciado sesión");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en login");
                ModelState.AddModelError(string.Empty, "Error al procesar la solicitud");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Salir()
        {
            var username = User.Identity?.Name;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation($"Usuario {username} ha cerrado sesión");
            return RedirectToAction("Login");
        }

        public IActionResult Denegado()
        {
            return View();
        }

        private async Task SignInUsuario(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Email, usuario.UserEmail),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}