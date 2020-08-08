using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Logic
{
    public interface IProductService
    {
      Response<ProductViewModel> GetProducts();
      Response<string> AddProducts(HttpFileCollectionBase images, NameValueCollection others);
      Response<string> AddCategory(CategoryTable model);
      Response<string> AddSubCategory(SubCategoryTable model);
    }
}