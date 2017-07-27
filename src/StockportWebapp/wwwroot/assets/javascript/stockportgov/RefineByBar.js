var STK = {};

STK.RefineBy = (function () {

    var closeFilters = function () {
        $(".refine").removeClass("open");
    };

    var openFilter = function (filter) {
        $(filter).toggleClass("open")
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