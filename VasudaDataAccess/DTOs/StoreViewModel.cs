using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
    public class StoreViewModel
    {
        public List<CategoryTable> Categories { get; set; }
        public List<ProductTable> Products { get; set; }
    }
}
