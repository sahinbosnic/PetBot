using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetBot
{
    class Database
    {
        public Database()
        {
            SQLitePCL.Batteries_V2.Init();
            Console.WriteLine("Connecting to database...");
        }

        public void ReadData()
        {
            using (var db = new PetBotContext())
            {
                db.Settings.Add(new Settings { 
                    Property = "Engine A 1",
                    Value = "26"
                });
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All blogs in database:");
                foreach (var setting in db.Settings)
                {
                    Console.WriteLine(" - {0} : {1}", setting.Property, setting.Value);
                }
            }
        }
    }
}
