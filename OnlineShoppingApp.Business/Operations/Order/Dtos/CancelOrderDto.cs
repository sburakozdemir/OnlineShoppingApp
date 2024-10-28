using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Business.Operations.Order.Dtos
{
    public class CancelOrderDto
    {

        public int OrderId { get; set; }
        public bool IsCanceled { get; set; }
        public string Message { get; set; }
    }
}
