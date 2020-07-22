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

    $("#NairaAmount").keyup(function () {

        var result = parseFloat($("#NairaAmount").val()) / parseFloat($("#exchangeRate").val());
        if (parseInt(result) >= 1) {

            $("#withdrawalAmount").val(result.toFixed(2));
            return;
        }

        $("#withdrawalAmount").val("0");
        return;
    });


    $("#SubmitRequest").click(function () {
        if (StartValidation("makeWithdrawalForm")) {
            $(".Main-loader").show();
            var details = {};
            details.Amount = $("#withdrawalAmount").val();
            details.WithdrawalAccount = $("#withdrawalAccountsList").val();
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