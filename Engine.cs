using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace PetBot
{
    class Engine
    {
        //private GpioCore gpio = new GpioCore();
        private int EngineA1;
        private int EngineA2;
        private int EngineB1;
        private int EngineB2;
        
        private Sensors sensor {get; set;}

        public Engine()
        {
            Console.WriteLine("Connecting to database...");
            ReadConfig(); //Initialize by getting data from database
            sensor = new Sensors();
        }

        public void ReadConfig()
        {
            DataController db = new DataController();
            try
            {
                EngineA1 = int.Parse(db.GetSetting("EngineA1"));
                EngineA2 = int.Parse(db.GetSetting("EngineA2"));
                EngineB1 = int.Parse(db.GetSetting("EngineB1"));
                EngineB2 = int.Parse(db.GetSetting("EngineB2"));
                ViewConfig();
            }
            catch
            {
                Console.WriteLine("Config not found, atempting to insert..");
                db.AddSetting("EngineA1", "0");
                db.AddSetting("EngineA2", "0");
                db.AddSetting("EngineB1", "0");
                db.AddSetting("EngineB2", "0");
                Console.WriteLine("Engine config created!");
                Console.WriteLine("Please edit engine config before attempting to start the engines");
                
                EditConfig();
            }
        }

        public void EditConfig()
        {
            DataController db = new DataController();

            Console.WriteLine("Would you like to edit (Y/N): EngineA1 - {0}", db.GetSetting("EngineA1"));
            if(Console.ReadLine() == "y" || Console.ReadLine() == "Y")
            {
                Console.WriteLine("Enter new value for EngineA1");
                string newVal = Console.ReadLine();
                db.UpdateSetting("EngineA1", newVal);
            }

            Console.WriteLine("Would you like to edit (Y/N): EngineA2 - {0}", db.GetSetting("EngineA2"));
            if(Console.ReadLine() == "y" || Console.ReadLine() == "Y")
            {
                Console.WriteLine("Enter new value for EngineA2");
                string newVal = Console.ReadLine();
                db.UpdateSetting("EngineA2", newVal);
            }

            Console.WriteLine("Would you like to edit (Y/N): EngineB1 - {0}", db.GetSetting("EngineB1"));
            if(Console.ReadLine() == "y" || Console.ReadLine() == "Y")
            {
                Console.WriteLine("Enter new value for EngineB1");
                string newVal = Console.ReadLine();
                db.UpdateSetting("EngineB1", newVal);
            }

            Console.WriteLine("Would you like to edit (Y/N): EngineB2 - {0}", db.GetSetting("EngineB2"));
            if(Console.ReadLine() == "y" || Console.ReadLine() == "Y")
            {
                Console.WriteLine("Enter new value for EngineB2");
                string newVal = Console.ReadLine();
                db.UpdateSetting("EngineB2", newVal);
            }

            ReadConfig();
        }

        public void ViewConfig()
        {
            DataController db = new DataController();

            Console.WriteLine("Getting engine config..");
            Console.WriteLine("EngineA1: " + db.GetSetting("EngineA1"));
            Console.WriteLine("EngineA2: " + db.GetSetting("EngineA2"));
            Console.WriteLine("EngineB1: " + db.GetSetting("EngineB1"));
            Console.WriteLine("EngineB2: " + db.GetSetting("EngineB2"));
        }

        public void ActivateEngines()
        {
            //Open engine pins
        
            GpioCore.Open(EngineA1);
            GpioCore.Open(EngineA2);
            GpioCore.Open(EngineB1);
            GpioCore.Open(EngineB2);

            //Set engine pin direction as 'out'
            GpioCore.Out(EngineA1);
            GpioCore.Out(EngineA2);
            GpioCore.Out(EngineB1);
            GpioCore.Out(EngineB2);

            Console.WriteLine("Engines activated");
        }

        public void Controller()
        {
            Console.WriteLine("");
            ActivateEngines(); //Activate engines
            sensor.ActivateSensors(); //Activate sensors

            bool driving = true;
            bool engines = false; // ON/OFF
            bool ObstacleFront = false;
            ConsoleKeyInfo cki; //Gets user input
            ConsoleKeyInfo state; //Saves state to avoid spamming

            DateTime time = DateTime.Now;
            Console.WriteLine("Use 'W A S D' keys to run the bot, any other key will terminate it");
            while (driving)
            {
                if(sensor.ScanFrontLeft() /*|| sensor.ScanFrontRight()*/)
                {
                    //An obstacle ahead, stop from continuing forward
                    ObstacleFront = true;
                    if(state.Key.ToString() == "W" && engines)
                    {
                        //Stop engines to avoid collision
                        Console.WriteLine("Stoping engines, obstacle detected!");
                        engines = false;
                        Stop();
                    }
                } else { ObstacleFront = false;}
                
                TimeSpan span = DateTime.Now - time;
                int ms = (int)span.TotalMilliseconds; //Get ms since last command

                if (Console.KeyAvailable)
                {
                    engines = true;
                    cki = Console.ReadKey(true);
                    Console.WriteLine(ms + "ms");

                    //Console.WriteLine("STATE: " + state.Key);
                    //Console.WriteLine("CKI: " + cki.Key);

                    if (ms >= 300 || cki.Key.ToString() != state.Key.ToString())
                    {
                        //Console.WriteLine(ms + " NEW MS");
                        Stop(); // Stop engines before changing direction
                        Thread.Sleep(100);
                        state = cki; //Set new state    
                        Console.WriteLine("NEW direction: " + cki.Key.ToString());
                        switch (cki.Key.ToString())
                        {
                            case "W":
                                if(!ObstacleFront)
                                {
                                    MoveForward();
                                }
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
                        state = new ConsoleKeyInfo();
                        engines = false;
                        Stop();
                    }
                }
            }
        }

        public void MoveForward()
        {
            Console.WriteLine("Moving FORWARD, " + EngineA1 + " " + EngineB1);
            GpioCore.High(EngineA1);
            GpioCore.High(EngineB1);

        }

        public void MoveReverse()
        {
            Console.WriteLine("Moving REVERSE, " + EngineA2 + " " + EngineB2);
            GpioCore.High(EngineA2);
            GpioCore.High(EngineB2);
        }

        public void MoveLeft()
        {
            Console.WriteLine("Left LEFT, " + EngineA2 + " " + EngineB1);
            GpioCore.High(EngineA2);
            GpioCore.High(EngineB1);

        }

        public void MoveRight()
        {
            Console.WriteLine("Left RIGHT, " + EngineA1 + " " + EngineB2);
            GpioCore.High(EngineA1);
            GpioCore.High(EngineB2);

        }

        public void Stop()
        {
            Console.WriteLine("Stopping Engines");
            GpioCore.Low(EngineA1);
            GpioCore.Low(EngineA2);
            GpioCore.Low(EngineB1);
            GpioCore.Low(EngineB2);

        }
    }
}
