using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence.Configuration
{
    public class PermissionTypeConfiguration : IEntityTypeConfiguration<PermissionType>
    {
        public void Configure(EntityTypeBuilder<PermissionType> builder)
        {
            builder.HasKey(x => x.Id);
            // id identity
            builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

            builder.Property(x => x.Description)
                   .IsRequired();
        }
    }
}
