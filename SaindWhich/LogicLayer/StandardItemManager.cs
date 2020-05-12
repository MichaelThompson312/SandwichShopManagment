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
    public class StandardItemManager : IStandardItemManager
    {
        private IStandardItemAccessor _standardItemAccessor;
        public StandardItemManager(IStandardItemAccessor standardItemAccessor)
        {
            _standardItemAccessor = standardItemAccessor;
        }

        public StandardItemManager()
        {
            _standardItemAccessor = new StandardItemAccessor();
        }


        public int CreateStandardItem()
        {
            int result = 0;

            try
            {
                result = _standardItemAccessor.CreateBaseStandardItem();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Creation Failed", ex);
            }
            return result;
        }
    }
}
