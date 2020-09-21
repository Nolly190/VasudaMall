using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Response<LayoutViewModel> GetLayout(string userId)
        {
            var response = new Response<LayoutViewModel>();

            try
            {
                var model = new LayoutViewModel();

                var checkouts = _unitOfWork.ItemsTable.GetAll(x => x.UserId == userId).ToList();
                model.PendingCheckout = checkouts.Count;


                response.Status = true;
                response.SetResult(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<AdminOrderDto> GetAllOrderInfo()
        {
            var response = new Response<AdminOrderDto>();
            try
            {
                var model = new AdminOrderDto();
                var domestic = DomesticOrderStatus.AwaitingQuotation.ToString();
                model.DomesticOrder = _unitOfWork.ItemsTable
                    .GetAll(x => x.DomesticItemTable.Status == domestic && x.IsActive).ToList();
                model.UnfinishedOrders = _unitOfWork.OrderTable.GetAll(x => !x.IsCompleted && x.IsActive).Select(x =>
                        new SingleAdminOrder()
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
                            TrackingNumber = x.TrackingNumber
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
                model.AllOrders = _unitOfWork.OrderTable.GetAll(x => x.UserId == userId)
                    .OrderByDescending(x => x.DateCreated).ToList();
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
            var index = 0;
            //var array = System.Enum.GetNames(typeof(ordertype));
            //var index = Array.FindIndex(array, x => x.Equals(status));
            //if (array.Length >= index + 1)
            //{
            //    return array[index + 1];
            //}

            //return array[index];
            switch (orderType)
            {
                case "Domestic":
                    var domeArray = System.Enum.GetNames(typeof(DomesticOrderStatus)).ToList();
                    index = domeArray.IndexOf(status);
                    return domeArray[index + 1];

                case "Purchase":
                    var purchaseArray = System.Enum.GetNames(typeof(PurchaseOrderStatus)).ToList();
                    index = purchaseArray.IndexOf(status);
                    return purchaseArray[index + 1];
                case "PurchaseAndShipping":
                    var purchaseShippingArray = System.Enum.GetNames(typeof(PurchaseAndShippingOrderStatus)).ToList();
                    index = purchaseShippingArray.IndexOf(status);
                    return purchaseShippingArray[index + 1];
                case "Product":
                    var productArray = System.Enum.GetNames(typeof(ProductTable)).ToList();
                    index = productArray.IndexOf(status);
                    return productArray[index + 1];
                default:
                    return null;
            }
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
                model.DomesticItems = _unitOfWork.ItemsTable.GetAll(
                        x => x.UserId == userId && x.IsActive == true &&
                             x.Type == ItemType.Domestic.ToString() &&
                             x.Status == ItemStatus.Pending.ToString())
                    .OrderByDescending(x => x.DateCreated).ToList();

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
                                                                        (x.Type != ItemType.Domestic.ToString() &&
                                                                         x.Type != ItemType.Product.ToString())
                                                                        && x.Status != ItemStatus.Closed.ToString())
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
                model.AllItems = _unitOfWork.ItemsTable
                    .GetAll(x => x.UserId == userId && x.IsActive == true && x.Status == ItemStatus.Pending.ToString())
                    .OrderByDescending(x => x.DateCreated).ToList();
                model.AllItems.RemoveAll(x => x.Type == ItemType.Domestic.ToString());
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
                    Title = model.Title,
                    Type = ItemType.Domestic.ToString(),
                    Status = ItemStatus.Pending.ToString(),
                    IsActive = true,
                    UnitPrice = 0,
                    ItemsPrice = 0,
                    TotalPrice = 0,
                    ServicePrice = 0,
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
                    Title = model.Title,
                    Description = model.Description,
                    IsActive = true,
                    UnitPrice = 0,
                    ItemsPrice = 0,
                    TotalPrice = 0,
                    ServicePrice = 0,
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


                var vendor = _unitOfWork.VendorTable.GetAll(x => x.Name == model.VendorName && x.IsActive == true)
                    .FirstOrDefault();
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
                    Title = model.Title,
                    Description = model.Description,
                    VendorId = vendor.Id,
                    ProductLink = model.ProductLink,
                    IsActive = true,
                    ServicePrice = serviceCharge,
                    TotalPrice = totalPrice,
                    DateCreated = DateTime.Now
                };


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
                var itemPrice = model.UnitPrice * model.Quantity;
                var serviceCharge = itemPrice * Convert.ToDecimal(0.03);
                var totalPrice = itemPrice + serviceCharge;

                var vendor = _unitOfWork.VendorTable.GetAll(x => x.Name == model.VendorName && x.IsActive == true)
                    .ToList();
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
                    Title = model.Title,
                    Description = model.Description,
                    VendorId = vendor.FirstOrDefault().Id,
                    ProductLink = model.ProductLink,
                    IsActive = true,
                    ItemsPrice = itemPrice,
                    ServicePrice = serviceCharge,
                    TotalPrice = totalPrice,
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

        public Response<string> AddProductItem(int quantity, string productId, string userId)
        {
            var response = new Response<string>()
            {
                Message = "Failed to add product item",
                Status = false
            };

            try
            {
                var prodId = Guid.Parse(productId);
                var product = _unitOfWork.ProductTable.Get(x => x.Id == prodId);
                if (product == null)
                {
                    return response;
                }

                if (product.Quantity < quantity)
                {
                    response.Message = "We do not have sufficient quantities to meet your need";
                    return response;
                }

                var itemPrice = quantity * product.Price;
                var serviceCharge = itemPrice * Convert.ToDecimal(0.03);
                var totalPrice = itemPrice + serviceCharge;

                //We will use the productId as vendorId here...
                var productItem = new ItemsTable
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    UserId = userId,
                    Type = ItemType.Product.ToString(),
                    Status = ItemStatus.Pending.ToString(),
                    UnitPrice = product.Price,
                    Quantity = quantity,
                    Title = product.Title,
                    Description = product.Description,
                    IsActive = true,
                    ItemsPrice = itemPrice,
                    ServicePrice = serviceCharge,
                    TotalPrice = quantity * product.Price,
                    DateCreated = DateTime.Now
                };

                _unitOfWork.ItemsTable.Add(productItem);
                _unitOfWork.Complete();
                response.Status = true;
                response.Message = "Successfully added product item for checkout.";
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
                var item = _unitOfWork.ItemsTable.Get(x => x.UserId == userId && x.Id == Id);
                if (item == null)
                {
                    return response;
                }

                item.IsActive = false;
                item.Status = ItemStatus.Deleted.ToString();
                _unitOfWork.ItemsTable.Update(item);
                _unitOfWork.Complete();
                response.Status = true;
                response.Message = "Successfully Deleted Item!";
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
                        Title = item.Title,
                        ProductLink = item.ProductLink,
                        Status = item.Status,
                        Type = item.Type,
                        Quantity = item.Quantity.Value,
                        UnitPrice = item.UnitPrice,
                        ServicePrice = item.ServicePrice,
                        ItemPrice = item.ItemsPrice,
                        TotalPrice = item.TotalPrice,
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
                        Title = item.Title,
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
                        ServicePrice = item.ServicePrice,
                        TotalPrice = item.TotalPrice,
                    };
                }

                if (item.Type == ItemType.PurchaseAndShipping.ToString())
                {
                    var vendor = _unitOfWork.VendorTable.Get(x => x.Id == item.VendorId);
                    var shipping = _unitOfWork.ShippingItemTable.Get(x => x.Id == item.Id);

                    model.PurchaseAndShippingItem = new SinglePurchaseAndShippingItemDTO
                    {
                        Title = item.Title,
                        Description = item.Description,
                        ProductLink = item.ProductLink,
                        Status = item.Status,
                        Type = item.Type,
                        Quantity = item.Quantity.Value,
                        UnitPrice = item.UnitPrice,
                        ServicePrice = item.ServicePrice,
                        ItemPrice = item.ItemsPrice,
                        TotalPrice = item.TotalPrice,
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
                        Title = item.Title,
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
                        Status = Util.DomesticStatusEnumConverter(
                            (DomesticOrderStatus)Enum.Parse(typeof(DomesticOrderStatus), domestic.Status)),
                        ServicePrice = item.ServicePrice,
                        TotalPrice = item.TotalPrice,
                    };
                }

                if (item.Type == ItemType.Product.ToString())
                {
                    model.ProductItem = new SingleProductItemDTO
                    {
                        Description = item.Description,
                        Title = item.Title,
                        Status = item.Status,
                        Type = item.Type,
                        Quantity = item.Quantity.Value,
                        UnitPrice = item.UnitPrice,
                        ServicePrice = item.ServicePrice,
                        ItemPrice = item.ItemsPrice,
                        TotalPrice = item.TotalPrice,
                        DateCreated = item.DateCreated.ToString("dddd, MMMM dd yyyy")
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
            var response = new Response<OrderItemDTO>();
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
                model.Item = _unitOfWork.ItemsTable.GetAll(x => x.OrderId == id).Select(x => new ItemDetailsDTO()
                {
                    Title = x.Title,
                    Description = x.Description,
                    Id = x.Id,
                    OrderType = x.Type,
                    ItemsPrice = x.ItemsPrice,
                    Quantity = x.Quantity,
                    ServicePrice = x.ServicePrice,
                    TotalPrice = x.TotalPrice,

                }).ToList();

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
                var getItem = _unitOfWork.ItemsTable.Get(x => x.Id == item);
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
                    Message =
                        $"The price quotation for your domestic order is $ {amount}, Kindly login to accept the quotation.",
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
                if (getOrder == null)
                {
                    response.Message = "Could not retrieve order";
                    return response;
                }

                if (getOrder.Status == "AwaitingShippingQuotation" || getOrder.Status == "Processing")
                {
                    getOrder.ShippingFee = Convert.ToDecimal(amount);
                }

                getOrder.Status = GetNextAction(getOrder.OrderType, getOrder.Status);
                if (getOrder.Status == "Completed")
                {
                    getOrder.IsCompleted = true;
                }

                var sendMail = _notification.SendEmail(new MailDTO()
                {
                    Email = getOrder.AspNetUser.Email,
                    Message = GetNotificationMessage(getOrder.Status, getOrder),
                    Subject = $"Vasuda Mall Order"
                });
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

        public string GetNotificationMessage(string currentStatus, OrderTable order)
        {
            switch (currentStatus)
            {
                case "Processing":
                    return "Your order with the is been processed";

                case "AwaitingUserAcceptance":
                    return "your order is waiting for your Acceptance";
                case "AwaitingQuotation":
                    return "Waiting for admin quotation";
                case "AwaitingArrival":
                    return "Waiting for arrival";
                case "AwaitingShippingPayment":
                    return "Awaiting shipping payment";
                case "AwaitingShippingQuotation":
                    return "Awaiting shipping quotation";
                case "Completed":
                    return "Your order has been completed";
                case "AwaitingPurchase":
                    return "Awaiting admin purchase";
                case "Arrived":
                    return "Your order has arrived";
                default:
                    return null;
            }
        }

        public string GetNextAction(string orderType, string currentStatus)
        {
            var index = 0;
            switch (orderType)
            {
                case "Domestic":
                    var domeArray = System.Enum.GetNames(typeof(DomesticOrderStatus)).ToList();
                    index = domeArray.IndexOf(currentStatus);
                    return domeArray[index + 1];

                case "Purchase":
                    var purchaseArray = System.Enum.GetNames(typeof(PurchaseOrderStatus)).ToList();
                    index = purchaseArray.IndexOf(currentStatus);
                    return purchaseArray[index + 1];
                case "PurchaseAndShipping":
                    var purchaseShippingArray = System.Enum.GetNames(typeof(PurchaseAndShippingOrderStatus)).ToList();
                    index = purchaseShippingArray.IndexOf(currentStatus);
                    return purchaseShippingArray[index + 1];
                case "Product":
                    var productArray = System.Enum.GetNames(typeof(ProductTable)).ToList();
                    index = productArray.IndexOf(currentStatus);
                    return productArray[index + 1];
                default:
                    return null;
            }
        }

        public Response<string> ProcessDomesticItem(string id, string action, string userId)
        {
            var response = new Response<string>()
            {
                Message = "Unable to make initial payment and create order for this item. Try again",
                Status = false
            };

            try
            {
                var Id = Guid.Parse(id);
                var item = _unitOfWork.ItemsTable.Get(x =>
                    x.UserId == userId && x.Id == Id && x.Status == ItemStatus.Pending.ToString());
                if (item == null)
                {
                    return response;
                }

                var domestic = _unitOfWork.DomesticItemTable.Get(x => x.Id == item.Id &&
                                                                      x.Status == DomesticOrderStatus
                                                                          .AwaitingUserAcceptance.ToString());
                if (domestic == null)
                {
                    return response;
                }

                if (action == "Accepted")
                {
                    var user = _unitOfWork.AspNetUser.Get(userId);
                    if (user == null)
                    {
                        return response;
                    }

                    if (user.Balance < item.TotalPrice)
                    {
                        response.Message = "Insufficient fund";
                        return response;
                    }

                    var order = new OrderTable
                    {
                        Id = Guid.NewGuid(),
                        OrderType = ItemType.Domestic.ToString(),
                        Status = DomesticOrderStatus.Processing.ToString(),
                        UserId = userId,
                        IsActive = true,
                        IsCompleted = false,
                        DateCreated = DateTime.Now,
                        TotalPrice = item.TotalPrice,
                        TotalServiceCharge = 0,
                        ShippingFee = 0
                    };
                    _unitOfWork.OrderTable.Add(order);

                    user.Balance -= item.TotalPrice;
                    _unitOfWork.AspNetUser.Update(user);

                    var payment = new PaymentHistoryTable()
                    {
                        UserId = item.UserId,
                        Amount = item.TotalPrice,
                        DateCreated = DateTime.Now,
                        Purpose = PaymentHistoryPurposeEnum.DomesticOrderProcessing.ToString(),
                        Id = Guid.NewGuid(),
                        Status = PaymentHistoryStatus.Completed.ToString(),
                        TransactionType = PaymentHistoryType.Debit.ToString()
                    };
                    _unitOfWork.PaymentHistoryTable.Add(payment);

                    domestic.Status = DomesticOrderStatus.Processing.ToString();
                    _unitOfWork.DomesticItemTable.Update(domestic);

                    item.Status = ItemStatus.Completed.ToString();
                    item.OrderId = order.Id;
                    _unitOfWork.ItemsTable.Update(item);

                    response.Message = "Domestic Order created successfully!";
                }
                else if (action == "Rejected")
                {
                    domestic.Status = DomesticOrderStatus.RejectedQuotation.ToString();
                    _unitOfWork.DomesticItemTable.Update(domestic);

                    response.Message = "Domestic order rejected!";
                }

                _unitOfWork.Complete();
                response.Status = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<string> CreateGeneralOrder(string[] itemIds, string userId)
        {
            var response = new Response<string>
            {
                Status = false,
                Message = "Could not create order(s)"
            };

            try
            {
                var items = _unitOfWork.ItemsTable.GetAll(x => itemIds.ToList().Contains(x.Id.ToString()));
                if (items == null)
                {
                    return response;
                }

                var overallPrice = items.Sum(x => x.TotalPrice);

                var user = _unitOfWork.AspNetUser.Get(userId);
                if (user == null)
                {
                    return response;
                }

                if (user.Balance < overallPrice)
                {
                    response.Message = "Insufficient fund to create order(s) request";
                    return response;
                }

                var PurchaseItems = new List<ItemsTable>();
                var PurchaseAndShippingItems = new List<ItemsTable>();
                var ProductItems = new List<ItemsTable>();
                foreach (var itemskey in items.GroupBy(x => x.Type).ToList())
                {
                    PurchaseItems.AddRange(itemskey.Where(x => x.Type == ItemType.Purchase.ToString()).ToList());
                    PurchaseAndShippingItems.AddRange(itemskey
                        .Where(x => x.Type == ItemType.PurchaseAndShipping.ToString()).ToList());
                    ProductItems.AddRange(itemskey.Where(x => x.Type == ItemType.Product.ToString()).ToList());
                }

                var ProductOrder = new OrderTable();
                var PurchaseOrder = new OrderTable();
                var PurchaseAndShippingOrder = new OrderTable();

                var dateCreated = DateTime.Now;

                if (PurchaseItems.Count > 0)
                {
                    //Deduct the fund for the request from the user balance...
                    var purchaseSum = PurchaseItems.Sum(x => x.TotalPrice);
                    user.Balance -= purchaseSum;

                    PurchaseOrder.Id = Guid.NewGuid();
                    PurchaseOrder.OrderType = ItemType.Purchase.ToString();
                    PurchaseOrder.Status = PurchaseOrderStatus.AwaitingPurchase.ToString();
                    PurchaseOrder.UserId = userId;
                    PurchaseOrder.IsActive = true;
                    PurchaseOrder.IsCompleted = false;
                    PurchaseOrder.DateCreated = dateCreated;
                    PurchaseOrder.TotalPrice = purchaseSum;
                    PurchaseOrder.TotalServiceCharge = 0;
                    PurchaseOrder.ShippingFee = 0;

                    foreach (var item in PurchaseItems)
                    {
                        item.OrderId = PurchaseOrder.Id;
                        item.Status = ItemStatus.Completed.ToString();
                        _unitOfWork.ItemsTable.Update(item);
                    }

                    var payment = new PaymentHistoryTable()
                    {
                        UserId = userId,
                        Amount = purchaseSum,
                        DateCreated = DateTime.Now,
                        Purpose = PaymentHistoryPurposeEnum.PurchaseOrder.ToString(),
                        Id = Guid.NewGuid(),
                        Status = PaymentHistoryStatus.Completed.ToString(),
                        TransactionType = PaymentHistoryType.Debit.ToString()
                    };
                    _unitOfWork.PaymentHistoryTable.Add(payment);

                    _unitOfWork.AspNetUser.Update(user);
                    _unitOfWork.OrderTable.Add(PurchaseOrder);
                }

                if (PurchaseAndShippingItems.Count > 0)
                {
                    //Deduct the fund for the request from the user balance...
                    var purchaseSum = PurchaseAndShippingItems.Sum(x => x.TotalPrice);
                    user.Balance -= purchaseSum;

                    PurchaseAndShippingOrder.Id = Guid.NewGuid();
                    PurchaseAndShippingOrder.OrderType = ItemType.PurchaseAndShipping.ToString();
                    PurchaseAndShippingOrder.Status = PurchaseOrderStatus.AwaitingPurchase.ToString();
                    PurchaseAndShippingOrder.UserId = userId;
                    PurchaseAndShippingOrder.IsActive = true;
                    PurchaseAndShippingOrder.IsCompleted = false;
                    PurchaseAndShippingOrder.DateCreated = dateCreated;
                    PurchaseAndShippingOrder.TotalPrice = purchaseSum;
                    PurchaseAndShippingOrder.TotalServiceCharge = 0;
                    PurchaseAndShippingOrder.ShippingFee = 0;

                    foreach (var item in PurchaseAndShippingItems)
                    {
                        item.OrderId = PurchaseAndShippingOrder.Id;
                        item.Status = ItemStatus.Completed.ToString();
                        _unitOfWork.ItemsTable.Update(item);
                    }

                    var payment = new PaymentHistoryTable()
                    {
                        UserId = userId,
                        Amount = purchaseSum,
                        DateCreated = DateTime.Now,
                        Purpose = PaymentHistoryPurposeEnum.PurchaseAndShippingOrderProcessing.ToString(),
                        Id = Guid.NewGuid(),
                        Status = PaymentHistoryStatus.Completed.ToString(),
                        TransactionType = PaymentHistoryType.Debit.ToString()
                    };
                    _unitOfWork.PaymentHistoryTable.Add(payment);

                    _unitOfWork.AspNetUser.Update(user);
                    _unitOfWork.OrderTable.Add(PurchaseAndShippingOrder);
                }

                if (ProductItems.Count > 0)
                {
                    var me = Guid.NewGuid();
                    var prodIds = new List<Guid>();
                    foreach (var prod in ProductItems)
                    {
                        prodIds.Add(prod.ProductId.Value);
                    }

                    //Products is the one from the product table...
                    //ProductItems is the one from the item table....

                    var Products = _unitOfWork.ProductTable.GetAll(x => prodIds.Contains(x.Id)).ToList();
                    var ProductsAndProductItems = Products.Zip(ProductItems, (P, PI) => new { Product = P, ProductItem = PI });

                    foreach (var product in ProductsAndProductItems)
                    {
                        if (product.Product.Quantity < product.ProductItem.Quantity)
                        {
                            response.Message = "We do not have sufficient quantity of Products to meet your need";
                            return response;
                        }

                        var currentProduct = Products.FirstOrDefault(x => x.Id == product.Product.Id);
                        currentProduct.Quantity -= (int)product.ProductItem.Quantity;
                        _unitOfWork.ProductTable.Update(currentProduct);
                    }

                    //Deduct the fund for the request from the user balance...
                    var purchaseSum = ProductItems.Sum(x => x.TotalPrice);
                    user.Balance -= purchaseSum;

                    ProductOrder.Id = Guid.NewGuid();
                    ProductOrder.OrderType = ItemType.Product.ToString();
                    ProductOrder.Status = PurchaseOrderStatus.Arrived.ToString();
                    ProductOrder.UserId = userId;
                    ProductOrder.IsActive = true;
                    ProductOrder.IsCompleted = false;
                    ProductOrder.DateCreated = dateCreated;
                    ProductOrder.TotalPrice = purchaseSum;
                    ProductOrder.TotalServiceCharge = 0;
                    ProductOrder.ShippingFee = 0;

                    foreach (var item in ProductItems)
                    {
                        item.OrderId = ProductOrder.Id;
                        item.Status = ItemStatus.Completed.ToString();
                        _unitOfWork.ItemsTable.Update(item);
                    }

                    var payment = new PaymentHistoryTable()
                    {
                        UserId = userId,
                        Amount = purchaseSum,
                        DateCreated = DateTime.Now,
                        Purpose = PaymentHistoryPurposeEnum.ProductOrder.ToString(),
                        Id = Guid.NewGuid(),
                        Status = PaymentHistoryStatus.Completed.ToString(),
                        TransactionType = PaymentHistoryType.Debit.ToString()
                    };

                    _unitOfWork.PaymentHistoryTable.Add(payment);
                    _unitOfWork.OrderTable.Add(ProductOrder);

                    _unitOfWork.AspNetUser.Update(user);
                }

                _unitOfWork.Complete();
                response.Status = true;
                response.Message = "Your order(s) have been created successfully!";
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<SingleOrderDTO> GetSingleOrder(string id, string userId)
        {
            var response = new Response<SingleOrderDTO>()
            {
                Message = "Unable to retrieve order..",
                Status = false
            };

            try
            {
                var Id = Guid.Parse(id);

                _unitOfWork._dbContext.Configuration.ProxyCreationEnabled = false;

                var order = _unitOfWork.OrderTable.Get(x => x.UserId == userId && x.Id == Id);
                if (order == null)
                {
                    return response;
                }

                var model = new SingleOrderDTO
                {
                    TotalPrice = order.TotalPrice.Value,
                    Type = order.OrderType,
                    Status = order.Status,
                    ShippingFee = order.ShippingFee.Value,
                    TotalServiceCharge = order.TotalServiceCharge.Value,
                    DateCreated = order.DateCreated.ToString("dddd, MMMM dd yyyy")
                };

                response.Status = true;
                response.Message = "Order retrieved successfully";
                response.SetResult(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<List<PriceTable>> GetAllPrice()
        {

            var response = new Response<List<PriceTable>>()
            {
                Message = "Unable to retrieve order..",
                Status = false
            };

            try
            {

                var prices = _unitOfWork.PriceTable.GetAll(x => x.IsActive).ToList();
                response.Status = true;
                response.Message = "Order retrieved successfully";
                response.SetResult(prices);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<ReportDTO> GetHomeReport()
        {
            var response = new Response<ReportDTO>()
            {
                Message = "Unable to retrieve order..",
                Status = false
            };

            try
            {

                var model = new ReportDTO();
                var allUsers = _unitOfWork.AspNetUser.GetAll();
                var allOrders = _unitOfWork.OrderTable.GetAll();
                model.MonthlyOrders = allOrders.Count(x => x.DateCreated.Month == DateTime.Now.Month);
                model.AllOrders = allOrders.Count();
                model.AllUsers = allUsers.Count(x => !x.AspNetRoles.Any());
                model.NewUsers = allUsers.Count(x => x.DateCreated.Month == DateTime.Now.Month);
                model.UnconfirmedUsers = allUsers.Count(x => !x.EmailConfirmed);
                model.CompletedOrder = allOrders.Count(x => !x.IsCompleted);
                model.NewNotification = _unitOfWork.SupportTable.GetAll().Count(x => x.SentBy == "User" && !x.IsRead);
                response.Status = true;
                response.Message = "Order retrieved successfully";
                response.SetResult(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return response;
        }

        public Response<GetSingleOrderResponseDTO> SingleCreatedOrder(string orderId)
        {
            var response = new Response<GetSingleOrderResponseDTO>()
            {
                Message = "Unable to retrieve order...",
                Status = false
            };

            try
            {
                _unitOfWork._dbContext.Configuration.ProxyCreationEnabled = false;
                var id = Guid.Parse(orderId);
                var order = _unitOfWork.OrderTable.Get(x => x.Id == id);
                var items = _unitOfWork.ItemsTable.GetAll(x => x.OrderId == order.Id).ToList();

                if (order == null || items == null)
                {
                    return response;
                }


                var model = new GetSingleOrderResponseDTO();
                model.PurchaseAndShippingOrders = new List<SinglePurchaseAndShippingItemDTO>();
                model.PurchaseOrders = new List<SinglePurchaseItemDTO>();
                model.DomesticOrder = new SingleDomesticItemDTO();

                if (order.OrderType == ItemType.Purchase.ToString())
                {
                    Parallel.ForEach(items, item =>
                    {
                        var singelItem = new SinglePurchaseItemDTO
                        {
                            Description = item.Description,
                            Title = item.Title,
                            ProductLink = item.ProductLink,
                            Quantity = item.Quantity.Value,
                            UnitPrice = item.UnitPrice,
                            ServicePrice = item.ServicePrice,
                            TotalPrice = item.TotalPrice,
                        };
                        model.PurchaseOrders.Add(singelItem);

                    });
                    model.PurchaseAndShippingOrders = null;
                    model.DomesticOrder = null;
                }

                if (order.OrderType == ItemType.PurchaseAndShipping.ToString())
                {
                    Parallel.ForEach(items, item =>
                    {
                        var shipping = _unitOfWork.ShippingItemTable.Get(x => x.Id == item.Id);

                        var singleItem = new SinglePurchaseAndShippingItemDTO
                        {
                            Title = item.Title,
                            Description = item.Description,
                            ProductLink = item.ProductLink,
                            Quantity = item.Quantity.Value,
                            TotalPrice = item.TotalPrice,
                            Type = ItemType.PurchaseAndShipping.ToString(),
                            ReceiverName = shipping.ReceiverName,
                        };
                        model.PurchaseAndShippingOrders.Add(singleItem);
                    });

                    model.PurchaseOrders = null;
                    model.DomesticOrder = null;

                }

                if (order.OrderType == ItemType.Domestic.ToString())
                {
                    var item = items.FirstOrDefault();
                    if (item == null)
                    {
                        return response;
                    }

                    var domestic = _unitOfWork.DomesticItemTable.Get(x => x.Id == item.Id);
                    var singleItem = new SingleDomesticItemDTO
                    {
                        Title = item.Title,
                        Description = item.Description,
                        Quantity = domestic.Quantity,
                        Weight = domestic.Weight,
                        ReceiverName = domestic.ReceiverName,
                        Status = Util.DomesticStatusEnumConverter((DomesticOrderStatus)Enum.Parse(typeof(DomesticOrderStatus), domestic.Status))
                    };

                    model.DomesticOrder = singleItem;
                    model.PurchaseOrders = null;
                    model.PurchaseAndShippingOrders = null;
                }

                response.Status = true;
                response.Message = "Orders retrieved successfully";
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
