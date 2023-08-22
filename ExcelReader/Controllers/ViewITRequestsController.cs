using ExcelReader.Data;
using ExcelReader.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExcelReader.Controllers
{
    public class ViewITRequestsController : Controller
    {
        /*private readonly DataContext _dataContext;*/
        private readonly IFileUploadService _fileUploadService;

        public ViewITRequestsController(IFileUploadService fileUploadService)
        {
            /*_dataContext = dataContext;*/
            _fileUploadService = fileUploadService;
        }

        public async Task<IActionResult> Index() // Add async to the action method
        {
            var itRequests = await _fileUploadService.GetExcelFileData(); // Await the task
            return View(itRequests);
        }
    }
}
