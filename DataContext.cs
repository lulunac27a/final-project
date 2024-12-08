using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;

public class DataContext : DbContext
{
  public DbSet<Product> Products { get; set; }

  private string path;
  private Logger logger;

  public DataContext()
  {
    path = Directory.GetCurrentDirectory() + "//nlog.config";
    var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();
  }

  public void AddProduct(Product product)
  {
    if (product == null)
    {
      logger.Error("Product is null");
      return;
    }
    Console.WriteLine("Enter category ID:");
    Console.WriteLine("1. Beverages");
    Console.WriteLine("2. Condiments");
    Console.WriteLine("3. Confections");
    Console.WriteLine("4. Dairy Products");
    Console.WriteLine("5. Grains");
    Console.WriteLine("6. Meat/Poultry");
    Console.WriteLine("7. Produce");
    Console.WriteLine("8. Seafood");
    int categoryID = Convert.ToInt32(Console.ReadLine());
    product.CategoryId = categoryID;
    if (categoryID < 1 || categoryID > 8)
    {
      logger.Error("Invalid category ID");
      return;
    }
    else
    {
      this.Products.Add(product);
      logger.Info("Product added successfully");
    }
    this.SaveChanges();
  }
  public void DeleteProduct(Product product)
  {
    this.Products.Remove(product);
    this.SaveChanges();
  }
  public void EditProduct(Product UpdatedProduct)
  {
    Product product = Products.Find(UpdatedProduct.ProductId)!;
    product.ProductName = UpdatedProduct.ProductName;
    this.SaveChanges();
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json");

    var config = configuration.Build();
    optionsBuilder.UseSqlServer(@config["Blogs:ConnectionString"]);
  }

}