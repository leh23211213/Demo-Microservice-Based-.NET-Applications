
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using template.Models;
namespace template.Data.Configuration
{
    public class SizeConfiguration : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.RAM).IsRequired().HasMaxLength(50);
        }
    }
}
