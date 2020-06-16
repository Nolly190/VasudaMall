using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.Model
{
    public class Setting
    {
        public Guid Id { get; set; }
        public string paystackSecretKey { get; set; }
        public string paystackPublicKey { get; set; }
        public string SmSUsername { get; set; }
        public string Password { get; set; }
    }
}
