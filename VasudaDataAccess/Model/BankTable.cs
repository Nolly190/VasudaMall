namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BankTable")]
    public partial class BankTable
    {
        public long Id { get; set; }

        [Required]
        [StringLength(20)]
        public string BankCode { get; set; }

        [Required]
        [StringLength(200)]
        public string BankName { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
