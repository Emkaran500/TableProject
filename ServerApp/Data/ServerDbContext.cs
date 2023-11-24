using Microsoft.EntityFrameworkCore;
using SharedLib.Models;

namespace ServerApp.Data
{
    public class ServerDbContext : DbContext
    {
        public DbSet<Table> Tables { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Operator> Operators { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            try
            {
                optionsBuilder.UseSqlServer(connectionString: "Server=localhost;Database=HoldDb;TrustServerCertificate=True;Integrated Security=SSPI;");
            }
            catch (Exception)
            {
                optionsBuilder.UseSqlServer(connectionString: "Server=localhost;Database=HoldDb;User Id=myUsername;Password=myPassword;TrustServerCertificate=True;");
            }
        }
    }
}