$(document).ready(function () {
    $(".schedule_multiple_events_inputs").hide();
    $(".schedule_multiple_events").show();
    $(".schedule_multiple_events").click(function () {
        $(".schedule_multiple_events_inputs").slideToggle(200);
    });
});
