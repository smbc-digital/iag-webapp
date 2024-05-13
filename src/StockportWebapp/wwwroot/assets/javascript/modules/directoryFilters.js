define(["jquery"], function ($) {

    var init = function () {
        $(document).ready(function () {
            $('.directory-results__collapsible').on('click', function() {
                $(this).next(".directory-filters__content").toggle();
                $(this).find("span").toggleClass("closed");
                $(this).attr('aria-expanded', $(this).attr('aria-expanded') === 'true' ? false : true);
            });

            $('.directory-filters__content').each(function(index, themeGroup) {
                if (!$(themeGroup).find('.directory-filters__checkbox:checked').length) {
                    $(themeGroup).hide();
                    const link = $(themeGroup).prev('.directory-results__collapsible');
                    link.attr('aria-expanded', 'false');
                }
            });

            var anyCheckboxChecked = $('.directory-filters__checkbox:checked').length > 0;
            if (!anyCheckboxChecked) {
                $('.directory-filters__content').first().show();
                $('.directory-results__collapsible').first().attr('aria-expanded', 'true');
            }

            var toggleIcons = document.querySelectorAll('.directory-results__toggle');
            toggleIcons.forEach(function(icon) {
                icon.classList.add('directory-results__toggle-js');
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