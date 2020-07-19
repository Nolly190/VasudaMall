﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Logic
{
    public interface IProfileService
    {
        Response<ProfileViewModel> GetProfileHomePage();
        Response<string> AddWithdrawalAccount(WithdrawalDetailsTable model);
    }
}
