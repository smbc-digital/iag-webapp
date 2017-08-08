define(['jquery'], function (jQuery) {
    // there's some dragons here...
    // the jQuery object is a reference to jQuery 1.6.4 that astun dropped on the page with the call in to InculdeJS in the loadAstun method below
    // there's also proptotype bound to $ in scope too

    var astunWrapperConstructor = function (config) {
        var removePanZoomControl = function () {
        };

        return {
            loadAstun: function (onAstunLoaded) {
                window.astun = window.astun || {};
                window.astun.settings = {
                    themeName: 'base'
                };
                // assumes presence of prototype and JSInclude script tags on the page
                Astun.JS.IncludeJS('solo', onAstunLoaded);
            },
            initialiseMap: function (onAstunMapInitialised) {
                var mapDiv = $('atMap');

                Event.observe(mapDiv, 'astun:sourceLoad', onAstunMapInitialised);

                mapDiv.map = new Astun.JS.Map('atMap', { mapSource: config.mapSource });
                window.astun.mapWrapper = mapDiv.map;
                mapDiv.mapSettings = Astun.JS.getConfiguration();
            },

            removePanZoomControl: function () {
                astun.mapWrapper.map.removeControl(
                    jQuery.grep(astun.mapWrapper.map.controls, function (control, idx) {
                        return control.displayClass === 'olControlPanZoomBar';
                    })[0]
                );
            },

            zoomIn: function () {
                window.astun.mapWrapper.map.zoomIn();
            },

            zoomOut: function () {
                window.astun.mapWrapper.map.zoomOut();
            },

            zoomTo: function (zoomLevel) {
                window.astun.mapWrapper.map.zoomTo(zoomLevel);
            },

            centreMapOn: function (easting, northing) {
                jQuery('#atMap').trigger('mapMove', [{ easting: easting, northing: northing, zoom: 500 }]);
            },

            stopEventsFromLiveSearchBleedingOutToAstun: function (selector) {
                jQuery(selector).keydown(function (e) { e.stopPropagation(); });
            }
        };
    };

    return astunWrapperConstructor;
});
