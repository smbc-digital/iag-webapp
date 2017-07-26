var STK = {};

STK.RefineBy = (function () {

    var closeFilters = function () {
        $(".refine").removeClass("open");
        $(".refine-filters").hide();
    };

    var openFilter = function (filter) {
        $(".refine", $(filter)).addClass("open");

        if ($(".refine", $(filter)).hasClass("open")) {
            $(".refine-filters", $(filter)).hide();
        } else {
            $(".refine-filters", $(filter)).show();
            $(".refine", $(filter)).removeClass("open");
        }
    };

    return {
        Init: function () {

            $(".refine").click(function () {
                closeFilters();
                openFilter(this);
            });
        }
    };
})();
STK.RefineBy.Init();