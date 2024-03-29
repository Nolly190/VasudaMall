﻿$(document).ready(function () {
    var domesticPriceId = "";
    var itemsList = "";
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

                        window.setTimeout(function () { location.reload(); }, 3000);
                        return;
                    }
                    Swal.fire(result.Message);
                }

            });
            return;
        }



    });


    $("#productTbody").on("click", ".DeleteProduct", function () {


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

    $("#VendorTbody").on("click", ".DeleteVendor", function () {

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

    $("#subCategoryTbody").on("click", ".DeleteSubCate", function () {

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

    $("#categoryTbody").on("click", ".DeleteCategory", function () {

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
                        $("#ProdSubCate").empty();
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
        var newCate = $("#EditedVendorName").val();
        if (newCate === "" || newCate === null) {
            Swal.fire("Vendor Name Field is required");
            return;
        }

        newCate = $("#EditedVendorLink").val();
        if (newCate === "" || newCate === null) {
            Swal.fire("Vendor Link Field is required");
            return;
        }
        $(".Main-loader").modal('show');
        $("#EditSubcategoryName").val()
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
        var newCate = $("#EditSubcategoryName").val();
        if (newCate === "" || newCate === null) {
            Swal.fire("Sub Category Field is required");
            return;
        }
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



    $("#UnfinishedTbody").on("click", ".NextAction", async function () {
     var orderId =   $(this).data("id");
        if ($(this).data("status") === "Processing" || $(this).data("status") ==="AwaitingShippingQuotation") {
         //Get the complete list of order type and check for their status
            const { value: shippingFee } = await Swal.fire({
                title: 'Enter shipping fee for this order',
                input: 'number',
                inputValue: inputValue,
                showCancelButton: true,
                inputValidator: (value) => {
                    if (!value) {
                        return 'Shipping Fee is required'
                    }
                }
            })

            if (shippingFee) {
                $(".Main-loader").modal('show');
                $.ajax({
                    url: "/Admin/ProcessOrder",
                    type: "Post",
                    data: { orderId: orderId , amount: shippingFee },
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
            }
        }
      else {
            Swal.fire({
                title: 'Are you sure?',
                //text: "You won't be able to revert this!",
                //icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes'
            }).then((result) => {
                if (result.value) {
                    $(".Main-loader").modal('show');
                    $.ajax({
                        url: "/Admin/ProcessOrder",
                        type: "Post",
                        data: { orderId: orderId },
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
                }
            })
        }


    });



    $("#AdminTbody").on("click", ".RemoveAdmin", async function () {


        Swal.fire({
            title: 'Are you sure?',
            //text: "You won't be able to revert this!",
            //icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.value) {
                $(".Main-loader").modal('show');
                $.ajax({
                    url: "/Admin/ManageAdmin",
                    type: "Post",
                    data: { userId: $(this).data("id"), makeAdmin: false },
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
            }
        })



    });



    $(".UnfinishedTbody").on("click", ".ViewOrderItems", function () {
        $(".Main-loader").modal('show');
        $.ajax({
            url: "/Admin/GetOrderItems",
            type: "Post",
            data: { orderId: $(this).data("id") },
            error: function (status, xhr) {
                $(".Main-loader").modal('hide');
            },
            success: function (result) {
                $(".Main-loader").modal('hide');
                if (result.Status === true) {
                    var html = "";
                    var data = $("#OrderItemTable").DataTable();
                    data.rows().remove().draw();
                    itemsList = "";
                    itemsList = result._entity.Item;
                    $.each(result._entity.Item,
                        function (index, row) {
                            data.row.add([
                                row.Title,
                                row.Description,
                                row.OrderType,
                                row.Quantity,
                                row.ItemsPrice,
                                row.ServicePrice,
                                row.TotalPrice,
                                row.OrderType === "Domestic" ? `<input type="button" class="btn btn-primary ViewSupplierInfo" data-id="${row.Id}"  data-type="Supplier" value="Supplier Details" />` : `<input type="button" class="btn btn-primary ViewSupplierInfo" data-id="${row.Id}"  data-type="Website" value="Goto Website" />`
                           ]).draw(false);
                        });
                    var orderInfo = result._entity.Order;
                    var user = result._entity;
                    $("#OrderName").text(user.FullName);
                    $("#OrderEmail").text(user.Email);
                    $("#OrderPhone").text(user.PhoneNumber);
                    $("#OrderAddress").text(user.Address);
                    $("#OrderType").text(orderInfo.OrderType);
                    $("#OrderStatus").text(orderInfo.Status);
                    $("#ItemsTableDiv").append(html);
                    $("#ItemsTableDiv").on("click",".ViewSupplierInfo",
                        
                        function () {
                            var id = $(this).data("id");
                            var type = $(this).data("type");
                            $.each(itemsList,
                                function(index,row) {
                                    if (row.Id === id) {
                                        if (type === "Supplier") {
                                            $("#SupName").text(row.SupplierName);
                                            $("#SupPhone").text(row.SupplierPhone);
                                            $("#SupAddress").text(row.SupplierAddress);
                                            $("#ViewSupplierInfo").modal("show");
                                        } else {
                                            window.location.replace(row.ProductLink);
                                        }

                                    }
                                });
                        });
                    $("#OrderModel").modal("show");
                    return;
                }
                Swal.fire(result.Message);
            }

        });
        return;

    });

    $("#CompletedOrderBtn").click(function () {
        $("#CompletedOrderDiv").show();
        $("#PendingOrderDiv").hide();
        $("#DomesticOrders").hide();
    });
    $("#ViewAdmins").click(function () {
        $("#UserDiv").toggle();
        $("#AdminDiv").toggle();
    });
    $("#DomesticOrdersBtn").click(function () {
        $("#CompletedOrderDiv").hide();
        $("#PendingOrderDiv").hide();
        $("#DomesticOrders").show();
    });
    $("#PendingOrders").click(function () {
        $("#CompletedOrderDiv").hide();
        $("#PendingOrderDiv").show();
        $("#DomesticOrders").hide();
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
            data: { model: details },
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
    $("#NotificationTbody").on("click", ".ViewNotification", function (data) {

        $(".Main-loader").modal('show');
        var id = $(this).data("id");
        $.ajax({
            url: "/Admin/ViewNotification",
            type: "Post",
            data: { noteId: id },
            error: function (status, xhr) {
                $(".Main-loader").modal('hide');
            },
            success: function (result) {

                $(".Main-loader").modal('hide');
                if (result.Status === true) {
                    $("#NotificationDiv").html(result._entity.Message);
                    $("#NotificationModal").modal("show");
                    return;
                }
                Swal.fire(result.Message);
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
            url: "/Admin/SendChat",
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
    $(".LoadMsg").click(function (data) {
        data.preventDefault();
        var userId = $(this).data("id");
        $(this).addClass("active");
        $.ajax({
            url: "/Admin/RetrieveChat",
            type: "Post",
            data: { userId: userId },
            error: function (status, xhr) {
                $(".Main-loader").modal('hide');
            },
            success: function (result) {

                $(".Main-loader").modal('hide');
                if (result.Status === true) {
                    var html = '';
                    $("#SubmitChat").attr('data-id', userId);
                    $("#MessageHistory").empty();

                    $.each(result._entity,
                        function (index, row) {
                            var date = new Date();
                            date.setTime(row.DateCreated.split("(")[1].split(")")[0]);
                            date = date.toDateString();
                            $("#SubmitChat").prop('disabled', false);
                            if (row.SentBy === "Admin") {
                                html = html +
                                    `      <div class="outgoing_msg">
                            <div class="sent_msg">
                                <p>
                                ${row.Message}
                                </p>
                                <span class="time_date">${date}</span>
                            </div>
                        </div>
                  `;
                            } else {
                                html = html +
                                    `     <div class="incoming_msg">
                            <div class="incoming_msg_img"> <img src="https://ptetutorials.com/images/user-profile.png" alt="sunil"> </div>
                            <div class="received_msg">
                                <div class="received_withd_msg">
                                    <p>
                                         ${row.Message}
                                    </p>
                                    <span class="time_date">${date}</span>
                                </div>
                            </div>
                        </div>
                  `;
                            }
                        });
                    $("#MessageHistory").append(html);
                }
                Swal.fire(result.Message);
            }

        });

    });
    $("#UserTbody").on("click", ".banUser", function () {

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
    $("#UserTbody").on("click", ".MakeAdmin", function () {


        Swal.fire({
            title: 'Are you sure?',
            //text: "You won't be able to revert this!",
            //icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.value) {
                $(".Main-loader").modal('show');
                $.ajax({
                    url: "/Admin/ManageAdmin",
                    type: "Post",
                    data: { userId: $(this).data("id"), makeAdmin: true },
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
            }
        })


    });
    $("#EditCategory").on("click", function () {
        var newCate = $("#NewCatName").val();
        if (newCate === "" || newCate === null) {
            Swal.fire("Category Field is required");
            return;
        }
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



    $("#subCategoryTbody").on("click", ".EditSubCategory", function () {


        var getParent = $(this).parent().parent();
        var cateValue = getParent[0].cells[1].innerText;
        $("#EditSubcategoryName").val(cateValue);
        $("#SubId").val(cateValue);
        $("#EditSubCategoryModal").modal("show");
    });

    $("#categoryTbody").on("click", ".EditCategory", function () {

        var getParent = $(this).parent().parent();
        var Catevalue = getParent[0].cells[1].innerText;
        $("#EditCatName").val(Catevalue);
        $("#NewCatName").val(Catevalue);
        $("#EditCategoryModal").modal("show");
    });

    $('.modal').on('shown.bs.modal', function () {
        $(document).off('focusin.modal');
    });
    $("#ContactUser").click(async function () {

        const { value: text } = await Swal.fire({
            input: 'textarea',
            inputPlaceholder: 'Type your message here...',
            inputAttributes: {
                'aria-label': 'Type your message here'
            },
            showCancelButton: true,
            inputValidator: (value) => {
                if (!value) {
                    return 'Message is required'
                }
            }
        });
        if (text) {

            $(".Main-loader").modal('show');
            var details = {};
            details.Message = text;
            details.Subject = "Domestic Order Quotation";
            details.Email = $("#DomeEmail").text();
            $.ajax({
                url: "/Admin/SendMail",
                type: "Post",
                data: { model: details },
                error: function (status, xhr) {
                    $(".Main-loader").modal('hide');
                },
                success: function (result) {

                    $(".Main-loader").modal('hide');
                    if (result.Status === true) {
                        Swal.fire('Mail Sent');
                        return;
                    }
                    Swal.fire(result.Message);
                }

            });

        }

    });

    $("#SubmitPrice").click(function () {
        Swal.fire({
            title: 'Are you sure?',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.value) {

                if ($("#DomesticQoutation").val() === "" || $("#DomesticQoutation").val() === null) {
                    Swal.fire("Price field is required");
                    return;
                }
                $(".Main-loader").modal('show');
                $.ajax({
                    url: "/Admin/SendPriceQuotation",
                    type: "Post",
                    data: { amount: $("#DomesticQoutation").val(), id: domesticPriceId },
                    error: function (status, xhr) {
                        $(".Main-loader").modal('hide');
                    },
                    success: function (result) {

                        $(".Main-loader").modal('hide');
                        if (result.Status === true) {
                            Swal.fire('Successful');
                            $("#DomesticQoutation").val("");
                            return;
                        }
                        Swal.fire(result.Message);
                    }

                });

            }
        });


    });

    $("#domesticTbody").on("click", ".DomesticViewMore", function () {
        var id = $(this).data("id");
        $(".Main-loader").modal('show');
        $.ajax({
            url: "/Admin/ViewMoreDomesticInfo",
            type: "Post",
            data: { id: id },
            error: function (status, xhr) {
                $(".Main-loader").modal('hide');
            },
            success: function (result) {

                $(".Main-loader").modal('hide');
                if (result.Status === true) {
                    domesticPriceId = result._entity.id;
                    $("#DomeAddress").text(result._entity.Address);
                    $("#DomeName").text(result._entity.FullName);
                    $("#DomeEmail").text(result._entity.Email);
                    $("#DomePhone").text(result._entity.PhoneNumber);
                    $("#DomeWeight").text(result._entity.Weight);
                    $("#DomeQuantity").text(result._entity.Quantity);
                    $("#DomeReceAddr").text(result._entity.ReceiverAddress);
                    $("#DomeReceName").text(result._entity.ReceiverName);
                    $("#DomeRecePhone").text(result._entity.ReceiverNumber);
                    $("#DomeSuppAddr").text(result._entity.SenderAddress);
                    $("#DomeSuppPhone").text(result._entity.SenderPhoneNumber);
                    $("#DomeSuppName").text(result._entity.SenderName);
                    $("#DomeCreated").text(result._entity.DateCreated);
                    $("#DomesticLargeModel").modal("show");
                    return;
                }
                Swal.fire(result.Message);
            }

        });
        return;

    });

    $("#VendorTbody").on("click", ".EditVendor", function () {

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
    $(".SubmitShippingPrice").click(function (data) {
        data.preventDefault();
        if (StartValidation("ShippingPriceForm")) {

            $(".Main-loader").modal('show');
            var details = {};
            if ($(this).data("type") === "Edit") {
                details.Id = $(this).data("id");
            }
            details.Type = $("#ShippingType").val();
            details.DeliveryTime = $("#Days").val();
            details.Country = $("#Country").val();
            details.MinKg = $("#MinKg").val();
            details.MaxKg = $("#MaxKg").val();
            details.PricePerKg = $("#Price").val();
            $.ajax({
                url: "/Admin/ManagePrice",
                type: "Post",
                data: { model: details },
                error: function (status, xhr) {
                    $(".Main-loader").modal('hide');
                },
                success: function (result) {

                    $(".Main-loader").modal('hide');
                    if (result.Status === true) {
                        Swal.fire('Successfully Created');
                        window.setTimeout(function () { location.reload(); }, 3000);

                        return;
                    }
                    Swal.fire(result.Message);
                }

            });
            return;
        }
    });

    $(".AllRequest").on("click", ".action", async function () {

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
                        window.setTimeout(function () { location.reload(); }, 3000);
                    }
                    Swal.fire(result.Message);
                }

            });
            return;
        }
        details.reason = $("#DeclineReason").val();

    });


    $("#ShippingPriceTbody").on("click", ".DeletePrice", async function () {

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

                var details = {};
                details.Id = $(this).data("id");
                details.PricePerKg = 0;
                $.ajax({
                    url: "/Admin/ManagePrice",
                    type: "Post",
                    data: { model: details },
                    error: function (status, xhr) {
                        $(".Main-loader").modal('hide');
                    },
                    success: function (result) {

                        $(".Main-loader").modal('hide');
                        if (result.Status === true) {
                            Swal.fire('Successfully Created');
                            window.setTimeout(function () { location.reload(); }, 3000);

                            return;
                        }
                        Swal.fire(result.Message);
                    }

                });
            }
        })
    });


    $("#ShippingPriceTbody").on("click", ".EditPrice", async function () {
        var trParent = $(this).parent().parent();
        var elementChildren = $(trParent).children();
        $("#ShippingType").val($(elementChildren[1]).text());
        $("#Days").val($(elementChildren[6]).text());
        $("#Country").val($(elementChildren[5]).text());
        $("#MinKg").val($(elementChildren[2]).text());
        $("#MaxKg").val($(elementChildren[3]).text());
        $("#Price").val($(elementChildren[4]).text());
        $("#ShipPriceEdit").show();
        $("#ShipPriceSave").hide();
        var id = $(this).data("id");
        $("#ShipPriceEdit").attr("data-id", id);
        $('html, body').animate({
            scrollTop: $("#NavigateToSection").offset().top
        });
    })


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