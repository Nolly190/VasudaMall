using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access
{
    public interface IAspNetUser : IRepository<AspNetUser>
    {
        AspNetUser GetUser(string userId);
    }
}

