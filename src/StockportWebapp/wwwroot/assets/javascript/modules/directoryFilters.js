define(["jquery"], function ($) {

    var init = function () {
        $(document).ready(function () {
            $('.directory-results__collapsible').on('click', function() {
                $(this).find("span").toggleClass("closed");
                $(this).next(".directory-filters__content").toggle();
            });


            $('#order-by').change(function() {
                $(this).closest('form').submit();
            });
        }
    )}

    return {
        Init: init
    }
});