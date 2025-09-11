define(["jquery"], function ($) {

    var init = function () {
        $(document).ready(function () {
            $('.directory-results__collapsible').on('click', function() {
                var $link = $(this);
                var $content = $link.next(".directory-filters__content");
                $content.toggle();
                
                var expanded = $link.attr('aria-expanded') === 'true';
                $link.attr('aria-expanded', !expanded);

                if ($link.attr('aria-expanded') === 'true') {
                    $link.find("span").removeClass("closed");
                } else {
                    $link.find("span").addClass("closed");
                }
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
            
            $('.directory-results__collapsible').each(function () {
                var $link = $(this);
                if ($link.attr('aria-expanded') === 'true') {
                    $link.find("span").removeClass("closed");
                } else {
                    $link.find("span").addClass("closed");
                }
            });

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