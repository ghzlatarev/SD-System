using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD.Data.Models.Identity;

namespace SD.Data.Context.Configurations.Identity
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}
