define(["jquery"], function ($) {

    var processValidation = function (validationResults, selectedInput, callback, bypassShowValidation) {
        var invalidResutls = $.grep(validationResults,
            function (v) {
                return v.isValid === false;
            });

        var selectedResults = $.grep(validationResults,
            function (v) {
                return v.questionId === selectedInput.attr("data-questionid");
            });

        $(selectedResults).each(function () {
            if (this.isValid || (!bypassShowValidation)) {
                callback(this.questionId, this.isValid, this.message);
            }
        });

        return (invalidResutls.length === 0);
    };

    return {
        processValidation: processValidation
    };
});

