$(document).ready(function () {

    $("#addShippingItemBtn").click(function () {
        if (StartValidation("addShippingItemForm")) {
            $(".Main-loader").show();
            var shipppingItemData = {};
            shipppingItemData.ReceiverName = $("#soReceiverName").val();
            shipppingItemData.ReceiverPhoneNumber = $("#soReceiverPhoneNumber").val();
            shipppingItemData.ReceiverAddress = $("#soReceiverAddress").val();
            shipppingItemData.Description = $("#soDescription").val();
            shipppingItemData.Quantity = $("#soQuantity").val();
            shipppingItemData.Weight = $("#soWeight").val();
            shipppingItemData.SenderName = $("#soSenderName").val();
            shipppingItemData.SenderPhoneNumber = $("#soSenderPhoneNumber").val();
            shipppingItemData.SenderAddress = $("#soSenderAddress").val();

            $.ajax({
                url: "/Item/AddShippingItem",
                type: "Post",
                data: { model: shipppingItemData },
                error: function (status, xhr) {
                    $(".Main-loader").hide();
                },
                success: function (result) {
                    if (result.Status === true) {
                        $(".Main-loader").hide();
                        $("#soSenderName").val("");
                        $("#soSenderPhoneNumber").val("");
                        $("#soSenderAddress").val("");
                        $("#soDescription").val("");
                        $("#soQuantity").val("");
                        $("#soWeight").val("");
                        $("#soReceiverName").val("");
                        $("#soReceiverPhoneNumber").val("");
                        $("#soReceiverAddress").val("");
                        location.reload(true);
                        Swal.fire('Shipping order added Successfully.');

                        return;
                    } else {
                        $(".Main-loader").hide();
                        Swal.fire(result.Message);
                        return;
                    }
                }
            });
        }
        return;
    });

    //Purchase item start
    $("#addPurchaseItemButton").click(function () {
        if (StartValidation("addPurchaseItemForm")) {
            $(".Main-loader").show();
            var purchaseItemData = {};
            purchaseItemData.VendorName = $("#poVendorWebsite").val();
            purchaseItemData.ProductLink = $("#poProductLink").val();
            purchaseItemData.Description = $("#poDescription").val();
            purchaseItemData.UnitPrice = $("#poUnitPrice").val();
            purchaseItemData.Quantity = $("#poQuantity").val();

            $.ajax({
                url: "/Item/AddPurchaseItem",
                type: "Post",
                data: { model: purchaseItemData },
                error: function (status, xhr) {
                    $(".Main-loader").hide();
                },
                success: function (result) {
                    if (result.Status === true) {
                        $(".Main-loader").hide();
                        $("#poVendorWebsite").val("");
                        $("#poProductLink").val("");
                        $("#poDescription").val("");
                        $("#poUnitPrice").val("");
                        $("#poQuantity").val("");
                        location.reload(true);
                        Swal.fire('Purchase item added Successfully.');

                        return;
                    } else {
                        $(".Main-loader").hide();
                        Swal.fire(result.Message);
                        return;
                    }
                }
            });
        }
        return;
    });


    //Purchase and shipping item start...
    $("#addShippingAndPurchaseItemButton").click(function () {
        if (StartValidation("addShippingAndPurchaseItemForm")) {
            $(".Main-loader").show();
            var shippingPurchaseItemData = {};
            shippingPurchaseItemData.VendorName = $("#spoVendorWebsite").val();
            shippingPurchaseItemData.ProductLink = $("#spoProductLink").val();
            shippingPurchaseItemData.Description = $("#spoDescription").val();
            shippingPurchaseItemData.UnitPrice = $("#spoUnitPrice").val();
            shippingPurchaseItemData.Quantity = $("#spoQuantity").val();
            shippingPurchaseItemData.ReceiverName = $("#spoReceiverName").val();
            shippingPurchaseItemData.ReceiverPhoneNumber = $("#spoReceiverPhoneNumber").val();
            shippingPurchaseItemData.ReceiverAddress = $("#spoReceiverAddress").val();

            $.ajax({
                url: "/Item/AddShippingAndPurchaseItem",
                type: "Post",
                data: { model: shippingPurchaseItemData },
                error: function (status, xhr) {
                    $(".Main-loader").hide();
                },
                success: function (result) {
                    if (result.Status === true) {
                        $(".Main-loader").hide();
                        $("#spoVendorWebsite").val("");
                        $("#spoProductLink").val("");
                        $("#spoDescription").val("");
                        $("#spoUnitPrice").val("");
                        $("#spoQuantity").val("");
                        $("#spoReceiverName").val("");
                        $("#spoReceiverPhoneNumber").val("");
                        $("#spoReceiverAddress").val("");
                        location.reload(true);
                        Swal.fire('Shipping and Purchase order added Successfully.');

                        return;
                    } else {
                        $(".Main-loader").hide();
                        Swal.fire(result.Message);
                        return;
                    }
                }
            });
        }
        return;
    });

    ////Get a general item details start
    //$(".generalItemsViewMoreButton").click(function () {
    //    $(".Main-loader").show();

    //    var id = $(this).data('id');
    //    $.ajax({
    //        url: "/Item/GetGeneralItem",
    //        type: "Post",
    //        data: { id : id},
    //        error: function (status, xhr) {
    //            $(".Main-loader").hide();
    //            alert(status);
    //            console.log(status.responseText);
    //            console.log(xhr);
    //        },
    //        success: function (result) {
    //            if (result.Status === true) {
    //                $(".Main-loader").hide();
    //                entity = result._entity;

    //                if (entity.PurchaseItem != null && entity.PurchaseItem.Type == "Purchase") {
    //                    pEntity = entity.PurchaseItem;

    //                    $("#pItemDescription").html(pEntity.Description);
    //                    $("#pItemUnitPrice").html(pEntity.UnitPrice);
    //                    $("#pItemQuantity").html(pEntity.Quantity);
    //                    $("#pItemServicePrice").html(pEntity.ServicePrice);
    //                    $("#pItemTotalPrice").html(pEntity.TotalPrice);
    //                    $("#pItemProductLink").html(pEntity.ProductLink);
    //                    $("#pItemVendor").html(pEntity.VendorName);
    //                    $("#pItemStatus").html(pEntity.Status);
    //                    $("#pItemDateCreated").html(pEntity.DateCreated);
    //                    $('#general-purchase-item-info').modal('show');
    //                }
    //                else if (entity.ShippingItem != null && entity.ShippingItem.Type == "Shipping") {
    //                    sEntity = entity.ShippingItem;;

    //                    $("#sItemSenderName").html(sEntity.SenderName);
    //                    $("#sItemSenderPhoneNumber").html(sEntity.SenderPhoneNumber);
    //                    $("#sItemSenderAddress").html(sEntity.SenderAddress);
    //                    $("#sItemDescription").html(sEntity.Description);
    //                    $("#sItemWeight").html(sEntity.Weight);
    //                    $("#sItemQuantity").html(sEntity.Quantity);
    //                    $("#sItemServicePrice").html(sEntity.ServicePrice);
    //                    $("#sItemTotalPrice").html(sEntity.TotalPrice);
    //                    $("#sItemStatus").html(sEntity.Status);
    //                    $("#sItemReceiverName").html(sEntity.ReceiverName);
    //                    $("#sItemReceiverNumber").html(sEntity.ReceiverNumber);
    //                    $("#sItemReceiverAddress").html(sEntity.ReceiverAddress);
    //                    $("#sItemDateCreated").html(sEntity.DateCreated);
    //                    $('#general-shipping-item-info').modal('show');
    //                }
    //                else if (entity.PurchaseAndShippingItem != null && entity.PurchaseAndShippingItem.Type == "PurchaseAndShipping") {
    //                    psEntity = entity.PurchaseAndShippingItem;;

    //                    $("#psItemDescription").html(psEntity.Description);
    //                    $("#psItemUnitPrice").html(psEntity.UnitPrice);
    //                    $("#psItemQuantity").html(psEntity.Quantity);
    //                    $("#psItemServicePrice").html(psEntity.ServicePrice);
    //                    $("#psItemTotalPrice").html(psEntity.TotalPrice);
    //                    $("#psItemProductLink").html(psEntity.ProductLink);
    //                    $("#psItemVendor").html(psEntity.VendorName);
    //                    $("#psItemStatus").html(psEntity.Status);
    //                    $("#psItemReceiverName").html(psEntity.ReceiverName);
    //                    $("#psItemReceiverNumber").html(psEntity.ReceiverNumber);
    //                    $("#psItemReceiverAddress").html(psEntity.ReceiverAddress);
    //                    $("#psItemDateCreated").html(psEntity.DateCreated);
    //                    $('#general-purchase-shipping-item-info').modal('show');
    //                }
    //                return;
    //            } else {
    //                $(".Main-loader").hide();
    //                Swal.fire(result.Message);
    //                return;
    //            }
    //        }
    //    });
    //});
});