using Microsoft.EntityFrameworkCore.Storage;
using OnlineShoppingApp.Data.Context;
using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineShoppingAppDbContext _db;
        private IDbContextTransaction _transaction;
        public UnitOfWork(OnlineShoppingAppDbContext db)
        {
            _db = db;

        }
        public async Task BeginTransaction()
        {
            _transaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await _transaction.CommitAsync();

        }

        public void Dispose()
        {
            _db.Dispose();
            //Garbage Collecter'a sen bunu temizleyebilirsin iznini verdiğimiz yer
            //o an silmiyoruz - silinebilir yapıyoruz . Rami temizleme ihtiyacında öncelikli olacaklar

            //direk çalıştırmak istersek GC.Collect();
        }

        public async Task RollBackTransaction()
        {
            await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            return new Repository<TEntity>(_db);
        }
    }
}
