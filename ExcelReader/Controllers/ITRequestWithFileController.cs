using ExcelReader.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExcelReader.Controllers
{
    public class ITRequestWithFileController : Controller
    {
        private readonly IFileUploadService _fileUploadService;

        public ITRequestWithFileController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }
        public async Task<IActionResult> Index()
        {
            var itRequestsWithFiles = await _fileUploadService.GetITRequestsWithFiles();

            // You can now use itRequestsWithFiles in your view or further process it as needed.

            return View(itRequestsWithFiles);
        }
    }
}
