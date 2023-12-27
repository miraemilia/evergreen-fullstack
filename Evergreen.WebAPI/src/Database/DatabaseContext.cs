using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
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
            Delimiter = ";",
            HeaderValidated = null,
            MissingFieldFound = null,        
        };
        using (var csv = new CsvReader(reader, config))
        {
            //csv.Context.TypeConverterCache.AddConverter<byte[]>(new CustomByteArrayConverter());
            //csv.Context.RegisterClassMap<UserMap>();
            var records = csv.GetRecords<T>();
            return records.ToList();
        }        

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
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

        modelBuilder.Entity<OrderProduct>().HasKey(e => new { e.OrderId, e.ProductId });
        //modelBuilder.Entity<ImageProduct>().HasKey(ip => new { ip.ProductId, ip.ImageId });

        modelBuilder.Entity<Product>().ToTable(p => p.HasCheckConstraint("CHK_Product_Price_Positive", "price >= 0"));
        modelBuilder.Entity<OrderProduct>().ToTable(p => p.HasCheckConstraint("CHK_OrderProduct_Quantity_Positive", "quantity >= 0"));
        modelBuilder.Entity<Product>().ToTable(p => p.HasCheckConstraint("CHK_Product_Inventory_Positive", "inventory >= 0"));

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        //var categories = ReadCsvFile<Category>("../Seed/categories.csv");
        //var images = ReadCsvFile<Image>("../Seed/images.csv");
        //var products = ReadCsvFile<Product>("../Seed/products.csv");
        //var image_product = ReadCsvFile<ImageProduct>("../Seed/image_product.csv");
        //var product_details = ReadCsvFile<ProductDetails>("../Seed/product_details.csv");
        //var users = ReadCsvFile<User>("../Seed/users.csv");
        //var orders = ReadCsvFile<Order>("../Seed/orders.csv");
        //var orders_products = ReadCsvFile<OrderProduct>("../Seed/orders_products.csv");

        //modelBuilder.Entity<Category>().HasData(categories);
        //modelBuilder.Entity<Image>().HasData(images);
        //modelBuilder.Entity<Product>().HasData(products);
        //modelBuilder.Entity<ImageProduct>().HasData(image_product);
        //modelBuilder.Entity<ProductDetails>().HasData(product_details);
        //modelBuilder.Entity<User>().HasData(users);
        //modelBuilder.Entity<Order>().HasData(orders);
        //modelBuilder.Entity<OrderProduct>().HasData(orders_products);
    }
}

/* public class CustomByteArrayConverter : ByteArrayConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text.StartsWith("\\x"))
        {
            text = text.Substring(2);
        }
        return base.ConvertFromString(text, row, memberMapData);
    }
} */