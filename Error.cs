using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PetBot
{
    class Error
    {
        GpioCore gpio = new GpioCore();
        public Error()
        {
            
        }

        public void SetError()
        {
            //Insert Error in database

            //Light up red Led
        }

        public void SetWarning()
        {
            //Insert Warning in database

            //Light up yellow Led
        }

        public void RemoveError()
        {
            //Turn off red led
            
            //Set status as false (handled)
        }

        public void RemoveWarning()
        {
            //Turn off yellow led

            //Set status as false (handled)
        }
    }
}
