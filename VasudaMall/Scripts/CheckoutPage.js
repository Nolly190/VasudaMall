$(document).ready(function () {

    var allSelected = [];

    $('.checkoutAllItemCheckBtn').click(function () {
        
        if ($(this).is(':checked')) {
            $('input:checkbox').prop('checked', true);
        } else {
            $('input:checkbox').prop('checked', false);
            $('#createOrderBtnRow').css('display', 'none');
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

    //Check if any of the checkboxes have been selected;
    $('#checkoutItemTable input[type=checkbox]').on('change', function () {
        var len = jQuery('#checkoutItemTable input[type=checkbox]:checked').length;
        if (len > 0) {
            $('#createOrderBtnRow').css('display', 'table-row');
        } else if (len <= 0) {
            $('#createOrderBtnRow').css('display', 'none');
        }
    }).trigger('change');

    //initialize the process to create the order
    $('#createOrderFromCheckoutBtn').click(function () {
        var TotalPrice = 0
        var count = 1

        $('#confirmCheckoutItemsTable').empty()
        $('#checkoutItemTable input[type=checkbox]:checked').each(function () {

            var id = $(this).data('id');
            if (id != null || id != undefined) {
                var row = $(this).closest("tr")[0];
                var price = row.cells[10].innerHTML;
                var title = row.cells[1].innerHTML

                TotalPrice += parseFloat(price);

                $('#confirmCheckoutItemsTable').append(
                    `
                        <tr>
                            <td>${count}</td>
                            <td>${title}</td>
                            <td>${price}</td>           
                        </tr>
                    `
                );

                count++;
                allSelected.push(id);
            }
       
        });

        $('#confirmCheckoutItemsTable').append(
            `
            <tr>
                <td colspan="2"><span class="float-right"><b>Total Price: </b></span></td>
                <td>${TotalPrice}</td>         
            </tr>
            `
        );

        $('#items-to-confirm-checkout').modal('show');
    });

    //Complete the process to create order(s)
    $("#confirmCreateOrderFromCheckoutBtn").click(function () {

        $('#items-to-confirm-checkout').modal('hide');
        $(".Main-loader").show();
        $.ajax({
            url: "/Order/CreateGeneralOrder",
            type: "Post",
            data: { model: allSelected },
            error: function (status, xhr) {
                $(".Main-loader").hide();
            },
            success: function (result) {
                if (result.Status === true) {
                    $(".Main-loader").hide();
                    Swal.fire(result.Message).then(
                        (result) => {
                            if (result.value) {
                                location.reload(true);
                            }
                        });

                    return;
                } else {
                    $(".Main-loader").hide();
                    Swal.fire(result.Message);
                    return;
                }
            }
        });
    });

    //Delete an item from checkout
    $(".itemTBody").on("click", ".itemsDeleteBtn", function () {

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
                            Swal.fire(result.Message).then(
                                (result) => {
                                    if (result.value) {
                                        location.reload(true);
                                    }
                                });

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
    });


    //Get a general item details start
    $(".itemTBody").on("click", ".itemsViewMoreButton" ,function () {
        $(".Main-loader").show();

        var id = $(this).data('id');
        $.ajax({
            url: "/Item/GetSingleItem",
            type: "Post",
            data: { id: id },
            error: function (status, xhr) {
                $(".Main-loader").hide();
                Swal.fire("Please Try Again!")
            },
            success: function (result) {
                if (result.Status === true) {
                    $(".Main-loader").hide();
                    entity = result._entity;

                    if (entity.PurchaseItem != null && entity.PurchaseItem.Type == "Purchase") {
                        pEntity = entity.PurchaseItem;
                        pDes = pEntity.Description == null || pEntity.Description.length == 0  ? "N/A" : pEntity.Description;

                        $("#pItemDescription").html(pDes);
                        $("#pTitle").html(pEntity.Title);
                        $("#pItemUnitPrice").html(pEntity.UnitPrice);
                        $("#pItemQuantity").html(pEntity.Quantity);
                        $("#pItemServicePrice").html(pEntity.ServicePrice);
                        $("#pItemItemPrice").html(pEntity.ItemPrice);
                        $("#pItemTotalPrice").html(pEntity.TotalPrice);
                        $("#pItemProductLink").html(pEntity.ProductLink);
                        $("#pItemVendor").html(pEntity.VendorName);
                        $("#pItemStatus").html(pEntity.Status);
                        $("#pItemDateCreated").html(pEntity.DateCreated);
                        $('#general-purchase-item-info').modal('show');
                    }
                    else if (entity.ShippingItem != null && entity.ShippingItem.Type == "Shipping") {
                        sEntity = entity.ShippingItem;;
                        sDes = sEntity.Description == null || sEntity.Description.length == 0  ? "N/A" : sEntity.Description;

                        $("#sItemSenderName").html(sEntity.SenderName);
                        $("#sItemSenderPhoneNumber").html(sEntity.SenderPhoneNumber);
                        $("#sItemSenderAddress").html(sEntity.SenderAddress);
                        $("#sTitle").html(sEntity.Title);
                        $("#sItemDescription").html(sDes);
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
                        psDes = psEntity.Description == null || psEntity.Description.length == 0  ? "N/A" : psEntity.Description;

                        $("#psItemDescription").html(psDes);
                        $("#psTitle").html(psEntity.Title);
                        $("#psItemUnitPrice").html(psEntity.UnitPrice);
                        $("#psItemQuantity").html(psEntity.Quantity);
                        $("#psItemServicePrice").html(psEntity.ServicePrice);
                        $("#psItemItemPrice").html(psEntity.ItemPrice);
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
                        dEntity = entity.DomesticItem;
                        dDes = dEntity.Description == null || dEntity.Description.length == 0 ? "N/A" : dEntity.Description;

                        if (dEntity.Status == "Awaiting User Acceptance")
                        {
                            $('#acceptDomesticItemModalBtn').css('display', 'inline-block');
                            $('#rejectDomesticItemModalBtn').css('display', 'inline-block');

                            $('#collectId').html(id);
                        }
                        else if (dEntity.Status == "Rejected Quotation") {
                            $('#acceptDomesticItemModalBtn').css('display', 'inline-block');

                            $('#collectId').html(id);
                        }
                        else if (dEntity.Status == "Awaiting Shipping Payment") {
                            $('#acceptShippingPaymentModalBtn').css('display', 'inline-block');

                            $('#collectId').html(id);
                        }
                        else {
                            $('#closeDomesticItemModalBtn').css('display', 'inline-block');
                        }

                        $("#dItemSenderName").html(dEntity.SenderName);
                        $("#dItemSenderPhoneNumber").html(dEntity.SenderPhoneNumber);
                        $("#dItemSenderAddress").html(dEntity.SenderAddress);
                        $("#dTitle").html(dEntity.Title);
                        $("#dItemDescription").html(dDes);
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
    $("#SubmitChat").click(function (data) {
        data.preventDefault();
        if ($("#ChatBox").val() === "" || $("#ChatBox").val() === null) {
            Swal.fire("Message is empty");
            return;
        }
        $(".Main-loader").modal('show');
        $.ajax({
            url: "/Dashboard/SendChat",
            type: "Post",
            data: { userId: $(this).data("id"), message: $("#ChatBox").val() },
            error: function (status, xhr) {
                $(".Main-loader").modal('hide');
            },
            success: function (result) {

                $(".Main-loader").modal('hide');
                if (result.Status === true) {
                    var d = new Date();
                    var html = `<div class="outgoing_msg">
                            <div class="sent_msg">
                                <p>
                                ${ $("#ChatBox").val()}
                                </p>
                                <span class="time_date">${d.toDateString()}</span>
                            </div>
                        </div>
                  `;
                    $("#MessageHistory").append(html);
                    $("#ChatBox").val("");
                    return;
                }
                Swal.fire(result.Message);
            }

        });

    });

})