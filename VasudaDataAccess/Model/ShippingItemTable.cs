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
    
    public partial class ShippingItemTable
    {
        public System.Guid Id { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverNumber { get; set; }
        public string ReceiverAddress { get; set; }
        public decimal Weight { get; set; }
        public long Quantity { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderPhoneNumber { get; set; }
        public System.DateTime DateCreated { get; set; }
    
        public virtual ItemsTable ItemsTable { get; set; }
    }
}
