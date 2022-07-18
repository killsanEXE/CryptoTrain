using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.Entities
{
    public class BTC
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float Price { get; set; }
    }
}