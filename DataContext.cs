using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class DataContext : DbContext
{
  public DbSet<Product> Products { get; set; }


  public void AddProduct(Product product)
  {
    this.Products.Add(product);
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