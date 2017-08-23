define(["jquery", "utils"], function ($, utils) {

    var handleVolunteering = function (input) {
        if ($(input).is(':checked') === true) {
            $("#volunteering-text").show();
        }
        else {
            $("#volunteering-text").hide();
        }
    };

    var init = function () {

        if ($(window).width() <= utils.MobileWidth) {
            $("#edit-search").hide();
            $(".result-arrow").addClass("result-search-down-arrow");

            $("#open-edit-search").click(function () {
                $("#edit-search").show();
                $(".result-arrow").toggleClass("result-search-down-arrow");
                $(".result-arrow").toggleClass("result-search-up-arrow");

                $(".result-search-down-arrow").parent().click(function () {
                    $("#edit-search").show();
                });
                $(".result-search-up-arrow").parent().click(function () {
                    $("#edit-search").hide();
                });
            });

            $(".result-search-up-arrow").parent().click(function () {
                $("#edit-search").hide();
            });
        }

        $(".print-this").click(function () {
            window.print();
        });

        handleVolunteering($("#volunteering-needed"));

        $("#volunteering-needed").on("change", function () {
            handleVolunteering(this);
        });

        $(".remove-favourite,.add-favourite").attr("href", "javascript:void(0)");
    };

    return {
        Init: init
    }
});





