using ExcelReaderAPI.Services;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ExcelReaderAPI.Controllers
{
    [Route("upload-it-request")]
    [ApiController]
    public class ITRequestUpload : ControllerBase
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly FileUploadService _fileUploadService;

        public ITRequestUpload(DatabaseHelper databaseHelper, FileUploadService fileUploadService)
        {
            _databaseHelper = databaseHelper;
            _fileUploadService = fileUploadService;
        }

        public SqlDbType ParamValue { get; private set; }

        /* [HttpPost]*/
        /* public async Task<IActionResult> UploadExcelFile(IFormFile file)
         {
             // Define the name and parameters for your stored procedure
             string storedProcedureName = "Upload_IT_Request";
             var parameters = new[]
             {
             new SqlParameter("@ParamName", ParamValue)
             // Add parameters as needed
             };

             // Execute the stored procedure
             var result = _databaseHelper.ExecuteStoredProcedure(storedProcedureName, parameters);

             // You can return the result as JSON or in any format you prefer
             *//*            return Ok(result);
             *//*
             if (file != null && file.Length > 0)
             {
                 bool uploadSuccessful = await _fileUploadService.UploadExcelFile(file);

                 if (uploadSuccessful)
                 {

                     return Ok("success");
                 }
             }


             return BadRequest("failure");
         }*/
        /*[HttpPost]
        public async Task<IActionResult> UploadITRequest(IFormFile file)
        {

            return Ok();
        }*/
        /* private readonly IFileUploadService _fileUploadService;*/

        /* public ITRequestWithFileController(IFileUploadService fileUploadService)
         {
             _fileUploadService = fileUploadService;
         }*/
        /*  public async Task<IActionResult> Index()
          {
              var itRequestsWithFiles = await _fileUploadService.GetITRequestsWithFiles();

              return View(itRequestsWithFiles);
          }*/
    }
}


