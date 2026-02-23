namespace QuotationManagement.Models.API
{
    public class Get_BranchDefaultSettingsByBranchCode
    {
        public BranchDefaultSettings branchSetting { get; set; }
        public List<HeaderFooterSetting> headerFooterSettings { get; set; }
    }
}
