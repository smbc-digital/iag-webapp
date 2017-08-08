define(['jquery'], function ($) {

    var assetsConstructor = function () {

        var insertionPoint = '.atPanelContent.olMap';

        return {
            hideUnwantedControls: function () {
                $('.olControlScaleLine').hide();
                $('.atBaseMapSwitcher').hide();
                $('.olControlAttribution').hide();
            },
            addZoomControls: function (onZoomIn, onZoomOut) {
                $('<div id="atZoomControls" class="atZoomControls">' +
                    '<button class="button-icon-small zoomin" type="submit"><i class="fa fa-plus fa-2x" aria-hidden="true"></i></button>' +
                    '<button class="button-icon-small zoomout" type="submit"><i class="fa fa-minus fa-2x" aria-hidden="true"></i></button>' +
                    '</div>')
                    .appendTo(insertionPoint);

                $(".zoomin").click(function (e) {
                    onZoomIn();
                    return false;
                });

                $(".zoomout").click(function (e) {
                    onZoomOut();
                    return false;
                });
            },
            addCopyrightInfomation: function () {
                var d = new Date();
                $('<div id="copyrightInfomation" class="copyrightInfomation"><p> \u00A9 Crown copyright and database rights ' +
                    d.getFullYear() +
                    ' Ordnance Survey 100019571</p>' +
                    '</div>')
                    .appendTo(insertionPoint);
            },
            configureTabBehaviourOfMap: function () {
                $(".live-search").ready(function (e) {

                    var fadeSearchResultsOut = function () {
                        searchResultFadeOutTimeout = window.setTimeout(function () {
                            $(".search-result").fadeOut();
                        }, 300);
                    };

                    var clearSearchResultFadeOutTimeout = function () {
                        if (searchResultFadeOutTimeout) {
                            window.clearTimeout(searchResultFadeOutTimeout);
                        }
                    }

                    var fadeSearchBarAndResultsOutWhenLosingFocus = function () {
                        $(this).focusout(function (e) {
                            fadeSearchResultsOut();
                        });
                        $(".olMapViewport").mousedown(function (e) {
                            fadeSearchResultsOut();
                        });
                    }

                    var preventSearchBarAndResultsFromFadingOutOnSearchButtonOrSearchResultsFocus = function () {
                        $("button.search-button").focus(function () {
                            clearSearchResultFadeOutTimeout();
                        });

                        $(".search-result").focusin(function () {
                            clearSearchResultFadeOutTimeout();
                        });
                    }

                    var preventJumpingToTopOfPageOnHashClick = function () {
                        $(".search-result").find("a").click(function (e) {
                            e.preventDefault();
                        });
                    }

                    var makeSearchResultsVisibleOnSearchButtonClick = function () {
                        $(".search-button").click(function (e) {
                            $(".search-result").fadeIn();
                        });
                    }

                    fadeSearchBarAndResultsOutWhenLosingFocus();
                    preventSearchBarAndResultsFromFadingOutOnSearchButtonOrSearchResultsFocus();
                    preventJumpingToTopOfPageOnHashClick();
                    makeSearchResultsVisibleOnSearchButtonClick();
                });
            },
            interceptMapClick: function (onPopupClosed) {
                $('#atMap').on('DOMNodeInserted', '.olPopup', function (e) {
                    var el = $(e.target);
                    var elementContainsQuadrantInfo = el.html().indexOf('olQuadrant') >= 0;
                    var insertionPoint = el.children().first();

                    // We have already rendered the popup, prevent a circular reference by preventing further DOM insertion events from proceeding
                    // IE fix - inserts assetsDefaultPopup div inside last 2 elements if we do not check for report-a-fault (twice)
                    if (el.hasClass('dtsAssetPopup') || el.hasClass('report-a-fault') || el.children().first().hasClass('report-a-fault')) {
                        return;
                    }

                    // Intercept assetsDefaultPopup insert
                    if (el.hasClass('assetsDefaultPopup')) {
                        el.empty();
                        return;
                    }

                    // The DOMNodeInserted event will fire twice. The first time no quadrant information is present. We want to test for this and ensure we only proceed
                    // when quadrant information is present. This enables us to determine how to position the popupContainer (bottom right, top right, etc).
                    if (!elementContainsQuadrantInfo) {

                        // The Astun Map layer creates a div whose size and position is determined by the content inserted on the first pass (does not contain quadrant info).
                        // This div is then positioned next to the selected asset on the map with a fixed position and size.
                        // Therefore, it is necessary to insert a DIV at this point which is big enough to contain our customised popup.
                        // If this step is ommitted, our custom popup content may be too large for the container and will not be displayed correctly
                        insertionPoint.html('<div class="assetsDefaultPopup"></div>');
                        return;
                    }

                    // Display overflow on parent, so that we can move the popup outside of it and display a caret next to the lamp post
                    el.css('overflow', 'visible');
                    insertionPoint.css('overflow', 'visible');

                    // Process close button click
                    $(smbcPopupContainer).find('i.close').on('click', onPopupClosed);

                    insertionPoint.replaceWith(smbcPopupContainer);
                });
            },
            fadeOutPopup: function () {
                $('.olPopup').fadeOut();
            }
        };
    };

    return assetsConstructor;
});
