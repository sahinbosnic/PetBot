using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PetBot
{
    class Engine
    {
        private int EngineA1;
        private int EngineA2;
        private int EngineB1;
        private int EngineB2;

        private int topSpeed;
        private int currentSpeed;

        public Engine()
        {
            ReadConfig(); //Initialize by getting data from config file
        }

        public void ReadConfig()
        {
            if (File.Exists("Engine.config"))
            {
                //Read from config file
                string[] lines = File.ReadAllLines("Engine.config");
                foreach (var line in lines)
                {
                    string[] newLine = line.Split(":");
                    if(newLine[0] == "Engine A 1" || newLine[1].Length > 0)
                    {
                        EngineA1 = int.Parse(newLine[1]);
                    }
                    else if (newLine[0] == "Engine A 2" || newLine[1].Length > 0)
                    {
                        EngineA2 = int.Parse(newLine[1]);
                    }
                    else if (newLine[0] == "Engine B 1" || newLine[1].Length > 0)
                    {
                        EngineB2 = int.Parse(newLine[1]);
                    }
                    else if (newLine[0] == "Engine B 2" || newLine[1].Length > 0)
                    {
                        EngineB2 = int.Parse(newLine[1]);
                    }
                    else
                    {
                        Console.WriteLine(string.Format("{0} has not been set, please edit the file and update config", newLine[0]));
                    }
                }

                Console.WriteLine("Data has been loaded from Engine.config");
            }
            else
            {
                //Create config file
                string config =
                    "Engine A 1: \n" +
                    "Engine A 2: \n" +
                    "Engine B 1: \n" +
                    "Engine B 2: ";

                File.WriteAllText("Engine.config", config);
                Console.WriteLine("Config file 'Engine.config' was created, please edit it!");
            }
        }

        public void EditConfig()
        {
            if (File.Exists("Engine.config"))
            {
                //Read from config file
                string[] lines = File.ReadAllLines("Engine.config");
                string newConfig = "";

                foreach (var line in lines)
                {
                    string[] newLine;
                    newLine = line.Split(":");
                    Console.WriteLine("Would you like to edit Y/N:");
                    Console.WriteLine(newLine[0] + " With the value: " + newLine[1] + " Y/N");
                    string choice = Console.ReadLine();
                    if (choice == "y" || choice == "Y")
                    {
                        Console.WriteLine("Enter new value:");
                        string newVal = Console.ReadLine();
                        newConfig += string.Format("{0}: {1}", newLine[0], newVal);
                    }
                    else
                    {
                        newConfig += string.Format("{0}: {1}", newLine[0], newLine[1]);
                    }
                    newConfig += "\n";
                }
                File.WriteAllText("Engine.config", newConfig);

            }
            else
            {
                Console.WriteLine("Engine.config not found, atempt to create new file...");
                ReadConfig();
            }
        }


        public void ViewConfig()
        {
            Console.WriteLine(File.ReadAllText("Engine.config"));
        }


        public void Controller()
        {

        }


        public void MoveForward(int lSpeed, int rSpeed)
        {

        }

        public void MoveReverse(int lSpeed, int rSpeed)
        {

        }

        public void Stop()
        {

        }
    }
}
