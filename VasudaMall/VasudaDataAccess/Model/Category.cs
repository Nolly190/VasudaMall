using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.Model
{
    public class Category
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
