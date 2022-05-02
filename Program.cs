using System;
using NLog.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NorthwindConsole.Model;
using Microsoft.EntityFrameworkCore;

namespace NorthwindConsole
{
    class Program
    {
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");
            try{
                string choice;
                do{
                    Console.WriteLine("1) Display Categories");
                    Console.WriteLine("2) Add Category");
                    Console.WriteLine("3) Display Category and related products");
                    Console.WriteLine("4) Add Product");
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info($"Option {choice} selected");
                    if(choice == "1"){
                        // display categories
                        var db = new NWConsole_48_TELContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{query.Count()} records returned");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        foreach(var item in query){
                            Console.WriteLine($"{item.CategoryName} - {item.Description}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if(choice == "2"){
                        // add category
                        var db = new NWConsole_48_TELContext();

                        Category category = InputCategory(db);
                        if(category != null){
                            db.AddCategory(category);
                            logger.Info($"Category added - {category.CategoryName}");
                        }
                    }
                    else if(choice == "3"){
                        // display products in category
                        var db = new NWConsole_48_TELContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine("Select the category whose products you want to display: ");
                        Category category = GetCategory(db);
                        if(category != null){
                            Console.ForegroundColor = ConsoleColor.Green;
                            logger.Info($"CategoryId {category.CategoryId} selected");
                            category = db.Categories.Include("Products").FirstOrDefault(c => c.CategoryId == category.CategoryId);
                            logger.Info($"{category.Products.Count()} items returned");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"{category.CategoryName} - {category.Description}");
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            foreach(Product p in category.Products){
                                Console.WriteLine(p.ProductName);
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else if(choice == "4"){
                        // add product
                        var db = new NWConsole_48_TELContext();

                        Product product = InputProduct(db);
                        if(product != null){
                            db.AddProduct(product);
                            logger.Info($"Product added - {product.ProductName}");
                        }
                    }
                    Console.WriteLine();
                } while(choice.ToLower() != "q");
            }
            catch(Exception ex){
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }

        public static Category GetCategory(NWConsole_48_TELContext db){
            // display all categories
            var categories = db.Categories.OrderBy(c => c.CategoryId);
            Console.ForegroundColor = ConsoleColor.Magenta;
            foreach(var item in categories){
                Console.WriteLine($"{item.CategoryId}: {item.CategoryName}");
            }
            Console.ForegroundColor = ConsoleColor.White;
            if(int.TryParse(Console.ReadLine(), out int CategoryId)){
                Console.Clear();
                Category category = db.Categories.FirstOrDefault(c => c.CategoryId == CategoryId);
                if(category != null)
                    return category;
            }
            logger.Error("Invalid CategoryId");
            return null;
        }

        public static Category InputCategory(NWConsole_48_TELContext db){
            Category category = new Category();
            Console.WriteLine("Enter the Category Name: ");
            category.CategoryName = Console.ReadLine();

            ValidationContext context = new ValidationContext(category, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(category, context, results, true);
            if(isValid){
                if(db.Categories.Any(c => c.CategoryName == category.CategoryName)){
                    //generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Name exists", new string[] { "CategoryName"}));
                }
                else{
                    Console.ForegroundColor = ConsoleColor.Green;
                    logger.Info("Validation passed");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Enter the Category Description: ");
                    category.Description = Console.ReadLine();
                    return category;
                }
            }
            if(!isValid){
                foreach(var result in results){
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
            return null;
        }

        public static Supplier GetSupplier(NWConsole_48_TELContext db){
            // display all categories
            var suppliers = db.Suppliers.OrderBy(s => s.SupplierId);
            Console.ForegroundColor = ConsoleColor.Magenta;
            foreach(var item in suppliers){
                Console.WriteLine($"{item.SupplierId}: {item.CompanyName}");
            }
            Console.ForegroundColor = ConsoleColor.White;
            if(int.TryParse(Console.ReadLine(), out int SupplierId)){
                Console.Clear();
                Supplier supplier = db.Suppliers.FirstOrDefault(s => s.SupplierId == SupplierId);
                if(supplier != null)
                    return supplier;
            }
            logger.Error("Invalid SupplierId");
            return null;
        }

        public static Product InputProduct(NWConsole_48_TELContext db){
            Product product = new Product();
            Console.WriteLine("Enter the Product Name: ");
            product.ProductName = Console.ReadLine();
            //generate QuantityPerUnit
            Console.WriteLine("Enter Quantity Per Unit Description: ");
            product.QuantityPerUnit = Console.ReadLine();
            //generate UnitPrice
            Console.WriteLine("Enter Unit Price:($)");
            if(decimal.TryParse(Console.ReadLine(), out decimal UnitPrice)){
                product.UnitPrice = UnitPrice;
            }
            else{
                logger.Error("Invalid Price");
                return null;
            }
            //genereate UnitsInStock
            Console.WriteLine("Enter initial Inventory: ");
            if(short.TryParse(Console.ReadLine(), out short UnitsInStock)){
                product.UnitsInStock = UnitsInStock;
            }
            else{
                logger.Error("Invalid Stock");
                return null;
            }

            ValidationContext context = new ValidationContext(product, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(product, context, results, true);
            if(isValid){
                if(db.Products.Any(p => p.ProductName == product.ProductName)){
                    //generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Name exists", new string[] { "ProductName"}));
                }
                else{
                    Console.ForegroundColor = ConsoleColor.Green;
                    logger.Info("Validation passed");
                    Console.ForegroundColor = ConsoleColor.White;
        
                    //generate category
                    Category category = GetCategory(db);
                    if(category != null){
                        //generate supplier
                        Supplier supplier = GetSupplier(db);
                        if(supplier != null){
                            //assign category and supplier values
                            product.Category = category;
                            product.CategoryId = category.CategoryId;
                            product.Supplier = supplier;
                            product.SupplierId = supplier.SupplierId;
                            return product;
                        }
                    }
                }
            }
            if(!isValid){
                foreach(var result in results){
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
            return null;
        }
    }
}
