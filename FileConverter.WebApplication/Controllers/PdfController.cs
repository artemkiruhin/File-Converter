using FileConverter.Services.Converters.Base;
using Microsoft.AspNetCore.Mvc;

namespace FileConverter.WebApplication.Controllers {
    public class PdfController : Controller {

        private readonly IConverter _converter;

        public PdfController(IConverter converter) {
            _converter = converter;
        }

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [HttpPost("convert")]
        public async Task<IActionResult> Convert(IFormFile markdownFile) {
            
            if (markdownFile == null || Path.GetExtension(markdownFile.FileName).ToLower() != ".md")
            {
                return BadRequest("Please upload a valid .md file");
            }
            
            try
            {
                string markdownText;
                using (var reader = new StreamReader(markdownFile.OpenReadStream()))
                {
                    markdownText = await reader.ReadToEndAsync();
                }

                byte[] pdfBytes = _converter.Convert(markdownText);
                return File(pdfBytes, "application/pdf", "converted.pdf");
            }
            catch
            {
                return BadRequest("Error during convertation");
            }
        }
    }
}
