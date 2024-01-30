using JokesWebApp_ENERO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JokesWebApp_ENERO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
        public IActionResult UploadVoucher()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadVoucher(IFormFile file)
        {
            // Validar el tamaño y tipo del archivo
            if (file == null || file.Length == 0 || file.Length > 5000000)
            {
                ViewBag.Error = "Archivo inválido. Por favor, asegúrate de que el tamaño sea menor a 5 MB.";
                return View();
            }

            string[] permittedExtensions = { ".pdf", ".jpg", ".png" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                ViewBag.Error = "Tipo de archivo no permitido. Solo se aceptan PDF, JPG y PNG.";
                return View();
            }

            // Guardar el archivo
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Procesar y validar el voucher
            bool isValidVoucher = ProcessAndValidateVoucher(filePath);

            if (isValidVoucher)
            {
                // Lógica para actualizar la base de datos
                // Ejemplo: Marcar una factura como pagada
            }
            else
            {
                ViewBag.Error = "El voucher subido no es válido.";
                return View();
            }

            return RedirectToAction("Index"); // O redirigir a una página de confirmación
        }

        private bool ProcessAndValidateVoucher(string filePath)
        {
            // Lógica para procesar y validar el voucher
            // Ejemplo: Verificar que el archivo contenga cierto texto
            string content = File.ReadAllText(filePath);
            return content.Contains("Código de Validación Esperado");
        }



    }
}
