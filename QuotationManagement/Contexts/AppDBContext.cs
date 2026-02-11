using Microsoft.EntityFrameworkCore;

namespace QuotationManagement.Contexts
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

    }
}
