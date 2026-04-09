using Microsoft.AspNetCore.Mvc;
using MvcOAuthApiEmpleados.Filters;
using MvcOAuthApiEmpleados.Models;
using MvcOAuthApiEmpleados.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcOAuthApiEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private ServiceEmpleados service;

        public EmpleadosController(ServiceEmpleados service)
        {
            this.service = service;
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.service.GetEmpleadosAsync();
            return View(empleados);
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Details(int id)
        {
            HttpContext.User.FindFirst(x => x.Type == "TOKEN");
            Empleado empleado = await this.service.FindEmpleadoAsync(id);
            return View(empleado);
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Perfil()
        {
            Empleado empleado = await this.service.GetPerfilAsync();
            return View(empleado);
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Compis()
        {
            List<Empleado> empleados = await this.service.GetCompisAsync();
            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosOficios()
        {
            List<string> oficios = await this.service.GetOficiosAsync();
            ViewBag.Oficios = oficios;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmpleadosOficios(int? incremento, List<string> oficio, string accion)
        {
            List<string> oficios = await this.service.GetOficiosAsync();
            ViewBag.Oficios = oficios;
            if(accion.ToLower() == "update")
            {
                await this.service.IncrementarSalarioAsync(incremento.Value, oficio);
            }
            List<Empleado> empleados = await this.service.GetEmpleadosOficiosAsync(oficio);
            return View(empleados);
        }

    }
}
