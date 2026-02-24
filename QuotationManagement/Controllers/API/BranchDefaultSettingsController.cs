using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotationManagement.Contexts;
using QuotationManagement.Models;
using QuotationManagement.Models.API;

namespace QuotationManagement.Controllers.API
{
    public class BranchDefaultSettingsController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public BranchDefaultSettingsController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("api/branch-default-settings/get-setting-by-branch-code/{branchCode}")]
        public async Task<JsonResult> getSettingByBranchCode(string branchCode)
        {
            List<HeaderFooterSetting> headerFooterSettings = new List<HeaderFooterSetting>();

            try
            {
                var branchSetting = await _appDBContext.Branch_DefaultSettings.Where(setting => setting.branchCode == branchCode).FirstOrDefaultAsync();

                var allHeaderFooterSettings = await _appDBContext.HeaderFooter_Setting.Where(setting => setting.branchCode == branchCode).ToListAsync();

                Get_BranchDefaultSettingsByBranchCode result = new Get_BranchDefaultSettingsByBranchCode()
                {
                    branchSetting = branchSetting ?? new BranchDefaultSettings(),
                    headerFooterSettings = allHeaderFooterSettings
                };

                return new JsonResult(new ApiResponse { isOkay = true, message = "Branch settings fetched successfully", data = result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse { isOkay = false, message = ex.Message, data = null });
            }
        }

        [HttpPost]
        [Route("api/branch-default-settings/create-or-update-setting")]
        public async Task<JsonResult> createOrUpdateSetting()
        {
            try
            {
                var reader = new StreamReader(Request.Body);
                string rawSettingInfo = await reader.ReadToEndAsync();
                var settingInfo = JsonSerializer.Deserialize<BranchDefaultSettings>(rawSettingInfo);

                if (settingInfo == null)
                {
                    return new JsonResult(new ApiResponse { isOkay = false, message = "Bad data input. Please try again.", data = null });
                }

                if (settingInfo.id == null)
                {
                    BranchDefaultSettings setting = new BranchDefaultSettings();

                    setting.branchCode = settingInfo.branchCode;
                    setting.quotationTypeId = settingInfo.quotationTypeId;
                    setting.defaultTaxRate = settingInfo.defaultTaxRate;
                    setting.headerFooterSettingCode = settingInfo.headerFooterSettingCode;
                    setting.hasGlobalDiscount = settingInfo.hasGlobalDiscount;
                    setting.globalDiscountAmount = settingInfo.globalDiscountAmount;
                    setting.hasGlobalSurcharge = settingInfo.hasGlobalSurcharge;
                    setting.globalSurchargeAmount = settingInfo.globalSurchargeAmount;
                    setting.hasPageBreakBeforeTable = settingInfo.hasPageBreakBeforeTable;
                    setting.hasContactInfoInCustomerDetails = settingInfo.hasContactInfoInCustomerDetails;
                    setting.hasQuantityAndUnitPriceColumns = settingInfo.hasQuantityAndUnitPriceColumns;
                    setting.hasPageBreakAfterTable = settingInfo.hasPageBreakAfterTable;
                    setting.showQuotationStartDate = settingInfo.showQuotationStartDate;
                    setting.showQuotationExpirationDate = settingInfo.showQuotationExpirationDate;
                    setting.customerAcceptanceSignatureBoxId = settingInfo.customerAcceptanceSignatureBoxId;
                    setting.quotationFooterPositionId = settingInfo.quotationFooterPositionId;
                    setting.letterTopTemplateId = settingInfo.letterTopTemplateId;

                    _appDBContext.Branch_DefaultSettings.Add(setting);
                    await _appDBContext.SaveChangesAsync();

                    return new JsonResult(new ApiResponse { isOkay = true, message = "Branch setting created successfully.", data = setting });
                }
                else
                {
                    var setting = await _appDBContext.Branch_DefaultSettings.Where(setting => setting.id == settingInfo.id).FirstOrDefaultAsync();

                    if (setting == null)
                    {
                        return new JsonResult(new ApiResponse { isOkay = false, message = "No branch setting found. Please try again", data = null });
                    }

                    setting.branchCode = settingInfo.branchCode;
                    setting.quotationTypeId = settingInfo.quotationTypeId;
                    setting.defaultTaxRate = settingInfo.defaultTaxRate;
                    setting.headerFooterSettingCode = settingInfo.headerFooterSettingCode;
                    setting.hasGlobalDiscount = settingInfo.hasGlobalDiscount;
                    setting.globalDiscountAmount = settingInfo.globalDiscountAmount;
                    setting.hasGlobalSurcharge = settingInfo.hasGlobalSurcharge;
                    setting.globalSurchargeAmount = settingInfo.globalSurchargeAmount;
                    setting.hasPageBreakBeforeTable = settingInfo.hasPageBreakBeforeTable;
                    setting.hasContactInfoInCustomerDetails = settingInfo.hasContactInfoInCustomerDetails;
                    setting.hasQuantityAndUnitPriceColumns = settingInfo.hasQuantityAndUnitPriceColumns;
                    setting.hasPageBreakAfterTable = settingInfo.hasPageBreakAfterTable;
                    setting.showQuotationStartDate = settingInfo.showQuotationStartDate;
                    setting.showQuotationExpirationDate = settingInfo.showQuotationExpirationDate;
                    setting.customerAcceptanceSignatureBoxId = settingInfo.customerAcceptanceSignatureBoxId;
                    setting.quotationFooterPositionId = settingInfo.quotationFooterPositionId;
                    setting.letterTopTemplateId = settingInfo.letterTopTemplateId;

                    await _appDBContext.SaveChangesAsync();

                    return new JsonResult(new ApiResponse { isOkay = true, message = "Branch setting updated successfully.", data = setting });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse { isOkay = false, message = ex.Message, data = null });
            }
        }
    }
}
