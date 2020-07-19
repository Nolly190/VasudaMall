using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Logic
{
  public interface IStoreService
  {
      Response<HomeProductViewModel> GetHomePage();
      Response<StoreViewModel> GetStorePage();
      Response<List<ProductTable>> ClearanceProduct();
  }
}
