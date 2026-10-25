$.validator.addMethod("notinfutureifowned", function (value, element, params) {
    if (!value) return true;

    var form = $(element).closest("form");
    var owned = form.find("input[type='checkbox'][name='" + params.owned + "']").is(":checked");
    if (!owned) return true;

    var now = new Date();
    var today = now.getFullYear() + "-" +
        String(now.getMonth() + 1).padStart(2, "0") + "-" +
        String(now.getDate()).padStart(2, "0");
    return value <= today;
});

$.validator.unobtrusive.adapters.add("notinfutureifowned", function (options) {
    options.rules["notinfutureifowned"] = { owned: options.params.owned };
    options.messages["notinfutureifowned"] = options.message;
});