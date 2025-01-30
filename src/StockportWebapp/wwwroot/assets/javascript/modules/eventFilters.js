define(["jquery"], function ($) {

    var init = function () {
        $(document).ready(function () {
            $('.events-filters__radio').change(function () {
                $('.events-filters__date').val('');
                $(this).closest('form').submit();
            });
            
            $('.events-filters__date').change(function () {
                $('.events-filters__radio').prop('checked', false);
                $(this).closest('form').submit();
            });
        }
    )}

    return {
        Init: init
    }
});