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
    // Unit of Work deseni uygulayan sınıf
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineShoppingAppDbContext _db; // Veritabanı bağlamı
        private IDbContextTransaction _transaction; // İşlem durumu

        public UnitOfWork(OnlineShoppingAppDbContext db)
        {
            _db = db; // Veritabanı bağlamını atama
        }

        // İşleme başlama metodu
        public async Task BeginTransaction()
        {
            _transaction = await _db.Database.BeginTransactionAsync(); // Yeni bir işlem başlat
        }

        // İşlemi tamamlama metodu
        public async Task CommitTransaction()
        {
            await _transaction.CommitAsync(); // İşlemi onayla
        }

        // Dispose metodu, kaynakları serbest bırakma
        public void Dispose()
        {
            _db.Dispose(); // Veritabanı bağlamını serbest bırak
            // Garbage Collector'a temizleme izni verilir
            // İsteğe bağlı olarak GC.Collect() ile zorlayabilirsiniz
        }

        // İşlemi geri alma metodu
        public async Task RollBackTransaction()
        {
            await _transaction.RollbackAsync(); // İşlemi geri al
        }

        // Değişiklikleri kaydetme metodu
        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync(); // Değişiklikleri kaydet
        }

        // Generic repository döndürme metodu
        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            return new Repository<TEntity>(_db); // İlgili repository'yi oluştur
        }
    }
}
