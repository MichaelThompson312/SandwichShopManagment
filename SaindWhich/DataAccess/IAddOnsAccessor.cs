using DataObjects;
using System;
using System.Collections.Generic;


namespace DataAccess
{
    public interface IAddOnsAccessor
    {
        List<AddOn> RetrieveAllAddOns();
        bool InsertAddOn(int orderID, int standardItemID, int ingredientID);
    }
}
