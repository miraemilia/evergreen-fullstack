using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
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

    static DatabaseContext()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DatabaseContext(DbContextOptions options, IConfiguration config) : base(options)
    {
        _config = config;
    }

    public List<T> ReadCsvFile<T>(string filePath) where T : class
    {
        using var reader = new StreamReader(filePath);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
            Delimiter = ";",
            HeaderValidated = null,
            MissingFieldFound = null         
        };
        using var csv = new CsvReader(reader, config);
        var records = csv.GetRecords<T>();
        return records.ToList();            

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_config.GetConnectionString("LocalDb"));
        dataSourceBuilder.MapEnum<UserRole>();
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
        modelBuilder.HasPostgresEnum<UserRole>();
        modelBuilder.Entity<User>(entity => entity.Property(e => e.Role).HasColumnType("user_role"));

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

        modelBuilder.Entity<Product>().ToTable(p => p.HasCheckConstraint("CHK_Product_Price_Positive", "price >= 0"));
        modelBuilder.Entity<OrderProduct>().ToTable(p => p.HasCheckConstraint("CHK_OrderProduct_Quantity_Positive", "quantity >= 0"));

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        //modelBuilder.Entity<ImageProduct>().HasKey(ip => new { ip.ProductId, ip.ProductImagesId });

/*         var categories = ReadCsvFile<Category>("../Seed/categories.csv");
        //var products = ReadCsvFile<Product>("../Seed/products.csv");
        var images = ReadCsvFile<Image>("../Seed/images.csv");
        //var image_product = ReadCsvFile<ImageProduct>("../Seed/image_product.csv");
        //var orders = ReadCsvFile<Order>("../Seed/orders.csv");
        //var orders_products = ReadCsvFile<OrderProduct>("../Seed/orders_products.csv");
        //var product_details = ReadCsvFile<ProductDetails>("../Seed/product_details.csv");
        //var users = ReadCsvFile<User>("../Seed/users.csv");


        modelBuilder.Entity<Category>().HasData(categories);
        //modelBuilder.Entity<Product>().HasData(products);
        modelBuilder.Entity<Image>().HasData(images);
        //modelBuilder.Entity<ImageProduct>().HasData(image_product);
        //modelBuilder.Entity<OrderProduct>().HasData(orders_products);
        //modelBuilder.Entity<ProductDetails>().HasData(product_details);
        //modelBuilder.Entity<Order>().HasData(orders);
        //modelBuilder.Entity<User>().HasData(users); */
    }
}

/* public sealed class ProductMap : ClassMap<Product>
{
    public ProductMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(p => p.ProductDetails).Ignore();
        Map(p => p.Category).Ignore();
    }
} */