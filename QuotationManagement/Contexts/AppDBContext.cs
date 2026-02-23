using Microsoft.EntityFrameworkCore;
using QuotationManagement.Models;

namespace QuotationManagement.Contexts
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<HeaderFooterTemplate> HeaderFooter_Template { get; set; }
        public DbSet<HeaderFooterSetting> HeaderFooter_Setting { get; set; }
        public DbSet<BranchDefaultSettings> Branch_DefaultSettings { get; set; }
        public DbSet<BranchDefaultSettings_QuotationType> Branch_DefaultSettings_QuotationType { get; set; }
        public DbSet<BranchDefaultSettings_CustomerAcceptanceBox> Branch_DefaultSettings_CustomerAcceptanceBox { get; set; }
        public DbSet<BranchDefaultSettings_QuotationFooterPosition> Branch_DefaultSettings_QuotationFooterPosition { get; set; }
        public DbSet<BranchDefaultSettings_LetterTopTemplate> Branch_DefaultSettings_LetterTopTemplate { get; set; }
    }
}
