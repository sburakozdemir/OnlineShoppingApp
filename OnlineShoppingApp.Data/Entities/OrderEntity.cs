using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.Entities
{
    public class OrderEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public virtual UserEntity Customer { get; set; } // Siparişi veren müşteri

        public virtual ICollection<OrderProductEntity> OrderProducts { get; set; } // Siparişin ürünleri
    }
}
}
