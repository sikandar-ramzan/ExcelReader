using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ExcelReaderAPI.Models;
using ExcelReaderAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ExcelReaderAPI.Controllers
{
    [Route("it-request")]
    [ApiController]
    public class ITRequestUpload : ControllerBase
    {
        private readonly FileUploadService _fileUploadService;
        private readonly IConfiguration _configuration;

        public ITRequestUpload(FileUploadService fileUploadService, IConfiguration configuration)
        {
            _fileUploadService = fileUploadService;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public ActionResult<List<ITRequestWithFile>> GetAllITRequests()
        {

            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();


            return _fileUploadService.GetAllItRequests();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public IActionResult UploadExcelFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadSuccessful = _fileUploadService.UploadExcelFile(file);

                if (uploadSuccessful != null)
                {
                    return Ok(uploadSuccessful);
                }
            }

            return BadRequest("failure");
        }
    }
}