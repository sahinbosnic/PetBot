using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PetBot
{
    class DataController
    {
        public DataController()
        {
            //Console.WriteLine("Connecting to database...");
        }

        public string GetSetting(string property)
        {
            string value;
            using (var db = new DataContext())
            {
                value = db.Settings.Where(x => x.Property == property).Select(x => x.Value).FirstOrDefault();
            }

            return value;
        }

        public void AddSetting(string property, string value)
        {
            using (var db = new DataContext())
            {
                db.Settings.Add(new Settings { Property = property, Value = value});
                db.SaveChanges();
            }
        }

        public void UpdateSetting(string property, string value)
        {
            using (var db = new DataContext())
            {
                Settings set = (from x in db.Settings
                    where x.Property == property
                    select x).First();
                    
                set.Value = value;
                db.SaveChanges();
            }
        }
    }
}
