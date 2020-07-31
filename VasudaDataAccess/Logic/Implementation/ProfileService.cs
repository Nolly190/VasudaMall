using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;
using VasudaDataAccess.Providers.Implementations;

namespace VasudaDataAccess.Logic.Implementation
{
    public class ProfileService : IProfileService
    {
        private UnitOfWork _unitOfWork;
        private Logger logger;
        private PaymentService _paymentService;

        public ProfileService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaModel());
            _paymentService = new PaymentService();
        }

        public Response<List<WithdrawalDetailsTable>> AddWithdrawalAccount(WithdrawalDetailsTable model)
        {
            var response = new Response<List<WithdrawalDetailsTable>>();
            response.Status = false;
            response.Message = " Could not add account";
            try
            {
               
                var accountName = _paymentService.GetValidAccountName(model.BankName, model.AccountNumber);
                var getBank = _unitOfWork.BankTable.Get(x => x.IsActive && x.BankCode == model.BankName);
                if (!accountName.Status)
                {
                    response.Message = "Account name could not be resolved";
                    return response;
                }

                model.Id = Guid.NewGuid().ToString();
                model.DateCreated = DateTime.UtcNow;
                model.AccountName = accountName._entity;
                model.BankName = getBank?.BankName;
                model.IsActive = true;
                _unitOfWork.WithdrawalDetailsTable.Add(model);
                _unitOfWork.Complete();
                response.Status = true;
                response.SetResult(_unitOfWork.WithdrawalDetailsTable.GetAll(x=>x.UserId==model.UserId && x.IsActive).ToList());
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
            }
            return response;
        }

        public Response<List<WithdrawalDetailsTable>> DisableWithdrawalAccount(string id)
        {
            var result = new Response<List<WithdrawalDetailsTable>>
            {
                Status = false,
                Message = "Could not retrieve info"
            };
            try
            {
                var getCard = _unitOfWork.WithdrawalDetailsTable.Get(id);
                if (getCard==null)
                {
                    result.Message = "Could not retrieve user account";
                    return result;
                }

                getCard.IsActive = false;
                _unitOfWork.Complete();
                result.Status = true;
                result.SetResult(_unitOfWork.WithdrawalDetailsTable.GetAll(x=>x.UserId ==getCard.UserId && x.IsActive).ToList());
               return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return result;
        }

        public Response<ProfileViewModel> GetProfileHomePage(string userId)
        {
            var response = new Response<ProfileViewModel>
            {
                Status = false,
                Message = "Could not retrieve info"
            };

            var model = new ProfileViewModel
            {
                RecentNotifications = new List<NotificationTable>(),
                User = new AspNetUser()
            };
            try
            {
                model.RecentNotifications = _unitOfWork.NotificationTable.GetRecentNotifications(null);
                model.User = _unitOfWork.AspNetUser.GetUser(userId);
                response.Message = "Successfully retrieved info";
                response.Status = true;
                response.SetResult(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());

            }

            response.SetResult(model);
            return response;
        }

        public Response<AspNetUser> UpdateUserProfile(AspNetUser model)
        {
            var response = new Response<AspNetUser>
            {
                Status = false,
                Message = "Could not update profile"
            };

            try
            {
                var existingUser = _unitOfWork.AspNetUser.Get(model.Id);
                if (existingUser == null)
                {
                    response.Message = "Cannot update profile of an unknown user.";
                    return response;
                }

                existingUser.FullName = string.IsNullOrEmpty(model.FullName) ? "" : model.FullName;
                existingUser.PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? "" : model.PhoneNumber;
                existingUser.Address = string.IsNullOrEmpty(model.Address) ? "" : model.Address;
                existingUser.City = string.IsNullOrEmpty(model.City) ? "" : model.City;
                existingUser.Country = string.IsNullOrEmpty(model.Country) ? "" : model.Country;
                _unitOfWork.Complete();

                response.Status = true;
                response.Message = "Successfully updated user profile";
                response.SetResult(_unitOfWork.AspNetUser.GetUser(existingUser.Id));
                return response;
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
            }

            return response;
        }

        public Response<SupportViewModel> GetAllChats(string userId)
        {
            var response = new Response<SupportViewModel>()
            {
                Status = false,
                Message = "Could not retrieve chats"
            };

            var model = new SupportViewModel
            {
                Chats = new List<ChatTable>()
            };
            try
            {
                model.Chats = _unitOfWork.ChatTable.GetAll(x => x.UserId == userId).OrderByDescending(x => x.DateCreated).ToList();
                response.Message = "Successfully retrieved chats";
                response.Status = true;
                response.SetResult(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());

            }
            return response;
        }
    }
}
