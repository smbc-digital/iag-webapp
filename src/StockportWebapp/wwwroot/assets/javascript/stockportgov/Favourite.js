define(["jquery"], function ($) {




    var addFavourites = function (slug,type) {
        $.get("/favourites/add?slug=" + slug + "&type="+ type,
            function (data, status) {

                
            });
    };
    var removeFavourites = function (slug, type) {
        $.get("/favourites/remove?slug=" + slug + "&type=" + type,
            function (data, status) {
               
            });
    };

    var handleClicks = function () {
        $(".add-favourite").on("click", function () {
            var slug = $(this).attr("data-slug");
            var type = $(this).attr("data-type");
            addFavourites(slug, type);
            $("#add-favourite-" + slug).addClass("hidden");
            $("#remove-favourite-" + slug).removeClass("hidden");
        });
        $(".remove-favourite").on("click", function () {
            var slug = $(this).attr("data-slug");
            var type = $(this).attr("data-type");
            removeFavourites(slug, type);
            $("#add-favourite-" + slug).removeClass("hidden");
            $("#remove-favourite-" + slug).addClass("hidden");
        });
    }

    var init = function () {
        handleClicks();
    };

    return {
        Init: init
    };


});