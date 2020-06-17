namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SupportTable
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string SentBy { get; set; }

        public bool IsRead { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
