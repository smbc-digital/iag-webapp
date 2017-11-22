define(["jquery", "utils"], function ($, utils) {

    var currentState = [];

    var favouritesVisible = false;

    var windowWidth = $(window).width();

    var closeFilters = function (link) {
        $('.link', '#listing-refine-bar').each(function () {
            if (this !== link && $(this).closest('.refine').hasClass('open')) {
                reApplyStateAndClose(this);
            }
        });
    };

    var openFilter = function (link) {

        var parent = $(link).parent();

        if ($(parent).hasClass('open')) {
            reApplyStateAndClose(link);
        }
        else {
            $(parent).addClass('open')
        }

        currentState = [];
        $('input[type=checkbox]', parent).each(function () {
            currentState.push($(this).prop('checked'));
        });
    };

    var reApplyStateAndClose = function (link) {
        var parent = $(link).closest('.refine');

        $('input[type=checkbox]', $(parent)).each(function (index) {
            $(this).prop('checked', currentState[index]);
        });

        $(parent).removeClass('open');
        if (favouritesVisible) {
            $('.favourites-bar').show();
        }

        setBadges();
    };

    var setBadges = function () {
        $('.badge', '#listing-refine-bar').css('visibility', 'hidden');

        var allcount = 0;
        $('.refine', '#listing-refine-bar').each(function () {
            var count = 0;
            $('input[type=checkbox]', $(this)).each(function () {
                if ($(this).prop('checked')) {
                    count++;
                    allcount++;
                }
            });

            if ($('.refine-filters.location', $(this)).length) {
                var $lat = $('input[name=latitude]', $('#refine-by-bar-container'));
                if ($lat.length == 1 && $lat.val() !== '' && $lat.val() !== '0') {
                    count++;
                    allcount++;
                }
            }

            if (count > 0) {
                $('.badge', $(this)).html('<span>' + count + '</span>').css('visibility', 'visible');
                if ($('.none-selected-error', this).length) {
                    $('.none-selected-error', this).hide();
                    $('.apply', this).removeClass('disabled').off('click').on('click', function () { applyFilter(); });
                    $('.update-button', '#listing-refine-bar').removeClass('disabled').prop('disabled', '');
                }
            }
            else if ($('.none-selected-error', this).length) {
                $('.none-selected-error', this).show();
                $('.apply', this).addClass('disabled').off('click');
                $('.update-button', '#listing-refine-bar').addClass('disabled').prop('disabled', 'disabled');
            }
        });

        if (allcount > 0) {
            $('.refine-all .badge', '#listing-refine-bar').html('<span>' + allcount + '</span>').css('visibility', 'visible');
        }
    };

    var applyFilter = function () {
        var href = window.location.href;

        href = utils.StripParamFromQueryString(href, 'page');
        href = utils.StripParamFromQueryString(href, 'keeptag');
        href = utils.StripParamFromQueryString(href, 'fromsearch');
        href = utils.StripParamFromQueryString(href, 'tag');
        href = utils.StripParamFromQueryString(href, 'price');
        href = utils.StripParamFromQueryString(href, 'getinvolved');
        href = utils.StripParamFromQueryString(href, 'subcategories');
        href = utils.StripParamFromQueryString(href, 'organisation');
        if ($('#KeepLocationQueryValues').val() !== "1") {
            href = utils.StripParamFromQueryString(href, 'longitude');
            href = utils.StripParamFromQueryString(href, 'latitude');
            href = utils.StripParamFromQueryString(href, 'location');
        }

        var tag = getTag();
        if (typeof (tag) == 'undefined') { tag = ''; }
        if (href.indexOf('?') < 0) {
            href += '?keeptag=' + tag + '&fromsearch=true';
        }
        else {
            href += '&keeptag=' + tag + '&fromsearch=true';
        }

        $('input:checked', '#listing-refine-bar').each(function () {
            href += '&' + $(this).prop('name') + '=' + $(this).val();
        });

        $('input[name=longitude]', '#refine-by-bar-container').each(function () {
            href += '&longitude=' + $(this).val();
        });

        $('input[name=latitude]', '#refine-by-bar-container').each(function () {
            href += '&latitude=' + $(this).val();
        });

        $('input[name=location]', '#refine-by-bar-container').each(function () {
            href += '&location=' + $(this).val();
        });

        window.location.href = href;
    };

    var getTag = function () {
        return $('input[name=tag]', '#listing-refine-bar').val();
    };

    var initialiseSlider = function () {
        var width = $(window).width();
        $('#refine-slider').css('left', width);

        var location = $('#location').val();
        if (location !== '') {
            $('.location-search-input').val(location);
            $('.search-all', '#listing-refine-bar').show();
            $('.search-all', '#primary-filter-listing-refine-bar').show();
        }
        else {
            $('.search-all', '#listing-refine-bar').hide();
        }
    };

    var searchAll = function () {
        $('#location').val('');
        $('#longitude').val('0');
        $('#latitude').val('0');
        $('.location-search-input').val('All locations');
        setBadges();
    };

    var setSliderHeight = function () {
        // outerHeight to include padding in the height
        var buttonBarHeight = $('.update-cancel-bar', '#listing-refine-bar').outerHeight();
        var theWindowHeight = $(window).height();
        var height = theWindowHeight - buttonBarHeight;
        $('.scroller', '#refine-slider').height(height);
    };

    var revealSlider = function () {

        favouritesVisible = $('.favourites-bar:visible').length;
        $('.favourites-bar').hide();

        $('#refine-slider').removeClass('hide-on-mobile').animate({ 'left': 0 }, 250);

        setSliderHeight();

        $('body').css('overflow-y', 'hidden');

        currentState = [];
        $('input[type=checkbox]', '#listing-refine-bar').each(function () {
            currentState.push($(this).prop('checked'));
        });

        windowWidth = $(window).width();
    };

    var clearHeight = function () {
        $('.scroller', '#refine-slider').height('');
    };

    var hideSlider = function () {
        var width = $(window).width();
        $('#refine-slider').animate({ 'left': width }, 250, 'swing', function () { $('#refine-slider').addClass('hide-on-mobile'); });
        $('#refine-slider').height('');
        $('.scroller', '#refine-slider').height('');
        $('body').css('overflow-y', 'scroll');
        $('input[type=checkbox]', '#listing-refine-bar').each(function (index) {
            $(this).prop('checked', currentState[index]);
        });
        setBadges();
    };

    var clearAllFilters = function () {
        $('input[type=checkbox]', '#listing-refine-bar').each(function () {
            $(this).prop('checked', false);
        });

        searchAll();
    };

    var applyLocation = function () {
        setBadges();
        applyFilter();
    }

    var showAll = function (element) {
        $("label.toggle", $(element).parent().parent().parent()).toggle();
        $(element).toggleClass("show-more");
        if ($(element).html().indexOf(" all ") > 0) {
            $(element).text($(element).text().replace(" all ", " fewer "));
        }
        else {
            $(element).text($(element).text().replace(" fewer ", " all "));
        }
    }

    return {
        Init: function () {
            setBadges();
            initialiseSlider();

            $('#listing-refine-bar').show();

            $('.link', '#listing-refine-bar').on('click', function () {
                closeFilters(this);
                openFilter(this);
            });

            $('.cancel', '#listing-refine-bar').on('click', function () {
                reApplyStateAndClose(this);
            });

            $('.apply', '#listing-refine-bar').on('click', function () {
                applyFilter();
            });

            $('input[type=checkbox]', '#listing-refine-bar').on('click', function () {
                setBadges();
            });

            $('#reveal-refine-by').click(function () {
                revealSlider();
            });

            $('.update-cancel-bar .cancel', '#listing-refine-bar').on('click', function () {
                hideSlider();
            });

            $('.update-cancel-bar .apply', '#listing-refine-bar').on('click', function () {
                $("#btnLocationAutoComplete").click();
            });

            $('.clear-all-filters a', '#listing-refine-bar').on('click', function () {
                clearAllFilters();
                setBadges();
            });

            $('.search-all', '#listing-refine-bar').on('click', function () {
                searchAll();
            });

            $('.show', '#listing-refine-bar').on('click', function () {
                showAll(this);
            });

            $(window).on('resize', function () {
                clearHeight();
                if (windowWidth !== $(window).width()) {
                    hideSlider();
                }
            });
        },
        ApplyLocation: applyLocation
    };
});

