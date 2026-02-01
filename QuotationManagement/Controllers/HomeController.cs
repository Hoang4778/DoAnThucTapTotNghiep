using Microsoft.AspNetCore.Mvc;

namespace QuotationManagement.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return RedirectToAction("Search", "Quotation");
        }
    }
}
