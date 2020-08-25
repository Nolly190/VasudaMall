using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VasudaDataAccess.Data_Access.Implementation;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Logic.Implementation
{
    public class BankService : IBankService
    {
        private UnitOfWork _unitOfWork;
        private Logger logger;

        public BankService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaModel());
        }

        public Response<AdminBankViewModel> GetAllBanks()
        {
            var response = new Response<AdminBankViewModel>()
            {
                Status = false,
                Message = "Could not retrieve banks"
            };

            var model = new AdminBankViewModel
            {
                Banks = new List<BankTable>()
            };
            try
            {
                model.Banks = _unitOfWork.BankTable.GetAll().ToList();
                response.Message = "Successfully retrieved all banks";
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
