﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access.Implementation
{
    public class SettingTableRepo : Repository<SettingTable>, ISettingTable
    {
        public SettingTableRepo(DbContext context) : base(context)
        {

        }

        public DbContext Context
        {
            get
            {
                return dbcontext as DbContext;
            }
        }

        public SettingTable GetSystemSetting()
        {
            SettingTable settingTable;
            settingTable = dbcontext.Set<SettingTable>().FirstOrDefault();
            return settingTable;
        }
    }
}
