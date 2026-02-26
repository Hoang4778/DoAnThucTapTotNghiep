namespace QuotationManagement.Models
{
    public class QuotationSearchCriteria
    {
        public string? quotationCode { get; set; }
        public string? quotationName { get; set; }
        public string? customerName { get; set; }
        public string? accountCode { get; set; }
        public DateTime? creationDateFrom { get; set; }
        public DateTime? creationDateTo { get; set; }
        public DateTime? expirationDateFrom { get; set; }
        public DateTime? expirationDateTo { get; set; }
        public bool? statusInProgress { get; set; }
        public bool? statusValidated { get; set; }
        public bool? statusExpired { get; set; }
        public bool? statusCancelled { get; set; }
    }
}
