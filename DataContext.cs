using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;

public class DataContext : DbContext
{
  public DbSet<Product> Products { get; set; }
  public DbSet<Category> Categories { get; set; }

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
    foreach (Category category in this.Categories)
    {
      Console.WriteLine($"{category.CategoryId}. {category.CategoryName}");
    }
    int categoryID = Convert.ToInt32(Console.ReadLine());
    Category? foundCategory = this.Categories.FirstOrDefault(c => c.CategoryId == categoryID);
    if (foundCategory != null)
    {
      product.CategoryId = categoryID;
    }
    else
    {
      logger.Error("Invalid category ID");
      return;
    }
    this.Products.Add(product);
    logger.Info("Product added successfully");

    this.SaveChanges();
  }
  public void AddCategory(Category category)
  {
    this.Categories.Add(category);
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