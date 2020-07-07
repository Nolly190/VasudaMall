namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SubCategoryTable
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual CategoryTable CategoryTable { get; set; }
    }
}
