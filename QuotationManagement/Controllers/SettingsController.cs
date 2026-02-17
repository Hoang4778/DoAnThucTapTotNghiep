using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using QuotationManagement.Contexts;
using QuotationManagement.Models;
using QuotationManagement.Models.ExternalModels;
using QuotationManagement.Models.ViewModels;

namespace QuotationManagement.Controllers
{
    public class SettingsController : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly HttpClient _httpClient;

        public SettingsController(AppDBContext dBContext, IWebHostEnvironment env, HttpClient httpClient)
        {
            _dbContext = dBContext;
            _env = env;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Default");
        }

        public IActionResult Default()
        {
            return View("/Views/Settings/Default.cshtml");
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

        public IActionResult HeaderFooterTemplates()
        {
            return View("/Views/Settings/HeaderFooterTemplates/Index.cshtml");
        }

        public async Task<IActionResult> HeaderFooterTemplatesCreate()
        {
            List<IMBranch> branches = new List<IMBranch>();
            string userPUID = "";

            var IMApiToken = Environment.GetEnvironmentVariable("IM_API_Access_Token");
            var IMApiEndpoint = Environment.GetEnvironmentVariable("IM_API_Endpoint");

            var languageFilePath = Path.Combine(_env.WebRootPath, "languages", "languages.json");
            var json = System.IO.File.ReadAllText(languageFilePath);
            var languages = JsonSerializer.Deserialize<List<Language>>(json);

            try
            {
                var IMIdentityRequest = new HttpRequestMessage(
                    HttpMethod.Get,
                    IMApiEndpoint + $"/identities?filters[emailAddress][$eq]={User.FindFirst(ClaimTypes.Email)?.Value}&populate=sales_scopes&fields[0]=PUID"
                );
                IMIdentityRequest.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", IMApiToken);
                var IMIdentityResponse = await _httpClient.SendAsync(IMIdentityRequest);

                if (IMIdentityResponse.IsSuccessStatusCode)
                {
                    var IMIdentityRawData = await IMIdentityResponse.Content.ReadAsStringAsync();
                    var IMIdentityGeneralResponse = JsonSerializer.Deserialize<IMIdentityResponse>(IMIdentityRawData);

                    if (IMIdentityGeneralResponse?.data.Count > 0)
                    {
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

                        if (response.IsSuccessStatusCode)
                        {
                            var rawData = await response.Content.ReadAsStringAsync();
                            var generalResponse = JsonSerializer.Deserialize<IMBranchResponse>(rawData);
                            branches = generalResponse != null ? generalResponse.data : new List<IMBranch>();
                        }
                        else
                        {
                            return View("/Views/Shared/Error.cshtml", new Error
                            {
                                message = response.ReasonPhrase,
                                code = response.StatusCode
                            });
                        }
                    }
                    else
                    {
                        return View("/Views/Shared/Error.cshtml", new Error
                        {
                            message = "No user information found. Please try again later.",
                            code = System.Net.HttpStatusCode.NotFound
                        });
                    }
                }
                else
                {
                    return View("/Views/Shared/Error.cshtml", new Error
                    {
                        message = IMIdentityResponse.ReasonPhrase,
                        code = IMIdentityResponse.StatusCode
                    });
                }
            }
            catch (Exception ex)
            {
                return View("/Views/Shared/Error.cshtml", new Error
                {
                    message = ex.Message,
                    code = System.Net.HttpStatusCode.InternalServerError
                });
            }


            HeaderFooterTemplate_Create viewModel = new HeaderFooterTemplate_Create()
            {
                languageList = languages ?? new List<Language>(),
                branchList = branches,
                userPUID = userPUID,
            };

            return View("/Views/Settings/HeaderFooterTemplates/Create.cshtml", viewModel);
        }

        public IActionResult HeaderFooterTemplatesEdit(string templateCode)
        {
            return View("/Views/Settings/HeaderFooterTemplates/Edit.cshtml");
        }
    }
}
