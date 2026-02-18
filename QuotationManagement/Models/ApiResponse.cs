namespace QuotationManagement.Models
{
    public class ApiResponse
    {
        public bool isOkay { get; set; }
        public string message { get; set; }
        public object? data { get; set; }
    }
}
