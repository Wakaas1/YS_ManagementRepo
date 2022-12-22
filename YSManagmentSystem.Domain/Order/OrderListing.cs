using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSManagmentSystem.Domain.Order
{
    public class OrderListing
    {
        public int Id { get; set; }
        public List<OrderItem> Items { get; set; }
       

    }
}
