$(document).ready(function () {

   
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
                        return;
                    } 
                    Swal.fire(result.Message);
                }

            });
        }
        return;
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
});