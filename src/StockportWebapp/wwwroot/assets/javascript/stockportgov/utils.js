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