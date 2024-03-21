using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTorus.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetEntityById(Guid id);
        ICollection<T> GetAllEntities();
        void AddEntity(T torus);
        void RemoveEntity(T torus);
        Task AddManyEntities(ICollection<T> tors);

    }
}
