using QuotationManagement.Models.ExternalModels;

namespace QuotationManagement.Models.ViewModels
{
    public class View_BranchDefaultSettings
    {
        public List<IMBranch> branches { get; set; }
        public List<BranchDefaultSettings_QuotationType> quotationTypes { get; set; }
        public List<BranchDefaultSettings_CustomerAcceptanceBox> customerAcceptanceBoxChoices { get; set; }
        public List<BranchDefaultSettings_QuotationFooterPosition> footerPositions { get; set; }
        public List<BranchDefaultSettings_LetterTopTemplate> letterTopTemplates { get; set; }
    }
}
