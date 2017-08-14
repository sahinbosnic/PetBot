using System;

namespace PetBot
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test
            GpioSwitch gpio = new GpioSwitch();
            while (true)
            {

                Console.WriteLine("1 = open pin, 2 = close pin");
                int val = int.Parse(Console.ReadLine());
                switch (val)
                {
                    case 1:
                        Console.WriteLine("which pin would you like to open?");
                        int open = int.Parse(Console.ReadLine());
                        gpio.TurnPinOn(open);
                        break;

                    case 2:
                        Console.WriteLine("which pin would you like to close?");
                        int close = int.Parse(Console.ReadLine());
                        gpio.TurnPinOff(close);
                        break;
                }
            }
        }
    }
}