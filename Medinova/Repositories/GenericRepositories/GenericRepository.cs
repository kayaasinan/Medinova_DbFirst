using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Medinova.Repositories.GenericRepositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MedinovaContext _context;
        private readonly DbSet<T> _table;

        public GenericRepository(MedinovaContext context)
        {
            _context = context;
            _table = context.Set<T>();
        }

        public List<T> GetAll()
        {
            return _table.ToList();
        }

        public T GetById(int id)
        {
            return _table.Find(id);
        }

        public void Add(T entity)
        {
            _table.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _table.Find(id);
            if (entity != null)
            {
                _table.Remove(entity);
                _context.SaveChanges();
            }
        }

        public T GetFirstOrDefault()
        {
            return _table.FirstOrDefault();
        }

        public List<T> GetWhere(Expression<Func<T, bool>> filter)
        {
            return _table.Where(filter).ToList();
        }
    }
}