using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class StandardItem
    {
        [Key]
        public int StandardItemID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public List<AddOn> AddOns { get; set; }

        public StandardItem()
        {
            AddOns = new List<AddOn>();
        }
    }
}
