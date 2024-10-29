using Microsoft.EntityFrameworkCore;
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
    // Generic repository sınıfı
    public class Repository<TEntity> : IRepository<TEntity>
         where TEntity : BaseEntity
    {
        private readonly OnlineShoppingAppDbContext _db; // Veritabanı bağlamı
        private readonly DbSet<TEntity> _dbSet; // Varlık kümesi

        public Repository(OnlineShoppingAppDbContext db)
        {
            _db = db; // Veritabanı bağlamını atama
            _dbSet = _db.Set<TEntity>(); // İlgili varlık kümesini al
        }

        // Varlık ekleme metodu
        public void Add(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now; // Oluşturulma tarihini ayarla
            _dbSet.Add(entity); // Varlığı ekle
        }

        // Varlık silme metodu (soft delete)
        public void Delete(TEntity entity, bool softDelete = true)
        {
            if (softDelete)
            {
                entity.ModifiedDate = DateTime.Now; // Güncellenme tarihini ayarla
                entity.IsDeleted = true; // Soft delete uygulama
                _dbSet.Update(entity); // Varlığı güncelle
            }
            else
            {
                _dbSet.Remove(entity); // Doğrudan sil
            }
        }

        // ID ile varlık silme metodu
        public void Delete(int id)
        {
            var entity = _dbSet.Find(id); // ID ile varlığı bul
            Delete(entity); // Bulunan varlığı sil
        }

        // Asenkron varlık silme metodu (soft delete)
        public async Task DeleteAsync(TEntity entity, bool softDelete = true)
        {
            if (softDelete)
            {
                entity.ModifiedDate = DateTime.Now; // Güncellenme tarihini ayarla
                entity.IsDeleted = true; // Soft delete uygulama
                _dbSet.Update(entity); // Varlığı güncelle
            }
            else
            {
                _dbSet.Remove(entity); // Doğrudan sil
            }
            await _db.SaveChangesAsync(); // Değişiklikleri kaydet
        }

        // Asenkron ID ile varlık silme metodu
        public async Task DeleteAsync(int id, bool softDelete = true)
        {
            var entity = await _dbSet.FindAsync(id); // ID ile varlığı bul
            if (entity != null)
            {
                await DeleteAsync(entity, softDelete); // Varlığı sil
            }
        }

        // Predicate kullanarak varlık getirme metodu
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate); // Koşula uyan ilk varlığı döndür
        }

        // Tüm varlıkları getirme metodu
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate is null ? _dbSet : _dbSet.Where(predicate); // Koşula göre varlıkları döndür
        }

        // ID ile varlık bulma metodu
        public TEntity GetById(int id)
        {
            return _dbSet.Find(id); // ID ile varlığı bul
        }

        // Varlığı güncelleme metodu
        public void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now; // Güncellenme tarihini ayarla
            _dbSet.Update(entity); // Varlığı güncelle
        }

        // Asenkron ID ile varlık bulma metodu
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id); // ID ile varlığı bul
        }

        // Asenkron varlık ekleme metodu
        public async Task AddAsync(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now; // Oluşturulma tarihini ayarla
            await _dbSet.AddAsync(entity); // Varlığı asenkron ekle
        }

        // Asenkron varlık güncelleme metodu
        public async Task UpdateAsync(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now; // Güncellenme tarihini ayarla
            _dbSet.Update(entity); // Varlığı güncelle
            await _db.SaveChangesAsync(); // Değişiklikleri kaydet
        }
    }
}
