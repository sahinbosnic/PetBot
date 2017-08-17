using System;

namespace PetBot
{
    class Program
    {
        private static GpioSwitch gpio = new GpioSwitch();
        private static Engine engine = new Engine();

        static void Main(string[] args)
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1. Drive PetBot");
                Console.WriteLine("2. Activate AI");
                Console.WriteLine("3. Engine Config");
                Console.WriteLine("4. Handle GPIO pins");
                int val = int.Parse(Console.ReadLine());
                switch (val)
                {
                    case 1:
                        break;

                    case 2:
                        break;

                    case 3:
                        //Engine Config
                        EngineMenu();
                        break;

                    case 4:
                        //Handle GPIO pins
                        GpioMenu();
                        break;

                    default:
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
                Console.WriteLine("1. Update from config file");
                Console.WriteLine("2. Edit config file");
                Console.WriteLine("3. View config file");
                Console.WriteLine("4. Back");
                int val = int.Parse(Console.ReadLine());
                switch (val)
                {
                    case 1:
                        //Update from config file
                        engine.ReadConfig();
                        break;

                    case 2:
                        //Edit config file
                        engine.EditConfig();
                        break;
                    case 3:
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
                Console.WriteLine("1. Open pin");
                Console.WriteLine("2. Close pin");
                Console.WriteLine("3. Back");
                int val = int.Parse(Console.ReadLine());
                switch (val)
                {
                    case 1:
                        Console.WriteLine("Which pin would you like to open?");
                        int open = int.Parse(Console.ReadLine());
                        gpio.OpenPin(open);
                        break;

                    case 2:
                        Console.WriteLine("Which pin would you like to close?");
                        int close = int.Parse(Console.ReadLine());
                        gpio.ClosePin(close);
                        break;
                    default:
                        menu = false;
                        break;
                }
            }
        }
    }
}