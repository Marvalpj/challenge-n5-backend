using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence.Configuration
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {

            builder.HasKey(x => x.Id);

            // id identity
            builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

            builder.Property(x => x.NameEmployee)
                   .IsRequired();

            builder.Property(x => x.LastNameEmployee)
                   .IsRequired();

            builder.Property(x => x.PermissionTypeId)
                   .IsRequired();

            builder.Property(x => x.Date)
                   .IsRequired();

            builder.Ignore(x => x.FullName);

            // Configura la relación con PermissionType
            builder.HasOne(x => x.PermissionType) // Relación de uno a muchos
                   .WithMany(p => p.Permissions)
                   .HasForeignKey(x => x.PermissionTypeId); // Clave foránea
        }
    }
}
