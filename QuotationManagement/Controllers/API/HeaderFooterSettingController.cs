using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotationManagement.Contexts;
using QuotationManagement.Models;

namespace QuotationManagement.Controllers.API
{
    public class HeaderFooterSettingController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public HeaderFooterSettingController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public async Task<int> getSettingCount()
        {
            int settingCount = await _appDBContext.HeaderFooter_Setting.CountAsync();

            return settingCount;
        }

        [HttpPost]
        [Route("api/settings/header-footer/create")]
        public async Task<JsonResult> createSetting()
        {
            try
            {
                var reader = new StreamReader(Request.Body);
                string rawSettingInfo = await reader.ReadToEndAsync();
                var settingInfo = JsonSerializer.Deserialize<HeaderFooterSetting>(rawSettingInfo);

                if (settingInfo == null)
                {
                    return new JsonResult(new ApiResponse { isOkay = false, message = "Bad data input. Please try again.", data = null });
                }

                int settingCount = await getSettingCount();
                HeaderFooterSetting setting = new HeaderFooterSetting();

                setting.code = $"HFS{settingInfo.code}{(settingCount + 1).ToString("0000")}";
                setting.name = settingInfo.name;
                setting.branchCode = settingInfo.branchCode;
                setting.languageName = settingInfo.languageName;
                setting.languageCode = settingInfo.languageCode;
                setting.status = settingInfo.status;
                setting.headerTemplate = settingInfo.headerTemplate;
                setting.footerTemplate = settingInfo.footerTemplate;
                setting.createdOn = DateTime.UtcNow;
                setting.updatedOn = null;
                setting.linkedHeaderTemplateCode = settingInfo.linkedHeaderTemplateCode;
                setting.linkedFooterTemplateCode = settingInfo.linkedFooterTemplateCode;

                _appDBContext.HeaderFooter_Setting.Add(setting);
                await _appDBContext.SaveChangesAsync();

                return new JsonResult(new ApiResponse { isOkay = true, message = "Setting created successfully.", data = setting });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse { isOkay = false, message = ex.Message, data = null });
            }
        }

        [HttpPost]
        [Route("api/settings/header-footer/edit")]
        public async Task<JsonResult> editSetting()
        {
            try
            {
                var reader = new StreamReader(Request.Body);
                string rawSettingInfo = await reader.ReadToEndAsync();
                var settingInfo = JsonSerializer.Deserialize<HeaderFooterSetting>(rawSettingInfo);

                if (settingInfo == null)
                {
                    return new JsonResult(new ApiResponse { isOkay = false, message = "Bad data input. Please try again.", data = null });
                }

                var setting = await _appDBContext.HeaderFooter_Setting.Where(setting => setting.id == settingInfo.id).FirstOrDefaultAsync();

                if (setting == null)
                {
                    return new JsonResult(new ApiResponse { isOkay = false, message = "No setting found. Please try again.", data = null });
                }

                setting.name = settingInfo.name;
                setting.branchCode = settingInfo.branchCode;
                setting.languageName = settingInfo.languageName;
                setting.languageCode = settingInfo.languageCode;
                setting.status = settingInfo.status;
                setting.headerTemplate = settingInfo.headerTemplate;
                setting.footerTemplate = settingInfo.footerTemplate;
                setting.updatedOn = DateTime.UtcNow;
                setting.linkedHeaderTemplateCode = settingInfo.linkedHeaderTemplateCode;
                setting.linkedFooterTemplateCode = settingInfo.linkedFooterTemplateCode;

                await _appDBContext.SaveChangesAsync();

                return new JsonResult(new ApiResponse { isOkay = true, message = "Setting changed successfully.", data = setting });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse { isOkay = false, message = ex.Message, data = null });
            }
        }
    }
}
