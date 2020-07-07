namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductTable()
        {
            ImageTables = new HashSet<ImageTable>();
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public bool IsActive { get; set; }
        public bool IsPopular { get; set; }

        public DateTime DateCreated { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImageTable> ImageTables { get; set; }
    }
}
