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

        [Route("api/quotation/search")]
        public async Task<JsonResult> searchQuotationsByQueries([FromQuery] QuotationSearchCriteria searchCriteria)
        {
            try
            {
                bool hasAnyFilter = false;

                var query = _appDBContext.Quotation.AsQueryable();

                if (searchCriteria.quotationCode != null)
                {
                    query = query.Where(q => q.code == searchCriteria.quotationCode);
                    hasAnyFilter = true;
                }

                if (searchCriteria.quotationName != null)
                {
                    query = query.Where(q => q.name == searchCriteria.quotationName);
                    hasAnyFilter = true;
                }

                if (searchCriteria.customerName != null)
                {
                    query = query.Where(q => q.customerName == searchCriteria.customerName);
                    hasAnyFilter = true;
                }

                if (searchCriteria.accountCode != null)
                {
                    query = query.Where(q => q.accountCode == searchCriteria.accountCode);
                    hasAnyFilter = true;
                }

                if (searchCriteria.creationDateFrom != null)
                {
                    query = query.Where(q => q.creationDate >= searchCriteria.creationDateFrom);
                    hasAnyFilter = true;
                }

                if (searchCriteria.creationDateTo != null)
                {
                    query = query.Where(q => q.creationDate <= searchCriteria.creationDateTo);
                    hasAnyFilter = true;
                }

                if (searchCriteria.expirationDateFrom != null)
                {
                    query = query.Where(q => q.expirationDate >= searchCriteria.expirationDateFrom);
                    hasAnyFilter = true;
                }

                if (searchCriteria.expirationDateTo != null)
                {
                    query = query.Where(q => q.expirationDate <= searchCriteria.expirationDateTo);
                    hasAnyFilter = true;
                }

                List<String> queryStatuses = new List<String>();

                if (searchCriteria.statusInProgress != null)
                {
                    queryStatuses.Add("In progress");
                }

                if (searchCriteria.statusValidated != null)
                {
                    queryStatuses.Add("Validated");
                }

                if (searchCriteria.statusExpired != null)
                {
                    queryStatuses.Add("Expired");
                }

                if (searchCriteria.statusCancelled != null)
                {
                    queryStatuses.Add("Cancelled");
                }

                if (queryStatuses.Count == 0)
                {
                    return new JsonResult(new ApiResponse { isOkay = false, message = "Quotation status chosen. Please choose at least one status to search.", data = null });
                }
                else
                {
                    query = query.Where(q => queryStatuses.Contains(q.status));
                    hasAnyFilter = true;
                }

                if (!hasAnyFilter)
                {
                    return new JsonResult(new ApiResponse { isOkay = false, message = "No search criteria found. Please apply at least one filter to search.", data = null });
                }

                var searchedQuotations = await query.Take(100).ToListAsync();

                return new JsonResult(new ApiResponse { isOkay = true, message = "Quotations fetched successfully", data = searchedQuotations });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse { isOkay = false, message = ex.Message, data = null });
            }
        }
    }
}
