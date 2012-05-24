using System.Data.Entity;
using System.IO;
using System.Web.Hosting;
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
            // Sql Ce has problems with creating the default database unless the App_Data folder exists
            var path = HostingEnvironment.MapPath("~/App_Data");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WakeyCatalog, WakeyDbConfiguration>());
        }
    }
}