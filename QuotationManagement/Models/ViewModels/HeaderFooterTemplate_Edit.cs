using QuotationManagement.Models.ExternalModels;

namespace QuotationManagement.Models.ViewModels
{
    public class HeaderFooterTemplate_Edit
    {
        public List<Language> languageList { get; set; }
        public List<IMBranch> branchList { get; set; }
        public string userPUID { get; set; }
        public HeaderFooterTemplate template { get; set; }
    }
}
