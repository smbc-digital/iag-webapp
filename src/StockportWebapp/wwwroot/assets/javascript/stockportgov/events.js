 $(document).ready(function () {
    if (!$("#EndDate").val() && !$("input[name='Frequency']:checked").val()) {
        $(".schedule_multiple_events_inputs").hide();
    }
    $(".additional-category").hide();  

    $(".schedule_multiple_events").show();
    $(".schedule_multiple_events").change(function () {        
            $(".schedule_multiple_events_inputs").slideToggle(200);        
    });

$("#Category1").change(
            function () {              
                if ($(this).val() && $("#Category2").css("display") == "none") {
                    $("#add-category2").show();
                }

            }
        );
    $("#Category2").change(
        function () {
            if ($(this).val() && $("#Category3").css("display") == "none") {
                $("#add-category3").show();
            }

        }
    );

    $("#add-category2").click(
        function () {
            var cat1Value = $("#Category1 :selected").val();
            if (cat1Value) {
                $("#Category2 option[value='"+ cat1Value+ "']").hide();
                $("#Category2").show();
                $(this).hide();
            }
            return false;
        }
    );
    $("#add-category3").click(
        function () {

            var cat1Value = $("#Category1 :selected").val();
            var cat2Value = $("#Category2 :selected").val();
            if (cat1Value && cat2Value) {
                $("#Category3 option[value='"+ cat1Value +"']").hide();
                $("#Category3 option[value='" + cat2Value +"']").hide();
                $("#Category3").show();
                $(this).hide();
            }
            return false;
        }
    );
});
