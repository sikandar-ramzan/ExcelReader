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
        

        public ITRequestController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
           
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        /* [HttpPost]
         public async Task<ActionResult> Index(IFormFile file)
         {
             var startUplaod = await _fileUploadService.UploadExcelFile(file);

             *//* var itRrequestFromDb = _fileUploadService.GetExcelFileData();*//*

             return View();
         }
         */

        [HttpPost]
        public async Task<ActionResult> Index(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                bool uploadSuccessful = await _fileUploadService.UploadExcelFile(file);

                if (uploadSuccessful)
                {
                   
                    return View("Success"); 
                }
            }

            
            return View("Failure");
        }
    }


}

