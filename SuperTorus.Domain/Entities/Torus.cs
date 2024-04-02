using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTorus.Domain.Entities
{
    public class Torus
    {
        public Guid Id { get; set; }
        public double OuterRadius { get; set; }
        public double InnerRadius { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double CenterZ { get; set; }

        //Thetta/Psi/Ksi
        public double Volume { get; set; }
        //public double AngleX { get; set; }
        //public double AngleZ { get; set; }
    }
}
