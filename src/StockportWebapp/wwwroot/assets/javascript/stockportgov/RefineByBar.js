STK.Utils = (function () {

    var stripParamFromQueryString = function (url, param) {
        url = url.toLowerCase();
        param = param.toLowerCase();

        var result = '';
        var splitter = '?';
        var urlArray = url.split('?');
        result = urlArray[0];
        if (urlArray.length > 1) {
            var params = urlArray[1].split('&');
            for (var i = 0; i < params.length; i++) {
                var entry = params[i].split('=')
                if (entry[0].toLowerCase() !== param.toLowerCase()) {
                    result = result + splitter + params[i];
                    splitter = '&';
                }
            }
        }

        return result;
    };

    return {
        StripParamFromQueryString: stripParamFromQueryString
    };
})();


STK.RefineBy = (function () {

    var currentState = [];

    var closeFilters = function (link) {
        $('.link', '#event-listing-refine-bar').each(function () {
            if (this !== link) {
                $(this).parent().removeClass('open');
            }
        });
    };

    var openFilter = function (link) {
        var parent = $(link).parent();
        $(parent).toggleClass('open')

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

        $('.refine', '#event-listing-refine-bar').each(function () {
            var count = 0;
            $('input[type=checkbox]', $(this)).each(function () {
                if ($(this).prop('checked')) {
                    count++;
                }
            });

            if (count > 0) {
                $('.badge', $(this)).html('<span>' + count + '</span>').css('visibility', 'visible');
            }
        });
    };

    var applyFilter = function () {
        var href = window.location.href;

        href = STK.Utils.StripParamFromQueryString(href, 'keeptag');
        href = STK.Utils.StripParamFromQueryString(href, 'fromsearch');
        href = STK.Utils.StripParamFromQueryString(href, 'tag');
        href = STK.Utils.StripParamFromQueryString(href, 'price');

        if (href.indexOf('?') < 0) {
            href += '?keeptag=' + getTag() + '&fromsearch=true';
        }
        else {
            href += '&keeptag=' + getTag() + '&fromsearch=true';
        }

        $('input:checked', '#event-listing-refine-bar').each(function () {
            href += '&' + $(this).prop('name') + '=' + $(this).val();
        });

        window.location.href = href;
    };

    var getTag = function () {
        return $('input[name=tag]', '#event-listing-refine-bar').val();
    };

    return {
        Init: function () {
            $('#event-listing-refine-bar').show();

            $('.link', '#event-listing-refine-bar').click(function () {
                closeFilters(this);
                openFilter(this);
            });

            $('.cancel', '#event-listing-refine-bar').click(function () {
                reApplyStateAndClose(this);
            });

            $('.apply', '#event-listing-refine-bar').click(function () {
                applyFilter();
            });

            setBadges();
            $('input[type=checkbox]', '#event-listing-refine-bar').click(function () {
                setBadges();
            });
        }
    };
})();

STK.RefineBy.Init();

