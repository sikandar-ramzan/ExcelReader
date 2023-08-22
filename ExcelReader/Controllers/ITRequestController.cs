using ExcelDataReader;
using ExcelReader.Data;
using ExcelReader.Models;
using ExcelReader.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExcelReader.Controllers
{
    [Route("/it-request")]
    public class ITRequestController : Controller
    {

        private readonly IFileUploadService _fileUploadService;
        /* private readonly DataContext _dataContext;*/

        public ITRequestController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
            /*_dataContext = dataContext;*/
        }


        [HttpGet]
        public IActionResult Index()
        {
            /* itRequest = itRequest == null ? new List<ExcelFile>() : itRequest;*/

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Index(IFormFile file)
        {
            await _fileUploadService.UploadExcelFile(file);

            /* var itRrequestFromDb = _fileUploadService.GetExcelFileData();*/

            return View();
        }


    }
}
