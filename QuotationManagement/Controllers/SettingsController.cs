using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Default()
        {
            List<IMBranch> branches = new List<IMBranch>();
            string userPUID = "";

            var IMApiToken = Environment.GetEnvironmentVariable("IM_API_Access_Token");
            var IMApiEndpoint = Environment.GetEnvironmentVariable("IM_API_Endpoint");

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

            }
            catch (Exception ex)
            {
                return View("/Views/Shared/Error.cshtml", new Error
                {
                    message = ex.Message,
                    code = System.Net.HttpStatusCode.InternalServerError
                });
            }

            return View("/Views/Settings/Default.cshtml", branches);
        }

        public async Task<IActionResult> HeaderFooter()
        {
            var settings = await _dbContext.HeaderFooter_Setting.ToListAsync();

            return View("/Views/Settings/HeaderFooter/Index.cshtml", settings);
        }

        public async Task<IActionResult> HeaderFooterCreate()
        {
            List<IMBranch> branches = new List<IMBranch>();
            string userPUID = "";
            List<HeaderFooterTemplate> headerTemplates = new List<HeaderFooterTemplate>();
            List<HeaderFooterTemplate> footerTemplates = new List<HeaderFooterTemplate>();

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
                var branchCodes = branches.Select(branch => branch.branchCode);

                var headerFooterTemplates = await _dbContext.HeaderFooter_Template.Where(template => template.status == true && branchCodes.Contains(template.branchCode)).ToListAsync();
                headerTemplates = headerFooterTemplates.Where(template => template.usableAsHeader == true).ToList();
                footerTemplates = headerFooterTemplates.Where(template => template.usableAsFooter == true).ToList();
            }
            catch (Exception ex)
            {
                return View("/Views/Shared/Error.cshtml", new Error
                {
                    message = ex.Message,
                    code = System.Net.HttpStatusCode.InternalServerError
                });
            }

            HeaderFooterSetting_Create viewModel = new HeaderFooterSetting_Create()
            {
                languageList = languages ?? new List<Language>(),
                branchList = branches,
                userPUID = userPUID,
                headerTemplates = headerTemplates,
                footerTemplates = footerTemplates
            };

            return View("/Views/Settings/HeaderFooter/Create.cshtml", viewModel);
        }

        public async Task<IActionResult> HeaderFooterEdit(string settingCode)
        {
            List<IMBranch> branches = new List<IMBranch>();
            string userPUID = "";
            List<HeaderFooterTemplate> headerTemplates = new List<HeaderFooterTemplate>();
            List<HeaderFooterTemplate> footerTemplates = new List<HeaderFooterTemplate>();
            HeaderFooterSetting setting = new HeaderFooterSetting();

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
                var branchCodes = branches.Select(branch => branch.branchCode);

                var headerFooterTemplates = await _dbContext.HeaderFooter_Template.Where(template => template.status == true && branchCodes.Contains(template.branchCode)).ToListAsync();
                headerTemplates = headerFooterTemplates.Where(template => template.usableAsHeader == true).ToList();
                footerTemplates = headerFooterTemplates.Where(template => template.usableAsFooter == true).ToList();

                var searchedSetting = await _dbContext.HeaderFooter_Setting.Where(setting => setting.code == settingCode).FirstOrDefaultAsync();
                setting = searchedSetting ?? new HeaderFooterSetting();
            }
            catch (Exception ex)
            {
                return View("/Views/Shared/Error.cshtml", new Error
                {
                    message = ex.Message,
                    code = System.Net.HttpStatusCode.InternalServerError
                });
            }

            HeaderFooterSetting_Edit viewModel = new HeaderFooterSetting_Edit()
            {
                languageList = languages ?? new List<Language>(),
                branchList = branches,
                userPUID = userPUID,
                headerTemplates = headerTemplates,
                footerTemplates = footerTemplates,
                setting = setting
            };

            return View("/Views/Settings/HeaderFooter/Edit.cshtml", viewModel);
        }

        public async Task<IActionResult> HeaderFooterTemplates()
        {
            try
            {
                var templates = await _dbContext.HeaderFooter_Template.ToListAsync();

                return View("/Views/Settings/HeaderFooterTemplates/Index.cshtml", templates);
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

        public async Task<IActionResult> HeaderFooterTemplatesEdit(string templateCode)
        {
            List<IMBranch> branches = new List<IMBranch>();
            string userPUID = "";
            HeaderFooterTemplate template = new HeaderFooterTemplate();
            List<HeaderFooterSetting> linkedSettings = new List<HeaderFooterSetting>();

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

                var searchedTemplate = await _dbContext.HeaderFooter_Template.Where(template => template.code == templateCode).FirstOrDefaultAsync();

                if (searchedTemplate == null)
                {
                    return View("/Views/Shared/Error.cshtml", new Error
                    {
                        message = "No header/footer template found. Please try again later.",
                        code = System.Net.HttpStatusCode.NotFound
                    });
                }

                template = searchedTemplate;

                var searchedSettings = await _dbContext.HeaderFooter_Setting.Where(setting => setting.linkedHeaderTemplateCode == template.code || setting.linkedFooterTemplateCode == template.code).ToListAsync();
                linkedSettings = searchedSettings;
            }
            catch (Exception ex)
            {
                return View("/Views/Shared/Error.cshtml", new Error
                {
                    message = ex.Message,
                    code = System.Net.HttpStatusCode.InternalServerError
                });
            }

            HeaderFooterTemplate_Edit viewModel = new HeaderFooterTemplate_Edit()
            {
                languageList = languages ?? new List<Language>(),
                branchList = branches,
                userPUID = userPUID,
                template = template,
                linkedSettings = linkedSettings,
            };

            return View("/Views/Settings/HeaderFooterTemplates/Edit.cshtml", viewModel);
        }
    }
}
