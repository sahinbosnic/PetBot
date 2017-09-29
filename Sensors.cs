using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace PetBot
{
    public class Sensors
    {
        public int ObstacleFR {get; set;}
        public int ObstacleFL {get; set;}

        public Sensors()
        {
            Console.WriteLine("");
            ReadConfig();
        }

        public void ReadConfig()
        {
            DataController db = new DataController();
            try
            {
                ObstacleFR = int.Parse(db.GetSetting("ObstacleFR"));
                ObstacleFL = int.Parse(db.GetSetting("ObstacleFL"));
                ViewConfig();
            }
            catch
            {
                Console.WriteLine("Sensor config not found, atempting to insert..");
                db.AddSetting("ObstacleFR", "0");
                db.AddSetting("ObstacleFL", "0");
                Console.WriteLine("Sensor config created!");
                Console.WriteLine("Please edit engine config before attempting to continue");
                
                EditConfig();
            }
        }

        public void EditConfig()
        {
            DataController db = new DataController();

            Console.WriteLine("Would you like to edit (Y/N): ObstacleFR - {0}", db.GetSetting("ObstacleFR"));
            if(Console.ReadLine() == "y" || Console.ReadLine() == "Y")
            {
                Console.WriteLine("Enter new value for ObstacleFR");
                string newVal = Console.ReadLine();
                db.UpdateSetting("ObstacleFR", newVal);
            }

            Console.WriteLine("Would you like to edit (Y/N): ObstacleFL - {0}", db.GetSetting("ObstacleFL"));
            if(Console.ReadLine() == "y" || Console.ReadLine() == "Y")
            {
                Console.WriteLine("Enter new value for ObstacleFL");
                string newVal = Console.ReadLine();
                db.UpdateSetting("ObstacleFL", newVal);
            }

            ReadConfig();
        }

        public void ViewConfig()
        {
            DataController db = new DataController();

            Console.WriteLine("Getting sensor config..");
            Console.WriteLine("ObstacleFR: " + db.GetSetting("ObstacleFR"));
            Console.WriteLine("ObstacleFL: " + db.GetSetting("ObstacleFL"));
        }

        public void ActivateSensors()
        {
            //Open sensor pins
            GpioCore.Open(ObstacleFR);
            GpioCore.Open(ObstacleFL);

            //Set sensor pin direction as 'in'
            GpioCore.In(ObstacleFR);
            GpioCore.In(ObstacleFL);


            Console.WriteLine("Sensors activated");
        }

        public bool ScanFront()
        {
            if(GpioCore.Read(ObstacleFR) || GpioCore.Read(ObstacleFL))
            {
                return true;
            }

            return false;
        }

        public bool ScanFrontRight()
        {
            if(GpioCore.Read(ObstacleFR))
            {
                //Console.WriteLine($"ObstacleFR detected an obstacle!");
                return true;
            }
            return false;
        }
        public bool ScanFrontLeft()
        {
            if(GpioCore.Read(ObstacleFL))
            {
                //Console.WriteLine($"ObstacleFL detected an obstacle!");
                return true;
            }
            return false;
        }

        
    }
}
