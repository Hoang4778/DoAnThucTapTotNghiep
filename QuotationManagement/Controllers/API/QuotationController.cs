using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotationManagement.Contexts;
using QuotationManagement.Models;

namespace QuotationManagement.Controllers.API
{
    public class QuotationController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public QuotationController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("api/quotation/search")]
        public async Task<JsonResult> searchQuotationsByQueries(string? quotationCode, string? quotationName, string? customerName, string? accountCode, DateTime? creationDateFrom, DateTime? creationDateTo, DateOnly? expirationDateFrom, DateTime? expirationDateTo, bool? statusInProgress, bool? statusValidated, bool? statusExpired, bool? statusCancelled)
        {
            try
            {
                var searchedQuotations = await _appDBContext.Quotation.Where(q => q.code == quotationCode).ToListAsync();

                return new JsonResult(new ApiResponse { isOkay = true, message = "Quotations fetched successfully", data = searchedQuotations });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse { isOkay = false, message = ex.Message, data = null });
            }
        }
    }
}
