﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.Entities
{
    public class ProductEntity:BaseEntity
    {

        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public virtual ICollection<OrderProductEntity> OrderProducts { get; set; }

    }

    public class ProductConfiguration : BaseConfiguration<ProductEntity>
    {
        public override void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.StockQuantity)
                .IsRequired();

            // Diğer konfigürasyonlar
            base.Configure(builder);
        }
    }
}
