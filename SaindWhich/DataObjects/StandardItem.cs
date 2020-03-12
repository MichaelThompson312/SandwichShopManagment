using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class StandardItem
    {
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
