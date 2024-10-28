﻿using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Data.Context;
using OnlineShoppingApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
         where TEntity : BaseEntity
    {
        private readonly OnlineShoppingAppDbContext _db;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(OnlineShoppingAppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            _dbSet.Add(entity);


        }

        public void Delete(TEntity entity, bool softDelete = true)
        {
            if (softDelete)
            {
                entity.ModifiedDate = DateTime.Now;
                entity.IsDeleted = true;
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public async Task DeleteAsync(TEntity entity, bool softDelete = true)
        {
            if (softDelete)
            {
                entity.ModifiedDate = DateTime.Now;
                entity.IsDeleted = true;
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, bool softDelete = true)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity, softDelete);
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate is null ? _dbSet : _dbSet.Where(predicate);
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            _dbSet.Update(entity);

        }

        public async Task<TEntity> GetByIdAsync(int id) 
        {
            return await _dbSet.FindAsync(id); 
        }

        public async Task AddAsync(TEntity entity) 
        {
            entity.CreatedDate = DateTime.Now;
            await _dbSet.AddAsync(entity); 
        }

        public async Task UpdateAsync(TEntity entity) // Asenkron güncelleme metodu
        {
            entity.ModifiedDate = DateTime.Now;
            _dbSet.Update(entity);
            await _db.SaveChangesAsync(); // Değişiklikleri kaydet
        }


    }
}
