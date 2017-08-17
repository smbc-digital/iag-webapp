define(["jquery"], function ($) {




    var addFavourites = function (slug,type) {
        $.get("/favourites/add?slug=" + slug + "&type="+ type,
            function (data, status) {

                
            });
    };
    var removeFavourites = function (slug, type) {
        $.get("/favourites/add?slug=" + slug + "&type=" + type,
            function (data, status) {
               
            });
    };

    var handleClicks = function () {
        $(".add-favourite").off("click");
        $(".remove-favourite").off("click");
       
        $(".add-favourite").on("click", function () {
            addFavourites($(this).attr("data-slug"), $(this).attr("data-type"));
            $(this).addClass("remove-favourite");
            $(this).removeClass("add-favourite");
            handleClicks();
        });
        $(".remove-favourite").on("click", function () {
            removeFavourites($(this).attr("data-slug"), $(this).attr("data-type"));
            $(this).addClass("add-favourite");
            $(this).removeClass("remove-favourite");
            handleClicks();
        });
    }

    var init = function () {
        handleClicks();
    };

    return {
        Init: init
    };


});