using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PetBot
{
    class GpioSwitch
    {

        public void OpenPin(int pinid)
        {
            if (!Directory.Exists("/sys/class/gpio/gpio" + pinid))
            {
                Console.WriteLine("...about to open pin " + pinid);
                File.WriteAllText("/sys/class/gpio/export", pinid.ToString());
            }
            else
            {
                Console.WriteLine("...pin " + pinid + " is already open");
            }
        }

        public void ClosePin(int pinid)
        {
            if (Directory.Exists("/sys/class/gpio/gpio" + pinid))
            {
                Console.WriteLine("...about to close pin " + pinid);
                File.WriteAllText("/sys/class/gpio/unexport", pinid.ToString());
            }
            else
            {
                Console.WriteLine("...pin " + pinid + " is already closed");
            }
        }

        public void ChangeValue(int pinid, int pinvalue)
        {
            if (Directory.Exists("/sys/class/gpio/gpio" + pinid))
            {
                //Do something
            }
            else
            {
                Console.WriteLine("error... pin " + pinid + " is  closed");
            }
        }

        public void ChangeDirection(int pinid, int pindirection)
        {
            if (Directory.Exists("/sys/class/gpio/gpio" + pinid))
            {
                //Do something
            }
            else
            {
                Console.WriteLine("error... pin " + pinid + " is  closed");
            }
        }

        public void ScanOpenPins()
        {
            Console.WriteLine("Scanning for open pins...");
            for(int i = 1; i <= 40; i++)
            {
                if (Directory.Exists("/sys/class/gpio/gpio" + i))
                {
                    Console.Write("Gpio Pin " + i );
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" OPEN");
                    Console.ResetColor();
                }
                else if (!Directory.Exists("/sys/class/gpio/gpio" + i))
                {
                    Console.Write("Gpio Pin " + i);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Closed");
                    Console.ResetColor();
                }
            }
        }
    }
}
