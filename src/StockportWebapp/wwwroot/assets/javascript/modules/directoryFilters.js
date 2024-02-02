define(["jquery"], function ($) {

    var init = function () {
        $(document).ready(function () {
            $('.directory-results__toggle').on('click', function() {
              $(this).parent().next(".directory-filters__content").toggle();
              $(this).toggleClass("expanded");
          });
    })}

    return {
        Init: init
    }
});
