﻿define(["jquery"], function ($) {

    var init = function () {

        $('.event-tracking').on('click', function () {

            var category = $(this)[0].dataset['category'];
            var action = $(this)[0].dataset['action'];
            var label = $(this)[0].dataset['label'];

            if (typeof (ga) !== 'undefined') {
                ga('send', {
                    hitType: 'event',
                    eventCategory: category,
                    eventAction: action,
                    eventLabel: label
                });
            }

            if (typeof (_sz) !== 'undefined') {
                _sz.push(['event', category, action, label]);
            }
        });

    };

    return {
        Init: init
    };

});