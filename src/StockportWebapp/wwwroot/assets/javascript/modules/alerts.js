define(['jquery'], function ($) {
    let hasInitialized = false;

    return {
        Init: function () {
            if (!hasInitialized) {
                hasInitialized = true;

                $(".close-alert").click(function () {
                    var parent = $(this).attr("data-parent");
                    $(this).closest(`.${parent}`).hide();
                });

                $(".dismiss a").click(function () {
                    const alertBox = $(this).closest(`.${$(this).attr("data-parent")}`);
                    var alertBoxContainer = alertBox.parent();

                    alertBox.hide();

                    var visibleChildren = alertBoxContainer.children(':visible');
                    if (visibleChildren.length === 0)
                        alertBoxContainer.hide();
                });

                $(".global-alert-close").click(function () {
                    const alertBox = $(this).closest(`.${$(this).attr("data-parent")}`);
                    var alertBoxContainer = alertBox.parent();

                    alertBox.hide();

                    var visibleChildren = alertBoxContainer.children(':visible');
                    if (visibleChildren.length === 0)
                        alertBoxContainer.hide();
                });
            }
        }
    };
});
