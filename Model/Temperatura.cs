using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace ModelsMQTT_Server
{
    public class Temperature
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public DateTime Fecha { get; set; }
    }
}