﻿using System;
using NLog.Web;
using System.IO;

namespace NorthwindConsole
{
    class Program
    {
        private static NLog.Logger logger = NLOgBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");
            
            Console.WriteLine("Hello World!");

            logger.Info("Program ended");
        }
    }
}
