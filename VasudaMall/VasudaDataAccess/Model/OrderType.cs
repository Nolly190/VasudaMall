using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.Model
{
    public class OrderType
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class ImageTable
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
