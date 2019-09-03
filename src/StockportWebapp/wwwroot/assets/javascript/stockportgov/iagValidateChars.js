define(["jquery"], function ($) {

    var setupCharsRemaining = function () {

        $(".chars-remaining").each(function (idx, elem) {

            $input = $(elem).siblings("textarea");
            var limit = $("textarea").attr("maxlength");
            displayCharsRemaining($input, limit);
            $input.on("keyup", function () { displayCharsRemaining(this, limit); });
        });
    };

    var displayCharsRemaining = function (input, limit) {

        var charsRemaining = numberOfRemainingCharacters(input, limit);
        console.log("input- " + input + ". limit- " + limit);

        if (charsRemaining < 0) {
            charsRemaining *= -1;
            $input.siblings(".chars-remaining").html("You are " + charsRemaining + " characters over the limit");
        }
        else {
            $input.siblings(".chars-remaining").html("You have " + charsRemaining + " characters remaining");
        }
    };

    var numberOfRemainingCharacters = function (input, limit) {
        var $input = $(input);
        var inputText = $input.val();
        var maxLength = parseInt(limit);
        var charsRemaining = maxLength - inputText.length;

        return charsRemaining;
    };

    var onlySubmitFormIfValid = function () {
        $("form")
            .submit(function (e) {
                var chars = $(".chars-remaining");
                if (chars.length > 0) {
                    $input = chars.siblings("textarea");
                    var limit = $input.data("maxlength");

                    if (numberOfRemainingCharacters($input, limit) < 0) {
                        e.preventDefault();
                    }
                }
            });
    };

    var init = function () {
        setupCharsRemaining();
        onlySubmitFormIfValid();
    };

    return {
        Init: init
    }
});
