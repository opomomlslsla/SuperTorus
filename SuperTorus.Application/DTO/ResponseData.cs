using SuperTorus.Domain.Entities;
using SuperTorus.Domain.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTorus.Application.DTO
{
    public class ResponseData
    {
        public double Nc { get; set; }
        public double Count { get; set; }
        public Sphere[] Toruses { get; set; } = new Sphere[0];
        public string Message { get; set; } = string.Empty;

    }
}
