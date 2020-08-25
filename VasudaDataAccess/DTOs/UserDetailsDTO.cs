using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.DTOs
{
   public class UserDetailsDTO
    {
        public AspNetUser User { get; set; }
        public List<OrderTable> OrderHistory { get; set; }
        public List<WithdrawalRequestTable> WithdrawalRequest { get; set; }
        public List<FundingRequestTable> FundingRequest { get; set; }
    }
}
