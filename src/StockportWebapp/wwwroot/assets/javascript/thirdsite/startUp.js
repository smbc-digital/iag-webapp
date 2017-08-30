define(["jquery"], function ($) {

    var documentReady = function () {

        $('.alert-close a').on('click', function () {
            $(this).closest('.alert').hide();
        });
    };

    var init = function () {
        $(document).ready(function () {
            documentReady();
        });
    };

    return {
        Init: init
    }
});

