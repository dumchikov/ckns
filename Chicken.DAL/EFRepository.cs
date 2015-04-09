using System.Data.Entity;
using System.Linq;
using Chicken.Domain.Interfaces;
using Chicken.Domain.Models;

namespace Chicken.DAL
{
    public class EFRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly ChickenDbContext Context;

        public EFRepository(ChickenDbContext context)
        {
            Context = context;
            Context.Configuration.AutoDetectChangesEnabled = false;
            Context.Configuration.ValidateOnSaveEnabled = false;
        }

        public virtual IQueryable<T> Query()
        {
            var query = Context.Set<T>();
            return query;
        }

        public virtual T GetById(int id)
        {
            var query = Context.Set<T>().SingleOrDefault(x => x.Id == id);
            return query;
        }

        public virtual void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
