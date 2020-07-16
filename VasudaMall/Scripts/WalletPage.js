$(document).ready(function () {

    var Validator = function (InputObj) {
        var result = [];
        var Name = $("#" + InputObj.id);
        var getFormObj = Name.val().trim();
        var dataName = Name.data("formname");

        if (InputObj.required === true) {

            if (InputObj.nodeName.toLowerCase() === "input") {
                if (getFormObj === null || getFormObj === "") {
                    result["Status"] = false;
                    result["Message"] = dataName + " Field is required";
                    return result;
                }
            }
            if (InputObj.nodeName.toLowerCase() === "select") {
                if (getFormObj === "0" || getFormObj === 0 || getFormObj === "") {
                    result["Status"] = false;
                    result["Message"] = "Select a value for " + dataName;
                    return result;
                }
            }
        }

        //if (InputObj.type.toLowerCase() === "email") {
        //    var pattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
        //    var MatchResult = getFormObj.match(pattern);
        //    if (MatchResult === null) {
        //        result["Status"] = false;
        //        result["Message"] = "Invalid email format for " + InputObj.id;
        //        return result;
        //    }

        //}

            if (InputObj.type.toLowerCase() === "number") {
            if (getFormObj < 0) {
                result["Status"] = false;
                result["Message"] = "Invalid number format for " + dataName;
                return result;
            }

        }

        return result;
    }

    $("#setWithdrawalAccountButton").click(function () {
        console.log("clicked me...");
        $(".Main-loader").show();
        var allInputFeilds = [];
        $("form#setWithdrawalAccountForm :input").each(function (index) {
            allInputFeilds[index] = $(this);
        });
        for (var i = 0; i < allInputFeilds.length - 1; i++) {

            var get = allInputFeilds[i];
            var checkForm = Validator(get[0]);
            if (checkForm["Status"] === false) {
                swal({
                    icon: 'error',
                    title: 'Oops...',
                    text: checkForm["Message"]
                });

                $(".Main-loader").hide();
                return;
            }
        }




        var accountInfo = {};

        accountInfo.AccountNumber = $("#accountName").val();
        accountInfo.BankName = $("#accountName").val();
        accountInfo.AccountName = $("#accountName").val();


        $.ajax({
            url: "/Dashboard/AddAccount",
            type: "Post",
            data: { jsonData: accountInfo },
            error: function (status, xhr) {
                $(".Main-loader").hide();
            },
            success: function (result) {

                $(".Main-loader").hide();
                if (result.status === "Successful") {
                    Swal.fire('Successfully Created');
                    $("#VerifyDetails").css("display", "none");
                    $("#LoadLodge").attr("disabled", false);
                    $("#formNum").val("");
                    return;
                }

                else if (result.status === undefined || result.status === "Refresh the page") {
                    window.setTimeout(function () { location.reload(); }, 3000);
                    return;
                }
                Swal.fire(result.status);
            }

        });
    });

});