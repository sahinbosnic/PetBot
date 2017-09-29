using System;
using System.Threading;

namespace PetBot
{
    class Program
    {
        private static GpioCore gpio = new GpioCore();
        private static Engine engine = new Engine();

        static void Main(string[] args)
        {
            //SQLite dependency for linux-arm
            SQLitePCL.Batteries_V2.Init();
            
            bool active = true;
            while (active)
            {
                try
                {
                    //Console.Clear();
                    Console.WriteLine("What would you like to do?");
                    Console.WriteLine("1. Drive PetBot");
                    Console.WriteLine("2. Activate AI");
                    Console.WriteLine("3. PetBot Config");
                    Console.WriteLine("4. Listen pin 23");
                    int val = int.Parse(Console.ReadLine());
                    switch (val)
                    {
                        case 1:
                            engine.Controller();
                            break;

                        case 2:
                            break;

                        case 3:
                            //Petbot Config
                            ConfigMenu();
                            break;
                        case 4:
                            bool loop = true;
                            GpioCore.Open(23);
                            GpioCore.In(23);
                            while(loop)
                            {
                                if(GpioCore.Read(23))
                                {
                                    Console.WriteLine("true");
                                }
                                else { Console.WriteLine("false"); }
                                Thread.Sleep(200);
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch(Exception ex)
                {
                    //Post info to database and light up RED led!
                    //Severety "SERIOUS"
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        static void ConfigMenu()
        {
            bool menu = true;
            while (menu)
            {
                Console.WriteLine("PetBot Config Menu");
                Console.WriteLine("1. Engine");
                Console.WriteLine("2. Obstacle sensors");
                Console.WriteLine("3. GPIO");
                Console.WriteLine("4. Back");
                int val = int.Parse(Console.ReadLine());
                switch (val)
                {
                    case 1:
                        EngineMenu();
                        break;
                    case 2:
                        break;
                    case 3:
                        GpioMenu();
                        break;
                    default:
                        menu = false;
                        break;
                }
            }
        }

        static void EngineMenu()
        {
            bool menu = true;
            while (menu)
            {
                Console.WriteLine("Engine Config Menu");
                Console.WriteLine("1. Reload config file");
                Console.WriteLine("2. Activate Engines");
                Console.WriteLine("3. Edit config file");
                Console.WriteLine("4. View config file");
                Console.WriteLine("5. Back");
                int val = int.Parse(Console.ReadLine());
                switch (val)
                {
                    case 1:
                        //Update from config file
                        engine.ReadConfig();
                        break;
                    case 2:
                        //Edit config file
                        engine.ActivateEngines();
                        break;
                    case 3:
                        //Edit config file
                        engine.EditConfig();
                        break;
                    case 4:
                        //View config file
                        engine.ViewConfig();
                        break;
                    default:
                        menu = false;
                        break;
                }
            }
        }

        static void GpioMenu()
        {
            bool menu = true;
            while (menu)
            {
                Console.WriteLine("GPIO Pin Menu");
                Console.WriteLine("1. Open");
                Console.WriteLine("2. Close");
                Console.WriteLine("3. Direction In");
                Console.WriteLine("4. Direction Out");
                Console.WriteLine("5. Value HIGH");
                Console.WriteLine("6. Value LOW");
                Console.WriteLine("DONT. Listen on pin");
                Console.WriteLine("8. Scan open pins");
                Console.WriteLine("9. Back");
                int val = int.Parse(Console.ReadLine());
                switch (val)
                {
                    case 1:
                        Console.WriteLine("Which pin would you like to open?");
                        int open = int.Parse(Console.ReadLine());
                        GpioCore.Open(open);
                        break;

                    case 2:
                        Console.WriteLine("Which pin would you like to close?");
                        int close = int.Parse(Console.ReadLine());
                        GpioCore.Close(close);
                        break;
                    case 3:
                        Console.WriteLine("Which pin would you like to set direction as 'IN'?");
                        int pinIn = int.Parse(Console.ReadLine());
                        GpioCore.In(pinIn);
                        break;
                    case 4:
                        Console.WriteLine("Which pin would you like to set direction as 'Out'?");
                        int pinOut = int.Parse(Console.ReadLine());
                        GpioCore.Out(pinOut);
                        break;
                    case 5:
                        Console.WriteLine("Which pin would you like to set value as '1' (HIGH)?");
                        int high = int.Parse(Console.ReadLine());
                        GpioCore.High(high);
                        break;
                    case 6:
                        Console.WriteLine("Which pin would you like to set value as '0' (LOW)?");
                        int low = int.Parse(Console.ReadLine());
                        GpioCore.Low(low);
                        break;
                    case 7:
                        /*Console.WriteLine("Which pin would you like to listen on?");
                        int listen = int.Parse(Console.ReadLine());
                        bool loop = true;
                        int result = 0;
                        while(loop)
                        {
                            result =  GpioCore.Listen(listen);
                            if(result != 0)
                            {
                               loop = false;
                            }
                        }*/
                        break;
                    case 8:
                        GpioCore.ScanOpenPins();
                        break;
                    default:
                        menu = false;
                        break;
                }
            }
        }
    }
}