using SuperTorus.Domain.Tools;
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
        public double Radius { get; set; }
        public double Tube { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double CenterZ { get; set; }
        public double Volume { get; set; }
        public double AngleX { get; set; }
        public double AngleY { get; set; }

        public List<Sphere> TurnIntoSpheres()
        {
            var circleRadius = Radius;
            var betweenpoints = Tube / 2;
            double circumference = 2 * Math.PI * circleRadius;
            int n = (int)(circumference / betweenpoints);
            List<Sphere> result = new List<Sphere>();
            for (int i = 0; i < n; i++)
            {

                var angle2 = AngleX;
                var angle3 = AngleY;
                double angle = 2 * Math.PI * i / n;
                double x = circleRadius * Math.Cos(angle);
                double y = circleRadius * Math.Sin(angle);
                double z = 0;


                var tempy = y;
                y = y * Math.Cos(angle2) - z * Math.Sin(angle2);
                z = tempy * Math.Sin(angle2) + z * Math.Cos(angle2);

                var tempx = x;
                x = x * Math.Cos(angle3) + z * Math.Sin(angle3);
                z = -tempx * Math.Sin(angle3) + z * Math.Cos(angle3);

                x = Math.Round(x, 6) + CenterX;
                y = Math.Round(y, 6) + CenterY;
                z = Math.Round(z, 6) + CenterZ;

                result.Add(new Sphere { CenterX = x, CenterY = y, CenterZ = z, Radius = Tube });
            }
            return result;
        }

    }
}
