using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UsersApi.Domain.Models;
using UsersApp.Domain.Models;

namespace UsersApi.Infrastructure.Data
{
    public class UsersDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserState> UserStates { get; set; }
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>().HasData(
                new UserGroup { Id = 1, Code = UserGroupCode.Admin, Description = "Administrator" },
                new UserGroup { Id = 2, Code = UserGroupCode.User, Description = "Regular user" });

            modelBuilder.Entity<UserState>().HasData(
                new UserState { Id = 1, Code = UserStateCode.Active, Description = "Active user" },
                new UserState { Id = 2, Code = UserStateCode.Blocked, Description = "Blocked user" });

            modelBuilder.Entity<UserGroup>().Property(e => e.Code)
                .HasConversion(new EnumToStringConverter<UserGroupCode>());
            modelBuilder.Entity<UserState>().Property(e => e.Code)
                .HasConversion(new EnumToStringConverter<UserStateCode>());

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserGroup)
                .WithMany()
                .HasForeignKey(u => u.UserGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserState)
                .WithMany()
                .HasForeignKey(u => u.UserStateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
