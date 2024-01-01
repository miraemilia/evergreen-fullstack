using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;
using Microsoft.EntityFrameworkCore;

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
        };
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.TypeConverterCache.AddConverter<byte[]>(new CustomByteConverter());
            csv.Context.RegisterClassMap(new ProductMap());
            csv.Context.RegisterClassMap(new ProductImageMap());
            csv.Context.RegisterClassMap(new ProductDetailsMap());
            csv.Context.RegisterClassMap(new OrderMap());
            csv.Context.RegisterClassMap(new OrderDetailsMap());
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

        modelBuilder.Entity<Product>().ToTable(p => p.HasCheckConstraint("CHK_Product_Price_Positive", "price >= 0"));
        modelBuilder.Entity<OrderProduct>().ToTable(p => p.HasCheckConstraint("CHK_OrderProduct_Quantity_Positive", "quantity >= 0"));
        modelBuilder.Entity<Product>().ToTable(p => p.HasCheckConstraint("CHK_Product_Inventory_Positive", "inventory >= 0"));

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        var categories = ReadCsvFile<Category>("../Seed/categories.csv");
        var products = ReadCsvFile<Product>("../Seed/products.csv");
        var images = ReadCsvFile<Image>("../Seed/images.csv");
        var product_details = ReadCsvFile<ProductDetails>("../Seed/product_details.csv");
        var users = ReadCsvFile<User>("../Seed/users.csv");
        var orders = ReadCsvFile<Order>("../Seed/orders.csv");
        var orders_products = ReadCsvFile<OrderProduct>("../Seed/orders_products.csv");

        modelBuilder.Entity<Category>().HasData(categories);
        modelBuilder.Entity<Product>().HasData(products);
        modelBuilder.Entity<Image>().HasData(images);
        modelBuilder.Entity<ProductDetails>().HasData(product_details);
        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<Order>().HasData(orders);
        modelBuilder.Entity<OrderProduct>().HasData(orders_products);

        base.OnModelCreating(modelBuilder);
    }
}

public sealed class ProductMap : ClassMap<Product>
{
    public ProductMap()
    {
        Map(p => p.Id);
        Map(p => p.Title);
        Map(p => p.LatinName);
        Map(p => p.Price);
        Map(p => p.Description);
        Map(p => p.CategoryId);
        Map(p => p.Inventory);
        Map(p => p.CreatedAt);
        Map(p => p.UpdatedAt);
    }
}

public sealed class ProductImageMap : ClassMap<Image>
{
    public ProductImageMap()
    {
        Map(p => p.Id);
        Map(p => p.ProductId);
        Map(p => p.ImageUrl);
        Map(p => p.Description);
        Map(p => p.CreatedAt);
        Map(p => p.UpdatedAt);
    }
}

public sealed class ProductDetailsMap : ClassMap<ProductDetails>
{
    public ProductDetailsMap()
    {
        Map(p => p.Id);
        Map(p => p.ProductId);
        Map(p => p.Size);
        Map(p => p.Watering);
        Map(p => p.Light);
        Map(p => p.Difficulty);
        Map(p => p.Hanging);
        Map(p => p.NonToxic);
        Map(p => p.AirPurifying);
        Map(p => p.CreatedAt);
        Map(p => p.UpdatedAt);
    }
}

public sealed class OrderMap : ClassMap<Order>
{
    public OrderMap()
    {
        Map(p => p.Id);
        Map(p => p.UserId);
        Map(p => p.OrderStatus);
        Map(p => p.CreatedAt);
        Map(p => p.UpdatedAt);
    }
}

public sealed class OrderDetailsMap : ClassMap<OrderProduct>
{
    public OrderDetailsMap()
    {
        Map(p => p.OrderId);
        Map(p => p.ProductId);
        Map(p => p.Quantity);
    }
}

public class CustomByteConverter : ByteConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text.StartsWith("\\x"))
        {
            text = text.Substring(2);
            byte[] saltBytes = StringToByteArray(text);
            return saltBytes;
        }
        return base.ConvertFromString(text, row, memberMapData);
    }
    private static byte[] StringToByteArray(string hex)
    {
        int numberChars = hex.Length;
        byte[] bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }
}