﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access.Implentations
{
    public class PaymentHistoryTableRepo : Repository<PaymentHistoryTable>, IPaymentHistoryTable
    {
        public PaymentHistoryTableRepo(DbContext context) : base(context)
        {

        }

        public DbContext Context
        {
            get
            {
                return dbcontext as DbContext;
            }
        }

        public List<PaymentHistoryTable> GetRecentTransactionsHistory(int? pageNum)
        {
            var lastTwoWeeks = DateTime.Now.Date.AddDays(-14);
            return pageNum.HasValue ? dbcontext.Set<PaymentHistoryTable>().Where(x => x.DateCreated >= lastTwoWeeks).Skip(pageNum.Value * 10).Take(10).ToList() : dbcontext.Set<PaymentHistoryTable>().Where(x => x.DateCreated >= lastTwoWeeks).Take(10).ToList();
        }
    }
}
