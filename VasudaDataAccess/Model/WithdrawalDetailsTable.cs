namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WithdrawalDetailsTable")]
    public partial class WithdrawalDetailsTable
    {
        [Key]
        [Column(Order = 0)]
        public string Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string AccountName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string BankName { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool IsActive { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime DateCreated { get; set; }
    }
}
