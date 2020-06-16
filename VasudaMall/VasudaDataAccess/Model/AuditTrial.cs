using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.Model
{
    public class AuditTrial
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }                
    }
}
