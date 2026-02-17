using System.Net;

namespace QuotationManagement.Models
{
    public class Error
    {
        public HttpStatusCode code { get; set; }
        public string message { get; set; }
    }
}
