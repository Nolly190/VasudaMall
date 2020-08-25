using System;
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
        Response<ProfileViewModel> GetProfileHomePage(string userId);
        Response<List<WithdrawalDetailsTable>> AddWithdrawalAccount(WithdrawalDetailsTable model);
        Response<List<WithdrawalDetailsTable>> DisableWithdrawalAccount(string id);
        Response<AspNetUser> UpdateUserProfile(AspNetUser model);
        Response<SupportViewModel> GetAllChats(string userId);
        Response<string> BanUser(string userId, bool action);
        Response<AdminProfileViewModel> AdminGetAllUsers();
    }
}
