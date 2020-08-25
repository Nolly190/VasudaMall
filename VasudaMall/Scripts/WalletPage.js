$(document).ready(function () {

    //$("#withdrawalAccounttr").on( 'click',"#setWithdrawalAccountButton",function () {
    $("#setWithdrawalAccountButton").click(function () {
        if (StartValidation("setWithdrawalAccountForm")) {
            var accountInfo = {};
            accountInfo.AccountNumber = $("#accountNumber").val();
            accountInfo.BankName = $("#banksList").val();
            accountInfo.AccountName = $("#accountName").val();
            $(".Main-loader").show();
            $.ajax({
                url: "/Dashboard/AddAccount",
                type: "Post",
                data: { model: accountInfo },
                error: function(status, xhr) {
                    $(".Main-loader").hide();
                },
                success: function(result) {

                    $(".Main-loader").hide();
                    if (result.Status === true) {
                        Swal.fire('Successfully Created');
                            $("#withdrawalAccounttr").empty();
                            $("#withdrawalAccountsList").empty();
                            var selectList = "";
                            var trList = "";
                            $.each(result._entity,
                                function (index, row) {
                                    trList = trList +
                                        ` <tr>
                                                            <td>${index + 1}</td>
                                                            <td>${row.AccountName}</td>
                                                            <td>${row.AccountNumber}</td>
                                                            <td>${row.BankName}</td>
                                                            <td><button data-id="${row.Id}" class="btn btn-danger removeCard">Delete</button></td>
                                                        </tr>`;
                                    selectList =
                                        selectList +
                                        `<option value='${row.AccountNumber}'>${row.AccountNumber} ${row.BankName}</option>`;
                                });
                            $("#withdrawalAccounttr").append(trList);
                        $("#withdrawalAccountsList").append(selectList);

                        return;
                    } 
                    Swal.fire(result.Message);
                }

            });
        }
        return;
    });

    $(".removeCard").click(function () {
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
                $(".Main-loader").show();
                var id = $(this).data('id');
                $.ajax({
                    url: "/Dashboard/DeleteAccount",
                    type: "Post",
                    data: { id: id },
                    error: function(status, xhr) {
                        $(".Main-loader").hide();
                    },
                    success: function(result) {
                        $(".Main-loader").hide();
                        if (result.Status === true) {
                            $("#withdrawalAccounttr").empty();
                            $("#withdrawalAccountsList").empty();
                            var selectList = "`<option value='0'>Select Withdrawal Account</option>";
                            var trList = "";
                            $.each(result._entity,
                                function(index, row) {
                                    trList = trList +
                                        ` <tr>
                                                            <td>${index + 1}</td>
                                                            <td>${row.AccountName}</td>
                                                            <td>${row.AccountNumber}</td>
                                                            <td>${row.BankName}</td>
                                                            <td><button data-id="${row.Id}" class="btn btn-danger removeCard">Delete</button></td>
                                                        </tr>`;
                                    selectList =
                                        selectList +
                                        `<option value='${row.AccountNumber}'>${row.AccountNumber} ${row.BankName}</option>`;
                                });
                            $("#withdrawalAccounttr").append(trList);
                            $("#withdrawalAccountsList").append(selectList);
                            return;
                        }
                        Swal.fire(result.Message);
                    }

                });


            }
        });
    });

    $("#accountNumber").keyup(function () {
        var accountNum = $("#accountNumber").val();
        var bank = $("#banksList").val();
        if (accountNum.length === 10 && bank !== "") {
            var details = {};
            details.AccountNumber = accountNum;
            details.BankName = bank;
            $.ajax({
                url: "/Dashboard/ResolveAccount",
                type: "Post",
                data: { model: details },
                error: function (status, xhr) {
                    $(".Main-loader").hide();
                },
                success: function (result) {
                    $(".Main-loader").hide();
                    if (result.Status === true) {
                        $("#accountName").val(result._entity);
                        return;
                    }
                    Swal.fire(result.Message);
                }

            });
        }

    });

    $(".convert").keyup(function () {
        var ansa = $(this).data('baltxt');
        var result = parseFloat($(this).val()) / parseFloat($("#exchangeRate").val());
        if (parseInt(result) >= 1) {

            $("#"+ansa).val(result.toFixed(2));
            return;
        }
        
        $("#" + ansa).val("0");
        return;
    });

    $("#SystemAccount").change(function() {
        if ($("#SystemAccount").val() !== "") {
            var getBankInfo = $("#SystemAccount").val().split("||");
            $("#bankName").text("Bank: "+ getBankInfo[0]);
            $("#SystemaccountName").text("Account Name: " +getBankInfo[1]);
            $("#SystemaccountNumber").text("Account Number: " +getBankInfo[2]);
            $("#SystemBankDiv").fadeIn(750);
        } 

    });

    $("#PaymentChannel").change(function() {
        if ($("#PaymentChannel").val() === "Online") {
            $("#BankDiv").fadeOut(750);
            $("#PamentNarration").fadeOut(750);

            $("#Narration").removeAttr("required");
            $("#Narration").removeAttr("data-formname");
            $("#SystemAccount").removeAttr("required");
            $("#SystemAccount").removeAttr("data-formname");
        }
        else if ($("#PaymentChannel").val() === "BankDeposit") {
            $("#BankDiv").fadeIn(750);
            $("#PamentNarration").fadeIn(750);
            $("#Narration").attr("required","required");
            $("#Narration").attr("data-formname","Payment Narration");
            $("#SystemAccount").attr("required","required");
            $("#SystemAccount").attr("data-formname","bank");
        } 

    });

    function CallFlutterwave (customerEmail,phone,name, publicKey, amt, paymentId) {
            FlutterwaveCheckout({
                public_key: publicKey,
                tx_ref: paymentId,
                amount: amt,
                currency: "NGN",
                payment_options: "card, mobilemoneyghana, ussd",
                redirect_url: "https://localhost:44312/Payment",
                customer: {
                    email: customerEmail,
                    phone_number: phone,
                    name: name,
                },
                callback: function(data) {
                    console.log(data);
                },
                customizations: {
                    title: "Vasuda Mall",
                    description: "Wallet funding",
                    logo: "https://vasudamall.com/images/logo.png"
                },
        });
    };


    $("#FundBtn").click(function () {
        if (StartValidation("fundingForm")) {
            $(".Main-loader").show();
            var details = {};
            details.NairaAmount = $("#fundingAmount").val();
            details.PaymentType = $("#PaymentChannel").val();
            details.SystemAccount = $("#SystemAccount").val().split("||")[3];
            details.TransactionNarration = $("#Narration").val();
            $.ajax({
                url: "/Dashboard/FundRequest",
                type: "Post",
                data: { model: details },
                error: function (status, xhr) {
                    $(".Main-loader").hide();
                },
                success: function (result) {
                    $(".Main-loader").hide();
                    if (result.Status) {
                        if ($("#PaymentChannel").val() === "Online") {
                            var paymentDetails = result._entity;
                            CallFlutterwave(paymentDetails.Email, paymentDetails.Phone, paymentDetails.Name, paymentDetails.ApiKey, paymentDetails.Amount, paymentDetails.PaymentId);
                        }
                    }
                    
                }

            });
        }

       

    });
    $("#SubmitRequest").click(function () {
        if (StartValidation("makeWithdrawalForm")) {
            $(".Main-loader").show();
            var details = {};
            details.Amount = $("#withdrawalAmount").val();
            details.WithdrawalAccount = $("#withdrawalAccountsList").val();
            details.Rate = $("#exchangeRate").val();
            $.ajax({
                url: "/Dashboard/WithdrawalRequest",
                type: "Post",
                data: { model: details },
                error: function (status, xhr) {
                    $(".Main-loader").hide();
                },
                success: function (result) {
                    $(".Main-loader").hide();
                    Swal.fire(result.Message);
                }

            });
        }

       

    });


});