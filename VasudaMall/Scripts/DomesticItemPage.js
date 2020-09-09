$(document).ready(function () {

    $("#addDomesticItemBtn").click(function () {
        if (StartValidation("addDomesticItemForm")) {
            $(".Main-loader").show();
            var domesticItemData = {};
            domesticItemData.SenderName = $("#senderName").val();
            domesticItemData.SenderPhoneNumber = $("#senderPhoneNumber").val();
            domesticItemData.SenderAddress = $("#senderAddress").val();
            domesticItemData.Title = $("#title").val();
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
                        $("#title").val("");
                        $("#description").val("");
                        $("#quantity").val("");
                        $("#weight").val("");
                        $("#receiverName").val("");
                        $("#receiverPhoneNumber").val("");
                        $("#receiverAddress").val("");
                        Swal.fire('Domestic order added Successfully.').then(
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
        return;
    })

    //Accept Domestic order
    $("#acceptDomesticItemModalBtn").click(function () {
        var totalPrice = $('#dItemTotalPrice').html();
        var id = $('#collectId').html();

        Swal.fire({
            title: `Do you accept the initial payment price of $${totalPrice} to process and create order for this item?`,
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, I Accept!'
        }).then((result) => {
            if (result.value) {
                $(".Main-loader").show();
                $.ajax({
                    url: "/Item/ProcessDomestic",
                    type: "Post",
                    data: { id: id, action: "Accepted" },
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
});