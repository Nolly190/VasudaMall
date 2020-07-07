namespace VasudaDataAccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SettingTable
    {
        public Guid Id { get; set; }

        public string paystackSecretKey { get; set; }

        public string paystackPublicKey { get; set; }

        public string SmSUsername { get; set; }

        public string Password { get; set; }
    }
}
