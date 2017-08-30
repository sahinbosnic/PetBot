using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetBot
{
    class Settings
    {
        public int Id {get; set;}
        public string Property {get; set;}
        public string Value {get; set;}
    }
}
