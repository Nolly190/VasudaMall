

function Validator(InputObj) {
    var result = [];
    if (InputObj.localName==="button") {
        return result;
    }
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
            if (InputObj.nodeName.toLowerCase() === "textarea") {
                if (getFormObj === "0" || getFormObj === 0 || getFormObj === "") {
                    result["Status"] = false;
                    result["Message"] = dataName + " Field is required";
                    return result;
                }
            }
        }

        if (InputObj.type.toLowerCase() === "email") {
            var pattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
            var MatchResult = getFormObj.match(pattern);
            if (MatchResult === null) {
                result["Status"] = false;
                result["Message"] = "Invalid email format for " + InputObj.id;
                return result;
            }

        }

        if (InputObj.type.toLowerCase() === "number") {
            if (getFormObj < 0) {
                result["Status"] = false;
                result["Message"] = "Invalid number format for " + dataName;
                return result;
            }

        }

        return result;
    }


    function StartValidation(formId) {
        var allInputFeilds = [];
        $("form#" + formId + " :input").each(function(index) {
            allInputFeilds[index] = $(this);
        });
        for (var i = 0; i < allInputFeilds.length - 1; i++) {

            var get = allInputFeilds[i];
            var checkForm = Validator(get[0]);
            if (checkForm["Status"] === false) {
                Swal.fire(checkForm["Message"]);
                return false;
            }
        }
        return true;
    }