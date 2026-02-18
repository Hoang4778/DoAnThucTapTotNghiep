namespace QuotationManagement.Models
{
    public class HeaderFooterTemplate
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string branchCode { get; set; }
        public string languageName { get; set; }
        public string languageCode { get; set; }
        public bool usableAsHeader { get; set; }
        public bool usableAsFooter { get; set; }
        public bool status { get; set; }
        public string content { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
