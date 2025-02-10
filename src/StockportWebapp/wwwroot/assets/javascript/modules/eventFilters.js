define(["jquery"], function ($) {
    var init = function () {
        $(document).ready(function () {
            $('.events-filters__radio').change(function () {
                $('.events-filters__date-item').val('');
                $(this).closest('form').submit();
            });

            $('.events-filters__date').change(function () {
                $('.events-filters__radio').prop('checked', false);
                $(this).closest('form').submit();
            });

            function checkEventDates() {
                var isWithinRange = window.matchMedia("(min-width: 446px) and (max-width: 767px)").matches;
            
                $('.card-wrapper .card-item').each(function () {
                    var $card = $(this);
                    var $nextCard = $card.next('.card-item');
            
                    var $date1 = $card.find('.card-item__date.event-same-date');
                    var $date2 = $nextCard.length ? $nextCard.find('.card-item__date.event-same-date') : null;
            
                    if (!$date1.length || ($date2 && !$date2.length)) return;
            
                    if (isWithinRange) {
                        if ($nextCard.length && !$date1.is(":visible") && (!$date2 || !$date2.is(":visible"))) {
                            $date1.hide();
                            if ($date2) $date2.hide();
                        }
                    } else {
                        $date1.show();
                        if ($date2) $date2.show();
                    }
                });
            }
            
            $(document).ready(checkEventDates);
            $(window).resize(checkEventDates);
        });
    };

    return {
        Init: init
    };
});