$(document).ready(function () {
 
    $(".loadMore").on("click",
        function() {
            var category = $(this).data("cate");
            var size = $("." + category).length;
            $.ajax({
                url: url,
                type: "Post",
                data: { size: size, category:category},
                error: function (status, xhr) {
                    $(".Main-loader").hide();
                },
                success: function (result) {

                    $(".Main-loader").hide();
                    if (result.status === "Successful") {
                        Swal.fire({
                            icon: 'success',
                            title: 'Successfully Saved ',
                            showConfirmButton: false,
                            timer: 2500
                        });
                        $("#VerifyDetails").css("display", "none");
                        $("#LoadLodge").attr("disabled", false);
                        $("#formNum").val("");
                        return;
                    }

                    else if (result.status === undefined || result.status === "Refresh the page") {
                        window.setTimeout(function () { location.reload(); }, 3000);
                        return;
                    }
                    Swal.fire({
                        icon: 'error',
                        title: result.status,
                        showConfirmButton: false,
                        timer: 2500
                    });
                }

            });
        });
    $("#ResendVerificationLink").click(function() {
        $(".Main-loader").modal('show');
        $.ajax({
            url: "/Account/ResendLink",
            type: "Get",
            error: function (status, xhr) {
                $(".Main-loader").hide();
            },
            success: function (result) {

                $(".Main-loader").hide();
                $(".modal-backdrop").removeClass("modal-backdrop show");
                if (result.Status === true) {
                    Swal.fire('Email sent successfully');
                    return;
                }

                else if (result.status === undefined || result.status === "Refresh the page") {
                    window.setTimeout(function () { location.reload(); }, 3000);
                    return;
                }
                Swal.fire({
                    icon: 'error',
                    title: result.Message,
                });
            }

        });

    });

    $("#Submit").click(function () {
        if (StartValidation("ContactUsForm")) {

            $(".Main-loader").modal('show');
            var details = {};
            details.Fullname = $("#fullName").val();
            details.Address = $("#address").val();
            details.Phone = $("#number").val();
            details.Message = $("#message").val();
            $.ajax({
                url: "/Home/AddContact",
                type: "Post",
                data: { model: details },
                error: function (status, xhr) {
                    $(".Main-loader").hide();
                },
                success: function (result) {

                    $(".Main-loader").hide();
                    $(".modal-backdrop").removeClass("modal-backdrop show");
                    if (result.Status === true) {
                        Swal.fire('Successfully Sent');
                         $("#fullName").val("");
                         $("#address").val("");
                         $("#number").val("");
                         $("#message").val("");
                        return;
                    }

                    else if (result.status === undefined || result.status === "Refresh the page") {
                        window.setTimeout(function () { location.reload(); }, 3000);
                        return;
                    }
                    Swal.fire({
                        icon: 'error',
                        title: result.Message,
                    });
                }

            });

        }
        return;

    });


    $(".addProductBtn").click(function () {

        $(".Main-loader").show();
        var id = $(this).data("id");
        var quantity = $(this).parent().children().find(".productQuantity").val();

        $.ajax({
            url: "/Item/AddProductItem",
            type: "Post",
            data: { id: id, quantity: quantity },
            error: function (status, xhr) {
                $(".Main-loader").hide
                Swal.fire("Please Try Again!")
            },
            success: function (result) {
                if (result.Status === true) {
                    $(".Main-loader").hide();
                    $(this).parent().children().find(".productQuantity").val(1);
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
    })
});