using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Areas.Admin.Models;
using Shop.Models;

namespace Shop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<StaffFunctionAuthority>()
                .HasKey(x => new { x.IdStaff, x.IdFunction });
            builder.Entity<StaffFunctionAuthority>()
                .HasOne(fa => fa.Function)
                .WithMany(fa => fa.Staff)
                .HasForeignKey(fa => fa.IdFunction);
            builder.Entity<StaffFunctionAuthority>()
                .HasOne(fa => fa.Staff)
                .WithMany(fa => fa.Fuction)
                .HasForeignKey(fa => fa.IdStaff);

        }
        public DbSet<Shop.Models.Products> Products { get; set; }
        public DbSet<Shop.Models.Posts> Posts { get; set; }
        public DbSet<Shop.Models.PostTypes> PostTypes { get; set; }
        public DbSet<Shop.Models.ProductTypes> ProductTypes { get; set; }
        public DbSet<Shop.Areas.Admin.Models.Staffs> Staffs { get; set; }
        public DbSet<Shop.Areas.Admin.Models.Functions> Functions { get; set; }
        public DbSet<Shop.Areas.Admin.Models.StaffFunctionAuthority> StaffFunctionAuthority { get; set; }
    }
}