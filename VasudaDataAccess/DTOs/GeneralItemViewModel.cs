using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
    public class GeneralItemViewModel
    {
        public AspNetUser User { get; set; }
        public List<ItemsTable> GeneralItems { get; set; }
        public List<VendorTableDTO> Vendors { get; set; }
    }
}
