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

            tableBuilder.Property(t => t.TableNumber).IsRequired().HasColumnName("Table number");

            tableBuilder.Property(t => t.OperatorId).IsRequired().HasColumnName("Operator Id");
        }
    }
}