﻿define(['jquery'], function ($) {
    var addCookie = function (slug, cookieType) {
        var url = "/cookies/add?slug=" + slug + "&cookieType=" + cookieType;
        $.post(url, function (data, status) { });
    };

    let hasInitialized = false;

    return {
        Init: function () {
            if (!hasInitialized) {
                hasInitialized = true;

                $(".close-alert").click(function () {
                    var slug = $(this).attr("data-slug");
                    var cookieType = "alert";
                    addCookie(slug, cookieType);
                    var parent = $(this).attr("data-parent");
                    $(this).closest("." + parent).hide();
                }); 

                $(".dismiss a").click(function () {
                    var slug = $(this).attr("data-slug");
                    var cookieType = "alert";
                    addCookie(slug, cookieType);

                    var alertBox = $(this).closest("." + $(this).attr("data-parent"));
                    var alertBoxContainer = alertBox.parent();

                    alertBox.hide();

                    var visibleChildren = alertBoxContainer.children(':visible');
                    if (visibleChildren.length === 0) {
                        alertBoxContainer.hide();
                    }
                });
            }
        }
    };
});
