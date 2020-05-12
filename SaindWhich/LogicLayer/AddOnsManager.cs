using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccess;
using System.Security.Cryptography;

namespace LogicLayer
{
    public class AddOnsManager : IAddOnsManager
    {
        private IAddOnsAccessor _addOnsAccessor;

        public AddOnsManager()
        {
            _addOnsAccessor = new AddOnsAccessor();
            List<string> addOnList = new List<string>();
        }

        public AddOnsManager(IAddOnsAccessor addOnsAccessor)
        {
            _addOnsAccessor = addOnsAccessor;
            List<string> addOnList = new List<string>();
        }

        public List<AddOn> GetAllAddOns()
        {
            try
            {
                return _addOnsAccessor.RetrieveAllAddOns();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("List Not Available", ex);
            }
        }

        
        public bool AddAddOn(int orderID, int standardItemID, int ingredientID)
        {
            bool result = false;

            try
            {

                result = _addOnsAccessor.InsertAddOn(orderID, standardItemID, ingredientID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed.", ex);
            }
            return result;
        }
    }
}
