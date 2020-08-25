$(document).ready(function () {

    $("#addDomesticItemBtn").click(function () {
        if (StartValidation("addDomesticItemForm")) {
            $(".Main-loader").show();
            var domesticItemData = {};
            domesticItemData.SenderName = $("#senderName").val();
            domesticItemData.SenderPhoneNumber = $("#senderPhoneNumber").val();
            domesticItemData.SenderAddress = $("#senderAddress").val();
            domesticItemData.Description = $("#description").val();
            domesticItemData.Quantity = $("#quantity").val();
            domesticItemData.Weight = $("#weight").val();
            domesticItemData.ReceiverName = $("#receiverName").val();
            domesticItemData.ReceiverPhoneNumber = $("#receiverPhoneNumber").val();
            domesticItemData.ReceiverAddress = $("#receiverAddress").val();

            $.ajax({
                url: "/Item/AddDomesticItem",
                type: "Post",
                data: { model: domesticItemData },
                error: function (status, xhr) {
                    $(".Main-loader").hide();
                },
                success: function (result) {
                    if (result.Status === true) {
                        $(".Main-loader").hide();
                        $("#senderName").val("");
                        $("#senderPhoneNumber").val("");
                        $("#senderAddress").val("");
                        $("#description").val("");
                        $("#quantity").val("");
                        $("#weight").val("");
                        $("#receiverName").val("");
                        $("#receiverPhoneNumber").val("");
                        $("#receiverAddress").val("");
                        Swal.fire('Domestic order added Successfully.');
                        //location.reload();

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
    })

    $(".domesticOrdersHistoryTableButton").click(function () {
        var id = $(this).data("id");

        alert(id);
    })
});