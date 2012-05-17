using System.Data.Entity;
using WakeyWakey.Web.Migrations;

namespace WakeyWakey.Web.Models
{
    public class WakeyCatalog : DbContext
    {
        public DbSet<Machine> Machines { get; set; }

        public WakeyCatalog() : base("WakeyWakey")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WakeyCatalog, WakeyDbConfiguration>());
        }
    }
}