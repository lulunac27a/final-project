using NLog;
partial class Program
{
    static void Main(string[] args)
    {

        string path = Directory.GetCurrentDirectory() + "//nlog.config";

        // create instance of Logger
        var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

        logger.Info("Program started");

        do
        {
            Console.WriteLine("Enter your choice:");
            Console.WriteLine("1. Add product");
            Console.WriteLine("2. Edit product");
            Console.WriteLine("3. Display Products");
            Console.WriteLine("4. Display a Specific Product");
            Console.WriteLine("5. Add Category");
            Console.WriteLine("6. Edit Category");
            Console.WriteLine("7. Display All Categories");
            Console.WriteLine("8. Display Categories with Names");
            string? choice = Console.ReadLine();
            var db = new DataContext();
            switch (choice)
            {
                case "1":

                    Product? product = InputProduct(db, logger);
                    if (product != null)
                        try
                        {
                            db.AddProduct(product);
                            logger.Info("Product added successfully");
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex, "Error adding product");
                        }
                    else
                    {
                        logger.Error("Product is null");
                    }
                    logger.Info("Product added successfully");
                    break;
                case "2":
                    var products = db.Products.OrderBy(p => p.ProductId).ToList();
                    if (products != null)
                    {
                        foreach (Product p in products)
                        {
                            Console.WriteLine($"{p.ProductId}. {p.ProductName}");
                        }
                    }
                    Product? foundProduct = null;
                    if (int.TryParse(Console.ReadLine(), out var ProductId))
                    {
                        foundProduct = db.Products.Find(ProductId);
                    }
                    if (foundProduct != null)
                    {
                        Product? UpdatedProduct = InputProduct(db, logger);
                        if (UpdatedProduct != null)
                        {
                            UpdatedProduct.ProductId = foundProduct.ProductId;
                            string? productName = Console.ReadLine();
                            UpdatedProduct.ProductName = productName;
                            db.EditProduct(UpdatedProduct);
                            logger.Info("Product edited successfully");
                        }
                    }
                    else
                    {
                        logger.Error("Product not found");
                    }
                    break;
                case "3":
                    Console.WriteLine("1. Display Discontinued Products");
                    Console.WriteLine("2. Display Active Products");
                    Console.WriteLine("3. Display All Products");
                    string? discontinued = Console.ReadLine();
                    switch (discontinued)
                    {
                        case "1":
                            foreach (Product currentProduct in db.Products)
                            {
                                if (currentProduct.Discontinued) Console.WriteLine(currentProduct.ProductName);
                            }
                            break;
                        case "2":
                            foreach (Product currentProduct in db.Products)
                            {
                                if (!currentProduct.Discontinued) Console.WriteLine(currentProduct.ProductName);
                            }
                            break;
                        case "3":

                            foreach (Product currentProduct in db.Products)
                            {
                                Console.WriteLine(currentProduct.ProductName);
                            }
                            break;
                        default:
                            logger.Error("Invalid choice");
                            break;
                    }
                    break;
                case "4":
                    Product? foundAProduct = null;
                    if (int.TryParse(Console.ReadLine(), out var enteredProductId))
                    {
                        foundAProduct = db.Products.Find(enteredProductId);
                    }
                    if (foundAProduct != null)
                    {
                        logger.Info("Product found");
                        Console.WriteLine($"Product ID: {foundAProduct.ProductId}, Product Name: {foundAProduct.ProductName}, Supplier ID: {foundAProduct.SupplierId}, Category ID: {foundAProduct.CategoryId}, Quantity Per Unit: {foundAProduct.QuantityPerUnit}, Unit Price: {foundAProduct.UnitPrice}, Units In Stock: {foundAProduct.UnitsInStock}, Units On Order: {foundAProduct.UnitsOnOrder}, Reorder Level: {foundAProduct.ReorderLevel}, Discontinued: {foundAProduct.Discontinued}");
                    }
                    else
                    {
                        logger.Error("Product not found");
                    }
                    break;
                case "5":
                    Category category = new();
                    Console.WriteLine("Enter category name:");
                    category.CategoryName = Console.ReadLine();
                    db.AddCategory(category);
                    logger.Info("Category added successfully");
                    break;
                case "6":
                    var categories = db.Categories.OrderBy(c => c.CategoryId).ToList();
                    if (categories != null)
                    {
                        foreach (Category c in categories)
                        {
                            Console.WriteLine($"{c.CategoryId}. {c.CategoryName}");
                        }
                    }
                    Category? foundCategory = null;
                    if (int.TryParse(Console.ReadLine(), out var CategoryId))
                    {
                        foundCategory = db.Categories.Find(CategoryId);
                    }
                    if (foundCategory != null)
                    {
                        Category? UpdatedCategory = new();
                        Console.WriteLine("Enter category name:");
                        UpdatedCategory.CategoryName = Console.ReadLine();
                        UpdatedCategory.CategoryId = foundCategory.CategoryId;
                        db.EditCategory(UpdatedCategory);
                        logger.Info("Category edited successfully");
                    }
                    break;
                case "7":
                    foreach (Category currentCategory in db.Categories)
                    {
                        Console.WriteLine($"{currentCategory.CategoryId}. Name: {currentCategory.CategoryName}, Description: {currentCategory.Description}");
                    }
                    break;
                case "8":
                    foreach (Category currentCategory in db.Categories)
                    {
                        List<Product> productsList = db.Products.Where(p => p.CategoryId == currentCategory.CategoryId).ToList();
                        if (productsList.Count > 0)
                        {
                            foreach (Product productList in productsList)
                            {
                                Console.WriteLine($"{productList.ProductId}. Name: {productList.ProductName}, Category: {currentCategory.CategoryName}");
                            }
                        }
                    }
                    break;
                default:
                    logger.Info("Invalid choice. Program ended");
                    break;
            }
        } while (true);
        static Product? InputProduct(DataContext db, NLog.Logger logger)
        {
            Product product = new();
            Console.WriteLine("Enter product name:");
            product.ProductName = Console.ReadLine();
            return product;
        }

    }
}