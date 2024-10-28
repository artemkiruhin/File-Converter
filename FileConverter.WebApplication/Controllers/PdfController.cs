using Microsoft.AspNetCore.Mvc;

namespace FileConverter.WebApplication.Controllers {
    public class PdfController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
