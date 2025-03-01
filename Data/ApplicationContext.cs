using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Models;

namespace Pizzeria.Data;

public class ApplicationContext : IdentityDbContext<User>
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ShopCartItem> ShopCartItems { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderDetails> OrderDetails { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().Property(e => e.DateOfPublication).HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<ShopCartItem>().Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<Order>().Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
    }
}