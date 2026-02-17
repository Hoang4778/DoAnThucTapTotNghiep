namespace QuotationManagement.Models.ExternalModels
{
    public class IMIdentity
    {
        public int id { get; set; }
        public string documentId { get; set; }
        public string PUID { get; set; }
        public List<IMSalesScope> sales_scopes { get; set; }
    }
}
