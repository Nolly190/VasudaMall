using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
   public class ProductViewModel
    {
        public List<ProductTable> Product { get; set; }
        public List<SubCategoryTable> SubCategory { get; set; }
        public List<CategoryTable> Category { get; set; }
        public List<VendorTable> Vendors { get; set; }
    }
}
