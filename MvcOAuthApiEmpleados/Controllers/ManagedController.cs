using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcOAuthApiEmpleados.Models;
using MvcOAuthApiEmpleados.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcOAuthApiEmpleados.Controllers
{
    public class ManagedController : Controller
    {
        private ServiceEmpleados service;

        public ManagedController(ServiceEmpleados service)
        {
            this.service = service;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string token = await this.service.LogInAsync(model.UserName, model.Password);
            if(token == null)
            {
                ViewBag.Mensaje = "Credenciales incorrectas";
            }
            else
            {
                ViewBag.Mensaje = "Ya tienes tu token";
                HttpContext.Session.SetString("TOKEN", token);
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                // ALMACENAMOS EL NOMBRE DEL USUARIO
                identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName));
                // ALMACENAMOS EL PASSWORD DEL USUARIO COMO IDENTIFIER
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, model.Password));
                identity.AddClaim(new Claim("TOKEN", token));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                // DAMOS DE ALTA AL USUARIO DURANTE 20 MINUTOS
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                });
                return RedirectToAction("Index", "Empleados");
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Empleados");
        }
    }
}
