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

            clientBuilder.Property(c => c.TableId).IsRequired().HasColumnName("Table number");
        }
    }
}