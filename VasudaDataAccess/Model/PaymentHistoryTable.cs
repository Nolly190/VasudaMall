//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaymentHistoryTable
    {
        public System.Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Purpose { get; set; }
        public string Status { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string UserId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
