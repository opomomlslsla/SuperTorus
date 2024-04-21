using FluentValidation;
using MethodTimer;
using SuperTorus.Application.DTO;
using SuperTorus.Application.Extensions;
using SuperTorus.Domain.Entities;
using System.Drawing;


namespace SuperTorus.Application.Services
{
    public sealed class TorusService(IValidator<RequestData> validator)
    {
        readonly Random _random = new Random();
        readonly IValidator<RequestData> _validator = validator;


        public async Task<double> CalculateTorusAsync(RequestData data)
        {
            _validator.ValidateAndThrow(data);
            Torus[] toruses = await CreateTorusesAsync2(data);
            double TorVolumeSum = toruses.AsParallel().Sum(x => x.Volume);
            double AVolume = Math.Pow(data.A, 3);
            var nc = TorVolumeSum / AVolume;
            return nc;
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

        //private Torus[] CreateTorusesMultithread(RequestData data)
        //{
        //    var torusarray;
        //}

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
        private async Task<Torus[]> CreateTorusesAsync(RequestData data)
        {
            Task<Torus>[] tasks = new Task<Torus>[data.Ncount];
            for (int i = 0; i < data.Ncount; i++)
            {
                tasks[i] = Task.Run(() => CreateOneTorus(data));
            }
            var result = await Task.WhenAll(tasks);
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
            var outradius = _random.GetRandomValue(data.MinRadius, data.MaxRadius);
            var torus = new Torus()
            {
                CenterX = _random.GetRandomValue(-data.A / 2, data.A / 2),
                CenterY = _random.GetRandomValue(-data.A / 2, data.A / 2),
                CenterZ = _random.GetRandomValue(-data.A / 2, data.A / 2),
                OuterRadius = outradius,
                InnerRadius = _random.GetRandomValue(outradius - data.Thickness, outradius)
            };
            var thikness = torus.OuterRadius - torus.InnerRadius;
            torus.Volume = Math.Pow(Math.PI, 2) * 2 * torus.OuterRadius * Math.Pow((thikness / 2), 2);
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

    }
}
