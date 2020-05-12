using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Order
    {        
        [Key]
        public int OrderID { get; set; }
        public List<StandardItem> StandardItem { get; set; }
        public string OrderFirstName { get; set; }
        public string OrderLastName { get; set; }
        public string OrderEmail { get; set; }
        public string OrderStatus { get; set; }
        public Order()
        {
            StandardItem = new List<StandardItem>();
        }

    }
}
