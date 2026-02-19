using QuotationManagement.Models.ExternalModels;

namespace QuotationManagement.Models.ViewModels
{
    public class HeaderFooterSetting_Create
    {
        public List<Language> languageList { get; set; }
        public List<IMBranch> branchList { get; set; }
        public string userPUID { get; set; }
        public List<HeaderFooterTemplate> headerTemplates { get; set; }
        public List<HeaderFooterTemplate> footerTemplates { get; set; }
    }
}
