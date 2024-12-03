using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

do
{
    Console.WriteLine("Enter your choice:");
    Console.WriteLine("1. Add product");
    string? choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            var db = new DataContext();
            Product? product = InputProduct(db, logger);
            db.AddProduct(product);
            logger.Info("Product added successfully");
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

