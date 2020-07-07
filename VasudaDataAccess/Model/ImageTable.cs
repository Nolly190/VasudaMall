namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ImageTable
    {
        public Guid Id { get; set; }

        public Guid? OrderId { get; set; }

        public Guid? ProductId { get; set; }

        public string Path { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual OrderTable OrderTable { get; set; }

        public virtual ProductTable ProductTable { get; set; }
    }
}
