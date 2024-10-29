using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.Context
{
    public class OnlineShoppingAppDbContext : DbContext
    {
        // Constructor - DbContextOptions parametresi alır ve temel sınıfa aktarır
        public OnlineShoppingAppDbContext(DbContextOptions<OnlineShoppingAppDbContext> options) : base(options)
        {
        }

        // DbSet'ler, veritabanındaki tabloları temsil eder
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderProductEntity> OrderProducts { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }

        // Model yapılandırmalarını tanımlamak için
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Her bir entity için konfigürasyonları uygula
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());

            // Varsayılan ayarları ekle (bakım modunu ayarla)
            modelBuilder.Entity<SettingEntity>().HasData(
                new SettingEntity()
                {
                    Id = 1,
                    MaintenanceMode = false // Bakım modu başlangıçta kapalı
                });

            base.OnModelCreating(modelBuilder); // Temel sınıfın OnModelCreating metodunu çağır
        }
    }
}
