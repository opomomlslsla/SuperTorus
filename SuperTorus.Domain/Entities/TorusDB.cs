using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTorus.Domain.Entities
{
    public class TorusDB
    {
        public Guid Id { get; set; }
        public double OuterRadius { get; set; }
        public double InnerRadius { get; set; }
        public double Volume { get; set; }
    }
}
