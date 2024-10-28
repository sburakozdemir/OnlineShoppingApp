﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        void Add(TEntity entity);
        void Delete(TEntity entity, bool softDelete = true);
        void Delete(int id);
        void Update(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);
        TEntity GetById(int id);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);
        Task AddAsync(TEntity entity);

        Task DeleteAsync(TEntity entity, bool softDelete = true);
        Task DeleteAsync(int id, bool softDelete = true);
        Task UpdateAsync(TEntity entity);


    }
}
