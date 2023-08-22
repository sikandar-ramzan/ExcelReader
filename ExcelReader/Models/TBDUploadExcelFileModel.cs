/*using ExcelReader.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace ExcelReader.Models
{
    public class UploadExcelFileModel
    {
        public IFormFile UploadExcelFile { get; set; }
        private readonly IFileUploadService _fileUploadService;


        public UploadExcelFileModel(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        public void OnGet()
        {
        }

      *//*  public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                await _fileUploadService.UploadExcelFile(file);
                return RedirectToPage("SuccessPage"); // Redirect to a success page
            }
            return Page();
        }*//*
    }
}

*/