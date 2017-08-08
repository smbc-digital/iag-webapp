define(["jquery"], function (jQuery) {

    var pageLoad = function () {

        var mapDiv = $('atMap');
        var loader = {};
        loader.mapID = 'atMap';

        Event.observe(mapDiv, 'astun:sourceLoad', function (evt) {
            astun.data = evt;

            for (var i = 0; i < iShareData.panels.length; i++) {
                iShareData.panels[i].pluginData = evt;
            }

            options = {
                panelWrapperTopPosition: 2,
                attachPanelWrapperTo: '#' + mapDiv.id
            };

            jqueryFunctions(jQuery, mapDiv, iShareData.panels, options);

            // Attempt to ensure the popup is not obscured by base map buttons
            astun.mapWrapper.map.paddingForPopups.right = 120;

            // Remove the PanZoomBar control, we will add a set of plain links which work reliably on all devices
            astun.mapWrapper.map.removeControl(
                jQuery.grep(astun.mapWrapper.map.controls, function (control, idx) {
                    return control.displayClass === 'olControlPanZoomBar'
                })[0]
            );

            var isSmallWindow = function () {
                return jQuery(window).width() < 740;
            };

            // New bolder menu toggle button
            jQuery('<div id="atPanelToggle" class="atPanelToggle"><a href="#" title="Menu">&nbsp;</a></div>').appendTo('.atPanelContent.olMap').click(function () {
                if (isSmallWindow()) {
                    openDialog();
                } else {
                    fixPanelPosition()
                    // Toggle the panel
                    jQuery('.atJqOpenClose').click();
                }
                return false;
            });

            fixPanelPosition();

            jQuery('<div id="atZoomControls" class="atZoomControls"><a href="#zoomin" class="zoomin" title="Zoom in">+</a><a href="#zoomout" class="zoomout" title="Zoom out">-</a></div>').appendTo('.atPanelContent.olMap').click(function (e) {
                if (e.target.hash === '#zoomin') {
                    astun.mapWrapper.map.zoomIn()
                } else {
                    astun.mapWrapper.map.zoomOut()
                }
                return false;
            });
        });

        mapDiv.map = new Astun.JS.Map(loader.mapID, {
            mapSource: iShareData.mapSource, layers: iShareData.layers
        });

        window.astun.mapWrapper = mapDiv.map;
        mapDiv.mapSettings = Astun.JS.getConfiguration();
        mapDiv.addressFinder = new Astun.JS.GetData.AddressFinder(mapDiv.map.mapElement, mapDiv.mapSettings.dataUrl);
    };

    var fixPanelPosition = function () {
        // Ensure the vertical positon of the panel is correct
        //$('.atPanelWrapperParent').css('top', Math.round(jQuery('#atMap').offset().top));
    };

    var init = function () {
        Event.observe(window, 'load', function () { Astun.JS.IncludeJS('solo', pageLoad); });
        if (!window.astun) { window.astun = {}; } window.astun.settings = { themeName: 'base' };
    };

    return {
        Init: init
    };
});
