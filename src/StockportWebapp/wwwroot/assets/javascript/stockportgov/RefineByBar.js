define(["jquery", "utils"], function ($, utils) {

    var currentState = [];

    var windowWidth = $(window).width();

    var closeFilters = function (link) {
        $('.link', '#event-listing-refine-bar').each(function () {
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
        setBadges();
    };

    var setBadges = function () {
        $('.badge', '#event-listing-refine-bar').css('visibility', 'hidden');

        var allcount = 0;
        $('.refine', '#event-listing-refine-bar').each(function () {
            var count = 0;
            $('input[type=checkbox]', $(this)).each(function () {
                if ($(this).prop('checked')) {
                    count++;
                    allcount++;
                }
            });

            var $lat = $('input[name=latitude]', $(this));
            if ($lat.length == 1 && $lat.val() !== '' && $lat.val() !== '0') {
                count++;
                allcount++;
            }

            if (count > 0) {
                $('.badge', $(this)).html('<span>' + count + '</span>').css('visibility', 'visible');
                if ($('.none-selected-error', this).length) {
                    $('.none-selected-error', this).hide();
                    $('.apply', this).removeClass('disabled').off('click').on('click', function () { applyFilter(); });
                    $('.update-button', '#event-listing-refine-bar').removeClass('disabled').prop('disabled', '');
                }
            }
            else if ($('.none-selected-error', this).length) {
                $('.none-selected-error', this).show();
                $('.apply', this).addClass('disabled').off('click');
                $('.update-button', '#event-listing-refine-bar').addClass('disabled').prop('disabled', 'disabled');
            }
        });

        if (allcount > 0) {
            $('.refine-all .badge', '#event-listing-refine-bar').html('<span>' + allcount + '</span>').css('visibility', 'visible');
        }
    };

    var applyFilter = function () {

        var href = window.location.href;

        href = utils.StripParamFromQueryString(href, 'page');
        href = utils.StripParamFromQueryString(href, 'keeptag');
        href = utils.StripParamFromQueryString(href, 'fromsearch');
        href = utils.StripParamFromQueryString(href, 'tag');
        href = utils.StripParamFromQueryString(href, 'price');
        href = utils.StripParamFromQueryString(href, 'longitude');
        href = utils.StripParamFromQueryString(href, 'latitude');
        href = utils.StripParamFromQueryString(href, 'location');

        var tag = getTag();
        if (typeof (tag) == 'undefined') { tag = ''; }
        if (href.indexOf('?') < 0) {
            href += '?keeptag=' + tag + '&fromsearch=true';
        }
        else {
            href += '&keeptag=' + tag + '&fromsearch=true';
        }

        $('input:checked', '#event-listing-refine-bar').each(function () {
            href += '&' + $(this).prop('name') + '=' + $(this).val();
        });

        $('input[name=longitude]', '#event-listing-refine-bar').each(function () {
            href += '&longitude=' + $(this).val();
        });

        $('input[name=latitude]', '#event-listing-refine-bar').each(function () {
            href += '&latitude=' + $(this).val();
        });

        $('input[name=location]', '#event-listing-refine-bar').each(function () {
            href += '&location=' + $(this).val();
        });

        window.location.href = href;
    };

    var getTag = function () {
        return $('input[name=tag]', '#event-listing-refine-bar').val();
    };

    var initialiseSlider = function () {
        var width = $(window).width();
        $('#refine-slider').css('left', width);

        var location = $('#location').val();
        if (location !== '') {
            $('.location-search-input').val(location);
            $('.search-all', '#event-listing-refine-bar').show();
            $('.search-all', '#primary-filter-listing-refine-bar').show();
        }
        else {
            $('.search-all', '#event-listing-refine-bar').hide();
        }
    };

    var searchAll = function () {
        $('#location').val('');
        $('#longitude').val('0');
        $('#latitude').val('0');
        $('.location-search-input').val('All locations');
        setBadges();
    };

    var revealSlider = function () {
        var top = $(document).scrollTop();
        $('#refine-slider').css('top', top).removeClass('hide-on-mobile').animate({ 'left': 0 }, 250);
        var height = $('#refine-slider').height() - 50 - $('.update-cancel-bar', '#event-listing-refine-bar').height();
        $('.scroller', '#refine-slider').height(height);
        $('body').css('overflow-y', 'hidden');

        currentState = [];
        $('input[type=checkbox]', '#event-listing-refine-bar').each(function () {
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
        $('body').css('overflow-y', 'scroll');
        $('input[type=checkbox]', '#event-listing-refine-bar').each(function (index) {
            $(this).prop('checked', currentState[index]);
        });
        setBadges();
    };

    var clearAllFilters = function () {
        $('input[type=checkbox]', '#event-listing-refine-bar').each(function () {
            $(this).prop('checked', false);
        });

        searchAll();
    };

    var applyLocation = function () {
        setBadges();
        applyFilter();
    }

    return {
        Init: function () {
            setBadges();
            initialiseSlider();

            $('#event-listing-refine-bar').show();

            $('.link', '#event-listing-refine-bar').on('click', function () {
                closeFilters(this);
                openFilter(this);
            });

            $('.cancel', '#event-listing-refine-bar').on('click', function () {
                reApplyStateAndClose(this);
            });

            $('.apply', '#event-listing-refine-bar').on('click', function () {
                applyFilter();
            });

            $('input[type=checkbox]', '#event-listing-refine-bar').on('click', function () {
                setBadges();
            });

            $('#reveal-refine-by').click(function () {
                revealSlider();
                $('#event-listing-container').addClass('hideThisDiv');  
                $('.atoz').addClass('hideThisDiv');   
                $('.l-container-footer').addClass('hideThisDiv');    
                $('.footer-bottom').addClass('hideThisDiv');
                $('.l-header-container').css('display', 'none');  
                $('.breadcrumb-container').css('display', 'none');  
                $('.full-width-title').css('display', 'none');  
                $('#events-filter-bar-container').css('display', 'none');  
            });

            $('.update-cancel-bar .cancel', '#event-listing-refine-bar').on('click', function () {
                hideSlider();                      
            });

            $('.update-cancel-bar .cancel').on('click', function () {
                $('#event-listing-container').removeClass('hideThisDiv'); 
                $('.atoz').removeClass('hideThisDiv'); 
                $('.l-container-footer').removeClass('hideThisDiv'); 
                $('.footer-bottom').removeClass('hideThisDiv'); 
                $('.l-header-container').css('display', 'block');  
                $('.breadcrumb-container').css('display', 'block');  
                $('.full-width-title').css('display', 'block');  
                $('#events-filter-bar-container').css('display', 'block');  
            });

            $('.update-cancel-bar .apply', '#event-listing-refine-bar').on('click', function () {
                $("#btnLocationAutoComplete").click();
            });

            $('.clear-all-filters a', '#event-listing-refine-bar').on('click', function () {
                clearAllFilters();
                setBadges();
            });

            $('.search-all', '#event-listing-refine-bar').on('click', function () {
                searchAll();
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

