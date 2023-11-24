using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLib.Models;

namespace ServerApp.Data.Configurations
{
    public class OperatorConfiguration : IEntityTypeConfiguration<Operator>
    {
        public void Configure(EntityTypeBuilder<Operator> operatorBuilder)
        {
            operatorBuilder.HasKey(o => o.Id);

            operatorBuilder.HasOne(o => o.Table).WithOne(t => t.Operator).HasForeignKey<Operator>(o => o.TableId);

            operatorBuilder.Property(o => o.Name).HasMaxLength(50);

            operatorBuilder.Property(o => o.TableId).IsRequired().HasColumnName("Table number");
        }
    }
}