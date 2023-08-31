using ExcelReaderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExcelReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFilesController : ControllerBase
    {
        /* private readonly DatabaseHelper _databaseHelper;*/
        private readonly FileUploadService _fileUploadService;

        public UserFilesController(FileUploadService uploadService)
        {
            /* _databaseHelper = dbHelper;*/
            _fileUploadService = uploadService;

        }


        [HttpGet]
        public async Task<IActionResult> UploadUserFileData()
        {
            var resp = _fileUploadService.AddUserFiles();

            return Ok(resp);
        }
    }
}
