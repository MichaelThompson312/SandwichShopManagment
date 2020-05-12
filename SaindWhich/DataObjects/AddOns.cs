using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class AddOn
    {
        [Key]
        public int IngredientID { get; set; }
        public int Quantity { get; set; }
        public String Name { get; set; }
        public string Description { get; set; }
    }
}
