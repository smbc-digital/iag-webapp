var STK = {};

STK.RefineBy = (function () {

    var closeFilters = function () {
        $(".refine a").removeClass("open");
        $(".refine-filters").hide();
    };

    var openFilter = function (filter) {

        debugger;

        $("a", $(filter)).addClass("open");

        if ($("a", $(filter)).hasClass("open")) {
            $(".refine-filters", $(filter)).hide();
        } else {
            $(".refine-filters", $(filter)).show();
            $("a", $(filter)).removeClass("open");
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