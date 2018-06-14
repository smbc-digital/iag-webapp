module.exports = function (callback, theData) {
    var jsreport = require('jsreport-core')();
    jsreport.use(require('jsreport-phantom-pdf')())
    jsreport.use(require('jsreport-jsrender')())
    jsreport.init().then(function () {
        return jsreport.render({
            template: {
                content: '{{:html}}',
                engine: 'jsrender',
                recipe: 'phantom-pdf',
                phantom: {
                    blockJavaScript: true,
                    waitForJS: false,
                    allowLocalFilesAccess: true
                }
            },
            data: {
                html: theData.data,
                delay: theData.delay
            }
        }).then(function (resp) {
            console.log("Successfully exported group to pdf");
            callback(/* error */ null, resp.content.toJSON().data);
        });
    }).catch(function (e) {
        console.log("Error exporting group to pdf: " + e);
        callback(/* error */ e, null);
    });
};