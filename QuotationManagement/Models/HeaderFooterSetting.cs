namespace QuotationManagement.Models
{
    public class HeaderFooterSetting
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string branchCode { get; set; }
        public string languageName { get; set; }
        public string languageCode { get; set; }
        public bool status { get; set; }
        public string headerTemplate { get; set; }
        public string footerTemplate { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
        public string? linkedHeaderTemplateCode { get; set; }
        public string? linkedFooterTemplateCode { get; set; }
    }
}
