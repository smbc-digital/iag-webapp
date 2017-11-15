define(["jquery"], function ($) {
    return {
        Init: function () {
            var form = $("#multistep-form");

            if (!$(".contact-validation-error").length)
            {
                // delete cookie if no validation error message
                // TODO: Make this work
                document.cookie =
                    "jQu3ry_5teps_St%40te_multistep-form-sections-wrapper=;expires=Thu, 01 Jan 1970 00:00:01 GMT;";
            }

            $("#multistep-form-sections-wrapper").steps({
                headerTag: "h2",
                bodyTag: "section",
                transitionEffect: "slideLeft",
                autoFocus: true,
                onStepChanging: function (event, currentIndex, newIndex) {
                    // Allways allow previous action even if the current form is not valid!
                    if (currentIndex > newIndex) {
                        return true;
                    }

                    // Needed in some cases if the user went back (clean up)
                    if (currentIndex < newIndex) {
                        // To remove error styles
                        form.find(".body:eq(" + newIndex + ") label.error").remove();
                        form.find(".body:eq(" + newIndex + ") .error").removeClass("error");
                    }

                    // validate
                    //if the inputs are valid
                    form.validate().settings.ignore = ":disabled,:hidden";
                    return form.valid();
                },
                onStepChanged: function (event, currentIndex, newIndex) {
                    // you can use currentIndex property to display what step you are on
                    return true;
                },
                labels: {
                    next: "Next step",
                    previous: "Back"
                },
                saveState: true,
                titleTemplate: "<span class=\"number\">#index#. </span>#title#",
            });
        }
    }
});