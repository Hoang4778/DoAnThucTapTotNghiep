namespace QuotationManagement.Models
{
    public class BranchDefaultSettings
    {
        public int id { get; set; }
        public string branchCode { get; set; }
        public int quotationTypeId { get; set; }
        public decimal defaultTaxRate { get; set; }
        public string headerFooterSettingCode { get; set; }
        public bool hasGlobalDiscount { get; set; }
        public decimal globalDiscountAmount { get; set; }
        public bool hasGlobalSurcharge { get; set; }
        public decimal globalSurchargeAmount { get; set; }
        public bool hasPageBreakBeforeTable { get; set; }
        public bool hasContactInfoInCustomerDetails { get; set; }
        public bool hasQuantityAndUnitPriceColumns { get; set; }
        public bool hasPageBreakAfterTable { get; set; }
        public bool showQuotationStartDate { get; set; }
        public bool showQuotationExpirationDate { get; set; }
        public int customerAcceptanceSignatureBoxId { get; set; }
        public int quotationFooterPositionId { get; set; }
        public int letterTopTemplateId { get; set; }
    }
}
