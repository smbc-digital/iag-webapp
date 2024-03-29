﻿define(["jquery", "modernizr"], function ($) {

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

    var init = function () {
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


