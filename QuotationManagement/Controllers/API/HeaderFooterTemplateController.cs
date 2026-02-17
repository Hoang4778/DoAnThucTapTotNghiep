using Microsoft.AspNetCore.Mvc;

namespace QuotationManagement.Controllers.API
{
    public class HeaderFooterTemplateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
