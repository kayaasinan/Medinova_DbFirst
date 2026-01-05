using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Medinova.Repositories.GenericRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        T GetFirstOrDefault();
        List<T> GetWhere(Expression<Func<T, bool>> filter);
    }
}
