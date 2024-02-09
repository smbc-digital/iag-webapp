define(["jquery"], function ($) {

    var init = function () {
        $(document).ready(function () {
            $('.directory-results__collapsible').on('click', function() {
                $(this).next(".directory-filters__content").toggle();
                $(this).find("span").toggleClass("closed");
                $(this).attr('aria-expanded', $(this).attr('aria-expanded') === 'true' ? false : true);
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