using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using NLog;
using VasudaDataAccess.Data_Access.Implentations;
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

                };
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
                    string.Equals(x.CategoryName.ToLower(), model.CategoryName.ToLower()));
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
    }
}