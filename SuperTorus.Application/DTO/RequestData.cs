using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTorus.Application.DTO
{
    public class RequestData
    {
        public double A { get; set; }
        public double MaxRadius { get; set; } 
        public double MinRadius { get; set; }
        public double Thickness { get; set; }
        public int Ncount { get; set; }
    }
}
