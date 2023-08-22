/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;


namespace ExcelReader.Models
{
    public class UploadFile : PageModel
    {
        public IFormFile UploadExcelFile { get; set; }

    }
}


*//*  private readonly IWebHostEnvironment _environment;*/

/* public UploadFile(IWebHostEnvironment environment)
 {
     _environment = environment;
 }*/

/*  [BindProperty]

  public async Task OnPostAsync()
  {
     *//* var uploadPath = Path.Combine(_environment.ContentRootPath, "uploads");*/
/*            Directory.CreateDirectory(uploadPath); // Create the directory if it doesn't exist
*//*
            var file = Path.Combine( Upload.FileName);
            using var fileStream = new FileStream(file, FileMode.Create);
            Console.WriteLine(file);
            Console.WriteLine(fileStream);
            *//*await Upload.CopyToAsync(fileStream);*//*
        }*/