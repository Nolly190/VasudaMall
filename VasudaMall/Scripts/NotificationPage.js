
$(document).ready(function () {

    $(".notificationTBody").on("click", ".viewMoreNotificationBtn", function () {
        $(".Main-loader").show();

        var id = $(this).data('id');
        $.ajax({
            url: "/Dashboard/GetSingleNotification",
            type: "Post",
            data: { id: id },
            error: function (status, xhr) {
                $(".Main-loader").hide();
                Swal.fire("Please Try Again!")
            },
            success: function (result) {
                if (result.Status === true) {
                    $(".Main-loader").hide();
                    entity = result._entity;

                    message = entity.Message == null || entity.Message.length == 0 ? "N/A" : entity.Message;

                    $("#nMessage").html(message);
                    $('#notification-view-more-modal').modal('show');

                    $(this).attr('class', 'btn btn-outline-success btn-sm viewMoreNotificationBtn');
                    
                    return;
                } else {
                    $(".Main-loader").hide();
                    Swal.fire(result.Message);
                    return;
                }
            }
        });
    });

})