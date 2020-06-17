namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NotificationTable
    {
        [Key]
        [Column(Order = 0)]
        public string Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool IsRead { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime DateCreated { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
