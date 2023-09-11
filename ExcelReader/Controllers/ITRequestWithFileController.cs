using ExcelReader.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExcelReader.Controllers
{
    public class ITRequestWithFileController : Controller
    {

        public ITRequestWithFileController()
        { }
        public IActionResult Index()
        {
            return View();
        }
    }
}
