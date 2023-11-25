using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLib.Models;

namespace ServerApp.Data.Configurations
{
    public class TableOperatorConfiguration : IEntityTypeConfiguration<TableOperator>
    {
        public void Configure(EntityTypeBuilder<TableOperator> tableOperatorBuilder)
        {
            tableOperatorBuilder.HasKey(to => to.Id);

            tableOperatorBuilder.HasOne(to => to.Operator).WithOne(o => o.TableOperator).HasForeignKey<TableOperator>(to => to.OperatorId);

            tableOperatorBuilder.HasOne(to => to.Table).WithOne(t => t.TableOperator).HasForeignKey<TableOperator>(to => to.TableId);

            tableOperatorBuilder.Property(to => to.OperatorId).IsRequired().HasColumnName("Operator Id");

            tableOperatorBuilder.Property(to => to.TableId).IsRequired().HasColumnName("Table Id");
        }
    }
}