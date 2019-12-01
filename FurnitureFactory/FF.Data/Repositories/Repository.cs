using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FF.Models.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _entities;
        private readonly DbContext _context;

        public Repository(FurnitureFactoryDbContext context)
        {
            _entities = context.Set<TEntity>();
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _entities;
        }

        public IQueryable<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeMembers)
        {
            return includeMembers(_entities);
        }

        public Task<TEntity> GetById(int id)
        {
            return _entities.FindAsync(id);
        }

        public TEntity Add(TEntity entity)
        {
            return _entities.Add(entity).Entity;
        }

        public void Delete(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public Task<int> Save() => _context.SaveChangesAsync();
    }
}
