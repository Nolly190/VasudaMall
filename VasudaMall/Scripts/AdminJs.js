$(document).ready(function () {


    $("#SaveProducts").click(function () {
        if (StartValidation("ProductForm")) {


            var formfield = new FormData();
            var getfiles = $("#Images").get(0);
            var docFiles = getfiles.files;
            $.each(docFiles, function (index, row) {
                formfield.append("File" + index, row);
            });
            formfield.append("Title ", $("#title").val());
            formfield.append("Price ", $("#price").val());
            formfield.append("Description ", $("#Description").val());
            formfield.append("IsPopular ", $("#IsPopular").val());
            formfield.append("Clearance ", $("#Clearance").val());
            formfield.append("Quantity ", $("#Qty").val());
            formfield.append("Location ", $("#Location").val());
            formfield.append("Category ", $("#ProdCate").val());
            formfield.append("SubCategory ", $("#ProdSubCate").val());
            var url = "/Admin/AddProducts";

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
        if ($("#CatName").val() !== "" || $("#CatName").val() !== null) {

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


    $(".DeleteProduct").click(function () {


        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.value) {
                $(".Main-loader").modal('show');
                $.ajax({
                    url: "/Admin/DeleteProduct",
                    type: "Post",
                    data: { id: $(this).data('id') },
                    error: function (status, xhr) {
                        $(".Main-loader").modal('hide');
                    },
                    success: function (result) {

                        $(".Main-loader").modal('hide');
                        if (result.Status === true) {
                            Swal.fire('Deleted');
                            window.setTimeout(function () { location.reload(); }, 3000);
                            return;
                        }
                        Swal.fire(result.Message);
                    }

                });
                return;
            }
        });
    });

    $(".DeleteVendor").click(function () {

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.value) {
                $(".Main-loader").modal('show');
                $.ajax({
                    url: "/Admin/DeleteProduct",
                    type: "Post",
                    data: { id: $(this).data('id') },
                    error: function (status, xhr) {
                        $(".Main-loader").modal('hide');
                    },
                    success: function (result) {

                        $(".Main-loader").modal('hide');
                        if (result.Status === true) {
                            Swal.fire('Deleted');
                            window.setTimeout(function () { location.reload(); }, 3000);
                            return;
                        }
                        Swal.fire(result.Message);
                    }

                });
                return;
            }
        });
    });

    $(".DeleteSubCate").click(function () {

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.value) {
                $(".Main-loader").modal('show');
                $.ajax({
                    url: "/Admin/DeleteSubCategory",
                    type: "Post",
                    data: { id: $(this).data('id') },
                    error: function (status, xhr) {
                        $(".Main-loader").modal('hide');
                    },
                    success: function (result) {

                        $(".Main-loader").modal('hide');
                        if (result.Status === true) {
                            Swal.fire('Deleted');
                            window.setTimeout(function () { location.reload(); }, 3000);
                            return;
                        }
                        Swal.fire(result.Message);
                    }

                });
                return;
            }
        });
    });

    $(".DeleteCategory").click(function () {

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.value) {
                $(".Main-loader").modal('show');
                $.ajax({
                    url: "/Admin/DeleteCategory",
                    type: "Post",
                    data: { id: $(this).data('id') },
                    error: function (status, xhr) {
                        $(".Main-loader").modal('hide');
                    },
                    success: function (result) {

                        $(".Main-loader").modal('hide');
                        if (result.Status === true) {
                            Swal.fire('Deleted');
                            window.setTimeout(function () { location.reload(); }, 3000);
                            return;
                        }
                        Swal.fire(result.Message);
                    }

                });
                return;
            }
        });
    });


    $("#ProdCate").change(function () {
        $.ajax({
            url: "/Admin/GetSubCategory",
            type: "Post",
            data: { categoryName: $("#ProdCate").val() },
            error: function (status, xhr) {
                $(".Main-loader").modal('hide');
            },
            success: function (result) {
                $(".Main-loader").modal('hide');
                if (result.Status === true) {
                    var html = "";
                    $.each(result._entity,
                        function (index, row) {
                            html = html + `  <option value='${row.Id}'>${row.Name}</option>`;
                        });
                    if (result._entity.length > 0) {
                        $("#subCatDropDown").show();
                        $("#ProdSubCate").attr("data-formname", "SubCategory");
                        $("#ProdSubCate").append(html);
                        $("#ProdSubCate").attr("Required", "true");
                    } else {

                        $("#subCatDropDown").hide();
                        $("#ProdSubCate").removeAttr("data-formname", "SubCategory");
                        $("#ProdSubCate").removeAttr("Required", "true");
                    }
                    return;
                }
            }

        });


    });

    $("#EditVendor").click(function () {

        $(".Main-loader").modal('show');
        $.ajax({
            url: "/Admin/EditVendor",
            type: "Post",
            data: { vendor: $("#EditedVendorName").val(), oldVendor: $("#OldVendorName").val(), link: $("#EditedVendorLink").val(), },
            error: function (status, xhr) {
                $(".Main-loader").modal('hide');
            },
            success: function (result) {
                $(".Main-loader").modal('hide');
                if (result.Status === true) {
                    Swal.fire('Successful');
                    window.setTimeout(function () { location.reload(); }, 3000);
                    return;
                }
                Swal.fire(result.Message);
            }

        });
        return;

    });
    $("#EditSubCategory").click(function () {
        $(".Main-loader").modal('show');
        $.ajax({
            url: "/Admin/EditSubCategory",
            type: "Post",
            data: { subCategory: $("#EditSubcategoryName").val(), oldSubCategory: $("#SubId").val() },
            error: function (status, xhr) {
                $(".Main-loader").modal('hide');
            },
            success: function (result) {

                $(".Main-loader").modal('hide');
                if (result.Status === true) {
                    Swal.fire('Successful');
                    window.setTimeout(function () { location.reload(); }, 3000);
                    return;
                }
                Swal.fire(result.Message);
            }

        });
        return;

    });

    //$("#MailType").select(function () {
    //    var mailType = $("#MailType").val();
    //    if (mailType === "Mail") {
    //        $("#")
    //    } else if (mailType === "Text") {

    //    }
    //});
    $("#SendMail").click(function (data) {
        data.preventDefault();
        if ($("#compose-textarea").val() === "" || $("#compose-textarea").val() === null) {
            Swal.fire("Message is empty");
            return;
        }
        if ($("#subject").val() === "" || $("#subject").val() === null) {
            Swal.fire("Subject is empty");
            return;

        }
        if ($("#email").val() === "" || $("#email").val() === null) {
            Swal.fire("Email is empty");
            return;

        }
            $(".Main-loader").modal('show');
            var details = {};
            details.Message = $("#compose-textarea").val();
            details.Subject = $("#subject").val();
            details.Email = $("#email").val();
            $.ajax({
                url: "/Admin/SendMail",
                type: "Post",
                data: { model:details },
                error: function (status, xhr) {
                    $(".Main-loader").modal('hide');
                },
                success: function (result) {

                    $(".Main-loader").modal('hide');
                    if (result.Status === true) {
                        Swal.fire('Successful');
                        $("#DangerAlert").text("Failed Mails " + result._entity);
                        $("#DangerAlert").show();
                        return;
                    }
                    Swal.fire(result.Message);
                }

            });

    });
    $(".banUser").click(function () {

        Swal.fire({
            title: 'Are you sure?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.value) {
                var action = $(this).data("action") === "Ban" ? true : false;
                var id = $(this).data('id');
                $(".Main-loader").modal('show');
                $.ajax({
                    url: "/Admin/BanUser",
                    type: "Post",
                    data: { action: action, id: id },
                    error: function (status, xhr) {
                        $(".Main-loader").modal('hide');
                    },
                    success: function (result) {

                        $(".Main-loader").modal('hide');
                        if (result.Status === true) {
                            Swal.fire('Successful');
                            window.setTimeout(function () { location.reload(); }, 3000);
                            return;
                        }
                        Swal.fire(result.Message);
                    }

                });

            }
        });

    });
    $("#EditCategory").click(function () {
        $(".Main-loader").modal('show');
        $.ajax({
            url: "/Admin/EditCategory",
            type: "Post",
            data: { oldCategory: $("#EditCatName").val(), category: $("#NewCatName").val() },
            error: function (status, xhr) {
                $(".Main-loader").modal('hide');
            },
            success: function (result) {

                $(".Main-loader").modal('hide');
                if (result.Status === true) {
                    Swal.fire('Successful');
                    window.setTimeout(function () { location.reload(); }, 3000);
                    return;
                }
                Swal.fire(result.Message);
            }

        });
        return;

    });



    $(".EditSubCategory").click(function () {


        var getParent = $(this).parent().parent();
        var cateValue = getParent[0].cells[1].innerText;
        $("#EditSubcategoryName").val(cateValue);
        $("#SubId").val(cateValue);
        $("#EditSubCategoryModal").modal("show");
    });



    $(".EditCategory").click(function () {

        var getParent = $(this).parent().parent();
        var Catevalue = getParent[0].cells[1].innerText;
        $("#EditCatName").val(Catevalue);
        $("#NewCatName").val(Catevalue);
        $("#EditCategoryModal").modal("show");
    });



    $(".EditVendor").click(function () {

        var getParent = $(this).parent().parent();
        var Catevalue = getParent[0].cells[1].innerText;
        $("#EditedVendorName").val(Catevalue);
        $("#OldVendorName").val(Catevalue);
        $("#EditedVendorLink").val(getParent[0].cells[1].innerText);
        $("#EditVendorModal").modal("show");
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
    $("#SaveVendor").click(function () {
        if (StartValidation("AddVendorForm")) {

            $(".Main-loader").modal('show');
            var details = {};
            details.Name = $("#VendorName").val();
            details.Link = $("#VendorLink").val();
            $.ajax({
                url: "/Admin/AddVendor",
                type: "Post",
                data: { model: details },
                error: function (status, xhr) {
                    $(".Main-loader").modal('hide');
                },
                success: function (result) {

                    $(".Main-loader").modal('hide');
                    if (result.Status === true) {
                        Swal.fire('Successfully Created');
                        $("#VendorName").val("");
                        $("#VendorLink").val("");
                        return;
                    }
                    Swal.fire(result.Message);
                }

            });
            return;
        }
    });

    $(".action").click(async function () {

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
        }

        else {
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