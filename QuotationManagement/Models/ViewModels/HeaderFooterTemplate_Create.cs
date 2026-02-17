using QuotationManagement.Models.ExternalModels;

namespace QuotationManagement.Models.ViewModels
{
    public class HeaderFooterTemplate_Create
    {
        public List<Language> languageList { get; set; }
        public List<IMBranch> branchList { get; set; }
        public string userPUID { get; set; }
    }
}
