module.exports = function (callback, theData) {
    var jsreport = require('jsreport-core')();

    jsreport.init().then(function() {
        return jsreport.render({
            template: {
                content: '{{:foo}}',
                engine: 'jsrender',
                recipe: 'phantom-pdf',
                phantom: {
                    printDelay: 1000
                }
            },
            data: {
                foo: theData
            }
        }).then(function(resp) {
            callback(/* error */ null, resp.content.toJSON().data);
        });
    }).catch(function(e) {
        callback(/* error */ e, null);
    });
};