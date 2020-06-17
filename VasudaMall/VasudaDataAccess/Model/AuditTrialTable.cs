namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AuditTrialTable
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Action { get; set; }

        public string Description { get; set; }

        public string ModifiedData { get; set; }

        public string OriginalData { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
