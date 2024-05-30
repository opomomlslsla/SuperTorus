using FluentValidation;
using MethodTimer;
using SuperTorus.Application.DTO;
using SuperTorus.Application.Extensions;
using SuperTorus.Domain.Entities;
using SuperTorus.Domain.Tools;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace SuperTorus.Application.Services
{
    public sealed class TorusService(IValidator<RequestData> validator)
    {
        readonly Random _random = new Random();
        readonly IValidator<RequestData> _validator = validator;


        public async Task<ResponseData> CalculateTorusAsync(RequestData data)
        {
            _validator.ValidateAndThrow(data);
            Torus[] toruses = await CreateTorusesAsync2(data);
            List<List<Sphere>> resultToruses = new List<List<Sphere>>();
            resultToruses.Add(toruses[0].TurnIntoSpheres());
            List<Sphere> res = new List<Sphere>();
            res.AddRange(resultToruses[0]);
            for (int i =0; i< toruses.Length; i++)
            {

                for (int counter=0; counter<100; counter++)
                {
                    var spheres1 = toruses[i].TurnIntoSpheres();
                    if(!CheckIntersections(spheres1, resultToruses, res))
                    {
                        break;
                    }
                }
                
            }
            
            double TorVolumeSum = toruses.AsParallel().Sum(x => x.Volume);
            double AVolume = Math.Pow(data.A, 3);
            var nc = TorVolumeSum / AVolume;
            return new ResponseData { Nc = nc, Toruses = res.ToArray()};
        }


        public double CalculateTorusParallel(RequestData data)
        {
            _validator.ValidateAndThrow(data);
            Torus[] toruses = CreateTorusesParralel(data);
            double TorVolumeSum = toruses.AsParallel().Sum(x => x.Volume);
            double AVolume = Math.Pow(data.A, 3);
            var nc = TorVolumeSum / AVolume;
            return nc;
        }


        [Time]
        private Torus[] CreateTorusesParralel(RequestData data)
        {
            var torusArray = new Torus[data.Ncount];
            Parallel.For(0, data.Ncount, new ParallelOptions { MaxDegreeOfParallelism =4} ,(int i) =>
            {
                var torus = CreateOneTorus(data);
                torusArray[i] = torus;
            });

            return torusArray;
        }

        [Time]
        private Torus[] CreateToruses(RequestData data, int ncount)
        {
            var torusArray = new Torus[ncount];
            for (int i = 0; i < ncount; i++)
            {
                var torus = CreateOneTorus(data);
                torusArray[i] = torus;
            }
            return torusArray;

        }

        private void CreateToruses2(RequestData data, int starti, int endi, Torus[] torusArray)
        {
            for (int i = starti; i < endi; i++)
            {
                var torus = CreateOneTorus(data);
                torusArray[i] = torus;
            }
        }


        [Time]
        private Torus[] CreateTorussesMultiThread(RequestData data) 
        {
            Torus[] result = new Torus[data.Ncount];
            int chunkSize = data.Ncount / 10;
            int threadCount = 10;
            Thread[] threads = new Thread[threadCount];


            for (int i = 0; i < threadCount ; i++)
            {
                int start = i * chunkSize;
                int end = (i == threadCount - 1) ? data.Ncount : (i + 1) * chunkSize;
                threads[i] =new Thread(() => 
                { 
                    for(int j = start; j<end; j++)
                    {
                        result[j] = CreateOneTorus(data);
                    }
                });
                threads[i].Start();
            }
            foreach( Thread thread in threads)
            {
                thread.Join();
            }
            return result;
        }

        [Time]
        private async Task<Torus[]> CreateTorusesAsync2(RequestData data)
        {
            Torus[] result = new Torus[data.Ncount];
            int chunkSize = data.Ncount / 10;
            int threadCount = 10;
            Task[] tasks = new Task[threadCount];

            for ( int i = 0; i < 10; i++)
            {
                int start = i * chunkSize;
                int end = (i == threadCount - 1) ? data.Ncount : (i + 1) * chunkSize;
                tasks[i] = Task.Run(() => CreateToruses2(data, start, end, result));
            }

            await Task.WhenAll(tasks);
            return result;
        }

        private Torus CreateOneTorus(RequestData data)
        {
            var torus = new Torus()
            {
                CenterX = _random.GetRandomValue(-data.A / 2, data.A / 2),
                CenterY = _random.GetRandomValue(-data.A / 2, data.A / 2),
                CenterZ = _random.GetRandomValue(-data.A / 2, data.A / 2),
                Radius = _random.GetRandomValue(data.MinRadius, data.MaxRadius),
                Tube = _random.GetRandomValue(0, data.Thickness),
                AngleX = _random.GetRandomValue(0,Math.PI / 2),
                AngleY = _random.GetRandomValue(0, Math.PI / 2)
            };
            torus.Volume = Math.Pow(Math.PI, 2) * 2 * torus.Radius * Math.Pow(torus.Tube, 2);
            return torus;
        }

        public async Task<string> ChekTorus(RequestData data)
        {
            _validator.ValidateAndThrow(data);
            Torus[] toruses = await Task.Run(() => CreateTorusesParralel(data));
            double TorVolumeSum = toruses.AsParallel().Sum(x => x.Volume);
            var nc = TorVolumeSum / Math.Pow(data.A,3);
            if (nc > 0.4)
            {
                return $"data is not correct";
            }
            return "Data is ok";
        }
        private static bool IsTorusesColliding(List<Sphere> spheres1, List<Sphere> spheres2)
        {
            for (int i = 0; i < spheres1.Count; i++)
            {
                for (int j = 0; j < spheres2.Count; j++)
                {
                    double x1 = spheres1[i].CenterX, y1 = spheres1[i].CenterY, z1 = spheres1[i].CenterZ, r1 = spheres1[i].Radius;
                    double x2 = spheres2[j].CenterX, y2 = spheres2[j].CenterY, z2 = spheres2[j].CenterZ, r2 = spheres2[j].Radius;

                    double distance = Math.Sqrt(Math.Pow(Math.Abs(x2 - x1), 2) + Math.Pow(Math.Abs(y2 - y1), 2) + Math.Pow(Math.Abs(z2 - z1), 2));
                    if (distance <= r1 + r2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Summary returns false if torus does not intersect
        private bool CheckIntersections(List<Sphere> spheres,List<List<Sphere>> listsOfSpheres, List<Sphere> res)
        {
            bool flag =false;

            for (int j = 0; j < listsOfSpheres.Count; j++)
            {
                if (IsTorusesColliding(spheres, listsOfSpheres[j]))
                {
                    flag = true;
                    return flag;
                }
            }
            if (!flag)
            {
                listsOfSpheres.Add(spheres);
                res.AddRange(spheres);
            }
            return flag;
        }

    }
}
