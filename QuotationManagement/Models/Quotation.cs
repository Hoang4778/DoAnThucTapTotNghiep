namespace QuotationManagement.Models
{
    public class Quotation
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string customerName { get; set; }
        public string accountCode { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime expirationDate { get; set; }
        public DateTime updatedOn { get; set; }
        public string status { get; set; }
    }
}
