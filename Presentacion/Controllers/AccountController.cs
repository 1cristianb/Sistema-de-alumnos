using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Presentacion.Helpers;
using Presentacion.Models;
using System.Security.Claims;

namespace Presentacion.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsuarioService _usuarioService = new UsuarioService();
        private readonly IConfiguration _configuracion;

        public AccountController(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Alumno");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var usuario = _usuarioService.Autenticar(model.NombreUsuario, model.Password, _configuracion.ObtenerCadena("LiteDB"));

                if (usuario == null)
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id ?? string.Empty),
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario)
                };
                var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identidad),
                    new AuthenticationProperties { IsPersistent = model.Recordarme });

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Alumno");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public IActionResult Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                _usuarioService.Registrar(model.NombreUsuario, model.Password, _configuracion.ObtenerCadena("LiteDB"));
                TempData["Mensaje"] = "Usuario creado. Ya podés iniciar sesión.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
