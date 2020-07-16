namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderTable()
        {
            ImageTables = new HashSet<ImageTable>();
        }

        public Guid Id { get; set; }

        public Guid ProductTypeId { get; set; }

        public string ProductLink { get; set; }

        public Guid VendorId { get; set; }

        public string SellerPhoneNumber { get; set; }

        public decimal UnitPrice { get; set; }

        public long Quantity { get; set; }

        public decimal ItemsPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImageTable> ImageTables { get; set; }

        public virtual VendorTable VendorTable { get; set; }
    }
}
