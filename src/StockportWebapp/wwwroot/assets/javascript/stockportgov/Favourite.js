define(["jquery"], function ($) {




    var addFavourites = function (slug,type) {
        $.get('/favourites/groups/add?slug=${slug}&type=${type}',
            function (data, status) {

                
            });
    };
    var removeFavourites = function () {
        $.get("",
            function (data, status) {

            });
    };

    var init = function () {
        $(".favourite").click(function() {
            addFavourites();
            var slug = $(this).prop("data-item");
        });
    };

    return {
        Init: init
    };


});