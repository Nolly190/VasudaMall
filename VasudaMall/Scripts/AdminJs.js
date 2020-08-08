$(document).ready(function () {
 
 
    $("#SaveProducts").click(function () {
        if (StartValidation("ProductForm")) {


                    var formfield = new FormData();
                    var getfiles = $("#Images").get(0);
            var docFiles = getfiles.files;
            $.each(docFiles, function(index,row) {
                formfield.append("File"+index, row);
            });
                    formfield.append("Title ", $("#title").val());
                    formfield.append("Price ", $("#price").val());
                    formfield.append("Description ", $("#Description").val());
                    formfield.append("IsPopular ", $("#IsPopular").val());
                    formfield.append("Clearance ", $("#Clearance").val());
                    formfield.append("Quantity ", $("#Qty").val());
                    formfield.append("Location ", $("#Location").val());
            var url =  "/Admin/AddProducts";

                    $(".Main-loader").modal('show');
                    $.ajax({
                        url: url,
                        error: function (status, xhr) {
                            $(".Main-loader").modal('hide');
                        },
                        type: "Post",
                        processData: false,
                        contentType: false,
                        data: formfield,
                        success: function (result) {

                            $(".Main-loader").modal('hide');
                            if (result.Status === true) {
                                Swal.fire("Product Added");
                                window.setTimeout(function () { location.reload(); }, 3000);
                                return;
                            }
                            Swal.fire(result.Message);
                            return;
                        }

                    });
                    return;
                }

         

    });

    $("#SaveCategory").click(function () {
        if ($("#CatName").val()!==""||$("#CatName").val()!==null) {

            $(".Main-loader").modal('show');
            var details = {};
            details.CategoryName = $("#CatName").val();
            $.ajax({
                url: "/Admin/AddCategory",
                type: "Post",
                data: { model: details },
                error: function (status, xhr) {
                    $(".Main-loader").modal('hide');
                },
                success: function (result) {

                    $(".Main-loader").modal('hide');
                    if (result.Status === true) {
                        Swal.fire('Successfully Created');
                        $("#CatName").val("");
                        return;
                    }
                    Swal.fire(result.Message);
                }

            });
                   return;
                }

         

    });


    $("#SaveSubCategory").click(function () {
        if (StartValidation("AddSubCategoryForm")) {

            $(".Main-loader").modal('show');
            var details = {};
            details.Name = $("#SubcategoryName").val();
            details.CategoryId = $("#subCats").val();
            $.ajax({
                url: "/Admin/AddSubCategory",
                type: "Post",
                data: { model: details },
                error: function (status, xhr) {
                    $(".Main-loader").modal('hide');
                },
                success: function (result) {

                    $(".Main-loader").modal('hide');
                    if (result.Status === true) {
                        Swal.fire('Successfully Created');
                        $("#SubcategoryName").val("");
                        $("#subCats").val("");
                        return;
                    }
                    Swal.fire(result.Message);
                }

            });
                   return;
                }

         

    });

    $(".action").click( async function () {

        $(".Main-loader").modal('show');
        var details = {};
        details.Id = $(this).data("id");
        details.Action = $(this).data("action");
        details.Type = $(this).data("type");
        if (details.Action === "decline") {

            $(".Main-loader").modal('hide');
            const { value: txtReason } = await Swal.fire({
                title: 'Enter your Reason',
                input: 'textarea',
                inputValue: "",
                showCancelButton: true,
                inputValidator: (value) => {
                    if (!value) {
                        return 'Enter reason for decline';
                    }
                }
        });

            if (txtReason) {
                $(".Main-loader").modal('show');
                details.Reason = txtReason;
                $.ajax({
                    url: "/Admin/Action",
                    type: "Post",
                    data: { model: details },
                    error: function (status, xhr) {
                        $(".Main-loader").modal('hide');
                    },
                    success: function (result) {

                        $(".Main-loader").modal('hide');
                        if (result.Status === true) {
                            Swal.fire('Completed Successfully');
                            window.setTimeout(function () { location.reload(); }, 3000);
                            return;
                        }
                        Swal.fire(result.Message);
                    }

                });
                return;
            }
        } else {
            $.ajax({
                url: "/Admin/Action",
                type: "Post",
                data: { model: details },
                error: function (status, xhr) {
                    $(".Main-loader").modal('hide');
                },
                success: function (result) {

                    $(".Main-loader").modal('hide');
                    if (result.Status === true) {
                        Swal.fire('Completed Successfully');
                        return;
                    }
                    Swal.fire(result.Message);
                }

            });
            return;
        }
        details.reason = $("#DeclineReason").val();
     
    });


    $("#RequestToggleBtn").click(function () {
        $("#FundRequestDiv").fadeToggle();
        $("#FundHistoryDiv").fadeToggle();
        if ($(this).val() === "View Fund Request") {

            $(this).val("View Fund Request");

        } else {

            $(this).val("View Fund History");
        }
    });
    $("#WithdrawalToggleBtn").click(function () {
        $("#withdrawalRequestDiv").fadeToggle();
        $("#WithdrawalHistoryDiv").fadeToggle();
        if ($(this).val() === "View Withdrawal Request") {
            $(this).val("View Withdrawal Request");
        } else {

            $(this).val("View Withdrawal History");
        }
    });
});