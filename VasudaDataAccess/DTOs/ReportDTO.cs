using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.DTOs
{
    public class ReportDTO
    {
        public long AllOrders { get; set; }
        public long MonthlyOrders { get; set; }
        public long AllUsers { get; set; }
        public long NewUsers { get; set; }
        public long UnconfirmedUsers { get; set; }
        public long CompletedOrder { get; set; }
        public long PendingOrder { get; set; }
        public decimal? Wallet { get; set; }
        public long NewNotification { get; set; }
    }
}
