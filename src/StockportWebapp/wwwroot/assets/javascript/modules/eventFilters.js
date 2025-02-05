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
            
                    if (isWithinRange) {
                        if ($nextCard.length && $date1.css('visibility') === 'hidden' && $date2.css('visibility') === 'hidden') {
                            $date1.css('display', 'none');
                            $date2.css('display', 'none');
                        }
                    } else {
                        $date1.css('display', '');
                        if ($date2) $date2.css('display', '');
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