$(document).ready(function () {
 
    $("#container_loader").modal('show');
    $(".loadMore").on("click",
        function() {
            var category = $(this).data("cate");
            var size = $("." + category).length;
            $.ajax({
                url: url,
                type: "Post",
                data: { size: size, category:category},
                error: function (status, xhr) {
                    $("#container_loader").hide();
                },
                success: function (result) {

                    $("#container_loader").hide();
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
});