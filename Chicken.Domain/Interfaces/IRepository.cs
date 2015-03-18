using System.Linq;
using Chicken.Domain.Models;

namespace Chicken.Domain.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        IQueryable<T> Query();

        T GetById(int id);

        void Add(T entity);

        void Delete(T entity);

        void Edit(T entity);

        void Save();
    }
}
