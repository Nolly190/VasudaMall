using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Data_Access.Implentations;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Logic.Implementation
{
    public class StoreService : IStoreService
    {
        private UnitOfWork _unitOfWork;

        public StoreService()
        {
            _unitOfWork = new UnitOfWork(new VasudaModel());
        }
        public Response<HomeProductViewModel> GetHomePage()
        {
            var result = new Response<HomeProductViewModel>();
            result.Status = false;
            result.Message = "Could not retrieve info";

            try
            {
                var model = new HomeProductViewModel();
                model.Popular = _unitOfWork.ProductTable.GetAllPopular(null);
                model.TopStory = _unitOfWork.ProductTable.GetAllPopular(null);
                result.Status = true;
                result.SetResult(model);

            }
            catch (Exception ex)
            {
                

            }

            return result;
        }
    }
}
