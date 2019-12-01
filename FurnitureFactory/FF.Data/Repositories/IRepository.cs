using System;
using System.Linq;
using System.Threading.Tasks;

namespace FF.Models.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeMembers);

        Task<TEntity> GetById(int id);

        TEntity Add(TEntity entity);

        void Delete(TEntity entity);

        Task<int> Save();
    }
}