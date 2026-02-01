using Microsoft.AspNetCore.Mvc;

namespace QuotationManagement.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
