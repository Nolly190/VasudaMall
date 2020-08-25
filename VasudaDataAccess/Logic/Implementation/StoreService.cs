using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using VasudaDataAccess.Data_Access.Implementation;
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
            _unitOfWork = new UnitOfWork(new VasudaModel());
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

        public Response<StoreViewModel> GetStorePage()
        {
            var response = new Response<StoreViewModel>();
            response.Message = "Could not Get product";
            var model = new StoreViewModel();
            model.Products = new List<ProductTable>();
            model.Categories = new List<CategoryTable>();
            response.SetResult(model);
            try
            {
                model.Products = _unitOfWork.ProductTable.GetAll(x => x.IsActive).ToList();
                model.Categories = _unitOfWork.CategoryTable.GetAll(x => x.IsActive).OrderByDescending(x=>x.DateCreated).Take(6).ToList();
                response.SetResult(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<List<ProductTable>> ClearanceProduct()
        {
            var response = new Response<List<ProductTable>>();
            try
            {
                var result = _unitOfWork.ProductTable.GetAll(x => x.Clearance).ToList();
                response.SetResult(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return response;
        }
    }
}
