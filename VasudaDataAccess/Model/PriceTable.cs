namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PriceTable
    {
        public Guid Id { get; set; }

        public string Category { get; set; }

        public string Type { get; set; }

        public string DeliveryTime { get; set; }

        public string Country { get; set; }

        public double MinKg { get; set; }

        public double MaxKg { get; set; }

        public decimal PricePerKg { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
