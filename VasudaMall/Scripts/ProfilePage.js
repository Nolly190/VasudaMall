
$(document).ready(function () {
    $("#changePasswordBtn").click(function () {
        if (StartValidation("changePasswordForm")) {
            $(".Main-loader").show();
            var passwordData = {};
            passwordData.OldPassword = $("#oldPassword").val();
            passwordData.NewPassword = $("#newPassword").val();
            passwordData.ConfirmNewPassword = $("#confirmNewPassword").val();

            var passwordEqual = passwordData.NewPassword == passwordData.ConfirmNewPassword;
            if (!passwordEqual) {
                $(".Main-loader").hide();
                Swal.fire("New password does not match confirm password.")
                return;
            }

            $.ajax({
                url: "/Account/ChangePassword",
                type: "Post",
                data: { model: passwordData },
                error: function (status, xhr) {
                    $(".Main-loader").hide();
                },
                success: function (result) {
                    if (result.Status === true) {
                        $(".Main-loader").hide();
                        $("#oldPassword").val("");
                        $("#newPassword").val("");
                        $("#confirmNewPassword").val("");
                        Swal.fire('Password changed Successfully.');

                        return;
                    } else {
                        $(".Main-loader").hide();
                        Swal.fire(result.Message);
                        return;
                    }
                }
            });
        }
        return;
    });


    $("#editUserProfileBtn").click(function () {
        $(".Main-loader").show();
        //$("#editUserProfileBtn").html("Updating... Please wait");
        var userData = {};
        userData.FullName = $("#fullName").val();
        userData.PhoneNumber = $("#phoneNumber").val();
        userData.Address = $("#address").val();
        userData.City = $("#city").val();
        userData.Country = $("#country").val();


        $.ajax({
            url: "/Dashboard/UpdateProfile",
            type: "Post",
            data: { model: userData },
            error: function (status, xhr) {
                $(".Main-loader").hide();
                //$("#editUserProfileBtn").html("Update");
            },
            success: function (result) {
                if (result.Status === true) {
                    var res = result._entity
                    $(".Main-loader").hide();
                    //$("#editUserProfileBtn").html("Update");

                    $("#fullName").val(res.FullName);
                    $("#phoneNumber").val(res.PhoneNumber);
                    $("#address").val(res.Address);
                    $("#city").val(res.City);
                    $("#country").val(res.Country);
                    $("#fullNameProfile").html(res.FullName);
                    $("#phoneNumberProfile").html(res.PhoneNumber);
                    $("#addressProfile").html(res.Address);
                    $("#cityProfile").html(res.City);
                    $("#countryProfile").html(res.Country);
                    Swal.fire(result.Message);

                    return;
                } else {
                    $(".Main-loader").hide();
                    //$("#editUserProfileBtn").html("Update");
                    Swal.fire(result.Message);
                    return;
                }
            }
        });
    });
    //Update Profile ends here...

    //Document.ready ends here
});