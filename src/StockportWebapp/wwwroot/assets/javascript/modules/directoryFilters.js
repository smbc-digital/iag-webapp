define(["jquery"], function ($) {

    var init = function () {
        $(document).ready(function () {
            $('.directory-results__collapsible').on('click', function() {
                $(this).next(".directory-filters__content").toggle();
                $(this).find("span").toggleClass("closed");
                $(this).attr('aria-expanded', $(this).attr('aria-expanded') === 'true' ? false : true);
            });

            $('.directory-filters__content').each(function(index, themeGroup) {
                if (index !== 0 && !$(themeGroup).find('.directory-filters__checkbox:checked').length) {
                    $(themeGroup).hide();
                    const link = $(themeGroup).prev('.directory-results__collapsible');
                    link.attr('aria-expanded', 'false');
                }
            });

            $('#order-by').change(function() {
                $(this).closest('form').submit();
            });

            $('.directory-filters__checkbox').change(function() {
                $(this).closest('form').submit();
            });
        }
    )}

    return {
        Init: init
    }
});