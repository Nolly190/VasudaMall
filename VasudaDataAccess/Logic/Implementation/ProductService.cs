using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using NLog;
using VasudaDataAccess.Data_Access.Implementation;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Logic.Implementation
{
    public class ProductService : IProductService
    {
        private UnitOfWork _unitOfWork;
        private Logger logger;

        public ProductService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaModel());
        }

        public Response<string> AddProducts(HttpFileCollectionBase images, NameValueCollection others)
        {
            var response = new Response<string>()
            {
                Status = false
            };
            try
            {
                var insertProduct = new ProductTable()
                {
                    Title = others[0],
                    Id = Guid.NewGuid(),
                    Price = Convert.ToDecimal(others[1]),
                    Clearance = others[4] == "on" ? true : false,
                    DateCreated = DateTime.UtcNow.AddHours(1),
                    Description = others[2],
                    Location = others[6],
                    Quantity = Convert.ToInt32(others[5]),
                    IsActive = true,
                    IsPopular = others[3] == "on" ? true : false,
                    Category = Guid.Parse(others[7]),
                };
                if (!string.IsNullOrEmpty(others[8]))
                {
                    insertProduct.SubCategory = Guid.Parse(others[8]);
                }
                _unitOfWork.ProductTable.Add(insertProduct);
                var ImageList = new List<string>();
                for (int i = 0; i < images.Count; i++)
                {
                    var result = UploadProductImage(insertProduct.Id, images[i]);
                    if (!result.Status)
                    {
                        DeleteProductImages(ImageList);
                        response.Message = result.Message;
                        return response;
                    }

                    var imageTable = new ImageTable()
                    {
                        Id =  Guid.NewGuid(),
                        DateCreated = DateTime.UtcNow.AddHours(1),
                        IsActive = true,
                        Path = result._entity,
                        ProductId = insertProduct.Id
                    };
                _unitOfWork.ImageTable.Add(imageTable);
                    ImageList.Add(result._entity);
                }
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                response.Message = "Could not add product";
            }

            return response;
        }

        public Response<string> UploadProductImage(Guid productId, HttpPostedFileBase image)
        {
            var response = new Response<string>()
            {
                Status = false
            };
            try
            {

                string[] excelFormats = { "jpg", "png", "jpeg" };
                var fileExt = image.FileName.Split('.');
                var actualFileExtension = fileExt[fileExt.Count() - 1];
                if (!excelFormats.Contains(actualFileExtension))
                {
                    response.Message = "upload a valid image";
                    return response;
                }
                var path = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("\\", "ProductImages"));
                Directory.CreateDirectory(path);
                var mainPath = Guid.NewGuid() + DateTime.UtcNow.ToLongDateString() + "." +
                               actualFileExtension;
                var filePath = Path.Combine(path, mainPath);
                image.SaveAs(filePath);
                response.Status = true;
                response.SetResult("~/ProductImages/" + mainPath);

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                response.Message = "Could not add product";
            }

            return response;
        }
        public Response<string> DeleteProductImages(List<string> imgUrl)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {

                foreach (var item in imgUrl)
                {
                    var getFileFullPath = System.Web.Hosting.HostingEnvironment.MapPath(item);
                    if (getFileFullPath != null)
                    {
                        File.Delete(getFileFullPath);
                    }

                }

                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }
        public Response<ProductViewModel> GetProducts()
        {
            var response = new Response<ProductViewModel>();
            try
            {
                var model = new ProductViewModel();
                model.Product = _unitOfWork.ProductTable.GetAll(x=>x.IsActive).ToList();
                model.Category = _unitOfWork.CategoryTable.GetAll(x => x.IsActive).ToList();
                model.SubCategory = _unitOfWork.SubCategoryTable.GetAll(x => x.IsActive).ToList();
                model.Vendors = _unitOfWork.VendorTable.GetAll(x => x.IsActive).ToList();
                response.SetResult(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<string> AddCategory(CategoryTable model)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var check = _unitOfWork.CategoryTable.Get(x =>
                    string.Equals(x.CategoryName.ToLower(), model.CategoryName.ToLower()) && !x.IsActive);
                if (check!=null)
                {
                    response.Message = "Category already exists";
                    return response;
                }
                model.DateCreated =DateTime.UtcNow.AddHours(1);
                model.Id=Guid.NewGuid();
                model.IsActive = true;
                _unitOfWork.CategoryTable.Add(model);
                _unitOfWork.Complete();
                response.Status=true;
            }
            catch (Exception ex)
            {
                response.Message = "Could not add category";
                logger.Error(ex.ToString());
            }

            return response;

        }

        public Response<string> AddSubCategory(SubCategoryTable model)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var check = _unitOfWork.SubCategoryTable.Get(x =>
                    string.Equals(x.Name, model.Name)&& x.CategoryId == model.CategoryId);
                if (check != null)
                {
                    response.Message = "Sub Category already exists";
                    return response;
                }
                model.DateCreated = DateTime.UtcNow.AddHours(1);
                model.Id = Guid.NewGuid();
                model.IsActive = true;
               _unitOfWork.SubCategoryTable.Add(model);
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Message = "Could not add sub category";
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<string> AddVendor(VendorTable model)
        {

            var response = new Response<string>()
            {
                Status = false
            };

            try
            {
                var checkVendor =
                    _unitOfWork.VendorTable.Get(x => x.IsActive && x.Name.ToLower() == model.Name.ToLower());
                if (checkVendor!=null)
                {
                    checkVendor.IsActive = false;
                }
                model.DateCreated =DateTime.UtcNow.AddHours(1);
                model.Id =Guid.NewGuid();
                model.IsActive = true;
                _unitOfWork.VendorTable.Add(model);
                _unitOfWork.Complete();
                response.Status = true;

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                response.Message = "Could not add vendor";
            }

            return response;
        }

        public Response<string> DeleteProduct(string id)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var getProduct = _unitOfWork.ProductTable.Get(Guid.Parse(id));
                if (getProduct==null)
                {
                    response.Message = "Could not retrieve product";
                    return response;
                }

                getProduct.IsActive = false;
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {

                response.Message = "could not delete product";
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<string> DeleteCategory(string id)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var getProduct = _unitOfWork.CategoryTable.Get(Guid.Parse(id));
                if (getProduct == null)
                {
                    response.Message = "Could not retrieve category";
                    return response;
                }

                getProduct.IsActive = false;
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Message = "could not delete category";
                logger.Error(ex.ToString());
            }
            return response;

        }

        public Response<string> DeleteVendor(string id)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var getProduct = _unitOfWork.VendorTable.Get(id);
                if (getProduct == null)
                {
                    response.Message = "Could not retrieve vendor";
                    return response;
                }

                getProduct.IsActive = false;
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {

                response.Message = "Could not delete vendor";
                logger.Error(ex.ToString());
            }
            return response;

        }
        public Response<string> DeleteSubCategory(string id)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var getProduct = _unitOfWork.SubCategoryTable.Get(Guid.Parse(id));
                if (getProduct == null)
                {
                    response.Message = "Could not retrieve sub category";
                    return response;
                }

                getProduct.IsActive = false;
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {

                response.Message = "Could not delete subcategory";
                logger.Error(ex.ToString());
            }
            return response;

        }

        public Response<ProductTable> GetProduct(string id)
        {
            var response = new Response<ProductTable>();
            response.Status = false;
            try
            {
                var getProducts = _unitOfWork.ProductTable.Get(Guid.Parse(id));
                response.SetResult(getProducts);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<List<SubCategoryTable>> GetSubCategory(string categoryName)
        {
            var response = new Response<List<SubCategoryTable>>()
            {
                Status = false
            };
            
            try
            {
                var categoryId = Guid.Parse(categoryName);
                _unitOfWork._dbContext.Configuration.ProxyCreationEnabled = false;
                var getSubCategory = _unitOfWork.SubCategoryTable.GetAll(x => x.CategoryId == categoryId && x.IsActive).ToList();
                response.SetResult(getSubCategory);
                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());

            }

            return response;
        }

        public Response<string> EditCategory(string category, string oldCategory)
        {
            var response = new Response<string>();
            try
            {
                var getCategory = _unitOfWork.CategoryTable.Get(x => x.CategoryName.ToLower() == oldCategory.ToLower() && x.IsActive);
                if (getCategory!= null)
                {
                    getCategory.CategoryName = category;
                    response.Status = true;
                    _unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<string> EditSubCategory(string subCategory, string oldCategoryName)
        {
            var response = new Response<string>();
            try
            {
                var getSubCategory = _unitOfWork.SubCategoryTable.Get(x => x.Name.ToLower() == oldCategoryName.ToLower() && x.IsActive);
                if (getSubCategory != null)
                {
                    getSubCategory.Name = subCategory;
                    response.Status = true;
                    _unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<string> EditVendor(string vendor, string oldVendorName, string link)
        {
            var response = new Response<string>();
            try
            {
                var getVendor = _unitOfWork.VendorTable.Get(x => x.Name.ToLower() == vendor.ToLower() && x.IsActive);
                if (getVendor != null)
                {
                    getVendor.Name = vendor;
                    getVendor.Link = link;
                    response.Status = true;
                    _unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<string> AddServicePrice(decimal amount)
        {
            var response = new Response<string>();
            try
            {
                var getPrice = _unitOfWork.SettingTable.GetAll().LastOrDefault();
                getPrice.ServiceCharge = amount ;
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<string> AddExchange(ExchangeRateTable model)
        {
            var response = new Response<string>();
            try
            {
                switch (model.BaseCurrency)
                {
                    case "Dollar-Yuan":
                        model.BaseCurrency = "Dollar";
                        model.ConvertedCurrency="Yuan";
                        break;
                    case "Dollar-Naira":
                        model.BaseCurrency = "Dollar";
                        model.ConvertedCurrency = "Naira";
                        break;
                    case "Yuan-Naira":
                        model.BaseCurrency = "Yuan";
                        model.ConvertedCurrency = "Naira";
                        break;
                }

                var getExisting = _unitOfWork.ExchangeRateTable.GetAll(x =>
                    x.IsActive && x.BaseCurrency == model.BaseCurrency &&
                    model.ConvertedCurrency == x.ConvertedCurrency);
                foreach (var item in getExisting)
                {
                    item.IsActive = false;
                }

                model.Id = Guid.NewGuid();
                model.DateCreated = DateTime.UtcNow.AddHours(1);
                model.IsActive = true;
                _unitOfWork.ExchangeRateTable.Add(model);
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }
    }
}