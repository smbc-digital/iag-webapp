define(['jquery'], function ($) {

  return function () {

    var disableNextButton = function () {
      var button = $(".question-button-next");
      button.prop("disabled", true);
      button.addClass("button-disabled");
    };

    var enableNextButton = function () {
      var button = $(".question-button-next");
      button.prop("disabled", false);
      button.removeClass("button-disabled");
    };

    var showNextbutton = function() {
      var scriptButton = $(".question-button-next-script");
      scriptButton.show();
    };

    var showValidationForQuestion = function (questionId, isValid, validationMessage) {
      var validationContainer = $("div[data-questionid=" + questionId + "], span[data-questionid=" + questionId + "]");
      var validationMessageSpan = validationContainer.find("span").last();
      var validatedInput = $("input[data-questionid='" + questionId + "']");
      var validationClass = "input-validation-error";

      validationMessageSpan.html(validationMessage);

      if (isValid) {
        validationContainer.hide();
        validatedInput.removeClass(validationClass);
      }
      else
      {
        validatedInput.addClass(validationClass);
        validationContainer.show();
      }
    };

    var bindEventListeners = function (validationCallback) {
      $("form.question-form input").change(function () {
        validationCallback($("form.question-form"), $(this));
      });
      $("form.question-form input").blur(function () {
        validationCallback($("form.question-form"), $(this));
      });
      $("form.question-form input").keyup(function (e) {
        if (e.keyCode !== 9) {
          validationCallback($("form.question-form"), $(this), true);
        }
      });
        $(document).ready(function () {
            validationCallback($("form.question-form"), $(this));
        });
    };

    return {
      showNextbutton: showNextbutton,
      bindEventListeners: bindEventListeners,
      disableNextButton: disableNextButton,
      enableNextButton: enableNextButton,
      showValidationForQuestion: showValidationForQuestion
    };

  }

});