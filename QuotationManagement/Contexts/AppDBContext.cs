using Microsoft.EntityFrameworkCore;
using QuotationManagement.Models;

namespace QuotationManagement.Contexts
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<HeaderFooterTemplate> HeaderFooter_Template { get; set; }
        public DbSet<HeaderFooterSetting> HeaderFooter_Setting { get; set; }
    }
}
