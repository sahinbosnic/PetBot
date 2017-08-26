using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace PetBot
{
    class Engine
    {
        private GpioCore gpio = new GpioCore();
        private int EngineA1;
        private int EngineA2;
        private int EngineB1;
        private int EngineB2;

        public Engine()
        {
            ReadConfig(); //Initialize by getting data from config file
            ActivateEngines(); //Activate engines
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
                    if (newLine[0].Equals("Engine A 1") && newLine[1].Length > 0)
                    {
                        EngineA1 = int.Parse(newLine[1]);
                        Console.WriteLine("EngineA1 set as: " + EngineA1);
                    }
                    else if (newLine[0].Equals("Engine A 2") && newLine[1].Length > 0)
                    {
                        EngineA2 = int.Parse(newLine[1]);
                        Console.WriteLine("EngineA2 set as: " + EngineA2);
                    }
                    else if (newLine[0].Equals("Engine B 1") && newLine[1].Length > 0)
                    {
                        EngineB1 = int.Parse(newLine[1]);
                        Console.WriteLine("EngineB1 set as: " + EngineB1);
                    }
                    else if (newLine[0].Equals("Engine B 2") && newLine[1].Length > 0)
                    {
                        EngineB2 = int.Parse(newLine[1]);
                        Console.WriteLine("EngineB2 set as: " + EngineB2);
                    }
                    else
                    {
                        Console.WriteLine(string.Format("{0} has not been set, please edit the file and reload the config", newLine[0]));
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
                    Console.WriteLine("Would you like to edit (Y/N)");
                    Console.WriteLine(newLine[0] + " With the value: " + newLine[1] + "");
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

        public void ActivateEngines()
        {
            //Open engine pins
            gpio.Open(EngineA1);
            gpio.Open(EngineA2);
            gpio.Open(EngineB1);
            gpio.Open(EngineB2);

            //Set engine pin direction as 'out'
            gpio.Out(EngineA1);
            gpio.Out(EngineA2);
            gpio.Out(EngineB1);
            gpio.Out(EngineB2);

            Console.WriteLine("Engines activated");
        }

        public void Controller()
        {
            bool driving = true;
            bool engines = false; // ON/OFF
            ConsoleKeyInfo cki; //Gets user input
            ConsoleKeyInfo state; //Saves state to avoid spamming

            DateTime time = DateTime.Now;
            Console.WriteLine("Use 'W A S D' keys to run the bot, any other key will terminate it");
            while (driving)
            {
                TimeSpan span = DateTime.Now - time;
                int ms = (int)span.TotalMilliseconds; //Get ms since last command

                if (Console.KeyAvailable)
                {
                    engines = true;
                    cki = Console.ReadKey(true);
                    Console.WriteLine(ms + "ms");

                    Console.WriteLine("STATE: " + state.Key);
                    Console.WriteLine("CKI: " + cki.Key);

                    if (ms >= 300 || cki.Key.ToString() != state.Key.ToString())
                    {
                        Console.WriteLine(ms + " NEW MS");
                        Stop(); // Stop engines before changing direction
                        state = cki; //Set new state    
                        Console.WriteLine("NEW direction: " + cki.Key.ToString());
                        switch (cki.Key.ToString())
                        {
                            case "W":
                                MoveForward();
                                break;
                            case "A":
                                MoveLeft();
                                break;
                            case "S":
                                MoveReverse();
                                break;
                            case "D":
                                MoveRight();
                                break;
                            default:
                                engines = false; // Turn of engines
                                driving = false; // Quit driving mode
                                break;
                        }
                        time = DateTime.Now; // Update timer
                        Console.WriteLine("");
                    }
                    else if(cki.Key.ToString() == state.Key.ToString())
                    {
                        Console.WriteLine("");
                        //Engine still running same direction
                        time = DateTime.Now; // Update timer
                    }

                }
                else if (ms > 300)
                {
                    //No keypress detected, turn of engines
                    if(engines)
                    {
                        engines = false;
                        Stop();
                    }
                }
            }
        }

        public void MoveForward()
        {
            Console.WriteLine("Moving FORWARD, " + EngineA1 + " " + EngineB1);
            gpio.High(EngineA1);
            gpio.High(EngineB1);

        }

        public void MoveReverse()
        {
            Console.WriteLine("Moving REVERSE, " + EngineA2 + " " + EngineB2);
            gpio.High(EngineA2);
            gpio.High(EngineB2);
        }

        public void MoveLeft()
        {
            Console.WriteLine("Left LEFT, " + EngineA2 + " " + EngineB1);
            gpio.High(EngineA2);
            gpio.High(EngineB1);

        }

        public void MoveRight()
        {
            Console.WriteLine("Left RIGHT, " + EngineA1 + " " + EngineB2);
            gpio.High(EngineA1);
            gpio.High(EngineB2);

        }

        public void Stop()
        {
            Console.WriteLine("Stopping Engines");
            gpio.Low(EngineA1);
            gpio.Low(EngineA2);
            gpio.Low(EngineB1);
            gpio.Low(EngineB2);
            //Thread.Sleep(100); //Cooldown

        }
    }
}
