using Core.App.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Firstname).IsRequired().HasMaxLength(150);
        builder.Property(u => u.Lastname).HasMaxLength(150);
        builder.Property(u => u.Email).HasMaxLength(150);
        builder.Property(u => u.UserName).HasMaxLength(150);
        
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.Property(e => e.UserName).IsRequired().HasMaxLength(100);
        
    }
}

public class TokenConfiguration : IEntityTypeConfiguration<AuthToken>
{
    public void Configure(EntityTypeBuilder<AuthToken> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.LoginToken).IsRequired().HasMaxLength(500);
        builder.Property(t => t.VerifyToken).HasMaxLength(500);
        
        builder.HasOne<AppUser>()
            .WithOne()
            .HasForeignKey<AuthToken>(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}