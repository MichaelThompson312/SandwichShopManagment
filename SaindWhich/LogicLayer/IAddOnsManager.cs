using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;


namespace LogicLayer
{
    public interface IAddOnsManager
    {
        List<AddOn> GetAllAddOns();

        bool AddAddOn(int orderID, int standardItemID, int ingredientID);
    }
}
