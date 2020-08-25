﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access.Implementation
{
    public class DomesticItemTableRepo : Repository<DomesticItemTable>, IDomesticItemTable
    {
        public DomesticItemTableRepo(DbContext context) : base(context)
        {

        }

        public DbContext Context
        {
            get
            {
                return dbcontext as DbContext;
            }
        }
    }

    public enum DomesticOrderStatus
    {
        AwaitingQuotation = 1, AcceptedQuotation, RejectedQuotation
    }
}