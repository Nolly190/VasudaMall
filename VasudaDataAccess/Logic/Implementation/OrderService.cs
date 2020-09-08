using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using VasudaDataAccess.Data_Access.Implementation;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;
using VasudaDataAccess.Utility;

namespace VasudaDataAccess.Logic.Implementation
{
    public class 
        OrderService : IOrderService
    {

        private UnitOfWork _unitOfWork;
        private Logger logger;
        private Notification _notification;

        public OrderService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaModel());
            _notification = new Notification();
        }

        public Response<AdminOrderDto> GetAllOrderInfo()
        {
            var response = new Response<AdminOrderDto>();
            try
            {
                var model = new AdminOrderDto();
                var domestic = DomesticOrderStatus.AwaitingQuotation.ToString();
                model.DomesticOrder = _unitOfWork.ItemsTable.GetAll(x=>x.DomesticItemTable.Status == domestic && x.IsActive).ToList() ;
                model.UnfinishedOrders = _unitOfWork.OrderTable.GetAll(x => !x.IsCompleted && x.IsActive).Select(x => new SingleAdminOrder()
                    {
                        AspNetUser = x.AspNetUser,
                        DateCreated = x.DateCreated,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        IsCompleted = x.IsCompleted,
                        ItemsTables = x.ItemsTables,
                        NextAction = GetOrderTypeNextAction(x.OrderType, x.Status),
                        OrderType = x.OrderType,
                        ShippingFee = x.ShippingFee,
                        Status = x.Status,
                    })
                    .ToList();
                model.FinishedOrders = _unitOfWork.OrderTable.GetAll(x => x.IsCompleted && x.IsActive).ToList();
                response.SetResult(model);
                response.Status = true;
               
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<OrderHistoryViewModel> GetAllOrdersHomePage(string userId)
        {
            var result = new Response<OrderHistoryViewModel>
            {
                Status = false,
                Message = "Could not retrieve orders"
            };

            var model = new OrderHistoryViewModel
            {
                AllOrders = new List<OrderTable>()
            };
            try
            {
                model.AllOrders = _unitOfWork.OrderTable.GetAll(x => x.UserId == userId).OrderByDescending(x => x.DateCreated).ToList();
                result.Status = true;
                result.Message = "Orders retrieved successfully";
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            result.SetResult(model);
            return result;
        }
        public string GetOrderTypeNextAction(string orderType, string status)
        {
            var array = WebConfigurationManager.AppSettings[orderType].ToString().Split(',');
            var index = Array.FindIndex(array, x => x.Equals(status));
            if (array.Length >= index + 1)
            {
                return array[index + 1];
            }
            return array[index];
        }
   

        public Response<DomesticItemViewModel> GetDomesticItemsPage(string userId)
        {
          

            var result = new Response<DomesticItemViewModel>
            {
                Status = false,
                Message = "Could not retrieve domestic items view model data"
            };

            var model = new DomesticItemViewModel
            {
                DomesticItems = new List<ItemsTable>(),
                User = new AspNetUser()
            };
            try
            {
                model.DomesticItems = _unitOfWork.ItemsTable.GetAll(x => x.UserId == userId && x.IsActive == true && x.Type == ItemType.Domestic.ToString()).ToList();
                model.User = _unitOfWork.AspNetUser.GetUser(userId);
                result.Status = true;
                result.Message = "Domestic Order items view model data retrieved successfully";
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            result.SetResult(model);
            return result;
        }

        public Response<GeneralItemViewModel> GetGeneralItemsPage(string userId)
        {
            var result = new Response<GeneralItemViewModel>
            {
                Status = false,
                Message = "Could not retrieve General items view model data"
            };

            var model = new GeneralItemViewModel
            {
                GeneralItems = new List<ItemsTable>(),
                Vendors = new List<VendorTableDTO>(),
                User = new AspNetUser()
            };
            try
            {
                List<VendorTable> vendorTable = new List<VendorTable>();

                model.GeneralItems = _unitOfWork.ItemsTable.GetAll(x => x.UserId == userId && x.IsActive == true &&
                                        (x.Type != ItemType.Domestic.ToString() && x.Type != ItemType.Product.ToString()))
                                        .OrderByDescending(x => x.DateCreated).ToList();
                vendorTable = _unitOfWork.VendorTable.GetAll(x => x.IsActive == true).ToList();
                foreach (var vendor in vendorTable)
                {
                    var vendorDTO = new VendorTableDTO { Name = vendor.Name };
                    model.Vendors.Add(vendorDTO);
                }
                model.User = _unitOfWork.AspNetUser.GetUser(userId);
                result.Status = true;
                result.Message = "General Order items view model data retrieved successfully";
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            result.SetResult(model);
            return result;
        }

        public Response<OrderCheckoutViewModel> GetAllItemsPage(string userId)
        {
            var result = new Response<OrderCheckoutViewModel>
            {
                Status = false,
                Message = "Could not retrieve items"
            };

            var model = new OrderCheckoutViewModel
            {
                AllItems = new List<ItemsTable>()
            };
            try
            {
                model.AllItems = _unitOfWork.ItemsTable.GetAll(x => x.UserId == userId && x.IsActive == true && x.Status == ItemStatus.Pending.ToString()).OrderByDescending(x => x.DateCreated).ToList();
                model.AllItems.RemoveAll(x => x.Type == ItemType.Domestic.ToString() && x.DomesticItemTable.Status != DomesticOrderStatus.AcceptedQuotation.ToString());
                result.Status = true;
                result.Message = "Items retrieved successfully";
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            result.SetResult(model);
            return result;
        }

        public Response<string> AddDomesticItem(AddDomesticItemViewModel model, string userId)
        {
            var response = new Response<string>()
            {
                Message = "Failed to add domestic item",
                Status = false
            };

            try
            {
                var itemModel = new ItemsTable()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Description = model.Description,
                    Type = ItemType.Domestic.ToString(),
                    Status = ItemStatus.Pending.ToString(),
                    IsActive = true,
                    DateCreated = DateTime.Now
                };

                var domesticOrderModel = new DomesticItemTable()
                {
                    Id = itemModel.Id,
                    SenderName = model.SenderName,
                    SenderPhoneNumber = model.SenderPhoneNumber,
                    SenderAddress = model.SenderAddress,
                    Quantity = model.Quantity,
                    Weight = model.Weight,
                    ReceiverName = model.ReceiverName,
                    ReceiverNumber = model.ReceiverPhoneNumber,
                    ReceiverAddress = model.ReceiverAddress,
                    Status = DomesticOrderStatus.AwaitingQuotation.ToString(),
                    DateCreated = itemModel.DateCreated
                };

                _unitOfWork.ItemsTable.Add(itemModel);
                _unitOfWork.DomesticItemTable.Add(domesticOrderModel);
                _unitOfWork.Complete();
                response.Status = true;
                response.Message = "Successfully added item";
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<string> AddShippingItem(AddShippingItemViewModel model, string userId)
        {
            var response = new Response<string>()
            {
                Message = "Failed to add shipping item",
                Status = false
            };

            try
            {
                var itemModel = new ItemsTable()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Type = ItemType.Shipping.ToString(),
                    Status = ItemStatus.Pending.ToString(),
                    Description = model.Description,
                    IsActive = true,
                    DateCreated = DateTime.Now
                };

                var shippingOrderModel = new ShippingItemTable()
                {
                    Id = itemModel.Id,
                    SenderName = model.SenderName,
                    SenderPhoneNumber = model.SenderPhoneNumber,
                    SenderAddress = model.SenderAddress,
                    Quantity = model.Quantity,
                    Weight = model.Weight,
                    ReceiverName = model.ReceiverName,
                    ReceiverNumber = model.ReceiverPhoneNumber,
                    ReceiverAddress = model.ReceiverAddress,
                    DateCreated = itemModel.DateCreated
                };

                _unitOfWork.ItemsTable.Add(itemModel);
                _unitOfWork.ShippingItemTable.Add(shippingOrderModel);
                _unitOfWork.Complete();
                response.Status = true;
                response.Message = "Successfully added shipping item";
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<string> AddPurchaseItem(AddPurchaseItemViewModel model, string userId)
        {
            var response = new Response<string>()
            {
                Message = "Failed to add purchase item",
                Status = false
            };

            try
            {
                var totalPrice = model.UnitPrice * model.Quantity;
                var serviceCharge = totalPrice * Convert.ToDecimal(0.03);
                var overrallCharge = totalPrice + serviceCharge;

                var user = _unitOfWork.AspNetUser.Get(userId);
                if (user == null)
                {
                    return response;
                }

                if (user.Balance < overrallCharge)
                {
                    response.Message = "You do not have sufficient fund for this request";
                    return response;
                }

                var vendor = _unitOfWork.VendorTable.GetAll(x => x.Name == model.VendorName && x.IsActive == true).FirstOrDefault();
                if (vendor == null)
                {
                    return response;
                }
                var itemModel = new ItemsTable()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Type = ItemType.Purchase.ToString(),
                    Status = ItemStatus.Pending.ToString(),
                    UnitPrice = model.UnitPrice,
                    Quantity = model.Quantity,
                    Description = model.Description,
                    VendorId = vendor.Id,
                    ProductLink = model.ProductLink,
                    IsActive = true,
                    ServicePrice = serviceCharge,
                    TotalPrice = totalPrice,
                    DateCreated = DateTime.Now
                };

                //Deduct the fund for the request from the user balance...
                user.Balance -= overrallCharge;

                _unitOfWork.AspNetUser.Update(user);
                _unitOfWork.ItemsTable.Add(itemModel);
                _unitOfWork.Complete();
                response.Status = true;
                response.Message = "Successfully added purchase item";
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<string> AddShippingAndPurchaseItem(AddPurchaseAndShippingItemViewModel model, string userId)
        {
            var response = new Response<string>()
            {
                Message = "Failed to add purchase and shipping item",
                Status = false
            };

            try
            {
                var vendor = _unitOfWork.VendorTable.GetAll(x => x.Name == model.VendorName && x.IsActive == true).ToList();
                if (vendor == null)
                {
                    return response;
                }
                var purchasePartModel = new ItemsTable()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Type = ItemType.PurchaseAndShipping.ToString(),
                    Status = ItemStatus.Pending.ToString(),
                    UnitPrice = model.UnitPrice,
                    Quantity = model.Quantity,
                    Description = model.Description,
                    VendorId = vendor.FirstOrDefault().Id,
                    ProductLink = model.ProductLink,
                    IsActive = true,
                    DateCreated = DateTime.Now
                };

                var shippingPartModel = new ShippingItemTable()
                {
                    Id = purchasePartModel.Id,
                    ReceiverName = model.ReceiverName,
                    ReceiverNumber = model.ReceiverPhoneNumber,
                    ReceiverAddress = model.ReceiverAddress,
                    DateCreated = purchasePartModel.DateCreated
                };


                _unitOfWork.ItemsTable.Add(purchasePartModel);
                _unitOfWork.ShippingItemTable.Add(shippingPartModel);
                _unitOfWork.Complete();
                response.Status = true;
                response.Message = "Successfully added purchase and shipping item";
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<string> DeleteItem(string id, string userId)
        {
            var response = new Response<string>()
            {
                Message = "Delete was not successful. Try again",
                Status = false
            };

            try
            {
                var Id = Guid.Parse(id);
                var item = _unitOfWork.ItemsTable.GetAll(x => x.UserId == userId && x.Id == Id).FirstOrDefault();
                if (item == null)
                {
                    return response;
                }

                item.IsActive = false;
                _unitOfWork.ItemsTable.Update(item);
                _unitOfWork.Complete();
                response.Status = true;
                response.Message = "Delete was successful";
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<GetSingleItemResponseDTO> GetSingleItem(string id, string userId)
        {
            var response = new Response<GetSingleItemResponseDTO>()
            {
                Message = "Unable to retrieve item..",
                Status = false
            };

            try
            {
                var Id = Guid.Parse(id);

                _unitOfWork._dbContext.Configuration.ProxyCreationEnabled = false;

                var item = _unitOfWork.ItemsTable.Get(x => x.UserId == userId && x.Id == Id);
                if (item == null)
                {
                    return response;
                }

                var model = new GetSingleItemResponseDTO();

                if (item.Type == ItemType.Purchase.ToString())
                {
                    var vendor = _unitOfWork.VendorTable.Get(x => x.Id == item.VendorId);

                    model.PurchaseItem = new SinglePurchaseItemDTO
                    {
                        Description = item.Description,
                        ProductLink = item.ProductLink,
                        Status = item.Status,
                        Type = item.Type,
                        Quantity = item.Quantity.Value,
                        UnitPrice = item.UnitPrice.Value,
                        ServicePrice = item.ServicePrice == null ? 0 : item.ServicePrice.Value,
                        TotalPrice = item.TotalPrice == null ? 0 : item.TotalPrice.Value,
                        VendorName = vendor.Name,
                        DateCreated = item.DateCreated.ToString("dddd, MMMM dd yyyy")
                    };
                }

                if (item.Type == ItemType.Shipping.ToString())
                {
                    var shipping = _unitOfWork.ShippingItemTable.Get(x => x.Id == item.Id);
                    model.ShippingItem = new SingleShippingItemDTO
                    {
                        Type = item.Type,
                        Status = item.Status,
                        Description = item.Description,
                        DateCreated = item.DateCreated.ToString("dddd, MMMM dd yyyy"),
                        SenderName = shipping.SenderName,
                        SenderPhoneNumber = shipping.SenderPhoneNumber,
                        SenderAddress = shipping.SenderAddress,
                        Quantity = shipping.Quantity,
                        Weight = shipping.Weight,
                        ReceiverName = shipping.ReceiverName,
                        ReceiverNumber = shipping.ReceiverNumber,
                        ReceiverAddress = shipping.ReceiverAddress,
                        ServicePrice = item.ServicePrice == null ? 0 : item.ServicePrice.Value,
                        TotalPrice = item.TotalPrice == null ? 0 : item.TotalPrice.Value,
                    };
                }

                if (item.Type == ItemType.PurchaseAndShipping.ToString())
                {
                    var vendor = _unitOfWork.VendorTable.Get(x => x.Id == item.VendorId);
                    var shipping = _unitOfWork.ShippingItemTable.Get(x => x.Id == item.Id);

                    model.PurchaseAndShippingItem = new SinglePurchaseAndShippingItemDTO
                    {
                        Description = item.Description,
                        ProductLink = item.ProductLink,
                        Status = item.Status,
                        Type = item.Type,
                        Quantity = item.Quantity.Value,
                        UnitPrice = item.UnitPrice.Value,
                        ServicePrice = item.ServicePrice == null ? 0 : item.ServicePrice.Value,
                        TotalPrice = item.TotalPrice == null ? 0 : item.TotalPrice.Value,
                        VendorName = vendor.Name,
                        ReceiverName = shipping.ReceiverName,
                        ReceiverNumber = shipping.ReceiverNumber,
                        ReceiverAddress = shipping.ReceiverAddress,
                        DateCreated = item.DateCreated.ToString("dddd, MMMM dd yyyy")
                    };
                }

                if (item.Type == ItemType.Domestic.ToString())
                {
                    var domestic = _unitOfWork.DomesticItemTable.Get(x => x.Id == item.Id);
                    model.DomesticItem = new SingleDomesticItemDTO
                    {
                        
                        Type = item.Type,
                        Description = item.Description,
                        DateCreated = item.DateCreated.ToString("dddd, MMMM dd yyyy"),
                        SenderName = domestic.SenderName,
                        SenderPhoneNumber = domestic.SenderPhoneNumber,
                        SenderAddress = domestic.SenderAddress,
                        Quantity = domestic.Quantity,
                        Weight = domestic.Weight,
                        ReceiverName = domestic.ReceiverName,
                        ReceiverNumber = domestic.ReceiverNumber,
                        ReceiverAddress = domestic.ReceiverAddress,
                        Status = domestic.Status,
                        ServicePrice = item.ServicePrice == null ? 0 : item.ServicePrice.Value,
                        TotalPrice = item.TotalPrice == null ? 0 : item.TotalPrice.Value,
                    };
                }

                response.Status = true;
                response.Message = "Item retrieved successfully";
                response.SetResult(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return response;
        }
        public Response<OrderItemDTO> GetOrderItems(string orderId)
        {
            var response = new Response<OrderItemDTO> ();
            response.Status = false;
            try
            {
                var id = Guid.Parse(orderId);
                var model = new OrderItemDTO();
                model.Order = _unitOfWork.OrderTable.GetAll(x => x.Id == id).Select(x => new SingleOrderDTO()
                    {
                       Status = x.Status,
                       UserId = x.UserId,
                       TotalPrice = x.TotalPrice,
                       TotalServiceCharge = x.TotalServiceCharge

                    })
                    .SingleOrDefault();
                
                if (model.Order == null)
                {
                    response.Message = "Could not retrieve order";
                    return response;
                }
                _unitOfWork._dbContext.Configuration.ProxyCreationEnabled = false;
               var user = _unitOfWork.AspNetUser.Get(model.Order.UserId);
               model.FullName = user.FullName;
               model.PhoneNumber = user.PhoneNumber;
               model.Address = user.Address;
               model.Email = user.Email;
               var item = _unitOfWork.ItemsTable.GetAll().Select(x=>new {x.Type,x.Status,x.ServicePrice}).Distinct().ToList();
               var item2 = _unitOfWork.ItemsTable.GetAll().Select(x=>x.Type).ToList();
               model.Item = _unitOfWork.ItemsTable.GetAll(x => x.OrderId == id).ToList();

                response.SetResult(model);
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Message = "Could not retrieve items";
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<DomesticOrderDTO> GetDomesticInfo(string id)
        {
            var response = new Response<DomesticOrderDTO>();
            response.Status = false;
            try
            {
              //  var test = _unitOfWork.ItemsTable.GetAll();
              //  var sumt = test.Sum(x => x.ItemsPrice);

              //  var groupBy = test.GroupBy(x => x.Type);
              //var productList =  groupBy.Where(x => x.Key == "Product").ToList();
              var itemId = Guid.Parse(id);
                var getItem = _unitOfWork.ItemsTable.Get(x => x.Id == itemId);
                if (getItem == null)
                {
                    response.Message = "Could not retrieve item";
                    return response;
                }
                var newmodel = new DomesticOrderDTO()
                {
                    id = getItem.Id.ToString(),
                    Address = getItem.AspNetUser.Address,
                    DateCreated = getItem.DateCreated.ToString("f"),
                    Email = getItem.AspNetUser.Email,
                    FullName = getItem.AspNetUser.FullName,
                    PhoneNumber = getItem.AspNetUser.PhoneNumber,
                    Quantity = getItem.DomesticItemTable.Quantity,
                    Weight = getItem.DomesticItemTable.Weight,
                    ReceiverAddress = getItem.DomesticItemTable.ReceiverAddress,
                    ReceiverName = getItem.DomesticItemTable.ReceiverName,
                    ReceiverNumber = getItem.DomesticItemTable.ReceiverNumber,
                    SenderAddress = getItem.DomesticItemTable.SenderAddress,
                    SenderName = getItem.DomesticItemTable.SenderName,
                    SenderPhoneNumber = getItem.DomesticItemTable.SenderPhoneNumber
                };
                response.SetResult(newmodel);
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Message = "Could not retrieve items";
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<string> PriceQuote(string id, decimal amount)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var item = Guid.Parse(id);
                var getItem = _unitOfWork.ItemsTable.Get(x => x.Id ==item);
                if (getItem == null)
                {
                    response.Message = "Could not retrieve item";
                    return response;
                }
                getItem.TotalPrice = amount;
                getItem.DomesticItemTable.Status = DomesticOrderStatus.AwaitingUserAcceptance.ToString();
                var newMail = new MailDTO()
                {
                    Email = getItem.AspNetUser.Email,
                    Message = $"The price quotation for your domestic order is $ {amount}, Kindly login to accept the quotation.",
                    Subject = "Vasuda Price Quotation"
                };
                if (_notification.SendEmail(newMail))
                {
                    var insertNotification = new NotificationTable()
                    {
                        DateCreated = DateTime.UtcNow.AddHours(1),
                        Id = Guid.NewGuid().ToString(),
                        IsRead = false,
                        Message = newMail.Message,
                        Subject = newMail.Subject,
                        UserId = getItem.AspNetUser.Id
                    };
                    _unitOfWork.NotificationTable.Add(insertNotification);
                }
                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Message = "Could not retrieve items";
                logger.Error(ex.ToString());
            }
            return response;
        }
       public Response<string> ProcessOrder(string orderId, string amount)
        {
            var response = new Response<string>();
            response.Status = false;
            try
            {
                var id = Guid.Parse(orderId);
                var getOrder = _unitOfWork.OrderTable.Get(x => x.Id == id);
                if (getOrder== null)
                {
                    response.Message = "Could not retrieve order";
                    return response;
                }
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Message = "Could not retrieve items";
                logger.Error(ex.ToString());
            }
            return response;
        }
    }
}
