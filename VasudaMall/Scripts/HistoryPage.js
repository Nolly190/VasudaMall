
$(document).ready(function () {


    //Get a general order details start
    $(".orderHistoryViewMoreBtn").click(function () {
        $(".Main-loader").show();

        var id = $(this).data('id');
        var totalPrice = $(this).data('total');
        $.ajax({
            url: "/Order/GetSingleOrder",
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
                    var count = 1;



                    if (entity.PurchaseOrders != null && entity.PurchaseOrders.length >= 0) {
                        $('#purchaseOrderDetailsTable').empty()
                        pOrders = entity.PurchaseOrders;
                        if (pOrders.length <= 0) {
                            $('#purchaseOrderDetailsTable').append(
                                `
                            <tr>
                                <td colspan="6" class="text-center"><h1>NO ITEM AVAILABLE</h1></td>      
                            </tr>
                            `
                            );
                        } else {
                            $.each(pOrders, function (index, order) {
                                pDes = order.Description == null || order.Description.length == 0 ? "N/A" : order.Description;

                                $('#purchaseOrderDetailsTable').append(
                                    `
                                    <tr>
                                        <td>${count}</td>
                                        <td>${order.Title}</td>
                                        <td>${pDes}</td>
                                        <td>${order.ProductLink}</td>           
                                        <td>${order.Quantity}</td>           
                                        <td>${order.UnitPrice}</td>           
                                    </tr>
                                    `
                                );
                                count++;
                            });

                            $('#purchaseOrderDetailsTable').append(
                                `
                            <tr>
                                <td colspan="6" class="text-center"><b>Order Total Price: </b>$${totalPrice}</td>      
                            </tr>
                            `
                            );
                        }

                        $('#purchase-order-history-info').modal('show');
                    }
                    if (entity.PurchaseAndShippingOrders != null) {
                        $('#purchaseAndShippingOrderDetailsTable').empty()
                        psOrders = entity.PurchaseAndShippingOrders;
                        if (psOrders.length <= 0) {
                            $('#purchaseAndShippingOrderDetailsTable').append(
                                `
                            <tr>
                                <td colspan="7" class="text-center"><h1>NO ITEM AVAILABLE</h1></td>      
                            </tr>
                            `
                            );
                        } else {
                            $.each(psOrders, function (index, order) {
                                pDes = order.Description == null || order.Description.length == 0 ? "N/A" : order.Description;

                                $('#purchaseAndShippingOrderDetailsTable').append(
                                    `
                                    <tr>
                                        <td>${count}</td>
                                        <td>${order.Title}</td>
                                        <td>${pDes}</td>
                                        <td>${order.ProductLink}</td>           
                                        <td>${order.Quantity}</td>           
                                        <td>${order.UnitPrice}</td>           
                                        <td>${order.ReceiverName}</td>           
                                    </tr>
                                    `
                                );
                                count++;
                            });

                            $('#purchaseAndShippingOrderDetailsTable').append(
                                `
                            <tr>
                                <td colspan="7" class="text-center"><b>Order Total Price: </b>$${totalPrice}</td>      
                            </tr>
                            `
                            );
                        }

                        $('#purchase-shipping-order-history-info').modal('show');
                    }
                    if (entity.DomesticOrder != null) {
                        $('#domesticOrderDetailsTable').empty()
                        dOrder = entity.DomesticOrder;

                        pDes = dOrder.Description == null || dOrder.Description.length == 0 ? "N/A" : dOrder.Description;

                        $('#domesticOrderDetailsTable').append(
                            `
                                    <tr>
                                        <td>${dOrder.Title}</td>
                                        <td>${pDes}</td>
                                        <td>${dOrder.Quantity}</td>           
                                        <td>${dOrder.Weight}</td>           
                                        <td>${dOrder.ReceiverName}</td>           
                                        <td>${dOrder.Status}</td>           
                                    </tr>
                                    `
                        );

                        $('#domesticOrderDetailsTable').append(
                            `
                            <tr>
                                <td colspan="7" class="text-center"><b>Order Total Price: </b>$${totalPrice}</td>      
                            </tr>
                            `
                        );

                        $('#domestic-order-history-info').modal('show');
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