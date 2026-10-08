using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories;
using TradingCompany.DAL.Repositories.Repository;

namespace TradingCompany.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var optionsBuilder = new DbContextOptionsBuilder<TradingCompanyDbContext>();
            optionsBuilder.UseSqlServer(@"Server=(local);Database=TradingCompanyDB;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;");

            using var context = new TradingCompanyDbContext(optionsBuilder.Options);

            var categoryRepo = new CategoryRepository(context);
            var productRepo = new ProductRepository(context);
            var supplierRepo = new SupplierRepository(context);
            var saleRepo = new SaleRepository(context);

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== ГОЛОВНЕ МЕНЮ: УПРАВЛІННЯ БАЗОЮ ДАНИХ ===");
                Console.WriteLine("1. Категорії (Categories)");
                Console.WriteLine("2. Товари (Products)");
                Console.WriteLine("3. Постачальники (Suppliers)");
                Console.WriteLine("4. Продажі (Sales)");
                Console.WriteLine("0. Вихід");
                Console.Write("\nОберіть таблицю для роботи: ");

                var tableChoice = Console.ReadLine();

                if (tableChoice == "0")
                {
                    exit = true;
                    Console.WriteLine("\nЗавершення роботи...");
                    continue;
                }

                switch (tableChoice)
                {
                    case "1":
                        await ManageCategories(categoryRepo);
                        break;
                    case "2":
                        await ManageProducts(productRepo, categoryRepo, supplierRepo);
                        break;
                    case "3":
                        await ManageSuppliers(supplierRepo);
                        break;
                    case "4":
                        await ManageSales(saleRepo, productRepo);
                        break;
                    default:
                        Console.WriteLine("\nНекоректний вибір. Натисніть клавішу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        //CATEGORIES
        private static async Task ManageCategories(CategoryRepository repo)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== ТАБЛИЦЯ: Categories ===");
                Console.WriteLine("1. Створити категорію");
                Console.WriteLine("2. Показати всі категорії");
                Console.WriteLine("3. Оновити категорію");
                Console.WriteLine("4. Видалити категорію");
                Console.WriteLine("0. Назад");
                Console.Write("\nОберіть дію: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Назва категорії (CategoryName): ");
                        string name = Console.ReadLine() ?? "";
                        Console.Write("Опис (Description): ");
                        string desc = Console.ReadLine() ?? "";

                        var cat = new Category { CategoryName = name, Description = desc };
                        await repo.AddAsync(cat);
                        Console.WriteLine($"\n[УСПІХ] Категорію успішно додано!");
                        break;

                    case "2":
                        var list = await repo.GetAllAsync();
                        Console.WriteLine("\n--- Список категорій ---");
                        foreach (var c in list)
                            Console.WriteLine($"ID: {c.CategoryId} | Назва: {c.CategoryName} | Опис: {c.Description}");
                        break;

                    case "3":
                        var currentList = await repo.GetAllAsync();
                        Console.WriteLine("\n--- Наявні категорії ---");
                        foreach (var c in currentList)
                            Console.WriteLine($"ID: {c.CategoryId} | Назва: {c.CategoryName}");

                        Console.Write("\nВведіть ID категорії, яку хочете оновити: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                        {
                            var item = await repo.GetByIdAsync(id);
                            if (item != null)
                            {
                                Console.Write($"Нова назва (поточна '{item.CategoryName}'): ");
                                string newName = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newName)) item.CategoryName = newName;

                                Console.Write($"Новий опис (поточний '{item.Description}'): ");
                                string newDesc = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newDesc)) item.Description = newDesc;

                                await repo.UpdateAsync(item);
                                Console.WriteLine("\n[УСПІХ] Категорію оновлено!");
                            }
                            else Console.WriteLine("\n[ПОМИЛКА] Категорію з таким ID не знайдено.");
                        }
                        break;

                    case "4":
                        var listForDel = await repo.GetAllAsync();
                        Console.WriteLine("\n--- Наявні категорії ---");
                        foreach (var c in listForDel)
                            Console.WriteLine($"ID: {c.CategoryId} | Назва: {c.CategoryName}");

                        Console.Write("\nВведіть ID категорії для видалення: ");
                        if (int.TryParse(Console.ReadLine(), out int delId))
                        {
                            await repo.DeleteAsync(delId);
                        }
                        break;

                    case "0":
                        back = true;
                        continue;
                }
                if (!back) { Console.WriteLine("\nНатисніть будь-яку клавішу..."); Console.ReadKey(); }
            }
        }

        //PRODUCTS
        private static async Task ManageProducts(ProductRepository productRepo, CategoryRepository categoryRepo, SupplierRepository supplierRepo)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== ТАБЛИЦЯ: Products ===");
                Console.WriteLine("1. Створити товар");
                Console.WriteLine("2. Показати всі товари");
                Console.WriteLine("3. Оновити товар");
                Console.WriteLine("4. Видалити товар");
                Console.WriteLine("0. Назад");
                Console.Write("\nОберіть дію: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        
                        var categories = await categoryRepo.GetAllAsync();
                        Console.WriteLine("\n--- Доступні категорії ---");
                        foreach (var c in categories)
                            Console.WriteLine($"ID: {c.CategoryId} | Назва: {c.CategoryName}");

                        Console.Write("\nВведіть ID категорії: ");
                        int catId = int.Parse(Console.ReadLine() ?? "0");

                       
                        var suppliers = await supplierRepo.GetAllAsync();
                        Console.WriteLine("\n--- Доступні постачальники ---");
                        foreach (var s in suppliers)
                            Console.WriteLine($"ID: {s.SupplierId} | Компанія: {s.CompanyName}");

                        Console.Write("\nВведіть ID постачальника: ");
                        int suppId = int.Parse(Console.ReadLine() ?? "0");

                        
                        Console.Write("SKU: ");
                        string sku = Console.ReadLine() ?? "";
                        Console.Write("ProductName: ");
                        string pName = Console.ReadLine() ?? "";
                        Console.Write("UnitPrice (ціна): ");
                        decimal price = decimal.Parse(Console.ReadLine() ?? "0");
                        Console.Write("StockQuantity (кількість на складі): ");
                        int stock = int.Parse(Console.ReadLine() ?? "0");

                        var product = new Product
                        {
                            CategoryId = catId,
                            SupplierId = suppId,
                            SKU = sku,
                            ProductName = pName,
                            UnitPrice = price,
                            StockQuantity = stock
                        };
                        await productRepo.AddAsync(product);
                        Console.WriteLine($"\n[УСПІХ] Товар успішно додано!");
                        break;

                    case "2":
                        var list = await productRepo.GetAllAsync();
                        Console.WriteLine("\n--- Список товарів ---");
                        foreach (var p in list)
                            Console.WriteLine($"ID: {p.ProductId} | SKU: {p.SKU} | Назва: {p.ProductName} | Ціна: {p.UnitPrice} | Кількість: {p.StockQuantity}");
                        break;

                    case "3":
                        var currentList = await productRepo.GetAllAsync();
                        Console.WriteLine("\n--- Наявні товари ---");
                        foreach (var p in currentList)
                            Console.WriteLine($"ID: {p.ProductId} | Назва: {p.ProductName}");

                        Console.Write("\nВведіть ID товару, який хочете оновити: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                            {
                            var item = await productRepo.GetByIdAsync(id);
                            if (item != null)
                            {
                                Console.Write($"Нова назва (поточна '{item.ProductName}'): ");
                                string newName = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newName)) item.ProductName = newName;

                                Console.Write($"Нова ціна (поточна '{item.UnitPrice}'): ");
                                string priceInput = Console.ReadLine();
                                if (decimal.TryParse(priceInput, out decimal newPrice)) item.UnitPrice = newPrice;

                                await productRepo.UpdateAsync(item);
                                Console.WriteLine("\n[УСПІХ] Товар оновлено!");
                            }
                            else Console.WriteLine("\n[ПОМИЛКА] Товар з таким ID не знайдено.");
                        }
                        break;

                    case "4":
                        var listForDel = await productRepo.GetAllAsync();
                        Console.WriteLine("\n--- Наявні товари ---");
                        foreach (var p in listForDel)
                            Console.WriteLine($"ID: {p.ProductId} | Назва: {p.ProductName}");

                        Console.Write("\nВведіть ID товару для видалення: ");
                        if (int.TryParse(Console.ReadLine(), out int delId))
                        {
                            bool success = await productRepo.DeleteAsync(delId);

                            if (success)
                            {
                                Console.WriteLine("\n[УСПІХ] Товар видалено!");
                            }
                            else
                            {
                                Console.WriteLine("\n[ПОМИЛКА] Товар із таким ID не знайдено.");
                            }
                        }
                        break;

                    case "0":
                        back = true;
                        continue;
                }
                if (!back) { Console.WriteLine("\nНатисніть будь-яку клавішу..."); Console.ReadKey(); }
            }
        }
        //SUPPLIERS
        private static async Task ManageSuppliers(SupplierRepository repo)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== ТАБЛИЦЯ: Suppliers ===");
                Console.WriteLine("1. Створити постачальника");
                Console.WriteLine("2. Показати всіх постачальників");
                Console.WriteLine("3. Оновити постачальника");
                Console.WriteLine("4. Видалити постачальника");
                Console.WriteLine("0. Назад");
                Console.Write("\nОберіть дію: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("CompanyName: ");
                        string compName = Console.ReadLine() ?? "";
                        Console.Write("ContactPhone: ");
                        string phone = Console.ReadLine() ?? "";
                        Console.Write("Email: ");
                        string email = Console.ReadLine() ?? "";
                        Console.Write("Address: ");
                        string addr = Console.ReadLine() ?? "";
                        Console.Write("City: ");
                        string city = Console.ReadLine() ?? "";

                        var supplier = new Supplier
                        {
                            CompanyName = compName,
                            ContactPhone = phone,
                            Email = email,
                            Address = addr,
                            City = city
                        };
                        await repo.AddAsync(supplier);
                        Console.WriteLine($"\n[УСПІХ] Постачальника успішно додано!");
                        break;

                    case "2":
                        var list = await repo.GetAllAsync();
                        Console.WriteLine("\n--- Список постачальників ---");
                        foreach (var s in list)
                            Console.WriteLine($"ID: {s.SupplierId} | Компанія: {s.CompanyName} | Місто: {s.City} | Телефон: {s.ContactPhone} | Email: {s.Email}");
                        break;

                    case "3":
                        
                        var currentList = await repo.GetAllAsync();
                        Console.WriteLine("\n--- Наявні постачальники ---");
                        foreach (var s in currentList)
                            Console.WriteLine($"ID: {s.SupplierId} | Компанія: {s.CompanyName} | Місто: {s.City}");

                        Console.Write("\nВведіть ID постачальника, якого хочете оновити: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                        {
                            var item = await repo.GetByIdAsync(id);
                            if (item != null)
                            {
                                Console.Write($"Нова назва компанії (поточна '{item.CompanyName}'): ");
                                string newCompName = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newCompName)) item.CompanyName = newCompName;

                                Console.Write($"Новий телефон (поточний '{item.ContactPhone}'): ");
                                string newPhone = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newPhone)) item.ContactPhone = newPhone;

                                Console.Write($"Новий email (поточний '{item.Email}'): ");
                                string newEmail = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newEmail)) item.Email = newEmail;

                                Console.Write($"Нова адреса (поточна '{item.Address}'): ");
                                string newAddr = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newAddr)) item.Address = newAddr;

                                Console.Write($"Нове місто (поточне '{item.City}'): ");
                                string newCity = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newCity)) item.City = newCity;

                                await repo.UpdateAsync(item);
                                Console.WriteLine("\n[УСПІХ] Постачальника оновлено!");
                            }
                            else Console.WriteLine("\n[ПОМИЛКА] Постачальника з таким ID не знайдено.");
                        }
                        break;

                    case "4":
                        var listForDel = await repo.GetAllAsync();
                        Console.WriteLine("\n--- Наявні постачальники ---");
                        foreach (var s in listForDel)
                            Console.WriteLine($"ID: {s.SupplierId} | Компанія: {s.CompanyName}");

                        Console.Write("\nВведіть ID постачальника для видалення: ");
                        if (int.TryParse(Console.ReadLine(), out int delId))
                        {
                            bool success = await repo.DeleteAsync(delId);

                            if (success)
                            {
                                Console.WriteLine("\n[УСПІХ] Постачальника видалено!");
                            }
                            else
                            {
                                Console.WriteLine("\n[ПОМИЛКА] Постачальника із таким ID не знайдено.");
                            }
                        }
                        break;

                    case "0":
                        back = true;
                        continue;
                }
                if (!back) { Console.WriteLine("\nНатисніть будь-яку клавішу..."); Console.ReadKey(); }
            }
        }
        //SALES
        private static async Task ManageSales(SaleRepository saleRepo, ProductRepository productRepo)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== ТАБЛИЦЯ: Sales ===");
                Console.WriteLine("1. Зареєструвати продаж");
                Console.WriteLine("2. Показати всі продажі");
                Console.WriteLine("3. Оновити продаж");
                Console.WriteLine("4. Видалити продаж");
                Console.WriteLine("0. Назад");
                Console.Write("\nОберіть дію: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        var products = await productRepo.GetAllAsync();
                        Console.WriteLine("\n--- Доступні товари ---");
                        foreach (var p in products)
                            Console.WriteLine($"ID: {p.ProductId} | Назва: {p.ProductName} | Ціна: {p.UnitPrice}");

                        Console.Write("\nВведіть ProductId: ");
                        if (!int.TryParse(Console.ReadLine(), out int prodId))
                        {
                            Console.WriteLine("[ПОМИЛКА] Невірний формат ID.");
                            break;
                        }

                        
                        var product = await productRepo.GetByIdAsync(prodId);
                        if (product == null)
                        {
                            Console.WriteLine("[ПОМИЛКА] Товар із таким ID не знайдено.");
                            break;
                        }

                        Console.Write("Quantity (кількість): ");
                        if (!int.TryParse(Console.ReadLine(), out int qty))
                        {
                            Console.WriteLine("[ПОМИЛКА] Невірна кількість.");
                            break;
                        }

                        Console.Write("PaymentMethod (готівка/картка тощо): ");
                        string payMethod = Console.ReadLine() ?? "";

                        Console.Write("Status (статус): ");
                        string status = Console.ReadLine() ?? "";

                        var sale = new Sale
                        {
                            ProductId = prodId,
                            Quantity = qty,
                            SalePrice = product.UnitPrice, 
                            SaleDate = DateTime.Now,
                            PaymentMethod = payMethod,
                            Status = status
                        };

                        await saleRepo.AddAsync(sale);
                        Console.WriteLine($"\n[УСПІХ] Продаж успішно зареєстровано за ціною {product.UnitPrice} грн!");
                        break;

                    case "2":
                        var list = await saleRepo.GetAllAsync();
                        Console.WriteLine("\n--- Список продажів ---");
                        foreach (var s in list)
                            Console.WriteLine($"ID: {s.SaleId} | Товар ID: {s.ProductId} | Кількість: {s.Quantity} | Сума: {s.TotalSum} | Дата: {s.SaleDate} | Метод: {s.PaymentMethod} | Статус: {s.Status}");
                        break;

                    case "3":
                        var currentList = await saleRepo.GetAllAsync();
                        Console.WriteLine("\n--- Наявні продажі ---");
                        foreach (var s in currentList)
                            Console.WriteLine($"ID: {s.SaleId} | Товар ID: {s.ProductId} | Сума: {s.TotalSum} | Дата: {s.SaleDate}");

                        Console.Write("\nВведіть ID продажу, який хочете оновити: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                        {
                            var item = await saleRepo.GetByIdAsync(id);
                            if (item != null)
                            {
                                Console.Write($"Новий ID товару (поточний '{item.ProductId}'): ");
                                string prodInput = Console.ReadLine();
                                if (int.TryParse(prodInput, out int newProdId)) item.ProductId = newProdId;

                                Console.Write($"Нова кількість (поточна '{item.Quantity}'): ");
                                string qtyInput = Console.ReadLine();
                                if (int.TryParse(qtyInput, out int newQty)) item.Quantity = newQty;

                                Console.Write($"Нова ціна за одиницю (поточна '{item.SalePrice}'): ");
                                string priceInput = Console.ReadLine();
                                if (decimal.TryParse(priceInput, out decimal newPrice)) item.SalePrice = newPrice;

                                Console.Write($"Новий метод оплати (поточний '{item.PaymentMethod}'): ");
                                string newPayment = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newPayment)) item.PaymentMethod = newPayment;

                                Console.Write($"Новий статус (поточний '{item.Status}'): ");
                                string newStatus = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newStatus)) item.Status = newStatus;

                                await saleRepo.UpdateAsync(item);
                                Console.WriteLine("\n[УСПІХ] Продаж оновлено!");
                            }
                            else Console.WriteLine("\n[ПОМИЛКА] Продаж з таким ID не знайдено.");
                        }
                        break;

                    case "4":
                        var listForDel = await saleRepo.GetAllAsync();
                        Console.WriteLine("\n--- Наявні продажі ---");
                        foreach (var s in listForDel)
                            Console.WriteLine($"ID: {s.SaleId} | Товар ID: {s.ProductId} | Сума: {s.TotalSum}");

                        Console.Write("\nВведіть ID продажу для видалення: ");
                        if (int.TryParse(Console.ReadLine(), out int delId))
                        {
                            bool success = await saleRepo.DeleteAsync(delId);

                            if (success)
                            {
                                Console.WriteLine("\n[УСПІХ] Продаж видалено!");
                            }
                            else
                            {
                                Console.WriteLine("\n[ПОМИЛКА] Продаж із таким ID не знайдено.");
                            }
                        }
                        break;

                    case "0":
                        back = true;
                        continue;
                }
                if (!back) { Console.WriteLine("\nНатисніть будь-яку клавішу..."); Console.ReadKey(); }
            }
        }
    }
}