using System;
using NLog.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NorthwindConsole.Model;

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
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info($"Option {choice} selected");
                    if(choice == "1"){
                        var db = new NWConsole_48_TELContext();
                        var query = db.Categories.OrderBy(p => p.CategoryName);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{query.Count()} records returned");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        foreach(var item in query){
                            Console.WriteLine($"{item.CategoryName} - {item.Description}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if(choice == "2"){
                        Category category = new Category();
                        Console.WriteLine("Enter Category Name: ");
                        category.CategoryName = Console.ReadLine();
                        Console.WriteLine("Enter the Category Description: ");
                        category.Description = Console.ReadLine();
                        
                        ValidationContext context = new ValidationContext(category, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(category, context, results, true);
                        if(isValid){
                            var db = new NWConsole_48_TELContext();
                            //check unique name
                            if(db.Categories.Any(c => c.CategoryName == category.CategoryName)){
                                //generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Name exists", new string[] { "CategoryName"}));
                            }
                            else{
                                logger.Info("Validation passed");
                            }
                        }
                        if(!isValid){
                            foreach(var result in results){
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
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
    }
}
