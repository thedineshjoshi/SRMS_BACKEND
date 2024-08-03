using Application.Common.Interfaces;
using Domain;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>, IApplicationDbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<StudentRegistration> StudentRegistrations => Set<StudentRegistration>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.UserClaims)
                .WithOne()
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.UserRoles)
                .WithOne()
                .HasForeignKey(r => r.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationRole>()
                .HasMany(r => r.RoleClaims)
                .WithOne()
                .HasForeignKey(c => c.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationRole>()
                .HasMany(r => r.UserRoles)
                .WithOne()
                .HasForeignKey(r => r.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            //Roles Seeding in my Database;
            var AdminRoleId = Guid.NewGuid();
            builder.Entity<ApplicationRole>().HasData(
                    new ApplicationRole {
                        
                        Id = AdminRoleId,
                        Name = "Admin", 
                        Description = "Can Manage Everything",
                        NormalizedName="ADMIN", 
                        ConcurrencyStamp=Guid.NewGuid().ToString() 
                    
                    }
                );

            //User_Seeding based on the above Role
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(), 
                UserName = "Dinesh25",
                NormalizedUserName = "DINESH25",
                Email = "dineshjoshi0025@gmail.com",
                NormalizedEmail = "DINESHJOSHI0025@GMAIL.COM",
                EmailConfirmed = true,
                IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()

            };
            user.PasswordHash = passwordHasher.HashPassword(user, "@Dineshdj25"); // Set the password hash

            // Seeding users
            builder.Entity<ApplicationUser>().HasData(user);

            // Seeding the user-role relationship
            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    UserId = user.Id,
                    RoleId = AdminRoleId
                }
            );
        }
    }
}
