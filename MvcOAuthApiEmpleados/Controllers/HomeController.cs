using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcOAuthApiEmpleados.Models;
using Azure.Security.KeyVault.Secrets;
using System.Threading.Tasks;

namespace MvcOAuthApiEmpleados.Controllers
{
    public class HomeController : Controller
    {
        private SecretClient secretClient;

        public HomeController(SecretClient secretClient)
        {
            this.secretClient = secretClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string secretname)
        {
            KeyVaultSecret secret = await this.secretClient.GetSecretAsync(secretname);
            ViewBag.Secreto = secret.Value; 
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
