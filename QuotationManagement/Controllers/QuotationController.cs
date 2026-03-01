using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using QuotationManagement.Models;
using QuotationManagement.Models.ExternalModels;

namespace QuotationManagement.Controllers
{
    public class QuotationController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _env;

        public QuotationController(IWebHostEnvironment env, HttpClient httpClient)
        {
            _env = env;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Search");
        }

        public IActionResult Search()
        {
            return View("/Views/Quotation/Search.cshtml");
        }

        public async Task<IActionResult> Create()
        {
            var IMApiToken = Environment.GetEnvironmentVariable("IM_API_Access_Token");
            var IMApiEndpoint = Environment.GetEnvironmentVariable("IM_API_Endpoint");
            List<IMBranch> branches = new List<IMBranch>();
            string userPUID = "";

            try
            {
                var IMIdentityRequest = new HttpRequestMessage(
                    HttpMethod.Get,
                    IMApiEndpoint + $"/identities?filters[emailAddress][$eq]={User.FindFirst(ClaimTypes.Email)?.Value}&populate=sales_scopes&fields[0]=PUID"
                );
                IMIdentityRequest.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", IMApiToken);
                var IMIdentityResponse = await _httpClient.SendAsync(IMIdentityRequest);

                if (!IMIdentityResponse.IsSuccessStatusCode)
                {
                    return View("/Views/Shared/Error.cshtml", new Error
                    {
                        message = IMIdentityResponse.ReasonPhrase,
                        code = IMIdentityResponse.StatusCode
                    });
                }

                var IMIdentityRawData = await IMIdentityResponse.Content.ReadAsStringAsync();
                var IMIdentityGeneralResponse = JsonSerializer.Deserialize<IMIdentityResponse>(IMIdentityRawData);

                if (IMIdentityGeneralResponse?.data.Count == 0)
                {
                    return View("/Views/Shared/Error.cshtml", new Error
                    {
                        message = "No user information found. Please try again later.",
                        code = System.Net.HttpStatusCode.NotFound
                    });
                }

                IMIdentity user = IMIdentityGeneralResponse?.data[0];
                userPUID = user.PUID;
                var salesScopes = user.sales_scopes.Select(user => user.countryCode);
                List<string> filterParamList = new List<string>();
                string filterParamStr = "";

                int i = 0;
                foreach (var salesScope in salesScopes)
                {
                    filterParamList.Add($"filters[country][$in][{i}]={salesScope}");
                    i += 1;
                }
                filterParamStr = string.Join("&", filterParamList);

                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    IMApiEndpoint + $"/branches?pagination[pageSize]=100&fields[0]=name&fields[1]=branchCode&{filterParamStr}"
                );
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", IMApiToken);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View("/Views/Shared/Error.cshtml", new Error
                    {
                        message = response.ReasonPhrase,
                        code = response.StatusCode
                    });
                }

                var rawData = await response.Content.ReadAsStringAsync();
                var generalResponse = JsonSerializer.Deserialize<IMBranchResponse>(rawData);
                branches = generalResponse != null ? generalResponse.data : new List<IMBranch>();

                return View("/Views/Quotation/Create.cshtml", branches);
            }
            catch (Exception ex)
            {
                return View("/Views/Shared/Error.cshtml", new Error
                {
                    message = ex.Message,
                    code = System.Net.HttpStatusCode.InternalServerError
                });
            }
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
