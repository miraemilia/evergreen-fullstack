using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Evergreen.WebAPI.src.Database;

public class DatabaseContext : DbContext
{
    private readonly IConfiguration _config;
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductDetails> ProductDetails { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> Orders_Products { get; set; }

    public DatabaseContext(DbContextOptions options, IConfiguration config) : base(options)
    {
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_config.GetConnectionString("LocalDb"));
        dataSourceBuilder.MapEnum<Role>();
        dataSourceBuilder.MapEnum<ProductSize>();
        dataSourceBuilder.MapEnum<DetailsOption>();
        dataSourceBuilder.MapEnum<OrderStatus>();
        var dataSource = dataSourceBuilder.Build();

        optionsBuilder
            .UseNpgsql(dataSource)
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(new TimeStampInterceptor());
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Role>();
        modelBuilder.Entity<User>(entity => entity.Property(e => e.Role).HasColumnType("role"));

        modelBuilder.HasPostgresEnum<DetailsOption>();
        modelBuilder.HasPostgresEnum<ProductSize>();
        modelBuilder.Entity<ProductDetails>(entity => entity.Property(e => e.Size).HasColumnType("product_size"));
        modelBuilder.Entity<ProductDetails>(entity => entity.Property(e => e.Watering).HasColumnType("details_option"));
        modelBuilder.Entity<ProductDetails>(entity => entity.Property(e => e.Light).HasColumnType("details_option"));
        modelBuilder.Entity<ProductDetails>(entity => entity.Property(e => e.Difficulty).HasColumnType("details_option"));

        modelBuilder.HasPostgresEnum<OrderStatus>();
        modelBuilder.Entity<Order>(entity => entity.Property(e => e.OrderStatus).HasColumnType("order_status"));

        modelBuilder.Entity<OrderProduct>()
            .HasKey(e => new { e.OrderId, e.ProductId });

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
    }
}