using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotationManagement.Contexts;
using QuotationManagement.Models;

namespace QuotationManagement.Controllers.API
{
    public class HeaderFooterTemplateController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public HeaderFooterTemplateController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public async Task<int> getTemplateCount()
        {
            int count = await _appDBContext.HeaderFooter_Template.CountAsync();
            return count;
        }

        [HttpPost]
        [Route("api/settings/header-footer-template/create")]
        public async Task<JsonResult> createTemplate()
        {
            try
            {
                var reader = new StreamReader(Request.Body);
                string rawTemplateInfo = await reader.ReadToEndAsync();
                var templateInfo = JsonSerializer.Deserialize<HeaderFooterTemplate>(rawTemplateInfo);

                if (templateInfo != null)
                {
                    int templateCount = await getTemplateCount();
                    HeaderFooterTemplate template = new HeaderFooterTemplate();

                    template.code = $"HFT{templateInfo.code}{(templateCount + 1).ToString("0000")}";
                    template.name = templateInfo.name;
                    template.branchCode = templateInfo.branchCode;
                    template.languageName = templateInfo.languageName;
                    template.languageCode = templateInfo.languageCode;
                    template.usableAsHeader = templateInfo.usableAsHeader;
                    template.usableAsFooter = templateInfo.usableAsFooter;
                    template.status = templateInfo.status;
                    template.content = templateInfo.content;
                    template.createdOn = DateTime.UtcNow;
                    template.updatedOn = null;

                    _appDBContext.HeaderFooter_Template.Add(template);
                    await _appDBContext.SaveChangesAsync();

                    return new JsonResult(new ApiResponse { isOkay = true, message = "Template created successfully.", data = template });
                }
                else
                {
                    return new JsonResult(new ApiResponse { isOkay = false, message = "Bad data input. Please try again.", data = null });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse { isOkay = false, message = ex.Message, data = null });
            }
        }

        [HttpPost]
        [Route("api/settings/header-footer-template/edit")]
        public async Task<JsonResult> updateTemplate()
        {
            try
            {
                var reader = new StreamReader(Request.Body);
                string rawTemplateInfo = await reader.ReadToEndAsync();
                var templateInfo = JsonSerializer.Deserialize<HeaderFooterTemplate>(rawTemplateInfo);

                if (templateInfo == null)
                {
                    return new JsonResult(new ApiResponse { isOkay = false, message = "Bad data input. Please try again.", data = null });
                }

                var template = await _appDBContext.HeaderFooter_Template.Where(template => template.id == templateInfo.id).FirstOrDefaultAsync();

                if (template == null)
                {
                    return new JsonResult(new ApiResponse { isOkay = false, message = "The template to edit is not found. Please try again.", data = null });
                }

                template.name = templateInfo.name;
                template.branchCode = templateInfo.branchCode;
                template.languageName = templateInfo.languageName;
                template.languageCode = templateInfo.languageCode;
                template.usableAsHeader = templateInfo.usableAsHeader;
                template.usableAsFooter = templateInfo.usableAsFooter;
                template.status = templateInfo.status;
                template.content = templateInfo.content;
                template.updatedOn = DateTime.UtcNow;

                await _appDBContext.SaveChangesAsync();

                return new JsonResult(new ApiResponse { isOkay = true, message = "Template edited successfully.", data = template });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse { isOkay = false, message = ex.Message, data = null });
            }
        }
    }
}
