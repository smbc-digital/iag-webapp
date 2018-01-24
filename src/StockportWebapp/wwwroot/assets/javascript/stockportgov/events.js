define(["jquery", "jquery-ui"], function ($) {

    var setEndDateToStartDate = function (endDateId, startDateId) {
        if (!Modernizr.inputtypes.date) {
            var startDate = $("#" + startDateId).val();

            if (startDate !== null) {
                var endDate = $("#" + endDateId).val();
                if (endDate === "") {
                    $("#" + endDateId).val(startDate);
                }
            }
        }
    }

    var setDatePickers = function () {
        if (!Modernizr.inputtypes.date) {
            if ($(".datepicker").length > 0) {
                $(".datepicker").datepicker({
                    inline: true,
                    dateFormat: 'dd/mm/yy'
                });    
            }

            $(".hasDatepicker").each(function () {
                var selectedDate = $(this).val();
                if (selectedDate !== null && selectedDate !== "") {
                    var eventdate = new Date(selectedDate);
                    $(this).val($.datepicker.formatDate('dd/mm/yy', eventdate));
                }
            });

            if ($.validator) {
                $.validator.addMethod('date',
                function (value, element) {
                    if (this.optional(element)) {
                        return true;
                    }

                    var ok = true;
                    try {
                        $.datepicker.parseDate('dd/mm/yy', value);
                    }
                    catch (err) {
                        ok = false;
                    }
                    return ok;
                });
            }
        }
    };

    var init = function () {
        setDatePickers();
        $(".schedule_multiple_events").show();

        if ($("#RecurringEventYnNo:checked").val()) {
            $(".schedule_multiple_events_inputs").hide();
        }
        else {
            $(".schedule_multiple_events_inputs").show();
        }

        $(".schedule_multiple_events").change(function () {
            $(".schedule_multiple_events_inputs").slideToggle(200);
        });

        if ($("#events-search-filter-container").hasClass("close-this-section")) {
            $('#events-search-filter-container').addClass('closed');
            $('#edit-search-link').addClass('open');
        }
    };

    return {
        Init: init,
        SetEndDateToStartDate: setEndDateToStartDate
    };

});


