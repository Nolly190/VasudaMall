using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
  public class HomeProductViewModel
    {
        public List<ProductTable> Popular { get; set; }
        public List<ProductTable> TopStory { get; set; }
    }
}
