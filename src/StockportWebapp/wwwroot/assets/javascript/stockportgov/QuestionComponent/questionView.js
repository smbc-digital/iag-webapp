define(["jquery"], function ($) {

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

    var showNextbutton = function () {
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
        else {
            validatedInput.addClass(validationClass);
            validationContainer.show();
        }
    };

    var showTertiaryInformation = function (questionId) {
        $("span[data-questionid=" + questionId + "]").toggle();
    }

    var bindEventListeners = function (validationCallback) {
        $("form.question-form input, form.question-form textarea").change(function () {
            validationCallback($("form.question-form"), $(this));
            showTertiaryInformation()
        });
        $("form.question-form input, form.question-form textarea").blur(function () {
            validationCallback($("form.question-form"), $(this));
        });
        $("form.question-form input, form.question-form textarea").keyup(function (e) {
            if (e.keyCode !== 9) {
                validationCallback($("form.question-form"), $(this), true);
            }
        });

        $(document).ready(function () {
            $(".secondary-info-text").addClass("hidden");
        })

        $(document).keyup(function (e) {
            if (e.keyCode === 13) {
                $("#next-button").click();
            }
        });
        $(document).ready(function () {
            validationCallback($("form.question-form"), $(this));
        }); 

        $(document).ready(function () {
            if ($("#_1").is(':checked')) {
                $(".info-drop-down-up").removeClass("fa-angle-down");
                $(".info-drop-down-up").addClass("fa-angle-up");
            } else {
                $(".info-drop-down-up").addClass("fa-angle-down");
                $(".info-drop-down-up").removeClass("fa-angle-up");
            }
        });  

        $(document).ready(function () {                       
            var isOpen = $("#_1").is(':checked');
            $(".secondary-info-container").on('click', function () {
                if (!isOpen) {                    
                    isOpen = true;                   
                    $(".info-drop-down-up").removeClass("fa-angle-down");
                    $(".info-drop-down-up").addClass("fa-angle-up");
                }
                else {                   
                    isOpen = false;
                    $(".info-drop-down-up").addClass("fa-angle-down");
                    $(".info-drop-down-up").removeClass("fa-angle-up");
                }
            }); 
        });

        $(document).ready(function () {
            $(".radio-image").click(function() {
                $(".radio-button-smart", $(this).parent()).click();
            });
        });
        
        $(document).ready(function () {
            $(".radio-button-smart").change(function() {
                $(".radio-button-smart").each(function (index, button) {
                    var ischecked = $(button).is(':checked');
                    if(ischecked) 
                    {
                        $(button).parent().parent().parent().addClass('checked');
                    }
                    else
                    {
                        $(button).parent().parent().parent().removeClass('checked');
                    }
                    
                });                    
            });
        });

        $(document).ready(function() {
            $("label.secondary-info-container").keyup(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    $("label.secondary-info-container").click();
                }
            })
        });

        $(document).ready(function () {
            $(".secondary-info-container").click(function () {
                $(".secondary-info-text").toggleClass("hidden");
            });
        })


    };

    return {
        showNextbutton: showNextbutton,
        bindEventListeners: bindEventListeners,
        disableNextButton: disableNextButton,
        enableNextButton: enableNextButton,
        showValidationForQuestion: showValidationForQuestion
    };
});

