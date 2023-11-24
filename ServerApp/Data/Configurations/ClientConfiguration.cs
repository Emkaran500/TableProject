using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLib.Models;

namespace ServerApp.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> clientBuilder)
        {
            clientBuilder.HasKey(c => c.Id);

            clientBuilder.HasOne(c => c.Table).WithMany(t => t.Clients).HasForeignKey(c => c.TableId);

            clientBuilder.Property(c => c.TableId).IsRequired().HasColumnName("Table number");
        }
    }
}