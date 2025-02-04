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

            function checkEventDates() {
                $('.card-wrapper').each(function () {
                    $(this).find('.card-item').each(function (index, card) {
                        var $card = $(card);
                        var $nextCard = $card.next('.card-item');

                        if ($nextCard.length) {
                            var $date1 = $card.find('.card-item__date.event-same-date');
                            var $date2 = $nextCard.find('.card-item__date.event-same-date');

                            if (
                                $date1.css('visibility') === 'hidden' &&
                                $date2.css('visibility') === 'hidden'
                            ) {
                                $date1.css('display', 'none');
                                $date2.css('display', 'none');
                            }
                        }
                    });
                });
            }

            checkEventDates();
            $(window).resize(checkEventDates);
        });
    };

    return {
        Init: init
    };
});
