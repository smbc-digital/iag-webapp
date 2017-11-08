define(["jquery", "matchboxconfig"], function ($, matchboxes) {


    var favouriteCount = 0;
    var favebar = true;
    var onFavouritesPage = false;

    var addFavourites = function (slug, cookieType) {
        $.get("/cookies/add?slug=" + slug + "&cookieType=" + cookieType,
            function (data, status) {
                if (favebar) {
                    favouriteCount++;
                    updateBar();
                }
            });
    };

    var removeFavourites = function (slug, type) {
        $.get("/cookies/remove?slug=" + slug + "&cookieType=" + type,
            function (data, status) {
                if (favebar) {
                    favouriteCount--;
                    updateBar();
                }

                if (onFavouritesPage) {
                    $("#item-card-" + slug).remove();

                    if ($(".group-card").length === 0) {
                        $("#no-results").show();
                        $("#favourites-list").hide();
                    }

                    matchboxes.Init();
                }
            });
    };

    var handleClicks = function () {
        $(".add-favourite").on("click", function () {
            var slug = $(this).attr("data-slug");
            var cookieType = $(this).attr("data-type");
            addFavourites(slug, cookieType);
            $("#add-favourite-" + slug).addClass("hidden");
            $("#remove-favourite-" + slug).removeClass("hidden");
            $("#print-favourite-group-item-" + slug).removeClass("hidden");
        });

        $(".remove-favourite").on("click", function () {
            var slug = $(this).attr("data-slug");
            var type = $(this).attr("data-type");
            removeFavourites(slug, type);
            $("#add-favourite-" + slug).removeClass("hidden");
            $("#remove-favourite-" + slug).addClass("hidden");
            $("#print-favourite-group-item-" + slug).addClass("hidden");
        });
    };

    var getRootfolder = function () {
        var path = window.location.href.toLowerCase().replace('http://', '').replace('https://', '');
        var folders = path.split('/');
        if (folders.length > 1) {
            var rootFolder = folders[1];
            if(rootFolder === "organisations"){
                return "groups";
            }
            return rootFolder;
        }
        else {
            return '';
        }
    };

    var getSiteArea = function () {
        var path = window.location.href.toLowerCase().replace('http://', '').replace('https://', '');
        var folders = path.split('/');
        if (folders.length > 1) {
            var root = folders[1];
            if (folders.length > 2 && (folders[2] === 'favourites' || folders[2] === 'manage')) {

                if (folders[2] === 'favourites') {
                    onFavouritesPage = true;
                }

                favebar = false;
                return '';
            }
        }

        switch (root) {
        case 'groups':
            return 'StockportWebapp.Models.Group';
        case 'organisations':
            return 'StockportWebapp.Models.Group';
        }

        return '';
    };

    var initialiseFaveBar = function () {
        
        var siteArea = getSiteArea();

        if (siteArea !== '' && readCookie('favourites')) {
            var favourites = $.parseJSON(unescape(readCookie('favourites')));
            var thisFaves = favourites[siteArea];
            if (typeof (thisFaves) !== 'undefined') {
                favouriteCount = thisFaves.length;
                updateBar();
            }
        }
    };

    var updateBar = function () {
        if (favouriteCount == 0) {
            $('.favourites-bar').hide();
        }
        else if ($('.favourites-bar').length > 0) {
            $('.count', '.favourites-bar').html(favouriteCount);
            $('.favourites-bar').css('display', 'block');
        }
    };

    var readCookie = function (name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }

        return '';
    };

    var init = function () {
        $('.favourites-bar').css('display', 'block');
        handleClicks();
        initialiseFaveBar();
    };

    return {
        Init: init
    };
});