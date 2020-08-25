$(document).ready(function () {


    $('.checkoutAllItemCheckBtn').click(function () {
        if ($(this).is(':checked')) {
            $('input:checkbox').prop('checked', true);
        } else {
            $('input:checkbox').prop('checked', false);
        }
    });

    $("input[type='checkbox'].checkoutItemCheckBtn").change(function () {
        var a = $("input[type='checkbox'].checkoutItemCheckBtn");
        if (a.length == a.filter(":checked").length) {
            $('.checkoutAllItemCheckBtn').prop('checked', true);
        }
        else {
            $('.checkoutAllItemCheckBtn').prop('checked', false);
        }
    });


    //Delete an item from checkout
    $(".itemsDeleteBtn").click(function () {

        Swal.fire({
            title: 'Are you sure you want to delete this item?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.value) {
                $(".Main-loader").show();
                var id = $(this).data('id');
                $.ajax({
                    url: "/Item/DeleteItem",
                    type: "Post",
                    data: { id: id },
                    error: function (status, xhr) {
                        $(".Main-loader").hide();
                    },
                    success: function (result) {
                        if (result.Status === true) {
                            $(".Main-loader").hide();
                            location.reload(true);
                            Swal.fire(result.Message);

                            return;
                        } else {
                            $(".Main-loader").hide();
                            Swal.fire(result.Message);
                            return;
                        }
                    }
                });
            }
        });
    })

    //Get a general item details start
    $(".itemsViewMoreButton").click(function () {
        $(".Main-loader").show();

        var id = $(this).data('id');
        $.ajax({
            url: "/Item/GetSingleItem",
            type: "Post",
            data: { id: id },
            error: function (status, xhr) {
                $(".Main-loader").hide();
                alert(status);
                console.log(status.responseText);
                console.log(xhr);
            },
            success: function (result) {
                if (result.Status === true) {
                    $(".Main-loader").hide();
                    entity = result._entity;

                    if (entity.PurchaseItem != null && entity.PurchaseItem.Type == "Purchase") {
                        pEntity = entity.PurchaseItem;

                        $("#pItemDescription").html(pEntity.Description);
                        $("#pItemUnitPrice").html(pEntity.UnitPrice);
                        $("#pItemQuantity").html(pEntity.Quantity);
                        $("#pItemServicePrice").html(pEntity.ServicePrice);
                        $("#pItemTotalPrice").html(pEntity.TotalPrice);
                        $("#pItemProductLink").html(pEntity.ProductLink);
                        $("#pItemVendor").html(pEntity.VendorName);
                        $("#pItemStatus").html(pEntity.Status);
                        $("#pItemDateCreated").html(pEntity.DateCreated);
                        $('#general-purchase-item-info').modal('show');
                    }
                    else if (entity.ShippingItem != null && entity.ShippingItem.Type == "Shipping") {
                        sEntity = entity.ShippingItem;;

                        $("#sItemSenderName").html(sEntity.SenderName);
                        $("#sItemSenderPhoneNumber").html(sEntity.SenderPhoneNumber);
                        $("#sItemSenderAddress").html(sEntity.SenderAddress);
                        $("#sItemDescription").html(sEntity.Description);
                        $("#sItemWeight").html(sEntity.Weight);
                        $("#sItemQuantity").html(sEntity.Quantity);
                        $("#sItemServicePrice").html(sEntity.ServicePrice);
                        $("#sItemTotalPrice").html(sEntity.TotalPrice);
                        $("#sItemStatus").html(sEntity.Status);
                        $("#sItemReceiverName").html(sEntity.ReceiverName);
                        $("#sItemReceiverNumber").html(sEntity.ReceiverNumber);
                        $("#sItemReceiverAddress").html(sEntity.ReceiverAddress);
                        $("#sItemDateCreated").html(sEntity.DateCreated);
                        $('#general-shipping-item-info').modal('show');
                    }
                    else if (entity.PurchaseAndShippingItem != null && entity.PurchaseAndShippingItem.Type == "PurchaseAndShipping") {
                        psEntity = entity.PurchaseAndShippingItem;;

                        $("#psItemDescription").html(psEntity.Description);
                        $("#psItemUnitPrice").html(psEntity.UnitPrice);
                        $("#psItemQuantity").html(psEntity.Quantity);
                        $("#psItemServicePrice").html(psEntity.ServicePrice);
                        $("#psItemTotalPrice").html(psEntity.TotalPrice);
                        $("#psItemProductLink").html(psEntity.ProductLink);
                        $("#psItemVendor").html(psEntity.VendorName);
                        $("#psItemStatus").html(psEntity.Status);
                        $("#psItemReceiverName").html(psEntity.ReceiverName);
                        $("#psItemReceiverNumber").html(psEntity.ReceiverNumber);
                        $("#psItemReceiverAddress").html(psEntity.ReceiverAddress);
                        $("#psItemDateCreated").html(psEntity.DateCreated);
                        $('#general-purchase-shipping-item-info').modal('show');
                    }
                    else if (entity.DomesticItem != null && entity.DomesticItem.Type == "Domestic") {
                        dEntity = entity.DomesticItem;;

                        $("#dItemSenderName").html(dEntity.SenderName);
                        $("#dItemSenderPhoneNumber").html(dEntity.SenderPhoneNumber);
                        $("#dItemSenderAddress").html(dEntity.SenderAddress);
                        $("#dItemDescription").html(dEntity.Description);
                        $("#dItemWeight").html(dEntity.Weight);
                        $("#dItemQuantity").html(dEntity.Quantity);
                        $("#dItemServicePrice").html(dEntity.ServicePrice);
                        $("#dItemTotalPrice").html(dEntity.TotalPrice);
                        $("#dItemStatus").html(dEntity.Status);
                        $("#dItemReceiverName").html(dEntity.ReceiverName);
                        $("#dItemReceiverNumber").html(dEntity.ReceiverNumber);
                        $("#dItemReceiverAddress").html(dEntity.ReceiverAddress);
                        $("#dItemDateCreated").html(dEntity.DateCreated);
                        $('#domestic-item-info').modal('show');
                    }
                    return;
                } else {
                    $(".Main-loader").hide();
                    Swal.fire(result.Message);
                    return;
                }
            }
        });
    });
})