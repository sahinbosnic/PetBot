using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PetBot
{
    class Engine
    {
        private int Engine1A;
        private int Engine1B;
        private int Engine2A;
        private int Engine2B;

        public Engine()
        {
            ReadConfig(); //Initialize by getting data from config file
        }

        public void ReadConfig()
        {
            if (File.Exists("Engine.config"))
            {
                //Read from config file
            }
            else
            {
                //Create config file
                string config =
                    "Engine 1 A: \n" +
                    "Engine 1 B: \n" +
                    "Engine 2 A: \n" +
                    "Engine 2 B: ";

                File.WriteAllText("Engine.config", config);
                Console.WriteLine("Config file 'Engine.config' was created, please edit it!");
            }
        }

        public void EditConfig()
        {
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
