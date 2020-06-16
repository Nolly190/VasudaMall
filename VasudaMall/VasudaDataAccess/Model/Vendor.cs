using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.Model
{
    public class Vendor
    {
        public Guid Id { get; set; }
        public Guid Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class SupportTable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string SentBy { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
