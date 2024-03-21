using SuperTorus.Application.DTO;
using SuperTorus.Domain.Entities;
using SuperTorus.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTorus.Application.Services
{
    public sealed class TorusService(IRepository<Torus> repository)
    {
        readonly IRepository<Torus> _repository = repository;
        readonly Random _random = new Random();

        public void AddTorus(RequestData data)
        {
            for (int i = 0; i < data.Ncount; i++)
            {
                var torus = new Torus()
                {
                    CenterX = GetRandomValue(-data.A / 2, data.A / 2),
                    CenterY = GetRandomValue(-data.A / 2, data.A / 2),
                    CenterZ = GetRandomValue(-data.A / 2, data.A / 2),
                    OuterRadius = GetRandomValue(data.MinRadius, data.MaxRadius),
                    InnerRadius = GetRandomValue(0, data.MinRadius) // [0;1) поэтому хз это надо править или нет
                };

                _repository.AddEntity(torus);
            }

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

    }
}
