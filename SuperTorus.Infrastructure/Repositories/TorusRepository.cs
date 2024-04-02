using SuperTorus.Domain.Entities;
using SuperTorus.Domain.Interfaces;
using SuperTorus.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTorus.Infrastructure.Repositories
{
    public class TorusRepository(Context context) : IRepository<Torus>
    {
        readonly Context _context = context;
        public async Task AddManyEntities(ICollection<Torus> tors)
        {
            await _context.Torus.AddRangeAsync(tors);
            await _context.SaveChangesAsync();
        }

        public void AddEntity(Torus torus)
        {
            _context.Torus.Add(torus);
            _context.SaveChanges();
        }

        public void RemoveEntity(Torus torus)
        {
            _context.Torus.Remove(torus);
            _context.SaveChanges();
        }

        public Torus GetEntityById(Guid id)
        {
            return _context.Torus.Find(id);
        }

        public ICollection<Torus> GetAllEntities()
        {
            return _context.Torus.ToList();
        }
    }
}
