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
    
    public partial class WithdrawalDetailsTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WithdrawalDetailsTable()
        {
            this.WithdrawalRequestTables = new HashSet<WithdrawalRequestTable>();
        }
    
        public string Id { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string UserId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WithdrawalRequestTable> WithdrawalRequestTables { get; set; }
    }
}
