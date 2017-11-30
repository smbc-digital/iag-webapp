define(["jquery", "questionview", "questionvalidator"], function ($, view, validator) {

    var route;

    function getValidationRoute() {
        var validateAction = 'validate';
        return route.lastIndexOf("/") === (route.length - 1) ? '/' + route + validateAction : '/' + route + '/' + validateAction;
    }

    function updateCheckboxes() {
        var listOfAnswers = [];
        $("#checkbox-list input[type='checkbox']:checked").each(function () {
            listOfAnswers.push($(this).parent().find(".question-option-value").val());
        });
        $("#checkbox-response").val(listOfAnswers.join());
    }

    var validateQuestions = function (form, selectedInput, bypassShowValidation) {
        updateCheckboxes();
        var deferredResult = $.Deferred();
        var validationRoute = getValidationRoute();
        $.post(validationRoute,
                form.serialize(),
                function () {
                })
            .done(function (response) {
                var isValid = validator.processValidation(response, selectedInput, view.showValidationForQuestion, bypassShowValidation);
                isValid ? view.enableNextButton() : view.disableNextButton();
                deferredResult.resolve();
            }).fail(function (xhr) {
                deferredResult.reject(xhr.status + ' [' + xhr.statusText + ']');
            });

        return deferredResult.promise();
    };

    var init = function (thisroute) {
        route = thisroute;
        view.showNextbutton();
        view.bindEventListeners(validateQuestions);
    };

    return {
        init: init,
        validateQuestions: validateQuestions
    };
});