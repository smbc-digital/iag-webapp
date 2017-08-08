define([], function () {

    var controllerConstructor = function (astunWrapper, view) {

        var init = function () {
            astunWrapper.loadAstun(onAstunLoaded);
        };

        var onAstunLoaded = function () {
            astunWrapper.initialiseMap(onAstunMapInitialised);
        };

        var onAstunMapInitialised = function () {
            astunWrapper.removePanZoomControl();
            view.hideUnwantedControls();
            view.addZoomControls(onZoomIn, onZoomOut);
            view.addCopyrightInfomation();
            view.configureTabBehaviourOfMap();
            view.interceptMapClick(onPopupClosed);
            astunWrapper.zoomTo(1);
            astunWrapper.stopEventsFromLiveSearchBleedingOutToAstun('.live-search');
        };

        var onZoomIn = function () {
            astunWrapper.zoomIn();
        };

        var onZoomOut = function () {
            astunWrapper.zoomOut();
        };

        var onHashChanged = function (hash) {
            var baseUrl = window.location.origin;
        };

        var onPopupClosed = function () {
            view.fadeOutPopup();
        };

        return {
            init: init,
            onAstunLoaded: onAstunLoaded,
            onAstunMapInitialised: onAstunMapInitialised,
            onZoomIn: onZoomIn,
            onZoomOut: onZoomOut,
            onHashChanged: onHashChanged,
            onPopupClosed: onPopupClosed
        };
    };

    return controllerConstructor;
});
