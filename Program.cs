using NLog;
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
    string? choice = Console.ReadLine();
    var db = new DataContext();
    switch (choice)
    {
        case "1":

            Product? product = InputProduct(db, logger);
            db.AddProduct(product);
            logger.Info("Product added successfully");
            break;
        case "2":
            var products = db.Products.OrderBy(p => p.ProductId);
            foreach (Product p in products)
            {
                Console.WriteLine($"{p.ProductId}. {p.ProductName}");
            }
            Product? foundProduct = null;
            if (int.TryParse(Console.ReadLine(), out var ProductId))
            {
                foundProduct = db.Products.Find(ProductId);
            }
            if (foundProduct != null)
            {
                Product UpdatedProduct = InputProduct(db, logger);
                UpdatedProduct.ProductId = foundProduct.ProductId;
                string productName = Console.ReadLine();
                UpdatedProduct.ProductName = productName;
                db.EditProduct(UpdatedProduct);
                logger.Info("Product edited successfully");
            }
            break;
        case "3":
            Console.WriteLine("1. Display Discontinued Products");
            Console.WriteLine("2. Display Active Products");
            Console.WriteLine("3. Display All Products");
            string discontinued = Console.ReadLine();
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
            }
            break;
        default:
            logger.Info("Program ended");
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

