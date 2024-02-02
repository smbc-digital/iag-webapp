define(["jquery"], function ($) {

    var init = function () {
        $(document).ready(function () {
            $('.directory-results__collapsible').on('click', function() {
                $(this).find("span").toggleClass("expanded");
                $(this).next(".directory-filters__content").toggle();
            });
    })}

    return {
        Init: init
    }
});
