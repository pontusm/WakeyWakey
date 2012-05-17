using System.Data.Entity.Migrations;
using WakeyWakey.Web.Models;

namespace WakeyWakey.Web.Migrations
{
    public class WakeyDbConfiguration : DbMigrationsConfiguration<WakeyCatalog>
    {
        public WakeyDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}