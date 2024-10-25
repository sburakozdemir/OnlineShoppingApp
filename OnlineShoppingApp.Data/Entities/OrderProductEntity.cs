using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.Entities
{
    public class OrderProductEntity
    {
        public int OrderId { get; set; }
        

        public int ProductId { get; set; }
       

        public int Quantity { get; set; }

        public virtual OrderEntity Order { get; set; } // İlişkilendirilen sipariş
        public virtual ProductEntity Product { get; set; } // İlişkilendirilen ürün
    }
}
