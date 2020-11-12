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
      Response<ProductTable> GetProduct(string id);
      Response<string> AddServicePrice(decimal amount);
      Response<string> AddExchange(ExchangeRateTable model);
      Response<string> AddProducts(HttpFileCollectionBase images, NameValueCollection others);
      Response<string> AddCategory(CategoryTable model);
      Response<string> EditCategory(string category, string oldCategory);
      Response<string> EditSubCategory(string subCategory, string oldCategoryName);
      Response<string> EditVendor(string vendor, string oldVendorName, string link);
      Response<string> AddSubCategory(SubCategoryTable model);
      Response<string> AddVendor(VendorTable model);
      Response<string> DeleteProduct(string id);
      Response<string> DeleteVendor(string id);
      Response<string> DeleteCategory(string id);
      Response<string> DeleteSubCategory(string id);
      Response<List<SubCategoryTable>> GetSubCategory(string categoryName);
    }
}