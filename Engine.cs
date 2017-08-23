using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace PetBot
{
    class Engine
    {
        private GpioSwitch gpio = new GpioSwitch();
        private int EngineA1;
        private int EngineA2;
        private int EngineB1;
        private int EngineB2;

        /*private int topSpeed;
        private int currentSpeed;*/

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
                int ms = (int)span.TotalMilliseconds;        
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);
                    Console.WriteLine(cki.Key);
                    Console.WriteLine(ms + "ms");
                    switch (cki.Key.ToString())
                    {
                        case "W":
                            if (ms >= 300)
                            {
                                if (state.Key.ToString() != "W")
                                {
                                    engines = false;
                                    Stop(); //Stop engine from running other direction
                                }

                                if (state.Key.ToString() == "W")
                                {
                                    //Engine is still running this way
                                    time = DateTime.Now;
                                    if(!engines)
                                    {
                                        engines = true;
                                        MoveForward();
                                    }
                                }
                                else
                                {
                                    //Activate engines
                                    MoveForward();
                                    state = cki;
                                    time = DateTime.Now; //Reset timer
                                    engines = true;
                                }
                            }
                            break;
                        case "A":
                            if (ms >= 300)
                            {
                                if (state.Key.ToString() != "A")
                                {
                                    engines = false;
                                    Stop(); //Stop engine from running other direction
                                }

                                if (state.Key.ToString() == "A")
                                {
                                    //Engine is still running this way
                                    time = DateTime.Now;
                                    if (!engines)
                                    {
                                        engines = true;
                                        Console.WriteLine("Moving LEFT");
                                    }
                                }
                                else
                                {
                                    //Activate engines
                                    Console.WriteLine("Moving LEFT");
                                    state = cki;
                                    time = DateTime.Now; //Reset timer
                                    engines = true;
                                }
                            }
                            break;
                        case "S":
                            if (ms >= 300)
                            {
                                if (state.Key.ToString() != "S")
                                {
                                    engines = false;
                                    Stop(); //Stop engine from running other direction
                                }

                                if (state.Key.ToString() == "S")
                                {
                                    //Engine is still running this way
                                    time = DateTime.Now;
                                    if (!engines)
                                    {
                                        engines = true;
                                        MoveReverse();
                                    }
                                }
                                else
                                {
                                    //Activate engines
                                    MoveReverse();
                                    state = cki;
                                    time = DateTime.Now; //Reset timer
                                    engines = true;
                                }
                            }
                            break;
                        case "D":
                            if (ms >= 300)
                            {
                                if (state.Key.ToString() != "D")
                                {
                                    engines = false;
                                    Stop(); //Stop engine from running other direction
                                }

                                if (state.Key.ToString() == "D")
                                {
                                    //Engine is still running this way
                                    time = DateTime.Now;
                                    if (!engines)
                                    {
                                        engines = true;
                                        Console.WriteLine("Moving RIGHT");
                                    }
                                }
                                else
                                {
                                    //Activate engines
                                    Console.WriteLine("Moving RIGHT");
                                    state = cki;
                                    time = DateTime.Now; //Reset timer
                                    engines = true;
                                }
                            }
                            break;
                        default:
                            engines = false; // Turn of engines
                            driving = false; // Quit driving mode
                            break;
                    }
                }
                else if (!Console.KeyAvailable && ms > 500 && engines)
                {
                    //No keypress detected, turn of engines
                    engines = false;
                    Stop();
                }
            }
        }


        public void MoveForward()
        {
            //Stop engines to avoid damage, then start moving forward
            Stop();
            Console.WriteLine("Moving FORWARD, " + EngineA1 + " " + EngineB1);
            gpio.High(EngineA1);
            gpio.High(EngineB1);

        }

        public void MoveReverse()
        {
            //Stop engines to avoid damage, then start moving reverse
            Stop();
            Console.WriteLine("Moving REVERSE, " + EngineA2 + " " + EngineB2);
            gpio.High(EngineA2);
            gpio.High(EngineB2);
        }

        public void MoveLeft()
        {
            //Stop engines to avoid damage, then start moving left
            Stop();
            Console.WriteLine("Left LEFT, " + EngineA1 + " " + EngineB1);
            gpio.High(EngineA2);
            gpio.High(EngineB1);

        }

        public void MoveRight()
        {
            //Stop engines to avoid damage, then start moving right
            Stop();
            Console.WriteLine("Left RIGHT, " + EngineA1 + " " + EngineB1);
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

        }
    }
}
