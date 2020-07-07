namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PaymentHistoryTable
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public string TransactionType { get; set; }

        public string Purpose { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
