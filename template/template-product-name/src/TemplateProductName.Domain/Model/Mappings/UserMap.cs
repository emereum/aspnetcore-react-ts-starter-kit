using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TemplateProductName.Domain.Model.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(x => x.LastLogin)
                .HasColumnName("last_login");

            var passwordBuilder = builder.OwnsOne(x => x.Password);

            passwordBuilder
                .Property(x => x.Hash)
                .HasColumnName("password_hash")
                .IsRequired();

            passwordBuilder
                .Property(x => x.Salt)
                .HasColumnName("password_salt")
                .IsRequired();
        }
    }
}
