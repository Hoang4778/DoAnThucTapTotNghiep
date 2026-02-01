using Microsoft.AspNetCore.Mvc;

namespace QuotationManagement.Controllers
{
    public class QuotationController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Search");
        }

        public IActionResult Search()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Setup(string quotation_code)
        {
            if (quotation_code == null)
            {
                return RedirectToAction("Search");
            }

            ViewData["quotation_code"] = quotation_code;

            return View();
        }
    }
}
