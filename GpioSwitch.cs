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

        public void ChangePinValue(int pinid, int pinvalue)
        {

        }
    }
}
