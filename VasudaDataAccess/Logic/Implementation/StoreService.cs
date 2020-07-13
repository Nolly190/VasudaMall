using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Logic.Implementation
{
    public class StoreService : IStoreService
    {
        private UnitOfWork _unitOfWork;
        private Logger logger;

        public StoreService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaDataModel());
        }
        public Response<HomeProductViewModel> GetHomePage()
        {
            var result = new Response<HomeProductViewModel>();
            result.Status = false;
            result.Message = "Could not retrieve info";
            var model = new HomeProductViewModel();
            model.TopStory = new List<ProductTable>();
            model.Popular = new List<ProductTable>();
            try
            {
                
                model.Popular = _unitOfWork.ProductTable.GetAllPopular(null);
                model.TopStory = _unitOfWork.ProductTable.GetAllTrending(null);
                result.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());

            }

            result.SetResult(model);
            return result;
        }
    }
}
