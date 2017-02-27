 $(document).ready(function () {
    if (!$("#EndDate").val() && !$("input[name='Frequency']:checked").val()) {
        $(".schedule_multiple_events_inputs").hide();
    }
    $(".schedule_multiple_events").show();
    $(".schedule_multiple_events").change(function () {        
            $(".schedule_multiple_events_inputs").slideToggle(200);        
    });
});
