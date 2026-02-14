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
            return View("/Views/Quotation/Search.cshtml");
        }

        public IActionResult Create()
        {
            return View("/Views/Quotation/Create.cshtml");
        }

        public IActionResult Setup(string quotationCode)
        {
            if (quotationCode == null)
            {
                return RedirectToAction("Search");
            }

            ViewData["quotationCode"] = quotationCode;

            return View();
        }

        public IActionResult Table(string quotationCode)
        {
            if (quotationCode == null)
            {
                return RedirectToAction("Search");
            }

            ViewData["quotationCode"] = quotationCode;

            return View();
        }

        public IActionResult Lifecycle(string quotationCode)
        {
            if (quotationCode == null)
            {
                return RedirectToAction("Search");
            }

            ViewData["quotationCode"] = quotationCode;

            return View();
        }

        public IActionResult Document(string quotationCode)
        {
            if (quotationCode == null)
            {
                return RedirectToAction("Search");
            }

            ViewData["quotationCode"] = quotationCode;

            return View();
        }
    }
}
