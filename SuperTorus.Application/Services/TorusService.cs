using FluentValidation;
using FluentValidation.Validators;
using MethodTimer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SuperTorus.Application.DTO;
using SuperTorus.Application.Validation;
using SuperTorus.Domain.Entities;
using SuperTorus.Domain.Interfaces;
using System;
using System.Data;

namespace SuperTorus.Application.Services
{
    public sealed class TorusService(IRepository<Torus> repository, IValidator<RequestData> validator)
    {
        readonly IRepository<Torus> _repository = repository;
        readonly Random _random = new Random();
        readonly IValidator<RequestData> _validator = validator;


        public double CalculateTorus(RequestData data)
        {
            _validator.ValidateAndThrow(data);
            Torus[] toruses = CreateTorussesMultiThread(data);
            double TorVolumeSum = toruses.AsParallel().Sum(x => x.Volume);
            //double TorVolumeSum = toruses.Sum(x => x.Volume);
            double AVolume = Math.Pow(data.A, 3);
            var nc = TorVolumeSum / AVolume;
            //toruses = toruses.OrderByDescending(x => x.Volume).ToArray();
            return nc;
        }

        public async Task AddTorus(RequestData data)
        {
            _validator.ValidateAndThrow(data);
            var toruses = CreateToruses(data, data.Ncount);
            await _repository.AddManyEntities(toruses);

        }

        public void RemoveTorus(Guid id)
        {
            var torus = _repository.GetEntityById(id);
            _repository.RemoveEntity(torus);
        }

        public Torus GetTorus(Guid id)
        {
            return _repository.GetEntityById(id);
        }

        public ICollection<Torus> GetAllTorus()
        {
            return _repository.GetAllEntities();
        }

        public void AddmanyToruses(ICollection<Torus> torus)
        {
            _repository.AddManyEntities(torus);
        }

        private double GetRandomValue(double minValue, double maxValue)
        {
            return _random.NextDouble() * (maxValue - minValue) + minValue;
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


        private Torus[] CreateToruses2(RequestData data, int starti, int endi, ref Torus[] torusArray)
        {
            for (int i = starti; i < endi; i++)
            {
                var torus = CreateOneTorus(data);
                torusArray[i] = torus;
            }
            return torusArray;

        }


        [Time]
        private Torus[] CreateTorussesMultiThread(RequestData data) 
        {
            Torus[] result = new Torus[data.Ncount];
            List<Thread> threads = new List<Thread>();
            for (int i = 1; i < 9; i++)
            {
                threads.Add(new Thread(() => CreateToruses2(data, data.Ncount / 10 * (i-1), data.Ncount/10 * i, ref result)));
            }

            threads.Add(new Thread(() => CreateToruses2(data, data.Ncount / 10 * 9, data.Ncount, ref result)));

            foreach( Thread thread in threads)
            {
                thread.Start();
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
            var tasks = new List<Task<Torus[]>>();
            for ( int i = 1; i < 9; i++)
            {
                tasks.Add(Task.Run(() => CreateToruses2(data, data.Ncount/10 * (i-1),data.Ncount/10 * i, ref result)));
            }

            tasks.Add(Task.Run(() => CreateToruses2(data, data.Ncount / 10 * 9, data.Ncount, ref result)));

            await Task.WhenAll(tasks);
            return result;
        }


        private Torus CreateOneTorus(RequestData data)
        {
            var outradius = GetRandomValue(data.MinRadius, data.MaxRadius);
            var torus = new Torus()
            {
                CenterX = GetRandomValue(-data.A / 2, data.A / 2),
                CenterY = GetRandomValue(-data.A / 2, data.A / 2),
                CenterZ = GetRandomValue(-data.A / 2, data.A / 2),
                OuterRadius = outradius,
                InnerRadius = GetRandomValue(outradius - data.Thickness, outradius)
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
