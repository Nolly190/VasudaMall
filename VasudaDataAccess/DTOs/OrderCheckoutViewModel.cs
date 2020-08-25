using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
    public class OrderCheckoutViewModel
    {
        public List<ItemsTable> AllItems { get; set; }
    }
}
