using Microsoft.AspNetCore.Mvc;

namespace ExcelReader.Controllers
{
    [Route("/it-request")]
    public class ITRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }


}

