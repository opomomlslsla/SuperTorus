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
    public class TorusRepository(Context context) : IRepository<TorusDB>
    {
        readonly Context _context = context;
        public async Task AddManyEntities(ICollection<TorusDB> tors)
        {
            await _context.Torus.AddRangeAsync(tors);
            await _context.SaveChangesAsync();
        }

        public void AddEntity(TorusDB torus)
        {
            _context.Torus.Add(torus);
            _context.SaveChanges();
        }

        public void RemoveEntity(TorusDB torus)
        {
            _context.Torus.Remove(torus);
            _context.SaveChanges();
        }

        public TorusDB GetEntityById(Guid id)
        {
            return _context.Torus.Find(id);
        }

        public ICollection<TorusDB> GetAllEntities()
        {
            return _context.Torus.ToList();
        }
    }
}
