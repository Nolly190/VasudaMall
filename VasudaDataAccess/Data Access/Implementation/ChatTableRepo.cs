using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;
using VasudaDataAccess.Data_Access.Implementation;

namespace VasudaDataAccess.Data_Access.Implementation
{
    public class ChatTableRepo : Repository<ChatTable>, IChatTable
    {
        public ChatTableRepo(DbContext context) : base(context)
        {

        }

        public DbContext Context
        {
            get
            {
                return dbcontext;
            }
        }
    }
}
