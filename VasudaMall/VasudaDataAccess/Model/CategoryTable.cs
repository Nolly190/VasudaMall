namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CategoryTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CategoryTable()
        {
            SubCategoryTables = new HashSet<SubCategoryTable>();
        }

        public Guid Id { get; set; }

        public string CategoryName { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubCategoryTable> SubCategoryTables { get; set; }
    }
}
