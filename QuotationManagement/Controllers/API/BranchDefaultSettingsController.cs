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
            BranchDefaultSettings setting = new BranchDefaultSettings();
            List<HeaderFooterSetting> headerFooterSettings = new List<HeaderFooterSetting>();

            try
            {
                var branchSetting = await _appDBContext.Branch_DefaultSettings.Where(setting => setting.branchCode == branchCode).FirstOrDefaultAsync();

                var allHeaderFooterSettings = await _appDBContext.HeaderFooter_Setting.Where(setting => setting.branchCode == branchCode).ToListAsync();

                Get_BranchDefaultSettingsByBranchCode result = new Get_BranchDefaultSettingsByBranchCode()
                {
                    branchSetting = setting,
                    headerFooterSettings = allHeaderFooterSettings
                };

                return new JsonResult(new ApiResponse { isOkay = true, message = "Branch settings fetched successfully", data = result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse { isOkay = false, message = ex.Message, data = null });
            }
        }
    }
}
