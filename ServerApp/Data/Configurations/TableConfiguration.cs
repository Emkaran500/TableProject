using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLib.Models;

namespace ServerApp.Data.Configurations
{
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> tableBuilder)
        {
            tableBuilder.HasKey(t => t.Id);

            tableBuilder.HasOne(t => t.Operator).WithOne(o => o.Table).HasForeignKey<Table>(t => t.OperatorId);

            tableBuilder.HasMany(t => t.Clients).WithOne(c => c.Table).HasForeignKey(c => c.TableId);

            tableBuilder.Property(t => t.TableNumber).IsRequired().HasColumnName("Table number");

            tableBuilder.Property(t => t.OperatorId).IsRequired().HasColumnName("Operator Id");
        }
    }
}