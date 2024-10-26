using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.UnitOfWork
{
    public interface IUnitOfWork :IDisposable
    {
        Task<int> SaveChangesAsync(); // Kaç kayda etki ettiğini geri döner o yüzden int.

        Task BeginTransaction();
        Task CommitTransaction();
        Task RollBackTransaction();

        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    }
}
