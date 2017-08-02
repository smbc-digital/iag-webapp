var STK = STK || {};

STK.Groups = (function () {

    var init = function () {
        $(document).ready(
            function () {

                if ($(window).width() <= STK.StartUp.MobileWidth) {
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

                $(".print-this")
               .click(
                   function () {
                       window.print();
                   }
               );

            }
        );
    };

    return {
        Init: init
    }
})();


STK.Groups.Init();





