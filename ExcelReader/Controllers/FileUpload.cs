using ExcelReader.Models;
using ExcelReader.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ExcelReader.Controllers
{
    public class FileUpload : Controller
    {
        private readonly ILogger<FileUpload> _logger;
        private readonly IFileUploadService _fileUploadService;

        public FileUpload(ILogger<FileUpload> logger, IFileUploadService fileUploadService)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {


            if (file == null || file.Length <= 0)
            {
                _logger.LogError("Error processing the uploaded file.");
                ViewBag.Message = "No file uploaded.";
                return View("Index");
            }

            try
            {
                await _fileUploadService.UploadExcelFile(file);
                ViewBag.Message = "File uploaded and data stored successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing the uploaded file.");
                ViewBag.Message = "An error occurred while processing the file.";
            }

            return View("Index");
        }
    }
}