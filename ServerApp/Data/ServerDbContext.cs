using Microsoft.EntityFrameworkCore;
using ServerApp.Data.Configurations;
using SharedLib.Models;

namespace ServerApp.Data
{
    public class ServerDbContext : DbContext
    {
        public DbSet<Table> Tables { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<TableOperator> TableOperators { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            try
            {
                optionsBuilder.UseSqlServer(connectionString: "Server=localhost;Database=TableDb;TrustServerCertificate=True;Integrated Security=SSPI;");
            }
            catch (Exception)
            {
                optionsBuilder.UseSqlServer(connectionString: "Server=localhost;Database=TableDb;User Id=myUsername;Password=myPassword;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new OperatorConfiguration());
            modelBuilder.ApplyConfiguration(new TableConfiguration());
            modelBuilder.ApplyConfiguration(new TableOperatorConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}