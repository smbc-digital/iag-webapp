jQuery(document).ready(function ($) {
    SetupCharsRemaining();
    OnlySubmitFormIfValid();
});

function SetupCharsRemaining() {

    $(".chars-remaining").each(function (idx, elem) {

        $input = $(elem).siblings(".form-control-deep");
            
        var limit = $input.data("val-length-max");
           
        DisplayCharsRemaining($input, limit);

        $input.on("keyup", function () { DisplayCharsRemaining(this, limit); });
    });
}


function DisplayCharsRemaining(input, limit) {

    var charsRemaining = NumberOfRemainingCharacters(input, limit);

    if (charsRemaining < 0) {
        charsRemaining *= -1;
        $input.siblings(".chars-remaining").html("<small class='error-text'>" + charsRemaining + " characters over the limit</small>");
    }
    else {
        $input.siblings(".chars-remaining").html("<small>" + charsRemaining + " characters remaining</small>");
    }
}

function NumberOfRemainingCharacters(input, limit) {
    var $input = $(input);
    var inputText = $input.val();
    var maxLength = parseInt(limit);

    var newLines = inputText.match(/(\n)/g);

    var addition = 0;
    if (newLines != null) {
        addition = newLines.length;
    }

    var currentLength = inputText.length + addition;
    var charsRemaining = maxLength - currentLength;

    return charsRemaining;
}

function OnlySubmitFormIfValid() {
    $("form")
        .submit(function (e) {
            console.log("form submission triggered");
            var chars = $(".chars-remaining");
            if (chars.length > 0) {
                $input = chars.siblings(".form-control-deep");
                var limit = $input.data("val-length-max");

                if (NumberOfRemainingCharacters($input, limit) < 0) {
                    e.preventDefault();
                }
            }
        });
}