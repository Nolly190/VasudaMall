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
});