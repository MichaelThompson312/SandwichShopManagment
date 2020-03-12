using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Order
    {
        public int OrderID { get; set; }
        public List<StandardItem> StandardItem { get; set; }
        public Order()
        {
            StandardItem = new List<StandardItem>();
        }

    }
}
