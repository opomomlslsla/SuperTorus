using FluentValidation;
using SuperTorus.Application.DTO;
using SuperTorus.Application.Extensions;
using SuperTorus.Domain.Entities;
using SuperTorus.Domain.Interfaces;
using SuperTorus.Infrastructure.Repositories;


namespace SuperTorus.Application.Services
{
    public class TorusDBService(IRepository<TorusDB> torusRepository, IValidator<RequestData> validator)
    {
        private readonly IRepository<TorusDB> _repository = torusRepository;
        private readonly IValidator<RequestData> _validator = validator;
        readonly Random _random = new Random();


        public async Task AddTorus(RequestData data)
        {
            _validator.ValidateAndThrow(data);
            var toruses = CreateToruses(data);
            await _repository.AddManyEntities(toruses);
        }

        private TorusDB[] CreateToruses(RequestData data)
        {
            var torusArray = new TorusDB[data.Ncount];
            for (int i = 0; i < data.Ncount; i++)
            {
                var torus = CreateOneTorus(data);
                torusArray[i] = torus;
            }
            return torusArray;
        }

        private TorusDB CreateOneTorus(RequestData data)
        {
            var outradius = _random.GetRandomValue(data.MinRadius, data.MaxRadius);
            var torus = new TorusDB()
            {
                OuterRadius = outradius,
                InnerRadius = _random.GetRandomValue(outradius - data.Thickness, outradius)
            };
            var thikness = torus.OuterRadius - torus.InnerRadius;
            torus.Volume = Math.Pow(Math.PI, 2) * 2 * torus.OuterRadius * Math.Pow((thikness / 2), 2);
            return torus;
        }

        public void RemoveTorus(Guid id)
        {
            var torus = _repository.GetEntityById(id);
            _repository.RemoveEntity(torus);
        }

        public TorusDB GetTorus(Guid id)
        {
            return _repository.GetEntityById(id);
        }

        public ICollection<TorusDB> GetAllTorus()
        {
            return _repository.GetAllEntities();
        }

        public async Task AddmanyToruses(ICollection<TorusDB> torus)
        {
            await _repository.AddManyEntities(torus);
        }
    }
}
