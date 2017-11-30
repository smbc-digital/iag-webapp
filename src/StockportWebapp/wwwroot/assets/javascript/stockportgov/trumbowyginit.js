define(["jquery", "trumbowyg"], function ($, trumbowyg) {
    
        var init = function () {
            $(document).ready(function() {
                $('.wysiwyg').trumbowyg({
                    btns: [
                        ['strong'],
                        ['link'],
                        ['unorderedList', 'orderedList']
                    ],
                    svgPath: '/lib/trumbowyg/icons.svg'
                });
            });
        };
    
        return {
            Init: init
        };
    
    });