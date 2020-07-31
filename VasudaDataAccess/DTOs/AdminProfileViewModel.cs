using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
    public class AdminProfileViewModel
    {
        public List<AspNetUser> Users { get; set; }
    }
}
