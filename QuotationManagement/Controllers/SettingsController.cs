using Microsoft.AspNetCore.Mvc;

namespace QuotationManagement.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Default");
        }

        public IActionResult Default()
        {
            return View("/Views/Settings/Index.cshtml");
        }

        public IActionResult HeaderFooter()
        {
            return View("/Views/Settings/HeaderFooter/Index.cshtml");
        }

        public IActionResult HeaderFooterCreate()
        {
            return View("/Views/Settings/HeaderFooter/Create.cshtml");
        }

        public IActionResult HeaderFooterEdit(string settingCode)
        {
            return View("/Views/Settings/HeaderFooter/Edit.cshtml");
        }
    }
}
