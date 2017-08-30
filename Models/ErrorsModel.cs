using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetBot
{
    class Errors
    {
        public int Id {get; set;}
        public string Severety {get; set;}
        public string Color {get; set;}
        public string Message {get; set;}
        public bool Status {get; set;}
    }
}
