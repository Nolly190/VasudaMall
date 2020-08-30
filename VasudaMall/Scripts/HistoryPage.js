import { Swal } from "../admin-lte/plugins/sweetalert2/sweetalert2";

$(document).ready(function () {
    //Get a general order details start
    $("#orderHistoryViewMoreBtn").click(function () {
        $(".Main-loader").show();

        var id = $(this).data('id');
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

                    $("#oType").html(entity.Type);
                    $("#oStatus").html(entity.Status);
                    $("#oDateCreated").html(entity.DateCreated);
                    $("#oShipppingFee").html(entity.ShippingFee);
                    $("#oServicePrice").html(entity.TotalServiceCharge);
                    $("#oTotalPrice").html(entity.TotalPrice);
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