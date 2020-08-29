using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using VasudaDataAccess.Data_Access.Implementation;
using VasudaDataAccess.DTOs;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Logic.Implementation
{
    public class OrderService : IOrderService
    {

        private UnitOfWork _unitOfWork;
        private Logger logger;

        public OrderService()
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = new UnitOfWork(new VasudaModel());
        }

        public Response<AdminOrderDto> GetAllOrderInfo()
        {
            var response = new Response<AdminOrderDto>();
            try
            {
                var model = new AdminOrderDto();
                model.DomesticOrder = new List<ItemsTable>() ;
                model.UnfinishedOrders = _unitOfWork.OrderTable.GetAdminOrders();
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
                var itemPrice = model.UnitPrice * model.Quantity;
                var serviceCharge = itemPrice * Convert.ToDecimal(0.03);
                var totalPrice = itemPrice + serviceCharge;

                //var user = _unitOfWork.AspNetUser.Get(userId);
                //if (user == null)
                //{
                //    return response;
                //}

                //if (user.Balance < overrallCharge)
                //{
                //    response.Message = "You do not have sufficient fund for this request";
                //    return response;
                //}

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
                    ItemsPrice = itemPrice,
                    ServicePrice = serviceCharge,
                    TotalPrice = totalPrice,
                    DateCreated = DateTime.Now
                };

                ////Deduct the fund for the request from the user balance...
                //user.Balance -= overrallCharge;

                //_unitOfWork.AspNetUser.Update(user);
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
                    ItemsPrice = itemPrice,
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
                        ItemPrice = item.ItemsPrice == null ? 0 : item.ItemsPrice.Value,
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
                        ItemPrice = item.ItemsPrice == null ? 0 : item.ItemsPrice.Value,
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
        public Response<List<ItemsTable>> GetOrderItems(string orderId)
        {
            var response = new Response<List<ItemsTable>>();
            response.Status = false;
            try
            {
                var getOrder = _unitOfWork.OrderTable.Get(x => x.Id == Guid.Parse(orderId));
                if (getOrder == null)
                {
                    response.Message = "Could not retrieve order";
                    return response;
                }
                response.SetResult(getOrder.ItemsTables.ToList());
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Message = "Could not retrieve items";
                logger.Error(ex.ToString());
            }
            return response;
        }

        public Response<string> CreateOrder(string[] itemIds, string userId)
        {
            var response = new Response<string>
            {
                Status = false,
                Message = "Could not create purchase order"
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

                var DomesticItem = new List<ItemsTable>();
                var PurchaseItem = new List<ItemsTable>();
                var PurchaseAndShippingItem = new List<ItemsTable>();
                var Product = new List<ItemsTable>();
                foreach (var itemskey in items.GroupBy(x => x.Type).ToList())
                {
                    DomesticItem.AddRange(itemskey.Where(x=>x.Type== ItemType.Domestic.ToString()).ToList());
                    PurchaseItem.AddRange(itemskey.Where(x=>x.Type== ItemType.Purchase.ToString()).ToList());
                    PurchaseAndShippingItem.AddRange(itemskey.Where(x=>x.Type== ItemType.PurchaseAndShipping.ToString()).ToList());
                    Product.AddRange(itemskey.Where(x=>x.Type== ItemType.Product.ToString()).ToList());
                }              

                var DomesticOrder = new OrderTable();
                var ProductOrder = new OrderTable();
                var PurchaseOrder = new OrderTable();
                var PurchaseAndShippingOrder = new OrderTable();

                var dateCreated = DateTime.Now;

                if (DomesticItem.Count > 0)
                {
                    DomesticOrder.Id = Guid.NewGuid();
                    DomesticOrder.OrderType = ItemType.Domestic.ToString();
                    DomesticOrder.Status = DomesticOrderStatus.AwaitingQuotation.ToString();
                    DomesticOrder.UserId = userId;
                    DomesticOrder.IsActive = true;
                    DomesticOrder.IsCompleted = false;
                    DomesticOrder.DateCreated = dateCreated;
                    DomesticOrder.TotalPrice = DomesticItem.Sum(x => x.TotalPrice);
                    DomesticOrder.TotalServiceCharge = 0;
                    DomesticOrder.ShippingFee = 0;

                    foreach (var item in DomesticItem)
                    {
                        items.Where(x => x.Id == item.Id).ToList().Select(x => 
                                { x.OrderId = DomesticOrder.Id;
                                    _unitOfWork.ItemsTable.Update(x);
                                    return x; 
                                });
                    }

                    _unitOfWork.OrderTable.Add(DomesticOrder);
                }
                if (PurchaseItem.Count > 0)
                {
                    //Deduct the fund for the request from the user balance...
                    var purchaseSum = PurchaseItem.Sum(x => x.TotalPrice);
                    user.Balance -= purchaseSum.Value;

                    PurchaseOrder.Id = Guid.NewGuid();
                    PurchaseOrder.OrderType = ItemType.Purchase.ToString();
                    PurchaseOrder.Status = PurchaseOrderStatus.AwaitingPurchase.ToString();
                    PurchaseOrder.UserId = userId;
                    PurchaseOrder.IsActive = true;
                    PurchaseOrder.IsCompleted = false;
                    PurchaseOrder.DateCreated = dateCreated;
                    PurchaseOrder.TotalPrice = DomesticItem.Sum(x => x.TotalPrice);
                    PurchaseOrder.TotalServiceCharge = 0;
                    PurchaseOrder.ShippingFee = 0;

                    foreach (var item in PurchaseItem)
                    {
                        items.Where(x => x.Id == item.Id).ToList().Select(x => 
                                { x.OrderId = PurchaseOrder.Id;
                                    _unitOfWork.ItemsTable.Update(x);
                                    return x; });
                    }

                    _unitOfWork.AspNetUser.Update(user);
                    _unitOfWork.OrderTable.Add(PurchaseOrder);
                }
                if (PurchaseAndShippingItem.Count > 0)
                {
                    //Deduct the fund for the request from the user balance...
                    var purchaseSum = PurchaseAndShippingItem.Sum(x => x.TotalPrice);
                    user.Balance -= purchaseSum.Value;

                    PurchaseAndShippingOrder.Id = Guid.NewGuid();
                    PurchaseAndShippingOrder.OrderType = ItemType.Purchase.ToString();
                    PurchaseAndShippingOrder.Status = PurchaseOrderStatus.AwaitingPurchase.ToString();
                    PurchaseAndShippingOrder.UserId = userId;
                    PurchaseAndShippingOrder.IsActive = true;
                    PurchaseAndShippingOrder.IsCompleted = false;
                    PurchaseAndShippingOrder.DateCreated = dateCreated;
                    PurchaseAndShippingOrder.TotalPrice = DomesticItem.Sum(x => x.TotalPrice);
                    PurchaseAndShippingOrder.TotalServiceCharge = 0;
                    PurchaseAndShippingOrder.ShippingFee = 0;

                    foreach (var item in PurchaseAndShippingItem)
                    {
                        items.Where(x => x.Id == item.Id).ToList().Select(x => 
                                { x.OrderId = PurchaseAndShippingOrder.Id;
                                    _unitOfWork.ItemsTable.Update(x); 
                                    return x; });
                    }

                    _unitOfWork.AspNetUser.Update(user);
                    _unitOfWork.OrderTable.Add(PurchaseAndShippingOrder);
                }

                //if (items.Any(x => x.Type != ItemType.Purchase.ToString()))
                //{
                //    response.Message = "Make sure that the items are of the same type to create the order.";
                //    return response;
                //}

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
    }
}
