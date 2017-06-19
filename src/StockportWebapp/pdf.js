module.exports = function (callback, theData) {
    var jsreport = require('jsreport-core')();
    jsreport.init().then(function() {
        return jsreport.render({
            template: {
                content: '{{:html}}',
                engine: 'jsrender',
                recipe: 'phantom-pdf',
                phantom: {
                    printDelay: '{{:delay}}'
                }
            },
            data: {
                html: theData.data,
                delay: theData.delay
            }
        }).then(function(resp) {
            callback(/* error */ null, resp.content.toJSON().data);
        });
    }).catch(function(e) {
        callback(/* error */ e, null);
    });
};