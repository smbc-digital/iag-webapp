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
                if ($("#Category1 :selected").val() === "") {
                    $("#add-category2").hide();
                    $("#remove-category1").hide();
                } else {
                    var cat1Value = $("#Category1 :selected").val();
                    var cat2Value = $("#Category2 :selected").val();
                    var cat3Value = $("#Category3 :selected").val();

                    $("#Category3 option").show();
                    $("#Category2 option").show()
                    if (cat2Value) {
                        $("#Category1 option[value='" + cat1Value + "']").hide();
                        $("#Category2 option[value='" + cat1Value + "']").hide();
                    }
                    if (cat3Value) {
                        $("#Category1 option[value='" + cat3Value + "']").hide();
                        $("#Category2 option[value='" + cat3Value + "']").hide();
                    }
                    $("#Category3 option[value='" + cat1Value + "']").hide();
                    $("#Category2 option[value='" + cat1Value + "']").hide();

                    if ($("#category-div2").css("display") === "none") {
                        $("#add-category2").show();
                        $("#remove-category1").hide();
                    } else {
                        $("#add-category2").hide();
                    }
                    $("#remove-category1").parent().show();
                }

            }
        );
    $("#Category2").change(
        function () {
            if ($("#category-div3").css("display") === "none") {
                $("#add-category3").show();
            }

            if (($("#Category2").val() === "") && $("#category-div3").css("display") === "none") {
                $("#add-category3").hide();
                $("#remove-category1").hide();
            }
            if ($("#Category2").val() !== "") {
                var cat1Value = $("#Category1 :selected").val();
                var cat2Value = $("#Category2 :selected").val();
                var cat3Value = $("#Category3 :selected").val();
                $("#Category3 option").show();
                $("#Category1 option").show();
                if (cat1Value) {
                    $("#Category2 option[value='" + cat1Value + "']").hide();
                    $("#Category3 option[value='" + cat1Value + "']").hide();
                }
                if (cat3Value) {
                    $("#Category2 option[value='" + cat3Value + "']").hide();
                    $("#Category1 option[value='" + cat3Value + "']").hide();
                }
                $("#Category1 option[value='" + cat2Value + "']").hide();
                $("#Category3 option[value='" + cat2Value + "']").hide();
                $("#remove-category1").show();
            }
        }
    );

    $("#Category3").change(
        function () {
            var cat1Value = $("#Category1 :selected").val();
            var cat2Value = $("#Category2 :selected").val();
            var cat3Value = $("#Category3 :selected").val();


            $("#Category1 option").show();
            $("#Category2 option").show();
            if (cat1Value) {
                $("#Category3 option[value='" + cat1Value + "']").hide();
                $("#Category2 option[value='" + cat1Value + "']").hide();

            }
            if (cat2Value) {
                $("#Category3 option[value='" + cat2Value + "']").hide();
                $("#Category1 option[value='" + cat2Value + "']").hide();
            };
            $("#Category1 option[value='" + cat3Value + "']").hide();
            $("#Category2 option[value='" + cat3Value + "']").hide();
        }
    );

    $("#add-category2").click(
        function () {
            //            alert("Add category 2");
            var cat1Value = $("#Category1 :selected").val();
            if (cat1Value) {
                $("#Category2 option[value='" + cat1Value + "']").hide();
                $("#category-div2").show();
                $("#remove-category2").parent().show();
                $("#remove-category2").show();
                $(this).hide();
            }
            return false;
        }
    );

    $("#add-category3").click(
        function () {

            //            alert("Add category 3");
            var cat1Value = $("#Category1 :selected").val();
            var cat2Value = $("#Category2 :selected").val();
            if (cat1Value && cat2Value) {
                $("#Category3 option[value='" + cat1Value + "']").hide();
                $("#Category3 option[value='" + cat2Value + "']").hide();
                $("#category-div3").show();
                $("#remove-category3").parent().show();
                $("#remove-category3").show();
                $(this).hide();
            }
            return false;
        }
    );

    $("#remove-category3").click(
        function () {
            UpdateOptions("Category3");
            $("#category-div3").hide();
            $("#Category3").val("");
            $(this).hide();
            if ($("#Category2").val()) {
                $("#add-category3").show();
            };
            return false;
        }
    );

    $("#remove-category2").click(
       function () {
           UpdateOptions("Category2");

           if ($("#category-div3").css('display') === "none") {
               $("#category-div2").hide();
               $("#add-category2").show();
               $("#Category2").val("");
               $("#remove-category1").hide();
           }
           else if (($("#Category3").css("display") === "block") && $("#Category3").val() !== "") {
               $("#Category2").val($("#Category3").val());
               $("#Category3").val("");
               $("#category-div3").hide();
               $("#add-category3").show();
           }
           else if ($("#Category3").css("display") === "block") {
               $("#Category2").val($("#Category3").val());
               $("#category-div3").hide();
           }
           if ($("#Category2").val() === "") {
               $("#add-category3").hide();
               $("#remove-category1").hide();
           }
           if ($("#Category1 :selected").val() === "") {
               $("#add-category2").hide();
           }
           return false;
       }
   );


    $("#remove-category1").click(
       function () {
           UpdateOptions("Category1");

           if (($("#category-div3").css("display") == "block") && $("#category-div2").css("display") == "block") {
               $("#Category1").val($("#Category2").val());
               $("#Category2").val($("#Category3").val());
               $("#Category3").val("");
               $("#category-div3").hide();
               $("#add-category3").show();
           }
           else if ($("#category-div3").css("display") == "none") {
               $("#Category1").val($("#Category2").val());
               $("#Category2").val("");
               $("#category-div2").hide();
               $("#add-category2").show();
               $(this).hide();
           }
           if ($("#Category1").val() === "") {
               $("#remove-category1").hide();
               $("#add-category2").hide();
           }
           if ($("#Category2 :selected").val() === "") {
               $("#add-category3").hide();
           }
           return false;
       }
   );
   
 });

function UpdateOptions(thisId) {
    var cat1Value = $("#Category1 :selected").val();
    var cat2Value = $("#Category2 :selected").val();
    var cat3Value = $("#Category3 :selected").val();
    var thisValue = $("#" + thisId + ":selected").val();

    $("#Category3 option[value='" + thisValue + "']").show();
    $("#Category2 option[value='" + thisValue + "']").show();
    $("#Category1 option[value='" + thisValue + "']").show();

    if (cat1Value && cat1Value !== thisValue) {
        $("#Category2 option[value='" + cat1Value + "']").hide();
        $("#Category3 option[value='" + cat1Value + "']").hide();
    }

    if (cat2Value && cat2Value !== thisValue) {
        $("#Category1 option[value='" + cat2Value + "']").hide();
        $("#Category3 option[value='" + cat2Value + "']").hide();
    }
    if (cat3Value && cat3Value !== thisValue) {
        $("#Category1 option[value='" + cat3Value + "']").hide();
        $("#Category2 option[value='" + cat3Value + "']").hide();
    }
}