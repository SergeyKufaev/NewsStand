using System.Collections.Generic;

namespace NewsStand.Core.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        T GetById(int id);
        IReadOnlyList<T> GetAll();
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
