var SMART = {};

// Launching fullscreen overlays
$('.launch-fullscreen-overlay[data-overlay]').on( 'click', function() {
   var overlayData = $(this).data('overlay'),
       $target = $('#'+overlayData),
       triggerEvent = 'launch-overlay-'+overlayData;
   $('body').addClass('overlay-open no-scroll').trigger(triggerEvent);

   if ( ! $('body').hasClass('container-layout-free') ) {
      var $wrapAll = $('#container .wrap-all').eq(0),
          width = $wrapAll.outerWidth(),
          left = $wrapAll.offset().left,
          css = { width: width, left: left, };
      $target.css(css);
      $target.find('.button-close').css(css);
   }

   $target.addClass('is-active');
   setTimeout( function() {
      $target.addClass('is-open');
   }, 10 );
});

// Closing Fullscreen Overlays
$('.fullscreen-overlay .button-close').on( 'click', function() {
   var $overlay = $(this).parent().trigger('close');
   $overlay.removeClass('is-open');
   setTimeout( function() {
      $overlay.removeClass('is-active');
      $('body').removeClass('no-scroll overlay-open');
   }, 500);
});

$('.alert-close a')
    .on('click',
        function() {
            $(this).closest('.alert').hide();            
            if ($('.alert:visible').length === 0) {
                $('.alert-container').css('margin-bottom', '0');
            }
        });

if ($('.alert:visible').length === 0) {
    $('.alert-container').css('margin-bottom', '0');
}

$(function () {

    // Find all YouTube videos
    var $allVideos = $("iframe[src^='https://www.youtube.com']"),

	    // The element that is fluid width
	    $fluidEl = $("article");

    // Figure out and save aspect ratio for each video
    $allVideos.each(function () {

        $(this)
			.data('aspectRatio', this.height / this.width)

			// and remove the hard coded width/height
			.removeAttr('height')
			.removeAttr('width');

    });

    // When the window is resized
    // (You'll probably want to debounce this)
    $(window).resize(function () {

        var newWidth = $fluidEl.width();

        // Resize all videos according to their own aspect ratio
        $allVideos.each(function () {

            var $el = $(this);
            $el
				.width(newWidth)
				.height(newWidth * $el.data('aspectRatio'));

        });

        // Kick off one resize to fix all videos on page load
    }).resize();

});

$(document).ready(
    function () {
        // only apply on mobile
        var mobileWidth = 767;

        if ($(window).width() <= mobileWidth) {
            $("#edit-search").hide();
            $(".result-arrow").addClass("result-search-down-arrow");

            $("#open-edit-search").click(function() {
                $("#edit-search").show();
                $(".result-arrow").toggleClass("result-search-down-arrow");
                $(".result-arrow").toggleClass("result-search-up-arrow");

                $(".result-search-down-arrow").parent().click(function() {
                    $("#edit-search").show();
                });
                $(".result-search-up-arrow").parent().click(function() {
                    $("#edit-search").hide();
                });
            });

            $(".result-search-up-arrow").parent().click(function () {
                $("#edit-search").hide();
            });
        }

        $(".print-this")
       .click(
           function () {
               window.print();
           }
       );

    }
);



// location defaults
var LocationDefaults = {
    Name: "Stockport",
    Latitude: 53.405817,
    Longitude: -2.158046,
    CurrentLocationError: "We couldn't find your current location -- please check the location settings on your device.",
    LocationLookupError: "We couldn't find this location -- please check the location and try again.",
    NoLocationError: "No location entered -- please enter a location and try again."
};

// location object
var Location = {
    Name: LocationDefaults.Name,
    Latitude: LocationDefaults.Latitude,
    Longitude: LocationDefaults.Longitude,
    HasBeenUpdated: false,
    ShowLocationError: function(error) {
        $("#location-autocomplete").css("border", "1px solid #C83725");
        $("#locationError").html(error);
        $("#locationError").show();
    },
    HideLocationError: function () {
        $("#location-autocomplete").css("border", "1px solid #D8D8D8");
        $("#locationError").hide();
    },
    SetLocationValues: function () {
        // take what has been filled in on the autocomplete and use those values
        $("#postcode").val(this.Name);
        $("#address").val(this.Name);
        $("#latitude").val(this.Latitude);
        $("#longitude").val(this.Longitude);
        UpdateLocationFieldSize();
        $("#getLocation").hide();
    },
    SetLocation: function(name, latitude, longitude) {
        this.Name = name;
        this.Longitude = longitude;
        this.Latitude = latitude;
        this.HasBeenUpdated = true;
    },
    GetLocationInputValue: function() {
        return $("#location-autocomplete").val();
    },
    SetLocationInputValue: function(value) {
        $("#location-autocomplete").val(value);
    }
};

$(window).resize(function () {
    // only apply on tablet and desktop
    if ($(window).width() > 767) {
        $("#hiddenSelectCategory").html("<option>" + $("#selectCategory").find(":selected").text() + "</option>");
        $("#selectCategory").width($("#hiddenSelectCategory").width());
        $("#hiddenSelectOrder").html("<option>" + $("#selectOrder").find(":selected").text() + "</option>");
        $("#selectOrder").width($("#hiddenSelectOrder").width());
        $("#hiddenSelectLocation").html("<option>" + $("#postcode").val() + "</option>");
        $("#postcode").width($("#hiddenSelectLocation").width());
    }
});

$(document).ready(function () {
    $("#selectCategory").change(function () {
        $("#hiddenSelectCategory").html("<option>" + $("#selectCategory").find(":selected").text() + "</option>");
        $("#selectCategory").width($("#hiddenSelectCategory").width());
    });

    $("#selectOrder").change(function () {
        $("#hiddenSelectOrder").html("<option>" + $("#selectOrder").find(":selected").text() + "</option>");
        $("#selectOrder").width($("#hiddenSelectOrder").width());
    });

    $("#postcode").change(function () {
        $("#hiddenSelectLocation").html("<option>" + $("#postcode").val() + "</option>");
        $("#postcode").width($("#hiddenSelectLocation").width());
    });

    $("#selectOrderMobile").change(function () {
        var orderby = $("#selectOrderMobile").find(":selected").text();
        $("#selectOrder").val(orderby);
        $("#filterButton").click();
    });

    // open the "location search" box
    $("#postcode").click(function () {
            $("#getLocation").toggle();
    });

    if ($('.location-search-input-autoset').length) { $('.location-search-input-autoset').val($('#address').val()); }

    // get current location
    $("#currentLocation").click(function () {
        navigator.geolocation.getCurrentPosition(
        function (position) {
            var geocoder = new google.maps.Geocoder();
            var latLng = new google.maps.LatLng(
                position.coords.latitude,
                position.coords.longitude);
            geocoder.geocode({
                'latLng': latLng
            },
            function (results, status) {
                if (status === google.maps.GeocoderStatus.OK) {
                    var jointLocation = buildLocation(results[0].address_components);

                    Location.SetLocationInputValue(jointLocation);
                    Location.SetLocation(jointLocation, results[0].geometry.location.lat(), results[0].geometry.location.lng());
                    Location.HideLocationError();
                }
                else {
                    Location.ShowLocationError(LocationDefaults.CurrentLocationError);
                }
            });
        },
        function () {
            Location.ShowLocationError(LocationDefaults.CurrentLocationError);
        },
        { maximumAge: 10000, timeout: 6000, enableHighAccuracy: true });
        return false;
    });

    // use location click
    $("#btnLocationAutoComplete").click(function (event) {
        event.preventDefault();
        var location = Location.GetLocationInputValue();
        if (location === "") {
            Location.ShowLocationError(LocationDefaults.NoLocationError);
            return false;
        }
        // check if location values have been set via autocomplete
        if (Location.HasBeenUpdated === true) {
            Location.SetLocationValues();
            Location.HideLocationError();
        } else {
            // perform a lookup on the location in the textbox
            LocationLookupNonAutocomplete();
        }
    });

    if ($('.location-search-input-autoset').length) {
        addValidationErrorToMeetingLocationField();

        $('.location-search-input-autoset').on('change', function () {
            $("#address").val('');
            $("#latitude").val(Location.Latitude);
            $("#longitude").val(Location.Longitude);
        });

        $('form').on('invalid-form.validate', function (event, validator) {
            for (var i = 0; i < validator.errorList.length; i++) {
                if (validator.errorList[i].element.id == 'address') {
                    $('#location-autocomplete').addClass('input-validation-error');
                    $("[data-valmsg-for='Address']").show();
                }
            }
        });
    }

    // only run of the auto complete is on the page
    if ($(".primary-filter-form-autocomplete").length || $('.location-search-input').length) {
        // Set the default bounds to the UK
        var defaultBounds = new google.maps.LatLngBounds(
            new google.maps.LatLng(49.383639452689664, -17.39866406249996),
            new google.maps.LatLng(59.53530451232491, 8.968523437500039));

        var options = {
            bounds: defaultBounds,
            // the type of location we want to return
            types: ['locality', 'postal_code', 'sublocality', 'country', 'administrative_area_level_1', 'administrative_area_level_2'],
            // the country to return results, the bounds above seemed to also be needed and not just this though
            // this isn't 100% though and is just a suggestion to first look in gb
            componentRestrictions: { country: 'gb' }
        };

        var searchBox = new google.maps.places.SearchBox(document.getElementById('location-autocomplete'), options);

        // Listen for the event fired when the user selects a prediction and retrieve more details for that place.
        searchBox.addListener('places_changed', function () {
            var places = searchBox.getPlaces();

            if (places.length === 0) {
                return;
            }

            var autocompleteName = places[0].name;
            if ($('.location-search-input-autoset').length) {

                if (typeof(places[0].formatted_address) !== 'undefined') {
                    autocompleteName = places[0].formatted_address.replace(', UK', '').replace(', United Kingdom', '');
                    if (autocompleteName.indexOf(places[0].name) < 0) {
                        autocompleteName = places[0].name + ', ' + autocompleteName;
                    }
                }

                $('#location-autocomplete').removeClass('input-validation-error');
                $("[data-valmsg-for='Address']").hide();
            }

            Location.SetLocation(autocompleteName, places[0].geometry.location.lat(), places[0].geometry.location.lng());
            Location.HideLocationError();

            if ($('.location-search-input-autoset').length) {
                Location.SetLocationValues();
            }
        });
    }
});

var extractFromAdress = function (components, type) {
    for (var i = 0; i < components.length; i++)
        for (var j = 0; j < components[i].types.length; j++)
            if (components[i].types[j] === type) return components[i].long_name;
    return "";
}

var UpdateLocationFieldSize = function () {
    $("#hiddenSelectLocation").html("<option>" + $("#postcode").val() + "</option>");
    $("#postcode").width($("#hiddenSelectLocation").width());
};

var buildLocation = function (addressComponents) {
    // take the address components and build a nice address from them
    var street = extractFromAdress(addressComponents, "route");
    var postcode = extractFromAdress(addressComponents, "postal_code");
    var city = extractFromAdress(addressComponents, "locality");
    var country = extractFromAdress(addressComponents, "country");
    var joinedLocation = (street + " " + postcode + " " + city).trim();

    if (joinedLocation === "") {
        // only add the country into the locaion if nothing else comes back for the location
        joinedLocation = country;
    }

    return joinedLocation;
};

var addValidationErrorToMeetingLocationField = function () {
    if ($("#address").hasClass('input-validation-error')) {
        $('#location-autocomplete').addClass('input-validation-error');
    }
}

// get location lookup, non auto complete
var LocationLookupNonAutocomplete = function () {
    var address = Location.GetLocationInputValue();
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'address': address + ", UK" }, function (results, status) {
        if (status === google.maps.GeocoderStatus.OK) {
            Location.SetLocation(buildLocation(results[0].address_components), results[0].geometry.location.lat(), results[0].geometry.location.lng());
            Location.SetLocationValues();
            Location.HideLocationError();
        } else {
            Location.ShowLocationError(LocationDefaults.LocationLookupError);
        }
    });
};
$(document)
    .ready(function () {
        $(".expanding-Link-items").addClass("is-collapsed");

        $(".expanding-Link-text").click(function () {
            if (!$('.expanding-Link-items:visible', this).length) {
                $('.expanding-Link-items').hide();
                $('.expanding-link-box-button').removeClass("is-collapsed-toggle");
            }

            $(this).children('.expanding-Link-items').slideToggle("fast");
            $(this).children('.expanding-link-box-button').toggleClass("is-collapsed-toggle");
        });
    });
$(".carousel a").css("display", "block");
$(".carousel div").css("display", "block");
$(document).ready(
        function() {
          $(".carousel").slick(
           {
               arrows: true,
               infinite: true,
               slidesToShow: 1,
               slidesToScroll: 1,
               dots: true,
               autoplay: true,
               autoplaySpeed: 5000});
    }
);

$(document).ready(function () {
    var CludoSearch;
    (function () {
        var cludoSettings = {
            customerId: 112,
            engineId: 1144,
            type: 'standardOverlay',
            hideSearchFilters: true,
            initSearchBoxText: '',
            searchInputs: ["cludo-search-form","cludo-search-mobile-form", "cludo-search-hero-form"],
            theme: { themeColor: '#055c58', themeBannerColor: {textColor: '#333', backgroundColor: '#f2f2f2'}, borderRadius: 10},
            language: 'en'
        };
        CludoSearch= new Cludo(cludoSettings);
        CludoSearch.init();
    })();
})
jQuery(document).ready(function ($) {
    SetupCharsRemaining();
    OnlySubmitFormIfValid();
});

function SetupCharsRemaining() {

    $(".chars-remaining").each(function (idx, elem) {

        $input = $(elem).siblings(".form-control-deep");
            
        var limit = $input.data("val-length-max");
           
        DisplayCharsRemaining($input, limit);

        $input.on("keyup", function () { DisplayCharsRemaining(this, limit); });
    });
}


function DisplayCharsRemaining(input, limit) {

    var charsRemaining = NumberOfRemainingCharacters(input, limit);

    if (charsRemaining < 0) {
        charsRemaining *= -1;
        $input.siblings(".chars-remaining").html("<small class='error-text'>" + charsRemaining + " characters over the limit</small>");
    }
    else {
        $input.siblings(".chars-remaining").html("<small>" + charsRemaining + " characters remaining</small>");
    }
}

function NumberOfRemainingCharacters(input, limit) {
    var $input = $(input);
    var inputText = $input.val();
    var maxLength = parseInt(limit);

    var newLines = inputText.match(/(\n)/g);

    var addition = 0;
    if (newLines != null) {
        addition = newLines.length;
    }

    var currentLength = inputText.length + addition;
    var charsRemaining = maxLength - currentLength;

    return charsRemaining;
}

function OnlySubmitFormIfValid() {
    $("form")
        .submit(function (e) {
            console.log("form submission triggered");
            var chars = $(".chars-remaining");
            if (chars.length > 0) {
                $input = chars.siblings(".form-control-deep");
                var limit = $input.data("val-length-max");

                if (NumberOfRemainingCharacters($input, limit) < 0) {
                    e.preventDefault();
                }
            }
        });
}
// TODO: in future try to replace most inline compability checks with polyfills for code readability 

// element.textContent polyfill.
// Unsupporting browsers: IE8

if (Object.defineProperty && Object.getOwnPropertyDescriptor && Object.getOwnPropertyDescriptor(Element.prototype, "textContent") && !Object.getOwnPropertyDescriptor(Element.prototype, "textContent").get) {
	(function() {
		var innerText = Object.getOwnPropertyDescriptor(Element.prototype, "innerText");
		Object.defineProperty(Element.prototype, "textContent",
			{
				get: function() {
					return innerText.get.call(this);
				},
				set: function(s) {
					return innerText.set.call(this, s);
				}
			}
		);
	})();
}

// isArray polyfill for ie8
if(!Array.isArray) {
  Array.isArray = function(arg) {
    return Object.prototype.toString.call(arg) === '[object Array]';
  };
};/**
 * @license wysihtml5x v0.4.13
 * https://github.com/Edicy/wysihtml5
 *
 * Author: Christopher Blum (https://github.com/tiff)
 * Secondary author of extended features: Oliver Pulges (https://github.com/pulges)
 *
 * Copyright (C) 2012 XING AG
 * Licensed under the MIT license (MIT)
 *
 */
var wysihtml5 = {
  version: "0.4.13",

  // namespaces
  commands:   {},
  dom:        {},
  quirks:     {},
  toolbar:    {},
  lang:       {},
  selection:  {},
  views:      {},

  INVISIBLE_SPACE: "\uFEFF",

  EMPTY_FUNCTION: function() {},

  ELEMENT_NODE: 1,
  TEXT_NODE:    3,

  BACKSPACE_KEY:  8,
  ENTER_KEY:      13,
  ESCAPE_KEY:     27,
  SPACE_KEY:      32,
  DELETE_KEY:     46
};
;/**
 * Rangy, a cross-browser JavaScript range and selection library
 * http://code.google.com/p/rangy/
 *
 * Copyright 2014, Tim Down
 * Licensed under the MIT license.
 * Version: 1.3alpha.20140804
 * Build date: 4 August 2014
 */

(function(factory, global) {
    if (typeof define == "function" && define.amd) {
        // AMD. Register as an anonymous module.
        define(factory);
/*
    TODO: look into this properly.
    
    } else if (typeof exports == "object") {
        // Node/CommonJS style for Browserify
        module.exports = factory;
*/
    } else {
        // No AMD or CommonJS support so we place Rangy in a global variable
        global.rangy = factory();
    }
})(function() {

    var OBJECT = "object", FUNCTION = "function", UNDEFINED = "undefined";

    // Minimal set of properties required for DOM Level 2 Range compliance. Comparison constants such as START_TO_START
    // are omitted because ranges in KHTML do not have them but otherwise work perfectly well. See issue 113.
    var domRangeProperties = ["startContainer", "startOffset", "endContainer", "endOffset", "collapsed",
        "commonAncestorContainer"];

    // Minimal set of methods required for DOM Level 2 Range compliance
    var domRangeMethods = ["setStart", "setStartBefore", "setStartAfter", "setEnd", "setEndBefore",
        "setEndAfter", "collapse", "selectNode", "selectNodeContents", "compareBoundaryPoints", "deleteContents",
        "extractContents", "cloneContents", "insertNode", "surroundContents", "cloneRange", "toString", "detach"];

    var textRangeProperties = ["boundingHeight", "boundingLeft", "boundingTop", "boundingWidth", "htmlText", "text"];

    // Subset of TextRange's full set of methods that we're interested in
    var textRangeMethods = ["collapse", "compareEndPoints", "duplicate", "moveToElementText", "parentElement", "select",
        "setEndPoint", "getBoundingClientRect"];

    /*----------------------------------------------------------------------------------------------------------------*/

    // Trio of functions taken from Peter Michaux's article:
    // http://peter.michaux.ca/articles/feature-detection-state-of-the-art-browser-scripting
    function isHostMethod(o, p) {
        var t = typeof o[p];
        return t == FUNCTION || (!!(t == OBJECT && o[p])) || t == "unknown";
    }

    function isHostObject(o, p) {
        return !!(typeof o[p] == OBJECT && o[p]);
    }

    function isHostProperty(o, p) {
        return typeof o[p] != UNDEFINED;
    }

    // Creates a convenience function to save verbose repeated calls to tests functions
    function createMultiplePropertyTest(testFunc) {
        return function(o, props) {
            var i = props.length;
            while (i--) {
                if (!testFunc(o, props[i])) {
                    return false;
                }
            }
            return true;
        };
    }

    // Next trio of functions are a convenience to save verbose repeated calls to previous two functions
    var areHostMethods = createMultiplePropertyTest(isHostMethod);
    var areHostObjects = createMultiplePropertyTest(isHostObject);
    var areHostProperties = createMultiplePropertyTest(isHostProperty);

    function isTextRange(range) {
        return range && areHostMethods(range, textRangeMethods) && areHostProperties(range, textRangeProperties);
    }

    function getBody(doc) {
        return isHostObject(doc, "body") ? doc.body : doc.getElementsByTagName("body")[0];
    }

    var modules = {};

    var api = {
        version: "1.3alpha.20140804",
        initialized: false,
        supported: true,

        util: {
            isHostMethod: isHostMethod,
            isHostObject: isHostObject,
            isHostProperty: isHostProperty,
            areHostMethods: areHostMethods,
            areHostObjects: areHostObjects,
            areHostProperties: areHostProperties,
            isTextRange: isTextRange,
            getBody: getBody
        },

        features: {},

        modules: modules,
        config: {
            alertOnFail: true,
            alertOnWarn: false,
            preferTextRange: false,
            autoInitialize: (typeof rangyAutoInitialize == UNDEFINED) ? true : rangyAutoInitialize
        }
    };

    function consoleLog(msg) {
        if (isHostObject(window, "console") && isHostMethod(window.console, "log")) {
            window.console.log(msg);
        }
    }

    function alertOrLog(msg, shouldAlert) {
        if (shouldAlert) {
            window.alert(msg);
        } else  {
            consoleLog(msg);
        }
    }

    function fail(reason) {
        api.initialized = true;
        api.supported = false;
        alertOrLog("Rangy is not supported on this page in your browser. Reason: " + reason, api.config.alertOnFail);
    }

    api.fail = fail;

    function warn(msg) {
        alertOrLog("Rangy warning: " + msg, api.config.alertOnWarn);
    }

    api.warn = warn;

    // Add utility extend() method
    if ({}.hasOwnProperty) {
        api.util.extend = function(obj, props, deep) {
            var o, p;
            for (var i in props) {
                if (props.hasOwnProperty(i)) {
                    o = obj[i];
                    p = props[i];
                    if (deep && o !== null && typeof o == "object" && p !== null && typeof p == "object") {
                        api.util.extend(o, p, true);
                    }
                    obj[i] = p;
                }
            }
            // Special case for toString, which does not show up in for...in loops in IE <= 8
            if (props.hasOwnProperty("toString")) {
                obj.toString = props.toString;
            }
            return obj;
        };
    } else {
        fail("hasOwnProperty not supported");
    }

    // Test whether Array.prototype.slice can be relied on for NodeLists and use an alternative toArray() if not
    (function() {
        var el = document.createElement("div");
        el.appendChild(document.createElement("span"));
        var slice = [].slice;
        var toArray;
        try {
            if (slice.call(el.childNodes, 0)[0].nodeType == 1) {
                toArray = function(arrayLike) {
                    return slice.call(arrayLike, 0);
                };
            }
        } catch (e) {}

        if (!toArray) {
            toArray = function(arrayLike) {
                var arr = [];
                for (var i = 0, len = arrayLike.length; i < len; ++i) {
                    arr[i] = arrayLike[i];
                }
                return arr;
            };
        }

        api.util.toArray = toArray;
    })();


    // Very simple event handler wrapper function that doesn't attempt to solve issues such as "this" handling or
    // normalization of event properties
    var addListener;
    if (isHostMethod(document, "addEventListener")) {
        addListener = function(obj, eventType, listener) {
            obj.addEventListener(eventType, listener, false);
        };
    } else if (isHostMethod(document, "attachEvent")) {
        addListener = function(obj, eventType, listener) {
            obj.attachEvent("on" + eventType, listener);
        };
    } else {
        fail("Document does not have required addEventListener or attachEvent method");
    }

    api.util.addListener = addListener;

    var initListeners = [];

    function getErrorDesc(ex) {
        return ex.message || ex.description || String(ex);
    }

    // Initialization
    function init() {
        if (api.initialized) {
            return;
        }
        var testRange;
        var implementsDomRange = false, implementsTextRange = false;

        // First, perform basic feature tests

        if (isHostMethod(document, "createRange")) {
            testRange = document.createRange();
            if (areHostMethods(testRange, domRangeMethods) && areHostProperties(testRange, domRangeProperties)) {
                implementsDomRange = true;
            }
        }

        var body = getBody(document);
        if (!body || body.nodeName.toLowerCase() != "body") {
            fail("No body element found");
            return;
        }

        if (body && isHostMethod(body, "createTextRange")) {
            testRange = body.createTextRange();
            if (isTextRange(testRange)) {
                implementsTextRange = true;
            }
        }

        if (!implementsDomRange && !implementsTextRange) {
            fail("Neither Range nor TextRange are available");
            return;
        }

        api.initialized = true;
        api.features = {
            implementsDomRange: implementsDomRange,
            implementsTextRange: implementsTextRange
        };

        // Initialize modules
        var module, errorMessage;
        for (var moduleName in modules) {
            if ( (module = modules[moduleName]) instanceof Module ) {
                module.init(module, api);
            }
        }

        // Call init listeners
        for (var i = 0, len = initListeners.length; i < len; ++i) {
            try {
                initListeners[i](api);
            } catch (ex) {
                errorMessage = "Rangy init listener threw an exception. Continuing. Detail: " + getErrorDesc(ex);
                consoleLog(errorMessage);
            }
        }
    }

    // Allow external scripts to initialize this library in case it's loaded after the document has loaded
    api.init = init;

    // Execute listener immediately if already initialized
    api.addInitListener = function(listener) {
        if (api.initialized) {
            listener(api);
        } else {
            initListeners.push(listener);
        }
    };

    var shimListeners = [];

    api.addShimListener = function(listener) {
        shimListeners.push(listener);
    };

    function shim(win) {
        win = win || window;
        init();

        // Notify listeners
        for (var i = 0, len = shimListeners.length; i < len; ++i) {
            shimListeners[i](win);
        }
    }

    api.shim = api.createMissingNativeApi = shim;

    function Module(name, dependencies, initializer) {
        this.name = name;
        this.dependencies = dependencies;
        this.initialized = false;
        this.supported = false;
        this.initializer = initializer;
    }

    Module.prototype = {
        init: function() {
            var requiredModuleNames = this.dependencies || [];
            for (var i = 0, len = requiredModuleNames.length, requiredModule, moduleName; i < len; ++i) {
                moduleName = requiredModuleNames[i];

                requiredModule = modules[moduleName];
                if (!requiredModule || !(requiredModule instanceof Module)) {
                    throw new Error("required module '" + moduleName + "' not found");
                }

                requiredModule.init();

                if (!requiredModule.supported) {
                    throw new Error("required module '" + moduleName + "' not supported");
                }
            }
            
            // Now run initializer
            this.initializer(this);
        },
        
        fail: function(reason) {
            this.initialized = true;
            this.supported = false;
            throw new Error("Module '" + this.name + "' failed to load: " + reason);
        },

        warn: function(msg) {
            api.warn("Module " + this.name + ": " + msg);
        },

        deprecationNotice: function(deprecated, replacement) {
            api.warn("DEPRECATED: " + deprecated + " in module " + this.name + "is deprecated. Please use " +
                replacement + " instead");
        },

        createError: function(msg) {
            return new Error("Error in Rangy " + this.name + " module: " + msg);
        }
    };
    
    function createModule(isCore, name, dependencies, initFunc) {
        var newModule = new Module(name, dependencies, function(module) {
            if (!module.initialized) {
                module.initialized = true;
                try {
                    initFunc(api, module);
                    module.supported = true;
                } catch (ex) {
                    var errorMessage = "Module '" + name + "' failed to load: " + getErrorDesc(ex);
                    consoleLog(errorMessage);
                }
            }
        });
        modules[name] = newModule;
    }

    api.createModule = function(name) {
        // Allow 2 or 3 arguments (second argument is an optional array of dependencies)
        var initFunc, dependencies;
        if (arguments.length == 2) {
            initFunc = arguments[1];
            dependencies = [];
        } else {
            initFunc = arguments[2];
            dependencies = arguments[1];
        }

        var module = createModule(false, name, dependencies, initFunc);

        // Initialize the module immediately if the core is already initialized
        if (api.initialized) {
            module.init();
        }
    };

    api.createCoreModule = function(name, dependencies, initFunc) {
        createModule(true, name, dependencies, initFunc);
    };

    /*----------------------------------------------------------------------------------------------------------------*/

    // Ensure rangy.rangePrototype and rangy.selectionPrototype are available immediately

    function RangePrototype() {}
    api.RangePrototype = RangePrototype;
    api.rangePrototype = new RangePrototype();

    function SelectionPrototype() {}
    api.selectionPrototype = new SelectionPrototype();

    /*----------------------------------------------------------------------------------------------------------------*/

    // Wait for document to load before running tests

    var docReady = false;

    var loadHandler = function(e) {
        if (!docReady) {
            docReady = true;
            if (!api.initialized && api.config.autoInitialize) {
                init();
            }
        }
    };

    // Test whether we have window and document objects that we will need
    if (typeof window == UNDEFINED) {
        fail("No window found");
        return;
    }
    if (typeof document == UNDEFINED) {
        fail("No document found");
        return;
    }

    if (isHostMethod(document, "addEventListener")) {
        document.addEventListener("DOMContentLoaded", loadHandler, false);
    }

    // Add a fallback in case the DOMContentLoaded event isn't supported
    addListener(window, "load", loadHandler);

    /*----------------------------------------------------------------------------------------------------------------*/
    
    // DOM utility methods used by Rangy
    api.createCoreModule("DomUtil", [], function(api, module) {
        var UNDEF = "undefined";
        var util = api.util;

        // Perform feature tests
        if (!util.areHostMethods(document, ["createDocumentFragment", "createElement", "createTextNode"])) {
            module.fail("document missing a Node creation method");
        }

        if (!util.isHostMethod(document, "getElementsByTagName")) {
            module.fail("document missing getElementsByTagName method");
        }

        var el = document.createElement("div");
        if (!util.areHostMethods(el, ["insertBefore", "appendChild", "cloneNode"] ||
                !util.areHostObjects(el, ["previousSibling", "nextSibling", "childNodes", "parentNode"]))) {
            module.fail("Incomplete Element implementation");
        }

        // innerHTML is required for Range's createContextualFragment method
        if (!util.isHostProperty(el, "innerHTML")) {
            module.fail("Element is missing innerHTML property");
        }

        var textNode = document.createTextNode("test");
        if (!util.areHostMethods(textNode, ["splitText", "deleteData", "insertData", "appendData", "cloneNode"] ||
                !util.areHostObjects(el, ["previousSibling", "nextSibling", "childNodes", "parentNode"]) ||
                !util.areHostProperties(textNode, ["data"]))) {
            module.fail("Incomplete Text Node implementation");
        }

        /*----------------------------------------------------------------------------------------------------------------*/

        // Removed use of indexOf because of a bizarre bug in Opera that is thrown in one of the Acid3 tests. I haven't been
        // able to replicate it outside of the test. The bug is that indexOf returns -1 when called on an Array that
        // contains just the document as a single element and the value searched for is the document.
        var arrayContains = /*Array.prototype.indexOf ?
            function(arr, val) {
                return arr.indexOf(val) > -1;
            }:*/

            function(arr, val) {
                var i = arr.length;
                while (i--) {
                    if (arr[i] === val) {
                        return true;
                    }
                }
                return false;
            };

        // Opera 11 puts HTML elements in the null namespace, it seems, and IE 7 has undefined namespaceURI
        function isHtmlNamespace(node) {
            var ns;
            return typeof node.namespaceURI == UNDEF || ((ns = node.namespaceURI) === null || ns == "http://www.w3.org/1999/xhtml");
        }

        function parentElement(node) {
            var parent = node.parentNode;
            return (parent.nodeType == 1) ? parent : null;
        }

        function getNodeIndex(node) {
            var i = 0;
            while( (node = node.previousSibling) ) {
                ++i;
            }
            return i;
        }

        function getNodeLength(node) {
            switch (node.nodeType) {
                case 7:
                case 10:
                    return 0;
                case 3:
                case 8:
                    return node.length;
                default:
                    return node.childNodes.length;
            }
        }

        function getCommonAncestor(node1, node2) {
            var ancestors = [], n;
            for (n = node1; n; n = n.parentNode) {
                ancestors.push(n);
            }

            for (n = node2; n; n = n.parentNode) {
                if (arrayContains(ancestors, n)) {
                    return n;
                }
            }

            return null;
        }

        function isAncestorOf(ancestor, descendant, selfIsAncestor) {
            var n = selfIsAncestor ? descendant : descendant.parentNode;
            while (n) {
                if (n === ancestor) {
                    return true;
                } else {
                    n = n.parentNode;
                }
            }
            return false;
        }

        function isOrIsAncestorOf(ancestor, descendant) {
            return isAncestorOf(ancestor, descendant, true);
        }

        function getClosestAncestorIn(node, ancestor, selfIsAncestor) {
            var p, n = selfIsAncestor ? node : node.parentNode;
            while (n) {
                p = n.parentNode;
                if (p === ancestor) {
                    return n;
                }
                n = p;
            }
            return null;
        }

        function isCharacterDataNode(node) {
            var t = node.nodeType;
            return t == 3 || t == 4 || t == 8 ; // Text, CDataSection or Comment
        }

        function isTextOrCommentNode(node) {
            if (!node) {
                return false;
            }
            var t = node.nodeType;
            return t == 3 || t == 8 ; // Text or Comment
        }

        function insertAfter(node, precedingNode) {
            var nextNode = precedingNode.nextSibling, parent = precedingNode.parentNode;
            if (nextNode) {
                parent.insertBefore(node, nextNode);
            } else {
                parent.appendChild(node);
            }
            return node;
        }

        // Note that we cannot use splitText() because it is bugridden in IE 9.
        function splitDataNode(node, index, positionsToPreserve) {
            var newNode = node.cloneNode(false);
            newNode.deleteData(0, index);
            node.deleteData(index, node.length - index);
            insertAfter(newNode, node);

            // Preserve positions
            if (positionsToPreserve) {
                for (var i = 0, position; position = positionsToPreserve[i++]; ) {
                    // Handle case where position was inside the portion of node after the split point
                    if (position.node == node && position.offset > index) {
                        position.node = newNode;
                        position.offset -= index;
                    }
                    // Handle the case where the position is a node offset within node's parent
                    else if (position.node == node.parentNode && position.offset > getNodeIndex(node)) {
                        ++position.offset;
                    }
                }
            }
            return newNode;
        }

        function getDocument(node) {
            if (node.nodeType == 9) {
                return node;
            } else if (typeof node.ownerDocument != UNDEF) {
                return node.ownerDocument;
            } else if (typeof node.document != UNDEF) {
                return node.document;
            } else if (node.parentNode) {
                return getDocument(node.parentNode);
            } else {
                throw module.createError("getDocument: no document found for node");
            }
        }

        function getWindow(node) {
            var doc = getDocument(node);
            if (typeof doc.defaultView != UNDEF) {
                return doc.defaultView;
            } else if (typeof doc.parentWindow != UNDEF) {
                return doc.parentWindow;
            } else {
                throw module.createError("Cannot get a window object for node");
            }
        }

        function getIframeDocument(iframeEl) {
            if (typeof iframeEl.contentDocument != UNDEF) {
                return iframeEl.contentDocument;
            } else if (typeof iframeEl.contentWindow != UNDEF) {
                return iframeEl.contentWindow.document;
            } else {
                throw module.createError("getIframeDocument: No Document object found for iframe element");
            }
        }

        function getIframeWindow(iframeEl) {
            if (typeof iframeEl.contentWindow != UNDEF) {
                return iframeEl.contentWindow;
            } else if (typeof iframeEl.contentDocument != UNDEF) {
                return iframeEl.contentDocument.defaultView;
            } else {
                throw module.createError("getIframeWindow: No Window object found for iframe element");
            }
        }

        // This looks bad. Is it worth it?
        function isWindow(obj) {
            return obj && util.isHostMethod(obj, "setTimeout") && util.isHostObject(obj, "document");
        }

        function getContentDocument(obj, module, methodName) {
            var doc;

            if (!obj) {
                doc = document;
            }

            // Test if a DOM node has been passed and obtain a document object for it if so
            else if (util.isHostProperty(obj, "nodeType")) {
                doc = (obj.nodeType == 1 && obj.tagName.toLowerCase() == "iframe") ?
                    getIframeDocument(obj) : getDocument(obj);
            }

            // Test if the doc parameter appears to be a Window object
            else if (isWindow(obj)) {
                doc = obj.document;
            }

            if (!doc) {
                throw module.createError(methodName + "(): Parameter must be a Window object or DOM node");
            }

            return doc;
        }

        function getRootContainer(node) {
            var parent;
            while ( (parent = node.parentNode) ) {
                node = parent;
            }
            return node;
        }

        function comparePoints(nodeA, offsetA, nodeB, offsetB) {
            // See http://www.w3.org/TR/DOM-Level-2-Traversal-Range/ranges.html#Level-2-Range-Comparing
            var nodeC, root, childA, childB, n;
            if (nodeA == nodeB) {
                // Case 1: nodes are the same
                return offsetA === offsetB ? 0 : (offsetA < offsetB) ? -1 : 1;
            } else if ( (nodeC = getClosestAncestorIn(nodeB, nodeA, true)) ) {
                // Case 2: node C (container B or an ancestor) is a child node of A
                return offsetA <= getNodeIndex(nodeC) ? -1 : 1;
            } else if ( (nodeC = getClosestAncestorIn(nodeA, nodeB, true)) ) {
                // Case 3: node C (container A or an ancestor) is a child node of B
                return getNodeIndex(nodeC) < offsetB  ? -1 : 1;
            } else {
                root = getCommonAncestor(nodeA, nodeB);
                if (!root) {
                    throw new Error("comparePoints error: nodes have no common ancestor");
                }

                // Case 4: containers are siblings or descendants of siblings
                childA = (nodeA === root) ? root : getClosestAncestorIn(nodeA, root, true);
                childB = (nodeB === root) ? root : getClosestAncestorIn(nodeB, root, true);

                if (childA === childB) {
                    // This shouldn't be possible
                    throw module.createError("comparePoints got to case 4 and childA and childB are the same!");
                } else {
                    n = root.firstChild;
                    while (n) {
                        if (n === childA) {
                            return -1;
                        } else if (n === childB) {
                            return 1;
                        }
                        n = n.nextSibling;
                    }
                }
            }
        }

        /*----------------------------------------------------------------------------------------------------------------*/

        // Test for IE's crash (IE 6/7) or exception (IE >= 8) when a reference to garbage-collected text node is queried
        var crashyTextNodes = false;

        function isBrokenNode(node) {
            var n;
            try {
                n = node.parentNode;
                return false;
            } catch (e) {
                return true;
            }
        }

        (function() {
            var el = document.createElement("b");
            el.innerHTML = "1";
            var textNode = el.firstChild;
            el.innerHTML = "<br>";
            crashyTextNodes = isBrokenNode(textNode);

            api.features.crashyTextNodes = crashyTextNodes;
        })();

        /*----------------------------------------------------------------------------------------------------------------*/

        function inspectNode(node) {
            if (!node) {
                return "[No node]";
            }
            if (crashyTextNodes && isBrokenNode(node)) {
                return "[Broken node]";
            }
            if (isCharacterDataNode(node)) {
                return '"' + node.data + '"';
            }
            if (node.nodeType == 1) {
                var idAttr = node.id ? ' id="' + node.id + '"' : "";
                return "<" + node.nodeName + idAttr + ">[index:" + getNodeIndex(node) + ",length:" + node.childNodes.length + "][" + (node.innerHTML || "[innerHTML not supported]").slice(0, 25) + "]";
            }
            return node.nodeName;
        }

        function fragmentFromNodeChildren(node) {
            var fragment = getDocument(node).createDocumentFragment(), child;
            while ( (child = node.firstChild) ) {
                fragment.appendChild(child);
            }
            return fragment;
        }

        var getComputedStyleProperty;
        if (typeof window.getComputedStyle != UNDEF) {
            getComputedStyleProperty = function(el, propName) {
                return getWindow(el).getComputedStyle(el, null)[propName];
            };
        } else if (typeof document.documentElement.currentStyle != UNDEF) {
            getComputedStyleProperty = function(el, propName) {
                return el.currentStyle[propName];
            };
        } else {
            module.fail("No means of obtaining computed style properties found");
        }

        function NodeIterator(root) {
            this.root = root;
            this._next = root;
        }

        NodeIterator.prototype = {
            _current: null,

            hasNext: function() {
                return !!this._next;
            },

            next: function() {
                var n = this._current = this._next;
                var child, next;
                if (this._current) {
                    child = n.firstChild;
                    if (child) {
                        this._next = child;
                    } else {
                        next = null;
                        while ((n !== this.root) && !(next = n.nextSibling)) {
                            n = n.parentNode;
                        }
                        this._next = next;
                    }
                }
                return this._current;
            },

            detach: function() {
                this._current = this._next = this.root = null;
            }
        };

        function createIterator(root) {
            return new NodeIterator(root);
        }

        function DomPosition(node, offset) {
            this.node = node;
            this.offset = offset;
        }

        DomPosition.prototype = {
            equals: function(pos) {
                return !!pos && this.node === pos.node && this.offset == pos.offset;
            },

            inspect: function() {
                return "[DomPosition(" + inspectNode(this.node) + ":" + this.offset + ")]";
            },

            toString: function() {
                return this.inspect();
            }
        };

        function DOMException(codeName) {
            this.code = this[codeName];
            this.codeName = codeName;
            this.message = "DOMException: " + this.codeName;
        }

        DOMException.prototype = {
            INDEX_SIZE_ERR: 1,
            HIERARCHY_REQUEST_ERR: 3,
            WRONG_DOCUMENT_ERR: 4,
            NO_MODIFICATION_ALLOWED_ERR: 7,
            NOT_FOUND_ERR: 8,
            NOT_SUPPORTED_ERR: 9,
            INVALID_STATE_ERR: 11,
            INVALID_NODE_TYPE_ERR: 24
        };

        DOMException.prototype.toString = function() {
            return this.message;
        };

        api.dom = {
            arrayContains: arrayContains,
            isHtmlNamespace: isHtmlNamespace,
            parentElement: parentElement,
            getNodeIndex: getNodeIndex,
            getNodeLength: getNodeLength,
            getCommonAncestor: getCommonAncestor,
            isAncestorOf: isAncestorOf,
            isOrIsAncestorOf: isOrIsAncestorOf,
            getClosestAncestorIn: getClosestAncestorIn,
            isCharacterDataNode: isCharacterDataNode,
            isTextOrCommentNode: isTextOrCommentNode,
            insertAfter: insertAfter,
            splitDataNode: splitDataNode,
            getDocument: getDocument,
            getWindow: getWindow,
            getIframeWindow: getIframeWindow,
            getIframeDocument: getIframeDocument,
            getBody: util.getBody,
            isWindow: isWindow,
            getContentDocument: getContentDocument,
            getRootContainer: getRootContainer,
            comparePoints: comparePoints,
            isBrokenNode: isBrokenNode,
            inspectNode: inspectNode,
            getComputedStyleProperty: getComputedStyleProperty,
            fragmentFromNodeChildren: fragmentFromNodeChildren,
            createIterator: createIterator,
            DomPosition: DomPosition
        };

        api.DOMException = DOMException;
    });

    /*----------------------------------------------------------------------------------------------------------------*/

    // Pure JavaScript implementation of DOM Range
    api.createCoreModule("DomRange", ["DomUtil"], function(api, module) {
        var dom = api.dom;
        var util = api.util;
        var DomPosition = dom.DomPosition;
        var DOMException = api.DOMException;

        var isCharacterDataNode = dom.isCharacterDataNode;
        var getNodeIndex = dom.getNodeIndex;
        var isOrIsAncestorOf = dom.isOrIsAncestorOf;
        var getDocument = dom.getDocument;
        var comparePoints = dom.comparePoints;
        var splitDataNode = dom.splitDataNode;
        var getClosestAncestorIn = dom.getClosestAncestorIn;
        var getNodeLength = dom.getNodeLength;
        var arrayContains = dom.arrayContains;
        var getRootContainer = dom.getRootContainer;
        var crashyTextNodes = api.features.crashyTextNodes;

        /*----------------------------------------------------------------------------------------------------------------*/

        // Utility functions

        function isNonTextPartiallySelected(node, range) {
            return (node.nodeType != 3) &&
                   (isOrIsAncestorOf(node, range.startContainer) || isOrIsAncestorOf(node, range.endContainer));
        }

        function getRangeDocument(range) {
            return range.document || getDocument(range.startContainer);
        }

        function getBoundaryBeforeNode(node) {
            return new DomPosition(node.parentNode, getNodeIndex(node));
        }

        function getBoundaryAfterNode(node) {
            return new DomPosition(node.parentNode, getNodeIndex(node) + 1);
        }

        function insertNodeAtPosition(node, n, o) {
            var firstNodeInserted = node.nodeType == 11 ? node.firstChild : node;
            if (isCharacterDataNode(n)) {
                if (o == n.length) {
                    dom.insertAfter(node, n);
                } else {
                    n.parentNode.insertBefore(node, o == 0 ? n : splitDataNode(n, o));
                }
            } else if (o >= n.childNodes.length) {
                n.appendChild(node);
            } else {
                n.insertBefore(node, n.childNodes[o]);
            }
            return firstNodeInserted;
        }

        function rangesIntersect(rangeA, rangeB, touchingIsIntersecting) {
            assertRangeValid(rangeA);
            assertRangeValid(rangeB);

            if (getRangeDocument(rangeB) != getRangeDocument(rangeA)) {
                throw new DOMException("WRONG_DOCUMENT_ERR");
            }

            var startComparison = comparePoints(rangeA.startContainer, rangeA.startOffset, rangeB.endContainer, rangeB.endOffset),
                endComparison = comparePoints(rangeA.endContainer, rangeA.endOffset, rangeB.startContainer, rangeB.startOffset);

            return touchingIsIntersecting ? startComparison <= 0 && endComparison >= 0 : startComparison < 0 && endComparison > 0;
        }

        function cloneSubtree(iterator) {
            var partiallySelected;
            for (var node, frag = getRangeDocument(iterator.range).createDocumentFragment(), subIterator; node = iterator.next(); ) {
                partiallySelected = iterator.isPartiallySelectedSubtree();
                node = node.cloneNode(!partiallySelected);
                if (partiallySelected) {
                    subIterator = iterator.getSubtreeIterator();
                    node.appendChild(cloneSubtree(subIterator));
                    subIterator.detach();
                }

                if (node.nodeType == 10) { // DocumentType
                    throw new DOMException("HIERARCHY_REQUEST_ERR");
                }
                frag.appendChild(node);
            }
            return frag;
        }

        function iterateSubtree(rangeIterator, func, iteratorState) {
            var it, n;
            iteratorState = iteratorState || { stop: false };
            for (var node, subRangeIterator; node = rangeIterator.next(); ) {
                if (rangeIterator.isPartiallySelectedSubtree()) {
                    if (func(node) === false) {
                        iteratorState.stop = true;
                        return;
                    } else {
                        // The node is partially selected by the Range, so we can use a new RangeIterator on the portion of
                        // the node selected by the Range.
                        subRangeIterator = rangeIterator.getSubtreeIterator();
                        iterateSubtree(subRangeIterator, func, iteratorState);
                        subRangeIterator.detach();
                        if (iteratorState.stop) {
                            return;
                        }
                    }
                } else {
                    // The whole node is selected, so we can use efficient DOM iteration to iterate over the node and its
                    // descendants
                    it = dom.createIterator(node);
                    while ( (n = it.next()) ) {
                        if (func(n) === false) {
                            iteratorState.stop = true;
                            return;
                        }
                    }
                }
            }
        }

        function deleteSubtree(iterator) {
            var subIterator;
            while (iterator.next()) {
                if (iterator.isPartiallySelectedSubtree()) {
                    subIterator = iterator.getSubtreeIterator();
                    deleteSubtree(subIterator);
                    subIterator.detach();
                } else {
                    iterator.remove();
                }
            }
        }

        function extractSubtree(iterator) {
            for (var node, frag = getRangeDocument(iterator.range).createDocumentFragment(), subIterator; node = iterator.next(); ) {

                if (iterator.isPartiallySelectedSubtree()) {
                    node = node.cloneNode(false);
                    subIterator = iterator.getSubtreeIterator();
                    node.appendChild(extractSubtree(subIterator));
                    subIterator.detach();
                } else {
                    iterator.remove();
                }
                if (node.nodeType == 10) { // DocumentType
                    throw new DOMException("HIERARCHY_REQUEST_ERR");
                }
                frag.appendChild(node);
            }
            return frag;
        }

        function getNodesInRange(range, nodeTypes, filter) {
            var filterNodeTypes = !!(nodeTypes && nodeTypes.length), regex;
            var filterExists = !!filter;
            if (filterNodeTypes) {
                regex = new RegExp("^(" + nodeTypes.join("|") + ")$");
            }

            var nodes = [];
            iterateSubtree(new RangeIterator(range, false), function(node) {
                if (filterNodeTypes && !regex.test(node.nodeType)) {
                    return;
                }
                if (filterExists && !filter(node)) {
                    return;
                }
                // Don't include a boundary container if it is a character data node and the range does not contain any
                // of its character data. See issue 190.
                var sc = range.startContainer;
                if (node == sc && isCharacterDataNode(sc) && range.startOffset == sc.length) {
                    return;
                }

                var ec = range.endContainer;
                if (node == ec && isCharacterDataNode(ec) && range.endOffset == 0) {
                    return;
                }

                nodes.push(node);
            });
            return nodes;
        }

        function inspect(range) {
            var name = (typeof range.getName == "undefined") ? "Range" : range.getName();
            return "[" + name + "(" + dom.inspectNode(range.startContainer) + ":" + range.startOffset + ", " +
                    dom.inspectNode(range.endContainer) + ":" + range.endOffset + ")]";
        }

        /*----------------------------------------------------------------------------------------------------------------*/

        // RangeIterator code partially borrows from IERange by Tim Ryan (http://github.com/timcameronryan/IERange)

        function RangeIterator(range, clonePartiallySelectedTextNodes) {
            this.range = range;
            this.clonePartiallySelectedTextNodes = clonePartiallySelectedTextNodes;


            if (!range.collapsed) {
                this.sc = range.startContainer;
                this.so = range.startOffset;
                this.ec = range.endContainer;
                this.eo = range.endOffset;
                var root = range.commonAncestorContainer;

                if (this.sc === this.ec && isCharacterDataNode(this.sc)) {
                    this.isSingleCharacterDataNode = true;
                    this._first = this._last = this._next = this.sc;
                } else {
                    this._first = this._next = (this.sc === root && !isCharacterDataNode(this.sc)) ?
                        this.sc.childNodes[this.so] : getClosestAncestorIn(this.sc, root, true);
                    this._last = (this.ec === root && !isCharacterDataNode(this.ec)) ?
                        this.ec.childNodes[this.eo - 1] : getClosestAncestorIn(this.ec, root, true);
                }
            }
        }

        RangeIterator.prototype = {
            _current: null,
            _next: null,
            _first: null,
            _last: null,
            isSingleCharacterDataNode: false,

            reset: function() {
                this._current = null;
                this._next = this._first;
            },

            hasNext: function() {
                return !!this._next;
            },

            next: function() {
                // Move to next node
                var current = this._current = this._next;
                if (current) {
                    this._next = (current !== this._last) ? current.nextSibling : null;

                    // Check for partially selected text nodes
                    if (isCharacterDataNode(current) && this.clonePartiallySelectedTextNodes) {
                        if (current === this.ec) {
                            (current = current.cloneNode(true)).deleteData(this.eo, current.length - this.eo);
                        }
                        if (this._current === this.sc) {
                            (current = current.cloneNode(true)).deleteData(0, this.so);
                        }
                    }
                }

                return current;
            },

            remove: function() {
                var current = this._current, start, end;

                if (isCharacterDataNode(current) && (current === this.sc || current === this.ec)) {
                    start = (current === this.sc) ? this.so : 0;
                    end = (current === this.ec) ? this.eo : current.length;
                    if (start != end) {
                        current.deleteData(start, end - start);
                    }
                } else {
                    if (current.parentNode) {
                        current.parentNode.removeChild(current);
                    } else {
                    }
                }
            },

            // Checks if the current node is partially selected
            isPartiallySelectedSubtree: function() {
                var current = this._current;
                return isNonTextPartiallySelected(current, this.range);
            },

            getSubtreeIterator: function() {
                var subRange;
                if (this.isSingleCharacterDataNode) {
                    subRange = this.range.cloneRange();
                    subRange.collapse(false);
                } else {
                    subRange = new Range(getRangeDocument(this.range));
                    var current = this._current;
                    var startContainer = current, startOffset = 0, endContainer = current, endOffset = getNodeLength(current);

                    if (isOrIsAncestorOf(current, this.sc)) {
                        startContainer = this.sc;
                        startOffset = this.so;
                    }
                    if (isOrIsAncestorOf(current, this.ec)) {
                        endContainer = this.ec;
                        endOffset = this.eo;
                    }

                    updateBoundaries(subRange, startContainer, startOffset, endContainer, endOffset);
                }
                return new RangeIterator(subRange, this.clonePartiallySelectedTextNodes);
            },

            detach: function() {
                this.range = this._current = this._next = this._first = this._last = this.sc = this.so = this.ec = this.eo = null;
            }
        };

        /*----------------------------------------------------------------------------------------------------------------*/

        var beforeAfterNodeTypes = [1, 3, 4, 5, 7, 8, 10];
        var rootContainerNodeTypes = [2, 9, 11];
        var readonlyNodeTypes = [5, 6, 10, 12];
        var insertableNodeTypes = [1, 3, 4, 5, 7, 8, 10, 11];
        var surroundNodeTypes = [1, 3, 4, 5, 7, 8];

        function createAncestorFinder(nodeTypes) {
            return function(node, selfIsAncestor) {
                var t, n = selfIsAncestor ? node : node.parentNode;
                while (n) {
                    t = n.nodeType;
                    if (arrayContains(nodeTypes, t)) {
                        return n;
                    }
                    n = n.parentNode;
                }
                return null;
            };
        }

        var getDocumentOrFragmentContainer = createAncestorFinder( [9, 11] );
        var getReadonlyAncestor = createAncestorFinder(readonlyNodeTypes);
        var getDocTypeNotationEntityAncestor = createAncestorFinder( [6, 10, 12] );

        function assertNoDocTypeNotationEntityAncestor(node, allowSelf) {
            if (getDocTypeNotationEntityAncestor(node, allowSelf)) {
                throw new DOMException("INVALID_NODE_TYPE_ERR");
            }
        }

        function assertValidNodeType(node, invalidTypes) {
            if (!arrayContains(invalidTypes, node.nodeType)) {
                throw new DOMException("INVALID_NODE_TYPE_ERR");
            }
        }

        function assertValidOffset(node, offset) {
            if (offset < 0 || offset > (isCharacterDataNode(node) ? node.length : node.childNodes.length)) {
                throw new DOMException("INDEX_SIZE_ERR");
            }
        }

        function assertSameDocumentOrFragment(node1, node2) {
            if (getDocumentOrFragmentContainer(node1, true) !== getDocumentOrFragmentContainer(node2, true)) {
                throw new DOMException("WRONG_DOCUMENT_ERR");
            }
        }

        function assertNodeNotReadOnly(node) {
            if (getReadonlyAncestor(node, true)) {
                throw new DOMException("NO_MODIFICATION_ALLOWED_ERR");
            }
        }

        function assertNode(node, codeName) {
            if (!node) {
                throw new DOMException(codeName);
            }
        }

        function isOrphan(node) {
            return (crashyTextNodes && dom.isBrokenNode(node)) ||
                !arrayContains(rootContainerNodeTypes, node.nodeType) && !getDocumentOrFragmentContainer(node, true);
        }

        function isValidOffset(node, offset) {
            return offset <= (isCharacterDataNode(node) ? node.length : node.childNodes.length);
        }

        function isRangeValid(range) {
            return (!!range.startContainer && !!range.endContainer &&
                    !isOrphan(range.startContainer) &&
                    !isOrphan(range.endContainer) &&
                    isValidOffset(range.startContainer, range.startOffset) &&
                    isValidOffset(range.endContainer, range.endOffset));
        }

        function assertRangeValid(range) {
            if (!isRangeValid(range)) {
                throw new Error("Range error: Range is no longer valid after DOM mutation (" + range.inspect() + ")");
            }
        }

        /*----------------------------------------------------------------------------------------------------------------*/

        // Test the browser's innerHTML support to decide how to implement createContextualFragment
        var styleEl = document.createElement("style");
        var htmlParsingConforms = false;
        try {
            styleEl.innerHTML = "<b>x</b>";
            htmlParsingConforms = (styleEl.firstChild.nodeType == 3); // Opera incorrectly creates an element node
        } catch (e) {
            // IE 6 and 7 throw
        }

        api.features.htmlParsingConforms = htmlParsingConforms;

        var createContextualFragment = htmlParsingConforms ?

            // Implementation as per HTML parsing spec, trusting in the browser's implementation of innerHTML. See
            // discussion and base code for this implementation at issue 67.
            // Spec: http://html5.org/specs/dom-parsing.html#extensions-to-the-range-interface
            // Thanks to Aleks Williams.
            function(fragmentStr) {
                // "Let node the context object's start's node."
                var node = this.startContainer;
                var doc = getDocument(node);

                // "If the context object's start's node is null, raise an INVALID_STATE_ERR
                // exception and abort these steps."
                if (!node) {
                    throw new DOMException("INVALID_STATE_ERR");
                }

                // "Let element be as follows, depending on node's interface:"
                // Document, Document Fragment: null
                var el = null;

                // "Element: node"
                if (node.nodeType == 1) {
                    el = node;

                // "Text, Comment: node's parentElement"
                } else if (isCharacterDataNode(node)) {
                    el = dom.parentElement(node);
                }

                // "If either element is null or element's ownerDocument is an HTML document
                // and element's local name is "html" and element's namespace is the HTML
                // namespace"
                if (el === null || (
                    el.nodeName == "HTML" &&
                    dom.isHtmlNamespace(getDocument(el).documentElement) &&
                    dom.isHtmlNamespace(el)
                )) {

                // "let element be a new Element with "body" as its local name and the HTML
                // namespace as its namespace.""
                    el = doc.createElement("body");
                } else {
                    el = el.cloneNode(false);
                }

                // "If the node's document is an HTML document: Invoke the HTML fragment parsing algorithm."
                // "If the node's document is an XML document: Invoke the XML fragment parsing algorithm."
                // "In either case, the algorithm must be invoked with fragment as the input
                // and element as the context element."
                el.innerHTML = fragmentStr;

                // "If this raises an exception, then abort these steps. Otherwise, let new
                // children be the nodes returned."

                // "Let fragment be a new DocumentFragment."
                // "Append all new children to fragment."
                // "Return fragment."
                return dom.fragmentFromNodeChildren(el);
            } :

            // In this case, innerHTML cannot be trusted, so fall back to a simpler, non-conformant implementation that
            // previous versions of Rangy used (with the exception of using a body element rather than a div)
            function(fragmentStr) {
                var doc = getRangeDocument(this);
                var el = doc.createElement("body");
                el.innerHTML = fragmentStr;

                return dom.fragmentFromNodeChildren(el);
            };

        function splitRangeBoundaries(range, positionsToPreserve) {
            assertRangeValid(range);

            var sc = range.startContainer, so = range.startOffset, ec = range.endContainer, eo = range.endOffset;
            var startEndSame = (sc === ec);

            if (isCharacterDataNode(ec) && eo > 0 && eo < ec.length) {
                splitDataNode(ec, eo, positionsToPreserve);
            }

            if (isCharacterDataNode(sc) && so > 0 && so < sc.length) {
                sc = splitDataNode(sc, so, positionsToPreserve);
                if (startEndSame) {
                    eo -= so;
                    ec = sc;
                } else if (ec == sc.parentNode && eo >= getNodeIndex(sc)) {
                    eo++;
                }
                so = 0;
            }
            range.setStartAndEnd(sc, so, ec, eo);
        }
        
        function rangeToHtml(range) {
            assertRangeValid(range);
            var container = range.commonAncestorContainer.parentNode.cloneNode(false);
            container.appendChild( range.cloneContents() );
            return container.innerHTML;
        }

        /*----------------------------------------------------------------------------------------------------------------*/

        var rangeProperties = ["startContainer", "startOffset", "endContainer", "endOffset", "collapsed",
            "commonAncestorContainer"];

        var s2s = 0, s2e = 1, e2e = 2, e2s = 3;
        var n_b = 0, n_a = 1, n_b_a = 2, n_i = 3;

        util.extend(api.rangePrototype, {
            compareBoundaryPoints: function(how, range) {
                assertRangeValid(this);
                assertSameDocumentOrFragment(this.startContainer, range.startContainer);

                var nodeA, offsetA, nodeB, offsetB;
                var prefixA = (how == e2s || how == s2s) ? "start" : "end";
                var prefixB = (how == s2e || how == s2s) ? "start" : "end";
                nodeA = this[prefixA + "Container"];
                offsetA = this[prefixA + "Offset"];
                nodeB = range[prefixB + "Container"];
                offsetB = range[prefixB + "Offset"];
                return comparePoints(nodeA, offsetA, nodeB, offsetB);
            },

            insertNode: function(node) {
                assertRangeValid(this);
                assertValidNodeType(node, insertableNodeTypes);
                assertNodeNotReadOnly(this.startContainer);

                if (isOrIsAncestorOf(node, this.startContainer)) {
                    throw new DOMException("HIERARCHY_REQUEST_ERR");
                }

                // No check for whether the container of the start of the Range is of a type that does not allow
                // children of the type of node: the browser's DOM implementation should do this for us when we attempt
                // to add the node

                var firstNodeInserted = insertNodeAtPosition(node, this.startContainer, this.startOffset);
                this.setStartBefore(firstNodeInserted);
            },

            cloneContents: function() {
                assertRangeValid(this);

                var clone, frag;
                if (this.collapsed) {
                    return getRangeDocument(this).createDocumentFragment();
                } else {
                    if (this.startContainer === this.endContainer && isCharacterDataNode(this.startContainer)) {
                        clone = this.startContainer.cloneNode(true);
                        clone.data = clone.data.slice(this.startOffset, this.endOffset);
                        frag = getRangeDocument(this).createDocumentFragment();
                        frag.appendChild(clone);
                        return frag;
                    } else {
                        var iterator = new RangeIterator(this, true);
                        clone = cloneSubtree(iterator);
                        iterator.detach();
                    }
                    return clone;
                }
            },

            canSurroundContents: function() {
                assertRangeValid(this);
                assertNodeNotReadOnly(this.startContainer);
                assertNodeNotReadOnly(this.endContainer);

                // Check if the contents can be surrounded. Specifically, this means whether the range partially selects
                // no non-text nodes.
                var iterator = new RangeIterator(this, true);
                var boundariesInvalid = (iterator._first && (isNonTextPartiallySelected(iterator._first, this)) ||
                        (iterator._last && isNonTextPartiallySelected(iterator._last, this)));
                iterator.detach();
                return !boundariesInvalid;
            },

            surroundContents: function(node) {
                assertValidNodeType(node, surroundNodeTypes);

                if (!this.canSurroundContents()) {
                    throw new DOMException("INVALID_STATE_ERR");
                }

                // Extract the contents
                var content = this.extractContents();

                // Clear the children of the node
                if (node.hasChildNodes()) {
                    while (node.lastChild) {
                        node.removeChild(node.lastChild);
                    }
                }

                // Insert the new node and add the extracted contents
                insertNodeAtPosition(node, this.startContainer, this.startOffset);
                node.appendChild(content);

                this.selectNode(node);
            },

            cloneRange: function() {
                assertRangeValid(this);
                var range = new Range(getRangeDocument(this));
                var i = rangeProperties.length, prop;
                while (i--) {
                    prop = rangeProperties[i];
                    range[prop] = this[prop];
                }
                return range;
            },

            toString: function() {
                assertRangeValid(this);
                var sc = this.startContainer;
                if (sc === this.endContainer && isCharacterDataNode(sc)) {
                    return (sc.nodeType == 3 || sc.nodeType == 4) ? sc.data.slice(this.startOffset, this.endOffset) : "";
                } else {
                    var textParts = [], iterator = new RangeIterator(this, true);
                    iterateSubtree(iterator, function(node) {
                        // Accept only text or CDATA nodes, not comments
                        if (node.nodeType == 3 || node.nodeType == 4) {
                            textParts.push(node.data);
                        }
                    });
                    iterator.detach();
                    return textParts.join("");
                }
            },

            // The methods below are all non-standard. The following batch were introduced by Mozilla but have since
            // been removed from Mozilla.

            compareNode: function(node) {
                assertRangeValid(this);

                var parent = node.parentNode;
                var nodeIndex = getNodeIndex(node);

                if (!parent) {
                    throw new DOMException("NOT_FOUND_ERR");
                }

                var startComparison = this.comparePoint(parent, nodeIndex),
                    endComparison = this.comparePoint(parent, nodeIndex + 1);

                if (startComparison < 0) { // Node starts before
                    return (endComparison > 0) ? n_b_a : n_b;
                } else {
                    return (endComparison > 0) ? n_a : n_i;
                }
            },

            comparePoint: function(node, offset) {
                assertRangeValid(this);
                assertNode(node, "HIERARCHY_REQUEST_ERR");
                assertSameDocumentOrFragment(node, this.startContainer);

                if (comparePoints(node, offset, this.startContainer, this.startOffset) < 0) {
                    return -1;
                } else if (comparePoints(node, offset, this.endContainer, this.endOffset) > 0) {
                    return 1;
                }
                return 0;
            },

            createContextualFragment: createContextualFragment,

            toHtml: function() {
                return rangeToHtml(this);
            },

            // touchingIsIntersecting determines whether this method considers a node that borders a range intersects
            // with it (as in WebKit) or not (as in Gecko pre-1.9, and the default)
            intersectsNode: function(node, touchingIsIntersecting) {
                assertRangeValid(this);
                assertNode(node, "NOT_FOUND_ERR");
                if (getDocument(node) !== getRangeDocument(this)) {
                    return false;
                }

                var parent = node.parentNode, offset = getNodeIndex(node);
                assertNode(parent, "NOT_FOUND_ERR");

                var startComparison = comparePoints(parent, offset, this.endContainer, this.endOffset),
                    endComparison = comparePoints(parent, offset + 1, this.startContainer, this.startOffset);

                return touchingIsIntersecting ? startComparison <= 0 && endComparison >= 0 : startComparison < 0 && endComparison > 0;
            },

            isPointInRange: function(node, offset) {
                assertRangeValid(this);
                assertNode(node, "HIERARCHY_REQUEST_ERR");
                assertSameDocumentOrFragment(node, this.startContainer);

                return (comparePoints(node, offset, this.startContainer, this.startOffset) >= 0) &&
                       (comparePoints(node, offset, this.endContainer, this.endOffset) <= 0);
            },

            // The methods below are non-standard and invented by me.

            // Sharing a boundary start-to-end or end-to-start does not count as intersection.
            intersectsRange: function(range) {
                return rangesIntersect(this, range, false);
            },

            // Sharing a boundary start-to-end or end-to-start does count as intersection.
            intersectsOrTouchesRange: function(range) {
                return rangesIntersect(this, range, true);
            },

            intersection: function(range) {
                if (this.intersectsRange(range)) {
                    var startComparison = comparePoints(this.startContainer, this.startOffset, range.startContainer, range.startOffset),
                        endComparison = comparePoints(this.endContainer, this.endOffset, range.endContainer, range.endOffset);

                    var intersectionRange = this.cloneRange();
                    if (startComparison == -1) {
                        intersectionRange.setStart(range.startContainer, range.startOffset);
                    }
                    if (endComparison == 1) {
                        intersectionRange.setEnd(range.endContainer, range.endOffset);
                    }
                    return intersectionRange;
                }
                return null;
            },

            union: function(range) {
                if (this.intersectsOrTouchesRange(range)) {
                    var unionRange = this.cloneRange();
                    if (comparePoints(range.startContainer, range.startOffset, this.startContainer, this.startOffset) == -1) {
                        unionRange.setStart(range.startContainer, range.startOffset);
                    }
                    if (comparePoints(range.endContainer, range.endOffset, this.endContainer, this.endOffset) == 1) {
                        unionRange.setEnd(range.endContainer, range.endOffset);
                    }
                    return unionRange;
                } else {
                    throw new DOMException("Ranges do not intersect");
                }
            },

            containsNode: function(node, allowPartial) {
                if (allowPartial) {
                    return this.intersectsNode(node, false);
                } else {
                    return this.compareNode(node) == n_i;
                }
            },

            containsNodeContents: function(node) {
                return this.comparePoint(node, 0) >= 0 && this.comparePoint(node, getNodeLength(node)) <= 0;
            },

            containsRange: function(range) {
                var intersection = this.intersection(range);
                return intersection !== null && range.equals(intersection);
            },

            containsNodeText: function(node) {
                var nodeRange = this.cloneRange();
                nodeRange.selectNode(node);
                var textNodes = nodeRange.getNodes([3]);
                if (textNodes.length > 0) {
                    nodeRange.setStart(textNodes[0], 0);
                    var lastTextNode = textNodes.pop();
                    nodeRange.setEnd(lastTextNode, lastTextNode.length);
                    return this.containsRange(nodeRange);
                } else {
                    return this.containsNodeContents(node);
                }
            },

            getNodes: function(nodeTypes, filter) {
                assertRangeValid(this);
                return getNodesInRange(this, nodeTypes, filter);
            },

            getDocument: function() {
                return getRangeDocument(this);
            },

            collapseBefore: function(node) {
                this.setEndBefore(node);
                this.collapse(false);
            },

            collapseAfter: function(node) {
                this.setStartAfter(node);
                this.collapse(true);
            },
            
            getBookmark: function(containerNode) {
                var doc = getRangeDocument(this);
                var preSelectionRange = api.createRange(doc);
                containerNode = containerNode || dom.getBody(doc);
                preSelectionRange.selectNodeContents(containerNode);
                var range = this.intersection(preSelectionRange);
                var start = 0, end = 0;
                if (range) {
                    preSelectionRange.setEnd(range.startContainer, range.startOffset);
                    start = preSelectionRange.toString().length;
                    end = start + range.toString().length;
                }

                return {
                    start: start,
                    end: end,
                    containerNode: containerNode
                };
            },
            
            moveToBookmark: function(bookmark) {
                var containerNode = bookmark.containerNode;
                var charIndex = 0;
                this.setStart(containerNode, 0);
                this.collapse(true);
                var nodeStack = [containerNode], node, foundStart = false, stop = false;
                var nextCharIndex, i, childNodes;

                while (!stop && (node = nodeStack.pop())) {
                    if (node.nodeType == 3) {
                        nextCharIndex = charIndex + node.length;
                        if (!foundStart && bookmark.start >= charIndex && bookmark.start <= nextCharIndex) {
                            this.setStart(node, bookmark.start - charIndex);
                            foundStart = true;
                        }
                        if (foundStart && bookmark.end >= charIndex && bookmark.end <= nextCharIndex) {
                            this.setEnd(node, bookmark.end - charIndex);
                            stop = true;
                        }
                        charIndex = nextCharIndex;
                    } else {
                        childNodes = node.childNodes;
                        i = childNodes.length;
                        while (i--) {
                            nodeStack.push(childNodes[i]);
                        }
                    }
                }
            },

            getName: function() {
                return "DomRange";
            },

            equals: function(range) {
                return Range.rangesEqual(this, range);
            },

            isValid: function() {
                return isRangeValid(this);
            },
            
            inspect: function() {
                return inspect(this);
            },
            
            detach: function() {
                // In DOM4, detach() is now a no-op.
            }
        });

        function copyComparisonConstantsToObject(obj) {
            obj.START_TO_START = s2s;
            obj.START_TO_END = s2e;
            obj.END_TO_END = e2e;
            obj.END_TO_START = e2s;

            obj.NODE_BEFORE = n_b;
            obj.NODE_AFTER = n_a;
            obj.NODE_BEFORE_AND_AFTER = n_b_a;
            obj.NODE_INSIDE = n_i;
        }

        function copyComparisonConstants(constructor) {
            copyComparisonConstantsToObject(constructor);
            copyComparisonConstantsToObject(constructor.prototype);
        }

        function createRangeContentRemover(remover, boundaryUpdater) {
            return function() {
                assertRangeValid(this);

                var sc = this.startContainer, so = this.startOffset, root = this.commonAncestorContainer;

                var iterator = new RangeIterator(this, true);

                // Work out where to position the range after content removal
                var node, boundary;
                if (sc !== root) {
                    node = getClosestAncestorIn(sc, root, true);
                    boundary = getBoundaryAfterNode(node);
                    sc = boundary.node;
                    so = boundary.offset;
                }

                // Check none of the range is read-only
                iterateSubtree(iterator, assertNodeNotReadOnly);

                iterator.reset();

                // Remove the content
                var returnValue = remover(iterator);
                iterator.detach();

                // Move to the new position
                boundaryUpdater(this, sc, so, sc, so);

                return returnValue;
            };
        }

        function createPrototypeRange(constructor, boundaryUpdater) {
            function createBeforeAfterNodeSetter(isBefore, isStart) {
                return function(node) {
                    assertValidNodeType(node, beforeAfterNodeTypes);
                    assertValidNodeType(getRootContainer(node), rootContainerNodeTypes);

                    var boundary = (isBefore ? getBoundaryBeforeNode : getBoundaryAfterNode)(node);
                    (isStart ? setRangeStart : setRangeEnd)(this, boundary.node, boundary.offset);
                };
            }

            function setRangeStart(range, node, offset) {
                var ec = range.endContainer, eo = range.endOffset;
                if (node !== range.startContainer || offset !== range.startOffset) {
                    // Check the root containers of the range and the new boundary, and also check whether the new boundary
                    // is after the current end. In either case, collapse the range to the new position
                    if (getRootContainer(node) != getRootContainer(ec) || comparePoints(node, offset, ec, eo) == 1) {
                        ec = node;
                        eo = offset;
                    }
                    boundaryUpdater(range, node, offset, ec, eo);
                }
            }

            function setRangeEnd(range, node, offset) {
                var sc = range.startContainer, so = range.startOffset;
                if (node !== range.endContainer || offset !== range.endOffset) {
                    // Check the root containers of the range and the new boundary, and also check whether the new boundary
                    // is after the current end. In either case, collapse the range to the new position
                    if (getRootContainer(node) != getRootContainer(sc) || comparePoints(node, offset, sc, so) == -1) {
                        sc = node;
                        so = offset;
                    }
                    boundaryUpdater(range, sc, so, node, offset);
                }
            }

            // Set up inheritance
            var F = function() {};
            F.prototype = api.rangePrototype;
            constructor.prototype = new F();

            util.extend(constructor.prototype, {
                setStart: function(node, offset) {
                    assertNoDocTypeNotationEntityAncestor(node, true);
                    assertValidOffset(node, offset);

                    setRangeStart(this, node, offset);
                },

                setEnd: function(node, offset) {
                    assertNoDocTypeNotationEntityAncestor(node, true);
                    assertValidOffset(node, offset);

                    setRangeEnd(this, node, offset);
                },

                /**
                 * Convenience method to set a range's start and end boundaries. Overloaded as follows:
                 * - Two parameters (node, offset) creates a collapsed range at that position
                 * - Three parameters (node, startOffset, endOffset) creates a range contained with node starting at
                 *   startOffset and ending at endOffset
                 * - Four parameters (startNode, startOffset, endNode, endOffset) creates a range starting at startOffset in
                 *   startNode and ending at endOffset in endNode
                 */
                setStartAndEnd: function() {
                    var args = arguments;
                    var sc = args[0], so = args[1], ec = sc, eo = so;

                    switch (args.length) {
                        case 3:
                            eo = args[2];
                            break;
                        case 4:
                            ec = args[2];
                            eo = args[3];
                            break;
                    }

                    boundaryUpdater(this, sc, so, ec, eo);
                },
                
                setBoundary: function(node, offset, isStart) {
                    this["set" + (isStart ? "Start" : "End")](node, offset);
                },

                setStartBefore: createBeforeAfterNodeSetter(true, true),
                setStartAfter: createBeforeAfterNodeSetter(false, true),
                setEndBefore: createBeforeAfterNodeSetter(true, false),
                setEndAfter: createBeforeAfterNodeSetter(false, false),

                collapse: function(isStart) {
                    assertRangeValid(this);
                    if (isStart) {
                        boundaryUpdater(this, this.startContainer, this.startOffset, this.startContainer, this.startOffset);
                    } else {
                        boundaryUpdater(this, this.endContainer, this.endOffset, this.endContainer, this.endOffset);
                    }
                },

                selectNodeContents: function(node) {
                    assertNoDocTypeNotationEntityAncestor(node, true);

                    boundaryUpdater(this, node, 0, node, getNodeLength(node));
                },

                selectNode: function(node) {
                    assertNoDocTypeNotationEntityAncestor(node, false);
                    assertValidNodeType(node, beforeAfterNodeTypes);

                    var start = getBoundaryBeforeNode(node), end = getBoundaryAfterNode(node);
                    boundaryUpdater(this, start.node, start.offset, end.node, end.offset);
                },

                extractContents: createRangeContentRemover(extractSubtree, boundaryUpdater),

                deleteContents: createRangeContentRemover(deleteSubtree, boundaryUpdater),

                canSurroundContents: function() {
                    assertRangeValid(this);
                    assertNodeNotReadOnly(this.startContainer);
                    assertNodeNotReadOnly(this.endContainer);

                    // Check if the contents can be surrounded. Specifically, this means whether the range partially selects
                    // no non-text nodes.
                    var iterator = new RangeIterator(this, true);
                    var boundariesInvalid = (iterator._first && isNonTextPartiallySelected(iterator._first, this) ||
                            (iterator._last && isNonTextPartiallySelected(iterator._last, this)));
                    iterator.detach();
                    return !boundariesInvalid;
                },

                splitBoundaries: function() {
                    splitRangeBoundaries(this);
                },

                splitBoundariesPreservingPositions: function(positionsToPreserve) {
                    splitRangeBoundaries(this, positionsToPreserve);
                },

                normalizeBoundaries: function() {
                    assertRangeValid(this);

                    var sc = this.startContainer, so = this.startOffset, ec = this.endContainer, eo = this.endOffset;

                    var mergeForward = function(node) {
                        var sibling = node.nextSibling;
                        if (sibling && sibling.nodeType == node.nodeType) {
                            ec = node;
                            eo = node.length;
                            node.appendData(sibling.data);
                            sibling.parentNode.removeChild(sibling);
                        }
                    };

                    var mergeBackward = function(node) {
                        var sibling = node.previousSibling;
                        if (sibling && sibling.nodeType == node.nodeType) {
                            sc = node;
                            var nodeLength = node.length;
                            so = sibling.length;
                            node.insertData(0, sibling.data);
                            sibling.parentNode.removeChild(sibling);
                            if (sc == ec) {
                                eo += so;
                                ec = sc;
                            } else if (ec == node.parentNode) {
                                var nodeIndex = getNodeIndex(node);
                                if (eo == nodeIndex) {
                                    ec = node;
                                    eo = nodeLength;
                                } else if (eo > nodeIndex) {
                                    eo--;
                                }
                            }
                        }
                    };

                    var normalizeStart = true;

                    if (isCharacterDataNode(ec)) {
                        if (ec.length == eo) {
                            mergeForward(ec);
                        }
                    } else {
                        if (eo > 0) {
                            var endNode = ec.childNodes[eo - 1];
                            if (endNode && isCharacterDataNode(endNode)) {
                                mergeForward(endNode);
                            }
                        }
                        normalizeStart = !this.collapsed;
                    }

                    if (normalizeStart) {
                        if (isCharacterDataNode(sc)) {
                            if (so == 0) {
                                mergeBackward(sc);
                            }
                        } else {
                            if (so < sc.childNodes.length) {
                                var startNode = sc.childNodes[so];
                                if (startNode && isCharacterDataNode(startNode)) {
                                    mergeBackward(startNode);
                                }
                            }
                        }
                    } else {
                        sc = ec;
                        so = eo;
                    }

                    boundaryUpdater(this, sc, so, ec, eo);
                },

                collapseToPoint: function(node, offset) {
                    assertNoDocTypeNotationEntityAncestor(node, true);
                    assertValidOffset(node, offset);
                    this.setStartAndEnd(node, offset);
                }
            });

            copyComparisonConstants(constructor);
        }

        /*----------------------------------------------------------------------------------------------------------------*/

        // Updates commonAncestorContainer and collapsed after boundary change
        function updateCollapsedAndCommonAncestor(range) {
            range.collapsed = (range.startContainer === range.endContainer && range.startOffset === range.endOffset);
            range.commonAncestorContainer = range.collapsed ?
                range.startContainer : dom.getCommonAncestor(range.startContainer, range.endContainer);
        }

        function updateBoundaries(range, startContainer, startOffset, endContainer, endOffset) {
            range.startContainer = startContainer;
            range.startOffset = startOffset;
            range.endContainer = endContainer;
            range.endOffset = endOffset;
            range.document = dom.getDocument(startContainer);

            updateCollapsedAndCommonAncestor(range);
        }

        function Range(doc) {
            this.startContainer = doc;
            this.startOffset = 0;
            this.endContainer = doc;
            this.endOffset = 0;
            this.document = doc;
            updateCollapsedAndCommonAncestor(this);
        }

        createPrototypeRange(Range, updateBoundaries);

        util.extend(Range, {
            rangeProperties: rangeProperties,
            RangeIterator: RangeIterator,
            copyComparisonConstants: copyComparisonConstants,
            createPrototypeRange: createPrototypeRange,
            inspect: inspect,
            toHtml: rangeToHtml,
            getRangeDocument: getRangeDocument,
            rangesEqual: function(r1, r2) {
                return r1.startContainer === r2.startContainer &&
                    r1.startOffset === r2.startOffset &&
                    r1.endContainer === r2.endContainer &&
                    r1.endOffset === r2.endOffset;
            }
        });

        api.DomRange = Range;
    });

    /*----------------------------------------------------------------------------------------------------------------*/

    // Wrappers for the browser's native DOM Range and/or TextRange implementation 
    api.createCoreModule("WrappedRange", ["DomRange"], function(api, module) {
        var WrappedRange, WrappedTextRange;
        var dom = api.dom;
        var util = api.util;
        var DomPosition = dom.DomPosition;
        var DomRange = api.DomRange;
        var getBody = dom.getBody;
        var getContentDocument = dom.getContentDocument;
        var isCharacterDataNode = dom.isCharacterDataNode;


        /*----------------------------------------------------------------------------------------------------------------*/

        if (api.features.implementsDomRange) {
            // This is a wrapper around the browser's native DOM Range. It has two aims:
            // - Provide workarounds for specific browser bugs
            // - provide convenient extensions, which are inherited from Rangy's DomRange

            (function() {
                var rangeProto;
                var rangeProperties = DomRange.rangeProperties;

                function updateRangeProperties(range) {
                    var i = rangeProperties.length, prop;
                    while (i--) {
                        prop = rangeProperties[i];
                        range[prop] = range.nativeRange[prop];
                    }
                    // Fix for broken collapsed property in IE 9.
                    range.collapsed = (range.startContainer === range.endContainer && range.startOffset === range.endOffset);
                }

                function updateNativeRange(range, startContainer, startOffset, endContainer, endOffset) {
                    var startMoved = (range.startContainer !== startContainer || range.startOffset != startOffset);
                    var endMoved = (range.endContainer !== endContainer || range.endOffset != endOffset);
                    var nativeRangeDifferent = !range.equals(range.nativeRange);

                    // Always set both boundaries for the benefit of IE9 (see issue 35)
                    if (startMoved || endMoved || nativeRangeDifferent) {
                        range.setEnd(endContainer, endOffset);
                        range.setStart(startContainer, startOffset);
                    }
                }

                var createBeforeAfterNodeSetter;

                WrappedRange = function(range) {
                    if (!range) {
                        throw module.createError("WrappedRange: Range must be specified");
                    }
                    this.nativeRange = range;
                    updateRangeProperties(this);
                };

                DomRange.createPrototypeRange(WrappedRange, updateNativeRange);

                rangeProto = WrappedRange.prototype;

                rangeProto.selectNode = function(node) {
                    this.nativeRange.selectNode(node);
                    updateRangeProperties(this);
                };

                rangeProto.cloneContents = function() {
                    return this.nativeRange.cloneContents();
                };

                // Due to a long-standing Firefox bug that I have not been able to find a reliable way to detect,
                // insertNode() is never delegated to the native range.

                rangeProto.surroundContents = function(node) {
                    this.nativeRange.surroundContents(node);
                    updateRangeProperties(this);
                };

                rangeProto.collapse = function(isStart) {
                    this.nativeRange.collapse(isStart);
                    updateRangeProperties(this);
                };

                rangeProto.cloneRange = function() {
                    return new WrappedRange(this.nativeRange.cloneRange());
                };

                rangeProto.refresh = function() {
                    updateRangeProperties(this);
                };

                rangeProto.toString = function() {
                    return this.nativeRange.toString();
                };

                // Create test range and node for feature detection

                var testTextNode = document.createTextNode("test");
                getBody(document).appendChild(testTextNode);
                var range = document.createRange();

                /*--------------------------------------------------------------------------------------------------------*/

                // Test for Firefox 2 bug that prevents moving the start of a Range to a point after its current end and
                // correct for it

                range.setStart(testTextNode, 0);
                range.setEnd(testTextNode, 0);

                try {
                    range.setStart(testTextNode, 1);

                    rangeProto.setStart = function(node, offset) {
                        this.nativeRange.setStart(node, offset);
                        updateRangeProperties(this);
                    };

                    rangeProto.setEnd = function(node, offset) {
                        this.nativeRange.setEnd(node, offset);
                        updateRangeProperties(this);
                    };

                    createBeforeAfterNodeSetter = function(name) {
                        return function(node) {
                            this.nativeRange[name](node);
                            updateRangeProperties(this);
                        };
                    };

                } catch(ex) {

                    rangeProto.setStart = function(node, offset) {
                        try {
                            this.nativeRange.setStart(node, offset);
                        } catch (ex) {
                            this.nativeRange.setEnd(node, offset);
                            this.nativeRange.setStart(node, offset);
                        }
                        updateRangeProperties(this);
                    };

                    rangeProto.setEnd = function(node, offset) {
                        try {
                            this.nativeRange.setEnd(node, offset);
                        } catch (ex) {
                            this.nativeRange.setStart(node, offset);
                            this.nativeRange.setEnd(node, offset);
                        }
                        updateRangeProperties(this);
                    };

                    createBeforeAfterNodeSetter = function(name, oppositeName) {
                        return function(node) {
                            try {
                                this.nativeRange[name](node);
                            } catch (ex) {
                                this.nativeRange[oppositeName](node);
                                this.nativeRange[name](node);
                            }
                            updateRangeProperties(this);
                        };
                    };
                }

                rangeProto.setStartBefore = createBeforeAfterNodeSetter("setStartBefore", "setEndBefore");
                rangeProto.setStartAfter = createBeforeAfterNodeSetter("setStartAfter", "setEndAfter");
                rangeProto.setEndBefore = createBeforeAfterNodeSetter("setEndBefore", "setStartBefore");
                rangeProto.setEndAfter = createBeforeAfterNodeSetter("setEndAfter", "setStartAfter");

                /*--------------------------------------------------------------------------------------------------------*/

                // Always use DOM4-compliant selectNodeContents implementation: it's simpler and less code than testing
                // whether the native implementation can be trusted
                rangeProto.selectNodeContents = function(node) {
                    this.setStartAndEnd(node, 0, dom.getNodeLength(node));
                };

                /*--------------------------------------------------------------------------------------------------------*/

                // Test for and correct WebKit bug that has the behaviour of compareBoundaryPoints round the wrong way for
                // constants START_TO_END and END_TO_START: https://bugs.webkit.org/show_bug.cgi?id=20738

                range.selectNodeContents(testTextNode);
                range.setEnd(testTextNode, 3);

                var range2 = document.createRange();
                range2.selectNodeContents(testTextNode);
                range2.setEnd(testTextNode, 4);
                range2.setStart(testTextNode, 2);

                if (range.compareBoundaryPoints(range.START_TO_END, range2) == -1 &&
                        range.compareBoundaryPoints(range.END_TO_START, range2) == 1) {
                    // This is the wrong way round, so correct for it

                    rangeProto.compareBoundaryPoints = function(type, range) {
                        range = range.nativeRange || range;
                        if (type == range.START_TO_END) {
                            type = range.END_TO_START;
                        } else if (type == range.END_TO_START) {
                            type = range.START_TO_END;
                        }
                        return this.nativeRange.compareBoundaryPoints(type, range);
                    };
                } else {
                    rangeProto.compareBoundaryPoints = function(type, range) {
                        return this.nativeRange.compareBoundaryPoints(type, range.nativeRange || range);
                    };
                }

                /*--------------------------------------------------------------------------------------------------------*/

                // Test for IE 9 deleteContents() and extractContents() bug and correct it. See issue 107.

                var el = document.createElement("div");
                el.innerHTML = "123";
                var textNode = el.firstChild;
                var body = getBody(document);
                body.appendChild(el);

                range.setStart(textNode, 1);
                range.setEnd(textNode, 2);
                range.deleteContents();

                if (textNode.data == "13") {
                    // Behaviour is correct per DOM4 Range so wrap the browser's implementation of deleteContents() and
                    // extractContents()
                    rangeProto.deleteContents = function() {
                        this.nativeRange.deleteContents();
                        updateRangeProperties(this);
                    };

                    rangeProto.extractContents = function() {
                        var frag = this.nativeRange.extractContents();
                        updateRangeProperties(this);
                        return frag;
                    };
                } else {
                }

                body.removeChild(el);
                body = null;

                /*--------------------------------------------------------------------------------------------------------*/

                // Test for existence of createContextualFragment and delegate to it if it exists
                if (util.isHostMethod(range, "createContextualFragment")) {
                    rangeProto.createContextualFragment = function(fragmentStr) {
                        return this.nativeRange.createContextualFragment(fragmentStr);
                    };
                }

                /*--------------------------------------------------------------------------------------------------------*/

                // Clean up
                getBody(document).removeChild(testTextNode);

                rangeProto.getName = function() {
                    return "WrappedRange";
                };

                api.WrappedRange = WrappedRange;

                api.createNativeRange = function(doc) {
                    doc = getContentDocument(doc, module, "createNativeRange");
                    return doc.createRange();
                };
            })();
        }
        
        if (api.features.implementsTextRange) {
            /*
            This is a workaround for a bug where IE returns the wrong container element from the TextRange's parentElement()
            method. For example, in the following (where pipes denote the selection boundaries):

            <ul id="ul"><li id="a">| a </li><li id="b"> b |</li></ul>

            var range = document.selection.createRange();
            alert(range.parentElement().id); // Should alert "ul" but alerts "b"

            This method returns the common ancestor node of the following:
            - the parentElement() of the textRange
            - the parentElement() of the textRange after calling collapse(true)
            - the parentElement() of the textRange after calling collapse(false)
            */
            var getTextRangeContainerElement = function(textRange) {
                var parentEl = textRange.parentElement();
                var range = textRange.duplicate();
                range.collapse(true);
                var startEl = range.parentElement();
                range = textRange.duplicate();
                range.collapse(false);
                var endEl = range.parentElement();
                var startEndContainer = (startEl == endEl) ? startEl : dom.getCommonAncestor(startEl, endEl);

                return startEndContainer == parentEl ? startEndContainer : dom.getCommonAncestor(parentEl, startEndContainer);
            };

            var textRangeIsCollapsed = function(textRange) {
                return textRange.compareEndPoints("StartToEnd", textRange) == 0;
            };

            // Gets the boundary of a TextRange expressed as a node and an offset within that node. This function started
            // out as an improved version of code found in Tim Cameron Ryan's IERange (http://code.google.com/p/ierange/)
            // but has grown, fixing problems with line breaks in preformatted text, adding workaround for IE TextRange
            // bugs, handling for inputs and images, plus optimizations.
            var getTextRangeBoundaryPosition = function(textRange, wholeRangeContainerElement, isStart, isCollapsed, startInfo) {
                var workingRange = textRange.duplicate();
                workingRange.collapse(isStart);
                var containerElement = workingRange.parentElement();

                // Sometimes collapsing a TextRange that's at the start of a text node can move it into the previous node, so
                // check for that
                if (!dom.isOrIsAncestorOf(wholeRangeContainerElement, containerElement)) {
                    containerElement = wholeRangeContainerElement;
                }


                // Deal with nodes that cannot "contain rich HTML markup". In practice, this means form inputs, images and
                // similar. See http://msdn.microsoft.com/en-us/library/aa703950%28VS.85%29.aspx
                if (!containerElement.canHaveHTML) {
                    var pos = new DomPosition(containerElement.parentNode, dom.getNodeIndex(containerElement));
                    return {
                        boundaryPosition: pos,
                        nodeInfo: {
                            nodeIndex: pos.offset,
                            containerElement: pos.node
                        }
                    };
                }

                var workingNode = dom.getDocument(containerElement).createElement("span");

                // Workaround for HTML5 Shiv's insane violation of document.createElement(). See Rangy issue 104 and HTML5
                // Shiv issue 64: https://github.com/aFarkas/html5shiv/issues/64
                if (workingNode.parentNode) {
                    workingNode.parentNode.removeChild(workingNode);
                }

                var comparison, workingComparisonType = isStart ? "StartToStart" : "StartToEnd";
                var previousNode, nextNode, boundaryPosition, boundaryNode;
                var start = (startInfo && startInfo.containerElement == containerElement) ? startInfo.nodeIndex : 0;
                var childNodeCount = containerElement.childNodes.length;
                var end = childNodeCount;

                // Check end first. Code within the loop assumes that the endth child node of the container is definitely
                // after the range boundary.
                var nodeIndex = end;

                while (true) {
                    if (nodeIndex == childNodeCount) {
                        containerElement.appendChild(workingNode);
                    } else {
                        containerElement.insertBefore(workingNode, containerElement.childNodes[nodeIndex]);
                    }
                    workingRange.moveToElementText(workingNode);
                    comparison = workingRange.compareEndPoints(workingComparisonType, textRange);
                    if (comparison == 0 || start == end) {
                        break;
                    } else if (comparison == -1) {
                        if (end == start + 1) {
                            // We know the endth child node is after the range boundary, so we must be done.
                            break;
                        } else {
                            start = nodeIndex;
                        }
                    } else {
                        end = (end == start + 1) ? start : nodeIndex;
                    }
                    nodeIndex = Math.floor((start + end) / 2);
                    containerElement.removeChild(workingNode);
                }


                // We've now reached or gone past the boundary of the text range we're interested in
                // so have identified the node we want
                boundaryNode = workingNode.nextSibling;

                if (comparison == -1 && boundaryNode && isCharacterDataNode(boundaryNode)) {
                    // This is a character data node (text, comment, cdata). The working range is collapsed at the start of
                    // the node containing the text range's boundary, so we move the end of the working range to the
                    // boundary point and measure the length of its text to get the boundary's offset within the node.
                    workingRange.setEndPoint(isStart ? "EndToStart" : "EndToEnd", textRange);

                    var offset;

                    if (/[\r\n]/.test(boundaryNode.data)) {
                        /*
                        For the particular case of a boundary within a text node containing rendered line breaks (within a
                        <pre> element, for example), we need a slightly complicated approach to get the boundary's offset in
                        IE. The facts:
                        
                        - Each line break is represented as \r in the text node's data/nodeValue properties
                        - Each line break is represented as \r\n in the TextRange's 'text' property
                        - The 'text' property of the TextRange does not contain trailing line breaks
                        
                        To get round the problem presented by the final fact above, we can use the fact that TextRange's
                        moveStart() and moveEnd() methods return the actual number of characters moved, which is not
                        necessarily the same as the number of characters it was instructed to move. The simplest approach is
                        to use this to store the characters moved when moving both the start and end of the range to the
                        start of the document body and subtracting the start offset from the end offset (the
                        "move-negative-gazillion" method). However, this is extremely slow when the document is large and
                        the range is near the end of it. Clearly doing the mirror image (i.e. moving the range boundaries to
                        the end of the document) has the same problem.
                        
                        Another approach that works is to use moveStart() to move the start boundary of the range up to the
                        end boundary one character at a time and incrementing a counter with the value returned by the
                        moveStart() call. However, the check for whether the start boundary has reached the end boundary is
                        expensive, so this method is slow (although unlike "move-negative-gazillion" is largely unaffected
                        by the location of the range within the document).
                        
                        The approach used below is a hybrid of the two methods above. It uses the fact that a string
                        containing the TextRange's 'text' property with each \r\n converted to a single \r character cannot
                        be longer than the text of the TextRange, so the start of the range is moved that length initially
                        and then a character at a time to make up for any trailing line breaks not contained in the 'text'
                        property. This has good performance in most situations compared to the previous two methods.
                        */
                        var tempRange = workingRange.duplicate();
                        var rangeLength = tempRange.text.replace(/\r\n/g, "\r").length;

                        offset = tempRange.moveStart("character", rangeLength);
                        while ( (comparison = tempRange.compareEndPoints("StartToEnd", tempRange)) == -1) {
                            offset++;
                            tempRange.moveStart("character", 1);
                        }
                    } else {
                        offset = workingRange.text.length;
                    }
                    boundaryPosition = new DomPosition(boundaryNode, offset);
                } else {

                    // If the boundary immediately follows a character data node and this is the end boundary, we should favour
                    // a position within that, and likewise for a start boundary preceding a character data node
                    previousNode = (isCollapsed || !isStart) && workingNode.previousSibling;
                    nextNode = (isCollapsed || isStart) && workingNode.nextSibling;
                    if (nextNode && isCharacterDataNode(nextNode)) {
                        boundaryPosition = new DomPosition(nextNode, 0);
                    } else if (previousNode && isCharacterDataNode(previousNode)) {
                        boundaryPosition = new DomPosition(previousNode, previousNode.data.length);
                    } else {
                        boundaryPosition = new DomPosition(containerElement, dom.getNodeIndex(workingNode));
                    }
                }

                // Clean up
                workingNode.parentNode.removeChild(workingNode);

                return {
                    boundaryPosition: boundaryPosition,
                    nodeInfo: {
                        nodeIndex: nodeIndex,
                        containerElement: containerElement
                    }
                };
            };

            // Returns a TextRange representing the boundary of a TextRange expressed as a node and an offset within that
            // node. This function started out as an optimized version of code found in Tim Cameron Ryan's IERange
            // (http://code.google.com/p/ierange/)
            var createBoundaryTextRange = function(boundaryPosition, isStart) {
                var boundaryNode, boundaryParent, boundaryOffset = boundaryPosition.offset;
                var doc = dom.getDocument(boundaryPosition.node);
                var workingNode, childNodes, workingRange = getBody(doc).createTextRange();
                var nodeIsDataNode = isCharacterDataNode(boundaryPosition.node);

                if (nodeIsDataNode) {
                    boundaryNode = boundaryPosition.node;
                    boundaryParent = boundaryNode.parentNode;
                } else {
                    childNodes = boundaryPosition.node.childNodes;
                    boundaryNode = (boundaryOffset < childNodes.length) ? childNodes[boundaryOffset] : null;
                    boundaryParent = boundaryPosition.node;
                }

                // Position the range immediately before the node containing the boundary
                workingNode = doc.createElement("span");

                // Making the working element non-empty element persuades IE to consider the TextRange boundary to be within
                // the element rather than immediately before or after it
                workingNode.innerHTML = "&#feff;";

                // insertBefore is supposed to work like appendChild if the second parameter is null. However, a bug report
                // for IERange suggests that it can crash the browser: http://code.google.com/p/ierange/issues/detail?id=12
                if (boundaryNode) {
                    boundaryParent.insertBefore(workingNode, boundaryNode);
                } else {
                    boundaryParent.appendChild(workingNode);
                }

                workingRange.moveToElementText(workingNode);
                workingRange.collapse(!isStart);

                // Clean up
                boundaryParent.removeChild(workingNode);

                // Move the working range to the text offset, if required
                if (nodeIsDataNode) {
                    workingRange[isStart ? "moveStart" : "moveEnd"]("character", boundaryOffset);
                }

                return workingRange;
            };

            /*------------------------------------------------------------------------------------------------------------*/

            // This is a wrapper around a TextRange, providing full DOM Range functionality using rangy's DomRange as a
            // prototype

            WrappedTextRange = function(textRange) {
                this.textRange = textRange;
                this.refresh();
            };

            WrappedTextRange.prototype = new DomRange(document);

            WrappedTextRange.prototype.refresh = function() {
                var start, end, startBoundary;

                // TextRange's parentElement() method cannot be trusted. getTextRangeContainerElement() works around that.
                var rangeContainerElement = getTextRangeContainerElement(this.textRange);

                if (textRangeIsCollapsed(this.textRange)) {
                    end = start = getTextRangeBoundaryPosition(this.textRange, rangeContainerElement, true,
                        true).boundaryPosition;
                } else {
                    startBoundary = getTextRangeBoundaryPosition(this.textRange, rangeContainerElement, true, false);
                    start = startBoundary.boundaryPosition;

                    // An optimization used here is that if the start and end boundaries have the same parent element, the
                    // search scope for the end boundary can be limited to exclude the portion of the element that precedes
                    // the start boundary
                    end = getTextRangeBoundaryPosition(this.textRange, rangeContainerElement, false, false,
                        startBoundary.nodeInfo).boundaryPosition;
                }

                this.setStart(start.node, start.offset);
                this.setEnd(end.node, end.offset);
            };

            WrappedTextRange.prototype.getName = function() {
                return "WrappedTextRange";
            };

            DomRange.copyComparisonConstants(WrappedTextRange);

            var rangeToTextRange = function(range) {
                if (range.collapsed) {
                    return createBoundaryTextRange(new DomPosition(range.startContainer, range.startOffset), true);
                } else {
                    var startRange = createBoundaryTextRange(new DomPosition(range.startContainer, range.startOffset), true);
                    var endRange = createBoundaryTextRange(new DomPosition(range.endContainer, range.endOffset), false);
                    var textRange = getBody( DomRange.getRangeDocument(range) ).createTextRange();
                    textRange.setEndPoint("StartToStart", startRange);
                    textRange.setEndPoint("EndToEnd", endRange);
                    return textRange;
                }
            };

            WrappedTextRange.rangeToTextRange = rangeToTextRange;

            WrappedTextRange.prototype.toTextRange = function() {
                return rangeToTextRange(this);
            };

            api.WrappedTextRange = WrappedTextRange;

            // IE 9 and above have both implementations and Rangy makes both available. The next few lines sets which
            // implementation to use by default.
            if (!api.features.implementsDomRange || api.config.preferTextRange) {
                // Add WrappedTextRange as the Range property of the global object to allow expression like Range.END_TO_END to work
                var globalObj = (function() { return this; })();
                if (typeof globalObj.Range == "undefined") {
                    globalObj.Range = WrappedTextRange;
                }

                api.createNativeRange = function(doc) {
                    doc = getContentDocument(doc, module, "createNativeRange");
                    return getBody(doc).createTextRange();
                };

                api.WrappedRange = WrappedTextRange;
            }
        }

        api.createRange = function(doc) {
            doc = getContentDocument(doc, module, "createRange");
            return new api.WrappedRange(api.createNativeRange(doc));
        };

        api.createRangyRange = function(doc) {
            doc = getContentDocument(doc, module, "createRangyRange");
            return new DomRange(doc);
        };

        api.createIframeRange = function(iframeEl) {
            module.deprecationNotice("createIframeRange()", "createRange(iframeEl)");
            return api.createRange(iframeEl);
        };

        api.createIframeRangyRange = function(iframeEl) {
            module.deprecationNotice("createIframeRangyRange()", "createRangyRange(iframeEl)");
            return api.createRangyRange(iframeEl);
        };

        api.addShimListener(function(win) {
            var doc = win.document;
            if (typeof doc.createRange == "undefined") {
                doc.createRange = function() {
                    return api.createRange(doc);
                };
            }
            doc = win = null;
        });
    });

    /*----------------------------------------------------------------------------------------------------------------*/

    // This module creates a selection object wrapper that conforms as closely as possible to the Selection specification
    // in the HTML Editing spec (http://dvcs.w3.org/hg/editing/raw-file/tip/editing.html#selections)
    api.createCoreModule("WrappedSelection", ["DomRange", "WrappedRange"], function(api, module) {
        api.config.checkSelectionRanges = true;

        var BOOLEAN = "boolean";
        var NUMBER = "number";
        var dom = api.dom;
        var util = api.util;
        var isHostMethod = util.isHostMethod;
        var DomRange = api.DomRange;
        var WrappedRange = api.WrappedRange;
        var DOMException = api.DOMException;
        var DomPosition = dom.DomPosition;
        var getNativeSelection;
        var selectionIsCollapsed;
        var features = api.features;
        var CONTROL = "Control";
        var getDocument = dom.getDocument;
        var getBody = dom.getBody;
        var rangesEqual = DomRange.rangesEqual;


        // Utility function to support direction parameters in the API that may be a string ("backward" or "forward") or a
        // Boolean (true for backwards).
        function isDirectionBackward(dir) {
            return (typeof dir == "string") ? /^backward(s)?$/i.test(dir) : !!dir;
        }

        function getWindow(win, methodName) {
            if (!win) {
                return window;
            } else if (dom.isWindow(win)) {
                return win;
            } else if (win instanceof WrappedSelection) {
                return win.win;
            } else {
                var doc = dom.getContentDocument(win, module, methodName);
                return dom.getWindow(doc);
            }
        }

        function getWinSelection(winParam) {
            return getWindow(winParam, "getWinSelection").getSelection();
        }

        function getDocSelection(winParam) {
            return getWindow(winParam, "getDocSelection").document.selection;
        }
        
        function winSelectionIsBackward(sel) {
            var backward = false;
            if (sel.anchorNode) {
                backward = (dom.comparePoints(sel.anchorNode, sel.anchorOffset, sel.focusNode, sel.focusOffset) == 1);
            }
            return backward;
        }

        // Test for the Range/TextRange and Selection features required
        // Test for ability to retrieve selection
        var implementsWinGetSelection = isHostMethod(window, "getSelection"),
            implementsDocSelection = util.isHostObject(document, "selection");

        features.implementsWinGetSelection = implementsWinGetSelection;
        features.implementsDocSelection = implementsDocSelection;

        var useDocumentSelection = implementsDocSelection && (!implementsWinGetSelection || api.config.preferTextRange);

        if (useDocumentSelection) {
            getNativeSelection = getDocSelection;
            api.isSelectionValid = function(winParam) {
                var doc = getWindow(winParam, "isSelectionValid").document, nativeSel = doc.selection;

                // Check whether the selection TextRange is actually contained within the correct document
                return (nativeSel.type != "None" || getDocument(nativeSel.createRange().parentElement()) == doc);
            };
        } else if (implementsWinGetSelection) {
            getNativeSelection = getWinSelection;
            api.isSelectionValid = function() {
                return true;
            };
        } else {
            module.fail("Neither document.selection or window.getSelection() detected.");
        }

        api.getNativeSelection = getNativeSelection;

        var testSelection = getNativeSelection();
        var testRange = api.createNativeRange(document);
        var body = getBody(document);

        // Obtaining a range from a selection
        var selectionHasAnchorAndFocus = util.areHostProperties(testSelection,
            ["anchorNode", "focusNode", "anchorOffset", "focusOffset"]);

        features.selectionHasAnchorAndFocus = selectionHasAnchorAndFocus;

        // Test for existence of native selection extend() method
        var selectionHasExtend = isHostMethod(testSelection, "extend");
        features.selectionHasExtend = selectionHasExtend;
        
        // Test if rangeCount exists
        var selectionHasRangeCount = (typeof testSelection.rangeCount == NUMBER);
        features.selectionHasRangeCount = selectionHasRangeCount;

        var selectionSupportsMultipleRanges = false;
        var collapsedNonEditableSelectionsSupported = true;

        var addRangeBackwardToNative = selectionHasExtend ?
            function(nativeSelection, range) {
                var doc = DomRange.getRangeDocument(range);
                var endRange = api.createRange(doc);
                endRange.collapseToPoint(range.endContainer, range.endOffset);
                nativeSelection.addRange(getNativeRange(endRange));
                nativeSelection.extend(range.startContainer, range.startOffset);
            } : null;

        if (util.areHostMethods(testSelection, ["addRange", "getRangeAt", "removeAllRanges"]) &&
                typeof testSelection.rangeCount == NUMBER && features.implementsDomRange) {

            (function() {
                // Previously an iframe was used but this caused problems in some circumstances in IE, so tests are
                // performed on the current document's selection. See issue 109.

                // Note also that if a selection previously existed, it is wiped by these tests. This should usually be fine
                // because initialization usually happens when the document loads, but could be a problem for a script that
                // loads and initializes Rangy later. If anyone complains, code could be added to save and restore the
                // selection.
                var sel = window.getSelection();
                if (sel) {
                    // Store the current selection
                    var originalSelectionRangeCount = sel.rangeCount;
                    var selectionHasMultipleRanges = (originalSelectionRangeCount > 1);
                    var originalSelectionRanges = [];
                    var originalSelectionBackward = winSelectionIsBackward(sel); 
                    for (var i = 0; i < originalSelectionRangeCount; ++i) {
                        originalSelectionRanges[i] = sel.getRangeAt(i);
                    }
                    
                    // Create some test elements
                    var body = getBody(document);
                    var testEl = body.appendChild( document.createElement("div") );
                    testEl.contentEditable = "false";
                    var textNode = testEl.appendChild( document.createTextNode("\u00a0\u00a0\u00a0") );

                    // Test whether the native selection will allow a collapsed selection within a non-editable element
                    var r1 = document.createRange();

                    r1.setStart(textNode, 1);
                    r1.collapse(true);
                    sel.addRange(r1);
                    collapsedNonEditableSelectionsSupported = (sel.rangeCount == 1);
                    sel.removeAllRanges();

                    // Test whether the native selection is capable of supporting multiple ranges.
                    if (!selectionHasMultipleRanges) {
                        // Doing the original feature test here in Chrome 36 (and presumably later versions) prints a
                        // console error of "Discontiguous selection is not supported." that cannot be suppressed. There's
                        // nothing we can do about this while retaining the feature test so we have to resort to a browser
                        // sniff. I'm not happy about it. See
                        // https://code.google.com/p/chromium/issues/detail?id=399791
                        var chromeMatch = window.navigator.appVersion.match(/Chrome\/(.*?) /);
                        if (chromeMatch && parseInt(chromeMatch[1]) >= 36) {
                            selectionSupportsMultipleRanges = false;
                        } else {
                            var r2 = r1.cloneRange();
                            r1.setStart(textNode, 0);
                            r2.setEnd(textNode, 3);
                            r2.setStart(textNode, 2);
                            sel.addRange(r1);
                            sel.addRange(r2);
                            selectionSupportsMultipleRanges = (sel.rangeCount == 2);
                        }
                    }

                    // Clean up
                    body.removeChild(testEl);
                    sel.removeAllRanges();

                    for (i = 0; i < originalSelectionRangeCount; ++i) {
                        if (i == 0 && originalSelectionBackward) {
                            if (addRangeBackwardToNative) {
                                addRangeBackwardToNative(sel, originalSelectionRanges[i]);
                            } else {
                                api.warn("Rangy initialization: original selection was backwards but selection has been restored forwards because the browser does not support Selection.extend");
                                sel.addRange(originalSelectionRanges[i]);
                            }
                        } else {
                            sel.addRange(originalSelectionRanges[i]);
                        }
                    }
                }
            })();
        }

        features.selectionSupportsMultipleRanges = selectionSupportsMultipleRanges;
        features.collapsedNonEditableSelectionsSupported = collapsedNonEditableSelectionsSupported;

        // ControlRanges
        var implementsControlRange = false, testControlRange;

        if (body && isHostMethod(body, "createControlRange")) {
            testControlRange = body.createControlRange();
            if (util.areHostProperties(testControlRange, ["item", "add"])) {
                implementsControlRange = true;
            }
        }
        features.implementsControlRange = implementsControlRange;

        // Selection collapsedness
        if (selectionHasAnchorAndFocus) {
            selectionIsCollapsed = function(sel) {
                return sel.anchorNode === sel.focusNode && sel.anchorOffset === sel.focusOffset;
            };
        } else {
            selectionIsCollapsed = function(sel) {
                return sel.rangeCount ? sel.getRangeAt(sel.rangeCount - 1).collapsed : false;
            };
        }

        function updateAnchorAndFocusFromRange(sel, range, backward) {
            var anchorPrefix = backward ? "end" : "start", focusPrefix = backward ? "start" : "end";
            sel.anchorNode = range[anchorPrefix + "Container"];
            sel.anchorOffset = range[anchorPrefix + "Offset"];
            sel.focusNode = range[focusPrefix + "Container"];
            sel.focusOffset = range[focusPrefix + "Offset"];
        }

        function updateAnchorAndFocusFromNativeSelection(sel) {
            var nativeSel = sel.nativeSelection;
            sel.anchorNode = nativeSel.anchorNode;
            sel.anchorOffset = nativeSel.anchorOffset;
            sel.focusNode = nativeSel.focusNode;
            sel.focusOffset = nativeSel.focusOffset;
        }

        function updateEmptySelection(sel) {
            sel.anchorNode = sel.focusNode = null;
            sel.anchorOffset = sel.focusOffset = 0;
            sel.rangeCount = 0;
            sel.isCollapsed = true;
            sel._ranges.length = 0;
        }

        function getNativeRange(range) {
            var nativeRange;
            if (range instanceof DomRange) {
                nativeRange = api.createNativeRange(range.getDocument());
                nativeRange.setEnd(range.endContainer, range.endOffset);
                nativeRange.setStart(range.startContainer, range.startOffset);
            } else if (range instanceof WrappedRange) {
                nativeRange = range.nativeRange;
            } else if (features.implementsDomRange && (range instanceof dom.getWindow(range.startContainer).Range)) {
                nativeRange = range;
            }
            return nativeRange;
        }

        function rangeContainsSingleElement(rangeNodes) {
            if (!rangeNodes.length || rangeNodes[0].nodeType != 1) {
                return false;
            }
            for (var i = 1, len = rangeNodes.length; i < len; ++i) {
                if (!dom.isAncestorOf(rangeNodes[0], rangeNodes[i])) {
                    return false;
                }
            }
            return true;
        }

        function getSingleElementFromRange(range) {
            var nodes = range.getNodes();
            if (!rangeContainsSingleElement(nodes)) {
                throw module.createError("getSingleElementFromRange: range " + range.inspect() + " did not consist of a single element");
            }
            return nodes[0];
        }

        // Simple, quick test which only needs to distinguish between a TextRange and a ControlRange
        function isTextRange(range) {
            return !!range && typeof range.text != "undefined";
        }

        function updateFromTextRange(sel, range) {
            // Create a Range from the selected TextRange
            var wrappedRange = new WrappedRange(range);
            sel._ranges = [wrappedRange];

            updateAnchorAndFocusFromRange(sel, wrappedRange, false);
            sel.rangeCount = 1;
            sel.isCollapsed = wrappedRange.collapsed;
        }

        function updateControlSelection(sel) {
            // Update the wrapped selection based on what's now in the native selection
            sel._ranges.length = 0;
            if (sel.docSelection.type == "None") {
                updateEmptySelection(sel);
            } else {
                var controlRange = sel.docSelection.createRange();
                if (isTextRange(controlRange)) {
                    // This case (where the selection type is "Control" and calling createRange() on the selection returns
                    // a TextRange) can happen in IE 9. It happens, for example, when all elements in the selected
                    // ControlRange have been removed from the ControlRange and removed from the document.
                    updateFromTextRange(sel, controlRange);
                } else {
                    sel.rangeCount = controlRange.length;
                    var range, doc = getDocument(controlRange.item(0));
                    for (var i = 0; i < sel.rangeCount; ++i) {
                        range = api.createRange(doc);
                        range.selectNode(controlRange.item(i));
                        sel._ranges.push(range);
                    }
                    sel.isCollapsed = sel.rangeCount == 1 && sel._ranges[0].collapsed;
                    updateAnchorAndFocusFromRange(sel, sel._ranges[sel.rangeCount - 1], false);
                }
            }
        }

        function addRangeToControlSelection(sel, range) {
            var controlRange = sel.docSelection.createRange();
            var rangeElement = getSingleElementFromRange(range);

            // Create a new ControlRange containing all the elements in the selected ControlRange plus the element
            // contained by the supplied range
            var doc = getDocument(controlRange.item(0));
            var newControlRange = getBody(doc).createControlRange();
            for (var i = 0, len = controlRange.length; i < len; ++i) {
                newControlRange.add(controlRange.item(i));
            }
            try {
                newControlRange.add(rangeElement);
            } catch (ex) {
                throw module.createError("addRange(): Element within the specified Range could not be added to control selection (does it have layout?)");
            }
            newControlRange.select();

            // Update the wrapped selection based on what's now in the native selection
            updateControlSelection(sel);
        }

        var getSelectionRangeAt;

        if (isHostMethod(testSelection, "getRangeAt")) {
            // try/catch is present because getRangeAt() must have thrown an error in some browser and some situation.
            // Unfortunately, I didn't write a comment about the specifics and am now scared to take it out. Let that be a
            // lesson to us all, especially me.
            getSelectionRangeAt = function(sel, index) {
                try {
                    return sel.getRangeAt(index);
                } catch (ex) {
                    return null;
                }
            };
        } else if (selectionHasAnchorAndFocus) {
            getSelectionRangeAt = function(sel) {
                var doc = getDocument(sel.anchorNode);
                var range = api.createRange(doc);
                range.setStartAndEnd(sel.anchorNode, sel.anchorOffset, sel.focusNode, sel.focusOffset);

                // Handle the case when the selection was selected backwards (from the end to the start in the
                // document)
                if (range.collapsed !== this.isCollapsed) {
                    range.setStartAndEnd(sel.focusNode, sel.focusOffset, sel.anchorNode, sel.anchorOffset);
                }

                return range;
            };
        }

        function WrappedSelection(selection, docSelection, win) {
            this.nativeSelection = selection;
            this.docSelection = docSelection;
            this._ranges = [];
            this.win = win;
            this.refresh();
        }

        WrappedSelection.prototype = api.selectionPrototype;

        function deleteProperties(sel) {
            sel.win = sel.anchorNode = sel.focusNode = sel._ranges = null;
            sel.rangeCount = sel.anchorOffset = sel.focusOffset = 0;
            sel.detached = true;
        }

        var cachedRangySelections = [];

        function actOnCachedSelection(win, action) {
            var i = cachedRangySelections.length, cached, sel;
            while (i--) {
                cached = cachedRangySelections[i];
                sel = cached.selection;
                if (action == "deleteAll") {
                    deleteProperties(sel);
                } else if (cached.win == win) {
                    if (action == "delete") {
                        cachedRangySelections.splice(i, 1);
                        return true;
                    } else {
                        return sel;
                    }
                }
            }
            if (action == "deleteAll") {
                cachedRangySelections.length = 0;
            }
            return null;
        }

        var getSelection = function(win) {
            // Check if the parameter is a Rangy Selection object
            if (win && win instanceof WrappedSelection) {
                win.refresh();
                return win;
            }

            win = getWindow(win, "getNativeSelection");

            var sel = actOnCachedSelection(win);
            var nativeSel = getNativeSelection(win), docSel = implementsDocSelection ? getDocSelection(win) : null;
            if (sel) {
                sel.nativeSelection = nativeSel;
                sel.docSelection = docSel;
                sel.refresh();
            } else {
                sel = new WrappedSelection(nativeSel, docSel, win);
                cachedRangySelections.push( { win: win, selection: sel } );
            }
            return sel;
        };

        api.getSelection = getSelection;

        api.getIframeSelection = function(iframeEl) {
            module.deprecationNotice("getIframeSelection()", "getSelection(iframeEl)");
            return api.getSelection(dom.getIframeWindow(iframeEl));
        };

        var selProto = WrappedSelection.prototype;

        function createControlSelection(sel, ranges) {
            // Ensure that the selection becomes of type "Control"
            var doc = getDocument(ranges[0].startContainer);
            var controlRange = getBody(doc).createControlRange();
            for (var i = 0, el, len = ranges.length; i < len; ++i) {
                el = getSingleElementFromRange(ranges[i]);
                try {
                    controlRange.add(el);
                } catch (ex) {
                    throw module.createError("setRanges(): Element within one of the specified Ranges could not be added to control selection (does it have layout?)");
                }
            }
            controlRange.select();

            // Update the wrapped selection based on what's now in the native selection
            updateControlSelection(sel);
        }

        // Selecting a range
        if (!useDocumentSelection && selectionHasAnchorAndFocus && util.areHostMethods(testSelection, ["removeAllRanges", "addRange"])) {
            selProto.removeAllRanges = function() {
                this.nativeSelection.removeAllRanges();
                updateEmptySelection(this);
            };

            var addRangeBackward = function(sel, range) {
                addRangeBackwardToNative(sel.nativeSelection, range);
                sel.refresh();
            };

            if (selectionHasRangeCount) {
                selProto.addRange = function(range, direction) {
                    if (implementsControlRange && implementsDocSelection && this.docSelection.type == CONTROL) {
                        addRangeToControlSelection(this, range);
                    } else {
                        if (isDirectionBackward(direction) && selectionHasExtend) {
                            addRangeBackward(this, range);
                        } else {
                            var previousRangeCount;
                            if (selectionSupportsMultipleRanges) {
                                previousRangeCount = this.rangeCount;
                            } else {
                                this.removeAllRanges();
                                previousRangeCount = 0;
                            }
                            // Clone the native range so that changing the selected range does not affect the selection.
                            // This is contrary to the spec but is the only way to achieve consistency between browsers. See
                            // issue 80.
                            this.nativeSelection.addRange(getNativeRange(range).cloneRange());

                            // Check whether adding the range was successful
                            this.rangeCount = this.nativeSelection.rangeCount;

                            if (this.rangeCount == previousRangeCount + 1) {
                                // The range was added successfully

                                // Check whether the range that we added to the selection is reflected in the last range extracted from
                                // the selection
                                if (api.config.checkSelectionRanges) {
                                    var nativeRange = getSelectionRangeAt(this.nativeSelection, this.rangeCount - 1);
                                    if (nativeRange && !rangesEqual(nativeRange, range)) {
                                        // Happens in WebKit with, for example, a selection placed at the start of a text node
                                        range = new WrappedRange(nativeRange);
                                    }
                                }
                                this._ranges[this.rangeCount - 1] = range;
                                updateAnchorAndFocusFromRange(this, range, selectionIsBackward(this.nativeSelection));
                                this.isCollapsed = selectionIsCollapsed(this);
                            } else {
                                // The range was not added successfully. The simplest thing is to refresh
                                this.refresh();
                            }
                        }
                    }
                };
            } else {
                selProto.addRange = function(range, direction) {
                    if (isDirectionBackward(direction) && selectionHasExtend) {
                        addRangeBackward(this, range);
                    } else {
                        this.nativeSelection.addRange(getNativeRange(range));
                        this.refresh();
                    }
                };
            }

            selProto.setRanges = function(ranges) {
                if (implementsControlRange && implementsDocSelection && ranges.length > 1) {
                    createControlSelection(this, ranges);
                } else {
                    this.removeAllRanges();
                    for (var i = 0, len = ranges.length; i < len; ++i) {
                        this.addRange(ranges[i]);
                    }
                }
            };
        } else if (isHostMethod(testSelection, "empty") && isHostMethod(testRange, "select") &&
                   implementsControlRange && useDocumentSelection) {

            selProto.removeAllRanges = function() {
                // Added try/catch as fix for issue #21
                try {
                    this.docSelection.empty();

                    // Check for empty() not working (issue #24)
                    if (this.docSelection.type != "None") {
                        // Work around failure to empty a control selection by instead selecting a TextRange and then
                        // calling empty()
                        var doc;
                        if (this.anchorNode) {
                            doc = getDocument(this.anchorNode);
                        } else if (this.docSelection.type == CONTROL) {
                            var controlRange = this.docSelection.createRange();
                            if (controlRange.length) {
                                doc = getDocument( controlRange.item(0) );
                            }
                        }
                        if (doc) {
                            var textRange = getBody(doc).createTextRange();
                            textRange.select();
                            this.docSelection.empty();
                        }
                    }
                } catch(ex) {}
                updateEmptySelection(this);
            };

            selProto.addRange = function(range) {
                if (this.docSelection.type == CONTROL) {
                    addRangeToControlSelection(this, range);
                } else {
                    api.WrappedTextRange.rangeToTextRange(range).select();
                    this._ranges[0] = range;
                    this.rangeCount = 1;
                    this.isCollapsed = this._ranges[0].collapsed;
                    updateAnchorAndFocusFromRange(this, range, false);
                }
            };

            selProto.setRanges = function(ranges) {
                this.removeAllRanges();
                var rangeCount = ranges.length;
                if (rangeCount > 1) {
                    createControlSelection(this, ranges);
                } else if (rangeCount) {
                    this.addRange(ranges[0]);
                }
            };
        } else {
            module.fail("No means of selecting a Range or TextRange was found");
            return false;
        }

        selProto.getRangeAt = function(index) {
            if (index < 0 || index >= this.rangeCount) {
                throw new DOMException("INDEX_SIZE_ERR");
            } else {
                // Clone the range to preserve selection-range independence. See issue 80.
                return this._ranges[index].cloneRange();
            }
        };

        var refreshSelection;

        if (useDocumentSelection) {
            refreshSelection = function(sel) {
                var range;
                if (api.isSelectionValid(sel.win)) {
                    range = sel.docSelection.createRange();
                } else {
                    range = getBody(sel.win.document).createTextRange();
                    range.collapse(true);
                }

                if (sel.docSelection.type == CONTROL) {
                    updateControlSelection(sel);
                } else if (isTextRange(range)) {
                    updateFromTextRange(sel, range);
                } else {
                    updateEmptySelection(sel);
                }
            };
        } else if (isHostMethod(testSelection, "getRangeAt") && typeof testSelection.rangeCount == NUMBER) {
            refreshSelection = function(sel) {
                if (implementsControlRange && implementsDocSelection && sel.docSelection.type == CONTROL) {
                    updateControlSelection(sel);
                } else {
                    sel._ranges.length = sel.rangeCount = sel.nativeSelection.rangeCount;
                    if (sel.rangeCount) {
                        for (var i = 0, len = sel.rangeCount; i < len; ++i) {
                            sel._ranges[i] = new api.WrappedRange(sel.nativeSelection.getRangeAt(i));
                        }
                        updateAnchorAndFocusFromRange(sel, sel._ranges[sel.rangeCount - 1], selectionIsBackward(sel.nativeSelection));
                        sel.isCollapsed = selectionIsCollapsed(sel);
                    } else {
                        updateEmptySelection(sel);
                    }
                }
            };
        } else if (selectionHasAnchorAndFocus && typeof testSelection.isCollapsed == BOOLEAN && typeof testRange.collapsed == BOOLEAN && features.implementsDomRange) {
            refreshSelection = function(sel) {
                var range, nativeSel = sel.nativeSelection;
                if (nativeSel.anchorNode) {
                    range = getSelectionRangeAt(nativeSel, 0);
                    sel._ranges = [range];
                    sel.rangeCount = 1;
                    updateAnchorAndFocusFromNativeSelection(sel);
                    sel.isCollapsed = selectionIsCollapsed(sel);
                } else {
                    updateEmptySelection(sel);
                }
            };
        } else {
            module.fail("No means of obtaining a Range or TextRange from the user's selection was found");
            return false;
        }

        selProto.refresh = function(checkForChanges) {
            var oldRanges = checkForChanges ? this._ranges.slice(0) : null;
            var oldAnchorNode = this.anchorNode, oldAnchorOffset = this.anchorOffset;

            refreshSelection(this);
            if (checkForChanges) {
                // Check the range count first
                var i = oldRanges.length;
                if (i != this._ranges.length) {
                    return true;
                }

                // Now check the direction. Checking the anchor position is the same is enough since we're checking all the
                // ranges after this
                if (this.anchorNode != oldAnchorNode || this.anchorOffset != oldAnchorOffset) {
                    return true;
                }

                // Finally, compare each range in turn
                while (i--) {
                    if (!rangesEqual(oldRanges[i], this._ranges[i])) {
                        return true;
                    }
                }
                return false;
            }
        };

        // Removal of a single range
        var removeRangeManually = function(sel, range) {
            var ranges = sel.getAllRanges();
            sel.removeAllRanges();
            for (var i = 0, len = ranges.length; i < len; ++i) {
                if (!rangesEqual(range, ranges[i])) {
                    sel.addRange(ranges[i]);
                }
            }
            if (!sel.rangeCount) {
                updateEmptySelection(sel);
            }
        };

        if (implementsControlRange && implementsDocSelection) {
            selProto.removeRange = function(range) {
                if (this.docSelection.type == CONTROL) {
                    var controlRange = this.docSelection.createRange();
                    var rangeElement = getSingleElementFromRange(range);

                    // Create a new ControlRange containing all the elements in the selected ControlRange minus the
                    // element contained by the supplied range
                    var doc = getDocument(controlRange.item(0));
                    var newControlRange = getBody(doc).createControlRange();
                    var el, removed = false;
                    for (var i = 0, len = controlRange.length; i < len; ++i) {
                        el = controlRange.item(i);
                        if (el !== rangeElement || removed) {
                            newControlRange.add(controlRange.item(i));
                        } else {
                            removed = true;
                        }
                    }
                    newControlRange.select();

                    // Update the wrapped selection based on what's now in the native selection
                    updateControlSelection(this);
                } else {
                    removeRangeManually(this, range);
                }
            };
        } else {
            selProto.removeRange = function(range) {
                removeRangeManually(this, range);
            };
        }

        // Detecting if a selection is backward
        var selectionIsBackward;
        if (!useDocumentSelection && selectionHasAnchorAndFocus && features.implementsDomRange) {
            selectionIsBackward = winSelectionIsBackward;

            selProto.isBackward = function() {
                return selectionIsBackward(this);
            };
        } else {
            selectionIsBackward = selProto.isBackward = function() {
                return false;
            };
        }

        // Create an alias for backwards compatibility. From 1.3, everything is "backward" rather than "backwards"
        selProto.isBackwards = selProto.isBackward;

        // Selection stringifier
        // This is conformant to the old HTML5 selections draft spec but differs from WebKit and Mozilla's implementation.
        // The current spec does not yet define this method.
        selProto.toString = function() {
            var rangeTexts = [];
            for (var i = 0, len = this.rangeCount; i < len; ++i) {
                rangeTexts[i] = "" + this._ranges[i];
            }
            return rangeTexts.join("");
        };

        function assertNodeInSameDocument(sel, node) {
            if (sel.win.document != getDocument(node)) {
                throw new DOMException("WRONG_DOCUMENT_ERR");
            }
        }

        // No current browser conforms fully to the spec for this method, so Rangy's own method is always used
        selProto.collapse = function(node, offset) {
            assertNodeInSameDocument(this, node);
            var range = api.createRange(node);
            range.collapseToPoint(node, offset);
            this.setSingleRange(range);
            this.isCollapsed = true;
        };

        selProto.collapseToStart = function() {
            if (this.rangeCount) {
                var range = this._ranges[0];
                this.collapse(range.startContainer, range.startOffset);
            } else {
                throw new DOMException("INVALID_STATE_ERR");
            }
        };

        selProto.collapseToEnd = function() {
            if (this.rangeCount) {
                var range = this._ranges[this.rangeCount - 1];
                this.collapse(range.endContainer, range.endOffset);
            } else {
                throw new DOMException("INVALID_STATE_ERR");
            }
        };

        // The spec is very specific on how selectAllChildren should be implemented so the native implementation is
        // never used by Rangy.
        selProto.selectAllChildren = function(node) {
            assertNodeInSameDocument(this, node);
            var range = api.createRange(node);
            range.selectNodeContents(node);
            this.setSingleRange(range);
        };

        selProto.deleteFromDocument = function() {
            // Sepcial behaviour required for IE's control selections
            if (implementsControlRange && implementsDocSelection && this.docSelection.type == CONTROL) {
                var controlRange = this.docSelection.createRange();
                var element;
                while (controlRange.length) {
                    element = controlRange.item(0);
                    controlRange.remove(element);
                    element.parentNode.removeChild(element);
                }
                this.refresh();
            } else if (this.rangeCount) {
                var ranges = this.getAllRanges();
                if (ranges.length) {
                    this.removeAllRanges();
                    for (var i = 0, len = ranges.length; i < len; ++i) {
                        ranges[i].deleteContents();
                    }
                    // The spec says nothing about what the selection should contain after calling deleteContents on each
                    // range. Firefox moves the selection to where the final selected range was, so we emulate that
                    this.addRange(ranges[len - 1]);
                }
            }
        };

        // The following are non-standard extensions
        selProto.eachRange = function(func, returnValue) {
            for (var i = 0, len = this._ranges.length; i < len; ++i) {
                if ( func( this.getRangeAt(i) ) ) {
                    return returnValue;
                }
            }
        };

        selProto.getAllRanges = function() {
            var ranges = [];
            this.eachRange(function(range) {
                ranges.push(range);
            });
            return ranges;
        };

        selProto.setSingleRange = function(range, direction) {
            this.removeAllRanges();
            this.addRange(range, direction);
        };

        selProto.callMethodOnEachRange = function(methodName, params) {
            var results = [];
            this.eachRange( function(range) {
                results.push( range[methodName].apply(range, params) );
            } );
            return results;
        };
        
        function createStartOrEndSetter(isStart) {
            return function(node, offset) {
                var range;
                if (this.rangeCount) {
                    range = this.getRangeAt(0);
                    range["set" + (isStart ? "Start" : "End")](node, offset);
                } else {
                    range = api.createRange(this.win.document);
                    range.setStartAndEnd(node, offset);
                }
                this.setSingleRange(range, this.isBackward());
            };
        }

        selProto.setStart = createStartOrEndSetter(true);
        selProto.setEnd = createStartOrEndSetter(false);
        
        // Add select() method to Range prototype. Any existing selection will be removed.
        api.rangePrototype.select = function(direction) {
            getSelection( this.getDocument() ).setSingleRange(this, direction);
        };

        selProto.changeEachRange = function(func) {
            var ranges = [];
            var backward = this.isBackward();

            this.eachRange(function(range) {
                func(range);
                ranges.push(range);
            });

            this.removeAllRanges();
            if (backward && ranges.length == 1) {
                this.addRange(ranges[0], "backward");
            } else {
                this.setRanges(ranges);
            }
        };

        selProto.containsNode = function(node, allowPartial) {
            return this.eachRange( function(range) {
                return range.containsNode(node, allowPartial);
            }, true ) || false;
        };

        selProto.getBookmark = function(containerNode) {
            return {
                backward: this.isBackward(),
                rangeBookmarks: this.callMethodOnEachRange("getBookmark", [containerNode])
            };
        };

        selProto.moveToBookmark = function(bookmark) {
            var selRanges = [];
            for (var i = 0, rangeBookmark, range; rangeBookmark = bookmark.rangeBookmarks[i++]; ) {
                range = api.createRange(this.win);
                range.moveToBookmark(rangeBookmark);
                selRanges.push(range);
            }
            if (bookmark.backward) {
                this.setSingleRange(selRanges[0], "backward");
            } else {
                this.setRanges(selRanges);
            }
        };

        selProto.toHtml = function() {
            var rangeHtmls = [];
            this.eachRange(function(range) {
                rangeHtmls.push( DomRange.toHtml(range) );
            });
            return rangeHtmls.join("");
        };

        if (features.implementsTextRange) {
            selProto.getNativeTextRange = function() {
                var sel, textRange;
                if ( (sel = this.docSelection) ) {
                    var range = sel.createRange();
                    if (isTextRange(range)) {
                        return range;
                    } else {
                        throw module.createError("getNativeTextRange: selection is a control selection"); 
                    }
                } else if (this.rangeCount > 0) {
                    return api.WrappedTextRange.rangeToTextRange( this.getRangeAt(0) );
                } else {
                    throw module.createError("getNativeTextRange: selection contains no range");
                }
            };
        }

        function inspect(sel) {
            var rangeInspects = [];
            var anchor = new DomPosition(sel.anchorNode, sel.anchorOffset);
            var focus = new DomPosition(sel.focusNode, sel.focusOffset);
            var name = (typeof sel.getName == "function") ? sel.getName() : "Selection";

            if (typeof sel.rangeCount != "undefined") {
                for (var i = 0, len = sel.rangeCount; i < len; ++i) {
                    rangeInspects[i] = DomRange.inspect(sel.getRangeAt(i));
                }
            }
            return "[" + name + "(Ranges: " + rangeInspects.join(", ") +
                    ")(anchor: " + anchor.inspect() + ", focus: " + focus.inspect() + "]";
        }

        selProto.getName = function() {
            return "WrappedSelection";
        };

        selProto.inspect = function() {
            return inspect(this);
        };

        selProto.detach = function() {
            actOnCachedSelection(this.win, "delete");
            deleteProperties(this);
        };

        WrappedSelection.detachAll = function() {
            actOnCachedSelection(null, "deleteAll");
        };

        WrappedSelection.inspect = inspect;
        WrappedSelection.isDirectionBackward = isDirectionBackward;

        api.Selection = WrappedSelection;

        api.selectionPrototype = selProto;

        api.addShimListener(function(win) {
            if (typeof win.getSelection == "undefined") {
                win.getSelection = function() {
                    return getSelection(win);
                };
            }
            win = null;
        });
    });
    

    /*----------------------------------------------------------------------------------------------------------------*/

    return api;
}, this);;/**
 * Selection save and restore module for Rangy.
 * Saves and restores user selections using marker invisible elements in the DOM.
 *
 * Part of Rangy, a cross-browser JavaScript range and selection library
 * http://code.google.com/p/rangy/
 *
 * Depends on Rangy core.
 *
 * Copyright 2014, Tim Down
 * Licensed under the MIT license.
 * Version: 1.3alpha.20140804
 * Build date: 4 August 2014
 */
(function(factory, global) {
    if (typeof define == "function" && define.amd) {
        // AMD. Register as an anonymous module with a dependency on Rangy.
        define(["rangy"], factory);
        /*
         } else if (typeof exports == "object") {
         // Node/CommonJS style for Browserify
         module.exports = factory;
         */
    } else {
        // No AMD or CommonJS support so we use the rangy global variable
        factory(global.rangy);
    }
})(function(rangy) {
    rangy.createModule("SaveRestore", ["WrappedRange"], function(api, module) {
        var dom = api.dom;

        var markerTextChar = "\ufeff";

        function gEBI(id, doc) {
            return (doc || document).getElementById(id);
        }

        function insertRangeBoundaryMarker(range, atStart) {
            var markerId = "selectionBoundary_" + (+new Date()) + "_" + ("" + Math.random()).slice(2);
            var markerEl;
            var doc = dom.getDocument(range.startContainer);

            // Clone the Range and collapse to the appropriate boundary point
            var boundaryRange = range.cloneRange();
            boundaryRange.collapse(atStart);

            // Create the marker element containing a single invisible character using DOM methods and insert it
            markerEl = doc.createElement("span");
            markerEl.id = markerId;
            markerEl.style.lineHeight = "0";
            markerEl.style.display = "none";
            markerEl.className = "rangySelectionBoundary";
            markerEl.appendChild(doc.createTextNode(markerTextChar));

            boundaryRange.insertNode(markerEl);
            return markerEl;
        }

        function setRangeBoundary(doc, range, markerId, atStart) {
            var markerEl = gEBI(markerId, doc);
            if (markerEl) {
                range[atStart ? "setStartBefore" : "setEndBefore"](markerEl);
                markerEl.parentNode.removeChild(markerEl);
            } else {
                module.warn("Marker element has been removed. Cannot restore selection.");
            }
        }

        function compareRanges(r1, r2) {
            return r2.compareBoundaryPoints(r1.START_TO_START, r1);
        }

        function saveRange(range, backward) {
            var startEl, endEl, doc = api.DomRange.getRangeDocument(range), text = range.toString();

            if (range.collapsed) {
                endEl = insertRangeBoundaryMarker(range, false);
                return {
                    document: doc,
                    markerId: endEl.id,
                    collapsed: true
                };
            } else {
                endEl = insertRangeBoundaryMarker(range, false);
                startEl = insertRangeBoundaryMarker(range, true);

                return {
                    document: doc,
                    startMarkerId: startEl.id,
                    endMarkerId: endEl.id,
                    collapsed: false,
                    backward: backward,
                    toString: function() {
                        return "original text: '" + text + "', new text: '" + range.toString() + "'";
                    }
                };
            }
        }

        function restoreRange(rangeInfo, normalize) {
            var doc = rangeInfo.document;
            if (typeof normalize == "undefined") {
                normalize = true;
            }
            var range = api.createRange(doc);
            if (rangeInfo.collapsed) {
                var markerEl = gEBI(rangeInfo.markerId, doc);
                if (markerEl) {
                    markerEl.style.display = "inline";
                    var previousNode = markerEl.previousSibling;

                    // Workaround for issue 17
                    if (previousNode && previousNode.nodeType == 3) {
                        markerEl.parentNode.removeChild(markerEl);
                        range.collapseToPoint(previousNode, previousNode.length);
                    } else {
                        range.collapseBefore(markerEl);
                        markerEl.parentNode.removeChild(markerEl);
                    }
                } else {
                    module.warn("Marker element has been removed. Cannot restore selection.");
                }
            } else {
                setRangeBoundary(doc, range, rangeInfo.startMarkerId, true);
                setRangeBoundary(doc, range, rangeInfo.endMarkerId, false);
            }

            if (normalize) {
                range.normalizeBoundaries();
            }

            return range;
        }

        function saveRanges(ranges, backward) {
            var rangeInfos = [], range, doc;

            // Order the ranges by position within the DOM, latest first, cloning the array to leave the original untouched
            ranges = ranges.slice(0);
            ranges.sort(compareRanges);

            for (var i = 0, len = ranges.length; i < len; ++i) {
                rangeInfos[i] = saveRange(ranges[i], backward);
            }

            // Now that all the markers are in place and DOM manipulation over, adjust each range's boundaries to lie
            // between its markers
            for (i = len - 1; i >= 0; --i) {
                range = ranges[i];
                doc = api.DomRange.getRangeDocument(range);
                if (range.collapsed) {
                    range.collapseAfter(gEBI(rangeInfos[i].markerId, doc));
                } else {
                    range.setEndBefore(gEBI(rangeInfos[i].endMarkerId, doc));
                    range.setStartAfter(gEBI(rangeInfos[i].startMarkerId, doc));
                }
            }

            return rangeInfos;
        }

        function saveSelection(win) {
            if (!api.isSelectionValid(win)) {
                module.warn("Cannot save selection. This usually happens when the selection is collapsed and the selection document has lost focus.");
                return null;
            }
            var sel = api.getSelection(win);
            var ranges = sel.getAllRanges();
            var backward = (ranges.length == 1 && sel.isBackward());

            var rangeInfos = saveRanges(ranges, backward);

            // Ensure current selection is unaffected
            if (backward) {
                sel.setSingleRange(ranges[0], "backward");
            } else {
                sel.setRanges(ranges);
            }

            return {
                win: win,
                rangeInfos: rangeInfos,
                restored: false
            };
        }

        function restoreRanges(rangeInfos) {
            var ranges = [];

            // Ranges are in reverse order of appearance in the DOM. We want to restore earliest first to avoid
            // normalization affecting previously restored ranges.
            var rangeCount = rangeInfos.length;

            for (var i = rangeCount - 1; i >= 0; i--) {
                ranges[i] = restoreRange(rangeInfos[i], true);
            }

            return ranges;
        }

        function restoreSelection(savedSelection, preserveDirection) {
            if (!savedSelection.restored) {
                var rangeInfos = savedSelection.rangeInfos;
                var sel = api.getSelection(savedSelection.win);
                var ranges = restoreRanges(rangeInfos), rangeCount = rangeInfos.length;

                if (rangeCount == 1 && preserveDirection && api.features.selectionHasExtend && rangeInfos[0].backward) {
                    sel.removeAllRanges();
                    sel.addRange(ranges[0], true);
                } else {
                    sel.setRanges(ranges);
                }

                savedSelection.restored = true;
            }
        }

        function removeMarkerElement(doc, markerId) {
            var markerEl = gEBI(markerId, doc);
            if (markerEl) {
                markerEl.parentNode.removeChild(markerEl);
            }
        }

        function removeMarkers(savedSelection) {
            var rangeInfos = savedSelection.rangeInfos;
            for (var i = 0, len = rangeInfos.length, rangeInfo; i < len; ++i) {
                rangeInfo = rangeInfos[i];
                if (rangeInfo.collapsed) {
                    removeMarkerElement(savedSelection.doc, rangeInfo.markerId);
                } else {
                    removeMarkerElement(savedSelection.doc, rangeInfo.startMarkerId);
                    removeMarkerElement(savedSelection.doc, rangeInfo.endMarkerId);
                }
            }
        }

        api.util.extend(api, {
            saveRange: saveRange,
            restoreRange: restoreRange,
            saveRanges: saveRanges,
            restoreRanges: restoreRanges,
            saveSelection: saveSelection,
            restoreSelection: restoreSelection,
            removeMarkerElement: removeMarkerElement,
            removeMarkers: removeMarkers
        });
    });
    
}, this);;/*
	Base.js, version 1.1a
	Copyright 2006-2010, Dean Edwards
	License: http://www.opensource.org/licenses/mit-license.php
*/

var Base = function() {
	// dummy
};

Base.extend = function(_instance, _static) { // subclass
	var extend = Base.prototype.extend;
	
	// build the prototype
	Base._prototyping = true;
	var proto = new this;
	extend.call(proto, _instance);
  proto.base = function() {
    // call this method from any other method to invoke that method's ancestor
  };
	delete Base._prototyping;
	
	// create the wrapper for the constructor function
	//var constructor = proto.constructor.valueOf(); //-dean
	var constructor = proto.constructor;
	var klass = proto.constructor = function() {
		if (!Base._prototyping) {
			if (this._constructing || this.constructor == klass) { // instantiation
				this._constructing = true;
				constructor.apply(this, arguments);
				delete this._constructing;
			} else if (arguments[0] != null) { // casting
				return (arguments[0].extend || extend).call(arguments[0], proto);
			}
		}
	};
	
	// build the class interface
	klass.ancestor = this;
	klass.extend = this.extend;
	klass.forEach = this.forEach;
	klass.implement = this.implement;
	klass.prototype = proto;
	klass.toString = this.toString;
	klass.valueOf = function(type) {
		//return (type == "object") ? klass : constructor; //-dean
		return (type == "object") ? klass : constructor.valueOf();
	};
	extend.call(klass, _static);
	// class initialisation
	if (typeof klass.init == "function") klass.init();
	return klass;
};

Base.prototype = {	
	extend: function(source, value) {
		if (arguments.length > 1) { // extending with a name/value pair
			var ancestor = this[source];
			if (ancestor && (typeof value == "function") && // overriding a method?
				// the valueOf() comparison is to avoid circular references
				(!ancestor.valueOf || ancestor.valueOf() != value.valueOf()) &&
				/\bbase\b/.test(value)) {
				// get the underlying method
				var method = value.valueOf();
				// override
				value = function() {
					var previous = this.base || Base.prototype.base;
					this.base = ancestor;
					var returnValue = method.apply(this, arguments);
					this.base = previous;
					return returnValue;
				};
				// point to the underlying method
				value.valueOf = function(type) {
					return (type == "object") ? value : method;
				};
				value.toString = Base.toString;
			}
			this[source] = value;
		} else if (source) { // extending with an object literal
			var extend = Base.prototype.extend;
			// if this object has a customised extend method then use it
			if (!Base._prototyping && typeof this != "function") {
				extend = this.extend || extend;
			}
			var proto = {toSource: null};
			// do the "toString" and other methods manually
			var hidden = ["constructor", "toString", "valueOf"];
			// if we are prototyping then include the constructor
			var i = Base._prototyping ? 0 : 1;
			while (key = hidden[i++]) {
				if (source[key] != proto[key]) {
					extend.call(this, key, source[key]);

				}
			}
			// copy each of the source object's properties to this object
			for (var key in source) {
				if (!proto[key]) extend.call(this, key, source[key]);
			}
		}
		return this;
	}
};

// initialise
Base = Base.extend({
	constructor: function() {
		this.extend(arguments[0]);
	}
}, {
	ancestor: Object,
	version: "1.1",
	
	forEach: function(object, block, context) {
		for (var key in object) {
			if (this.prototype[key] === undefined) {
				block.call(context, object[key], key, object);
			}
		}
	},
		
	implement: function() {
		for (var i = 0; i < arguments.length; i++) {
			if (typeof arguments[i] == "function") {
				// if it's a function, call it
				arguments[i](this.prototype);
			} else {
				// add the interface using the extend method
				this.prototype.extend(arguments[i]);
			}
		}
		return this;
	},
	
	toString: function() {
		return String(this.valueOf());
	}
});;/**
 * Detect browser support for specific features
 */
wysihtml5.browser = (function() {
  var userAgent   = navigator.userAgent,
      testElement = document.createElement("div"),
      // Browser sniffing is unfortunately needed since some behaviors are impossible to feature detect
      isGecko     = userAgent.indexOf("Gecko")        !== -1 && userAgent.indexOf("KHTML") === -1,
      isWebKit    = userAgent.indexOf("AppleWebKit/") !== -1,
      isChrome    = userAgent.indexOf("Chrome/")      !== -1,
      isOpera     = userAgent.indexOf("Opera/")       !== -1;

  function iosVersion(userAgent) {
    return +((/ipad|iphone|ipod/.test(userAgent) && userAgent.match(/ os (\d+).+? like mac os x/)) || [undefined, 0])[1];
  }

  function androidVersion(userAgent) {
    return +(userAgent.match(/android (\d+)/) || [undefined, 0])[1];
  }

  function isIE(version, equation) {
    var rv = -1,
        re;

    if (navigator.appName == 'Microsoft Internet Explorer') {
      re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
    } else if (navigator.appName == 'Netscape') {
      re = new RegExp("Trident/.*rv:([0-9]{1,}[\.0-9]{0,})");
    }

    if (re && re.exec(navigator.userAgent) != null) {
      rv = parseFloat(RegExp.$1);
    }

    if (rv === -1) { return false; }
    if (!version) { return true; }
    if (!equation) { return version === rv; }
    if (equation === "<") { return version < rv; }
    if (equation === ">") { return version > rv; }
    if (equation === "<=") { return version <= rv; }
    if (equation === ">=") { return version >= rv; }
  }

  return {
    // Static variable needed, publicly accessible, to be able override it in unit tests
    USER_AGENT: userAgent,

    /**
     * Exclude browsers that are not capable of displaying and handling
     * contentEditable as desired:
     *    - iPhone, iPad (tested iOS 4.2.2) and Android (tested 2.2) refuse to make contentEditables focusable
     *    - IE < 8 create invalid markup and crash randomly from time to time
     *
     * @return {Boolean}
     */
    supported: function() {
      var userAgent                   = this.USER_AGENT.toLowerCase(),
          // Essential for making html elements editable
          hasContentEditableSupport   = "contentEditable" in testElement,
          // Following methods are needed in order to interact with the contentEditable area
          hasEditingApiSupport        = document.execCommand && document.queryCommandSupported && document.queryCommandState,
          // document selector apis are only supported by IE 8+, Safari 4+, Chrome and Firefox 3.5+
          hasQuerySelectorSupport     = document.querySelector && document.querySelectorAll,
          // contentEditable is unusable in mobile browsers (tested iOS 4.2.2, Android 2.2, Opera Mobile, WebOS 3.05)
          isIncompatibleMobileBrowser = (this.isIos() && iosVersion(userAgent) < 5) || (this.isAndroid() && androidVersion(userAgent) < 4) || userAgent.indexOf("opera mobi") !== -1 || userAgent.indexOf("hpwos/") !== -1;
      return hasContentEditableSupport
        && hasEditingApiSupport
        && hasQuerySelectorSupport
        && !isIncompatibleMobileBrowser;
    },

    isTouchDevice: function() {
      return this.supportsEvent("touchmove");
    },

    isIos: function() {
      return (/ipad|iphone|ipod/i).test(this.USER_AGENT);
    },

    isAndroid: function() {
      return this.USER_AGENT.indexOf("Android") !== -1;
    },

    /**
     * Whether the browser supports sandboxed iframes
     * Currently only IE 6+ offers such feature <iframe security="restricted">
     *
     * http://msdn.microsoft.com/en-us/library/ms534622(v=vs.85).aspx
     * http://blogs.msdn.com/b/ie/archive/2008/01/18/using-frames-more-securely.aspx
     *
     * HTML5 sandboxed iframes are still buggy and their DOM is not reachable from the outside (except when using postMessage)
     */
    supportsSandboxedIframes: function() {
      return isIE();
    },

    /**
     * IE6+7 throw a mixed content warning when the src of an iframe
     * is empty/unset or about:blank
     * window.querySelector is implemented as of IE8
     */
    throwsMixedContentWarningWhenIframeSrcIsEmpty: function() {
      return !("querySelector" in document);
    },

    /**
     * Whether the caret is correctly displayed in contentEditable elements
     * Firefox sometimes shows a huge caret in the beginning after focusing
     */
    displaysCaretInEmptyContentEditableCorrectly: function() {
      return isIE();
    },

    /**
     * Opera and IE are the only browsers who offer the css value
     * in the original unit, thx to the currentStyle object
     * All other browsers provide the computed style in px via window.getComputedStyle
     */
    hasCurrentStyleProperty: function() {
      return "currentStyle" in testElement;
    },

    /**
     * Firefox on OSX navigates through history when hitting CMD + Arrow right/left
     */
    hasHistoryIssue: function() {
      return isGecko && navigator.platform.substr(0, 3) === "Mac";
    },

    /**
     * Whether the browser inserts a <br> when pressing enter in a contentEditable element
     */
    insertsLineBreaksOnReturn: function() {
      return isGecko;
    },

    supportsPlaceholderAttributeOn: function(element) {
      return "placeholder" in element;
    },

    supportsEvent: function(eventName) {
      return "on" + eventName in testElement || (function() {
        testElement.setAttribute("on" + eventName, "return;");
        return typeof(testElement["on" + eventName]) === "function";
      })();
    },

    /**
     * Opera doesn't correctly fire focus/blur events when clicking in- and outside of iframe
     */
    supportsEventsInIframeCorrectly: function() {
      return !isOpera;
    },

    /**
     * Everything below IE9 doesn't know how to treat HTML5 tags
     *
     * @param {Object} context The document object on which to check HTML5 support
     *
     * @example
     *    wysihtml5.browser.supportsHTML5Tags(document);
     */
    supportsHTML5Tags: function(context) {
      var element = context.createElement("div"),
          html5   = "<article>foo</article>";
      element.innerHTML = html5;
      return element.innerHTML.toLowerCase() === html5;
    },

    /**
     * Checks whether a document supports a certain queryCommand
     * In particular, Opera needs a reference to a document that has a contentEditable in it's dom tree
     * in oder to report correct results
     *
     * @param {Object} doc Document object on which to check for a query command
     * @param {String} command The query command to check for
     * @return {Boolean}
     *
     * @example
     *    wysihtml5.browser.supportsCommand(document, "bold");
     */
    supportsCommand: (function() {
      // Following commands are supported but contain bugs in some browsers
      var buggyCommands = {
        // formatBlock fails with some tags (eg. <blockquote>)
        "formatBlock":          isIE(10, "<="),
         // When inserting unordered or ordered lists in Firefox, Chrome or Safari, the current selection or line gets
         // converted into a list (<ul><li>...</li></ul>, <ol><li>...</li></ol>)
         // IE and Opera act a bit different here as they convert the entire content of the current block element into a list
        "insertUnorderedList":  isIE(),
        "insertOrderedList":    isIE()
      };

      // Firefox throws errors for queryCommandSupported, so we have to build up our own object of supported commands
      var supported = {
        "insertHTML": isGecko
      };

      return function(doc, command) {
        var isBuggy = buggyCommands[command];
        if (!isBuggy) {
          // Firefox throws errors when invoking queryCommandSupported or queryCommandEnabled
          try {
            return doc.queryCommandSupported(command);
          } catch(e1) {}

          try {
            return doc.queryCommandEnabled(command);
          } catch(e2) {
            return !!supported[command];
          }
        }
        return false;
      };
    })(),

    /**
     * IE: URLs starting with:
     *    www., http://, https://, ftp://, gopher://, mailto:, new:, snews:, telnet:, wasis:, file://,
     *    nntp://, newsrc:, ldap://, ldaps://, outlook:, mic:// and url:
     * will automatically be auto-linked when either the user inserts them via copy&paste or presses the
     * space bar when the caret is directly after such an url.
     * This behavior cannot easily be avoided in IE < 9 since the logic is hardcoded in the mshtml.dll
     * (related blog post on msdn
     * http://blogs.msdn.com/b/ieinternals/archive/2009/09/17/prevent-automatic-hyperlinking-in-contenteditable-html.aspx).
     */
    doesAutoLinkingInContentEditable: function() {
      return isIE();
    },

    /**
     * As stated above, IE auto links urls typed into contentEditable elements
     * Since IE9 it's possible to prevent this behavior
     */
    canDisableAutoLinking: function() {
      return this.supportsCommand(document, "AutoUrlDetect");
    },

    /**
     * IE leaves an empty paragraph in the contentEditable element after clearing it
     * Chrome/Safari sometimes an empty <div>
     */
    clearsContentEditableCorrectly: function() {
      return isGecko || isOpera || isWebKit;
    },

    /**
     * IE gives wrong results for getAttribute
     */
    supportsGetAttributeCorrectly: function() {
      var td = document.createElement("td");
      return td.getAttribute("rowspan") != "1";
    },

    /**
     * When clicking on images in IE, Opera and Firefox, they are selected, which makes it easy to interact with them.
     * Chrome and Safari both don't support this
     */
    canSelectImagesInContentEditable: function() {
      return isGecko || isIE() || isOpera;
    },

    /**
     * All browsers except Safari and Chrome automatically scroll the range/caret position into view
     */
    autoScrollsToCaret: function() {
      return !isWebKit;
    },

    /**
     * Check whether the browser automatically closes tags that don't need to be opened
     */
    autoClosesUnclosedTags: function() {
      var clonedTestElement = testElement.cloneNode(false),
          returnValue,
          innerHTML;

      clonedTestElement.innerHTML = "<p><div></div>";
      innerHTML                   = clonedTestElement.innerHTML.toLowerCase();
      returnValue                 = innerHTML === "<p></p><div></div>" || innerHTML === "<p><div></div></p>";

      // Cache result by overwriting current function
      this.autoClosesUnclosedTags = function() { return returnValue; };

      return returnValue;
    },

    /**
     * Whether the browser supports the native document.getElementsByClassName which returns live NodeLists
     */
    supportsNativeGetElementsByClassName: function() {
      return String(document.getElementsByClassName).indexOf("[native code]") !== -1;
    },

    /**
     * As of now (19.04.2011) only supported by Firefox 4 and Chrome
     * See https://developer.mozilla.org/en/DOM/Selection/modify
     */
    supportsSelectionModify: function() {
      return "getSelection" in window && "modify" in window.getSelection();
    },

    /**
     * Opera needs a white space after a <br> in order to position the caret correctly
     */
    needsSpaceAfterLineBreak: function() {
      return isOpera;
    },

    /**
     * Whether the browser supports the speech api on the given element
     * See http://mikepultz.com/2011/03/accessing-google-speech-api-chrome-11/
     *
     * @example
     *    var input = document.createElement("input");
     *    if (wysihtml5.browser.supportsSpeechApiOn(input)) {
     *      // ...
     *    }
     */
    supportsSpeechApiOn: function(input) {
      var chromeVersion = userAgent.match(/Chrome\/(\d+)/) || [undefined, 0];
      return chromeVersion[1] >= 11 && ("onwebkitspeechchange" in input || "speech" in input);
    },

    /**
     * IE9 crashes when setting a getter via Object.defineProperty on XMLHttpRequest or XDomainRequest
     * See https://connect.microsoft.com/ie/feedback/details/650112
     * or try the POC http://tifftiff.de/ie9_crash/
     */
    crashesWhenDefineProperty: function(property) {
      return isIE(9) && (property === "XMLHttpRequest" || property === "XDomainRequest");
    },

    /**
     * IE is the only browser who fires the "focus" event not immediately when .focus() is called on an element
     */
    doesAsyncFocus: function() {
      return isIE();
    },

    /**
     * In IE it's impssible for the user and for the selection library to set the caret after an <img> when it's the lastChild in the document
     */
    hasProblemsSettingCaretAfterImg: function() {
      return isIE();
    },

    hasUndoInContextMenu: function() {
      return isGecko || isChrome || isOpera;
    },

    /**
     * Opera sometimes doesn't insert the node at the right position when range.insertNode(someNode)
     * is used (regardless if rangy or native)
     * This especially happens when the caret is positioned right after a <br> because then
     * insertNode() will insert the node right before the <br>
     */
    hasInsertNodeIssue: function() {
      return isOpera;
    },

    /**
     * IE 8+9 don't fire the focus event of the <body> when the iframe gets focused (even though the caret gets set into the <body>)
     */
    hasIframeFocusIssue: function() {
      return isIE();
    },

    /**
     * Chrome + Safari create invalid nested markup after paste
     *
     *  <p>
     *    foo
     *    <p>bar</p> <!-- BOO! -->
     *  </p>
     */
    createsNestedInvalidMarkupAfterPaste: function() {
      return isWebKit;
    },

    supportsMutationEvents: function() {
        return ("MutationEvent" in window);
    }
  };
})();
;wysihtml5.lang.array = function(arr) {
  return {
    /**
     * Check whether a given object exists in an array
     *
     * @example
     *    wysihtml5.lang.array([1, 2]).contains(1);
     *    // => true
     *
     * Can be used to match array with array. If intersection is found true is returned
     */
    contains: function(needle) {
      if (Array.isArray(needle)) {
        for (var i = needle.length; i--;) {
          if (wysihtml5.lang.array(arr).indexOf(needle[i]) !== -1) {
            return true;
          }
        }
        return false;
      } else {
        return wysihtml5.lang.array(arr).indexOf(needle) !== -1;
      }
    },

    /**
     * Check whether a given object exists in an array and return index
     * If no elelemt found returns -1
     *
     * @example
     *    wysihtml5.lang.array([1, 2]).indexOf(2);
     *    // => 1
     */
    indexOf: function(needle) {
        if (arr.indexOf) {
          return arr.indexOf(needle);
        } else {
          for (var i=0, length=arr.length; i<length; i++) {
            if (arr[i] === needle) { return i; }
          }
          return -1;
        }
    },

    /**
     * Substract one array from another
     *
     * @example
     *    wysihtml5.lang.array([1, 2, 3, 4]).without([3, 4]);
     *    // => [1, 2]
     */
    without: function(arrayToSubstract) {
      arrayToSubstract = wysihtml5.lang.array(arrayToSubstract);
      var newArr  = [],
          i       = 0,
          length  = arr.length;
      for (; i<length; i++) {
        if (!arrayToSubstract.contains(arr[i])) {
          newArr.push(arr[i]);
        }
      }
      return newArr;
    },

    /**
     * Return a clean native array
     *
     * Following will convert a Live NodeList to a proper Array
     * @example
     *    var childNodes = wysihtml5.lang.array(document.body.childNodes).get();
     */
    get: function() {
      var i        = 0,
          length   = arr.length,
          newArray = [];
      for (; i<length; i++) {
        newArray.push(arr[i]);
      }
      return newArray;
    },

    /**
     * Creates a new array with the results of calling a provided function on every element in this array.
     * optionally this can be provided as second argument
     *
     * @example
     *    var childNodes = wysihtml5.lang.array([1,2,3,4]).map(function (value, index, array) {
            return value * 2;
     *    });
     *    // => [2,4,6,8]
     */
    map: function(callback, thisArg) {
      if (Array.prototype.map) {
        return arr.map(callback, thisArg);
      } else {
        var len = arr.length >>> 0,
            A = new Array(len),
            i = 0;
        for (; i < len; i++) {
           A[i] = callback.call(thisArg, arr[i], i, arr);
        }
        return A;
      }
    },

    /* ReturnS new array without duplicate entries
     *
     * @example
     *    var uniq = wysihtml5.lang.array([1,2,3,2,1,4]).unique();
     *    // => [1,2,3,4]
     */
    unique: function() {
      var vals = [],
          max = arr.length,
          idx = 0;

      while (idx < max) {
        if (!wysihtml5.lang.array(vals).contains(arr[idx])) {
          vals.push(arr[idx]);
        }
        idx++;
      }
      return vals;
    }

  };
};
;wysihtml5.lang.Dispatcher = Base.extend(
  /** @scope wysihtml5.lang.Dialog.prototype */ {
  on: function(eventName, handler) {
    this.events = this.events || {};
    this.events[eventName] = this.events[eventName] || [];
    this.events[eventName].push(handler);
    return this;
  },

  off: function(eventName, handler) {
    this.events = this.events || {};
    var i = 0,
        handlers,
        newHandlers;
    if (eventName) {
      handlers    = this.events[eventName] || [],
      newHandlers = [];
      for (; i<handlers.length; i++) {
        if (handlers[i] !== handler && handler) {
          newHandlers.push(handlers[i]);
        }
      }
      this.events[eventName] = newHandlers;
    } else {
      // Clean up all events
      this.events = {};
    }
    return this;
  },

  fire: function(eventName, payload) {
    this.events = this.events || {};
    var handlers = this.events[eventName] || [],
        i        = 0;
    for (; i<handlers.length; i++) {
      handlers[i].call(this, payload);
    }
    return this;
  },

  // deprecated, use .on()
  observe: function() {
    return this.on.apply(this, arguments);
  },

  // deprecated, use .off()
  stopObserving: function() {
    return this.off.apply(this, arguments);
  }
});
;wysihtml5.lang.object = function(obj) {
  return {
    /**
     * @example
     *    wysihtml5.lang.object({ foo: 1, bar: 1 }).merge({ bar: 2, baz: 3 }).get();
     *    // => { foo: 1, bar: 2, baz: 3 }
     */
    merge: function(otherObj) {
      for (var i in otherObj) {
        obj[i] = otherObj[i];
      }
      return this;
    },

    get: function() {
      return obj;
    },

    /**
     * @example
     *    wysihtml5.lang.object({ foo: 1 }).clone();
     *    // => { foo: 1 }
     */
    clone: function() {
      var newObj = {},
          i;
      for (i in obj) {
        newObj[i] = obj[i];
      }
      return newObj;
    },

    /**
     * @example
     *    wysihtml5.lang.object([]).isArray();
     *    // => true
     */
    isArray: function() {
      return Object.prototype.toString.call(obj) === "[object Array]";
    }
  };
};
;(function() {
  var WHITE_SPACE_START = /^\s+/,
      WHITE_SPACE_END   = /\s+$/,
      ENTITY_REG_EXP    = /[&<>"]/g,
      ENTITY_MAP = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': "&quot;"
      };
  wysihtml5.lang.string = function(str) {
    str = String(str);
    return {
      /**
       * @example
       *    wysihtml5.lang.string("   foo   ").trim();
       *    // => "foo"
       */
      trim: function() {
        return str.replace(WHITE_SPACE_START, "").replace(WHITE_SPACE_END, "");
      },

      /**
       * @example
       *    wysihtml5.lang.string("Hello #{name}").interpolate({ name: "Christopher" });
       *    // => "Hello Christopher"
       */
      interpolate: function(vars) {
        for (var i in vars) {
          str = this.replace("#{" + i + "}").by(vars[i]);
        }
        return str;
      },

      /**
       * @example
       *    wysihtml5.lang.string("Hello Tom").replace("Tom").with("Hans");
       *    // => "Hello Hans"
       */
      replace: function(search) {
        return {
          by: function(replace) {
            return str.split(search).join(replace);
          }
        };
      },

      /**
       * @example
       *    wysihtml5.lang.string("hello<br>").escapeHTML();
       *    // => "hello&lt;br&gt;"
       */
      escapeHTML: function() {
        return str.replace(ENTITY_REG_EXP, function(c) { return ENTITY_MAP[c]; });
      }
    };
  };
})();
;/**
 * Find urls in descendant text nodes of an element and auto-links them
 * Inspired by http://james.padolsey.com/javascript/find-and-replace-text-with-javascript/
 *
 * @param {Element} element Container element in which to search for urls
 *
 * @example
 *    <div id="text-container">Please click here: www.google.com</div>
 *    <script>wysihtml5.dom.autoLink(document.getElementById("text-container"));</script>
 */
(function(wysihtml5) {
  var /**
       * Don't auto-link urls that are contained in the following elements:
       */
      IGNORE_URLS_IN        = wysihtml5.lang.array(["CODE", "PRE", "A", "SCRIPT", "HEAD", "TITLE", "STYLE"]),
      /**
       * revision 1:
       *    /(\S+\.{1}[^\s\,\.\!]+)/g
       *
       * revision 2:
       *    /(\b(((https?|ftp):\/\/)|(www\.))[-A-Z0-9+&@#\/%?=~_|!:,.;\[\]]*[-A-Z0-9+&@#\/%=~_|])/gim
       *
       * put this in the beginning if you don't wan't to match within a word
       *    (^|[\>\(\{\[\s\>])
       */
      URL_REG_EXP           = /((https?:\/\/|www\.)[^\s<]{3,})/gi,
      TRAILING_CHAR_REG_EXP = /([^\w\/\-](,?))$/i,
      MAX_DISPLAY_LENGTH    = 100,
      BRACKETS              = { ")": "(", "]": "[", "}": "{" };

  function autoLink(element, ignoreInClasses) {
    if (_hasParentThatShouldBeIgnored(element, ignoreInClasses)) {
      return element;
    }

    if (element === element.ownerDocument.documentElement) {
      element = element.ownerDocument.body;
    }

    return _parseNode(element, ignoreInClasses);
  }

  /**
   * This is basically a rebuild of
   * the rails auto_link_urls text helper
   */
  function _convertUrlsToLinks(str) {
    return str.replace(URL_REG_EXP, function(match, url) {
      var punctuation = (url.match(TRAILING_CHAR_REG_EXP) || [])[1] || "",
          opening     = BRACKETS[punctuation];
      url = url.replace(TRAILING_CHAR_REG_EXP, "");

      if (url.split(opening).length > url.split(punctuation).length) {
        url = url + punctuation;
        punctuation = "";
      }
      var realUrl    = url,
          displayUrl = url;
      if (url.length > MAX_DISPLAY_LENGTH) {
        displayUrl = displayUrl.substr(0, MAX_DISPLAY_LENGTH) + "...";
      }
      // Add http prefix if necessary
      if (realUrl.substr(0, 4) === "www.") {
        realUrl = "http://" + realUrl;
      }

      return '<a href="' + realUrl + '">' + displayUrl + '</a>' + punctuation;
    });
  }

  /**
   * Creates or (if already cached) returns a temp element
   * for the given document object
   */
  function _getTempElement(context) {
    var tempElement = context._wysihtml5_tempElement;
    if (!tempElement) {
      tempElement = context._wysihtml5_tempElement = context.createElement("div");
    }
    return tempElement;
  }

  /**
   * Replaces the original text nodes with the newly auto-linked dom tree
   */
  function _wrapMatchesInNode(textNode) {
    var parentNode  = textNode.parentNode,
        nodeValue   = wysihtml5.lang.string(textNode.data).escapeHTML(),
        tempElement = _getTempElement(parentNode.ownerDocument);

    // We need to insert an empty/temporary <span /> to fix IE quirks
    // Elsewise IE would strip white space in the beginning
    tempElement.innerHTML = "<span></span>" + _convertUrlsToLinks(nodeValue);
    tempElement.removeChild(tempElement.firstChild);

    while (tempElement.firstChild) {
      // inserts tempElement.firstChild before textNode
      parentNode.insertBefore(tempElement.firstChild, textNode);
    }
    parentNode.removeChild(textNode);
  }

  function _hasParentThatShouldBeIgnored(node, ignoreInClasses) {
    var nodeName;
    while (node.parentNode) {
      node = node.parentNode;
      nodeName = node.nodeName;
      if (node.className && wysihtml5.lang.array(node.className.split(' ')).contains(ignoreInClasses)) {
        return true;
      }
      if (IGNORE_URLS_IN.contains(nodeName)) {
        return true;
      } else if (nodeName === "body") {
        return false;
      }
    }
    return false;
  }

  function _parseNode(element, ignoreInClasses) {
    if (IGNORE_URLS_IN.contains(element.nodeName)) {
      return;
    }

    if (element.className && wysihtml5.lang.array(element.className.split(' ')).contains(ignoreInClasses)) {
      return;
    }

    if (element.nodeType === wysihtml5.TEXT_NODE && element.data.match(URL_REG_EXP)) {
      _wrapMatchesInNode(element);
      return;
    }

    var childNodes        = wysihtml5.lang.array(element.childNodes).get(),
        childNodesLength  = childNodes.length,
        i                 = 0;

    for (; i<childNodesLength; i++) {
      _parseNode(childNodes[i], ignoreInClasses);
    }

    return element;
  }

  wysihtml5.dom.autoLink = autoLink;

  // Reveal url reg exp to the outside
  wysihtml5.dom.autoLink.URL_REG_EXP = URL_REG_EXP;
})(wysihtml5);
;(function(wysihtml5) {
  var api = wysihtml5.dom;

  api.addClass = function(element, className) {
    var classList = element.classList;
    if (classList) {
      return classList.add(className);
    }
    if (api.hasClass(element, className)) {
      return;
    }
    element.className += " " + className;
  };

  api.removeClass = function(element, className) {
    var classList = element.classList;
    if (classList) {
      return classList.remove(className);
    }

    element.className = element.className.replace(new RegExp("(^|\\s+)" + className + "(\\s+|$)"), " ");
  };

  api.hasClass = function(element, className) {
    var classList = element.classList;
    if (classList) {
      return classList.contains(className);
    }

    var elementClassName = element.className;
    return (elementClassName.length > 0 && (elementClassName == className || new RegExp("(^|\\s)" + className + "(\\s|$)").test(elementClassName)));
  };
})(wysihtml5);
;wysihtml5.dom.contains = (function() {
  var documentElement = document.documentElement;
  if (documentElement.contains) {
    return function(container, element) {
      if (element.nodeType !== wysihtml5.ELEMENT_NODE) {
        element = element.parentNode;
      }
      return container !== element && container.contains(element);
    };
  } else if (documentElement.compareDocumentPosition) {
    return function(container, element) {
      // https://developer.mozilla.org/en/DOM/Node.compareDocumentPosition
      return !!(container.compareDocumentPosition(element) & 16);
    };
  }
})();
;/**
 * Converts an HTML fragment/element into a unordered/ordered list
 *
 * @param {Element} element The element which should be turned into a list
 * @param {String} listType The list type in which to convert the tree (either "ul" or "ol")
 * @return {Element} The created list
 *
 * @example
 *    <!-- Assume the following dom: -->
 *    <span id="pseudo-list">
 *      eminem<br>
 *      dr. dre
 *      <div>50 Cent</div>
 *    </span>
 *
 *    <script>
 *      wysihtml5.dom.convertToList(document.getElementById("pseudo-list"), "ul");
 *    </script>
 *
 *    <!-- Will result in: -->
 *    <ul>
 *      <li>eminem</li>
 *      <li>dr. dre</li>
 *      <li>50 Cent</li>
 *    </ul>
 */
wysihtml5.dom.convertToList = (function() {
  function _createListItem(doc, list) {
    var listItem = doc.createElement("li");
    list.appendChild(listItem);
    return listItem;
  }

  function _createList(doc, type) {
    return doc.createElement(type);
  }

  function convertToList(element, listType, uneditableClass) {
    if (element.nodeName === "UL" || element.nodeName === "OL" || element.nodeName === "MENU") {
      // Already a list
      return element;
    }

    var doc               = element.ownerDocument,
        list              = _createList(doc, listType),
        lineBreaks        = element.querySelectorAll("br"),
        lineBreaksLength  = lineBreaks.length,
        childNodes,
        childNodesLength,
        childNode,
        lineBreak,
        parentNode,
        isBlockElement,
        isLineBreak,
        currentListItem,
        i;

    // First find <br> at the end of inline elements and move them behind them
    for (i=0; i<lineBreaksLength; i++) {
      lineBreak = lineBreaks[i];
      while ((parentNode = lineBreak.parentNode) && parentNode !== element && parentNode.lastChild === lineBreak) {
        if (wysihtml5.dom.getStyle("display").from(parentNode) === "block") {
          parentNode.removeChild(lineBreak);
          break;
        }
        wysihtml5.dom.insert(lineBreak).after(lineBreak.parentNode);
      }
    }

    childNodes        = wysihtml5.lang.array(element.childNodes).get();
    childNodesLength  = childNodes.length;

    for (i=0; i<childNodesLength; i++) {
      currentListItem   = currentListItem || _createListItem(doc, list);
      childNode         = childNodes[i];
      isBlockElement    = wysihtml5.dom.getStyle("display").from(childNode) === "block";
      isLineBreak       = childNode.nodeName === "BR";

      // consider uneditable as an inline element
      if (isBlockElement && (!uneditableClass || !wysihtml5.dom.hasClass(childNode, uneditableClass))) {
        // Append blockElement to current <li> if empty, otherwise create a new one
        currentListItem = currentListItem.firstChild ? _createListItem(doc, list) : currentListItem;
        currentListItem.appendChild(childNode);
        currentListItem = null;
        continue;
      }

      if (isLineBreak) {
        // Only create a new list item in the next iteration when the current one has already content
        currentListItem = currentListItem.firstChild ? null : currentListItem;
        continue;
      }

      currentListItem.appendChild(childNode);
    }

    if (childNodes.length === 0) {
      _createListItem(doc, list);
    }

    element.parentNode.replaceChild(list, element);
    return list;
  }

  return convertToList;
})();
;/**
 * Copy a set of attributes from one element to another
 *
 * @param {Array} attributesToCopy List of attributes which should be copied
 * @return {Object} Returns an object which offers the "from" method which can be invoked with the element where to
 *    copy the attributes from., this again returns an object which provides a method named "to" which can be invoked
 *    with the element where to copy the attributes to (see example)
 *
 * @example
 *    var textarea    = document.querySelector("textarea"),
 *        div         = document.querySelector("div[contenteditable=true]"),
 *        anotherDiv  = document.querySelector("div.preview");
 *    wysihtml5.dom.copyAttributes(["spellcheck", "value", "placeholder"]).from(textarea).to(div).andTo(anotherDiv);
 *
 */
wysihtml5.dom.copyAttributes = function(attributesToCopy) {
  return {
    from: function(elementToCopyFrom) {
      return {
        to: function(elementToCopyTo) {
          var attribute,
              i         = 0,
              length    = attributesToCopy.length;
          for (; i<length; i++) {
            attribute = attributesToCopy[i];
            if (typeof(elementToCopyFrom[attribute]) !== "undefined" && elementToCopyFrom[attribute] !== "") {
              elementToCopyTo[attribute] = elementToCopyFrom[attribute];
            }
          }
          return { andTo: arguments.callee };
        }
      };
    }
  };
};
;/**
 * Copy a set of styles from one element to another
 * Please note that this only works properly across browsers when the element from which to copy the styles
 * is in the dom
 *
 * Interesting article on how to copy styles
 *
 * @param {Array} stylesToCopy List of styles which should be copied
 * @return {Object} Returns an object which offers the "from" method which can be invoked with the element where to
 *    copy the styles from., this again returns an object which provides a method named "to" which can be invoked
 *    with the element where to copy the styles to (see example)
 *
 * @example
 *    var textarea    = document.querySelector("textarea"),
 *        div         = document.querySelector("div[contenteditable=true]"),
 *        anotherDiv  = document.querySelector("div.preview");
 *    wysihtml5.dom.copyStyles(["overflow-y", "width", "height"]).from(textarea).to(div).andTo(anotherDiv);
 *
 */
(function(dom) {

  /**
   * Mozilla, WebKit and Opera recalculate the computed width when box-sizing: boder-box; is set
   * So if an element has "width: 200px; -moz-box-sizing: border-box; border: 1px;" then
   * its computed css width will be 198px
   *
   * See https://bugzilla.mozilla.org/show_bug.cgi?id=520992
   */
  var BOX_SIZING_PROPERTIES = ["-webkit-box-sizing", "-moz-box-sizing", "-ms-box-sizing", "box-sizing"];

  var shouldIgnoreBoxSizingBorderBox = function(element) {
    if (hasBoxSizingBorderBox(element)) {
       return parseInt(dom.getStyle("width").from(element), 10) < element.offsetWidth;
    }
    return false;
  };

  var hasBoxSizingBorderBox = function(element) {
    var i       = 0,
        length  = BOX_SIZING_PROPERTIES.length;
    for (; i<length; i++) {
      if (dom.getStyle(BOX_SIZING_PROPERTIES[i]).from(element) === "border-box") {
        return BOX_SIZING_PROPERTIES[i];
      }
    }
  };

  dom.copyStyles = function(stylesToCopy) {
    return {
      from: function(element) {
        if (shouldIgnoreBoxSizingBorderBox(element)) {
          stylesToCopy = wysihtml5.lang.array(stylesToCopy).without(BOX_SIZING_PROPERTIES);
        }

        var cssText = "",
            length  = stylesToCopy.length,
            i       = 0,
            property;
        for (; i<length; i++) {
          property = stylesToCopy[i];
          cssText += property + ":" + dom.getStyle(property).from(element) + ";";
        }

        return {
          to: function(element) {
            dom.setStyles(cssText).on(element);
            return { andTo: arguments.callee };
          }
        };
      }
    };
  };
})(wysihtml5.dom);
;/**
 * Event Delegation
 *
 * @example
 *    wysihtml5.dom.delegate(document.body, "a", "click", function() {
 *      // foo
 *    });
 */
(function(wysihtml5) {

  wysihtml5.dom.delegate = function(container, selector, eventName, handler) {
    return wysihtml5.dom.observe(container, eventName, function(event) {
      var target    = event.target,
          match     = wysihtml5.lang.array(container.querySelectorAll(selector));

      while (target && target !== container) {
        if (match.contains(target)) {
          handler.call(target, event);
          break;
        }
        target = target.parentNode;
      }
    });
  };

})(wysihtml5);
;// TODO: Refactor dom tree traversing here
(function(wysihtml5) {
  wysihtml5.dom.domNode = function(node) {
    var defaultNodeTypes = [wysihtml5.ELEMENT_NODE, wysihtml5.TEXT_NODE];

    var _isBlankText = function(node) {
      return node.nodeType === wysihtml5.TEXT_NODE && (/^\s*$/g).test(node.data);
    };

    return {

      // var node = wysihtml5.dom.domNode(element).prev({nodeTypes: [1,3], ignoreBlankTexts: true});
      prev: function(options) {
        var prevNode = node.previousSibling,
            types = (options && options.nodeTypes) ? options.nodeTypes : defaultNodeTypes;
        
        if (!prevNode) {
          return null;
        }

        if (
          (!wysihtml5.lang.array(types).contains(prevNode.nodeType)) || // nodeTypes check.
          (options && options.ignoreBlankTexts && _isBlankText(prevNode)) // Blank text nodes bypassed if set
        ) {
          return wysihtml5.dom.domNode(prevNode).prev(options);
        }
        
        return prevNode;
      },

      // var node = wysihtml5.dom.domNode(element).next({nodeTypes: [1,3], ignoreBlankTexts: true});
      next: function(options) {
        var nextNode = node.nextSibling,
            types = (options && options.nodeTypes) ? options.nodeTypes : defaultNodeTypes;
        
        if (!nextNode) {
          return null;
        }

        if (
          (!wysihtml5.lang.array(types).contains(nextNode.nodeType)) || // nodeTypes check.
          (options && options.ignoreBlankTexts && _isBlankText(nextNode)) // blank text nodes bypassed if set
        ) {
          return wysihtml5.dom.domNode(nextNode).next(options);
        }
        
        return nextNode;
      }



    };
  };
})(wysihtml5);;/**
 * Returns the given html wrapped in a div element
 *
 * Fixing IE's inability to treat unknown elements (HTML5 section, article, ...) correctly
 * when inserted via innerHTML
 *
 * @param {String} html The html which should be wrapped in a dom element
 * @param {Obejct} [context] Document object of the context the html belongs to
 *
 * @example
 *    wysihtml5.dom.getAsDom("<article>foo</article>");
 */
wysihtml5.dom.getAsDom = (function() {

  var _innerHTMLShiv = function(html, context) {
    var tempElement = context.createElement("div");
    tempElement.style.display = "none";
    context.body.appendChild(tempElement);
    // IE throws an exception when trying to insert <frameset></frameset> via innerHTML
    try { tempElement.innerHTML = html; } catch(e) {}
    context.body.removeChild(tempElement);
    return tempElement;
  };

  /**
   * Make sure IE supports HTML5 tags, which is accomplished by simply creating one instance of each element
   */
  var _ensureHTML5Compatibility = function(context) {
    if (context._wysihtml5_supportsHTML5Tags) {
      return;
    }
    for (var i=0, length=HTML5_ELEMENTS.length; i<length; i++) {
      context.createElement(HTML5_ELEMENTS[i]);
    }
    context._wysihtml5_supportsHTML5Tags = true;
  };


  /**
   * List of html5 tags
   * taken from http://simon.html5.org/html5-elements
   */
  var HTML5_ELEMENTS = [
    "abbr", "article", "aside", "audio", "bdi", "canvas", "command", "datalist", "details", "figcaption",
    "figure", "footer", "header", "hgroup", "keygen", "mark", "meter", "nav", "output", "progress",
    "rp", "rt", "ruby", "svg", "section", "source", "summary", "time", "track", "video", "wbr"
  ];

  return function(html, context) {
    context = context || document;
    var tempElement;
    if (typeof(html) === "object" && html.nodeType) {
      tempElement = context.createElement("div");
      tempElement.appendChild(html);
    } else if (wysihtml5.browser.supportsHTML5Tags(context)) {
      tempElement = context.createElement("div");
      tempElement.innerHTML = html;
    } else {
      _ensureHTML5Compatibility(context);
      tempElement = _innerHTMLShiv(html, context);
    }
    return tempElement;
  };
})();
;/**
 * Walks the dom tree from the given node up until it finds a match
 * Designed for optimal performance.
 *
 * @param {Element} node The from which to check the parent nodes
 * @param {Object} matchingSet Object to match against (possible properties: nodeName, className, classRegExp)
 * @param {Number} [levels] How many parents should the function check up from the current node (defaults to 50)
 * @return {null|Element} Returns the first element that matched the desiredNodeName(s)
 * @example
 *    var listElement = wysihtml5.dom.getParentElement(document.querySelector("li"), { nodeName: ["MENU", "UL", "OL"] });
 *    // ... or ...
 *    var unorderedListElement = wysihtml5.dom.getParentElement(document.querySelector("li"), { nodeName: "UL" });
 *    // ... or ...
 *    var coloredElement = wysihtml5.dom.getParentElement(myTextNode, { nodeName: "SPAN", className: "wysiwyg-color-red", classRegExp: /wysiwyg-color-[a-z]/g });
 */
wysihtml5.dom.getParentElement = (function() {

  function _isSameNodeName(nodeName, desiredNodeNames) {
    if (!desiredNodeNames || !desiredNodeNames.length) {
      return true;
    }

    if (typeof(desiredNodeNames) === "string") {
      return nodeName === desiredNodeNames;
    } else {
      return wysihtml5.lang.array(desiredNodeNames).contains(nodeName);
    }
  }

  function _isElement(node) {
    return node.nodeType === wysihtml5.ELEMENT_NODE;
  }

  function _hasClassName(element, className, classRegExp) {
    var classNames = (element.className || "").match(classRegExp) || [];
    if (!className) {
      return !!classNames.length;
    }
    return classNames[classNames.length - 1] === className;
  }

  function _hasStyle(element, cssStyle, styleRegExp) {
    var styles = (element.getAttribute('style') || "").match(styleRegExp) || [];
    if (!cssStyle) {
      return !!styles.length;
    }
    return styles[styles.length - 1] === cssStyle;
  }

  return function(node, matchingSet, levels, container) {
    var findByStyle = (matchingSet.cssStyle || matchingSet.styleRegExp),
        findByClass = (matchingSet.className || matchingSet.classRegExp);

    levels = levels || 50; // Go max 50 nodes upwards from current node

    while (levels-- && node && node.nodeName !== "BODY" && (!container || node !== container)) {
      if (_isElement(node) && _isSameNodeName(node.nodeName, matchingSet.nodeName) &&
          (!findByStyle || _hasStyle(node, matchingSet.cssStyle, matchingSet.styleRegExp)) &&
          (!findByClass || _hasClassName(node, matchingSet.className, matchingSet.classRegExp))
      ) {
        return node;
      }
      node = node.parentNode;
    }
    return null;
  };
})();
;/**
 * Get element's style for a specific css property
 *
 * @param {Element} element The element on which to retrieve the style
 * @param {String} property The CSS property to retrieve ("float", "display", "text-align", ...)
 *
 * @example
 *    wysihtml5.dom.getStyle("display").from(document.body);
 *    // => "block"
 */
wysihtml5.dom.getStyle = (function() {
  var stylePropertyMapping = {
        "float": ("styleFloat" in document.createElement("div").style) ? "styleFloat" : "cssFloat"
      },
      REG_EXP_CAMELIZE = /\-[a-z]/g;

  function camelize(str) {
    return str.replace(REG_EXP_CAMELIZE, function(match) {
      return match.charAt(1).toUpperCase();
    });
  }

  return function(property) {
    return {
      from: function(element) {
        if (element.nodeType !== wysihtml5.ELEMENT_NODE) {
          return;
        }

        var doc               = element.ownerDocument,
            camelizedProperty = stylePropertyMapping[property] || camelize(property),
            style             = element.style,
            currentStyle      = element.currentStyle,
            styleValue        = style[camelizedProperty];
        if (styleValue) {
          return styleValue;
        }

        // currentStyle is no standard and only supported by Opera and IE but it has one important advantage over the standard-compliant
        // window.getComputedStyle, since it returns css property values in their original unit:
        // If you set an elements width to "50%", window.getComputedStyle will give you it's current width in px while currentStyle
        // gives you the original "50%".
        // Opera supports both, currentStyle and window.getComputedStyle, that's why checking for currentStyle should have higher prio
        if (currentStyle) {
          try {
            return currentStyle[camelizedProperty];
          } catch(e) {
            //ie will occasionally fail for unknown reasons. swallowing exception
          }
        }

        var win                 = doc.defaultView || doc.parentWindow,
            needsOverflowReset  = (property === "height" || property === "width") && element.nodeName === "TEXTAREA",
            originalOverflow,
            returnValue;

        if (win.getComputedStyle) {
          // Chrome and Safari both calculate a wrong width and height for textareas when they have scroll bars
          // therfore we remove and restore the scrollbar and calculate the value in between
          if (needsOverflowReset) {
            originalOverflow = style.overflow;
            style.overflow = "hidden";
          }
          returnValue = win.getComputedStyle(element, null).getPropertyValue(property);
          if (needsOverflowReset) {
            style.overflow = originalOverflow || "";
          }
          return returnValue;
        }
      }
    };
  };
})();
;wysihtml5.dom.getTextNodes = function(node, ingoreEmpty){
  var all = [];
  for (node=node.firstChild;node;node=node.nextSibling){
    if (node.nodeType == 3) {
      if (!ingoreEmpty || !(/^\s*$/).test(node.innerText || node.textContent)) {
        all.push(node);
      }
    } else {
      all = all.concat(wysihtml5.dom.getTextNodes(node, ingoreEmpty));
    }
  }
  return all;
};;/**
 * High performant way to check whether an element with a specific tag name is in the given document
 * Optimized for being heavily executed
 * Unleashes the power of live node lists
 *
 * @param {Object} doc The document object of the context where to check
 * @param {String} tagName Upper cased tag name
 * @example
 *    wysihtml5.dom.hasElementWithTagName(document, "IMG");
 */
wysihtml5.dom.hasElementWithTagName = (function() {
  var LIVE_CACHE          = {},
      DOCUMENT_IDENTIFIER = 1;

  function _getDocumentIdentifier(doc) {
    return doc._wysihtml5_identifier || (doc._wysihtml5_identifier = DOCUMENT_IDENTIFIER++);
  }

  return function(doc, tagName) {
    var key         = _getDocumentIdentifier(doc) + ":" + tagName,
        cacheEntry  = LIVE_CACHE[key];
    if (!cacheEntry) {
      cacheEntry = LIVE_CACHE[key] = doc.getElementsByTagName(tagName);
    }

    return cacheEntry.length > 0;
  };
})();
;/**
 * High performant way to check whether an element with a specific class name is in the given document
 * Optimized for being heavily executed
 * Unleashes the power of live node lists
 *
 * @param {Object} doc The document object of the context where to check
 * @param {String} tagName Upper cased tag name
 * @example
 *    wysihtml5.dom.hasElementWithClassName(document, "foobar");
 */
(function(wysihtml5) {
  var LIVE_CACHE          = {},
      DOCUMENT_IDENTIFIER = 1;

  function _getDocumentIdentifier(doc) {
    return doc._wysihtml5_identifier || (doc._wysihtml5_identifier = DOCUMENT_IDENTIFIER++);
  }

  wysihtml5.dom.hasElementWithClassName = function(doc, className) {
    // getElementsByClassName is not supported by IE<9
    // but is sometimes mocked via library code (which then doesn't return live node lists)
    if (!wysihtml5.browser.supportsNativeGetElementsByClassName()) {
      return !!doc.querySelector("." + className);
    }

    var key         = _getDocumentIdentifier(doc) + ":" + className,
        cacheEntry  = LIVE_CACHE[key];
    if (!cacheEntry) {
      cacheEntry = LIVE_CACHE[key] = doc.getElementsByClassName(className);
    }

    return cacheEntry.length > 0;
  };
})(wysihtml5);
;wysihtml5.dom.insert = function(elementToInsert) {
  return {
    after: function(element) {
      element.parentNode.insertBefore(elementToInsert, element.nextSibling);
    },

    before: function(element) {
      element.parentNode.insertBefore(elementToInsert, element);
    },

    into: function(element) {
      element.appendChild(elementToInsert);
    }
  };
};
;wysihtml5.dom.insertCSS = function(rules) {
  rules = rules.join("\n");

  return {
    into: function(doc) {
      var styleElement = doc.createElement("style");
      styleElement.type = "text/css";

      if (styleElement.styleSheet) {
        styleElement.styleSheet.cssText = rules;
      } else {
        styleElement.appendChild(doc.createTextNode(rules));
      }

      var link = doc.querySelector("head link");
      if (link) {
        link.parentNode.insertBefore(styleElement, link);
        return;
      } else {
        var head = doc.querySelector("head");
        if (head) {
          head.appendChild(styleElement);
        }
      }
    }
  };
};
;// TODO: Refactor dom tree traversing here
(function(wysihtml5) {
  wysihtml5.dom.lineBreaks = function(node) {

    function _isLineBreak(n) {
      return n.nodeName === "BR";
    }

    /**
     * Checks whether the elment causes a visual line break
     * (<br> or block elements)
     */
    function _isLineBreakOrBlockElement(element) {
      if (_isLineBreak(element)) {
        return true;
      }

      if (wysihtml5.dom.getStyle("display").from(element) === "block") {
        return true;
      }

      return false;
    }

    return {

      /* wysihtml5.dom.lineBreaks(element).add();
       *
       * Adds line breaks before and after the given node if the previous and next siblings
       * aren't already causing a visual line break (block element or <br>)
       */
      add: function(options) {
        var doc             = node.ownerDocument,
          nextSibling     = wysihtml5.dom.domNode(node).next({ignoreBlankTexts: true}),
          previousSibling = wysihtml5.dom.domNode(node).prev({ignoreBlankTexts: true});

        if (nextSibling && !_isLineBreakOrBlockElement(nextSibling)) {
          wysihtml5.dom.insert(doc.createElement("br")).after(node);
        }
        if (previousSibling && !_isLineBreakOrBlockElement(previousSibling)) {
          wysihtml5.dom.insert(doc.createElement("br")).before(node);
        }
      },

      /* wysihtml5.dom.lineBreaks(element).remove();
       *
       * Removes line breaks before and after the given node
       */
      remove: function(options) {
        var nextSibling     = wysihtml5.dom.domNode(node).next({ignoreBlankTexts: true}),
            previousSibling = wysihtml5.dom.domNode(node).prev({ignoreBlankTexts: true});

        if (nextSibling && _isLineBreak(nextSibling)) {
          nextSibling.parentNode.removeChild(nextSibling);
        }
        if (previousSibling && _isLineBreak(previousSibling)) {
          previousSibling.parentNode.removeChild(previousSibling);
        }
      }
    };
  };
})(wysihtml5);;/**
 * Method to set dom events
 *
 * @example
 *    wysihtml5.dom.observe(iframe.contentWindow.document.body, ["focus", "blur"], function() { ... });
 */
wysihtml5.dom.observe = function(element, eventNames, handler) {
  eventNames = typeof(eventNames) === "string" ? [eventNames] : eventNames;

  var handlerWrapper,
      eventName,
      i       = 0,
      length  = eventNames.length;

  for (; i<length; i++) {
    eventName = eventNames[i];
    if (element.addEventListener) {
      element.addEventListener(eventName, handler, false);
    } else {
      handlerWrapper = function(event) {
        if (!("target" in event)) {
          event.target = event.srcElement;
        }
        event.preventDefault = event.preventDefault || function() {
          this.returnValue = false;
        };
        event.stopPropagation = event.stopPropagation || function() {
          this.cancelBubble = true;
        };
        handler.call(element, event);
      };
      element.attachEvent("on" + eventName, handlerWrapper);
    }
  }

  return {
    stop: function() {
      var eventName,
          i       = 0,
          length  = eventNames.length;
      for (; i<length; i++) {
        eventName = eventNames[i];
        if (element.removeEventListener) {
          element.removeEventListener(eventName, handler, false);
        } else {
          element.detachEvent("on" + eventName, handlerWrapper);
        }
      }
    }
  };
};
;/**
 * HTML Sanitizer
 * Rewrites the HTML based on given rules
 *
 * @param {Element|String} elementOrHtml HTML String to be sanitized OR element whose content should be sanitized
 * @param {Object} [rules] List of rules for rewriting the HTML, if there's no rule for an element it will
 *    be converted to a "span". Each rule is a key/value pair where key is the tag to convert, and value the
 *    desired substitution.
 * @param {Object} context Document object in which to parse the html, needed to sandbox the parsing
 *
 * @return {Element|String} Depends on the elementOrHtml parameter. When html then the sanitized html as string elsewise the element.
 *
 * @example
 *    var userHTML = '<div id="foo" onclick="alert(1);"><p><font color="red">foo</font><script>alert(1);</script></p></div>';
 *    wysihtml5.dom.parse(userHTML, {
 *      tags {
 *        p:      "div",      // Rename p tags to div tags
 *        font:   "span"      // Rename font tags to span tags
 *        div:    true,       // Keep them, also possible (same result when passing: "div" or true)
 *        script: undefined   // Remove script elements
 *      }
 *    });
 *    // => <div><div><span>foo bar</span></div></div>
 *
 *    var userHTML = '<table><tbody><tr><td>I'm a table!</td></tr></tbody></table>';
 *    wysihtml5.dom.parse(userHTML);
 *    // => '<span><span><span><span>I'm a table!</span></span></span></span>'
 *
 *    var userHTML = '<div>foobar<br>foobar</div>';
 *    wysihtml5.dom.parse(userHTML, {
 *      tags: {
 *        div: undefined,
 *        br:  true
 *      }
 *    });
 *    // => ''
 *
 *    var userHTML = '<div class="red">foo</div><div class="pink">bar</div>';
 *    wysihtml5.dom.parse(userHTML, {
 *      classes: {
 *        red:    1,
 *        green:  1
 *      },
 *      tags: {
 *        div: {
 *          rename_tag:     "p"
 *        }
 *      }
 *    });
 *    // => '<p class="red">foo</p><p>bar</p>'
 */

wysihtml5.dom.parse = (function() {

  /**
   * It's not possible to use a XMLParser/DOMParser as HTML5 is not always well-formed XML
   * new DOMParser().parseFromString('<img src="foo.gif">') will cause a parseError since the
   * node isn't closed
   *
   * Therefore we've to use the browser's ordinary HTML parser invoked by setting innerHTML.
   */
  var NODE_TYPE_MAPPING = {
        "1": _handleElement,
        "3": _handleText,
        "8": _handleComment
      },
      // Rename unknown tags to this
      DEFAULT_NODE_NAME   = "span",
      WHITE_SPACE_REG_EXP = /\s+/,
      defaultRules        = { tags: {}, classes: {} },
      currentRules        = {},
      uneditableClass     = false;

  /**
   * Iterates over all childs of the element, recreates them, appends them into a document fragment
   * which later replaces the entire body content
   */
   function parse(elementOrHtml, config) {
    wysihtml5.lang.object(currentRules).merge(defaultRules).merge(config.rules).get();

    var context       = config.context || elementOrHtml.ownerDocument || document,
        fragment      = context.createDocumentFragment(),
        isString      = typeof(elementOrHtml) === "string",
        clearInternals = false,
        element,
        newNode,
        firstChild;

    if (config.clearInternals === true) {
      clearInternals = true;
    }

    if (config.uneditableClass) {
      uneditableClass = config.uneditableClass;
    }

    if (isString) {
      element = wysihtml5.dom.getAsDom(elementOrHtml, context);
    } else {
      element = elementOrHtml;
    }

    while (element.firstChild) {
      firstChild = element.firstChild;
      newNode = _convert(firstChild, config.cleanUp, clearInternals);
      if (newNode) {
        fragment.appendChild(newNode);
      }
      if (firstChild !== newNode) {
        element.removeChild(firstChild);
      }
    }

    // Clear element contents
    element.innerHTML = "";

    // Insert new DOM tree
    element.appendChild(fragment);

    return isString ? wysihtml5.quirks.getCorrectInnerHTML(element) : element;
  }

  function _convert(oldNode, cleanUp, clearInternals) {
    var oldNodeType     = oldNode.nodeType,
        oldChilds       = oldNode.childNodes,
        oldChildsLength = oldChilds.length,
        method          = NODE_TYPE_MAPPING[oldNodeType],
        i               = 0,
        fragment,
        newNode,
        newChild;

    // Passes directly elemets with uneditable class
    if (uneditableClass && oldNodeType === 1 && wysihtml5.dom.hasClass(oldNode, uneditableClass)) {
        return oldNode;
    }

    newNode = method && method(oldNode, clearInternals);

    // Remove or unwrap node in case of return value null or false
    if (!newNode) {
        if (newNode === false) {
            // false defines that tag should be removed but contents should remain (unwrap)
            fragment = oldNode.ownerDocument.createDocumentFragment();

            for (i = oldChildsLength; i--;) {
              if (oldChilds[i]) {
                newChild = _convert(oldChilds[i], cleanUp, clearInternals);
                if (newChild) {
                  if (oldChilds[i] === newChild) {
                    i--;
                  }
                  fragment.insertBefore(newChild, fragment.firstChild);
                }
              }
            }

            // TODO: try to minimize surplus spaces
            if (wysihtml5.lang.array([
                "div", "pre", "p",
                "table", "td", "th",
                "ul", "ol", "li",
                "dd", "dl",
                "footer", "header", "section",
                "h1", "h2", "h3", "h4", "h5", "h6"
            ]).contains(oldNode.nodeName.toLowerCase()) && oldNode.parentNode.lastChild !== oldNode) {
                // add space at first when unwraping non-textflow elements
                if (!oldNode.nextSibling || oldNode.nextSibling.nodeType !== 3 || !(/^\s/).test(oldNode.nextSibling.nodeValue)) {
                  fragment.appendChild(oldNode.ownerDocument.createTextNode(" "));
                }
            }

            if (fragment.normalize) {
              fragment.normalize();
            }
            return fragment;
        } else {
          // Remove
          return null;
        }
    }

    // Converts all childnodes
    for (i=0; i<oldChildsLength; i++) {
      if (oldChilds[i]) {
        newChild = _convert(oldChilds[i], cleanUp, clearInternals);
        if (newChild) {
          if (oldChilds[i] === newChild) {
            i--;
          }
          newNode.appendChild(newChild);
        }
      }
    }

    // Cleanup senseless <span> elements
    if (cleanUp &&
        newNode.nodeName.toLowerCase() === DEFAULT_NODE_NAME &&
        (!newNode.childNodes.length ||
         ((/^\s*$/gi).test(newNode.innerHTML) && (clearInternals || (oldNode.className !== "_wysihtml5-temp-placeholder" && oldNode.className !== "rangySelectionBoundary"))) ||
         !newNode.attributes.length)
        ) {
      fragment = newNode.ownerDocument.createDocumentFragment();
      while (newNode.firstChild) {
        fragment.appendChild(newNode.firstChild);
      }
      if (fragment.normalize) {
        fragment.normalize();
      }
      return fragment;
    }

    if (newNode.normalize) {
      newNode.normalize();
    }
    return newNode;
  }

  function _handleElement(oldNode, clearInternals) {
    var rule,
        newNode,
        tagRules    = currentRules.tags,
        nodeName    = oldNode.nodeName.toLowerCase(),
        scopeName   = oldNode.scopeName;

    /**
     * We already parsed that element
     * ignore it! (yes, this sometimes happens in IE8 when the html is invalid)
     */
    if (oldNode._wysihtml5) {
      return null;
    }
    oldNode._wysihtml5 = 1;

    if (oldNode.className === "wysihtml5-temp") {
      return null;
    }

    /**
     * IE is the only browser who doesn't include the namespace in the
     * nodeName, that's why we have to prepend it by ourselves
     * scopeName is a proprietary IE feature
     * read more here http://msdn.microsoft.com/en-us/library/ms534388(v=vs.85).aspx
     */
    if (scopeName && scopeName != "HTML") {
      nodeName = scopeName + ":" + nodeName;
    }
    /**
     * Repair node
     * IE is a bit bitchy when it comes to invalid nested markup which includes unclosed tags
     * A <p> doesn't need to be closed according HTML4-5 spec, we simply replace it with a <div> to preserve its content and layout
     */
    if ("outerHTML" in oldNode) {
      if (!wysihtml5.browser.autoClosesUnclosedTags() &&
          oldNode.nodeName === "P" &&
          oldNode.outerHTML.slice(-4).toLowerCase() !== "</p>") {
        nodeName = "div";
      }
    }

    if (nodeName in tagRules) {
      rule = tagRules[nodeName];
      if (!rule || rule.remove) {
        return null;
      } else if (rule.unwrap) {
        return false;
      }
      rule = typeof(rule) === "string" ? { rename_tag: rule } : rule;
    } else if (oldNode.firstChild) {
      rule = { rename_tag: DEFAULT_NODE_NAME };
    } else {
      // Remove empty unknown elements
      return null;
    }

    newNode = oldNode.ownerDocument.createElement(rule.rename_tag || nodeName);
    _handleAttributes(oldNode, newNode, rule, clearInternals);
    _handleStyles(oldNode, newNode, rule);
    // tests if type condition is met or node should be removed/unwrapped
    if (rule.one_of_type && !_testTypes(oldNode, currentRules, rule.one_of_type, clearInternals)) {
      return (rule.remove_action && rule.remove_action == "unwrap") ? false : null;
    }

    oldNode = null;

    if (newNode.normalize) { newNode.normalize(); }
    return newNode;
  }

  function _testTypes(oldNode, rules, types, clearInternals) {
    var definition, type;

    // do not interfere with placeholder span or pasting caret position is not maintained
    if (oldNode.nodeName === "SPAN" && !clearInternals && (oldNode.className === "_wysihtml5-temp-placeholder" || oldNode.className === "rangySelectionBoundary")) {
      return true;
    }

    for (type in types) {
      if (types.hasOwnProperty(type) && rules.type_definitions && rules.type_definitions[type]) {
        definition = rules.type_definitions[type];
        if (_testType(oldNode, definition)) {
          return true;
        }
      }
    }
    return false;
  }

  function array_contains(a, obj) {
      var i = a.length;
      while (i--) {
         if (a[i] === obj) {
             return true;
         }
      }
      return false;
  }

  function _testType(oldNode, definition) {

    var nodeClasses = oldNode.getAttribute("class"),
        nodeStyles =  oldNode.getAttribute("style"),
        classesLength, s, s_corrected, a, attr, currentClass, styleProp;

    // test for methods
    if (definition.methods) {
      for (var m in definition.methods) {
        if (definition.methods.hasOwnProperty(m) && typeCeckMethods[m]) {

          if (typeCeckMethods[m](oldNode)) {
            return true;
          }
        }
      }
    }

    // test for classes, if one found return true
    if (nodeClasses && definition.classes) {
      nodeClasses = nodeClasses.replace(/^\s+/g, '').replace(/\s+$/g, '').split(WHITE_SPACE_REG_EXP);
      classesLength = nodeClasses.length;
      for (var i = 0; i < classesLength; i++) {
        if (definition.classes[nodeClasses[i]]) {
          return true;
        }
      }
    }

    // test for styles, if one found return true
    if (nodeStyles && definition.styles) {

      nodeStyles = nodeStyles.split(';');
      for (s in definition.styles) {
        if (definition.styles.hasOwnProperty(s)) {
          for (var sp = nodeStyles.length; sp--;) {
            styleProp = nodeStyles[sp].split(':');

            if (styleProp[0].replace(/\s/g, '').toLowerCase() === s) {
              if (definition.styles[s] === true || definition.styles[s] === 1 || wysihtml5.lang.array(definition.styles[s]).contains(styleProp[1].replace(/\s/g, '').toLowerCase()) ) {
                return true;
              }
            }
          }
        }
      }
    }

    // test for attributes in general against regex match
    if (definition.attrs) {
        for (a in definition.attrs) {
            if (definition.attrs.hasOwnProperty(a)) {
                attr = _getAttribute(oldNode, a);
                if (typeof(attr) === "string") {
                    if (attr.search(definition.attrs[a]) > -1) {
                        return true;
                    }
                }
            }
        }
    }
    return false;
  }

  function _handleStyles(oldNode, newNode, rule) {
    var s;
    if(rule && rule.keep_styles) {
      for (s in rule.keep_styles) {
        if (rule.keep_styles.hasOwnProperty(s)) {
          if (s == "float") {
            // IE compability
            if (oldNode.style.styleFloat) {
              newNode.style.styleFloat = oldNode.style.styleFloat;
            }
            if (oldNode.style.cssFloat) {
              newNode.style.cssFloat = oldNode.style.cssFloat;
            }
           } else if (oldNode.style[s]) {
             newNode.style[s] = oldNode.style[s];
           }
        }
      }
    }
  }

  // TODO: refactor. Too long to read
  function _handleAttributes(oldNode, newNode, rule, clearInternals) {
    var attributes          = {},                         // fresh new set of attributes to set on newNode
        setClass            = rule.set_class,             // classes to set
        addClass            = rule.add_class,             // add classes based on existing attributes
        addStyle            = rule.add_style,             // add styles based on existing attributes
        setAttributes       = rule.set_attributes,        // attributes to set on the current node
        checkAttributes     = rule.check_attributes,      // check/convert values of attributes
        allowedClasses      = currentRules.classes,
        i                   = 0,
        classes             = [],
        styles              = [],
        newClasses          = [],
        oldClasses          = [],
        classesLength,
        newClassesLength,
        currentClass,
        newClass,
        attributeName,
        newAttributeValue,
        method,
        oldAttribute;

    if (setAttributes) {
      attributes = wysihtml5.lang.object(setAttributes).clone();
    }

    if (checkAttributes) {
      for (attributeName in checkAttributes) {
        method = attributeCheckMethods[checkAttributes[attributeName]];
        if (!method) {
          continue;
        }
        oldAttribute = _getAttribute(oldNode, attributeName);
        if (oldAttribute || (attributeName === "alt" && oldNode.nodeName == "IMG")) {
          newAttributeValue = method(oldAttribute);
          if (typeof(newAttributeValue) === "string") {
            attributes[attributeName] = newAttributeValue;
          }
        }
      }
    }

    if (setClass) {
      classes.push(setClass);
    }

    if (addClass) {
      for (attributeName in addClass) {
        method = addClassMethods[addClass[attributeName]];
        if (!method) {
          continue;
        }
        newClass = method(_getAttribute(oldNode, attributeName));
        if (typeof(newClass) === "string") {
          classes.push(newClass);
        }
      }
    }

    if (addStyle) {
      for (attributeName in addStyle) {
        method = addStyleMethods[addStyle[attributeName]];
        if (!method) {
          continue;
        }

        newStyle = method(_getAttribute(oldNode, attributeName));
        if (typeof(newStyle) === "string") {
          styles.push(newStyle);
        }
      }
    }


    if (typeof(allowedClasses) === "string" && allowedClasses === "any" && oldNode.getAttribute("class")) {
      attributes["class"] = oldNode.getAttribute("class");
    } else {
      // make sure that wysihtml5 temp class doesn't get stripped out
      if (!clearInternals) {
        allowedClasses["_wysihtml5-temp-placeholder"] = 1;
        allowedClasses["_rangySelectionBoundary"] = 1;
        allowedClasses["wysiwyg-tmp-selected-cell"] = 1;
      }

      // add old classes last
      oldClasses = oldNode.getAttribute("class");
      if (oldClasses) {
        classes = classes.concat(oldClasses.split(WHITE_SPACE_REG_EXP));
      }
      classesLength = classes.length;
      for (; i<classesLength; i++) {
        currentClass = classes[i];
        if (allowedClasses[currentClass]) {
          newClasses.push(currentClass);
        }
      }

      if (newClasses.length) {
        attributes["class"] = wysihtml5.lang.array(newClasses).unique().join(" ");
      }
    }

    // remove table selection class if present
    if (attributes["class"] && clearInternals) {
      attributes["class"] = attributes["class"].replace("wysiwyg-tmp-selected-cell", "");
      if ((/^\s*$/g).test(attributes["class"])) {
        delete attributes["class"];
      }
    }

    if (styles.length) {
      attributes["style"] = wysihtml5.lang.array(styles).unique().join(" ");
    }

    // set attributes on newNode
    for (attributeName in attributes) {
      // Setting attributes can cause a js error in IE under certain circumstances
      // eg. on a <img> under https when it's new attribute value is non-https
      // TODO: Investigate this further and check for smarter handling
      try {
        newNode.setAttribute(attributeName, attributes[attributeName]);
      } catch(e) {}
    }

    // IE8 sometimes loses the width/height attributes when those are set before the "src"
    // so we make sure to set them again
    if (attributes.src) {
      if (typeof(attributes.width) !== "undefined") {
        newNode.setAttribute("width", attributes.width);
      }
      if (typeof(attributes.height) !== "undefined") {
        newNode.setAttribute("height", attributes.height);
      }
    }
  }

  /**
   * IE gives wrong results for hasAttribute/getAttribute, for example:
   *    var td = document.createElement("td");
   *    td.getAttribute("rowspan"); // => "1" in IE
   *
   * Therefore we have to check the element's outerHTML for the attribute
   */
  var HAS_GET_ATTRIBUTE_BUG = !wysihtml5.browser.supportsGetAttributeCorrectly();
  function _getAttribute(node, attributeName) {
    attributeName = attributeName.toLowerCase();
    var nodeName = node.nodeName;
    if (nodeName == "IMG" && attributeName == "src" && _isLoadedImage(node) === true) {
      // Get 'src' attribute value via object property since this will always contain the
      // full absolute url (http://...)
      // this fixes a very annoying bug in firefox (ver 3.6 & 4) and IE 8 where images copied from the same host
      // will have relative paths, which the sanitizer strips out (see attributeCheckMethods.url)
      return node.src;
    } else if (HAS_GET_ATTRIBUTE_BUG && "outerHTML" in node) {
      // Don't trust getAttribute/hasAttribute in IE 6-8, instead check the element's outerHTML
      var outerHTML      = node.outerHTML.toLowerCase(),
          // TODO: This might not work for attributes without value: <input disabled>
          hasAttribute   = outerHTML.indexOf(" " + attributeName +  "=") != -1;

      return hasAttribute ? node.getAttribute(attributeName) : null;
    } else{
      return node.getAttribute(attributeName);
    }
  }

  /**
   * Check whether the given node is a proper loaded image
   * FIXME: Returns undefined when unknown (Chrome, Safari)
   */
  function _isLoadedImage(node) {
    try {
      return node.complete && !node.mozMatchesSelector(":-moz-broken");
    } catch(e) {
      if (node.complete && node.readyState === "complete") {
        return true;
      }
    }
  }

  var INVISIBLE_SPACE_REG_EXP = /\uFEFF/g;
  function _handleText(oldNode) {
    var nextSibling = oldNode.nextSibling;
    if (nextSibling && nextSibling.nodeType === wysihtml5.TEXT_NODE) {
      // Concatenate text nodes
      nextSibling.data = oldNode.data.replace(INVISIBLE_SPACE_REG_EXP, "") + nextSibling.data.replace(INVISIBLE_SPACE_REG_EXP, "");
    } else {
      // \uFEFF = wysihtml5.INVISIBLE_SPACE (used as a hack in certain rich text editing situations)
      var data = oldNode.data.replace(INVISIBLE_SPACE_REG_EXP, "");
      return oldNode.ownerDocument.createTextNode(data);
    }
  }

  function _handleComment(oldNode) {
    if (currentRules.comments) {
      return oldNode.ownerDocument.createComment(oldNode.nodeValue);
    }
  }

  // ------------ attribute checks ------------ \\
  var attributeCheckMethods = {
    url: (function() {
      var REG_EXP = /^https?:\/\//i;
      return function(attributeValue) {
        if (!attributeValue || !attributeValue.match(REG_EXP)) {
          return null;
        }
        return attributeValue.replace(REG_EXP, function(match) {
          return match.toLowerCase();
        });
      };
    })(),

    src: (function() {
      var REG_EXP = /^(\/|https?:\/\/)/i;
      return function(attributeValue) {
        if (!attributeValue || !attributeValue.match(REG_EXP)) {
          return null;
        }
        return attributeValue.replace(REG_EXP, function(match) {
          return match.toLowerCase();
        });
      };
    })(),

    href: (function() {
      var REG_EXP = /^(#|\/|https?:\/\/|mailto:)/i;
      return function(attributeValue) {
        if (!attributeValue || !attributeValue.match(REG_EXP)) {
          return null;
        }
        return attributeValue.replace(REG_EXP, function(match) {
          return match.toLowerCase();
        });
      };
    })(),

    alt: (function() {
      var REG_EXP = /[^ a-z0-9_\-]/gi;
      return function(attributeValue) {
        if (!attributeValue) {
          return "";
        }
        return attributeValue.replace(REG_EXP, "");
      };
    })(),

    numbers: (function() {
      var REG_EXP = /\D/g;
      return function(attributeValue) {
        attributeValue = (attributeValue || "").replace(REG_EXP, "");
        return attributeValue || null;
      };
    })(),

    any: (function() {
      return function(attributeValue) {
        return attributeValue;
      };
    })()
  };

  // ------------ style converter (converts an html attribute to a style) ------------ \\
  var addStyleMethods = {
    align_text: (function() {
      var mapping = {
        left:     "text-align: left;",
        right:    "text-align: right;",
        center:   "text-align: center;"
      };
      return function(attributeValue) {
        return mapping[String(attributeValue).toLowerCase()];
      };
    })(),
  };

  // ------------ class converter (converts an html attribute to a class name) ------------ \\
  var addClassMethods = {
    align_img: (function() {
      var mapping = {
        left:   "wysiwyg-float-left",
        right:  "wysiwyg-float-right"
      };
      return function(attributeValue) {
        return mapping[String(attributeValue).toLowerCase()];
      };
    })(),

    align_text: (function() {
      var mapping = {
        left:     "wysiwyg-text-align-left",
        right:    "wysiwyg-text-align-right",
        center:   "wysiwyg-text-align-center",
        justify:  "wysiwyg-text-align-justify"
      };
      return function(attributeValue) {
        return mapping[String(attributeValue).toLowerCase()];
      };
    })(),

    clear_br: (function() {
      var mapping = {
        left:   "wysiwyg-clear-left",
        right:  "wysiwyg-clear-right",
        both:   "wysiwyg-clear-both",
        all:    "wysiwyg-clear-both"
      };
      return function(attributeValue) {
        return mapping[String(attributeValue).toLowerCase()];
      };
    })(),

    size_font: (function() {
      var mapping = {
        "1": "wysiwyg-font-size-xx-small",
        "2": "wysiwyg-font-size-small",
        "3": "wysiwyg-font-size-medium",
        "4": "wysiwyg-font-size-large",
        "5": "wysiwyg-font-size-x-large",
        "6": "wysiwyg-font-size-xx-large",
        "7": "wysiwyg-font-size-xx-large",
        "-": "wysiwyg-font-size-smaller",
        "+": "wysiwyg-font-size-larger"
      };
      return function(attributeValue) {
        return mapping[String(attributeValue).charAt(0)];
      };
    })()
  };

  // checks if element is possibly visible
  var typeCeckMethods = {
    has_visible_contet: (function() {
      var txt,
          isVisible = false,
          visibleElements = ['img', 'video', 'picture', 'br', 'script', 'noscript',
                             'style', 'table', 'iframe', 'object', 'embed', 'audio',
                             'svg', 'input', 'button', 'select','textarea', 'canvas'];

      return function(el) {

        // has visible innertext. so is visible
        txt = (el.innerText || el.textContent).replace(/\s/g, '');
        if (txt && txt.length > 0) {
          return true;
        }

        // matches list of visible dimensioned elements
        for (var i = visibleElements.length; i--;) {
          if (el.querySelector(visibleElements[i])) {
            return true;
          }
        }

        // try to measure dimesions in last resort. (can find only of elements in dom)
        if (el.offsetWidth && el.offsetWidth > 0 && el.offsetHeight && el.offsetHeight > 0) {
          return true;
        }

        return false;
      };
    })()
  };

  return parse;
})();
;/**
 * Checks for empty text node childs and removes them
 *
 * @param {Element} node The element in which to cleanup
 * @example
 *    wysihtml5.dom.removeEmptyTextNodes(element);
 */
wysihtml5.dom.removeEmptyTextNodes = function(node) {
  var childNode,
      childNodes        = wysihtml5.lang.array(node.childNodes).get(),
      childNodesLength  = childNodes.length,
      i                 = 0;
  for (; i<childNodesLength; i++) {
    childNode = childNodes[i];
    if (childNode.nodeType === wysihtml5.TEXT_NODE && childNode.data === "") {
      childNode.parentNode.removeChild(childNode);
    }
  }
};
;/**
 * Renames an element (eg. a <div> to a <p>) and keeps its childs
 *
 * @param {Element} element The list element which should be renamed
 * @param {Element} newNodeName The desired tag name
 *
 * @example
 *    <!-- Assume the following dom: -->
 *    <ul id="list">
 *      <li>eminem</li>
 *      <li>dr. dre</li>
 *      <li>50 Cent</li>
 *    </ul>
 *
 *    <script>
 *      wysihtml5.dom.renameElement(document.getElementById("list"), "ol");
 *    </script>
 *
 *    <!-- Will result in: -->
 *    <ol>
 *      <li>eminem</li>
 *      <li>dr. dre</li>
 *      <li>50 Cent</li>
 *    </ol>
 */
wysihtml5.dom.renameElement = function(element, newNodeName) {
  var newElement = element.ownerDocument.createElement(newNodeName),
      firstChild;
  while (firstChild = element.firstChild) {
    newElement.appendChild(firstChild);
  }
  wysihtml5.dom.copyAttributes(["align", "className"]).from(element).to(newElement);
  element.parentNode.replaceChild(newElement, element);
  return newElement;
};
;/**
 * Takes an element, removes it and replaces it with it's childs
 *
 * @param {Object} node The node which to replace with it's child nodes
 * @example
 *    <div id="foo">
 *      <span>hello</span>
 *    </div>
 *    <script>
 *      // Remove #foo and replace with it's children
 *      wysihtml5.dom.replaceWithChildNodes(document.getElementById("foo"));
 *    </script>
 */
wysihtml5.dom.replaceWithChildNodes = function(node) {
  if (!node.parentNode) {
    return;
  }

  if (!node.firstChild) {
    node.parentNode.removeChild(node);
    return;
  }

  var fragment = node.ownerDocument.createDocumentFragment();
  while (node.firstChild) {
    fragment.appendChild(node.firstChild);
  }
  node.parentNode.replaceChild(fragment, node);
  node = fragment = null;
};
;/**
 * Unwraps an unordered/ordered list
 *
 * @param {Element} element The list element which should be unwrapped
 *
 * @example
 *    <!-- Assume the following dom: -->
 *    <ul id="list">
 *      <li>eminem</li>
 *      <li>dr. dre</li>
 *      <li>50 Cent</li>
 *    </ul>
 *
 *    <script>
 *      wysihtml5.dom.resolveList(document.getElementById("list"));
 *    </script>
 *
 *    <!-- Will result in: -->
 *    eminem<br>
 *    dr. dre<br>
 *    50 Cent<br>
 */
(function(dom) {
  function _isBlockElement(node) {
    return dom.getStyle("display").from(node) === "block";
  }

  function _isLineBreak(node) {
    return node.nodeName === "BR";
  }

  function _appendLineBreak(element) {
    var lineBreak = element.ownerDocument.createElement("br");
    element.appendChild(lineBreak);
  }

  function resolveList(list, useLineBreaks) {
    if (!list.nodeName.match(/^(MENU|UL|OL)$/)) {
      return;
    }

    var doc             = list.ownerDocument,
        fragment        = doc.createDocumentFragment(),
        previousSibling = wysihtml5.dom.domNode(list).prev({ignoreBlankTexts: true}),
        firstChild,
        lastChild,
        isLastChild,
        shouldAppendLineBreak,
        paragraph,
        listItem;

    if (useLineBreaks) {
      // Insert line break if list is after a non-block element
      if (previousSibling && !_isBlockElement(previousSibling) && !_isLineBreak(previousSibling)) {
        _appendLineBreak(fragment);
      }

      while (listItem = (list.firstElementChild || list.firstChild)) {
        lastChild = listItem.lastChild;
        while (firstChild = listItem.firstChild) {
          isLastChild           = firstChild === lastChild;
          // This needs to be done before appending it to the fragment, as it otherwise will lose style information
          shouldAppendLineBreak = isLastChild && !_isBlockElement(firstChild) && !_isLineBreak(firstChild);
          fragment.appendChild(firstChild);
          if (shouldAppendLineBreak) {
            _appendLineBreak(fragment);
          }
        }

        listItem.parentNode.removeChild(listItem);
      }
    } else {
      while (listItem = (list.firstElementChild || list.firstChild)) {
        if (listItem.querySelector && listItem.querySelector("div, p, ul, ol, menu, blockquote, h1, h2, h3, h4, h5, h6")) {
          while (firstChild = listItem.firstChild) {
            fragment.appendChild(firstChild);
          }
        } else {
          paragraph = doc.createElement("p");
          while (firstChild = listItem.firstChild) {
            paragraph.appendChild(firstChild);
          }
          fragment.appendChild(paragraph);
        }
        listItem.parentNode.removeChild(listItem);
      }
    }

    list.parentNode.replaceChild(fragment, list);
  }

  dom.resolveList = resolveList;
})(wysihtml5.dom);
;/**
 * Sandbox for executing javascript, parsing css styles and doing dom operations in a secure way
 *
 * Browser Compatibility:
 *  - Secure in MSIE 6+, but only when the user hasn't made changes to his security level "restricted"
 *  - Partially secure in other browsers (Firefox, Opera, Safari, Chrome, ...)
 *
 * Please note that this class can't benefit from the HTML5 sandbox attribute for the following reasons:
 *    - sandboxing doesn't work correctly with inlined content (src="javascript:'<html>...</html>'")
 *    - sandboxing of physical documents causes that the dom isn't accessible anymore from the outside (iframe.contentWindow, ...)
 *    - setting the "allow-same-origin" flag would fix that, but then still javascript and dom events refuse to fire
 *    - therefore the "allow-scripts" flag is needed, which then would deactivate any security, as the js executed inside the iframe
 *      can do anything as if the sandbox attribute wasn't set
 *
 * @param {Function} [readyCallback] Method that gets invoked when the sandbox is ready
 * @param {Object} [config] Optional parameters
 *
 * @example
 *    new wysihtml5.dom.Sandbox(function(sandbox) {
 *      sandbox.getWindow().document.body.innerHTML = '<img src=foo.gif onerror="alert(document.cookie)">';
 *    });
 */
(function(wysihtml5) {
  var /**
       * Default configuration
       */
      doc                 = document,
      /**
       * Properties to unset/protect on the window object
       */
      windowProperties    = [
        "parent", "top", "opener", "frameElement", "frames",
        "localStorage", "globalStorage", "sessionStorage", "indexedDB"
      ],
      /**
       * Properties on the window object which are set to an empty function
       */
      windowProperties2   = [
        "open", "close", "openDialog", "showModalDialog",
        "alert", "confirm", "prompt",
        "openDatabase", "postMessage",
        "XMLHttpRequest", "XDomainRequest"
      ],
      /**
       * Properties to unset/protect on the document object
       */
      documentProperties  = [
        "referrer",
        "write", "open", "close"
      ];

  wysihtml5.dom.Sandbox = Base.extend(
    /** @scope wysihtml5.dom.Sandbox.prototype */ {

    constructor: function(readyCallback, config) {
      this.callback = readyCallback || wysihtml5.EMPTY_FUNCTION;
      this.config   = wysihtml5.lang.object({}).merge(config).get();
      this.editableArea   = this._createIframe();
    },

    insertInto: function(element) {
      if (typeof(element) === "string") {
        element = doc.getElementById(element);
      }

      element.appendChild(this.editableArea);
    },

    getIframe: function() {
      return this.editableArea;
    },

    getWindow: function() {
      this._readyError();
    },

    getDocument: function() {
      this._readyError();
    },

    destroy: function() {
      var iframe = this.getIframe();
      iframe.parentNode.removeChild(iframe);
    },

    _readyError: function() {
      throw new Error("wysihtml5.Sandbox: Sandbox iframe isn't loaded yet");
    },

    /**
     * Creates the sandbox iframe
     *
     * Some important notes:
     *  - We can't use HTML5 sandbox for now:
     *    setting it causes that the iframe's dom can't be accessed from the outside
     *    Therefore we need to set the "allow-same-origin" flag which enables accessing the iframe's dom
     *    But then there's another problem, DOM events (focus, blur, change, keypress, ...) aren't fired.
     *    In order to make this happen we need to set the "allow-scripts" flag.
     *    A combination of allow-scripts and allow-same-origin is almost the same as setting no sandbox attribute at all.
     *  - Chrome & Safari, doesn't seem to support sandboxing correctly when the iframe's html is inlined (no physical document)
     *  - IE needs to have the security="restricted" attribute set before the iframe is
     *    inserted into the dom tree
     *  - Believe it or not but in IE "security" in document.createElement("iframe") is false, even
     *    though it supports it
     *  - When an iframe has security="restricted", in IE eval() & execScript() don't work anymore
     *  - IE doesn't fire the onload event when the content is inlined in the src attribute, therefore we rely
     *    on the onreadystatechange event
     */
    _createIframe: function() {
      var that   = this,
          iframe = doc.createElement("iframe");
      iframe.className = "wysihtml5-sandbox";
      wysihtml5.dom.setAttributes({
        "security":           "restricted",
        "allowtransparency":  "true",
        "frameborder":        0,
        "width":              0,
        "height":             0,
        "marginwidth":        0,
        "marginheight":       0
      }).on(iframe);

      // Setting the src like this prevents ssl warnings in IE6
      if (wysihtml5.browser.throwsMixedContentWarningWhenIframeSrcIsEmpty()) {
        iframe.src = "javascript:'<html></html>'";
      }

      iframe.onload = function() {
        iframe.onreadystatechange = iframe.onload = null;
        that._onLoadIframe(iframe);
      };

      iframe.onreadystatechange = function() {
        if (/loaded|complete/.test(iframe.readyState)) {
          iframe.onreadystatechange = iframe.onload = null;
          that._onLoadIframe(iframe);
        }
      };

      return iframe;
    },

    /**
     * Callback for when the iframe has finished loading
     */
    _onLoadIframe: function(iframe) {
      // don't resume when the iframe got unloaded (eg. by removing it from the dom)
      if (!wysihtml5.dom.contains(doc.documentElement, iframe)) {
        return;
      }

      var that           = this,
          iframeWindow   = iframe.contentWindow,
          iframeDocument = iframe.contentWindow.document,
          charset        = doc.characterSet || doc.charset || "utf-8",
          sandboxHtml    = this._getHtml({
            charset:      charset,
            stylesheets:  this.config.stylesheets
          });

      // Create the basic dom tree including proper DOCTYPE and charset
      iframeDocument.open("text/html", "replace");
      iframeDocument.write(sandboxHtml);
      iframeDocument.close();

      this.getWindow = function() { return iframe.contentWindow; };
      this.getDocument = function() { return iframe.contentWindow.document; };

      // Catch js errors and pass them to the parent's onerror event
      // addEventListener("error") doesn't work properly in some browsers
      // TODO: apparently this doesn't work in IE9!
      iframeWindow.onerror = function(errorMessage, fileName, lineNumber) {
        throw new Error("wysihtml5.Sandbox: " + errorMessage, fileName, lineNumber);
      };

      if (!wysihtml5.browser.supportsSandboxedIframes()) {
        // Unset a bunch of sensitive variables
        // Please note: This isn't hack safe!
        // It more or less just takes care of basic attacks and prevents accidental theft of sensitive information
        // IE is secure though, which is the most important thing, since IE is the only browser, who
        // takes over scripts & styles into contentEditable elements when copied from external websites
        // or applications (Microsoft Word, ...)
        var i, length;
        for (i=0, length=windowProperties.length; i<length; i++) {
          this._unset(iframeWindow, windowProperties[i]);
        }
        for (i=0, length=windowProperties2.length; i<length; i++) {
          this._unset(iframeWindow, windowProperties2[i], wysihtml5.EMPTY_FUNCTION);
        }
        for (i=0, length=documentProperties.length; i<length; i++) {
          this._unset(iframeDocument, documentProperties[i]);
        }
        // This doesn't work in Safari 5
        // See http://stackoverflow.com/questions/992461/is-it-possible-to-override-document-cookie-in-webkit
        this._unset(iframeDocument, "cookie", "", true);
      }

      this.loaded = true;

      // Trigger the callback
      setTimeout(function() { that.callback(that); }, 0);
    },

    _getHtml: function(templateVars) {
      var stylesheets = templateVars.stylesheets,
          html        = "",
          i           = 0,
          length;
      stylesheets = typeof(stylesheets) === "string" ? [stylesheets] : stylesheets;
      if (stylesheets) {
        length = stylesheets.length;
        for (; i<length; i++) {
          html += '<link rel="stylesheet" href="' + stylesheets[i] + '">';
        }
      }
      templateVars.stylesheets = html;

      return wysihtml5.lang.string(
        '<!DOCTYPE html><html><head>'
        + '<meta charset="#{charset}">#{stylesheets}</head>'
        + '<body></body></html>'
      ).interpolate(templateVars);
    },

    /**
     * Method to unset/override existing variables
     * @example
     *    // Make cookie unreadable and unwritable
     *    this._unset(document, "cookie", "", true);
     */
    _unset: function(object, property, value, setter) {
      try { object[property] = value; } catch(e) {}

      try { object.__defineGetter__(property, function() { return value; }); } catch(e) {}
      if (setter) {
        try { object.__defineSetter__(property, function() {}); } catch(e) {}
      }

      if (!wysihtml5.browser.crashesWhenDefineProperty(property)) {
        try {
          var config = {
            get: function() { return value; }
          };
          if (setter) {
            config.set = function() {};
          }
          Object.defineProperty(object, property, config);
        } catch(e) {}
      }
    }
  });
})(wysihtml5);
;(function(wysihtml5) {
  var doc = document;
  wysihtml5.dom.ContentEditableArea = Base.extend({
      getContentEditable: function() {
        return this.element;
      },

      getWindow: function() {
        return this.element.ownerDocument.defaultView;
      },

      getDocument: function() {
        return this.element.ownerDocument;
      },

      constructor: function(readyCallback, config, contentEditable) {
        this.callback = readyCallback || wysihtml5.EMPTY_FUNCTION;
        this.config   = wysihtml5.lang.object({}).merge(config).get();
        if (contentEditable) {
            this.element = this._bindElement(contentEditable);
        } else {
            this.element = this._createElement();
        }
      },

      // creates a new contenteditable and initiates it
      _createElement: function() {
        var element = doc.createElement("div");
        element.className = "wysihtml5-sandbox";
        this._loadElement(element);
        return element;
      },

      // initiates an allready existent contenteditable
      _bindElement: function(contentEditable) {
        contentEditable.className = (contentEditable.className && contentEditable.className != '') ? contentEditable.className + " wysihtml5-sandbox" : "wysihtml5-sandbox";
        this._loadElement(contentEditable, true);
        return contentEditable;
      },

      _loadElement: function(element, contentExists) {
          var that = this;
        if (!contentExists) {
            var sandboxHtml = this._getHtml();
            element.innerHTML = sandboxHtml;
        }

        this.getWindow = function() { return element.ownerDocument.defaultView; };
        this.getDocument = function() { return element.ownerDocument; };

        // Catch js errors and pass them to the parent's onerror event
        // addEventListener("error") doesn't work properly in some browsers
        // TODO: apparently this doesn't work in IE9!
        // TODO: figure out and bind the errors logic for contenteditble mode
        /*iframeWindow.onerror = function(errorMessage, fileName, lineNumber) {
          throw new Error("wysihtml5.Sandbox: " + errorMessage, fileName, lineNumber);
        }
        */
        this.loaded = true;
        // Trigger the callback
        setTimeout(function() { that.callback(that); }, 0);
      },

      _getHtml: function(templateVars) {
        return '';
      }

  });
})(wysihtml5);
;(function() {
  var mapping = {
    "className": "class"
  };
  wysihtml5.dom.setAttributes = function(attributes) {
    return {
      on: function(element) {
        for (var i in attributes) {
          element.setAttribute(mapping[i] || i, attributes[i]);
        }
      }
    };
  };
})();
;wysihtml5.dom.setStyles = function(styles) {
  return {
    on: function(element) {
      var style = element.style;
      if (typeof(styles) === "string") {
        style.cssText += ";" + styles;
        return;
      }
      for (var i in styles) {
        if (i === "float") {
          style.cssFloat = styles[i];
          style.styleFloat = styles[i];
        } else {
          style[i] = styles[i];
        }
      }
    }
  };
};
;/**
 * Simulate HTML5 placeholder attribute
 *
 * Needed since
 *    - div[contentEditable] elements don't support it
 *    - older browsers (such as IE8 and Firefox 3.6) don't support it at all
 *
 * @param {Object} parent Instance of main wysihtml5.Editor class
 * @param {Element} view Instance of wysihtml5.views.* class
 * @param {String} placeholderText
 *
 * @example
 *    wysihtml.dom.simulatePlaceholder(this, composer, "Foobar");
 */
(function(dom) {
  dom.simulatePlaceholder = function(editor, view, placeholderText) {
    var CLASS_NAME = "placeholder",
        unset = function() {
          var composerIsVisible   = view.element.offsetWidth > 0 && view.element.offsetHeight > 0;
          if (view.hasPlaceholderSet()) {
            view.clear();
            view.element.focus();
            if (composerIsVisible ) {
              setTimeout(function() {
                var sel = view.selection.getSelection();
                if (!sel.focusNode || !sel.anchorNode) {
                  view.selection.selectNode(view.element.firstChild || view.element);
                }
              }, 0);
            }
          }
          view.placeholderSet = false;
          dom.removeClass(view.element, CLASS_NAME);
        },
        set = function() {
          if (view.isEmpty()) {
            view.placeholderSet = true;
            view.setValue(placeholderText);
            dom.addClass(view.element, CLASS_NAME);
          }
        };

    editor
      .on("set_placeholder", set)
      .on("unset_placeholder", unset)
      .on("focus:composer", unset)
      .on("paste:composer", unset)
      .on("blur:composer", set);

    set();
  };
})(wysihtml5.dom);
;(function(dom) {
  var documentElement = document.documentElement;
  if ("textContent" in documentElement) {
    dom.setTextContent = function(element, text) {
      element.textContent = text;
    };

    dom.getTextContent = function(element) {
      return element.textContent;
    };
  } else if ("innerText" in documentElement) {
    dom.setTextContent = function(element, text) {
      element.innerText = text;
    };

    dom.getTextContent = function(element) {
      return element.innerText;
    };
  } else {
    dom.setTextContent = function(element, text) {
      element.nodeValue = text;
    };

    dom.getTextContent = function(element) {
      return element.nodeValue;
    };
  }
})(wysihtml5.dom);

;/**
 * Get a set of attribute from one element
 *
 * IE gives wrong results for hasAttribute/getAttribute, for example:
 *    var td = document.createElement("td");
 *    td.getAttribute("rowspan"); // => "1" in IE
 *
 * Therefore we have to check the element's outerHTML for the attribute
*/

wysihtml5.dom.getAttribute = function(node, attributeName) {
  var HAS_GET_ATTRIBUTE_BUG = !wysihtml5.browser.supportsGetAttributeCorrectly();
  attributeName = attributeName.toLowerCase();
  var nodeName = node.nodeName;
  if (nodeName == "IMG" && attributeName == "src" && _isLoadedImage(node) === true) {
    // Get 'src' attribute value via object property since this will always contain the
    // full absolute url (http://...)
    // this fixes a very annoying bug in firefox (ver 3.6 & 4) and IE 8 where images copied from the same host
    // will have relative paths, which the sanitizer strips out (see attributeCheckMethods.url)
    return node.src;
  } else if (HAS_GET_ATTRIBUTE_BUG && "outerHTML" in node) {
    // Don't trust getAttribute/hasAttribute in IE 6-8, instead check the element's outerHTML
    var outerHTML      = node.outerHTML.toLowerCase(),
        // TODO: This might not work for attributes without value: <input disabled>
        hasAttribute   = outerHTML.indexOf(" " + attributeName +  "=") != -1;

    return hasAttribute ? node.getAttribute(attributeName) : null;
  } else{
    return node.getAttribute(attributeName);
  }
};
;(function(wysihtml5) {

    var api = wysihtml5.dom;

    var MapCell = function(cell) {
      this.el = cell;
      this.isColspan= false;
      this.isRowspan= false;
      this.firstCol= true;
      this.lastCol= true;
      this.firstRow= true;
      this.lastRow= true;
      this.isReal= true;
      this.spanCollection= [];
      this.modified = false;
    };

    var TableModifyerByCell = function (cell, table) {
        if (cell) {
            this.cell = cell;
            this.table = api.getParentElement(cell, { nodeName: ["TABLE"] });
        } else if (table) {
            this.table = table;
            this.cell = this.table.querySelectorAll('th, td')[0];
        }
    };

    function queryInList(list, query) {
        var ret = [],
            q;
        for (var e = 0, len = list.length; e < len; e++) {
            q = list[e].querySelectorAll(query);
            if (q) {
                for(var i = q.length; i--; ret.unshift(q[i]));
            }
        }
        return ret;
    }

    function removeElement(el) {
        el.parentNode.removeChild(el);
    }

    function insertAfter(referenceNode, newNode) {
        referenceNode.parentNode.insertBefore(newNode, referenceNode.nextSibling);
    }

    function nextNode(node, tag) {
        var element = node.nextSibling;
        while (element.nodeType !=1) {
            element = element.nextSibling;
            if (!tag || tag == element.tagName.toLowerCase()) {
                return element;
            }
        }
        return null;
    }

    TableModifyerByCell.prototype = {

        addSpannedCellToMap: function(cell, map, r, c, cspan, rspan) {
            var spanCollect = [],
                rmax = r + ((rspan) ? parseInt(rspan, 10) - 1 : 0),
                cmax = c + ((cspan) ? parseInt(cspan, 10) - 1 : 0);

            for (var rr = r; rr <= rmax; rr++) {
                if (typeof map[rr] == "undefined") { map[rr] = []; }
                for (var cc = c; cc <= cmax; cc++) {
                    map[rr][cc] = new MapCell(cell);
                    map[rr][cc].isColspan = (cspan && parseInt(cspan, 10) > 1);
                    map[rr][cc].isRowspan = (rspan && parseInt(rspan, 10) > 1);
                    map[rr][cc].firstCol = cc == c;
                    map[rr][cc].lastCol = cc == cmax;
                    map[rr][cc].firstRow = rr == r;
                    map[rr][cc].lastRow = rr == rmax;
                    map[rr][cc].isReal = cc == c && rr == r;
                    map[rr][cc].spanCollection = spanCollect;

                    spanCollect.push(map[rr][cc]);
                }
            }
        },

        setCellAsModified: function(cell) {
            cell.modified = true;
            if (cell.spanCollection.length > 0) {
              for (var s = 0, smax = cell.spanCollection.length; s < smax; s++) {
                cell.spanCollection[s].modified = true;
              }
            }
        },

        setTableMap: function() {
            var map = [];
            var tableRows = this.getTableRows(),
                ridx, row, cells, cidx, cell,
                c,
                cspan, rspan;

            for (ridx = 0; ridx < tableRows.length; ridx++) {
                row = tableRows[ridx];
                cells = this.getRowCells(row);
                c = 0;
                if (typeof map[ridx] == "undefined") { map[ridx] = []; }
                for (cidx = 0; cidx < cells.length; cidx++) {
                    cell = cells[cidx];

                    // If cell allready set means it is set by col or rowspan,
                    // so increase cols index until free col is found
                    while (typeof map[ridx][c] != "undefined") { c++; }

                    cspan = api.getAttribute(cell, 'colspan');
                    rspan = api.getAttribute(cell, 'rowspan');

                    if (cspan || rspan) {
                        this.addSpannedCellToMap(cell, map, ridx, c, cspan, rspan);
                        c = c + ((cspan) ? parseInt(cspan, 10) : 1);
                    } else {
                        map[ridx][c] = new MapCell(cell);
                        c++;
                    }
                }
            }
            this.map = map;
            return map;
        },

        getRowCells: function(row) {
            var inlineTables = this.table.querySelectorAll('table'),
                inlineCells = (inlineTables) ? queryInList(inlineTables, 'th, td') : [],
                allCells = row.querySelectorAll('th, td'),
                tableCells = (inlineCells.length > 0) ? wysihtml5.lang.array(allCells).without(inlineCells) : allCells;

            return tableCells;
        },

        getTableRows: function() {
          var inlineTables = this.table.querySelectorAll('table'),
              inlineRows = (inlineTables) ? queryInList(inlineTables, 'tr') : [],
              allRows = this.table.querySelectorAll('tr'),
              tableRows = (inlineRows.length > 0) ? wysihtml5.lang.array(allRows).without(inlineRows) : allRows;

          return tableRows;
        },

        getMapIndex: function(cell) {
          var r_length = this.map.length,
              c_length = (this.map && this.map[0]) ? this.map[0].length : 0;

          for (var r_idx = 0;r_idx < r_length; r_idx++) {
              for (var c_idx = 0;c_idx < c_length; c_idx++) {
                  if (this.map[r_idx][c_idx].el === cell) {
                      return {'row': r_idx, 'col': c_idx};
                  }
              }
          }
          return false;
        },

        getElementAtIndex: function(idx) {
            this.setTableMap();
            if (this.map[idx.row] && this.map[idx.row][idx.col] && this.map[idx.row][idx.col].el) {
                return this.map[idx.row][idx.col].el;
            }
            return null;
        },

        getMapElsTo: function(to_cell) {
            var els = [];
            this.setTableMap();
            this.idx_start = this.getMapIndex(this.cell);
            this.idx_end = this.getMapIndex(to_cell);

            // switch indexes if start is bigger than end
            if (this.idx_start.row > this.idx_end.row || (this.idx_start.row == this.idx_end.row && this.idx_start.col > this.idx_end.col)) {
                var temp_idx = this.idx_start;
                this.idx_start = this.idx_end;
                this.idx_end = temp_idx;
            }
            if (this.idx_start.col > this.idx_end.col) {
                var temp_cidx = this.idx_start.col;
                this.idx_start.col = this.idx_end.col;
                this.idx_end.col = temp_cidx;
            }

            if (this.idx_start != null && this.idx_end != null) {
                for (var row = this.idx_start.row, maxr = this.idx_end.row; row <= maxr; row++) {
                    for (var col = this.idx_start.col, maxc = this.idx_end.col; col <= maxc; col++) {
                        els.push(this.map[row][col].el);
                    }
                }
            }
            return els;
        },

        orderSelectionEnds: function(secondcell) {
            this.setTableMap();
            this.idx_start = this.getMapIndex(this.cell);
            this.idx_end = this.getMapIndex(secondcell);

            // switch indexes if start is bigger than end
            if (this.idx_start.row > this.idx_end.row || (this.idx_start.row == this.idx_end.row && this.idx_start.col > this.idx_end.col)) {
                var temp_idx = this.idx_start;
                this.idx_start = this.idx_end;
                this.idx_end = temp_idx;
            }
            if (this.idx_start.col > this.idx_end.col) {
                var temp_cidx = this.idx_start.col;
                this.idx_start.col = this.idx_end.col;
                this.idx_end.col = temp_cidx;
            }

            return {
                "start": this.map[this.idx_start.row][this.idx_start.col].el,
                "end": this.map[this.idx_end.row][this.idx_end.col].el
            };
        },

        createCells: function(tag, nr, attrs) {
            var doc = this.table.ownerDocument,
                frag = doc.createDocumentFragment(),
                cell;
            for (var i = 0; i < nr; i++) {
                cell = doc.createElement(tag);

                if (attrs) {
                    for (var attr in attrs) {
                        if (attrs.hasOwnProperty(attr)) {
                            cell.setAttribute(attr, attrs[attr]);
                        }
                    }
                }

                // add non breaking space
                cell.appendChild(document.createTextNode("\u00a0"));

                frag.appendChild(cell);
            }
            return frag;
        },

        // Returns next real cell (not part of spanned cell unless first) on row if selected index is not real. I no real cells -1 will be returned
        correctColIndexForUnreals: function(col, row) {
            var r = this.map[row],
                corrIdx = -1;
            for (var i = 0, max = col; i < col; i++) {
                if (r[i].isReal){
                    corrIdx++;
                }
            }
            return corrIdx;
        },

        getLastNewCellOnRow: function(row, rowLimit) {
            var cells = this.getRowCells(row),
                cell, idx;

            for (var cidx = 0, cmax = cells.length; cidx < cmax; cidx++) {
                cell = cells[cidx];
                idx = this.getMapIndex(cell);
                if (idx === false || (typeof rowLimit != "undefined" && idx.row != rowLimit)) {
                    return cell;
                }
            }
            return null;
        },

        removeEmptyTable: function() {
            var cells = this.table.querySelectorAll('td, th');
            if (!cells || cells.length == 0) {
                removeElement(this.table);
                return true;
            } else {
                return false;
            }
        },

        // Splits merged cell on row to unique cells
        splitRowToCells: function(cell) {
            if (cell.isColspan) {
                var colspan = parseInt(api.getAttribute(cell.el, 'colspan') || 1, 10),
                    cType = cell.el.tagName.toLowerCase();
                if (colspan > 1) {
                    var newCells = this.createCells(cType, colspan -1);
                    insertAfter(cell.el, newCells);
                }
                cell.el.removeAttribute('colspan');
            }
        },

        getRealRowEl: function(force, idx) {
            var r = null,
                c = null;

            idx = idx || this.idx;

            for (var cidx = 0, cmax = this.map[idx.row].length; cidx < cmax; cidx++) {
                c = this.map[idx.row][cidx];
                if (c.isReal) {
                    r = api.getParentElement(c.el, { nodeName: ["TR"] });
                    if (r) {
                        return r;
                    }
                }
            }

            if (r === null && force) {
                r = api.getParentElement(this.map[idx.row][idx.col].el, { nodeName: ["TR"] }) || null;
            }

            return r;
        },

        injectRowAt: function(row, col, colspan, cType, c) {
            var r = this.getRealRowEl(false, {'row': row, 'col': col}),
                new_cells = this.createCells(cType, colspan);

            if (r) {
                var n_cidx = this.correctColIndexForUnreals(col, row);
                if (n_cidx >= 0) {
                    insertAfter(this.getRowCells(r)[n_cidx], new_cells);
                } else {
                    r.insertBefore(new_cells, r.firstChild);
                }
            } else {
                var rr = this.table.ownerDocument.createElement('tr');
                rr.appendChild(new_cells);
                insertAfter(api.getParentElement(c.el, { nodeName: ["TR"] }), rr);
            }
        },

        canMerge: function(to) {
            this.to = to;
            this.setTableMap();
            this.idx_start = this.getMapIndex(this.cell);
            this.idx_end = this.getMapIndex(this.to);

            // switch indexes if start is bigger than end
            if (this.idx_start.row > this.idx_end.row || (this.idx_start.row == this.idx_end.row && this.idx_start.col > this.idx_end.col)) {
                var temp_idx = this.idx_start;
                this.idx_start = this.idx_end;
                this.idx_end = temp_idx;
            }
            if (this.idx_start.col > this.idx_end.col) {
                var temp_cidx = this.idx_start.col;
                this.idx_start.col = this.idx_end.col;
                this.idx_end.col = temp_cidx;
            }

            for (var row = this.idx_start.row, maxr = this.idx_end.row; row <= maxr; row++) {
                for (var col = this.idx_start.col, maxc = this.idx_end.col; col <= maxc; col++) {
                    if (this.map[row][col].isColspan || this.map[row][col].isRowspan) {
                        return false;
                    }
                }
            }
            return true;
        },

        decreaseCellSpan: function(cell, span) {
            var nr = parseInt(api.getAttribute(cell.el, span), 10) - 1;
            if (nr >= 1) {
                cell.el.setAttribute(span, nr);
            } else {
                cell.el.removeAttribute(span);
                if (span == 'colspan') {
                    cell.isColspan = false;
                }
                if (span == 'rowspan') {
                    cell.isRowspan = false;
                }
                cell.firstCol = true;
                cell.lastCol = true;
                cell.firstRow = true;
                cell.lastRow = true;
                cell.isReal = true;
            }
        },

        removeSurplusLines: function() {
            var row, cell, ridx, rmax, cidx, cmax, allRowspan;

            this.setTableMap();
            if (this.map) {
                ridx = 0;
                rmax = this.map.length;
                for (;ridx < rmax; ridx++) {
                    row = this.map[ridx];
                    allRowspan = true;
                    cidx = 0;
                    cmax = row.length;
                    for (; cidx < cmax; cidx++) {
                        cell = row[cidx];
                        if (!(api.getAttribute(cell.el, "rowspan") && parseInt(api.getAttribute(cell.el, "rowspan"), 10) > 1 && cell.firstRow !== true)) {
                            allRowspan = false;
                            break;
                        }
                    }
                    if (allRowspan) {
                        cidx = 0;
                        for (; cidx < cmax; cidx++) {
                            this.decreaseCellSpan(row[cidx], 'rowspan');
                        }
                    }
                }

                // remove rows without cells
                var tableRows = this.getTableRows();
                ridx = 0;
                rmax = tableRows.length;
                for (;ridx < rmax; ridx++) {
                    row = tableRows[ridx];
                    if (row.childNodes.length == 0 && (/^\s*$/.test(row.textContent || row.innerText))) {
                        removeElement(row);
                    }
                }
            }
        },

        fillMissingCells: function() {
            var r_max = 0,
                c_max = 0,
                prevcell = null;

            this.setTableMap();
            if (this.map) {

                // find maximal dimensions of broken table
                r_max = this.map.length;
                for (var ridx = 0; ridx < r_max; ridx++) {
                    if (this.map[ridx].length > c_max) { c_max = this.map[ridx].length; }
                }

                for (var row = 0; row < r_max; row++) {
                    for (var col = 0; col < c_max; col++) {
                        if (this.map[row] && !this.map[row][col]) {
                            if (col > 0) {
                                this.map[row][col] = new MapCell(this.createCells('td', 1));
                                prevcell = this.map[row][col-1];
                                if (prevcell && prevcell.el && prevcell.el.parent) { // if parent does not exist element is removed from dom
                                    insertAfter(this.map[row][col-1].el, this.map[row][col].el);
                                }
                            }
                        }
                    }
                }
            }
        },

        rectify: function() {
            if (!this.removeEmptyTable()) {
                this.removeSurplusLines();
                this.fillMissingCells();
                return true;
            } else {
                return false;
            }
        },

        unmerge: function() {
            if (this.rectify()) {
                this.setTableMap();
                this.idx = this.getMapIndex(this.cell);

                if (this.idx) {
                    var thisCell = this.map[this.idx.row][this.idx.col],
                        colspan = (api.getAttribute(thisCell.el, "colspan")) ? parseInt(api.getAttribute(thisCell.el, "colspan"), 10) : 1,
                        cType = thisCell.el.tagName.toLowerCase();

                    if (thisCell.isRowspan) {
                        var rowspan = parseInt(api.getAttribute(thisCell.el, "rowspan"), 10);
                        if (rowspan > 1) {
                            for (var nr = 1, maxr = rowspan - 1; nr <= maxr; nr++){
                                this.injectRowAt(this.idx.row + nr, this.idx.col, colspan, cType, thisCell);
                            }
                        }
                        thisCell.el.removeAttribute('rowspan');
                    }
                    this.splitRowToCells(thisCell);
                }
            }
        },

        // merges cells from start cell (defined in creating obj) to "to" cell
        merge: function(to) {
            if (this.rectify()) {
                if (this.canMerge(to)) {
                    var rowspan = this.idx_end.row - this.idx_start.row + 1,
                        colspan = this.idx_end.col - this.idx_start.col + 1;

                    for (var row = this.idx_start.row, maxr = this.idx_end.row; row <= maxr; row++) {
                        for (var col = this.idx_start.col, maxc = this.idx_end.col; col <= maxc; col++) {

                            if (row == this.idx_start.row && col == this.idx_start.col) {
                                if (rowspan > 1) {
                                    this.map[row][col].el.setAttribute('rowspan', rowspan);
                                }
                                if (colspan > 1) {
                                    this.map[row][col].el.setAttribute('colspan', colspan);
                                }
                            } else {
                                // transfer content
                                if (!(/^\s*<br\/?>\s*$/.test(this.map[row][col].el.innerHTML.toLowerCase()))) {
                                    this.map[this.idx_start.row][this.idx_start.col].el.innerHTML += ' ' + this.map[row][col].el.innerHTML;
                                }
                                removeElement(this.map[row][col].el);
                            }
                        }
                    }
                    this.rectify();
                } else {
                    if (window.console) {
                        console.log('Do not know how to merge allready merged cells.');
                    }
                }
            }
        },

        // Decreases rowspan of a cell if it is done on first cell of rowspan row (real cell)
        // Cell is moved to next row (if it is real)
        collapseCellToNextRow: function(cell) {
            var cellIdx = this.getMapIndex(cell.el),
                newRowIdx = cellIdx.row + 1,
                newIdx = {'row': newRowIdx, 'col': cellIdx.col};

            if (newRowIdx < this.map.length) {

                var row = this.getRealRowEl(false, newIdx);
                if (row !== null) {
                    var n_cidx = this.correctColIndexForUnreals(newIdx.col, newIdx.row);
                    if (n_cidx >= 0) {
                        insertAfter(this.getRowCells(row)[n_cidx], cell.el);
                    } else {
                        var lastCell = this.getLastNewCellOnRow(row, newRowIdx);
                        if (lastCell !== null) {
                            insertAfter(lastCell, cell.el);
                        } else {
                            row.insertBefore(cell.el, row.firstChild);
                        }
                    }
                    if (parseInt(api.getAttribute(cell.el, 'rowspan'), 10) > 2) {
                        cell.el.setAttribute('rowspan', parseInt(api.getAttribute(cell.el, 'rowspan'), 10) - 1);
                    } else {
                        cell.el.removeAttribute('rowspan');
                    }
                }
            }
        },

        // Removes a cell when removing a row
        // If is rowspan cell then decreases the rowspan
        // and moves cell to next row if needed (is first cell of rowspan)
        removeRowCell: function(cell) {
            if (cell.isReal) {
               if (cell.isRowspan) {
                   this.collapseCellToNextRow(cell);
               } else {
                   removeElement(cell.el);
               }
            } else {
                if (parseInt(api.getAttribute(cell.el, 'rowspan'), 10) > 2) {
                    cell.el.setAttribute('rowspan', parseInt(api.getAttribute(cell.el, 'rowspan'), 10) - 1);
                } else {
                    cell.el.removeAttribute('rowspan');
                }
            }
        },

        getRowElementsByCell: function() {
            var cells = [];
            this.setTableMap();
            this.idx = this.getMapIndex(this.cell);
            if (this.idx !== false) {
                var modRow = this.map[this.idx.row];
                for (var cidx = 0, cmax = modRow.length; cidx < cmax; cidx++) {
                    if (modRow[cidx].isReal) {
                        cells.push(modRow[cidx].el);
                    }
                }
            }
            return cells;
        },

        getColumnElementsByCell: function() {
            var cells = [];
            this.setTableMap();
            this.idx = this.getMapIndex(this.cell);
            if (this.idx !== false) {
                for (var ridx = 0, rmax = this.map.length; ridx < rmax; ridx++) {
                    if (this.map[ridx][this.idx.col] && this.map[ridx][this.idx.col].isReal) {
                        cells.push(this.map[ridx][this.idx.col].el);
                    }
                }
            }
            return cells;
        },

        // Removes the row of selected cell
        removeRow: function() {
            var oldRow = api.getParentElement(this.cell, { nodeName: ["TR"] });
            if (oldRow) {
                this.setTableMap();
                this.idx = this.getMapIndex(this.cell);
                if (this.idx !== false) {
                    var modRow = this.map[this.idx.row];
                    for (var cidx = 0, cmax = modRow.length; cidx < cmax; cidx++) {
                        if (!modRow[cidx].modified) {
                            this.setCellAsModified(modRow[cidx]);
                            this.removeRowCell(modRow[cidx]);
                        }
                    }
                }
                removeElement(oldRow);
            }
        },

        removeColCell: function(cell) {
            if (cell.isColspan) {
                if (parseInt(api.getAttribute(cell.el, 'colspan'), 10) > 2) {
                    cell.el.setAttribute('colspan', parseInt(api.getAttribute(cell.el, 'colspan'), 10) - 1);
                } else {
                    cell.el.removeAttribute('colspan');
                }
            } else if (cell.isReal) {
                removeElement(cell.el);
            }
        },

        removeColumn: function() {
            this.setTableMap();
            this.idx = this.getMapIndex(this.cell);
            if (this.idx !== false) {
                for (var ridx = 0, rmax = this.map.length; ridx < rmax; ridx++) {
                    if (!this.map[ridx][this.idx.col].modified) {
                        this.setCellAsModified(this.map[ridx][this.idx.col]);
                        this.removeColCell(this.map[ridx][this.idx.col]);
                    }
                }
            }
        },

        // removes row or column by selected cell element
        remove: function(what) {
            if (this.rectify()) {
                switch (what) {
                    case 'row':
                        this.removeRow();
                    break;
                    case 'column':
                        this.removeColumn();
                    break;
                }
                this.rectify();
            }
        },

        addRow: function(where) {
            var doc = this.table.ownerDocument;

            this.setTableMap();
            this.idx = this.getMapIndex(this.cell);
            if (where == "below" && api.getAttribute(this.cell, 'rowspan')) {
                this.idx.row = this.idx.row + parseInt(api.getAttribute(this.cell, 'rowspan'), 10) - 1;
            }

            if (this.idx !== false) {
                var modRow = this.map[this.idx.row],
                    newRow = doc.createElement('tr');

                for (var ridx = 0, rmax = modRow.length; ridx < rmax; ridx++) {
                    if (!modRow[ridx].modified) {
                        this.setCellAsModified(modRow[ridx]);
                        this.addRowCell(modRow[ridx], newRow, where);
                    }
                }

                switch (where) {
                    case 'below':
                        insertAfter(this.getRealRowEl(true), newRow);
                    break;
                    case 'above':
                        var cr = api.getParentElement(this.map[this.idx.row][this.idx.col].el, { nodeName: ["TR"] });
                        if (cr) {
                            cr.parentNode.insertBefore(newRow, cr);
                        }
                    break;
                }
            }
        },

        addRowCell: function(cell, row, where) {
            var colSpanAttr = (cell.isColspan) ? {"colspan" : api.getAttribute(cell.el, 'colspan')} : null;
            if (cell.isReal) {
                if (where != 'above' && cell.isRowspan) {
                    cell.el.setAttribute('rowspan', parseInt(api.getAttribute(cell.el,'rowspan'), 10) + 1);
                } else {
                    row.appendChild(this.createCells('td', 1, colSpanAttr));
                }
            } else {
                if (where != 'above' && cell.isRowspan && cell.lastRow) {
                    row.appendChild(this.createCells('td', 1, colSpanAttr));
                } else if (c.isRowspan) {
                    cell.el.attr('rowspan', parseInt(api.getAttribute(cell.el, 'rowspan'), 10) + 1);
                }
            }
        },

        add: function(where) {
            if (this.rectify()) {
                if (where == 'below' || where == 'above') {
                    this.addRow(where);
                }
                if (where == 'before' || where == 'after') {
                    this.addColumn(where);
                }
            }
        },

        addColCell: function (cell, ridx, where) {
            var doAdd,
                cType = cell.el.tagName.toLowerCase();

            // defines add cell vs expand cell conditions
            // true means add
            switch (where) {
                case "before":
                    doAdd = (!cell.isColspan || cell.firstCol);
                break;
                case "after":
                    doAdd = (!cell.isColspan || cell.lastCol || (cell.isColspan && c.el == this.cell));
                break;
            }

            if (doAdd){
                // adds a cell before or after current cell element
                switch (where) {
                    case "before":
                        cell.el.parentNode.insertBefore(this.createCells(cType, 1), cell.el);
                    break;
                    case "after":
                        insertAfter(cell.el, this.createCells(cType, 1));
                    break;
                }

                // handles if cell has rowspan
                if (cell.isRowspan) {
                    this.handleCellAddWithRowspan(cell, ridx+1, where);
                }

            } else {
                // expands cell
                cell.el.setAttribute('colspan',  parseInt(api.getAttribute(cell.el, 'colspan'), 10) + 1);
            }
        },

        addColumn: function(where) {
            var row, modCell;

            this.setTableMap();
            this.idx = this.getMapIndex(this.cell);
            if (where == "after" && api.getAttribute(this.cell, 'colspan')) {
              this.idx.col = this.idx.col + parseInt(api.getAttribute(this.cell, 'colspan'), 10) - 1;
            }

            if (this.idx !== false) {
                for (var ridx = 0, rmax = this.map.length; ridx < rmax; ridx++ ) {
                    row = this.map[ridx];
                    if (row[this.idx.col]) {
                        modCell = row[this.idx.col];
                        if (!modCell.modified) {
                            this.setCellAsModified(modCell);
                            this.addColCell(modCell, ridx , where);
                        }
                    }
                }
            }
        },

        handleCellAddWithRowspan: function (cell, ridx, where) {
            var addRowsNr = parseInt(api.getAttribute(this.cell, 'rowspan'), 10) - 1,
                crow = api.getParentElement(cell.el, { nodeName: ["TR"] }),
                cType = cell.el.tagName.toLowerCase(),
                cidx, temp_r_cells,
                doc = this.table.ownerDocument,
                nrow;

            for (var i = 0; i < addRowsNr; i++) {
                cidx = this.correctColIndexForUnreals(this.idx.col, (ridx + i));
                crow = nextNode(crow, 'tr');
                if (crow) {
                    if (cidx > 0) {
                        switch (where) {
                            case "before":
                                temp_r_cells = this.getRowCells(crow);
                                if (cidx > 0 && this.map[ridx + i][this.idx.col].el != temp_r_cells[cidx] && cidx == temp_r_cells.length - 1) {
                                     insertAfter(temp_r_cells[cidx], this.createCells(cType, 1));
                                } else {
                                    temp_r_cells[cidx].parentNode.insertBefore(this.createCells(cType, 1), temp_r_cells[cidx]);
                                }

                            break;
                            case "after":
                                insertAfter(this.getRowCells(crow)[cidx], this.createCells(cType, 1));
                            break;
                        }
                    } else {
                        crow.insertBefore(this.createCells(cType, 1), crow.firstChild);
                    }
                } else {
                    nrow = doc.createElement('tr');
                    nrow.appendChild(this.createCells(cType, 1));
                    this.table.appendChild(nrow);
                }
            }
        }
    };

    api.table = {
        getCellsBetween: function(cell1, cell2) {
            var c1 = new TableModifyerByCell(cell1);
            return c1.getMapElsTo(cell2);
        },

        addCells: function(cell, where) {
            var c = new TableModifyerByCell(cell);
            c.add(where);
        },

        removeCells: function(cell, what) {
            var c = new TableModifyerByCell(cell);
            c.remove(what);
        },

        mergeCellsBetween: function(cell1, cell2) {
            var c1 = new TableModifyerByCell(cell1);
            c1.merge(cell2);
        },

        unmergeCell: function(cell) {
            var c = new TableModifyerByCell(cell);
            c.unmerge();
        },

        orderSelectionEnds: function(cell, cell2) {
            var c = new TableModifyerByCell(cell);
            return c.orderSelectionEnds(cell2);
        },

        indexOf: function(cell) {
            var c = new TableModifyerByCell(cell);
            c.setTableMap();
            return c.getMapIndex(cell);
        },

        findCell: function(table, idx) {
            var c = new TableModifyerByCell(null, table);
            return c.getElementAtIndex(idx);
        },

        findRowByCell: function(cell) {
            var c = new TableModifyerByCell(cell);
            return c.getRowElementsByCell();
        },

        findColumnByCell: function(cell) {
            var c = new TableModifyerByCell(cell);
            return c.getColumnElementsByCell();
        },

        canMerge: function(cell1, cell2) {
            var c = new TableModifyerByCell(cell1);
            return c.canMerge(cell2);
        }
    };



})(wysihtml5);
;// does a selector query on element or array of elements

wysihtml5.dom.query = function(elements, query) {
    var ret = [],
        q;

    if (elements.nodeType) {
        elements = [elements];
    }

    for (var e = 0, len = elements.length; e < len; e++) {
        q = elements[e].querySelectorAll(query);
        if (q) {
            for(var i = q.length; i--; ret.unshift(q[i]));
        }
    }
    return ret;
};
;wysihtml5.dom.compareDocumentPosition = (function() {
  var documentElement = document.documentElement;
  if (documentElement.compareDocumentPosition) {
    return function(container, element) {
      return container.compareDocumentPosition(element);
    };
  } else {
    return function( container, element ) {
      // implementation borrowed from https://github.com/tmpvar/jsdom/blob/681a8524b663281a0f58348c6129c8c184efc62c/lib/jsdom/level3/core.js // MIT license
      var thisOwner, otherOwner;

      if( container.nodeType === 9) // Node.DOCUMENT_NODE
        thisOwner = container;
      else
        thisOwner = container.ownerDocument;

      if( element.nodeType === 9) // Node.DOCUMENT_NODE
        otherOwner = element;
      else
        otherOwner = element.ownerDocument;

      if( container === element ) return 0;
      if( container === element.ownerDocument ) return 4 + 16; //Node.DOCUMENT_POSITION_FOLLOWING + Node.DOCUMENT_POSITION_CONTAINED_BY;
      if( container.ownerDocument === element ) return 2 + 8;  //Node.DOCUMENT_POSITION_PRECEDING + Node.DOCUMENT_POSITION_CONTAINS;
      if( thisOwner !== otherOwner ) return 1; // Node.DOCUMENT_POSITION_DISCONNECTED;

      // Text nodes for attributes does not have a _parentNode. So we need to find them as attribute child.
      if( container.nodeType === 2 /*Node.ATTRIBUTE_NODE*/ && container.childNodes && wysihtml5.lang.array(container.childNodes).indexOf( element ) !== -1)
        return 4 + 16; //Node.DOCUMENT_POSITION_FOLLOWING + Node.DOCUMENT_POSITION_CONTAINED_BY;

      if( element.nodeType === 2 /*Node.ATTRIBUTE_NODE*/ && element.childNodes && wysihtml5.lang.array(element.childNodes).indexOf( container ) !== -1)
        return 2 + 8; //Node.DOCUMENT_POSITION_PRECEDING + Node.DOCUMENT_POSITION_CONTAINS;

      var point = container;
      var parents = [ ];
      var previous = null;
      while( point ) {
        if( point == element ) return 2 + 8; //Node.DOCUMENT_POSITION_PRECEDING + Node.DOCUMENT_POSITION_CONTAINS;
        parents.push( point );
        point = point.parentNode;
      }
      point = element;
      previous = null;
      while( point ) {
        if( point == container ) return 4 + 16; //Node.DOCUMENT_POSITION_FOLLOWING + Node.DOCUMENT_POSITION_CONTAINED_BY;
        var location_index = wysihtml5.lang.array(parents).indexOf( point );
        if( location_index !== -1) {
         var smallest_common_ancestor = parents[ location_index ];
         var this_index = wysihtml5.lang.array(smallest_common_ancestor.childNodes).indexOf( parents[location_index - 1]);//smallest_common_ancestor.childNodes.toArray().indexOf( parents[location_index - 1] );
         var other_index = wysihtml5.lang.array(smallest_common_ancestor.childNodes).indexOf( previous ); //smallest_common_ancestor.childNodes.toArray().indexOf( previous );
         if( this_index > other_index ) {
               return 2; //Node.DOCUMENT_POSITION_PRECEDING;
         }
         else {
           return 4; //Node.DOCUMENT_POSITION_FOLLOWING;
         }
        }
        previous = point;
        point = point.parentNode;
      }
      return 1; //Node.DOCUMENT_POSITION_DISCONNECTED;
    };
  }
})();
;wysihtml5.dom.unwrap = function(node) {
  if (node.parentNode) {
    while (node.lastChild) {
      wysihtml5.dom.insert(node.lastChild).after(node);
    }
    node.parentNode.removeChild(node);
  }
};;/**
 * Fix most common html formatting misbehaviors of browsers implementation when inserting
 * content via copy & paste contentEditable
 *
 * @author Christopher Blum
 */
wysihtml5.quirks.cleanPastedHTML = (function() {
  // TODO: We probably need more rules here
  var defaultRules = {
    // When pasting underlined links <a> into a contentEditable, IE thinks, it has to insert <u> to keep the styling
    "a u": wysihtml5.dom.replaceWithChildNodes
  };

  function cleanPastedHTML(elementOrHtml, rules, context) {
    rules   = rules || defaultRules;
    context = context || elementOrHtml.ownerDocument || document;

    var element,
        isString = typeof(elementOrHtml) === "string",
        method,
        matches,
        matchesLength,
        i,
        j = 0, n;
    if (isString) {
      element = wysihtml5.dom.getAsDom(elementOrHtml, context);
    } else {
      element = elementOrHtml;
    }

    for (i in rules) {
      matches       = element.querySelectorAll(i);
      method        = rules[i];
      matchesLength = matches.length;
      for (; j<matchesLength; j++) {
        method(matches[j]);
      }
    }

    // replace joined non-breakable spaces with unjoined
    var txtnodes = wysihtml5.dom.getTextNodes(element);
    for (n = txtnodes.length; n--;) {
      txtnodes[n].nodeValue = txtnodes[n].nodeValue.replace(/([\S\u00A0])\u00A0/gi, "$1 ");
    }

    matches = elementOrHtml = rules = null;

    return isString ? element.innerHTML : element;
  }

  return cleanPastedHTML;
})();
;/**
 * IE and Opera leave an empty paragraph in the contentEditable element after clearing it
 *
 * @param {Object} contentEditableElement The contentEditable element to observe for clearing events
 * @exaple
 *    wysihtml5.quirks.ensureProperClearing(myContentEditableElement);
 */
wysihtml5.quirks.ensureProperClearing = (function() {
  var clearIfNecessary = function() {
    var element = this;
    setTimeout(function() {
      var innerHTML = element.innerHTML.toLowerCase();
      if (innerHTML == "<p>&nbsp;</p>" ||
          innerHTML == "<p>&nbsp;</p><p>&nbsp;</p>") {
        element.innerHTML = "";
      }
    }, 0);
  };

  return function(composer) {
    wysihtml5.dom.observe(composer.element, ["cut", "keydown"], clearIfNecessary);
  };
})();
;// See https://bugzilla.mozilla.org/show_bug.cgi?id=664398
//
// In Firefox this:
//      var d = document.createElement("div");
//      d.innerHTML ='<a href="~"></a>';
//      d.innerHTML;
// will result in:
//      <a href="%7E"></a>
// which is wrong
(function(wysihtml5) {
  var TILDE_ESCAPED = "%7E";
  wysihtml5.quirks.getCorrectInnerHTML = function(element) {
    var innerHTML = element.innerHTML;
    if (innerHTML.indexOf(TILDE_ESCAPED) === -1) {
      return innerHTML;
    }

    var elementsWithTilde = element.querySelectorAll("[href*='~'], [src*='~']"),
        url,
        urlToSearch,
        length,
        i;
    for (i=0, length=elementsWithTilde.length; i<length; i++) {
      url         = elementsWithTilde[i].href || elementsWithTilde[i].src;
      urlToSearch = wysihtml5.lang.string(url).replace("~").by(TILDE_ESCAPED);
      innerHTML   = wysihtml5.lang.string(innerHTML).replace(urlToSearch).by(url);
    }
    return innerHTML;
  };
})(wysihtml5);
;/**
 * Force rerendering of a given element
 * Needed to fix display misbehaviors of IE
 *
 * @param {Element} element The element object which needs to be rerendered
 * @example
 *    wysihtml5.quirks.redraw(document.body);
 */
(function(wysihtml5) {
  var CLASS_NAME = "wysihtml5-quirks-redraw";

  wysihtml5.quirks.redraw = function(element) {
    wysihtml5.dom.addClass(element, CLASS_NAME);
    wysihtml5.dom.removeClass(element, CLASS_NAME);

    // Following hack is needed for firefox to make sure that image resize handles are properly removed
    try {
      var doc = element.ownerDocument;
      doc.execCommand("italic", false, null);
      doc.execCommand("italic", false, null);
    } catch(e) {}
  };
})(wysihtml5);
;wysihtml5.quirks.tableCellsSelection = function(editable, editor) {

    var dom = wysihtml5.dom,
        select = {
            table: null,
            start: null,
            end: null,
            cells: null,
            select: selectCells
        },
        selection_class = "wysiwyg-tmp-selected-cell",
        moveHandler = null,
        upHandler = null;

    function init () {

        dom.observe(editable, "mousedown", function(event) {
          var target = wysihtml5.dom.getParentElement(event.target, { nodeName: ["TD", "TH"] });
          if (target) {
              handleSelectionMousedown(target);
          }
        });

        return select;
    }

    function handleSelectionMousedown (target) {
      select.start = target;
      select.end = target;
      select.cells = [target];
      select.table = dom.getParentElement(select.start, { nodeName: ["TABLE"] });

      if (select.table) {
        removeCellSelections();
        dom.addClass(target, selection_class);
        moveHandler = dom.observe(editable, "mousemove", handleMouseMove);
        upHandler = dom.observe(editable, "mouseup", handleMouseUp);
        editor.fire("tableselectstart").fire("tableselectstart:composer");
      }
    }

    // remove all selection classes
    function removeCellSelections () {
        if (editable) {
            var selectedCells = editable.querySelectorAll('.' + selection_class);
            if (selectedCells.length > 0) {
              for (var i = 0; i < selectedCells.length; i++) {
                  dom.removeClass(selectedCells[i], selection_class);
              }
            }
        }
    }

    function addSelections (cells) {
      for (var i = 0; i < cells.length; i++) {
        dom.addClass(cells[i], selection_class);
      }
    }

    function handleMouseMove (event) {
      var curTable = null,
          cell = dom.getParentElement(event.target, { nodeName: ["TD","TH"] }),
          oldEnd;

      if (cell && select.table && select.start) {
        curTable =  dom.getParentElement(cell, { nodeName: ["TABLE"] });
        if (curTable && curTable === select.table) {
          removeCellSelections();
          oldEnd = select.end;
          select.end = cell;
          select.cells = dom.table.getCellsBetween(select.start, cell);
          if (select.cells.length > 1) {
            editor.composer.selection.deselect();
          }
          addSelections(select.cells);
          if (select.end !== oldEnd) {
            editor.fire("tableselectchange").fire("tableselectchange:composer");
          }
        }
      }
    }

    function handleMouseUp (event) {
      moveHandler.stop();
      upHandler.stop();
      editor.fire("tableselect").fire("tableselect:composer");
      setTimeout(function() {
        bindSideclick();
      },0);
    }

    function bindSideclick () {
        var sideClickHandler = dom.observe(editable.ownerDocument, "click", function(event) {
          sideClickHandler.stop();
          if (dom.getParentElement(event.target, { nodeName: ["TABLE"] }) != select.table) {
              removeCellSelections();
              select.table = null;
              select.start = null;
              select.end = null;
              editor.fire("tableunselect").fire("tableunselect:composer");
          }
        });
    }

    function selectCells (start, end) {
        select.start = start;
        select.end = end;
        select.table = dom.getParentElement(select.start, { nodeName: ["TABLE"] });
        selectedCells = dom.table.getCellsBetween(select.start, select.end);
        addSelections(selectedCells);
        bindSideclick();
        editor.fire("tableselect").fire("tableselect:composer");
    }

    return init();

};
;(function(wysihtml5) {
  var RGBA_REGEX     = /^rgba\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*([\d\.]+)\s*\)/i,
      RGB_REGEX      = /^rgb\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*(\d{1,3})\s*\)/i,
      HEX6_REGEX     = /^#([0-9a-f][0-9a-f])([0-9a-f][0-9a-f])([0-9a-f][0-9a-f])/i,
      HEX3_REGEX     = /^#([0-9a-f])([0-9a-f])([0-9a-f])/i;

  var param_REGX = function (p) {
    return new RegExp("(^|\\s|;)" + p + "\\s*:\\s*[^;$]+" , "gi");
  };

  wysihtml5.quirks.styleParser = {

    parseColor: function(stylesStr, paramName) {
      var paramRegex = param_REGX(paramName),
          params = stylesStr.match(paramRegex),
          radix = 10,
          str, colorMatch;

      if (params) {
        for (var i = params.length; i--;) {
          params[i] = wysihtml5.lang.string(params[i].split(':')[1]).trim();
        }
        str = params[params.length-1];

        if (RGBA_REGEX.test(str)) {
          colorMatch = str.match(RGBA_REGEX);
        } else if (RGB_REGEX.test(str)) {
          colorMatch = str.match(RGB_REGEX);
        } else if (HEX6_REGEX.test(str)) {
          colorMatch = str.match(HEX6_REGEX);
          radix = 16;
        } else if (HEX3_REGEX.test(str)) {
          colorMatch = str.match(HEX3_REGEX);
          colorMatch.shift();
          colorMatch.push(1);
          return wysihtml5.lang.array(colorMatch).map(function(d, idx) {
            return (idx < 3) ? (parseInt(d, 16) * 16) + parseInt(d, 16): parseFloat(d);
          });
        }

        if (colorMatch) {
          colorMatch.shift();
          if (!colorMatch[3]) {
            colorMatch.push(1);
          }
          return wysihtml5.lang.array(colorMatch).map(function(d, idx) {
            return (idx < 3) ? parseInt(d, radix): parseFloat(d);
          });
        }
      }
      return false;
    },

    unparseColor: function(val, props) {
      if (props) {
        if (props == "hex") {
          return (val[0].toString(16).toUpperCase()) + (val[1].toString(16).toUpperCase()) + (val[2].toString(16).toUpperCase());
        } else if (props == "hash") {
          return "#" + (val[0].toString(16).toUpperCase()) + (val[1].toString(16).toUpperCase()) + (val[2].toString(16).toUpperCase());
        } else if (props == "rgb") {
          return "rgb(" + val[0] + "," + val[1] + "," + val[2] + ")";
        } else if (props == "rgba") {
          return "rgba(" + val[0] + "," + val[1] + "," + val[2] + "," + val[3] + ")";
        } else if (props == "csv") {
          return  val[0] + "," + val[1] + "," + val[2] + "," + val[3];
        }
      }

      if (val[3] && val[3] !== 1) {
        return "rgba(" + val[0] + "," + val[1] + "," + val[2] + "," + val[3] + ")";
      } else {
        return "rgb(" + val[0] + "," + val[1] + "," + val[2] + ")";
      }
    },

    parseFontSize: function(stylesStr) {
      var params = stylesStr.match(param_REGX('font-size'));
      if (params) {
        return wysihtml5.lang.string(params[params.length - 1].split(':')[1]).trim();
      }
      return false;
    }
  };

})(wysihtml5);
;/**
 * Selection API
 *
 * @example
 *    var selection = new wysihtml5.Selection(editor);
 */
(function(wysihtml5) {
  var dom = wysihtml5.dom;

  function _getCumulativeOffsetTop(element) {
    var top = 0;
    if (element.parentNode) {
      do {
        top += element.offsetTop || 0;
        element = element.offsetParent;
      } while (element);
    }
    return top;
  }

  // Provides the depth of ``descendant`` relative to ``ancestor``
  function getDepth(ancestor, descendant) {
      var ret = 0;
      while (descendant !== ancestor) {
          ret++;
          descendant = descendant.parentNode;
          if (!descendant)
              throw new Error("not a descendant of ancestor!");
      }
      return ret;
  }

  // Should fix the obtained ranges that cannot surrond contents normally to apply changes upon
  // Being considerate to firefox that sets range start start out of span and end inside on doubleclick initiated selection
  function expandRangeToSurround(range) {
      if (range.canSurroundContents()) return;

      var common = range.commonAncestorContainer,
          start_depth = getDepth(common, range.startContainer),
          end_depth = getDepth(common, range.endContainer);

      while(!range.canSurroundContents()) {
        // In the following branches, we cannot just decrement the depth variables because the setStartBefore/setEndAfter may move the start or end of the range more than one level relative to ``common``. So we need to recompute the depth.
        if (start_depth > end_depth) {
            range.setStartBefore(range.startContainer);
            start_depth = getDepth(common, range.startContainer);
        }
        else {
            range.setEndAfter(range.endContainer);
            end_depth = getDepth(common, range.endContainer);
        }
      }
  }

  wysihtml5.Selection = Base.extend(
    /** @scope wysihtml5.Selection.prototype */ {
    constructor: function(editor, contain, unselectableClass) {
      // Make sure that our external range library is initialized
      window.rangy.init();

      this.editor   = editor;
      this.composer = editor.composer;
      this.doc      = this.composer.doc;
      this.contain = contain;
      this.unselectableClass = unselectableClass || false;
    },

    /**
     * Get the current selection as a bookmark to be able to later restore it
     *
     * @return {Object} An object that represents the current selection
     */
    getBookmark: function() {
      var range = this.getRange();
      if (range) expandRangeToSurround(range);
      return range && range.cloneRange();
    },

    /**
     * Restore a selection retrieved via wysihtml5.Selection.prototype.getBookmark
     *
     * @param {Object} bookmark An object that represents the current selection
     */
    setBookmark: function(bookmark) {
      if (!bookmark) {
        return;
      }

      this.setSelection(bookmark);
    },

    /**
     * Set the caret in front of the given node
     *
     * @param {Object} node The element or text node where to position the caret in front of
     * @example
     *    selection.setBefore(myElement);
     */
    setBefore: function(node) {
      var range = rangy.createRange(this.doc);
      range.setStartBefore(node);
      range.setEndBefore(node);
      return this.setSelection(range);
    },

    /**
     * Set the caret after the given node
     *
     * @param {Object} node The element or text node where to position the caret in front of
     * @example
     *    selection.setBefore(myElement);
     */
    setAfter: function(node) {
      var range = rangy.createRange(this.doc);

      range.setStartAfter(node);
      range.setEndAfter(node);
      return this.setSelection(range);
    },

    /**
     * Ability to select/mark nodes
     *
     * @param {Element} node The node/element to select
     * @example
     *    selection.selectNode(document.getElementById("my-image"));
     */
    selectNode: function(node, avoidInvisibleSpace) {
      var range           = rangy.createRange(this.doc),
          isElement       = node.nodeType === wysihtml5.ELEMENT_NODE,
          canHaveHTML     = "canHaveHTML" in node ? node.canHaveHTML : (node.nodeName !== "IMG"),
          content         = isElement ? node.innerHTML : node.data,
          isEmpty         = (content === "" || content === wysihtml5.INVISIBLE_SPACE),
          displayStyle    = dom.getStyle("display").from(node),
          isBlockElement  = (displayStyle === "block" || displayStyle === "list-item");

      if (isEmpty && isElement && canHaveHTML && !avoidInvisibleSpace) {
        // Make sure that caret is visible in node by inserting a zero width no breaking space
        try { node.innerHTML = wysihtml5.INVISIBLE_SPACE; } catch(e) {}
      }

      if (canHaveHTML) {
        range.selectNodeContents(node);
      } else {
        range.selectNode(node);
      }

      if (canHaveHTML && isEmpty && isElement) {
        range.collapse(isBlockElement);
      } else if (canHaveHTML && isEmpty) {
        range.setStartAfter(node);
        range.setEndAfter(node);
      }

      this.setSelection(range);
    },

    /**
     * Get the node which contains the selection
     *
     * @param {Boolean} [controlRange] (only IE) Whether it should return the selected ControlRange element when the selection type is a "ControlRange"
     * @return {Object} The node that contains the caret
     * @example
     *    var nodeThatContainsCaret = selection.getSelectedNode();
     */
    getSelectedNode: function(controlRange) {
      var selection,
          range;

      if (controlRange && this.doc.selection && this.doc.selection.type === "Control") {
        range = this.doc.selection.createRange();
        if (range && range.length) {
          return range.item(0);
        }
      }

      selection = this.getSelection(this.doc);
      if (selection.focusNode === selection.anchorNode) {
        return selection.focusNode;
      } else {
        range = this.getRange(this.doc);
        return range ? range.commonAncestorContainer : this.doc.body;
      }
    },

    fixSelBorders: function() {
      var range = this.getRange();
      expandRangeToSurround(range);
      this.setSelection(range);
    },

    getSelectedOwnNodes: function(controlRange) {
      var selection,
          ranges = this.getOwnRanges(),
          ownNodes = [];

      for (var i = 0, maxi = ranges.length; i < maxi; i++) {
          ownNodes.push(ranges[i].commonAncestorContainer || this.doc.body);
      }
      return ownNodes;
    },

    findNodesInSelection: function(nodeTypes) {
      var ranges = this.getOwnRanges(),
          nodes = [], curNodes;
      for (var i = 0, maxi = ranges.length; i < maxi; i++) {
        curNodes = ranges[i].getNodes([1], function(node) {
            return wysihtml5.lang.array(nodeTypes).contains(node.nodeName);
        });
        nodes = nodes.concat(curNodes);
      }
      return nodes;
    },

    containsUneditable: function() {
      var uneditables = this.getOwnUneditables(),
          selection = this.getSelection();

      for (var i = 0, maxi = uneditables.length; i < maxi; i++) {
        if (selection.containsNode(uneditables[i])) {
          return true;
        }
      }

      return false;
    },

    deleteContents: function()  {
      var ranges = this.getOwnRanges();
      for (var i = ranges.length; i--;) {
        ranges[i].deleteContents();
      }
      this.setSelection(ranges[0]);
    },

    getPreviousNode: function(node, ignoreEmpty) {
      if (!node) {
        var selection = this.getSelection();
        node = selection.anchorNode;
      }

      if (node === this.contain) {
          return false;
      }

      var ret = node.previousSibling,
          parent;

      if (ret === this.contain) {
          return false;
      }

      if (ret && ret.nodeType !== 3 && ret.nodeType !== 1) {
         // do not count comments and other node types
         ret = this.getPreviousNode(ret, ignoreEmpty);
      } else if (ret && ret.nodeType === 3 && (/^\s*$/).test(ret.textContent)) {
        // do not count empty textnodes as previus nodes
        ret = this.getPreviousNode(ret, ignoreEmpty);
      } else if (ignoreEmpty && ret && ret.nodeType === 1 && !wysihtml5.lang.array(["BR", "HR", "IMG"]).contains(ret.nodeName) && (/^[\s]*$/).test(ret.innerHTML)) {
        // Do not count empty nodes if param set.
        // Contenteditable tends to bypass and delete these silently when deleting with caret
        ret = this.getPreviousNode(ret, ignoreEmpty);
      } else if (!ret && node !== this.contain) {
        parent = node.parentNode;
        if (parent !== this.contain) {
            ret = this.getPreviousNode(parent, ignoreEmpty);
        }
      }

      return (ret !== this.contain) ? ret : false;
    },

    getSelectionParentsByTag: function(tagName) {
      var nodes = this.getSelectedOwnNodes(),
          curEl, parents = [];

      for (var i = 0, maxi = nodes.length; i < maxi; i++) {
        curEl = (nodes[i].nodeName &&  nodes[i].nodeName === 'LI') ? nodes[i] : wysihtml5.dom.getParentElement(nodes[i], { nodeName: ['LI']}, false, this.contain);
        if (curEl) {
          parents.push(curEl);
        }
      }
      return (parents.length) ? parents : null;
    },

    getRangeToNodeEnd: function() {
      if (this.isCollapsed()) {
        var range = this.getRange(),
            sNode = range.startContainer,
            pos = range.startOffset,
            lastR = rangy.createRange(this.doc);

        lastR.selectNodeContents(sNode);
        lastR.setStart(sNode, pos);
        return lastR;
      }
    },

    caretIsLastInSelection: function() {
      var r = rangy.createRange(this.doc),
          s = this.getSelection(),
          endc = this.getRangeToNodeEnd().cloneContents(),
          endtxt = endc.textContent;

      return (/^\s*$/).test(endtxt);
    },

    caretIsFirstInSelection: function() {
      var r = rangy.createRange(this.doc),
          s = this.getSelection(),
          range = this.getRange(),
          startNode = range.startContainer;
      
      if (startNode.nodeType === wysihtml5.TEXT_NODE) {
        return this.isCollapsed() && (startNode.nodeType === wysihtml5.TEXT_NODE && (/^\s*$/).test(startNode.data.substr(0,range.startOffset)));
      } else {
        r.selectNodeContents(this.getRange().commonAncestorContainer);
        r.collapse(true);
        return (this.isCollapsed() && (r.startContainer === s.anchorNode || r.endContainer === s.anchorNode) && r.startOffset === s.anchorOffset);
      }
    },

    caretIsInTheBeginnig: function(ofNode) {
        var selection = this.getSelection(),
            node = selection.anchorNode,
            offset = selection.anchorOffset;
        if (ofNode) {
          return (offset === 0 && (node.nodeName && node.nodeName === ofNode.toUpperCase() || wysihtml5.dom.getParentElement(node.parentNode, { nodeName: ofNode }, 1)));
        } else {
          return (offset === 0 && !this.getPreviousNode(node, true));
        }
    },

    caretIsBeforeUneditable: function() {
      var selection = this.getSelection(),
          node = selection.anchorNode,
          offset = selection.anchorOffset;

      if (offset === 0) {
        var prevNode = this.getPreviousNode(node, true);
        if (prevNode) {
          var uneditables = this.getOwnUneditables();
          for (var i = 0, maxi = uneditables.length; i < maxi; i++) {
            if (prevNode === uneditables[i]) {
              return uneditables[i];
            }
          }
        }
      }
      return false;
    },

    // TODO: Figure out a method from following 3 that would work universally
    executeAndRestoreRangy: function(method, restoreScrollPosition) {
      var win = this.doc.defaultView || this.doc.parentWindow,
          sel = rangy.saveSelection(win);

      if (!sel) {
        method();
      } else {
        try {
          method();
        } catch(e) {
          setTimeout(function() { throw e; }, 0);
        }
      }
      rangy.restoreSelection(sel);
    },

    // TODO: has problems in chrome 12. investigate block level and uneditable area inbetween
    executeAndRestore: function(method, restoreScrollPosition) {
      var body                  = this.doc.body,
          oldScrollTop          = restoreScrollPosition && body.scrollTop,
          oldScrollLeft         = restoreScrollPosition && body.scrollLeft,
          className             = "_wysihtml5-temp-placeholder",
          placeholderHtml       = '<span class="' + className + '">' + wysihtml5.INVISIBLE_SPACE + '</span>',
          range                 = this.getRange(true),
          caretPlaceholder,
          newCaretPlaceholder,
          nextSibling, prevSibling,
          node, node2, range2,
          newRange;

      // Nothing selected, execute and say goodbye
      if (!range) {
        method(body, body);
        return;
      }

      if (!range.collapsed) {
        range2 = range.cloneRange();
        node2 = range2.createContextualFragment(placeholderHtml);
        range2.collapse(false);
        range2.insertNode(node2);
        range2.detach();
      }

      node = range.createContextualFragment(placeholderHtml);
      range.insertNode(node);

      if (node2) {
        caretPlaceholder = this.contain.querySelectorAll("." + className);
        range.setStartBefore(caretPlaceholder[0]);
        range.setEndAfter(caretPlaceholder[caretPlaceholder.length -1]);
      }
      this.setSelection(range);

      // Make sure that a potential error doesn't cause our placeholder element to be left as a placeholder
      try {
        method(range.startContainer, range.endContainer);
      } catch(e) {
        setTimeout(function() { throw e; }, 0);
      }
      caretPlaceholder = this.contain.querySelectorAll("." + className);
      if (caretPlaceholder && caretPlaceholder.length) {
        newRange = rangy.createRange(this.doc);
        nextSibling = caretPlaceholder[0].nextSibling;
        if (caretPlaceholder.length > 1) {
          prevSibling = caretPlaceholder[caretPlaceholder.length -1].previousSibling;
        }
        if (prevSibling && nextSibling) {
          newRange.setStartBefore(nextSibling);
          newRange.setEndAfter(prevSibling);
        } else {
          newCaretPlaceholder = this.doc.createTextNode(wysihtml5.INVISIBLE_SPACE);
          dom.insert(newCaretPlaceholder).after(caretPlaceholder[0]);
          newRange.setStartBefore(newCaretPlaceholder);
          newRange.setEndAfter(newCaretPlaceholder);
        }
        this.setSelection(newRange);
        for (var i = caretPlaceholder.length; i--;) {
         caretPlaceholder[i].parentNode.removeChild(caretPlaceholder[i]);
        }

      } else {
        // fallback for when all hell breaks loose
        this.contain.focus();
      }

      if (restoreScrollPosition) {
        body.scrollTop  = oldScrollTop;
        body.scrollLeft = oldScrollLeft;
      }

      // Remove it again, just to make sure that the placeholder is definitely out of the dom tree
      try {
        caretPlaceholder.parentNode.removeChild(caretPlaceholder);
      } catch(e2) {}
    },

    set: function(node, offset) {
      var newRange = rangy.createRange(this.doc);
      newRange.setStart(node, offset || 0);
      this.setSelection(newRange);
    },

    /**
     * Insert html at the caret position and move the cursor after the inserted html
     *
     * @param {String} html HTML string to insert
     * @example
     *    selection.insertHTML("<p>foobar</p>");
     */
    insertHTML: function(html) {
      var range     = rangy.createRange(this.doc),
          node      = range.createContextualFragment(html),
          lastChild = node.lastChild;

      this.insertNode(node);
      if (lastChild) {
        this.setAfter(lastChild);
      }
    },

    /**
     * Insert a node at the caret position and move the cursor behind it
     *
     * @param {Object} node HTML string to insert
     * @example
     *    selection.insertNode(document.createTextNode("foobar"));
     */
    insertNode: function(node) {
      var range = this.getRange();
      if (range) {
        range.insertNode(node);
      }
    },

    /**
     * Wraps current selection with the given node
     *
     * @param {Object} node The node to surround the selected elements with
     */
    surround: function(nodeOptions) {
      var ranges = this.getOwnRanges(),
          node, nodes = [];
      if (ranges.length == 0) {
        return nodes;
      }

      for (var i = ranges.length; i--;) {
        node = this.doc.createElement(nodeOptions.nodeName);
        nodes.push(node);
        if (nodeOptions.className) {
          node.className = nodeOptions.className;
        }
        if (nodeOptions.cssStyle) {
          node.setAttribute('style', nodeOptions.cssStyle);
        }
        try {
          // This only works when the range boundaries are not overlapping other elements
          ranges[i].surroundContents(node);
          this.selectNode(node);
        } catch(e) {
          // fallback
          node.appendChild(ranges[i].extractContents());
          ranges[i].insertNode(node);
        }
      }
      return nodes;
    },

    deblockAndSurround: function(nodeOptions) {
      var tempElement = this.doc.createElement('div'),
          range = rangy.createRange(this.doc),
          tempDivElements,
          tempElements,
          firstChild;

      tempElement.className = nodeOptions.className;

      this.composer.commands.exec("formatBlock", nodeOptions.nodeName, nodeOptions.className);
      tempDivElements = this.contain.querySelectorAll("." + nodeOptions.className);
      if (tempDivElements[0]) {
        tempDivElements[0].parentNode.insertBefore(tempElement, tempDivElements[0]);

        range.setStartBefore(tempDivElements[0]);
        range.setEndAfter(tempDivElements[tempDivElements.length - 1]);
        tempElements = range.extractContents();

        while (tempElements.firstChild) {
          firstChild = tempElements.firstChild;
          if (firstChild.nodeType == 1 && wysihtml5.dom.hasClass(firstChild, nodeOptions.className)) {
            while (firstChild.firstChild) {
              tempElement.appendChild(firstChild.firstChild);
            }
            if (firstChild.nodeName !== "BR") { tempElement.appendChild(this.doc.createElement('br')); }
            tempElements.removeChild(firstChild);
          } else {
            tempElement.appendChild(firstChild);
          }
        }
      } else {
        tempElement = null;
      }

      return tempElement;
    },

    /**
     * Scroll the current caret position into the view
     * FIXME: This is a bit hacky, there might be a smarter way of doing this
     *
     * @example
     *    selection.scrollIntoView();
     */
    scrollIntoView: function() {
      var doc           = this.doc,
          tolerance     = 5, // px
          hasScrollBars = doc.documentElement.scrollHeight > doc.documentElement.offsetHeight,
          tempElement   = doc._wysihtml5ScrollIntoViewElement = doc._wysihtml5ScrollIntoViewElement || (function() {
            var element = doc.createElement("span");
            // The element needs content in order to be able to calculate it's position properly
            element.innerHTML = wysihtml5.INVISIBLE_SPACE;
            return element;
          })(),
          offsetTop;

      if (hasScrollBars) {
        this.insertNode(tempElement);
        offsetTop = _getCumulativeOffsetTop(tempElement);
        tempElement.parentNode.removeChild(tempElement);
        if (offsetTop >= (doc.body.scrollTop + doc.documentElement.offsetHeight - tolerance)) {
          doc.body.scrollTop = offsetTop;
        }
      }
    },

    /**
     * Select line where the caret is in
     */
    selectLine: function() {
      if (wysihtml5.browser.supportsSelectionModify()) {
        this._selectLine_W3C();
      } else if (this.doc.selection) {
        this._selectLine_MSIE();
      }
    },

    /**
     * See https://developer.mozilla.org/en/DOM/Selection/modify
     */
    _selectLine_W3C: function() {
      var win       = this.doc.defaultView,
          selection = win.getSelection();
      selection.modify("move", "left", "lineboundary");
      selection.modify("extend", "right", "lineboundary");
    },

    _selectLine_MSIE: function() {
      var range       = this.doc.selection.createRange(),
          rangeTop    = range.boundingTop,
          scrollWidth = this.doc.body.scrollWidth,
          rangeBottom,
          rangeEnd,
          measureNode,
          i,
          j;

      if (!range.moveToPoint) {
        return;
      }

      if (rangeTop === 0) {
        // Don't know why, but when the selection ends at the end of a line
        // range.boundingTop is 0
        measureNode = this.doc.createElement("span");
        this.insertNode(measureNode);
        rangeTop = measureNode.offsetTop;
        measureNode.parentNode.removeChild(measureNode);
      }

      rangeTop += 1;

      for (i=-10; i<scrollWidth; i+=2) {
        try {
          range.moveToPoint(i, rangeTop);
          break;
        } catch(e1) {}
      }

      // Investigate the following in order to handle multi line selections
      // rangeBottom = rangeTop + (rangeHeight ? (rangeHeight - 1) : 0);
      rangeBottom = rangeTop;
      rangeEnd = this.doc.selection.createRange();
      for (j=scrollWidth; j>=0; j--) {
        try {
          rangeEnd.moveToPoint(j, rangeBottom);
          break;
        } catch(e2) {}
      }

      range.setEndPoint("EndToEnd", rangeEnd);
      range.select();
    },

    getText: function() {
      var selection = this.getSelection();
      return selection ? selection.toString() : "";
    },

    getNodes: function(nodeType, filter) {
      var range = this.getRange();
      if (range) {
        return range.getNodes([nodeType], filter);
      } else {
        return [];
      }
    },

    fixRangeOverflow: function(range) {
      if (this.contain && this.contain.firstChild && range) {
        var containment = range.compareNode(this.contain);
        if (containment !== 2) {
          if (containment === 1) {
            range.setStartBefore(this.contain.firstChild);
          }
          if (containment === 0) {
            range.setEndAfter(this.contain.lastChild);
          }
          if (containment === 3) {
            range.setStartBefore(this.contain.firstChild);
            range.setEndAfter(this.contain.lastChild);
          }
        } else if (this._detectInlineRangeProblems(range)) {
          var previousElementSibling = range.endContainer.previousElementSibling;
          if (previousElementSibling) {
            range.setEnd(previousElementSibling, this._endOffsetForNode(previousElementSibling));
          }
        }
      }
    },

    _endOffsetForNode: function(node) {
      var range = document.createRange();
      range.selectNodeContents(node);
      return range.endOffset;
    },

    _detectInlineRangeProblems: function(range) {
      var position = dom.compareDocumentPosition(range.startContainer, range.endContainer);
      return (
        range.endOffset == 0 &&
        position & 4 //Node.DOCUMENT_POSITION_FOLLOWING
      );
    },

    getRange: function(dontFix) {
      var selection = this.getSelection(),
          range = selection && selection.rangeCount && selection.getRangeAt(0);

      if (dontFix !== true) {
        this.fixRangeOverflow(range);
      }

      return range;
    },

    getOwnUneditables: function() {
      var allUneditables = dom.query(this.contain, '.' + this.unselectableClass),
          deepUneditables = dom.query(allUneditables, '.' + this.unselectableClass);

      return wysihtml5.lang.array(allUneditables).without(deepUneditables);
    },

    // Returns an array of ranges that belong only to this editable
    // Needed as uneditable block in contenteditabel can split range into pieces
    // If manipulating content reverse loop is usually needed as manipulation can shift subsequent ranges
    getOwnRanges: function()  {
      var ranges = [],
          r = this.getRange(),
          tmpRanges;

      if (r) { ranges.push(r); }

      if (this.unselectableClass && this.contain && r) {
          var uneditables = this.getOwnUneditables(),
              tmpRange;
          if (uneditables.length > 0) {
            for (var i = 0, imax = uneditables.length; i < imax; i++) {
              tmpRanges = [];
              for (var j = 0, jmax = ranges.length; j < jmax; j++) {
                if (ranges[j]) {
                  switch (ranges[j].compareNode(uneditables[i])) {
                    case 2:
                      // all selection inside uneditable. remove
                    break;
                    case 3:
                      //section begins before and ends after uneditable. spilt
                      tmpRange = ranges[j].cloneRange();
                      tmpRange.setEndBefore(uneditables[i]);
                      tmpRanges.push(tmpRange);

                      tmpRange = ranges[j].cloneRange();
                      tmpRange.setStartAfter(uneditables[i]);
                      tmpRanges.push(tmpRange);
                    break;
                    default:
                      // in all other cases uneditable does not touch selection. dont modify
                      tmpRanges.push(ranges[j]);
                  }
                }
                ranges = tmpRanges;
              }
            }
          }
      }
      return ranges;
    },

    getSelection: function() {
      return rangy.getSelection(this.doc.defaultView || this.doc.parentWindow);
    },

    setSelection: function(range) {
      var win       = this.doc.defaultView || this.doc.parentWindow,
          selection = rangy.getSelection(win);
      return selection.setSingleRange(range);
    },

    createRange: function() {
      return rangy.createRange(this.doc);
    },

    isCollapsed: function() {
        return this.getSelection().isCollapsed;
    },

    isEndToEndInNode: function(nodeNames) {
      var range = this.getRange(),
          parentElement = range.commonAncestorContainer,
          startNode = range.startContainer,
          endNode = range.endContainer;


        if (parentElement.nodeType === wysihtml5.TEXT_NODE) {
          parentElement = parentElement.parentNode;
        }

        if (startNode.nodeType === wysihtml5.TEXT_NODE && !(/^\s*$/).test(startNode.data.substr(range.startOffset))) {
          return false;
        }

        if (endNode.nodeType === wysihtml5.TEXT_NODE && !(/^\s*$/).test(endNode.data.substr(range.endOffset))) {
          return false;
        }

        while (startNode && startNode !== parentElement) {
          if (startNode.nodeType !== wysihtml5.TEXT_NODE && !wysihtml5.dom.contains(parentElement, startNode)) {
            return false;
          }
          if (wysihtml5.dom.domNode(startNode).prev({ignoreBlankTexts: true})) {
            return false;
          }
          startNode = startNode.parentNode;
        }

        while (endNode && endNode !== parentElement) {
          if (endNode.nodeType !== wysihtml5.TEXT_NODE && !wysihtml5.dom.contains(parentElement, endNode)) {
            return false;
          }
          if (wysihtml5.dom.domNode(endNode).next({ignoreBlankTexts: true})) {
            return false;
          }
          endNode = endNode.parentNode;
        }

        return (wysihtml5.lang.array(nodeNames).contains(parentElement.nodeName)) ? parentElement : false;
    },

    deselect: function() {
      var sel = this.getSelection();
      sel && sel.removeAllRanges();
    }
  });

})(wysihtml5);
;/**
 * Inspired by the rangy CSS Applier module written by Tim Down and licensed under the MIT license.
 * http://code.google.com/p/rangy/
 *
 * changed in order to be able ...
 *    - to use custom tags
 *    - to detect and replace similar css classes via reg exp
 */
(function(wysihtml5, rangy) {
  var defaultTagName = "span";

  var REG_EXP_WHITE_SPACE = /\s+/g;

  function hasClass(el, cssClass, regExp) {
    if (!el.className) {
      return false;
    }

    var matchingClassNames = el.className.match(regExp) || [];
    return matchingClassNames[matchingClassNames.length - 1] === cssClass;
  }

  function hasStyleAttr(el, regExp) {
    if (!el.getAttribute || !el.getAttribute('style')) {
      return false;
    }
    var matchingStyles = el.getAttribute('style').match(regExp);
    return  (el.getAttribute('style').match(regExp)) ? true : false;
  }

  function addStyle(el, cssStyle, regExp) {
    if (el.getAttribute('style')) {
      removeStyle(el, regExp);
      if (el.getAttribute('style') && !(/^\s*$/).test(el.getAttribute('style'))) {
        el.setAttribute('style', cssStyle + ";" + el.getAttribute('style'));
      } else {
        el.setAttribute('style', cssStyle);
      }
    } else {
      el.setAttribute('style', cssStyle);
    }
  }

  function addClass(el, cssClass, regExp) {
    if (el.className) {
      removeClass(el, regExp);
      el.className += " " + cssClass;
    } else {
      el.className = cssClass;
    }
  }

  function removeClass(el, regExp) {
    if (el.className) {
      el.className = el.className.replace(regExp, "");
    }
  }

  function removeStyle(el, regExp) {
    var s,
        s2 = [];
    if (el.getAttribute('style')) {
      s = el.getAttribute('style').split(';');
      for (var i = s.length; i--;) {
        if (!s[i].match(regExp) && !(/^\s*$/).test(s[i])) {
          s2.push(s[i]);
        }
      }
      if (s2.length) {
        el.setAttribute('style', s2.join(';'));
      } else {
        el.removeAttribute('style');
      }
    }
  }

  function getMatchingStyleRegexp(el, style) {
    var regexes = [],
        sSplit = style.split(';'),
        elStyle = el.getAttribute('style');

    if (elStyle) {
      elStyle = elStyle.replace(/\s/gi, '').toLowerCase();
      regexes.push(new RegExp("(^|\\s|;)" + style.replace(/\s/gi, '').replace(/([\(\)])/gi, "\\$1").toLowerCase().replace(";", ";?").replace(/rgb\\\((\d+),(\d+),(\d+)\\\)/gi, "\\s?rgb\\($1,\\s?$2,\\s?$3\\)"), "gi"));

      for (var i = sSplit.length; i-- > 0;) {
        if (!(/^\s*$/).test(sSplit[i])) {
          regexes.push(new RegExp("(^|\\s|;)" + sSplit[i].replace(/\s/gi, '').replace(/([\(\)])/gi, "\\$1").toLowerCase().replace(";", ";?").replace(/rgb\\\((\d+),(\d+),(\d+)\\\)/gi, "\\s?rgb\\($1,\\s?$2,\\s?$3\\)"), "gi"));
        }
      }
      for (var j = 0, jmax = regexes.length; j < jmax; j++) {
        if (elStyle.match(regexes[j])) {
          return regexes[j];
        }
      }
    }

    return false;
  }

  function isMatchingAllready(node, tags, style, className) {
    if (style) {
      return getMatchingStyleRegexp(node, style);
    } else if (className) {
      return wysihtml5.dom.hasClass(node, className);
    } else {
      return rangy.dom.arrayContains(tags, node.tagName.toLowerCase());
    }
  }

  function areMatchingAllready(nodes, tags, style, className) {
    for (var i = nodes.length; i--;) {
      if (!isMatchingAllready(nodes[i], tags, style, className)) {
        return false;
      }
    }
    return nodes.length ? true : false;
  }

  function removeOrChangeStyle(el, style, regExp) {

    var exactRegex = getMatchingStyleRegexp(el, style);
    if (exactRegex) {
      // adding same style value on property again removes style
      removeStyle(el, exactRegex);
      return "remove";
    } else {
      // adding new style value changes value
      addStyle(el, style, regExp);
      return "change";
    }
  }

  function hasSameClasses(el1, el2) {
    return el1.className.replace(REG_EXP_WHITE_SPACE, " ") == el2.className.replace(REG_EXP_WHITE_SPACE, " ");
  }

  function replaceWithOwnChildren(el) {
    var parent = el.parentNode;
    while (el.firstChild) {
      parent.insertBefore(el.firstChild, el);
    }
    parent.removeChild(el);
  }

  function elementsHaveSameNonClassAttributes(el1, el2) {
    if (el1.attributes.length != el2.attributes.length) {
      return false;
    }
    for (var i = 0, len = el1.attributes.length, attr1, attr2, name; i < len; ++i) {
      attr1 = el1.attributes[i];
      name = attr1.name;
      if (name != "class") {
        attr2 = el2.attributes.getNamedItem(name);
        if (attr1.specified != attr2.specified) {
          return false;
        }
        if (attr1.specified && attr1.nodeValue !== attr2.nodeValue) {
          return false;
        }
      }
    }
    return true;
  }

  function isSplitPoint(node, offset) {
    if (rangy.dom.isCharacterDataNode(node)) {
      if (offset == 0) {
        return !!node.previousSibling;
      } else if (offset == node.length) {
        return !!node.nextSibling;
      } else {
        return true;
      }
    }

    return offset > 0 && offset < node.childNodes.length;
  }

  function splitNodeAt(node, descendantNode, descendantOffset, container) {
    var newNode;
    if (rangy.dom.isCharacterDataNode(descendantNode)) {
      if (descendantOffset == 0) {
        descendantOffset = rangy.dom.getNodeIndex(descendantNode);
        descendantNode = descendantNode.parentNode;
      } else if (descendantOffset == descendantNode.length) {
        descendantOffset = rangy.dom.getNodeIndex(descendantNode) + 1;
        descendantNode = descendantNode.parentNode;
      } else {
        newNode = rangy.dom.splitDataNode(descendantNode, descendantOffset);
      }
    }
    if (!newNode) {
      if (!container || descendantNode !== container) {

        newNode = descendantNode.cloneNode(false);
        if (newNode.id) {
          newNode.removeAttribute("id");
        }
        var child;
        while ((child = descendantNode.childNodes[descendantOffset])) {
          newNode.appendChild(child);
        }
        rangy.dom.insertAfter(newNode, descendantNode);

      }
    }
    return (descendantNode == node) ? newNode :  splitNodeAt(node, newNode.parentNode, rangy.dom.getNodeIndex(newNode), container);
  }

  function Merge(firstNode) {
    this.isElementMerge = (firstNode.nodeType == wysihtml5.ELEMENT_NODE);
    this.firstTextNode = this.isElementMerge ? firstNode.lastChild : firstNode;
    this.textNodes = [this.firstTextNode];
  }

  Merge.prototype = {
    doMerge: function() {
      var textBits = [], textNode, parent, text;
      for (var i = 0, len = this.textNodes.length; i < len; ++i) {
        textNode = this.textNodes[i];
        parent = textNode.parentNode;
        textBits[i] = textNode.data;
        if (i) {
          parent.removeChild(textNode);
          if (!parent.hasChildNodes()) {
            parent.parentNode.removeChild(parent);
          }
        }
      }
      this.firstTextNode.data = text = textBits.join("");
      return text;
    },

    getLength: function() {
      var i = this.textNodes.length, len = 0;
      while (i--) {
        len += this.textNodes[i].length;
      }
      return len;
    },

    toString: function() {
      var textBits = [];
      for (var i = 0, len = this.textNodes.length; i < len; ++i) {
        textBits[i] = "'" + this.textNodes[i].data + "'";
      }
      return "[Merge(" + textBits.join(",") + ")]";
    }
  };

  function HTMLApplier(tagNames, cssClass, similarClassRegExp, normalize, cssStyle, similarStyleRegExp, container) {
    this.tagNames = tagNames || [defaultTagName];
    this.cssClass = cssClass || ((cssClass === false) ? false : "");
    this.similarClassRegExp = similarClassRegExp;
    this.cssStyle = cssStyle || "";
    this.similarStyleRegExp = similarStyleRegExp;
    this.normalize = normalize;
    this.applyToAnyTagName = false;
    this.container = container;
  }

  HTMLApplier.prototype = {
    getAncestorWithClass: function(node) {
      var cssClassMatch;
      while (node) {
        cssClassMatch = this.cssClass ? hasClass(node, this.cssClass, this.similarClassRegExp) : (this.cssStyle !== "") ? false : true;
        if (node.nodeType == wysihtml5.ELEMENT_NODE && node.getAttribute("contenteditable") != "false" &&  rangy.dom.arrayContains(this.tagNames, node.tagName.toLowerCase()) && cssClassMatch) {
          return node;
        }
        node = node.parentNode;
      }
      return false;
    },

    // returns parents of node with given style attribute
    getAncestorWithStyle: function(node) {
      var cssStyleMatch;
      while (node) {
        cssStyleMatch = this.cssStyle ? hasStyleAttr(node, this.similarStyleRegExp) : false;

        if (node.nodeType == wysihtml5.ELEMENT_NODE && node.getAttribute("contenteditable") != "false" && rangy.dom.arrayContains(this.tagNames, node.tagName.toLowerCase()) && cssStyleMatch) {
          return node;
        }
        node = node.parentNode;
      }
      return false;
    },

    getMatchingAncestor: function(node) {
      var ancestor = this.getAncestorWithClass(node),
          matchType = false;

      if (!ancestor) {
        ancestor = this.getAncestorWithStyle(node);
        if (ancestor) {
          matchType = "style";
        }
      } else {
        if (this.cssStyle) {
          matchType = "class";
        }
      }

      return {
        "element": ancestor,
        "type": matchType
      };
    },

    // Normalizes nodes after applying a CSS class to a Range.
    postApply: function(textNodes, range) {
      var firstNode = textNodes[0], lastNode = textNodes[textNodes.length - 1];

      var merges = [], currentMerge;

      var rangeStartNode = firstNode, rangeEndNode = lastNode;
      var rangeStartOffset = 0, rangeEndOffset = lastNode.length;

      var textNode, precedingTextNode;

      for (var i = 0, len = textNodes.length; i < len; ++i) {
        textNode = textNodes[i];
        precedingTextNode = null;
        if (textNode && textNode.parentNode) {
          precedingTextNode = this.getAdjacentMergeableTextNode(textNode.parentNode, false);
        }
        if (precedingTextNode) {
          if (!currentMerge) {
            currentMerge = new Merge(precedingTextNode);
            merges.push(currentMerge);
          }
          currentMerge.textNodes.push(textNode);
          if (textNode === firstNode) {
            rangeStartNode = currentMerge.firstTextNode;
            rangeStartOffset = rangeStartNode.length;
          }
          if (textNode === lastNode) {
            rangeEndNode = currentMerge.firstTextNode;
            rangeEndOffset = currentMerge.getLength();
          }
        } else {
          currentMerge = null;
        }
      }
      // Test whether the first node after the range needs merging
      if(lastNode && lastNode.parentNode) {
        var nextTextNode = this.getAdjacentMergeableTextNode(lastNode.parentNode, true);
        if (nextTextNode) {
          if (!currentMerge) {
            currentMerge = new Merge(lastNode);
            merges.push(currentMerge);
          }
          currentMerge.textNodes.push(nextTextNode);
        }
      }
      // Do the merges
      if (merges.length) {
        for (i = 0, len = merges.length; i < len; ++i) {
          merges[i].doMerge();
        }
        // Set the range boundaries
        range.setStart(rangeStartNode, rangeStartOffset);
        range.setEnd(rangeEndNode, rangeEndOffset);
      }
    },

    getAdjacentMergeableTextNode: function(node, forward) {
        var isTextNode = (node.nodeType == wysihtml5.TEXT_NODE);
        var el = isTextNode ? node.parentNode : node;
        var adjacentNode;
        var propName = forward ? "nextSibling" : "previousSibling";
        if (isTextNode) {
          // Can merge if the node's previous/next sibling is a text node
          adjacentNode = node[propName];
          if (adjacentNode && adjacentNode.nodeType == wysihtml5.TEXT_NODE) {
            return adjacentNode;
          }
        } else {
          // Compare element with its sibling
          adjacentNode = el[propName];
          if (adjacentNode && this.areElementsMergeable(node, adjacentNode)) {
            return adjacentNode[forward ? "firstChild" : "lastChild"];
          }
        }
        return null;
    },

    areElementsMergeable: function(el1, el2) {
      return rangy.dom.arrayContains(this.tagNames, (el1.tagName || "").toLowerCase())
        && rangy.dom.arrayContains(this.tagNames, (el2.tagName || "").toLowerCase())
        && hasSameClasses(el1, el2)
        && elementsHaveSameNonClassAttributes(el1, el2);
    },

    createContainer: function(doc) {
      var el = doc.createElement(this.tagNames[0]);
      if (this.cssClass) {
        el.className = this.cssClass;
      }
      if (this.cssStyle) {
        el.setAttribute('style', this.cssStyle);
      }
      return el;
    },

    applyToTextNode: function(textNode) {
      var parent = textNode.parentNode;
      if (parent.childNodes.length == 1 && rangy.dom.arrayContains(this.tagNames, parent.tagName.toLowerCase())) {

        if (this.cssClass) {
          addClass(parent, this.cssClass, this.similarClassRegExp);
        }
        if (this.cssStyle) {
          addStyle(parent, this.cssStyle, this.similarStyleRegExp);
        }
      } else {
        var el = this.createContainer(rangy.dom.getDocument(textNode));
        textNode.parentNode.insertBefore(el, textNode);
        el.appendChild(textNode);
      }
    },

    isRemovable: function(el) {
      return rangy.dom.arrayContains(this.tagNames, el.tagName.toLowerCase()) &&
              wysihtml5.lang.string(el.className).trim() === "" &&
              (
                !el.getAttribute('style') ||
                wysihtml5.lang.string(el.getAttribute('style')).trim() === ""
              );
    },

    undoToTextNode: function(textNode, range, ancestorWithClass, ancestorWithStyle) {
      var styleMode = (ancestorWithClass) ? false : true,
          ancestor = ancestorWithClass || ancestorWithStyle,
          styleChanged = false;
      if (!range.containsNode(ancestor)) {
        // Split out the portion of the ancestor from which we can remove the CSS class
        var ancestorRange = range.cloneRange();
            ancestorRange.selectNode(ancestor);

        if (ancestorRange.isPointInRange(range.endContainer, range.endOffset) && isSplitPoint(range.endContainer, range.endOffset)) {
            splitNodeAt(ancestor, range.endContainer, range.endOffset, this.container);
            range.setEndAfter(ancestor);
        }
        if (ancestorRange.isPointInRange(range.startContainer, range.startOffset) && isSplitPoint(range.startContainer, range.startOffset)) {
            ancestor = splitNodeAt(ancestor, range.startContainer, range.startOffset, this.container);
        }
      }

      if (!styleMode && this.similarClassRegExp) {
        removeClass(ancestor, this.similarClassRegExp);
      }

      if (styleMode && this.similarStyleRegExp) {
        styleChanged = (removeOrChangeStyle(ancestor, this.cssStyle, this.similarStyleRegExp) === "change");
      }
      if (this.isRemovable(ancestor) && !styleChanged) {
        replaceWithOwnChildren(ancestor);
      }
    },

    applyToRange: function(range) {
        var textNodes;
        for (var ri = range.length; ri--;) {
            textNodes = range[ri].getNodes([wysihtml5.TEXT_NODE]);

            if (!textNodes.length) {
              try {
                var node = this.createContainer(range[ri].endContainer.ownerDocument);
                range[ri].surroundContents(node);
                this.selectNode(range[ri], node);
                return;
              } catch(e) {}
            }

            range[ri].splitBoundaries();
            textNodes = range[ri].getNodes([wysihtml5.TEXT_NODE]);
            if (textNodes.length) {
              var textNode;

              for (var i = 0, len = textNodes.length; i < len; ++i) {
                textNode = textNodes[i];
                if (!this.getMatchingAncestor(textNode).element) {
                  this.applyToTextNode(textNode);
                }
              }

              range[ri].setStart(textNodes[0], 0);
              textNode = textNodes[textNodes.length - 1];
              range[ri].setEnd(textNode, textNode.length);

              if (this.normalize) {
                this.postApply(textNodes, range[ri]);
              }
            }

        }
    },

    undoToRange: function(range) {
      var textNodes, textNode, ancestorWithClass, ancestorWithStyle, ancestor;
      for (var ri = range.length; ri--;) {

          textNodes = range[ri].getNodes([wysihtml5.TEXT_NODE]);
          if (textNodes.length) {
            range[ri].splitBoundaries();
            textNodes = range[ri].getNodes([wysihtml5.TEXT_NODE]);
          } else {
            var doc = range[ri].endContainer.ownerDocument,
                node = doc.createTextNode(wysihtml5.INVISIBLE_SPACE);
            range[ri].insertNode(node);
            range[ri].selectNode(node);
            textNodes = [node];
          }

          for (var i = 0, len = textNodes.length; i < len; ++i) {
            if (range[ri].isValid()) {
              textNode = textNodes[i];

              ancestor = this.getMatchingAncestor(textNode);
              if (ancestor.type === "style") {
                this.undoToTextNode(textNode, range[ri], false, ancestor.element);
              } else if (ancestor.element) {
                this.undoToTextNode(textNode, range[ri], ancestor.element);
              }
            }
          }

          if (len == 1) {
            this.selectNode(range[ri], textNodes[0]);
          } else {
            range[ri].setStart(textNodes[0], 0);
            textNode = textNodes[textNodes.length - 1];
            range[ri].setEnd(textNode, textNode.length);

            if (this.normalize) {
              this.postApply(textNodes, range[ri]);
            }
          }

      }
    },

    selectNode: function(range, node) {
      var isElement       = node.nodeType === wysihtml5.ELEMENT_NODE,
          canHaveHTML     = "canHaveHTML" in node ? node.canHaveHTML : true,
          content         = isElement ? node.innerHTML : node.data,
          isEmpty         = (content === "" || content === wysihtml5.INVISIBLE_SPACE);

      if (isEmpty && isElement && canHaveHTML) {
        // Make sure that caret is visible in node by inserting a zero width no breaking space
        try { node.innerHTML = wysihtml5.INVISIBLE_SPACE; } catch(e) {}
      }
      range.selectNodeContents(node);
      if (isEmpty && isElement) {
        range.collapse(false);
      } else if (isEmpty) {
        range.setStartAfter(node);
        range.setEndAfter(node);
      }
    },

    getTextSelectedByRange: function(textNode, range) {
      var textRange = range.cloneRange();
      textRange.selectNodeContents(textNode);

      var intersectionRange = textRange.intersection(range);
      var text = intersectionRange ? intersectionRange.toString() : "";
      textRange.detach();

      return text;
    },

    isAppliedToRange: function(range) {
      var ancestors = [],
          appliedType = "full",
          ancestor, styleAncestor, textNodes;

      for (var ri = range.length; ri--;) {

        textNodes = range[ri].getNodes([wysihtml5.TEXT_NODE]);
        if (!textNodes.length) {
          ancestor = this.getMatchingAncestor(range[ri].startContainer).element;

          return (ancestor) ? {
            "elements": [ancestor],
            "coverage": appliedType
          } : false;
        }

        for (var i = 0, len = textNodes.length, selectedText; i < len; ++i) {
          selectedText = this.getTextSelectedByRange(textNodes[i], range[ri]);
          ancestor = this.getMatchingAncestor(textNodes[i]).element;
          if (ancestor && selectedText != "") {
            ancestors.push(ancestor);

            if (wysihtml5.dom.getTextNodes(ancestor, true).length === 1) {
              appliedType = "full";
            } else if (appliedType === "full") {
              appliedType = "inline";
            }
          } else if (!ancestor) {
            appliedType = "partial";
          }
        }

      }

      return (ancestors.length) ? {
        "elements": ancestors,
        "coverage": appliedType
      } : false;
    },

    toggleRange: function(range) {
      var isApplied = this.isAppliedToRange(range),
          parentsExactMatch;

      if (isApplied) {
        if (isApplied.coverage === "full") {
          this.undoToRange(range);
        } else if (isApplied.coverage === "inline") {
          parentsExactMatch = areMatchingAllready(isApplied.elements, this.tagNames, this.cssStyle, this.cssClass);
          this.undoToRange(range);
          if (!parentsExactMatch) {
            this.applyToRange(range);
          }
        } else {
          // partial
          if (!areMatchingAllready(isApplied.elements, this.tagNames, this.cssStyle, this.cssClass)) {
            this.undoToRange(range);
          }
          this.applyToRange(range);
        }
      } else {
        this.applyToRange(range);
      }
    }
  };

  wysihtml5.selection.HTMLApplier = HTMLApplier;

})(wysihtml5, rangy);
;/**
 * Rich Text Query/Formatting Commands
 *
 * @example
 *    var commands = new wysihtml5.Commands(editor);
 */
wysihtml5.Commands = Base.extend(
  /** @scope wysihtml5.Commands.prototype */ {
  constructor: function(editor) {
    this.editor   = editor;
    this.composer = editor.composer;
    this.doc      = this.composer.doc;
  },

  /**
   * Check whether the browser supports the given command
   *
   * @param {String} command The command string which to check (eg. "bold", "italic", "insertUnorderedList")
   * @example
   *    commands.supports("createLink");
   */
  support: function(command) {
    return wysihtml5.browser.supportsCommand(this.doc, command);
  },

  /**
   * Check whether the browser supports the given command
   *
   * @param {String} command The command string which to execute (eg. "bold", "italic", "insertUnorderedList")
   * @param {String} [value] The command value parameter, needed for some commands ("createLink", "insertImage", ...), optional for commands that don't require one ("bold", "underline", ...)
   * @example
   *    commands.exec("insertImage", "http://a1.twimg.com/profile_images/113868655/schrei_twitter_reasonably_small.jpg");
   */
  exec: function(command, value) {
    var obj     = wysihtml5.commands[command],
        args    = wysihtml5.lang.array(arguments).get(),
        method  = obj && obj.exec,
        result  = null;

    this.editor.fire("beforecommand:composer");

    if (method) {
      args.unshift(this.composer);
      result = method.apply(obj, args);
    } else {
      try {
        // try/catch for buggy firefox
        result = this.doc.execCommand(command, false, value);
      } catch(e) {}
    }

    this.editor.fire("aftercommand:composer");
    return result;
  },

  /**
   * Check whether the current command is active
   * If the caret is within a bold text, then calling this with command "bold" should return true
   *
   * @param {String} command The command string which to check (eg. "bold", "italic", "insertUnorderedList")
   * @param {String} [commandValue] The command value parameter (eg. for "insertImage" the image src)
   * @return {Boolean} Whether the command is active
   * @example
   *    var isCurrentSelectionBold = commands.state("bold");
   */
  state: function(command, commandValue) {
    var obj     = wysihtml5.commands[command],
        args    = wysihtml5.lang.array(arguments).get(),
        method  = obj && obj.state;
    if (method) {
      args.unshift(this.composer);
      return method.apply(obj, args);
    } else {
      try {
        // try/catch for buggy firefox
        return this.doc.queryCommandState(command);
      } catch(e) {
        return false;
      }
    }
  },

  /* Get command state parsed value if command has stateValue parsing function */
  stateValue: function(command) {
    var obj     = wysihtml5.commands[command],
        args    = wysihtml5.lang.array(arguments).get(),
        method  = obj && obj.stateValue;
    if (method) {
      args.unshift(this.composer);
      return method.apply(obj, args);
    } else {
      return false;
    }
  }
});
;wysihtml5.commands.bold = {
  exec: function(composer, command) {
    wysihtml5.commands.formatInline.execWithToggle(composer, command, "b");
  },

  state: function(composer, command) {
    // element.ownerDocument.queryCommandState("bold") results:
    // firefox: only <b>
    // chrome:  <b>, <strong>, <h1>, <h2>, ...
    // ie:      <b>, <strong>
    // opera:   <b>, <strong>
    return wysihtml5.commands.formatInline.state(composer, command, "b");
  }
};

;(function(wysihtml5) {
  var undef,
      NODE_NAME = "A",
      dom       = wysihtml5.dom;

  function _format(composer, attributes) {
    var doc             = composer.doc,
        tempClass       = "_wysihtml5-temp-" + (+new Date()),
        tempClassRegExp = /non-matching-class/g,
        i               = 0,
        length,
        anchors,
        anchor,
        hasElementChild,
        isEmpty,
        elementToSetCaretAfter,
        textContent,
        whiteSpace,
        j;
    wysihtml5.commands.formatInline.exec(composer, undef, NODE_NAME, tempClass, tempClassRegExp, undef, undef, true, true);
    anchors = doc.querySelectorAll(NODE_NAME + "." + tempClass);
    length  = anchors.length;
    for (; i<length; i++) {
      anchor = anchors[i];
      anchor.removeAttribute("class");
      for (j in attributes) {
        // Do not set attribute "text" as it is meant for setting string value if created link has no textual data
        if (j !== "text") {
          anchor.setAttribute(j, attributes[j]);
        }
      }
    }

    elementToSetCaretAfter = anchor;
    if (length === 1) {
      textContent = dom.getTextContent(anchor);
      hasElementChild = !!anchor.querySelector("*");
      isEmpty = textContent === "" || textContent === wysihtml5.INVISIBLE_SPACE;
      if (!hasElementChild && isEmpty) {
        dom.setTextContent(anchor, attributes.text || anchor.href);
        whiteSpace = doc.createTextNode(" ");
        composer.selection.setAfter(anchor);
        dom.insert(whiteSpace).after(anchor);
        elementToSetCaretAfter = whiteSpace;
      }
    }
    composer.selection.setAfter(elementToSetCaretAfter);
  }

  // Changes attributes of links
  function _changeLinks(composer, anchors, attributes) {
    var oldAttrs;
    for (var a = anchors.length; a--;) {

      // Remove all old attributes
      oldAttrs = anchors[a].attributes;
      for (var oa = oldAttrs.length; oa--;) {
        anchors[a].removeAttribute(oldAttrs.item(oa).name);
      }

      // Set new attributes
      for (var j in attributes) {
        if (attributes.hasOwnProperty(j)) {
          anchors[a].setAttribute(j, attributes[j]);
        }
      }

    }
  }

  wysihtml5.commands.createLink = {
    /**
     * TODO: Use HTMLApplier or formatInline here
     *
     * Turns selection into a link
     * If selection is already a link, it just changes the attributes
     *
     * @example
     *    // either ...
     *    wysihtml5.commands.createLink.exec(composer, "createLink", "http://www.google.de");
     *    // ... or ...
     *    wysihtml5.commands.createLink.exec(composer, "createLink", { href: "http://www.google.de", target: "_blank" });
     */
    exec: function(composer, command, value) {
      var anchors = this.state(composer, command);
      if (anchors) {
        // Selection contains links then change attributes of these links
        composer.selection.executeAndRestore(function() {
          _changeLinks(composer, anchors, value);
        });
      } else {
        // Create links
        value = typeof(value) === "object" ? value : { href: value };
        _format(composer, value);
      }
    },

    state: function(composer, command) {
      return wysihtml5.commands.formatInline.state(composer, command, "A");
    }
  };
})(wysihtml5);
;(function(wysihtml5) {
  var dom = wysihtml5.dom;

  function _removeFormat(composer, anchors) {
    var length  = anchors.length,
        i       = 0,
        anchor,
        codeElement,
        textContent;
    for (; i<length; i++) {
      anchor      = anchors[i];
      codeElement = dom.getParentElement(anchor, { nodeName: "code" });
      textContent = dom.getTextContent(anchor);

      // if <a> contains url-like text content, rename it to <code> to prevent re-autolinking
      // else replace <a> with its childNodes
      if (textContent.match(dom.autoLink.URL_REG_EXP) && !codeElement) {
        // <code> element is used to prevent later auto-linking of the content
        codeElement = dom.renameElement(anchor, "code");
      } else {
        dom.replaceWithChildNodes(anchor);
      }
    }
  }

  wysihtml5.commands.removeLink = {
    /*
     * If selection is a link, it removes the link and wraps it with a <code> element
     * The <code> element is needed to avoid auto linking
     *
     * @example
     *    wysihtml5.commands.createLink.exec(composer, "removeLink");
     */

    exec: function(composer, command) {
      var anchors = this.state(composer, command);
      if (anchors) {
        composer.selection.executeAndRestore(function() {
          _removeFormat(composer, anchors);
        });
      }
    },

    state: function(composer, command) {
      return wysihtml5.commands.formatInline.state(composer, command, "A");
    }
  };
})(wysihtml5);
;/**
 * document.execCommand("fontSize") will create either inline styles (firefox, chrome) or use font tags
 * which we don't want
 * Instead we set a css class
 */
(function(wysihtml5) {
  var REG_EXP = /wysiwyg-font-size-[0-9a-z\-]+/g;

  wysihtml5.commands.fontSize = {
    exec: function(composer, command, size) {
        wysihtml5.commands.formatInline.execWithToggle(composer, command, "span", "wysiwyg-font-size-" + size, REG_EXP);
    },

    state: function(composer, command, size) {
      return wysihtml5.commands.formatInline.state(composer, command, "span", "wysiwyg-font-size-" + size, REG_EXP);
    }
  };
})(wysihtml5);
;/* In case font size adjustment to any number defined by user is preferred, we cannot use classes and must use inline styles. */
(function(wysihtml5) {
  var REG_EXP = /(\s|^)font-size\s*:\s*[^;\s]+;?/gi;

  wysihtml5.commands.fontSizeStyle = {
    exec: function(composer, command, size) {
      size = (typeof(size) == "object") ? size.size : size;
      if (!(/^\s*$/).test(size)) {
        wysihtml5.commands.formatInline.execWithToggle(composer, command, "span", false, false, "font-size:" + size, REG_EXP);
      }
    },

    state: function(composer, command, size) {
      return wysihtml5.commands.formatInline.state(composer, command, "span", false, false, "font-size", REG_EXP);
    },

    stateValue: function(composer, command) {
      var st = this.state(composer, command),
          styleStr, fontsizeMatches,
          val = false;

      if (st && wysihtml5.lang.object(st).isArray()) {
          st = st[0];
      }
      if (st) {
        styleStr = st.getAttribute('style');
        if (styleStr) {
          return wysihtml5.quirks.styleParser.parseFontSize(styleStr);
        }
      }
      return false;
    }
  };
})(wysihtml5);
;/**
 * document.execCommand("foreColor") will create either inline styles (firefox, chrome) or use font tags
 * which we don't want
 * Instead we set a css class
 */
(function(wysihtml5) {
  var REG_EXP = /wysiwyg-color-[0-9a-z]+/g;

  wysihtml5.commands.foreColor = {
    exec: function(composer, command, color) {
        wysihtml5.commands.formatInline.execWithToggle(composer, command, "span", "wysiwyg-color-" + color, REG_EXP);
    },

    state: function(composer, command, color) {
      return wysihtml5.commands.formatInline.state(composer, command, "span", "wysiwyg-color-" + color, REG_EXP);
    }
  };
})(wysihtml5);
;/**
 * document.execCommand("foreColor") will create either inline styles (firefox, chrome) or use font tags
 * which we don't want
 * Instead we set a css class
 */
(function(wysihtml5) {
  var REG_EXP = /(\s|^)color\s*:\s*[^;\s]+;?/gi;

  wysihtml5.commands.foreColorStyle = {
    exec: function(composer, command, color) {
      var colorVals  = wysihtml5.quirks.styleParser.parseColor((typeof(color) == "object") ? "color:" + color.color : "color:" + color, "color"),
          colString;

      if (colorVals) {
        colString = "color: rgb(" + colorVals[0] + ',' + colorVals[1] + ',' + colorVals[2] + ');';
        if (colorVals[3] !== 1) {
          colString += "color: rgba(" + colorVals[0] + ',' + colorVals[1] + ',' + colorVals[2] + ',' + colorVals[3] + ');';
        }
        wysihtml5.commands.formatInline.execWithToggle(composer, command, "span", false, false, colString, REG_EXP);
      }
    },

    state: function(composer, command) {
      return wysihtml5.commands.formatInline.state(composer, command, "span", false, false, "color", REG_EXP);
    },

    stateValue: function(composer, command, props) {
      var st = this.state(composer, command),
          colorStr;

      if (st && wysihtml5.lang.object(st).isArray()) {
        st = st[0];
      }

      if (st) {
        colorStr = st.getAttribute('style');
        if (colorStr) {
          if (colorStr) {
            val = wysihtml5.quirks.styleParser.parseColor(colorStr, "color");
            return wysihtml5.quirks.styleParser.unparseColor(val, props);
          }
        }
      }
      return false;
    }

  };
})(wysihtml5);
;/* In case background adjustment to any color defined by user is preferred, we cannot use classes and must use inline styles. */
(function(wysihtml5) {
  var REG_EXP = /(\s|^)background-color\s*:\s*[^;\s]+;?/gi;

  wysihtml5.commands.bgColorStyle = {
    exec: function(composer, command, color) {
      var colorVals  = wysihtml5.quirks.styleParser.parseColor((typeof(color) == "object") ? "background-color:" + color.color : "background-color:" + color, "background-color"),
          colString;

      if (colorVals) {
        colString = "background-color: rgb(" + colorVals[0] + ',' + colorVals[1] + ',' + colorVals[2] + ');';
        if (colorVals[3] !== 1) {
          colString += "background-color: rgba(" + colorVals[0] + ',' + colorVals[1] + ',' + colorVals[2] + ',' + colorVals[3] + ');';
        }
        wysihtml5.commands.formatInline.execWithToggle(composer, command, "span", false, false, colString, REG_EXP);
      }
    },

    state: function(composer, command) {
      return wysihtml5.commands.formatInline.state(composer, command, "span", false, false, "background-color", REG_EXP);
    },

    stateValue: function(composer, command, props) {
      var st = this.state(composer, command),
          colorStr,
          val = false;

      if (st && wysihtml5.lang.object(st).isArray()) {
        st = st[0];
      }

      if (st) {
        colorStr = st.getAttribute('style');
        if (colorStr) {
          val = wysihtml5.quirks.styleParser.parseColor(colorStr, "background-color");
          return wysihtml5.quirks.styleParser.unparseColor(val, props);
        }
      }
      return false;
    }

  };
})(wysihtml5);
;(function(wysihtml5) {
  var dom                     = wysihtml5.dom,
      // Following elements are grouped
      // when the caret is within a H1 and the H4 is invoked, the H1 should turn into H4
      // instead of creating a H4 within a H1 which would result in semantically invalid html
      BLOCK_ELEMENTS_GROUP    = ["H1", "H2", "H3", "H4", "H5", "H6", "P", "PRE", "DIV"];

  /**
   * Remove similiar classes (based on classRegExp)
   * and add the desired class name
   */
  function _addClass(element, className, classRegExp) {
    if (element.className) {
      _removeClass(element, classRegExp);
      element.className = wysihtml5.lang.string(element.className + " " + className).trim();
    } else {
      element.className = className;
    }
  }

  function _addStyle(element, cssStyle, styleRegExp) {
    _removeStyle(element, styleRegExp);
    if (element.getAttribute('style')) {
      element.setAttribute('style', wysihtml5.lang.string(element.getAttribute('style') + " " + cssStyle).trim());
    } else {
      element.setAttribute('style', cssStyle);
    }
  }

  function _removeClass(element, classRegExp) {
    var ret = classRegExp.test(element.className);
    element.className = element.className.replace(classRegExp, "");
    if (wysihtml5.lang.string(element.className).trim() == '') {
        element.removeAttribute('class');
    }
    return ret;
  }

  function _removeStyle(element, styleRegExp) {
    var ret = styleRegExp.test(element.getAttribute('style'));
    element.setAttribute('style', (element.getAttribute('style') || "").replace(styleRegExp, ""));
    if (wysihtml5.lang.string(element.getAttribute('style') || "").trim() == '') {
      element.removeAttribute('style');
    }
    return ret;
  }

  function _removeLastChildIfLineBreak(node) {
    var lastChild = node.lastChild;
    if (lastChild && _isLineBreak(lastChild)) {
      lastChild.parentNode.removeChild(lastChild);
    }
  }

  function _isLineBreak(node) {
    return node.nodeName === "BR";
  }

  /**
   * Execute native query command
   * and if necessary modify the inserted node's className
   */
  function _execCommand(doc, composer, command, nodeName, className) {
    var ranges = composer.selection.getOwnRanges();
    for (var i = ranges.length; i--;){
      composer.selection.getSelection().removeAllRanges();
      composer.selection.setSelection(ranges[i]);
      if (className) {
        var eventListener = dom.observe(doc, "DOMNodeInserted", function(event) {
          var target = event.target,
              displayStyle;
          if (target.nodeType !== wysihtml5.ELEMENT_NODE) {
            return;
          }
          displayStyle = dom.getStyle("display").from(target);
          if (displayStyle.substr(0, 6) !== "inline") {
            // Make sure that only block elements receive the given class
            target.className += " " + className;
          }
        });
      }
      doc.execCommand(command, false, nodeName);

      if (eventListener) {
        eventListener.stop();
      }
    }
  }

  function _selectionWrap(composer, options) {
    if (composer.selection.isCollapsed()) {
        composer.selection.selectLine();
    }

    var surroundedNodes = composer.selection.surround(options);
    for (var i = 0, imax = surroundedNodes.length; i < imax; i++) {
      wysihtml5.dom.lineBreaks(surroundedNodes[i]).remove();
      _removeLastChildIfLineBreak(surroundedNodes[i]);
    }

    // rethink restoring selection
    // composer.selection.selectNode(element, wysihtml5.browser.displaysCaretInEmptyContentEditableCorrectly());
  }

  function _hasClasses(element) {
    return !!wysihtml5.lang.string(element.className).trim();
  }

  function _hasStyles(element) {
    return !!wysihtml5.lang.string(element.getAttribute('style') || '').trim();
  }

  wysihtml5.commands.formatBlock = {
    exec: function(composer, command, nodeName, className, classRegExp, cssStyle, styleRegExp) {
      var doc             = composer.doc,
          blockElements    = this.state(composer, command, nodeName, className, classRegExp, cssStyle, styleRegExp),
          useLineBreaks   = composer.config.useLineBreaks,
          defaultNodeName = useLineBreaks ? "DIV" : "P",
          selectedNodes, classRemoveAction, blockRenameFound, styleRemoveAction, blockElement;
      nodeName = typeof(nodeName) === "string" ? nodeName.toUpperCase() : nodeName;

      if (blockElements.length) {
        composer.selection.executeAndRestoreRangy(function() {
          for (var b = blockElements.length; b--;) {
            if (classRegExp) {
              classRemoveAction = _removeClass(blockElements[b], classRegExp);
            }
            if (styleRegExp) {
              styleRemoveAction = _removeStyle(blockElements[b], styleRegExp);
            }

            if ((styleRemoveAction || classRemoveAction) && nodeName === null && blockElements[b].nodeName != defaultNodeName) {
              // dont rename or remove element when just setting block formating class or style
              return;
            }

            var hasClasses = _hasClasses(blockElements[b]),
                hasStyles = _hasStyles(blockElements[b]);

            if (!hasClasses && !hasStyles && (useLineBreaks || nodeName === "P")) {
              // Insert a line break afterwards and beforewards when there are siblings
              // that are not of type line break or block element
              wysihtml5.dom.lineBreaks(blockElements[b]).add();
              dom.replaceWithChildNodes(blockElements[b]);
            } else {
              // Make sure that styling is kept by renaming the element to a <div> or <p> and copying over the class name
              dom.renameElement(blockElements[b], nodeName === "P" ? "DIV" : defaultNodeName);
            }
          }
        });

        return;
      }

      // Find similiar block element and rename it (<h2 class="foo"></h2>  =>  <h1 class="foo"></h1>)
      if (nodeName === null || wysihtml5.lang.array(BLOCK_ELEMENTS_GROUP).contains(nodeName)) {
        selectedNodes = composer.selection.findNodesInSelection(BLOCK_ELEMENTS_GROUP).concat(composer.selection.getSelectedOwnNodes());
        composer.selection.executeAndRestoreRangy(function() {
          for (var n = selectedNodes.length; n--;) {
            blockElement = dom.getParentElement(selectedNodes[n], {
              nodeName: BLOCK_ELEMENTS_GROUP
            });
            if (blockElement == composer.element) {
              blockElement = null;
            }
            if (blockElement) {
                // Rename current block element to new block element and add class
                if (nodeName) {
                  blockElement = dom.renameElement(blockElement, nodeName);
                }
                if (className) {
                  _addClass(blockElement, className, classRegExp);
                }
                if (cssStyle) {
                  _addStyle(blockElement, cssStyle, styleRegExp);
                }
              blockRenameFound = true;
            }
          }

        });

        if (blockRenameFound) {
          return;
        }
      }

      _selectionWrap(composer, {
        "nodeName": (nodeName || defaultNodeName),
        "className": className || null,
        "cssStyle": cssStyle || null
      });
    },

    state: function(composer, command, nodeName, className, classRegExp, cssStyle, styleRegExp) {
      var nodes = composer.selection.getSelectedOwnNodes(),
          parents = [],
          parent;

      nodeName = typeof(nodeName) === "string" ? nodeName.toUpperCase() : nodeName;

      //var selectedNode = composer.selection.getSelectedNode();
      for (var i = 0, maxi = nodes.length; i < maxi; i++) {
        parent = dom.getParentElement(nodes[i], {
          nodeName:     nodeName,
          className:    className,
          classRegExp:  classRegExp,
          cssStyle:     cssStyle,
          styleRegExp:  styleRegExp
        });
        if (parent && wysihtml5.lang.array(parents).indexOf(parent) == -1) {
          parents.push(parent);
        }
      }
      if (parents.length == 0) {
        return false;
      }
      return parents;
    }


  };
})(wysihtml5);
;/* Formats block for as a <pre><code class="classname"></code></pre> block
 * Useful in conjuction for sytax highlight utility: highlight.js
 *
 * Usage:
 *
 * editorInstance.composer.commands.exec("formatCode", "language-html");
*/

wysihtml5.commands.formatCode = {

  exec: function(composer, command, classname) {
    var pre = this.state(composer),
        code, range, selectedNodes;
    if (pre) {
      // caret is already within a <pre><code>...</code></pre>
      composer.selection.executeAndRestore(function() {
        code = pre.querySelector("code");
        wysihtml5.dom.replaceWithChildNodes(pre);
        if (code) {
          wysihtml5.dom.replaceWithChildNodes(code);
        }
      });
    } else {
      // Wrap in <pre><code>...</code></pre>
      range = composer.selection.getRange();
      selectedNodes = range.extractContents();
      pre = composer.doc.createElement("pre");
      code = composer.doc.createElement("code");

      if (classname) {
        code.className = classname;
      }

      pre.appendChild(code);
      code.appendChild(selectedNodes);
      range.insertNode(pre);
      composer.selection.selectNode(pre);
    }
  },

  state: function(composer) {
    var selectedNode = composer.selection.getSelectedNode();
    if (selectedNode && selectedNode.nodeName && selectedNode.nodeName == "PRE"&&
        selectedNode.firstChild && selectedNode.firstChild.nodeName && selectedNode.firstChild.nodeName == "CODE") {
      return selectedNode;
    } else {
      return wysihtml5.dom.getParentElement(selectedNode, { nodeName: "CODE" }) && wysihtml5.dom.getParentElement(selectedNode, { nodeName: "PRE" });
    }
  }
};;/**
 * formatInline scenarios for tag "B" (| = caret, |foo| = selected text)
 *
 *   #1 caret in unformatted text:
 *      abcdefg|
 *   output:
 *      abcdefg<b>|</b>
 *
 *   #2 unformatted text selected:
 *      abc|deg|h
 *   output:
 *      abc<b>|deg|</b>h
 *
 *   #3 unformatted text selected across boundaries:
 *      ab|c <span>defg|h</span>
 *   output:
 *      ab<b>|c </b><span><b>defg</b>|h</span>
 *
 *   #4 formatted text entirely selected
 *      <b>|abc|</b>
 *   output:
 *      |abc|
 *
 *   #5 formatted text partially selected
 *      <b>ab|c|</b>
 *   output:
 *      <b>ab</b>|c|
 *
 *   #6 formatted text selected across boundaries
 *      <span>ab|c</span> <b>de|fgh</b>
 *   output:
 *      <span>ab|c</span> de|<b>fgh</b>
 */
(function(wysihtml5) {
  var // Treat <b> as <strong> and vice versa
      ALIAS_MAPPING = {
        "strong": "b",
        "em":     "i",
        "b":      "strong",
        "i":      "em"
      },
      htmlApplier = {};

  function _getTagNames(tagName) {
    var alias = ALIAS_MAPPING[tagName];
    return alias ? [tagName.toLowerCase(), alias.toLowerCase()] : [tagName.toLowerCase()];
  }

  function _getApplier(tagName, className, classRegExp, cssStyle, styleRegExp, container) {
    var identifier = tagName;
    
    if (className) {
      identifier += ":" + className;
    }
    if (cssStyle) {
      identifier += ":" + cssStyle;
    }

    if (!htmlApplier[identifier]) {
      htmlApplier[identifier] = new wysihtml5.selection.HTMLApplier(_getTagNames(tagName), className, classRegExp, true, cssStyle, styleRegExp, container);
    }

    return htmlApplier[identifier];
  }

  wysihtml5.commands.formatInline = {
    exec: function(composer, command, tagName, className, classRegExp, cssStyle, styleRegExp, dontRestoreSelect, noCleanup) {
      var range = composer.selection.createRange(),
          ownRanges = composer.selection.getOwnRanges();

      if (!ownRanges || ownRanges.length == 0) {
        return false;
      }
      composer.selection.getSelection().removeAllRanges();

      _getApplier(tagName, className, classRegExp, cssStyle, styleRegExp, composer.element).toggleRange(ownRanges);

      if (!dontRestoreSelect) {
        range.setStart(ownRanges[0].startContainer,  ownRanges[0].startOffset);
        range.setEnd(
          ownRanges[ownRanges.length - 1].endContainer,
          ownRanges[ownRanges.length - 1].endOffset
        );
        composer.selection.setSelection(range);
        composer.selection.executeAndRestore(function() {
          if (!noCleanup) {
            composer.cleanUp();
          }
        }, true, true);
      } else if (!noCleanup) {
        composer.cleanUp();
      }
    },

    // Executes so that if collapsed caret is in a state and executing that state it should unformat that state
    // It is achieved by selecting the entire state element before executing.
    // This works on built in contenteditable inline format commands
    execWithToggle: function(composer, command, tagName, className, classRegExp, cssStyle, styleRegExp) {
      var that = this;

      if (this.state(composer, command, tagName, className, classRegExp, cssStyle, styleRegExp) &&
        composer.selection.isCollapsed() &&
        !composer.selection.caretIsLastInSelection() &&
        !composer.selection.caretIsFirstInSelection()
      ) {
        var state_element = that.state(composer, command, tagName, className, classRegExp)[0];
        composer.selection.executeAndRestoreRangy(function() {
          var parent = state_element.parentNode;
          composer.selection.selectNode(state_element, true);
          wysihtml5.commands.formatInline.exec(composer, command, tagName, className, classRegExp, cssStyle, styleRegExp, true, true);
        });
      } else {
        if (this.state(composer, command, tagName, className, classRegExp, cssStyle, styleRegExp) && !composer.selection.isCollapsed()) {
          composer.selection.executeAndRestoreRangy(function() {
            wysihtml5.commands.formatInline.exec(composer, command, tagName, className, classRegExp, cssStyle, styleRegExp, true, true);
          });
        } else {
          wysihtml5.commands.formatInline.exec(composer, command, tagName, className, classRegExp, cssStyle, styleRegExp);
        }
      }
    },

    state: function(composer, command, tagName, className, classRegExp, cssStyle, styleRegExp) {
      var doc           = composer.doc,
          aliasTagName  = ALIAS_MAPPING[tagName] || tagName,
          ownRanges, isApplied;

      // Check whether the document contains a node with the desired tagName
      if (!wysihtml5.dom.hasElementWithTagName(doc, tagName) &&
          !wysihtml5.dom.hasElementWithTagName(doc, aliasTagName)) {
        return false;
      }

       // Check whether the document contains a node with the desired className
      if (className && !wysihtml5.dom.hasElementWithClassName(doc, className)) {
         return false;
      }

      ownRanges = composer.selection.getOwnRanges();

      if (!ownRanges || ownRanges.length === 0) {
        return false;
      }

      isApplied = _getApplier(tagName, className, classRegExp, cssStyle, styleRegExp, composer.element).isAppliedToRange(ownRanges);

      return (isApplied && isApplied.elements) ? isApplied.elements : false;
    }
  };
})(wysihtml5);
;(function(wysihtml5) {

  wysihtml5.commands.insertBlockQuote = {
    exec: function(composer, command) {
      var state = this.state(composer, command),
          endToEndParent = composer.selection.isEndToEndInNode(['H1', 'H2', 'H3', 'H4', 'H5', 'H6', 'P']),
          prevNode, nextNode;

      composer.selection.executeAndRestore(function() {
        if (state) {
          if (composer.config.useLineBreaks) {
             wysihtml5.dom.lineBreaks(state).add();
          }
          wysihtml5.dom.unwrap(state);
        } else {
          if (composer.selection.isCollapsed()) {
            composer.selection.selectLine();
          }
          
          if (endToEndParent) {
            var qouteEl = endToEndParent.ownerDocument.createElement('blockquote');
            wysihtml5.dom.insert(qouteEl).after(endToEndParent);
            qouteEl.appendChild(endToEndParent);
          } else {
            composer.selection.surround({nodeName: "blockquote"});
          }
        }
      });
    },
    state: function(composer, command) {
      var selectedNode  = composer.selection.getSelectedNode(),
          node = wysihtml5.dom.getParentElement(selectedNode, { nodeName: "BLOCKQUOTE" }, false, composer.element);

      return (node) ? node : false;
    }
  };

})(wysihtml5);;wysihtml5.commands.insertHTML = {
  exec: function(composer, command, html) {
    if (composer.commands.support(command)) {
      composer.doc.execCommand(command, false, html);
    } else {
      composer.selection.insertHTML(html);
    }
  },

  state: function() {
    return false;
  }
};
;(function(wysihtml5) {
  var NODE_NAME = "IMG";

  wysihtml5.commands.insertImage = {
    /**
     * Inserts an <img>
     * If selection is already an image link, it removes it
     *
     * @example
     *    // either ...
     *    wysihtml5.commands.insertImage.exec(composer, "insertImage", "http://www.google.de/logo.jpg");
     *    // ... or ...
     *    wysihtml5.commands.insertImage.exec(composer, "insertImage", { src: "http://www.google.de/logo.jpg", title: "foo" });
     */
    exec: function(composer, command, value) {
      value = typeof(value) === "object" ? value : { src: value };

      var doc     = composer.doc,
          image   = this.state(composer),
          textNode,
          parent;

      if (image) {
        // Image already selected, set the caret before it and delete it
        composer.selection.setBefore(image);
        parent = image.parentNode;
        parent.removeChild(image);

        // and it's parent <a> too if it hasn't got any other relevant child nodes
        wysihtml5.dom.removeEmptyTextNodes(parent);
        if (parent.nodeName === "A" && !parent.firstChild) {
          composer.selection.setAfter(parent);
          parent.parentNode.removeChild(parent);
        }

        // firefox and ie sometimes don't remove the image handles, even though the image got removed
        wysihtml5.quirks.redraw(composer.element);
        return;
      }

      image = doc.createElement(NODE_NAME);

      for (var i in value) {
        image.setAttribute(i === "className" ? "class" : i, value[i]);
      }

      composer.selection.insertNode(image);
      if (wysihtml5.browser.hasProblemsSettingCaretAfterImg()) {
        textNode = doc.createTextNode(wysihtml5.INVISIBLE_SPACE);
        composer.selection.insertNode(textNode);
        composer.selection.setAfter(textNode);
      } else {
        composer.selection.setAfter(image);
      }
    },

    state: function(composer) {
      var doc = composer.doc,
          selectedNode,
          text,
          imagesInSelection;

      if (!wysihtml5.dom.hasElementWithTagName(doc, NODE_NAME)) {
        return false;
      }

      selectedNode = composer.selection.getSelectedNode();
      if (!selectedNode) {
        return false;
      }

      if (selectedNode.nodeName === NODE_NAME) {
        // This works perfectly in IE
        return selectedNode;
      }

      if (selectedNode.nodeType !== wysihtml5.ELEMENT_NODE) {
        return false;
      }

      text = composer.selection.getText();
      text = wysihtml5.lang.string(text).trim();
      if (text) {
        return false;
      }

      imagesInSelection = composer.selection.getNodes(wysihtml5.ELEMENT_NODE, function(node) {
        return node.nodeName === "IMG";
      });

      if (imagesInSelection.length !== 1) {
        return false;
      }

      return imagesInSelection[0];
    }
  };
})(wysihtml5);
;(function(wysihtml5) {
  var LINE_BREAK = "<br>" + (wysihtml5.browser.needsSpaceAfterLineBreak() ? " " : "");

  wysihtml5.commands.insertLineBreak = {
    exec: function(composer, command) {
      if (composer.commands.support(command)) {
        composer.doc.execCommand(command, false, null);
        if (!wysihtml5.browser.autoScrollsToCaret()) {
          composer.selection.scrollIntoView();
        }
      } else {
        composer.commands.exec("insertHTML", LINE_BREAK);
      }
    },

    state: function() {
      return false;
    }
  };
})(wysihtml5);
;wysihtml5.commands.insertOrderedList = {
  exec: function(composer, command) {
    wysihtml5.commands.insertList.exec(composer, command, "OL");
  },

  state: function(composer, command) {
    return wysihtml5.commands.insertList.state(composer, command, "OL");
  }
};
;wysihtml5.commands.insertUnorderedList = {
  exec: function(composer, command) {
    wysihtml5.commands.insertList.exec(composer, command, "UL");
  },

  state: function(composer, command) {
    return wysihtml5.commands.insertList.state(composer, command, "UL");
  }
};
;wysihtml5.commands.insertList = (function(wysihtml5) {

  var isNode = function(node, name) {
    if (node && node.nodeName) {
      if (typeof name === 'string') {
        name = [name];
      }
      for (var n = name.length; n--;) {
        if (node.nodeName === name[n]) {
          return true;
        }
      }
    }
    return false;
  };

  var findListEl = function(node, nodeName, composer) {
    var ret = {
          el: null,
          other: false
        };

    if (node) {
      var parentLi = wysihtml5.dom.getParentElement(node, { nodeName: "LI" }),
          otherNodeName = (nodeName === "UL") ? "OL" : "UL";

      if (isNode(node, nodeName)) {
        ret.el = node;
      } else if (isNode(node, otherNodeName)) {
        ret = {
          el: node,
          other: true
        };
      } else if (parentLi) {
        if (isNode(parentLi.parentNode, nodeName)) {
          ret.el = parentLi.parentNode;
        } else if (isNode(parentLi.parentNode, otherNodeName)) {
          ret = {
            el : parentLi.parentNode,
            other: true
          };
        }
      }
    }

    // do not count list elements outside of composer
    if (ret.el && !composer.element.contains(ret.el)) {
      ret.el = null;
    }

    return ret;
  };

  var handleSameTypeList = function(el, nodeName, composer) {
    var otherNodeName = (nodeName === "UL") ? "OL" : "UL",
        otherLists, innerLists;
    // Unwrap list
    // <ul><li>foo</li><li>bar</li></ul>
    // becomes:
    // foo<br>bar<br>
    composer.selection.executeAndRestore(function() {
      var otherLists = getListsInSelection(otherNodeName, composer);
      if (otherLists.length) {
        for (var l = otherLists.length; l--;) {
          wysihtml5.dom.renameElement(otherLists[l], nodeName.toLowerCase());
        }
      } else {
        innerLists = getListsInSelection(['OL', 'UL'], composer);
        for (var i = innerLists.length; i--;) {
          wysihtml5.dom.resolveList(innerLists[i], composer.config.useLineBreaks);
        }
        wysihtml5.dom.resolveList(el, composer.config.useLineBreaks);
      }
    });
  };

  var handleOtherTypeList =  function(el, nodeName, composer) {
    var otherNodeName = (nodeName === "UL") ? "OL" : "UL";
    // Turn an ordered list into an unordered list
    // <ol><li>foo</li><li>bar</li></ol>
    // becomes:
    // <ul><li>foo</li><li>bar</li></ul>
    // Also rename other lists in selection
    composer.selection.executeAndRestore(function() {
      var renameLists = [el].concat(getListsInSelection(otherNodeName, composer));

      // All selection inner lists get renamed too
      for (var l = renameLists.length; l--;) {
        wysihtml5.dom.renameElement(renameLists[l], nodeName.toLowerCase());
      }
    });
  };

  var getListsInSelection = function(nodeName, composer) {
      var ranges = composer.selection.getOwnRanges(),
          renameLists = [];

      for (var r = ranges.length; r--;) {
        renameLists = renameLists.concat(ranges[r].getNodes([1], function(node) {
          return isNode(node, nodeName);
        }));
      }

      return renameLists;
  };

  var createListFallback = function(nodeName, composer) {
    // Fallback for Create list
    composer.selection.executeAndRestoreRangy(function() {
      var tempClassName =  "_wysihtml5-temp-" + new Date().getTime(),
          tempElement = composer.selection.deblockAndSurround({
            "nodeName": "div",
            "className": tempClassName
          }),
          isEmpty, list;

      // This space causes new lists to never break on enter 
      var INVISIBLE_SPACE_REG_EXP = /\uFEFF/g;
      tempElement.innerHTML = tempElement.innerHTML.replace(INVISIBLE_SPACE_REG_EXP, "");
      
      if (tempElement) {
        isEmpty = wysihtml5.lang.array(["", "<br>", wysihtml5.INVISIBLE_SPACE]).contains(tempElement.innerHTML);
        list = wysihtml5.dom.convertToList(tempElement, nodeName.toLowerCase(), composer.parent.config.uneditableContainerClassname);
        if (isEmpty) {
          composer.selection.selectNode(list.querySelector("li"), true);
        }
      }
    });
  };

  return {
    exec: function(composer, command, nodeName) {
      var doc           = composer.doc,
          cmd           = (nodeName === "OL") ? "insertOrderedList" : "insertUnorderedList",
          selectedNode  = composer.selection.getSelectedNode(),
          list          = findListEl(selectedNode, nodeName, composer);

      if (!list.el) {
        if (composer.commands.support(cmd)) {
          doc.execCommand(cmd, false, null);
        } else {
          createListFallback(nodeName, composer);
        }
      } else if (list.other) {
        handleOtherTypeList(list.el, nodeName, composer);
      } else {
        handleSameTypeList(list.el, nodeName, composer);
      }
    },

    state: function(composer, command, nodeName) {
      var selectedNode = composer.selection.getSelectedNode(),
          list         = findListEl(selectedNode, nodeName, composer);

      return (list.el && !list.other) ? list.el : false;
    }
  };

})(wysihtml5);;wysihtml5.commands.italic = {
  exec: function(composer, command) {
    wysihtml5.commands.formatInline.execWithToggle(composer, command, "i");
  },

  state: function(composer, command) {
    // element.ownerDocument.queryCommandState("italic") results:
    // firefox: only <i>
    // chrome:  <i>, <em>, <blockquote>, ...
    // ie:      <i>, <em>
    // opera:   only <i>
    return wysihtml5.commands.formatInline.state(composer, command, "i");
  }
};
;(function(wysihtml5) {
  var CLASS_NAME  = "wysiwyg-text-align-center",
      REG_EXP     = /wysiwyg-text-align-[0-9a-z]+/g;

  wysihtml5.commands.justifyCenter = {
    exec: function(composer, command) {
      return wysihtml5.commands.formatBlock.exec(composer, "formatBlock", null, CLASS_NAME, REG_EXP);
    },

    state: function(composer, command) {
      return wysihtml5.commands.formatBlock.state(composer, "formatBlock", null, CLASS_NAME, REG_EXP);
    }
  };
})(wysihtml5);
;(function(wysihtml5) {
  var CLASS_NAME  = "wysiwyg-text-align-left",
      REG_EXP     = /wysiwyg-text-align-[0-9a-z]+/g;

  wysihtml5.commands.justifyLeft = {
    exec: function(composer, command) {
      return wysihtml5.commands.formatBlock.exec(composer, "formatBlock", null, CLASS_NAME, REG_EXP);
    },

    state: function(composer, command) {
      return wysihtml5.commands.formatBlock.state(composer, "formatBlock", null, CLASS_NAME, REG_EXP);
    }
  };
})(wysihtml5);
;(function(wysihtml5) {
  var CLASS_NAME  = "wysiwyg-text-align-right",
      REG_EXP     = /wysiwyg-text-align-[0-9a-z]+/g;

  wysihtml5.commands.justifyRight = {
    exec: function(composer, command) {
      return wysihtml5.commands.formatBlock.exec(composer, "formatBlock", null, CLASS_NAME, REG_EXP);
    },

    state: function(composer, command) {
      return wysihtml5.commands.formatBlock.state(composer, "formatBlock", null, CLASS_NAME, REG_EXP);
    }
  };
})(wysihtml5);
;(function(wysihtml5) {
  var CLASS_NAME  = "wysiwyg-text-align-justify",
      REG_EXP     = /wysiwyg-text-align-[0-9a-z]+/g;

  wysihtml5.commands.justifyFull = {
    exec: function(composer, command) {
      return wysihtml5.commands.formatBlock.exec(composer, "formatBlock", null, CLASS_NAME, REG_EXP);
    },

    state: function(composer, command) {
      return wysihtml5.commands.formatBlock.state(composer, "formatBlock", null, CLASS_NAME, REG_EXP);
    }
  };
})(wysihtml5);
;(function(wysihtml5) {
  var STYLE_STR  = "text-align: right;",
      REG_EXP = /(\s|^)text-align\s*:\s*[^;\s]+;?/gi;

  wysihtml5.commands.alignRightStyle = {
    exec: function(composer, command) {
      return wysihtml5.commands.formatBlock.exec(composer, "formatBlock", null, null, null, STYLE_STR, REG_EXP);
    },

    state: function(composer, command) {
      return wysihtml5.commands.formatBlock.state(composer, "formatBlock", null, null, null, STYLE_STR, REG_EXP);
    }
  };
})(wysihtml5);
;(function(wysihtml5) {
  var STYLE_STR  = "text-align: left;",
      REG_EXP = /(\s|^)text-align\s*:\s*[^;\s]+;?/gi;

  wysihtml5.commands.alignLeftStyle = {
    exec: function(composer, command) {
      return wysihtml5.commands.formatBlock.exec(composer, "formatBlock", null, null, null, STYLE_STR, REG_EXP);
    },

    state: function(composer, command) {
      return wysihtml5.commands.formatBlock.state(composer, "formatBlock", null, null, null, STYLE_STR, REG_EXP);
    }
  };
})(wysihtml5);
;(function(wysihtml5) {
  var STYLE_STR  = "text-align: center;",
      REG_EXP = /(\s|^)text-align\s*:\s*[^;\s]+;?/gi;

  wysihtml5.commands.alignCenterStyle = {
    exec: function(composer, command) {
      return wysihtml5.commands.formatBlock.exec(composer, "formatBlock", null, null, null, STYLE_STR, REG_EXP);
    },

    state: function(composer, command) {
      return wysihtml5.commands.formatBlock.state(composer, "formatBlock", null, null, null, STYLE_STR, REG_EXP);
    }
  };
})(wysihtml5);
;wysihtml5.commands.redo = {
  exec: function(composer) {
    return composer.undoManager.redo();
  },

  state: function(composer) {
    return false;
  }
};
;wysihtml5.commands.underline = {
  exec: function(composer, command) {
    wysihtml5.commands.formatInline.execWithToggle(composer, command, "u");
  },

  state: function(composer, command) {
    return wysihtml5.commands.formatInline.state(composer, command, "u");
  }
};
;wysihtml5.commands.undo = {
  exec: function(composer) {
    return composer.undoManager.undo();
  },

  state: function(composer) {
    return false;
  }
};
;wysihtml5.commands.createTable = {
  exec: function(composer, command, value) {
      var col, row, html;
      if (value && value.cols && value.rows && parseInt(value.cols, 10) > 0 && parseInt(value.rows, 10) > 0) {
          if (value.tableStyle) {
            html = "<table style=\"" + value.tableStyle + "\">";
          } else {
            html = "<table>";
          }
          html += "<tbody>";
          for (row = 0; row < value.rows; row ++) {
              html += '<tr>';
              for (col = 0; col < value.cols; col ++) {
                  html += "<td>&nbsp;</td>";
              }
              html += '</tr>';
          }
          html += "</tbody></table>";
          composer.commands.exec("insertHTML", html);
          //composer.selection.insertHTML(html);
      }


  },

  state: function(composer, command) {
      return false;
  }
};
;wysihtml5.commands.mergeTableCells = {
  exec: function(composer, command) {
      if (composer.tableSelection && composer.tableSelection.start && composer.tableSelection.end) {
          if (this.state(composer, command)) {
              wysihtml5.dom.table.unmergeCell(composer.tableSelection.start);
          } else {
              wysihtml5.dom.table.mergeCellsBetween(composer.tableSelection.start, composer.tableSelection.end);
          }
      }
  },

  state: function(composer, command) {
      if (composer.tableSelection) {
          var start = composer.tableSelection.start,
              end = composer.tableSelection.end;
          if (start && end && start == end &&
              ((
                  wysihtml5.dom.getAttribute(start, "colspan") &&
                  parseInt(wysihtml5.dom.getAttribute(start, "colspan"), 10) > 1
              ) || (
                  wysihtml5.dom.getAttribute(start, "rowspan") &&
                  parseInt(wysihtml5.dom.getAttribute(start, "rowspan"), 10) > 1
              ))
          ) {
              return [start];
          }
      }
      return false;
  }
};
;wysihtml5.commands.addTableCells = {
  exec: function(composer, command, value) {
      if (composer.tableSelection && composer.tableSelection.start && composer.tableSelection.end) {

          // switches start and end if start is bigger than end (reverse selection)
          var tableSelect = wysihtml5.dom.table.orderSelectionEnds(composer.tableSelection.start, composer.tableSelection.end);
          if (value == "before" || value == "above") {
              wysihtml5.dom.table.addCells(tableSelect.start, value);
          } else if (value == "after" || value == "below") {
              wysihtml5.dom.table.addCells(tableSelect.end, value);
          }
          setTimeout(function() {
              composer.tableSelection.select(tableSelect.start, tableSelect.end);
          },0);
      }
  },

  state: function(composer, command) {
      return false;
  }
};
;wysihtml5.commands.deleteTableCells = {
  exec: function(composer, command, value) {
      if (composer.tableSelection && composer.tableSelection.start && composer.tableSelection.end) {
          var tableSelect = wysihtml5.dom.table.orderSelectionEnds(composer.tableSelection.start, composer.tableSelection.end),
              idx = wysihtml5.dom.table.indexOf(tableSelect.start),
              selCell,
              table = composer.tableSelection.table;

          wysihtml5.dom.table.removeCells(tableSelect.start, value);
          setTimeout(function() {
              // move selection to next or previous if not present
              selCell = wysihtml5.dom.table.findCell(table, idx);

              if (!selCell){
                  if (value == "row") {
                      selCell = wysihtml5.dom.table.findCell(table, {
                          "row": idx.row - 1,
                          "col": idx.col
                      });
                  }

                  if (value == "column") {
                      selCell = wysihtml5.dom.table.findCell(table, {
                          "row": idx.row,
                          "col": idx.col - 1
                      });
                  }
              }
              if (selCell) {
                  composer.tableSelection.select(selCell, selCell);
              }
          }, 0);

      }
  },

  state: function(composer, command) {
      return false;
  }
};
;wysihtml5.commands.indentList = {
  exec: function(composer, command, value) {
    var listEls = composer.selection.getSelectionParentsByTag('LI');
    if (listEls) {
      return this.tryToPushLiLevel(listEls, composer.selection);
    }
    return false;
  },

  state: function(composer, command) {
      return false;
  },

  tryToPushLiLevel: function(liNodes, selection) {
    var listTag, list, prevLi, liNode, prevLiList,
        found = false;

    selection.executeAndRestoreRangy(function() {

      for (var i = liNodes.length; i--;) {
        liNode = liNodes[i];
        listTag = (liNode.parentNode.nodeName === 'OL') ? 'OL' : 'UL';
        list = liNode.ownerDocument.createElement(listTag);
        prevLi = wysihtml5.dom.domNode(liNode).prev({nodeTypes: [wysihtml5.ELEMENT_NODE]});
        prevLiList = (prevLi) ? prevLi.querySelector('ul, ol') : null;

        if (prevLi) {
          if (prevLiList) {
            prevLiList.appendChild(liNode);
          } else {
            list.appendChild(liNode);
            prevLi.appendChild(list);
          }
          found = true;
        }
      }

    });
    return found;
  }
};
;wysihtml5.commands.outdentList = {
  exec: function(composer, command, value) {
    var listEls = composer.selection.getSelectionParentsByTag('LI');
    if (listEls) {
      return this.tryToPullLiLevel(listEls, composer);
    }
    return false;
  },

  state: function(composer, command) {
      return false;
  },

  tryToPullLiLevel: function(liNodes, composer) {
    var listNode, outerListNode, outerLiNode, list, prevLi, liNode, afterList,
        found = false,
        that = this;

    composer.selection.executeAndRestoreRangy(function() {

      for (var i = liNodes.length; i--;) {
        liNode = liNodes[i];
        if (liNode.parentNode) {
          listNode = liNode.parentNode;

          if (listNode.tagName === 'OL' || listNode.tagName === 'UL') {
            found = true;

            outerListNode = wysihtml5.dom.getParentElement(listNode.parentNode, { nodeName: ['OL', 'UL']}, false, composer.element);
            outerLiNode = wysihtml5.dom.getParentElement(listNode.parentNode, { nodeName: ['LI']}, false, composer.element);

            if (outerListNode && outerLiNode) {

              if (liNode.nextSibling) {
                afterList = that.getAfterList(listNode, liNode);
                liNode.appendChild(afterList);
              }
              outerListNode.insertBefore(liNode, outerLiNode.nextSibling);

            } else {

              if (liNode.nextSibling) {
                afterList = that.getAfterList(listNode, liNode);
                liNode.appendChild(afterList);
              }

              for (var j = liNode.childNodes.length; j--;) {
                listNode.parentNode.insertBefore(liNode.childNodes[j], listNode.nextSibling);
              }

              listNode.parentNode.insertBefore(document.createElement('br'), listNode.nextSibling);
              liNode.parentNode.removeChild(liNode);

            }

            // cleanup
            if (listNode.childNodes.length === 0) {
                listNode.parentNode.removeChild(listNode);
            }
          }
        }
      }

    });
    return found;
  },

  getAfterList: function(listNode, liNode) {
    var nodeName = listNode.nodeName,
        newList = document.createElement(nodeName);

    while (liNode.nextSibling) {
      newList.appendChild(liNode.nextSibling);
    }
    return newList;
  }

};;/**
 * Undo Manager for wysihtml5
 * slightly inspired by http://rniwa.com/editing/undomanager.html#the-undomanager-interface
 */
(function(wysihtml5) {
  var Z_KEY               = 90,
      Y_KEY               = 89,
      BACKSPACE_KEY       = 8,
      DELETE_KEY          = 46,
      MAX_HISTORY_ENTRIES = 25,
      DATA_ATTR_NODE      = "data-wysihtml5-selection-node",
      DATA_ATTR_OFFSET    = "data-wysihtml5-selection-offset",
      UNDO_HTML           = '<span id="_wysihtml5-undo" class="_wysihtml5-temp">' + wysihtml5.INVISIBLE_SPACE + '</span>',
      REDO_HTML           = '<span id="_wysihtml5-redo" class="_wysihtml5-temp">' + wysihtml5.INVISIBLE_SPACE + '</span>',
      dom                 = wysihtml5.dom;

  function cleanTempElements(doc) {
    var tempElement;
    while (tempElement = doc.querySelector("._wysihtml5-temp")) {
      tempElement.parentNode.removeChild(tempElement);
    }
  }

  wysihtml5.UndoManager = wysihtml5.lang.Dispatcher.extend(
    /** @scope wysihtml5.UndoManager.prototype */ {
    constructor: function(editor) {
      this.editor = editor;
      this.composer = editor.composer;
      this.element = this.composer.element;

      this.position = 0;
      this.historyStr = [];
      this.historyDom = [];

      this.transact();

      this._observe();
    },

    _observe: function() {
      var that      = this,
          doc       = this.composer.sandbox.getDocument(),
          lastKey;

      // Catch CTRL+Z and CTRL+Y
      dom.observe(this.element, "keydown", function(event) {
        if (event.altKey || (!event.ctrlKey && !event.metaKey)) {
          return;
        }

        var keyCode = event.keyCode,
            isUndo = keyCode === Z_KEY && !event.shiftKey,
            isRedo = (keyCode === Z_KEY && event.shiftKey) || (keyCode === Y_KEY);

        if (isUndo) {
          that.undo();
          event.preventDefault();
        } else if (isRedo) {
          that.redo();
          event.preventDefault();
        }
      });

      // Catch delete and backspace
      dom.observe(this.element, "keydown", function(event) {
        var keyCode = event.keyCode;
        if (keyCode === lastKey) {
          return;
        }

        lastKey = keyCode;

        if (keyCode === BACKSPACE_KEY || keyCode === DELETE_KEY) {
          that.transact();
        }
      });

      this.editor
        .on("newword:composer", function() {
          that.transact();
        })

        .on("beforecommand:composer", function() {
          that.transact();
        });
    },

    transact: function() {
      var previousHtml      = this.historyStr[this.position - 1],
          currentHtml       = this.composer.getValue(false, false),
          composerIsVisible   = this.element.offsetWidth > 0 && this.element.offsetHeight > 0,
          range, node, offset, element, position;

      if (currentHtml === previousHtml) {
        return;
      }

      var length = this.historyStr.length = this.historyDom.length = this.position;
      if (length > MAX_HISTORY_ENTRIES) {
        this.historyStr.shift();
        this.historyDom.shift();
        this.position--;
      }

      this.position++;

      if (composerIsVisible) {
        // Do not start saving selection if composer is not visible
        range   = this.composer.selection.getRange();
        node    = (range && range.startContainer) ? range.startContainer : this.element;
        offset  = (range && range.startOffset) ? range.startOffset : 0;

        if (node.nodeType === wysihtml5.ELEMENT_NODE) {
          element = node;
        } else {
          element  = node.parentNode;
          position = this.getChildNodeIndex(element, node);
        }

        element.setAttribute(DATA_ATTR_OFFSET, offset);
        if (typeof(position) !== "undefined") {
          element.setAttribute(DATA_ATTR_NODE, position);
        }
      }

      var clone = this.element.cloneNode(!!currentHtml);
      this.historyDom.push(clone);
      this.historyStr.push(currentHtml);

      if (element) {
        element.removeAttribute(DATA_ATTR_OFFSET);
        element.removeAttribute(DATA_ATTR_NODE);
      }

    },

    undo: function() {
      this.transact();

      if (!this.undoPossible()) {
        return;
      }

      this.set(this.historyDom[--this.position - 1]);
      this.editor.fire("undo:composer");
    },

    redo: function() {
      if (!this.redoPossible()) {
        return;
      }

      this.set(this.historyDom[++this.position - 1]);
      this.editor.fire("redo:composer");
    },

    undoPossible: function() {
      return this.position > 1;
    },

    redoPossible: function() {
      return this.position < this.historyStr.length;
    },

    set: function(historyEntry) {
      this.element.innerHTML = "";

      var i = 0,
          childNodes = historyEntry.childNodes,
          length = historyEntry.childNodes.length;

      for (; i<length; i++) {
        this.element.appendChild(childNodes[i].cloneNode(true));
      }

      // Restore selection
      var offset,
          node,
          position;

      if (historyEntry.hasAttribute(DATA_ATTR_OFFSET)) {
        offset    = historyEntry.getAttribute(DATA_ATTR_OFFSET);
        position  = historyEntry.getAttribute(DATA_ATTR_NODE);
        node      = this.element;
      } else {
        node      = this.element.querySelector("[" + DATA_ATTR_OFFSET + "]") || this.element;
        offset    = node.getAttribute(DATA_ATTR_OFFSET);
        position  = node.getAttribute(DATA_ATTR_NODE);
        node.removeAttribute(DATA_ATTR_OFFSET);
        node.removeAttribute(DATA_ATTR_NODE);
      }

      if (position !== null) {
        node = this.getChildNodeByIndex(node, +position);
      }

      this.composer.selection.set(node, offset);
    },

    getChildNodeIndex: function(parent, child) {
      var i           = 0,
          childNodes  = parent.childNodes,
          length      = childNodes.length;
      for (; i<length; i++) {
        if (childNodes[i] === child) {
          return i;
        }
      }
    },

    getChildNodeByIndex: function(parent, index) {
      return parent.childNodes[index];
    }
  });
})(wysihtml5);
;/**
 * TODO: the following methods still need unit test coverage
 */
wysihtml5.views.View = Base.extend(
  /** @scope wysihtml5.views.View.prototype */ {
  constructor: function(parent, textareaElement, config) {
    this.parent   = parent;
    this.element  = textareaElement;
    this.config   = config;
    if (!this.config.noTextarea) {
        this._observeViewChange();
    }
  },

  _observeViewChange: function() {
    var that = this;
    this.parent.on("beforeload", function() {
      that.parent.on("change_view", function(view) {
        if (view === that.name) {
          that.parent.currentView = that;
          that.show();
          // Using tiny delay here to make sure that the placeholder is set before focusing
          setTimeout(function() { that.focus(); }, 0);
        } else {
          that.hide();
        }
      });
    });
  },

  focus: function() {
    if (this.element.ownerDocument.querySelector(":focus") === this.element) {
      return;
    }

    try { this.element.focus(); } catch(e) {}
  },

  hide: function() {
    this.element.style.display = "none";
  },

  show: function() {
    this.element.style.display = "";
  },

  disable: function() {
    this.element.setAttribute("disabled", "disabled");
  },

  enable: function() {
    this.element.removeAttribute("disabled");
  }
});
;(function(wysihtml5) {
  var dom       = wysihtml5.dom,
      browser   = wysihtml5.browser;

  wysihtml5.views.Composer = wysihtml5.views.View.extend(
    /** @scope wysihtml5.views.Composer.prototype */ {
    name: "composer",

    // Needed for firefox in order to display a proper caret in an empty contentEditable
    CARET_HACK: "<br>",

    constructor: function(parent, editableElement, config) {
      this.base(parent, editableElement, config);
      if (!this.config.noTextarea) {
          this.textarea = this.parent.textarea;
      } else {
          this.editableArea = editableElement;
      }
      if (this.config.contentEditableMode) {
          this._initContentEditableArea();
      } else {
          this._initSandbox();
      }
    },

    clear: function() {
      this.element.innerHTML = browser.displaysCaretInEmptyContentEditableCorrectly() ? "" : this.CARET_HACK;
    },

    getValue: function(parse, clearInternals) {
      var value = this.isEmpty() ? "" : wysihtml5.quirks.getCorrectInnerHTML(this.element);
      if (parse !== false) {
        value = this.parent.parse(value, (clearInternals === false) ? false : true);
      }

      return value;
    },

    setValue: function(html, parse) {
      if (parse) {
        html = this.parent.parse(html);
      }

      try {
        this.element.innerHTML = html;
      } catch (e) {
        this.element.innerText = html;
      }
    },

    cleanUp: function() {
        this.parent.parse(this.element);
    },

    show: function() {
      this.editableArea.style.display = this._displayStyle || "";

      if (!this.config.noTextarea && !this.textarea.element.disabled) {
        // Firefox needs this, otherwise contentEditable becomes uneditable
        this.disable();
        this.enable();
      }
    },

    hide: function() {
      this._displayStyle = dom.getStyle("display").from(this.editableArea);
      if (this._displayStyle === "none") {
        this._displayStyle = null;
      }
      this.editableArea.style.display = "none";
    },

    disable: function() {
      this.parent.fire("disable:composer");
      this.element.removeAttribute("contentEditable");
    },

    enable: function() {
      this.parent.fire("enable:composer");
      this.element.setAttribute("contentEditable", "true");
    },

    focus: function(setToEnd) {
      // IE 8 fires the focus event after .focus()
      // This is needed by our simulate_placeholder.js to work
      // therefore we clear it ourselves this time
      if (wysihtml5.browser.doesAsyncFocus() && this.hasPlaceholderSet()) {
        this.clear();
      }

      this.base();

      var lastChild = this.element.lastChild;
      if (setToEnd && lastChild && this.selection) {
        if (lastChild.nodeName === "BR") {
          this.selection.setBefore(this.element.lastChild);
        } else {
          this.selection.setAfter(this.element.lastChild);
        }
      }
    },

    getTextContent: function() {
      return dom.getTextContent(this.element);
    },

    hasPlaceholderSet: function() {
      return this.getTextContent() == ((this.config.noTextarea) ? this.editableArea.getAttribute("data-placeholder") : this.textarea.element.getAttribute("placeholder")) && this.placeholderSet;
    },

    isEmpty: function() {
      var innerHTML = this.element.innerHTML.toLowerCase();
      return (/^(\s|<br>|<\/br>|<p>|<\/p>)*$/i).test(innerHTML)  ||
             innerHTML === ""            ||
             innerHTML === "<br>"        ||
             innerHTML === "<p></p>"     ||
             innerHTML === "<p><br></p>" ||
             this.hasPlaceholderSet();
    },

    _initContentEditableArea: function() {
        var that = this;

        if (this.config.noTextarea) {
            this.sandbox = new dom.ContentEditableArea(function() {
                that._create();
            }, {}, this.editableArea);
        } else {
            this.sandbox = new dom.ContentEditableArea(function() {
                that._create();
            });
            this.editableArea = this.sandbox.getContentEditable();
            dom.insert(this.editableArea).after(this.textarea.element);
            this._createWysiwygFormField();
        }
    },

    _initSandbox: function() {
      var that = this;

      this.sandbox = new dom.Sandbox(function() {
        that._create();
      }, {
        stylesheets:  this.config.stylesheets
      });
      this.editableArea  = this.sandbox.getIframe();

      var textareaElement = this.textarea.element;
      dom.insert(this.editableArea).after(textareaElement);

      this._createWysiwygFormField();
    },

    // Creates hidden field which tells the server after submit, that the user used an wysiwyg editor
    _createWysiwygFormField: function() {
        if (this.textarea.element.form) {
          var hiddenField = document.createElement("input");
          hiddenField.type   = "hidden";
          hiddenField.name   = "_wysihtml5_mode";
          hiddenField.value  = 1;
          dom.insert(hiddenField).after(this.textarea.element);
        }
    },

    _create: function() {
      var that = this;
      this.doc                = this.sandbox.getDocument();
      this.element            = (this.config.contentEditableMode) ? this.sandbox.getContentEditable() : this.doc.body;
      if (!this.config.noTextarea) {
          this.textarea           = this.parent.textarea;
          this.element.innerHTML  = this.textarea.getValue(true, false);
      } else {
          this.cleanUp(); // cleans contenteditable on initiation as it may contain html
      }

      // Make sure our selection handler is ready
      this.selection = new wysihtml5.Selection(this.parent, this.element, this.config.uneditableContainerClassname);

      // Make sure commands dispatcher is ready
      this.commands  = new wysihtml5.Commands(this.parent);

      if (!this.config.noTextarea) {
          dom.copyAttributes([
              "className", "spellcheck", "title", "lang", "dir", "accessKey"
          ]).from(this.textarea.element).to(this.element);
      }

      dom.addClass(this.element, this.config.composerClassName);
      //
      // Make the editor look like the original textarea, by syncing styles
      if (this.config.style && !this.config.contentEditableMode) {
        this.style();
      }

      this.observe();

      var name = this.config.name;
      if (name) {
        dom.addClass(this.element, name);
        if (!this.config.contentEditableMode) { dom.addClass(this.editableArea, name); }
      }

      this.enable();

      if (!this.config.noTextarea && this.textarea.element.disabled) {
        this.disable();
      }

      // Simulate html5 placeholder attribute on contentEditable element
      var placeholderText = typeof(this.config.placeholder) === "string"
        ? this.config.placeholder
        : ((this.config.noTextarea) ? this.editableArea.getAttribute("data-placeholder") : this.textarea.element.getAttribute("placeholder"));
      if (placeholderText) {
        dom.simulatePlaceholder(this.parent, this, placeholderText);
      }

      // Make sure that the browser avoids using inline styles whenever possible
      this.commands.exec("styleWithCSS", false);

      this._initAutoLinking();
      this._initObjectResizing();
      this._initUndoManager();
      this._initLineBreaking();

      // Simulate html5 autofocus on contentEditable element
      // This doesn't work on IOS (5.1.1)
      if (!this.config.noTextarea && (this.textarea.element.hasAttribute("autofocus") || document.querySelector(":focus") == this.textarea.element) && !browser.isIos()) {
        setTimeout(function() { that.focus(true); }, 100);
      }

      // IE sometimes leaves a single paragraph, which can't be removed by the user
      if (!browser.clearsContentEditableCorrectly()) {
        wysihtml5.quirks.ensureProperClearing(this);
      }

      // Set up a sync that makes sure that textarea and editor have the same content
      if (this.initSync && this.config.sync) {
        this.initSync();
      }

      // Okay hide the textarea, we are ready to go
      if (!this.config.noTextarea) { this.textarea.hide(); }

      // Fire global (before-)load event
      this.parent.fire("beforeload").fire("load");
    },

    _initAutoLinking: function() {
      var that                           = this,
          supportsDisablingOfAutoLinking = browser.canDisableAutoLinking(),
          supportsAutoLinking            = browser.doesAutoLinkingInContentEditable();
      if (supportsDisablingOfAutoLinking) {
        this.commands.exec("autoUrlDetect", false);
      }

      if (!this.config.autoLink) {
        return;
      }

      // Only do the auto linking by ourselves when the browser doesn't support auto linking
      // OR when he supports auto linking but we were able to turn it off (IE9+)
      if (!supportsAutoLinking || (supportsAutoLinking && supportsDisablingOfAutoLinking)) {
        this.parent.on("newword:composer", function() {
          if (dom.getTextContent(that.element).match(dom.autoLink.URL_REG_EXP)) {
            that.selection.executeAndRestore(function(startContainer, endContainer) {
              var uneditables = that.element.querySelectorAll("." + that.config.uneditableContainerClassname),
                  isInUneditable = false;

              for (var i = uneditables.length; i--;) {
                if (wysihtml5.dom.contains(uneditables[i], endContainer)) {
                  isInUneditable = true;
                }
              }

              if (!isInUneditable) dom.autoLink(endContainer.parentNode, [that.config.uneditableContainerClassname]);
            });
          }
        });

        dom.observe(this.element, "blur", function() {
          dom.autoLink(that.element, [that.config.uneditableContainerClassname]);
        });
      }

      // Assuming we have the following:
      //  <a href="http://www.google.de">http://www.google.de</a>
      // If a user now changes the url in the innerHTML we want to make sure that
      // it's synchronized with the href attribute (as long as the innerHTML is still a url)
      var // Use a live NodeList to check whether there are any links in the document
          links           = this.sandbox.getDocument().getElementsByTagName("a"),
          // The autoLink helper method reveals a reg exp to detect correct urls
          urlRegExp       = dom.autoLink.URL_REG_EXP,
          getTextContent  = function(element) {
            var textContent = wysihtml5.lang.string(dom.getTextContent(element)).trim();
            if (textContent.substr(0, 4) === "www.") {
              textContent = "http://" + textContent;
            }
            return textContent;
          };

      dom.observe(this.element, "keydown", function(event) {
        if (!links.length) {
          return;
        }

        var selectedNode = that.selection.getSelectedNode(event.target.ownerDocument),
            link         = dom.getParentElement(selectedNode, { nodeName: "A" }, 4),
            textContent;

        if (!link) {
          return;
        }

        textContent = getTextContent(link);
        // keydown is fired before the actual content is changed
        // therefore we set a timeout to change the href
        setTimeout(function() {
          var newTextContent = getTextContent(link);
          if (newTextContent === textContent) {
            return;
          }

          // Only set href when new href looks like a valid url
          if (newTextContent.match(urlRegExp)) {
            link.setAttribute("href", newTextContent);
          }
        }, 0);
      });
    },

    _initObjectResizing: function() {
      this.commands.exec("enableObjectResizing", true);

      // IE sets inline styles after resizing objects
      // The following lines make sure that the width/height css properties
      // are copied over to the width/height attributes
      if (browser.supportsEvent("resizeend")) {
        var properties        = ["width", "height"],
            propertiesLength  = properties.length,
            element           = this.element;

        dom.observe(element, "resizeend", function(event) {
          var target = event.target || event.srcElement,
              style  = target.style,
              i      = 0,
              property;

          if (target.nodeName !== "IMG") {
            return;
          }

          for (; i<propertiesLength; i++) {
            property = properties[i];
            if (style[property]) {
              target.setAttribute(property, parseInt(style[property], 10));
              style[property] = "";
            }
          }

          // After resizing IE sometimes forgets to remove the old resize handles
          wysihtml5.quirks.redraw(element);
        });
      }
    },

    _initUndoManager: function() {
      this.undoManager = new wysihtml5.UndoManager(this.parent);
    },

    _initLineBreaking: function() {
      var that                              = this,
          USE_NATIVE_LINE_BREAK_INSIDE_TAGS = ["LI", "P", "H1", "H2", "H3", "H4", "H5", "H6"],
          LIST_TAGS                         = ["UL", "OL", "MENU"];

      function adjust(selectedNode) {
        var parentElement = dom.getParentElement(selectedNode, { nodeName: ["P", "DIV"] }, 2);
        if (parentElement && dom.contains(that.element, parentElement)) {
          that.selection.executeAndRestore(function() {
            if (that.config.useLineBreaks) {
              dom.replaceWithChildNodes(parentElement);
            } else if (parentElement.nodeName !== "P") {
              dom.renameElement(parentElement, "p");
            }
          });
        }
      }

      if (!this.config.useLineBreaks) {
        dom.observe(this.element, ["focus", "keydown"], function() {
          if (that.isEmpty()) {
            var paragraph = that.doc.createElement("P");
            that.element.innerHTML = "";
            that.element.appendChild(paragraph);
            if (!browser.displaysCaretInEmptyContentEditableCorrectly()) {
              paragraph.innerHTML = "<br>";
              that.selection.setBefore(paragraph.firstChild);
            } else {
              that.selection.selectNode(paragraph, true);
            }
          }
        });
      }

      // Under certain circumstances Chrome + Safari create nested <p> or <hX> tags after paste
      // Inserting an invisible white space in front of it fixes the issue
      // This is too hacky and causes selection not to replace content on paste in chrome
     /* if (browser.createsNestedInvalidMarkupAfterPaste()) {
        dom.observe(this.element, "paste", function(event) {
          var invisibleSpace = that.doc.createTextNode(wysihtml5.INVISIBLE_SPACE);
          that.selection.insertNode(invisibleSpace);
        });
      }*/


      dom.observe(this.element, "keydown", function(event) {
        var keyCode = event.keyCode;

        if (event.shiftKey) {
          return;
        }

        if (keyCode !== wysihtml5.ENTER_KEY && keyCode !== wysihtml5.BACKSPACE_KEY) {
          return;
        }
        var blockElement = dom.getParentElement(that.selection.getSelectedNode(), { nodeName: USE_NATIVE_LINE_BREAK_INSIDE_TAGS }, 4);
        if (blockElement) {
          setTimeout(function() {
            // Unwrap paragraph after leaving a list or a H1-6
            var selectedNode = that.selection.getSelectedNode(),
                list;

            if (blockElement.nodeName === "LI") {
              if (!selectedNode) {
                return;
              }

              list = dom.getParentElement(selectedNode, { nodeName: LIST_TAGS }, 2);

              if (!list) {
                adjust(selectedNode);
              }
            }

            if (keyCode === wysihtml5.ENTER_KEY && blockElement.nodeName.match(/^H[1-6]$/)) {
              adjust(selectedNode);
            }
          }, 0);
          return;
        }

        if (that.config.useLineBreaks && keyCode === wysihtml5.ENTER_KEY && !wysihtml5.browser.insertsLineBreaksOnReturn()) {
          event.preventDefault();
          that.commands.exec("insertLineBreak");

        }
      });
    }
  });
})(wysihtml5);
;(function(wysihtml5) {
  var dom             = wysihtml5.dom,
      doc             = document,
      win             = window,
      HOST_TEMPLATE   = doc.createElement("div"),
      /**
       * Styles to copy from textarea to the composer element
       */
      TEXT_FORMATTING = [
        "background-color",
        "color", "cursor",
        "font-family", "font-size", "font-style", "font-variant", "font-weight",
        "line-height", "letter-spacing",
        "text-align", "text-decoration", "text-indent", "text-rendering",
        "word-break", "word-wrap", "word-spacing"
      ],
      /**
       * Styles to copy from textarea to the iframe
       */
      BOX_FORMATTING = [
        "background-color",
        "border-collapse",
        "border-bottom-color", "border-bottom-style", "border-bottom-width",
        "border-left-color", "border-left-style", "border-left-width",
        "border-right-color", "border-right-style", "border-right-width",
        "border-top-color", "border-top-style", "border-top-width",
        "clear", "display", "float",
        "margin-bottom", "margin-left", "margin-right", "margin-top",
        "outline-color", "outline-offset", "outline-width", "outline-style",
        "padding-left", "padding-right", "padding-top", "padding-bottom",
        "position", "top", "left", "right", "bottom", "z-index",
        "vertical-align", "text-align",
        "-webkit-box-sizing", "-moz-box-sizing", "-ms-box-sizing", "box-sizing",
        "-webkit-box-shadow", "-moz-box-shadow", "-ms-box-shadow","box-shadow",
        "-webkit-border-top-right-radius", "-moz-border-radius-topright", "border-top-right-radius",
        "-webkit-border-bottom-right-radius", "-moz-border-radius-bottomright", "border-bottom-right-radius",
        "-webkit-border-bottom-left-radius", "-moz-border-radius-bottomleft", "border-bottom-left-radius",
        "-webkit-border-top-left-radius", "-moz-border-radius-topleft", "border-top-left-radius",
        "width", "height"
      ],
      ADDITIONAL_CSS_RULES = [
        "html                 { height: 100%; }",
        "body                 { height: 100%; padding: 1px 0 0 0; margin: -1px 0 0 0; }",
        "body > p:first-child { margin-top: 0; }",
        "._wysihtml5-temp     { display: none; }",
        wysihtml5.browser.isGecko ?
          "body.placeholder { color: graytext !important; }" :
          "body.placeholder { color: #a9a9a9 !important; }",
        // Ensure that user see's broken images and can delete them
        "img:-moz-broken      { -moz-force-broken-image-icon: 1; height: 24px; width: 24px; }"
      ];

  /**
   * With "setActive" IE offers a smart way of focusing elements without scrolling them into view:
   * http://msdn.microsoft.com/en-us/library/ms536738(v=vs.85).aspx
   *
   * Other browsers need a more hacky way: (pssst don't tell my mama)
   * In order to prevent the element being scrolled into view when focusing it, we simply
   * move it out of the scrollable area, focus it, and reset it's position
   */
  var focusWithoutScrolling = function(element) {
    if (element.setActive) {
      // Following line could cause a js error when the textarea is invisible
      // See https://github.com/xing/wysihtml5/issues/9
      try { element.setActive(); } catch(e) {}
    } else {
      var elementStyle = element.style,
          originalScrollTop = doc.documentElement.scrollTop || doc.body.scrollTop,
          originalScrollLeft = doc.documentElement.scrollLeft || doc.body.scrollLeft,
          originalStyles = {
            position:         elementStyle.position,
            top:              elementStyle.top,
            left:             elementStyle.left,
            WebkitUserSelect: elementStyle.WebkitUserSelect
          };

      dom.setStyles({
        position:         "absolute",
        top:              "-99999px",
        left:             "-99999px",
        // Don't ask why but temporarily setting -webkit-user-select to none makes the whole thing performing smoother
        WebkitUserSelect: "none"
      }).on(element);

      element.focus();

      dom.setStyles(originalStyles).on(element);

      if (win.scrollTo) {
        // Some browser extensions unset this method to prevent annoyances
        // "Better PopUp Blocker" for Chrome http://code.google.com/p/betterpopupblocker/source/browse/trunk/blockStart.js#100
        // Issue: http://code.google.com/p/betterpopupblocker/issues/detail?id=1
        win.scrollTo(originalScrollLeft, originalScrollTop);
      }
    }
  };


  wysihtml5.views.Composer.prototype.style = function() {
    var that                  = this,
        originalActiveElement = doc.querySelector(":focus"),
        textareaElement       = this.textarea.element,
        hasPlaceholder        = textareaElement.hasAttribute("placeholder"),
        originalPlaceholder   = hasPlaceholder && textareaElement.getAttribute("placeholder"),
        originalDisplayValue  = textareaElement.style.display,
        originalDisabled      = textareaElement.disabled,
        displayValueForCopying;

    this.focusStylesHost      = HOST_TEMPLATE.cloneNode(false);
    this.blurStylesHost       = HOST_TEMPLATE.cloneNode(false);
    this.disabledStylesHost   = HOST_TEMPLATE.cloneNode(false);

    // Remove placeholder before copying (as the placeholder has an affect on the computed style)
    if (hasPlaceholder) {
      textareaElement.removeAttribute("placeholder");
    }

    if (textareaElement === originalActiveElement) {
      textareaElement.blur();
    }

    // enable for copying styles
    textareaElement.disabled = false;

    // set textarea to display="none" to get cascaded styles via getComputedStyle
    textareaElement.style.display = displayValueForCopying = "none";

    if ((textareaElement.getAttribute("rows") && dom.getStyle("height").from(textareaElement) === "auto") ||
        (textareaElement.getAttribute("cols") && dom.getStyle("width").from(textareaElement) === "auto")) {
      textareaElement.style.display = displayValueForCopying = originalDisplayValue;
    }

    // --------- iframe styles (has to be set before editor styles, otherwise IE9 sets wrong fontFamily on blurStylesHost) ---------
    dom.copyStyles(BOX_FORMATTING).from(textareaElement).to(this.editableArea).andTo(this.blurStylesHost);

    // --------- editor styles ---------
    dom.copyStyles(TEXT_FORMATTING).from(textareaElement).to(this.element).andTo(this.blurStylesHost);

    // --------- apply standard rules ---------
    dom.insertCSS(ADDITIONAL_CSS_RULES).into(this.element.ownerDocument);

    // --------- :disabled styles ---------
    textareaElement.disabled = true;
    dom.copyStyles(BOX_FORMATTING).from(textareaElement).to(this.disabledStylesHost);
    dom.copyStyles(TEXT_FORMATTING).from(textareaElement).to(this.disabledStylesHost);
    textareaElement.disabled = originalDisabled;

    // --------- :focus styles ---------
    textareaElement.style.display = originalDisplayValue;
    focusWithoutScrolling(textareaElement);
    textareaElement.style.display = displayValueForCopying;

    dom.copyStyles(BOX_FORMATTING).from(textareaElement).to(this.focusStylesHost);
    dom.copyStyles(TEXT_FORMATTING).from(textareaElement).to(this.focusStylesHost);

    // reset textarea
    textareaElement.style.display = originalDisplayValue;

    dom.copyStyles(["display"]).from(textareaElement).to(this.editableArea);

    // Make sure that we don't change the display style of the iframe when copying styles oblur/onfocus
    // this is needed for when the change_view event is fired where the iframe is hidden and then
    // the blur event fires and re-displays it
    var boxFormattingStyles = wysihtml5.lang.array(BOX_FORMATTING).without(["display"]);

    // --------- restore focus ---------
    if (originalActiveElement) {
      originalActiveElement.focus();
    } else {
      textareaElement.blur();
    }

    // --------- restore placeholder ---------
    if (hasPlaceholder) {
      textareaElement.setAttribute("placeholder", originalPlaceholder);
    }

    // --------- Sync focus/blur styles ---------
    this.parent.on("focus:composer", function() {
      dom.copyStyles(boxFormattingStyles) .from(that.focusStylesHost).to(that.editableArea);
      dom.copyStyles(TEXT_FORMATTING)     .from(that.focusStylesHost).to(that.element);
    });

    this.parent.on("blur:composer", function() {
      dom.copyStyles(boxFormattingStyles) .from(that.blurStylesHost).to(that.editableArea);
      dom.copyStyles(TEXT_FORMATTING)     .from(that.blurStylesHost).to(that.element);
    });

    this.parent.observe("disable:composer", function() {
      dom.copyStyles(boxFormattingStyles) .from(that.disabledStylesHost).to(that.editableArea);
      dom.copyStyles(TEXT_FORMATTING)     .from(that.disabledStylesHost).to(that.element);
    });

    this.parent.observe("enable:composer", function() {
      dom.copyStyles(boxFormattingStyles) .from(that.blurStylesHost).to(that.editableArea);
      dom.copyStyles(TEXT_FORMATTING)     .from(that.blurStylesHost).to(that.element);
    });

    return this;
  };
})(wysihtml5);
;/**
 * Taking care of events
 *  - Simulating 'change' event on contentEditable element
 *  - Handling drag & drop logic
 *  - Catch paste events
 *  - Dispatch proprietary newword:composer event
 *  - Keyboard shortcuts
 */
(function(wysihtml5) {
  var dom       = wysihtml5.dom,
      browser   = wysihtml5.browser,
      /**
       * Map keyCodes to query commands
       */
      shortcuts = {
        "66": "bold",     // B
        "73": "italic",   // I
        "85": "underline" // U
      };

  var deleteAroundEditable = function(selection, uneditable, element) {
    // merge node with previous node from uneditable
    var prevNode = selection.getPreviousNode(uneditable, true),
        curNode = selection.getSelectedNode();

    if (curNode.nodeType !== 1 && curNode.parentNode !== element) { curNode = curNode.parentNode; }
    if (prevNode) {
      if (curNode.nodeType == 1) {
        var first = curNode.firstChild;

        if (prevNode.nodeType == 1) {
          while (curNode.firstChild) {
            prevNode.appendChild(curNode.firstChild);
          }
        } else {
          while (curNode.firstChild) {
            uneditable.parentNode.insertBefore(curNode.firstChild, uneditable);
          }
        }
        if (curNode.parentNode) {
          curNode.parentNode.removeChild(curNode);
        }
        selection.setBefore(first);
      } else {
        if (prevNode.nodeType == 1) {
          prevNode.appendChild(curNode);
        } else {
          uneditable.parentNode.insertBefore(curNode, uneditable);
        }
        selection.setBefore(curNode);
      }
    }
  };

  var handleDeleteKeyPress = function(event, selection, element, composer) {
    if (selection.isCollapsed()) {
      if (selection.caretIsInTheBeginnig('LI')) {
        event.preventDefault();
        composer.commands.exec('outdentList');
      } else if (selection.caretIsInTheBeginnig()) {
        event.preventDefault();
      } else {

        if (selection.caretIsFirstInSelection() &&
            selection.getPreviousNode() &&
            selection.getPreviousNode().nodeName &&
            (/^H\d$/gi).test(selection.getPreviousNode().nodeName)
        ) {
          var prevNode = selection.getPreviousNode();
          event.preventDefault();
          if ((/^\s*$/).test(prevNode.textContent || prevNode.innerText)) {
            // heading is empty
            prevNode.parentNode.removeChild(prevNode);
          } else {
            var range = prevNode.ownerDocument.createRange();
            range.selectNodeContents(prevNode);
            range.collapse(false);
            selection.setSelection(range);
          }
        }

        var beforeUneditable = selection.caretIsBeforeUneditable();
        // Do a special delete if caret would delete uneditable
        if (beforeUneditable) {
          event.preventDefault();
          deleteAroundEditable(selection, beforeUneditable, element);
        }
      }
    } else {
      if (selection.containsUneditable()) {
        event.preventDefault();
        selection.deleteContents();
      }
    }
  };

  var handleTabKeyDown = function(composer, element) {
    if (!composer.selection.isCollapsed()) {
      composer.selection.deleteContents();
    } else if (composer.selection.caretIsInTheBeginnig('LI')) {
      if (composer.commands.exec('indentList')) return;
    }

    // Is &emsp; close enough to tab. Could not find enough counter arguments for now.
    composer.commands.exec("insertHTML", "&emsp;");
  };

  wysihtml5.views.Composer.prototype.observe = function() {
    var that                = this,
        state               = this.getValue(false, false),
        container           = (this.sandbox.getIframe) ? this.sandbox.getIframe() : this.sandbox.getContentEditable(),
        element             = this.element,
        focusBlurElement    = (browser.supportsEventsInIframeCorrectly() || this.sandbox.getContentEditable) ? element : this.sandbox.getWindow(),
        pasteEvents         = ["drop", "paste"],
        interactionEvents   = ["drop", "paste", "mouseup", "focus", "keyup"];

    // --------- destroy:composer event ---------
    dom.observe(container, "DOMNodeRemoved", function() {
      clearInterval(domNodeRemovedInterval);
      that.parent.fire("destroy:composer");
    });

    // DOMNodeRemoved event is not supported in IE 8
    if (!browser.supportsMutationEvents()) {
        var domNodeRemovedInterval = setInterval(function() {
          if (!dom.contains(document.documentElement, container)) {
            clearInterval(domNodeRemovedInterval);
            that.parent.fire("destroy:composer");
          }
        }, 250);
    }

    // --------- User interaction tracking --

    dom.observe(focusBlurElement, interactionEvents, function() {
      setTimeout(function() {
        that.parent.fire("interaction").fire("interaction:composer");
      }, 0);
    });


    if (this.config.handleTables) {
      if(!this.tableClickHandle && this.doc.execCommand && wysihtml5.browser.supportsCommand(this.doc, "enableObjectResizing") && wysihtml5.browser.supportsCommand(this.doc, "enableInlineTableEditing")) {
        if (this.sandbox.getIframe) {
          this.tableClickHandle = dom.observe(container , ["focus", "mouseup", "mouseover"], function() {
            that.doc.execCommand("enableObjectResizing", false, "false");
            that.doc.execCommand("enableInlineTableEditing", false, "false");
            that.tableClickHandle.stop();
          });
        } else {
          setTimeout(function() {
            that.doc.execCommand("enableObjectResizing", false, "false");
            that.doc.execCommand("enableInlineTableEditing", false, "false");
          }, 0);
        }
      }
      this.tableSelection = wysihtml5.quirks.tableCellsSelection(element, that.parent);
    }

    // --------- Focus & blur logic ---------
    dom.observe(focusBlurElement, "focus", function(event) {
      that.parent.fire("focus", event).fire("focus:composer", event);

      // Delay storing of state until all focus handler are fired
      // especially the one which resets the placeholder
      setTimeout(function() { state = that.getValue(false, false); }, 0);
    });

    dom.observe(focusBlurElement, "blur", function(event) {
      if (state !== that.getValue(false, false)) {
        //create change event if supported (all except IE8)
        var changeevent = event;
        if(typeof Object.create == 'function') {
          changeevent = Object.create(event, { type: { value: 'change' } });
        }
        that.parent.fire("change", changeevent).fire("change:composer", changeevent);
      }
      that.parent.fire("blur", event).fire("blur:composer", event);
    });

    // --------- Drag & Drop logic ---------
    dom.observe(element, "dragenter", function() {
      that.parent.fire("unset_placeholder");
    });

    dom.observe(element, pasteEvents, function(event) {
      setTimeout(function() {
        that.parent.fire(event.type, event).fire(event.type + ":composer", event);
      }, 0);
    });

    // --------- neword event ---------
    dom.observe(element, "keyup", function(event) {
      var keyCode = event.keyCode;
      if (keyCode === wysihtml5.SPACE_KEY || keyCode === wysihtml5.ENTER_KEY) {
        that.parent.fire("newword:composer");
      }
    });

    this.parent.on("paste:composer", function() {
      setTimeout(function() { that.parent.fire("newword:composer"); }, 0);
    });

    // --------- Make sure that images are selected when clicking on them ---------
    if (!browser.canSelectImagesInContentEditable()) {
      dom.observe(element, "mousedown", function(event) {
        var target = event.target;
        var allImages = element.querySelectorAll('img'),
            notMyImages = element.querySelectorAll('.' + that.config.uneditableContainerClassname + ' img'),
            myImages = wysihtml5.lang.array(allImages).without(notMyImages);

        if (target.nodeName === "IMG" && wysihtml5.lang.array(myImages).contains(target)) {
          that.selection.selectNode(target);
        }
      });
    }

    if (!browser.canSelectImagesInContentEditable()) {
        dom.observe(element, "drop", function(event) {
            // TODO: if I knew how to get dropped elements list from event I could limit it to only IMG element case
            setTimeout(function() {
                that.selection.getSelection().removeAllRanges();
            }, 0);
        });
    }

    if (browser.hasHistoryIssue() && browser.supportsSelectionModify()) {
      dom.observe(element, "keydown", function(event) {
        if (!event.metaKey && !event.ctrlKey) {
          return;
        }

        var keyCode   = event.keyCode,
            win       = element.ownerDocument.defaultView,
            selection = win.getSelection();

        if (keyCode === 37 || keyCode === 39) {
          if (keyCode === 37) {
            selection.modify("extend", "left", "lineboundary");
            if (!event.shiftKey) {
              selection.collapseToStart();
            }
          }
          if (keyCode === 39) {
            selection.modify("extend", "right", "lineboundary");
            if (!event.shiftKey) {
              selection.collapseToEnd();
            }
          }
          event.preventDefault();
        }
      });
    }

    // --------- Shortcut logic ---------
    dom.observe(element, "keydown", function(event) {
      var keyCode  = event.keyCode,
          command  = shortcuts[keyCode];
      if ((event.ctrlKey || event.metaKey) && !event.altKey && command) {
        that.commands.exec(command);
        event.preventDefault();
      }
      if (keyCode === 8) {
        // delete key
        handleDeleteKeyPress(event, that.selection, element, that);
      } else if (that.config.handleTabKey && keyCode === 9) {
        event.preventDefault();
        handleTabKeyDown(that, element);
      }
    });

    // --------- Make sure that when pressing backspace/delete on selected images deletes the image and it's anchor ---------
    dom.observe(element, "keydown", function(event) {
      var target  = that.selection.getSelectedNode(true),
          keyCode = event.keyCode,
          parent;
      if (target && target.nodeName === "IMG" && (keyCode === wysihtml5.BACKSPACE_KEY || keyCode === wysihtml5.DELETE_KEY)) { // 8 => backspace, 46 => delete
        parent = target.parentNode;
        // delete the <img>
        parent.removeChild(target);
        // and it's parent <a> too if it hasn't got any other child nodes
        if (parent.nodeName === "A" && !parent.firstChild) {
          parent.parentNode.removeChild(parent);
        }

        setTimeout(function() { wysihtml5.quirks.redraw(element); }, 0);
        event.preventDefault();
      }
    });

    // --------- IE 8+9 focus the editor when the iframe is clicked (without actually firing the 'focus' event on the <body>) ---------
    if (!this.config.contentEditableMode && browser.hasIframeFocusIssue()) {
      dom.observe(container, "focus", function() {
        setTimeout(function() {
          if (that.doc.querySelector(":focus") !== that.element) {
            that.focus();
          }
        }, 0);
      });

      dom.observe(this.element, "blur", function() {
        setTimeout(function() {
          that.selection.getSelection().removeAllRanges();
        }, 0);
      });
    }

    // --------- Show url in tooltip when hovering links or images ---------
    var titlePrefixes = {
      IMG: "Image: ",
      A:   "Link: "
    };

    dom.observe(element, "mouseover", function(event) {
      var target   = event.target,
          nodeName = target.nodeName,
          title;
      if (nodeName !== "A" && nodeName !== "IMG") {
        return;
      }
      var hasTitle = target.hasAttribute("title");
      if(!hasTitle){
        title = titlePrefixes[nodeName] + (target.getAttribute("href") || target.getAttribute("src"));
        target.setAttribute("title", title);
      }
    });
  };
})(wysihtml5);
;/**
 * Class that takes care that the value of the composer and the textarea is always in sync
 */
(function(wysihtml5) {
  var INTERVAL = 400;

  wysihtml5.views.Synchronizer = Base.extend(
    /** @scope wysihtml5.views.Synchronizer.prototype */ {

    constructor: function(editor, textarea, composer) {
      this.editor   = editor;
      this.textarea = textarea;
      this.composer = composer;

      this._observe();
    },

    /**
     * Sync html from composer to textarea
     * Takes care of placeholders
     * @param {Boolean} shouldParseHtml Whether the html should be sanitized before inserting it into the textarea
     */
    fromComposerToTextarea: function(shouldParseHtml) {
      this.textarea.setValue(wysihtml5.lang.string(this.composer.getValue(false, false)).trim(), shouldParseHtml);
    },

    /**
     * Sync value of textarea to composer
     * Takes care of placeholders
     * @param {Boolean} shouldParseHtml Whether the html should be sanitized before inserting it into the composer
     */
    fromTextareaToComposer: function(shouldParseHtml) {
      var textareaValue = this.textarea.getValue(false, false);
      if (textareaValue) {
        this.composer.setValue(textareaValue, shouldParseHtml);
      } else {
        this.composer.clear();
        this.editor.fire("set_placeholder");
      }
    },

    /**
     * Invoke syncing based on view state
     * @param {Boolean} shouldParseHtml Whether the html should be sanitized before inserting it into the composer/textarea
     */
    sync: function(shouldParseHtml) {
      if (this.editor.currentView.name === "textarea") {
        this.fromTextareaToComposer(shouldParseHtml);
      } else {
        this.fromComposerToTextarea(shouldParseHtml);
      }
    },

    /**
     * Initializes interval-based syncing
     * also makes sure that on-submit the composer's content is synced with the textarea
     * immediately when the form gets submitted
     */
    _observe: function() {
      var interval,
          that          = this,
          form          = this.textarea.element.form,
          startInterval = function() {
            interval = setInterval(function() { that.fromComposerToTextarea(); }, INTERVAL);
          },
          stopInterval  = function() {
            clearInterval(interval);
            interval = null;
          };

      startInterval();

      if (form) {
        // If the textarea is in a form make sure that after onreset and onsubmit the composer
        // has the correct state
        wysihtml5.dom.observe(form, "submit", function() {
          that.sync(true);
        });
        wysihtml5.dom.observe(form, "reset", function() {
          setTimeout(function() { that.fromTextareaToComposer(); }, 0);
        });
      }

      this.editor.on("change_view", function(view) {
        if (view === "composer" && !interval) {
          that.fromTextareaToComposer(true);
          startInterval();
        } else if (view === "textarea") {
          that.fromComposerToTextarea(true);
          stopInterval();
        }
      });

      this.editor.on("destroy:composer", stopInterval);
    }
  });
})(wysihtml5);
;wysihtml5.views.Textarea = wysihtml5.views.View.extend(
  /** @scope wysihtml5.views.Textarea.prototype */ {
  name: "textarea",

  constructor: function(parent, textareaElement, config) {
    this.base(parent, textareaElement, config);

    this._observe();
  },

  clear: function() {
    this.element.value = "";
  },

  getValue: function(parse) {
    var value = this.isEmpty() ? "" : this.element.value;
    if (parse !== false) {
      value = this.parent.parse(value);
    }
    return value;
  },

  setValue: function(html, parse) {
    if (parse) {
      html = this.parent.parse(html);
    }
    this.element.value = html;
  },

  cleanUp: function() {
      var html = this.parent.parse(this.element.value);
      this.element.value = html;
  },

  hasPlaceholderSet: function() {
    var supportsPlaceholder = wysihtml5.browser.supportsPlaceholderAttributeOn(this.element),
        placeholderText     = this.element.getAttribute("placeholder") || null,
        value               = this.element.value,
        isEmpty             = !value;
    return (supportsPlaceholder && isEmpty) || (value === placeholderText);
  },

  isEmpty: function() {
    return !wysihtml5.lang.string(this.element.value).trim() || this.hasPlaceholderSet();
  },

  _observe: function() {
    var element = this.element,
        parent  = this.parent,
        eventMapping = {
          focusin:  "focus",
          focusout: "blur"
        },
        /**
         * Calling focus() or blur() on an element doesn't synchronously trigger the attached focus/blur events
         * This is the case for focusin and focusout, so let's use them whenever possible, kkthxbai
         */
        events = wysihtml5.browser.supportsEvent("focusin") ? ["focusin", "focusout", "change"] : ["focus", "blur", "change"];

    parent.on("beforeload", function() {
      wysihtml5.dom.observe(element, events, function(event) {
        var eventName = eventMapping[event.type] || event.type;
        parent.fire(eventName).fire(eventName + ":textarea");
      });

      wysihtml5.dom.observe(element, ["paste", "drop"], function() {
        setTimeout(function() { parent.fire("paste").fire("paste:textarea"); }, 0);
      });
    });
  }
});
;/**
 * WYSIHTML5 Editor
 *
 * @param {Element} editableElement Reference to the textarea which should be turned into a rich text interface
 * @param {Object} [config] See defaultConfig object below for explanation of each individual config option
 *
 * @events
 *    load
 *    beforeload (for internal use only)
 *    focus
 *    focus:composer
 *    focus:textarea
 *    blur
 *    blur:composer
 *    blur:textarea
 *    change
 *    change:composer
 *    change:textarea
 *    paste
 *    paste:composer
 *    paste:textarea
 *    newword:composer
 *    destroy:composer
 *    undo:composer
 *    redo:composer
 *    beforecommand:composer
 *    aftercommand:composer
 *    enable:composer
 *    disable:composer
 *    change_view
 */
(function(wysihtml5) {
  var undef;

  var defaultConfig = {
    // Give the editor a name, the name will also be set as class name on the iframe and on the iframe's body
    name:                 undef,
    // Whether the editor should look like the textarea (by adopting styles)
    style:                true,
    // Id of the toolbar element, pass falsey value if you don't want any toolbar logic
    toolbar:              undef,
    // Whether toolbar is displayed after init by script automatically.
    // Can be set to false if toolobar is set to display only on editable area focus
    showToolbarAfterInit: true,
    // Whether urls, entered by the user should automatically become clickable-links
    autoLink:             true,
    // Includes table editing events and cell selection tracking
    handleTables:         true,
    // Tab key inserts tab into text as default behaviour. It can be disabled to regain keyboard navigation
    handleTabKey:         true,
    // Object which includes parser rules to apply when html gets inserted via copy & paste
    // See parser_rules/*.js for examples
    parserRules:          { tags: { br: {}, span: {}, div: {}, p: {} }, classes: {} },
    // Parser method to use when the user inserts content via copy & paste
    parser:               wysihtml5.dom.parse,
    // Class name which should be set on the contentEditable element in the created sandbox iframe, can be styled via the 'stylesheets' option
    composerClassName:    "wysihtml5-editor",
    // Class name to add to the body when the wysihtml5 editor is supported
    bodyClassName:        "wysihtml5-supported",
    // By default wysihtml5 will insert a <br> for line breaks, set this to false to use <p>
    useLineBreaks:        true,
    // Array (or single string) of stylesheet urls to be loaded in the editor's iframe
    stylesheets:          [],
    // Placeholder text to use, defaults to the placeholder attribute on the textarea element
    placeholderText:      undef,
    // Whether the rich text editor should be rendered on touch devices (wysihtml5 >= 0.3.0 comes with basic support for iOS 5)
    supportTouchDevices:  true,
    // Whether senseless <span> elements (empty or without attributes) should be removed/replaced with their content
    cleanUp:              true,
    // Whether to use div instead of secure iframe
    contentEditableMode: false,
    // Classname of container that editor should not touch and pass through
    // Pass false to disable
    uneditableContainerClassname: "wysihtml5-uneditable-container"
  };

  wysihtml5.Editor = wysihtml5.lang.Dispatcher.extend(
    /** @scope wysihtml5.Editor.prototype */ {
    constructor: function(editableElement, config) {
      this.editableElement  = typeof(editableElement) === "string" ? document.getElementById(editableElement) : editableElement;
      this.config           = wysihtml5.lang.object({}).merge(defaultConfig).merge(config).get();
      this._isCompatible    = wysihtml5.browser.supported();

      if (this.editableElement.nodeName.toLowerCase() != "textarea") {
          this.config.contentEditableMode = true;
          this.config.noTextarea = true;
      }
      if (!this.config.noTextarea) {
          this.textarea         = new wysihtml5.views.Textarea(this, this.editableElement, this.config);
          this.currentView      = this.textarea;
      }

      // Sort out unsupported/unwanted browsers here
      if (!this._isCompatible || (!this.config.supportTouchDevices && wysihtml5.browser.isTouchDevice())) {
        var that = this;
        setTimeout(function() { that.fire("beforeload").fire("load"); }, 0);
        return;
      }

      // Add class name to body, to indicate that the editor is supported
      wysihtml5.dom.addClass(document.body, this.config.bodyClassName);

      this.composer = new wysihtml5.views.Composer(this, this.editableElement, this.config);
      this.currentView = this.composer;

      if (typeof(this.config.parser) === "function") {
        this._initParser();
      }

      this.on("beforeload", this.handleBeforeLoad);
    },

    handleBeforeLoad: function() {
        if (!this.config.noTextarea) {
            this.synchronizer = new wysihtml5.views.Synchronizer(this, this.textarea, this.composer);
        }
        if (this.config.toolbar) {
          this.toolbar = new wysihtml5.toolbar.Toolbar(this, this.config.toolbar, this.config.showToolbarAfterInit);
        }
    },

    isCompatible: function() {
      return this._isCompatible;
    },

    clear: function() {
      this.currentView.clear();
      return this;
    },

    getValue: function(parse, clearInternals) {
      return this.currentView.getValue(parse, clearInternals);
    },

    setValue: function(html, parse) {
      this.fire("unset_placeholder");

      if (!html) {
        return this.clear();
      }

      this.currentView.setValue(html, parse);
      return this;
    },

    cleanUp: function() {
        this.currentView.cleanUp();
    },

    focus: function(setToEnd) {
      this.currentView.focus(setToEnd);
      return this;
    },

    /**
     * Deactivate editor (make it readonly)
     */
    disable: function() {
      this.currentView.disable();
      return this;
    },

    /**
     * Activate editor
     */
    enable: function() {
      this.currentView.enable();
      return this;
    },

    isEmpty: function() {
      return this.currentView.isEmpty();
    },

    hasPlaceholderSet: function() {
      return this.currentView.hasPlaceholderSet();
    },

    parse: function(htmlOrElement, clearInternals) {
      var parseContext = (this.config.contentEditableMode) ? document : ((this.composer) ? this.composer.sandbox.getDocument() : null);
      var returnValue = this.config.parser(htmlOrElement, {
        "rules": this.config.parserRules,
        "cleanUp": this.config.cleanUp,
        "context": parseContext,
        "uneditableClass": this.config.uneditableContainerClassname,
        "clearInternals" : clearInternals
      });
      if (typeof(htmlOrElement) === "object") {
        wysihtml5.quirks.redraw(htmlOrElement);
      }
      return returnValue;
    },

    /**
     * Prepare html parser logic
     *  - Observes for paste and drop
     */
    _initParser: function() {
      this.on("paste:composer", function() {
        var keepScrollPosition  = true,
            that                = this;
        that.composer.selection.executeAndRestore(function() {
          wysihtml5.quirks.cleanPastedHTML(that.composer.element);
          that.parse(that.composer.element);
        }, keepScrollPosition);
      });
    }
  });
})(wysihtml5);
;/**
 * Toolbar Dialog
 *
 * @param {Element} link The toolbar link which causes the dialog to show up
 * @param {Element} container The dialog container
 *
 * @example
 *    <!-- Toolbar link -->
 *    <a data-wysihtml5-command="insertImage">insert an image</a>
 *
 *    <!-- Dialog -->
 *    <div data-wysihtml5-dialog="insertImage" style="display: none;">
 *      <label>
 *        URL: <input data-wysihtml5-dialog-field="src" value="http://">
 *      </label>
 *      <label>
 *        Alternative text: <input data-wysihtml5-dialog-field="alt" value="">
 *      </label>
 *    </div>
 *
 *    <script>
 *      var dialog = new wysihtml5.toolbar.Dialog(
 *        document.querySelector("[data-wysihtml5-command='insertImage']"),
 *        document.querySelector("[data-wysihtml5-dialog='insertImage']")
 *      );
 *      dialog.observe("save", function(attributes) {
 *        // do something
 *      });
 *    </script>
 */
(function(wysihtml5) {
  var dom                     = wysihtml5.dom,
      CLASS_NAME_OPENED       = "wysihtml5-command-dialog-opened",
      SELECTOR_FORM_ELEMENTS  = "input, select, textarea",
      SELECTOR_FIELDS         = "[data-wysihtml5-dialog-field]",
      ATTRIBUTE_FIELDS        = "data-wysihtml5-dialog-field";


  wysihtml5.toolbar.Dialog = wysihtml5.lang.Dispatcher.extend(
    /** @scope wysihtml5.toolbar.Dialog.prototype */ {
    constructor: function(link, container) {
      this.link       = link;
      this.container  = container;
    },

    _observe: function() {
      if (this._observed) {
        return;
      }

      var that = this,
          callbackWrapper = function(event) {
            var attributes = that._serialize();
            if (attributes == that.elementToChange) {
              that.fire("edit", attributes);
            } else {
              that.fire("save", attributes);
            }
            that.hide();
            event.preventDefault();
            event.stopPropagation();
          };

      dom.observe(that.link, "click", function() {
        if (dom.hasClass(that.link, CLASS_NAME_OPENED)) {
          setTimeout(function() { that.hide(); }, 0);
        }
      });

      dom.observe(this.container, "keydown", function(event) {
        var keyCode = event.keyCode;
        if (keyCode === wysihtml5.ENTER_KEY) {
          callbackWrapper(event);
        }
        if (keyCode === wysihtml5.ESCAPE_KEY) {
          that.fire("cancel");
          that.hide();
        }
      });

      dom.delegate(this.container, "[data-wysihtml5-dialog-action=save]", "click", callbackWrapper);

      dom.delegate(this.container, "[data-wysihtml5-dialog-action=cancel]", "click", function(event) {
        that.fire("cancel");
        that.hide();
        event.preventDefault();
        event.stopPropagation();
      });

      var formElements  = this.container.querySelectorAll(SELECTOR_FORM_ELEMENTS),
          i             = 0,
          length        = formElements.length,
          _clearInterval = function() { clearInterval(that.interval); };
      for (; i<length; i++) {
        dom.observe(formElements[i], "change", _clearInterval);
      }

      this._observed = true;
    },

    /**
     * Grabs all fields in the dialog and puts them in key=>value style in an object which
     * then gets returned
     */
    _serialize: function() {
      var data    = this.elementToChange || {},
          fields  = this.container.querySelectorAll(SELECTOR_FIELDS),
          length  = fields.length,
          i       = 0;

      for (; i<length; i++) {
        data[fields[i].getAttribute(ATTRIBUTE_FIELDS)] = fields[i].value;
      }
      return data;
    },

    /**
     * Takes the attributes of the "elementToChange"
     * and inserts them in their corresponding dialog input fields
     *
     * Assume the "elementToChange" looks like this:
     *    <a href="http://www.google.com" target="_blank">foo</a>
     *
     * and we have the following dialog:
     *    <input type="text" data-wysihtml5-dialog-field="href" value="">
     *    <input type="text" data-wysihtml5-dialog-field="target" value="">
     *
     * after calling _interpolate() the dialog will look like this
     *    <input type="text" data-wysihtml5-dialog-field="href" value="http://www.google.com">
     *    <input type="text" data-wysihtml5-dialog-field="target" value="_blank">
     *
     * Basically it adopted the attribute values into the corresponding input fields
     *
     */
    _interpolate: function(avoidHiddenFields) {
      var field,
          fieldName,
          newValue,
          focusedElement = document.querySelector(":focus"),
          fields         = this.container.querySelectorAll(SELECTOR_FIELDS),
          length         = fields.length,
          i              = 0;
      for (; i<length; i++) {
        field = fields[i];

        // Never change elements where the user is currently typing in
        if (field === focusedElement) {
          continue;
        }

        // Don't update hidden fields
        // See https://github.com/xing/wysihtml5/pull/14
        if (avoidHiddenFields && field.type === "hidden") {
          continue;
        }

        fieldName = field.getAttribute(ATTRIBUTE_FIELDS);
        newValue  = (this.elementToChange && typeof(this.elementToChange) !== 'boolean') ? (this.elementToChange.getAttribute(fieldName) || "") : field.defaultValue;
        field.value = newValue;
      }
    },

    /**
     * Show the dialog element
     */
    show: function(elementToChange) {
      if (dom.hasClass(this.link, CLASS_NAME_OPENED)) {
        return;
      }

      var that        = this,
          firstField  = this.container.querySelector(SELECTOR_FORM_ELEMENTS);
      this.elementToChange = elementToChange;
      this._observe();
      this._interpolate();
      if (elementToChange) {
        this.interval = setInterval(function() { that._interpolate(true); }, 500);
      }
      dom.addClass(this.link, CLASS_NAME_OPENED);
      this.container.style.display = "";
      this.fire("show");
      if (firstField && !elementToChange) {
        try {
          firstField.focus();
        } catch(e) {}
      }
    },

    /**
     * Hide the dialog element
     */
    hide: function() {
      clearInterval(this.interval);
      this.elementToChange = null;
      dom.removeClass(this.link, CLASS_NAME_OPENED);
      this.container.style.display = "none";
      this.fire("hide");
    }
  });
})(wysihtml5);
;/**
 * Converts speech-to-text and inserts this into the editor
 * As of now (2011/03/25) this only is supported in Chrome >= 11
 *
 * Note that it sends the recorded audio to the google speech recognition api:
 * http://stackoverflow.com/questions/4361826/does-chrome-have-buil-in-speech-recognition-for-input-type-text-x-webkit-speec
 *
 * Current HTML5 draft can be found here
 * http://lists.w3.org/Archives/Public/public-xg-htmlspeech/2011Feb/att-0020/api-draft.html
 *
 * "Accessing Google Speech API Chrome 11"
 * http://mikepultz.com/2011/03/accessing-google-speech-api-chrome-11/
 */
(function(wysihtml5) {
  var dom = wysihtml5.dom;

  var linkStyles = {
    position: "relative"
  };

  var wrapperStyles = {
    left:     0,
    margin:   0,
    opacity:  0,
    overflow: "hidden",
    padding:  0,
    position: "absolute",
    top:      0,
    zIndex:   1
  };

  var inputStyles = {
    cursor:     "inherit",
    fontSize:   "50px",
    height:     "50px",
    marginTop:  "-25px",
    outline:    0,
    padding:    0,
    position:   "absolute",
    right:      "-4px",
    top:        "50%"
  };

  var inputAttributes = {
    "x-webkit-speech": "",
    "speech":          ""
  };

  wysihtml5.toolbar.Speech = function(parent, link) {
    var input = document.createElement("input");
    if (!wysihtml5.browser.supportsSpeechApiOn(input)) {
      link.style.display = "none";
      return;
    }
    var lang = parent.editor.textarea.element.getAttribute("lang");
    if (lang) {
      inputAttributes.lang = lang;
    }

    var wrapper = document.createElement("div");

    wysihtml5.lang.object(wrapperStyles).merge({
      width:  link.offsetWidth  + "px",
      height: link.offsetHeight + "px"
    });

    dom.insert(input).into(wrapper);
    dom.insert(wrapper).into(link);

    dom.setStyles(inputStyles).on(input);
    dom.setAttributes(inputAttributes).on(input);

    dom.setStyles(wrapperStyles).on(wrapper);
    dom.setStyles(linkStyles).on(link);

    var eventName = "onwebkitspeechchange" in input ? "webkitspeechchange" : "speechchange";
    dom.observe(input, eventName, function() {
      parent.execCommand("insertText", input.value);
      input.value = "";
    });

    dom.observe(input, "click", function(event) {
      if (dom.hasClass(link, "wysihtml5-command-disabled")) {
        event.preventDefault();
      }

      event.stopPropagation();
    });
  };
})(wysihtml5);
;/**
 * Toolbar
 *
 * @param {Object} parent Reference to instance of Editor instance
 * @param {Element} container Reference to the toolbar container element
 *
 * @example
 *    <div id="toolbar">
 *      <a data-wysihtml5-command="createLink">insert link</a>
 *      <a data-wysihtml5-command="formatBlock" data-wysihtml5-command-value="h1">insert h1</a>
 *    </div>
 *
 *    <script>
 *      var toolbar = new wysihtml5.toolbar.Toolbar(editor, document.getElementById("toolbar"));
 *    </script>
 */
(function(wysihtml5) {
  var CLASS_NAME_COMMAND_DISABLED   = "wysihtml5-command-disabled",
      CLASS_NAME_COMMANDS_DISABLED  = "wysihtml5-commands-disabled",
      CLASS_NAME_COMMAND_ACTIVE     = "wysihtml5-command-active",
      CLASS_NAME_ACTION_ACTIVE      = "wysihtml5-action-active",
      dom                           = wysihtml5.dom;

  wysihtml5.toolbar.Toolbar = Base.extend(
    /** @scope wysihtml5.toolbar.Toolbar.prototype */ {
    constructor: function(editor, container, showOnInit) {
      this.editor     = editor;
      this.container  = typeof(container) === "string" ? document.getElementById(container) : container;
      this.composer   = editor.composer;

      this._getLinks("command");
      this._getLinks("action");

      this._observe();
      if (showOnInit) { this.show(); }

      var speechInputLinks  = this.container.querySelectorAll("[data-wysihtml5-command=insertSpeech]"),
          length            = speechInputLinks.length,
          i                 = 0;
      for (; i<length; i++) {
        new wysihtml5.toolbar.Speech(this, speechInputLinks[i]);
      }
    },

    _getLinks: function(type) {
      var links   = this[type + "Links"] = wysihtml5.lang.array(this.container.querySelectorAll("[data-wysihtml5-" + type + "]")).get(),
          length  = links.length,
          i       = 0,
          mapping = this[type + "Mapping"] = {},
          link,
          group,
          name,
          value,
          dialog;
      for (; i<length; i++) {
        link    = links[i];
        name    = link.getAttribute("data-wysihtml5-" + type);
        value   = link.getAttribute("data-wysihtml5-" + type + "-value");
        group   = this.container.querySelector("[data-wysihtml5-" + type + "-group='" + name + "']");
        dialog  = this._getDialog(link, name);

        mapping[name + ":" + value] = {
          link:   link,
          group:  group,
          name:   name,
          value:  value,
          dialog: dialog,
          state:  false
        };
      }
    },

    _getDialog: function(link, command) {
      var that          = this,
          dialogElement = this.container.querySelector("[data-wysihtml5-dialog='" + command + "']"),
          dialog,
          caretBookmark;

      if (dialogElement) {
        if (wysihtml5.toolbar["Dialog_" + command]) {
            dialog = new wysihtml5.toolbar["Dialog_" + command](link, dialogElement);
        } else {
            dialog = new wysihtml5.toolbar.Dialog(link, dialogElement);
        }

        dialog.on("show", function() {
          caretBookmark = that.composer.selection.getBookmark();

          that.editor.fire("show:dialog", { command: command, dialogContainer: dialogElement, commandLink: link });
        });

        dialog.on("save", function(attributes) {
          if (caretBookmark) {
            that.composer.selection.setBookmark(caretBookmark);
          }
          that._execCommand(command, attributes);

          that.editor.fire("save:dialog", { command: command, dialogContainer: dialogElement, commandLink: link });
        });

        dialog.on("cancel", function() {
          that.editor.focus(false);
          that.editor.fire("cancel:dialog", { command: command, dialogContainer: dialogElement, commandLink: link });
        });
      }
      return dialog;
    },

    /**
     * @example
     *    var toolbar = new wysihtml5.Toolbar();
     *    // Insert a <blockquote> element or wrap current selection in <blockquote>
     *    toolbar.execCommand("formatBlock", "blockquote");
     */
    execCommand: function(command, commandValue) {
      if (this.commandsDisabled) {
        return;
      }

      var commandObj = this.commandMapping[command + ":" + commandValue];

      // Show dialog when available
      if (commandObj && commandObj.dialog && !commandObj.state) {
        commandObj.dialog.show();
      } else {
        this._execCommand(command, commandValue);
      }
    },

    _execCommand: function(command, commandValue) {
      // Make sure that composer is focussed (false => don't move caret to the end)
      this.editor.focus(false);

      this.composer.commands.exec(command, commandValue);
      this._updateLinkStates();
    },

    execAction: function(action) {
      var editor = this.editor;
      if (action === "change_view") {
        if (editor.textarea) {
            if (editor.currentView === editor.textarea) {
              editor.fire("change_view", "composer");
            } else {
              editor.fire("change_view", "textarea");
            }
        }
      }
      if (action == "showSource") {
          editor.fire("showSource");
      }
    },

    _observe: function() {
      var that      = this,
          editor    = this.editor,
          container = this.container,
          links     = this.commandLinks.concat(this.actionLinks),
          length    = links.length,
          i         = 0;

      for (; i<length; i++) {
        // 'javascript:;' and unselectable=on Needed for IE, but done in all browsers to make sure that all get the same css applied
        // (you know, a:link { ... } doesn't match anchors with missing href attribute)
        if (links[i].nodeName === "A") {
          dom.setAttributes({
            href:         "javascript:;",
            unselectable: "on"
          }).on(links[i]);
        } else {
          dom.setAttributes({ unselectable: "on" }).on(links[i]);
        }
      }

      // Needed for opera and chrome
      dom.delegate(container, "[data-wysihtml5-command], [data-wysihtml5-action]", "mousedown", function(event) { event.preventDefault(); });

      dom.delegate(container, "[data-wysihtml5-command]", "click", function(event) {
        var link          = this,
            command       = link.getAttribute("data-wysihtml5-command"),
            commandValue  = link.getAttribute("data-wysihtml5-command-value");
        that.execCommand(command, commandValue);
        event.preventDefault();
      });

      dom.delegate(container, "[data-wysihtml5-action]", "click", function(event) {
        var action = this.getAttribute("data-wysihtml5-action");
        that.execAction(action);
        event.preventDefault();
      });

      editor.on("interaction:composer", function() {
          that._updateLinkStates();
      });

      editor.on("focus:composer", function() {
        that.bookmark = null;
      });

      if (this.editor.config.handleTables) {
          editor.on("tableselect:composer", function() {
              that.container.querySelectorAll('[data-wysihtml5-hiddentools="table"]')[0].style.display = "";
          });
          editor.on("tableunselect:composer", function() {
              that.container.querySelectorAll('[data-wysihtml5-hiddentools="table"]')[0].style.display = "none";
          });
      }

      editor.on("change_view", function(currentView) {
        // Set timeout needed in order to let the blur event fire first
        if (editor.textarea) {
            setTimeout(function() {
              that.commandsDisabled = (currentView !== "composer");
              that._updateLinkStates();
              if (that.commandsDisabled) {
                dom.addClass(container, CLASS_NAME_COMMANDS_DISABLED);
              } else {
                dom.removeClass(container, CLASS_NAME_COMMANDS_DISABLED);
              }
            }, 0);
        }
      });
    },

    _updateLinkStates: function() {

      var commandMapping    = this.commandMapping,
          actionMapping     = this.actionMapping,
          i,
          state,
          action,
          command;
      // every millisecond counts... this is executed quite often
      for (i in commandMapping) {
        command = commandMapping[i];
        if (this.commandsDisabled) {
          state = false;
          dom.removeClass(command.link, CLASS_NAME_COMMAND_ACTIVE);
          if (command.group) {
            dom.removeClass(command.group, CLASS_NAME_COMMAND_ACTIVE);
          }
          if (command.dialog) {
            command.dialog.hide();
          }
        } else {
          state = this.composer.commands.state(command.name, command.value);
          dom.removeClass(command.link, CLASS_NAME_COMMAND_DISABLED);
          if (command.group) {
            dom.removeClass(command.group, CLASS_NAME_COMMAND_DISABLED);
          }
        }
        if (command.state === state) {
          continue;
        }

        command.state = state;
        if (state) {
          dom.addClass(command.link, CLASS_NAME_COMMAND_ACTIVE);
          if (command.group) {
            dom.addClass(command.group, CLASS_NAME_COMMAND_ACTIVE);
          }
          if (command.dialog) {
            if (typeof(state) === "object" || wysihtml5.lang.object(state).isArray()) {

              if (!command.dialog.multiselect && wysihtml5.lang.object(state).isArray()) {
                // Grab first and only object/element in state array, otherwise convert state into boolean
                // to avoid showing a dialog for multiple selected elements which may have different attributes
                // eg. when two links with different href are selected, the state will be an array consisting of both link elements
                // but the dialog interface can only update one
                state = state.length === 1 ? state[0] : true;
                command.state = state;
              }
              command.dialog.show(state);
            } else {
              command.dialog.hide();
            }
          }
        } else {
          dom.removeClass(command.link, CLASS_NAME_COMMAND_ACTIVE);
          if (command.group) {
            dom.removeClass(command.group, CLASS_NAME_COMMAND_ACTIVE);
          }
          if (command.dialog) {
            command.dialog.hide();
          }
        }
      }

      for (i in actionMapping) {
        action = actionMapping[i];

        if (action.name === "change_view") {
          action.state = this.editor.currentView === this.editor.textarea;
          if (action.state) {
            dom.addClass(action.link, CLASS_NAME_ACTION_ACTIVE);
          } else {
            dom.removeClass(action.link, CLASS_NAME_ACTION_ACTIVE);
          }
        }
      }
    },

    show: function() {
      this.container.style.display = "";
    },

    hide: function() {
      this.container.style.display = "none";
    }
  });

})(wysihtml5);
;(function(wysihtml5) {
    wysihtml5.toolbar.Dialog_createTable = wysihtml5.toolbar.Dialog.extend({
        show: function(elementToChange) {
            this.base(elementToChange);

        }

    });
})(wysihtml5);
;(function(wysihtml5) {
  var dom                     = wysihtml5.dom,
      SELECTOR_FIELDS         = "[data-wysihtml5-dialog-field]",
      ATTRIBUTE_FIELDS        = "data-wysihtml5-dialog-field";

  wysihtml5.toolbar.Dialog_foreColorStyle = wysihtml5.toolbar.Dialog.extend({
    multiselect: true,

    _serialize: function() {
      var data    = {},
          fields  = this.container.querySelectorAll(SELECTOR_FIELDS),
          length  = fields.length,
          i       = 0;

      for (; i<length; i++) {
        data[fields[i].getAttribute(ATTRIBUTE_FIELDS)] = fields[i].value;
      }
      return data;
    },

    _interpolate: function(avoidHiddenFields) {
      var field,
          fieldName,
          newValue,
          focusedElement = document.querySelector(":focus"),
          fields         = this.container.querySelectorAll(SELECTOR_FIELDS),
          length         = fields.length,
          i              = 0,
          firstElement   = (this.elementToChange) ? ((wysihtml5.lang.object(this.elementToChange).isArray()) ? this.elementToChange[0] : this.elementToChange) : null,
          colorStr       = (firstElement) ? firstElement.getAttribute('style') : null,
          color          = (colorStr) ? wysihtml5.quirks.styleParser.parseColor(colorStr, "color") : null;

      for (; i<length; i++) {
        field = fields[i];
        // Never change elements where the user is currently typing in
        if (field === focusedElement) {
          continue;
        }
        // Don't update hidden fields3
        if (avoidHiddenFields && field.type === "hidden") {
          continue;
        }
        if (field.getAttribute(ATTRIBUTE_FIELDS) === "color") {
          if (color) {
            if (color[3] && color[3] != 1) {
              field.value = "rgba(" + color[0] + "," + color[1] + "," + color[2] + "," + color[3] + ");";
            } else {
              field.value = "rgb(" + color[0] + "," + color[1] + "," + color[2] + ");";
            }
          } else {
            field.value = "rgb(0,0,0);";
          }
        }
      }
    }

  });
})(wysihtml5);
;(function(wysihtml5) {
  var dom                     = wysihtml5.dom,
      SELECTOR_FIELDS         = "[data-wysihtml5-dialog-field]",
      ATTRIBUTE_FIELDS        = "data-wysihtml5-dialog-field";

  wysihtml5.toolbar.Dialog_fontSizeStyle = wysihtml5.toolbar.Dialog.extend({
    multiselect: true,

    _serialize: function() {
      return {"size" : this.container.querySelector('[data-wysihtml5-dialog-field="size"]').value};
    },

    _interpolate: function(avoidHiddenFields) {
      var focusedElement = document.querySelector(":focus"),
          field          = this.container.querySelector("[data-wysihtml5-dialog-field='size']"),
          firstElement   = (this.elementToChange) ? ((wysihtml5.lang.object(this.elementToChange).isArray()) ? this.elementToChange[0] : this.elementToChange) : null,
          styleStr       = (firstElement) ? firstElement.getAttribute('style') : null,
          size           = (styleStr) ? wysihtml5.quirks.styleParser.parseFontSize(styleStr) : null;

      if (field && field !== focusedElement && size && !(/^\s*$/).test(size)) {
        field.value = size;
      }
    }

  });
})(wysihtml5);
/*!

 handlebars v1.3.0

Copyright (C) 2011 by Yehuda Katz

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

@license
*/
var Handlebars=function(){var a=function(){"use strict";function a(a){this.string=a}var b;return a.prototype.toString=function(){return""+this.string},b=a}(),b=function(a){"use strict";function b(a){return h[a]||"&amp;"}function c(a,b){for(var c in b)Object.prototype.hasOwnProperty.call(b,c)&&(a[c]=b[c])}function d(a){return a instanceof g?a.toString():a||0===a?(a=""+a,j.test(a)?a.replace(i,b):a):""}function e(a){return a||0===a?m(a)&&0===a.length?!0:!1:!0}var f={},g=a,h={"&":"&amp;","<":"&lt;",">":"&gt;",'"':"&quot;","'":"&#x27;","`":"&#x60;"},i=/[&<>"'`]/g,j=/[&<>"'`]/;f.extend=c;var k=Object.prototype.toString;f.toString=k;var l=function(a){return"function"==typeof a};l(/x/)&&(l=function(a){return"function"==typeof a&&"[object Function]"===k.call(a)});var l;f.isFunction=l;var m=Array.isArray||function(a){return a&&"object"==typeof a?"[object Array]"===k.call(a):!1};return f.isArray=m,f.escapeExpression=d,f.isEmpty=e,f}(a),c=function(){"use strict";function a(a,b){var d;b&&b.firstLine&&(d=b.firstLine,a+=" - "+d+":"+b.firstColumn);for(var e=Error.prototype.constructor.call(this,a),f=0;f<c.length;f++)this[c[f]]=e[c[f]];d&&(this.lineNumber=d,this.column=b.firstColumn)}var b,c=["description","fileName","lineNumber","message","name","number","stack"];return a.prototype=new Error,b=a}(),d=function(a,b){"use strict";function c(a,b){this.helpers=a||{},this.partials=b||{},d(this)}function d(a){a.registerHelper("helperMissing",function(a){if(2===arguments.length)return void 0;throw new h("Missing helper: '"+a+"'")}),a.registerHelper("blockHelperMissing",function(b,c){var d=c.inverse||function(){},e=c.fn;return m(b)&&(b=b.call(this)),b===!0?e(this):b===!1||null==b?d(this):l(b)?b.length>0?a.helpers.each(b,c):d(this):e(b)}),a.registerHelper("each",function(a,b){var c,d=b.fn,e=b.inverse,f=0,g="";if(m(a)&&(a=a.call(this)),b.data&&(c=q(b.data)),a&&"object"==typeof a)if(l(a))for(var h=a.length;h>f;f++)c&&(c.index=f,c.first=0===f,c.last=f===a.length-1),g+=d(a[f],{data:c});else for(var i in a)a.hasOwnProperty(i)&&(c&&(c.key=i,c.index=f,c.first=0===f),g+=d(a[i],{data:c}),f++);return 0===f&&(g=e(this)),g}),a.registerHelper("if",function(a,b){return m(a)&&(a=a.call(this)),!b.hash.includeZero&&!a||g.isEmpty(a)?b.inverse(this):b.fn(this)}),a.registerHelper("unless",function(b,c){return a.helpers["if"].call(this,b,{fn:c.inverse,inverse:c.fn,hash:c.hash})}),a.registerHelper("with",function(a,b){return m(a)&&(a=a.call(this)),g.isEmpty(a)?void 0:b.fn(a)}),a.registerHelper("log",function(b,c){var d=c.data&&null!=c.data.level?parseInt(c.data.level,10):1;a.log(d,b)})}function e(a,b){p.log(a,b)}var f={},g=a,h=b,i="1.3.0";f.VERSION=i;var j=4;f.COMPILER_REVISION=j;var k={1:"<= 1.0.rc.2",2:"== 1.0.0-rc.3",3:"== 1.0.0-rc.4",4:">= 1.0.0"};f.REVISION_CHANGES=k;var l=g.isArray,m=g.isFunction,n=g.toString,o="[object Object]";f.HandlebarsEnvironment=c,c.prototype={constructor:c,logger:p,log:e,registerHelper:function(a,b,c){if(n.call(a)===o){if(c||b)throw new h("Arg not supported with multiple helpers");g.extend(this.helpers,a)}else c&&(b.not=c),this.helpers[a]=b},registerPartial:function(a,b){n.call(a)===o?g.extend(this.partials,a):this.partials[a]=b}};var p={methodMap:{0:"debug",1:"info",2:"warn",3:"error"},DEBUG:0,INFO:1,WARN:2,ERROR:3,level:3,log:function(a,b){if(p.level<=a){var c=p.methodMap[a];"undefined"!=typeof console&&console[c]&&console[c].call(console,b)}}};f.logger=p,f.log=e;var q=function(a){var b={};return g.extend(b,a),b};return f.createFrame=q,f}(b,c),e=function(a,b,c){"use strict";function d(a){var b=a&&a[0]||1,c=m;if(b!==c){if(c>b){var d=n[c],e=n[b];throw new l("Template was precompiled with an older version of Handlebars than the current runtime. Please update your precompiler to a newer version ("+d+") or downgrade your runtime to an older version ("+e+").")}throw new l("Template was precompiled with a newer version of Handlebars than the current runtime. Please update your runtime to a newer version ("+a[1]+").")}}function e(a,b){if(!b)throw new l("No environment passed to template");var c=function(a,c,d,e,f,g){var h=b.VM.invokePartial.apply(this,arguments);if(null!=h)return h;if(b.compile){var i={helpers:e,partials:f,data:g};return f[c]=b.compile(a,{data:void 0!==g},b),f[c](d,i)}throw new l("The partial "+c+" could not be compiled when running in runtime-only mode")},d={escapeExpression:k.escapeExpression,invokePartial:c,programs:[],program:function(a,b,c){var d=this.programs[a];return c?d=g(a,b,c):d||(d=this.programs[a]=g(a,b)),d},merge:function(a,b){var c=a||b;return a&&b&&a!==b&&(c={},k.extend(c,b),k.extend(c,a)),c},programWithDepth:b.VM.programWithDepth,noop:b.VM.noop,compilerInfo:null};return function(c,e){e=e||{};var f,g,h=e.partial?e:b;e.partial||(f=e.helpers,g=e.partials);var i=a.call(d,h,c,f,g,e.data);return e.partial||b.VM.checkRevision(d.compilerInfo),i}}function f(a,b,c){var d=Array.prototype.slice.call(arguments,3),e=function(a,e){return e=e||{},b.apply(this,[a,e.data||c].concat(d))};return e.program=a,e.depth=d.length,e}function g(a,b,c){var d=function(a,d){return d=d||{},b(a,d.data||c)};return d.program=a,d.depth=0,d}function h(a,b,c,d,e,f){var g={partial:!0,helpers:d,partials:e,data:f};if(void 0===a)throw new l("The partial "+b+" could not be found");return a instanceof Function?a(c,g):void 0}function i(){return""}var j={},k=a,l=b,m=c.COMPILER_REVISION,n=c.REVISION_CHANGES;return j.checkRevision=d,j.template=e,j.programWithDepth=f,j.program=g,j.invokePartial=h,j.noop=i,j}(b,c,d),f=function(a,b,c,d,e){"use strict";var f,g=a,h=b,i=c,j=d,k=e,l=function(){var a=new g.HandlebarsEnvironment;return j.extend(a,g),a.SafeString=h,a.Exception=i,a.Utils=j,a.VM=k,a.template=function(b){return k.template(b,a)},a},m=l();return m.create=l,f=m}(d,a,c,b,e);return f}();this["wysihtml5"] = this["wysihtml5"] || {};
this["wysihtml5"]["tpl"] = this["wysihtml5"]["tpl"] || {};

this["wysihtml5"]["tpl"]["blockquote"] = Handlebars.template(function (Handlebars,depth0,helpers,partials,data) {
  this.compilerInfo = [4,'>= 1.0.0'];
helpers = this.merge(helpers, Handlebars.helpers); data = data || {};
  var buffer = "", stack1, functionType="function", escapeExpression=this.escapeExpression, self=this;

function program1(depth0,data) {
  
  var buffer = "", stack1;
  buffer += "btn-"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1));
  return buffer;
  }

function program3(depth0,data) {
  
  
  return " \n      <span class=\"fa fa-quote-left\"></span>\n    ";
  }

function program5(depth0,data) {
  
  
  return "\n      <span class=\"glyphicon glyphicon-quote\"></span>\n    ";
  }

  buffer += "<li>\n  <a class=\"btn ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += " btn-default\" data-wysihtml5-command=\"formatBlock\" data-wysihtml5-command-value=\"blockquote\" data-wysihtml5-display-format-name=\"false\" tabindex=\"-1\">\n    ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.fa), {hash:{},inverse:self.program(5, program5, data),fn:self.program(3, program3, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += "\n  </a>\n</li>\n";
  return buffer;
  });

this["wysihtml5"]["tpl"]["color"] = Handlebars.template(function (Handlebars,depth0,helpers,partials,data) {
  this.compilerInfo = [4,'>= 1.0.0'];
helpers = this.merge(helpers, Handlebars.helpers); data = data || {};
  var buffer = "", stack1, functionType="function", escapeExpression=this.escapeExpression, self=this;

function program1(depth0,data) {
  
  var buffer = "", stack1;
  buffer += "btn-"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1));
  return buffer;
  }

  buffer += "<li class=\"dropdown\">\n  <a class=\"btn btn-default dropdown-toggle ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += "\" data-toggle=\"dropdown\" tabindex=\"-1\">\n    <span class=\"current-color\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.colours)),stack1 == null || stack1 === false ? stack1 : stack1.black)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</span>\n    <b class=\"caret\"></b>\n  </a>\n  <ul class=\"dropdown-menu\">\n    <li><div class=\"wysihtml5-colors\" data-wysihtml5-command-value=\"black\"></div><a class=\"wysihtml5-colors-title\" data-wysihtml5-command=\"foreColor\" data-wysihtml5-command-value=\"black\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.colours)),stack1 == null || stack1 === false ? stack1 : stack1.black)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><div class=\"wysihtml5-colors\" data-wysihtml5-command-value=\"silver\"></div><a class=\"wysihtml5-colors-title\" data-wysihtml5-command=\"foreColor\" data-wysihtml5-command-value=\"silver\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.colours)),stack1 == null || stack1 === false ? stack1 : stack1.silver)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><div class=\"wysihtml5-colors\" data-wysihtml5-command-value=\"gray\"></div><a class=\"wysihtml5-colors-title\" data-wysihtml5-command=\"foreColor\" data-wysihtml5-command-value=\"gray\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.colours)),stack1 == null || stack1 === false ? stack1 : stack1.gray)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><div class=\"wysihtml5-colors\" data-wysihtml5-command-value=\"maroon\"></div><a class=\"wysihtml5-colors-title\" data-wysihtml5-command=\"foreColor\" data-wysihtml5-command-value=\"maroon\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.colours)),stack1 == null || stack1 === false ? stack1 : stack1.maroon)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><div class=\"wysihtml5-colors\" data-wysihtml5-command-value=\"red\"></div><a class=\"wysihtml5-colors-title\" data-wysihtml5-command=\"foreColor\" data-wysihtml5-command-value=\"red\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.colours)),stack1 == null || stack1 === false ? stack1 : stack1.red)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><div class=\"wysihtml5-colors\" data-wysihtml5-command-value=\"purple\"></div><a class=\"wysihtml5-colors-title\" data-wysihtml5-command=\"foreColor\" data-wysihtml5-command-value=\"purple\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.colours)),stack1 == null || stack1 === false ? stack1 : stack1.purple)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><div class=\"wysihtml5-colors\" data-wysihtml5-command-value=\"green\"></div><a class=\"wysihtml5-colors-title\" data-wysihtml5-command=\"foreColor\" data-wysihtml5-command-value=\"green\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.colours)),stack1 == null || stack1 === false ? stack1 : stack1.green)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><div class=\"wysihtml5-colors\" data-wysihtml5-command-value=\"olive\"></div><a class=\"wysihtml5-colors-title\" data-wysihtml5-command=\"foreColor\" data-wysihtml5-command-value=\"olive\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.colours)),stack1 == null || stack1 === false ? stack1 : stack1.olive)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><div class=\"wysihtml5-colors\" data-wysihtml5-command-value=\"navy\"></div><a class=\"wysihtml5-colors-title\" data-wysihtml5-command=\"foreColor\" data-wysihtml5-command-value=\"navy\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.colours)),stack1 == null || stack1 === false ? stack1 : stack1.navy)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><div class=\"wysihtml5-colors\" data-wysihtml5-command-value=\"blue\"></div><a class=\"wysihtml5-colors-title\" data-wysihtml5-command=\"foreColor\" data-wysihtml5-command-value=\"blue\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.colours)),stack1 == null || stack1 === false ? stack1 : stack1.blue)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><div class=\"wysihtml5-colors\" data-wysihtml5-command-value=\"orange\"></div><a class=\"wysihtml5-colors-title\" data-wysihtml5-command=\"foreColor\" data-wysihtml5-command-value=\"orange\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.colours)),stack1 == null || stack1 === false ? stack1 : stack1.orange)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n  </ul>\n</li>\n";
  return buffer;
  });

this["wysihtml5"]["tpl"]["emphasis"] = Handlebars.template(function (Handlebars,depth0,helpers,partials,data) {
  this.compilerInfo = [4,'>= 1.0.0'];
helpers = this.merge(helpers, Handlebars.helpers); data = data || {};
  var buffer = "", stack1, functionType="function", escapeExpression=this.escapeExpression, self=this;

function program1(depth0,data) {
  
  var buffer = "", stack1;
  buffer += "btn-"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1));
  return buffer;
  }

function program3(depth0,data) {
  
  var buffer = "", stack1;
  buffer += "\n    <a class=\"btn ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += " btn-default\" data-wysihtml5-command=\"small\" title=\"CTRL+S\" tabindex=\"-1\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.emphasis)),stack1 == null || stack1 === false ? stack1 : stack1.small)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a>\n    ";
  return buffer;
  }

  buffer += "<li>\n  <div class=\"btn-group\">\n    <a class=\"btn ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += " btn-default\" data-wysihtml5-command=\"bold\" title=\"CTRL+B\" tabindex=\"-1\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.emphasis)),stack1 == null || stack1 === false ? stack1 : stack1.bold)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a>\n    <a class=\"btn ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  //if(stack1 || stack1 === 0) { buffer += stack1; }
  //buffer += " btn-default\" data-wysihtml5-command=\"italic\" title=\"CTRL+I\" tabindex=\"-1\">"
  //  + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.emphasis)),stack1 == null || stack1 === false ? stack1 : stack1.italic)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
  //  + "</a>\n    <a class=\"btn ";
  //stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  //if(stack1 || stack1 === 0) { buffer += stack1; }
  //buffer += " btn-default\" data-wysihtml5-command=\"underline\" title=\"CTRL+U\" tabindex=\"-1\">"
  //  + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.emphasis)),stack1 == null || stack1 === false ? stack1 : stack1.underline)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
  //  + "</a>\n    ";
  //stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.emphasis)),stack1 == null || stack1 === false ? stack1 : stack1.small), {hash:{},inverse:self.noop,fn:self.program(3, program3, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += "\n  </div>\n</li>\n";
  return buffer;
  });

this["wysihtml5"]["tpl"]["font-styles"] = Handlebars.template(function (Handlebars,depth0,helpers,partials,data) {
  this.compilerInfo = [4,'>= 1.0.0'];
helpers = this.merge(helpers, Handlebars.helpers); data = data || {};
  var buffer = "", stack1, functionType="function", escapeExpression=this.escapeExpression, self=this;

function program1(depth0,data) {
  
  var buffer = "", stack1;
  buffer += "btn-"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1));
  return buffer;
  }

function program3(depth0,data) {
  
  
  return "\n      <span class=\"fa fa-font\"></span>\n    ";
  }

function program5(depth0,data) {
  
  
  return "\n      <span class=\"glyphicon glyphicon-font\"></span>\n    ";
  }

  buffer += "<li class=\"dropdown\">\n  <a class=\"btn btn-default dropdown-toggle ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += "\" data-toggle=\"dropdown\">\n    ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.fa), {hash:{},inverse:self.program(5, program5, data),fn:self.program(3, program3, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += "\n    <span class=\"current-font\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.font_styles)),stack1 == null || stack1 === false ? stack1 : stack1.normal)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</span>\n    <b class=\"caret\"></b>\n  </a>\n  <ul class=\"dropdown-menu\">\n    <li><a data-wysihtml5-command=\"formatBlock\" data-wysihtml5-command-value=\"p\" tabindex=\"-1\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.font_styles)),stack1 == null || stack1 === false ? stack1 : stack1.normal)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><a data-wysihtml5-command=\"formatBlock\" data-wysihtml5-command-value=\"h1\" tabindex=\"-1\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.font_styles)),stack1 == null || stack1 === false ? stack1 : stack1.h1)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><a data-wysihtml5-command=\"formatBlock\" data-wysihtml5-command-value=\"h2\" tabindex=\"-1\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.font_styles)),stack1 == null || stack1 === false ? stack1 : stack1.h2)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><a data-wysihtml5-command=\"formatBlock\" data-wysihtml5-command-value=\"h3\" tabindex=\"-1\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.font_styles)),stack1 == null || stack1 === false ? stack1 : stack1.h3)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><a data-wysihtml5-command=\"formatBlock\" data-wysihtml5-command-value=\"h4\" tabindex=\"-1\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.font_styles)),stack1 == null || stack1 === false ? stack1 : stack1.h4)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><a data-wysihtml5-command=\"formatBlock\" data-wysihtml5-command-value=\"h5\" tabindex=\"-1\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.font_styles)),stack1 == null || stack1 === false ? stack1 : stack1.h5)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n    <li><a data-wysihtml5-command=\"formatBlock\" data-wysihtml5-command-value=\"h6\" tabindex=\"-1\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.font_styles)),stack1 == null || stack1 === false ? stack1 : stack1.h6)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a></li>\n  </ul>\n</li>\n";
  return buffer;
  });

this["wysihtml5"]["tpl"]["html"] = Handlebars.template(function (Handlebars,depth0,helpers,partials,data) {
  this.compilerInfo = [4,'>= 1.0.0'];
helpers = this.merge(helpers, Handlebars.helpers); data = data || {};
  var buffer = "", stack1, functionType="function", escapeExpression=this.escapeExpression, self=this;

function program1(depth0,data) {
  
  var buffer = "", stack1;
  buffer += "btn-"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1));
  return buffer;
  }

function program3(depth0,data) {
  
  
  return "\n        <span class=\"fa fa-pencil\"></span>\n      ";
  }

function program5(depth0,data) {
  
  
  return "\n        <span class=\"glyphicon glyphicon-pencil\"></span>\n      ";
  }

  buffer += "<li>\n  <div class=\"btn-group\">\n    <a class=\"btn ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += " btn-default\" data-wysihtml5-action=\"change_view\" title=\""
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.html)),stack1 == null || stack1 === false ? stack1 : stack1.edit)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "\" tabindex=\"-1\">\n      ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.fa), {hash:{},inverse:self.program(5, program5, data),fn:self.program(3, program3, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += "\n    </a>\n  </div>\n</li>\n";
  return buffer;
  });

this["wysihtml5"]["tpl"]["image"] = Handlebars.template(function (Handlebars,depth0,helpers,partials,data) {
  this.compilerInfo = [4,'>= 1.0.0'];
helpers = this.merge(helpers, Handlebars.helpers); data = data || {};
  var buffer = "", stack1, functionType="function", escapeExpression=this.escapeExpression, self=this;

function program1(depth0,data) {
  
  
  return "modal-sm";
  }

function program3(depth0,data) {
  
  var buffer = "", stack1;
  buffer += "btn-"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1));
  return buffer;
  }

function program5(depth0,data) {
  
  
  return "\n      <span class=\"fa fa-file-image-o\"></span>\n    ";
  }

function program7(depth0,data) {
  
  
  return "\n      <span class=\"glyphicon glyphicon-picture\"></span>\n    ";
  }

  buffer += "<li>\n  <div class=\"bootstrap-wysihtml5-insert-image-modal modal fade\" data-wysihtml5-dialog=\"insertImage\">\n    <div class=\"modal-dialog ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.smallmodals), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += "\">\n      <div class=\"modal-content\">\n        <div class=\"modal-header\">\n          <a class=\"close\" data-dismiss=\"modal\">&times;</a>\n          <h3>"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.image)),stack1 == null || stack1 === false ? stack1 : stack1.insert)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</h3>\n        </div>\n        <div class=\"modal-body\">\n          <div class=\"form-group\">\n            <input value=\"http://\" class=\"bootstrap-wysihtml5-insert-image-url form-control\">\n          </div> \n        </div>\n        <div class=\"modal-footer\">\n          <a class=\"btn btn-default\" data-dismiss=\"modal\" data-wysihtml5-dialog-action=\"cancel\" href=\"#\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.image)),stack1 == null || stack1 === false ? stack1 : stack1.cancel)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a>\n          <a class=\"btn btn-primary\" data-dismiss=\"modal\"  data-wysihtml5-dialog-action=\"save\" href=\"#\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.image)),stack1 == null || stack1 === false ? stack1 : stack1.insert)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a>\n        </div>\n      </div>\n    </div>\n  </div>\n  <a class=\"btn ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(3, program3, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += " btn-default\" data-wysihtml5-command=\"insertImage\" title=\""
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.image)),stack1 == null || stack1 === false ? stack1 : stack1.insert)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "\" tabindex=\"-1\">\n    ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.fa), {hash:{},inverse:self.program(7, program7, data),fn:self.program(5, program5, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += "\n  </a>\n</li>\n";
  return buffer;
  });

this["wysihtml5"]["tpl"]["link"] = Handlebars.template(function (Handlebars,depth0,helpers,partials,data) {
  this.compilerInfo = [4,'>= 1.0.0'];
helpers = this.merge(helpers, Handlebars.helpers); data = data || {};
  var buffer = "", stack1, functionType="function", escapeExpression=this.escapeExpression, self=this;

function program1(depth0,data) {
  
  
  return "modal-sm";
  }

function program3(depth0,data) {
  
  var buffer = "", stack1;
  buffer += "btn-"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1));
  return buffer;
  }

function program5(depth0,data) {
  
  
  return "\n      <span class=\"fa fa-share-square-o\"></span>\n    ";
  }

function program7(depth0,data) {
  
  
  return "\n      <span class=\"glyphicon glyphicon-share\"></span>\n    ";
  }

  buffer += "<li>\n  <div class=\"bootstrap-wysihtml5-insert-link-modal modal fade\" data-wysihtml5-dialog=\"createLink\">\n    <div class=\"modal-dialog ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.smallmodals), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += "\">\n      <div class=\"modal-content\">\n        <div class=\"modal-header\">\n          <a class=\"close\" data-dismiss=\"modal\">&times;</a>\n          <h3>"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.link)),stack1 == null || stack1 === false ? stack1 : stack1.insert)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</h3>\n        </div>\n        <div class=\"modal-body\">\n          <div class=\"form-group\">\n            <input value=\"http://\" class=\"bootstrap-wysihtml5-insert-link-url form-control\" data-wysihtml5-dialog-field=\"href\">\n          </div> \n          <div class=\"checkbox\">\n            <label> \n              <input type=\"checkbox\" class=\"bootstrap-wysihtml5-insert-link-target\" checked>"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.link)),stack1 == null || stack1 === false ? stack1 : stack1.target)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "\n            </label>\n          </div>\n        </div>\n        <div class=\"modal-footer\">\n          <a class=\"btn btn-default\" data-dismiss=\"modal\" data-wysihtml5-dialog-action=\"cancel\" href=\"#\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.link)),stack1 == null || stack1 === false ? stack1 : stack1.cancel)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a>\n          <a href=\"#\" class=\"btn btn-primary\" data-dismiss=\"modal\" data-wysihtml5-dialog-action=\"save\">"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.link)),stack1 == null || stack1 === false ? stack1 : stack1.insert)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "</a>\n        </div>\n      </div>\n    </div>\n  </div>\n  <a class=\"btn ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(3, program3, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += " btn-default\" data-wysihtml5-command=\"createLink\" title=\""
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.link)),stack1 == null || stack1 === false ? stack1 : stack1.insert)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "\" tabindex=\"-1\">\n    ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.fa), {hash:{},inverse:self.program(7, program7, data),fn:self.program(5, program5, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += "\n  </a>\n</li>\n";
  return buffer;
  });

this["wysihtml5"]["tpl"]["lists"] = Handlebars.template(function (Handlebars,depth0,helpers,partials,data) {
  this.compilerInfo = [4,'>= 1.0.0'];
helpers = this.merge(helpers, Handlebars.helpers); data = data || {};
  var buffer = "", stack1, functionType="function", escapeExpression=this.escapeExpression, self=this;

function program1(depth0,data) {
  
  var buffer = "", stack1;
  buffer += "btn-"
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1));
  return buffer;
  }

function program3(depth0,data) {
  
  
  return "\n      <span class=\"fa fa-list-ul\"></span>\n    ";
  }

function program5(depth0,data) {
  
  
  return "\n      <span class=\"glyphicon glyphicon-list\"></span>\n    ";
  }

function program7(depth0,data) {
  
  
  return "\n      <span class=\"fa fa-list-ol\"></span>\n    ";
  }

function program9(depth0,data) {
  
  
  return "\n      <span class=\"glyphicon glyphicon-th-list\"></span>\n    ";
  }

function program11(depth0,data) {
  
  
  return "\n      <span class=\"fa fa-outdent\"></span>\n    ";
  }

function program13(depth0,data) {
  
  
  return "\n      <span class=\"glyphicon glyphicon-indent-right\"></span>\n    ";
  }

function program15(depth0,data) {
  
  
  return "\n      <span class=\"fa fa-indent\"></span>\n    ";
  }

function program17(depth0,data) {
  
  
  return "\n      <span class=\"glyphicon glyphicon-indent-left\"></span>\n    ";
  }

  buffer += "<li>\n  <div class=\"btn-group\">\n    <a class=\"btn ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += " btn-default\" data-wysihtml5-command=\"insertUnorderedList\" title=\""
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.lists)),stack1 == null || stack1 === false ? stack1 : stack1.unordered)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "\" tabindex=\"-1\">\n    ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.fa), {hash:{},inverse:self.program(5, program5, data),fn:self.program(3, program3, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += "\n    </a>\n    <a class=\"btn ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += " btn-default\" data-wysihtml5-command=\"insertOrderedList\" title=\""
    + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.lists)),stack1 == null || stack1 === false ? stack1 : stack1.ordered)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
    + "\" tabindex=\"-1\">\n    ";
  stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.fa), {hash:{},inverse:self.program(9, program9, data),fn:self.program(7, program7, data),data:data});
  //if(stack1 || stack1 === 0) { buffer += stack1; }
  //buffer += "\n    </a>\n    <a class=\"btn ";
  //stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  //if(stack1 || stack1 === 0) { buffer += stack1; }
  //buffer += " btn-default\" data-wysihtml5-command=\"Outdent\" title=\""
  //  + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.lists)),stack1 == null || stack1 === false ? stack1 : stack1.outdent)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
  //  + "\" tabindex=\"-1\">\n    ";
  //stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.fa), {hash:{},inverse:self.program(13, program13, data),fn:self.program(11, program11, data),data:data});
  //if(stack1 || stack1 === 0) { buffer += stack1; }
  //buffer += "\n    </a>\n    <a class=\"btn ";
  //stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.size), {hash:{},inverse:self.noop,fn:self.program(1, program1, data),data:data});
  //if(stack1 || stack1 === 0) { buffer += stack1; }
  //buffer += " btn-default\" data-wysihtml5-command=\"Indent\" title=\""
  //  + escapeExpression(((stack1 = ((stack1 = ((stack1 = (depth0 && depth0.locale)),stack1 == null || stack1 === false ? stack1 : stack1.lists)),stack1 == null || stack1 === false ? stack1 : stack1.indent)),typeof stack1 === functionType ? stack1.apply(depth0) : stack1))
  //  + "\" tabindex=\"-1\">\n    ";
  //stack1 = helpers['if'].call(depth0, ((stack1 = ((stack1 = (depth0 && depth0.options)),stack1 == null || stack1 === false ? stack1 : stack1.toolbar)),stack1 == null || stack1 === false ? stack1 : stack1.fa), {hash:{},inverse:self.program(17, program17, data),fn:self.program(15, program15, data),data:data});
  if(stack1 || stack1 === 0) { buffer += stack1; }
  buffer += "\n    </a>\n  </div>\n</li>\n";
  return buffer;
  });/* jshint expr: true */
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define('bootstrap.wysihtml5', ['jquery', 'wysihtml5', 'bootstrap', 'bootstrap.wysihtml5.templates', 'bootstrap.wysihtml5.commands'], factory);
    } else {
        // Browser globals
        factory(jQuery, wysihtml5);
    }
}(function ($, wysihtml5) {

var bsWysihtml5 = function($, wysihtml5) {
  'use strict';

  var templates = function(key, locale, options) {
    if(wysihtml5.tpl[key]) {
      return wysihtml5.tpl[key]({locale: locale, options: options});
    }
  };

  var Wysihtml5 = function(el, options) {
    this.el = el;
    var toolbarOpts = $.extend(true, {}, defaultOptions, options);
    for(var t in toolbarOpts.customTemplates) {
      wysihtml5.tpl[t] = toolbarOpts.customTemplates[t];
    }
    this.toolbar = this.createToolbar(el, toolbarOpts);
    this.editor =  this.createEditor(toolbarOpts);
  };

  Wysihtml5.prototype = {

    constructor: Wysihtml5,

    createEditor: function(options) {
      options = options || {};

      // Add the toolbar to a clone of the options object so multiple instances
      // of the WYISYWG don't break because 'toolbar' is already defined
      options = $.extend(true, {}, options);
      options.toolbar = this.toolbar[0];

      var editor = new wysihtml5.Editor(this.el[0], options);

      // #30 - body is in IE 10 not created by default, which leads to nullpointer
      // 2014/02/13 - adapted to wysihtml5-0.4, does not work in IE
      if(editor.composer.editableArea.contentDocument) {
        this.addMoreShortcuts(editor, editor.composer.editableArea.contentDocument.body || editor.composer.editableArea.contentDocument, options.shortcuts);
      } else {
        this.addMoreShortcuts(editor, editor.composer.editableArea, options.shortcuts);    
      }
      

      if(options && options.events) {
        for(var eventName in options.events) {
          editor.on(eventName, options.events[eventName]);
        }
      }
      
      editor.on('beforeload', this.syncBootstrapDialogEvents);
      //syncBootstrapDialogEvents();
      return editor;
    },

    //sync wysihtml5 events for dialogs with bootstrap events
    syncBootstrapDialogEvents: function() {
      var editor = this;
      $.map(this.toolbar.commandMapping, function(value, index) {
        return [value];
      }).filter(function(commandObj, idx, arr) {
        return commandObj.dialog;
      }).map(function(commandObj, idx, arr) {
        return commandObj.dialog;
      }).forEach(function(dialog, idx, arr) {
        dialog.on('show', function(event) {
          $(this.container).modal('show');
        });
        dialog.on('hide', function(event) {
          $(this.container).modal('hide');
          editor.composer.focus();
        });
        $(dialog.container).on('shown.bs.modal', function () {
          $(this).find('input, select, textarea').first().focus();
        });
      });
    },

    createToolbar: function(el, options) {
      var self = this;
      var toolbar = $('<ul/>', {
        'class' : 'wysihtml5-toolbar',
        'style': 'display:none'
      });
      var culture = options.locale || defaultOptions.locale || 'en';
      if(!locale.hasOwnProperty(culture)) {
        console.debug('Locale \'' + culture + '\' not found. Available locales are: ' + Object.keys(locale) + '. Falling back to \'en\'.');
        culture = 'en';
      }
      var localeObject = $.extend(true, {}, locale.en, locale[culture]);
      for(var key in options.toolbar) {
        if(options.toolbar[key]) {
          toolbar.append(templates(key, localeObject, options));

          if(key === 'html') {
            this.initHtml(toolbar);
          }

        }
      }

      toolbar.find('a[data-wysihtml5-command="formatBlock"]').click(function(e) {
        var target = e.delegateTarget || e.target || e.srcElement,
            el = $(target),
            showformat = el.data('wysihtml5-display-format-name'),
            formatname = el.data('wysihtml5-format-name') || el.html();
        if(showformat === undefined || showformat === 'true') {
          self.toolbar.find('.current-font').text(formatname);
        }
      });

      toolbar.find('a[data-wysihtml5-command="foreColor"]').click(function(e) {
        var target = e.target || e.srcElement;
        var el = $(target);
        self.toolbar.find('.current-color').text(el.html());
      });

      this.el.before(toolbar);

      return toolbar;
    },

    initHtml: function(toolbar) {
      var changeViewSelector = 'a[data-wysihtml5-action="change_view"]';
      toolbar.find(changeViewSelector).click(function(e) {
        toolbar.find('a.btn').not(changeViewSelector).toggleClass('disabled');
      });
    },

    addMoreShortcuts: function(editor, el, shortcuts) {
      /* some additional shortcuts */
      wysihtml5.dom.observe(el, 'keydown', function(event) {
        var keyCode  = event.keyCode,
            command  = shortcuts[keyCode];
        if ((event.ctrlKey || event.metaKey || event.altKey) && command && wysihtml5.commands[command]) {

          var commandObj = editor.toolbar.commandMapping[command + ':null'];
          if (commandObj && commandObj.dialog && !commandObj.state) {
            commandObj.dialog.show();
          } else {
            wysihtml5.commands[command].exec(editor.composer, command);
          }
          event.preventDefault();
        }
      });
    }
  };

  // these define our public api
  var methods = {
    resetDefaults: function() {
      $.fn.wysihtml5.defaultOptions = $.extend(true, {}, $.fn.wysihtml5.defaultOptionsCache);
    },
    bypassDefaults: function(options) {
      return this.each(function () {
        var $this = $(this);
        $this.data('wysihtml5', new Wysihtml5($this, options));
      });
    },
    shallowExtend: function (options) {
      var settings = $.extend({}, $.fn.wysihtml5.defaultOptions, options || {}, $(this).data());
      var that = this;
      return methods.bypassDefaults.apply(that, [settings]);
    },
    deepExtend: function(options) {
      var settings = $.extend(true, {}, $.fn.wysihtml5.defaultOptions, options || {});
      var that = this;
      return methods.bypassDefaults.apply(that, [settings]);
    },
    init: function(options) {
      var that = this;
      return methods.shallowExtend.apply(that, [options]);
    }
  };

  $.fn.wysihtml5 = function ( method ) {
    if ( methods[method] ) {
      return methods[method].apply( this, Array.prototype.slice.call( arguments, 1 ));
    } else if ( typeof method === 'object' || ! method ) {
      return methods.init.apply( this, arguments );
    } else {
      $.error( 'Method ' +  method + ' does not exist on jQuery.wysihtml5' );
    }    
  };

  $.fn.wysihtml5.Constructor = Wysihtml5;

  var defaultOptions = $.fn.wysihtml5.defaultOptions = {
    toolbar: {
      'font-styles': true,
      'color': false,
      'emphasis': {
        'small': true
      },
      'blockquote': true,
      'lists': true,
      'html': false,
      'link': true,
      'image': true,
      'smallmodals': false
    },
    parserRules: {
      classes: {
        'wysiwyg-color-silver' : 1,
        'wysiwyg-color-gray' : 1,
        'wysiwyg-color-white' : 1,
        'wysiwyg-color-maroon' : 1,
        'wysiwyg-color-red' : 1,
        'wysiwyg-color-purple' : 1,
        'wysiwyg-color-fuchsia' : 1,
        'wysiwyg-color-green' : 1,
        'wysiwyg-color-lime' : 1,
        'wysiwyg-color-olive' : 1,
        'wysiwyg-color-yellow' : 1,
        'wysiwyg-color-navy' : 1,
        'wysiwyg-color-blue' : 1,
        'wysiwyg-color-teal' : 1,
        'wysiwyg-color-aqua' : 1,
        'wysiwyg-color-orange' : 1
      },
      tags: {
        'b':  {},
        'i':  {},
        'strong': {},
        'em': {},
        'p': {},
        'br': {},
        'ol': {},
        'ul': {},
        'li': {},
        'h1': {},
        'h2': {},
        'h3': {},
        'h4': {},
        'h5': {},
        'h6': {},
        'blockquote': {},
        'u': 1,
        'img': {
          'check_attributes': {
            'width': 'numbers',
            'alt': 'alt',
            'src': 'url',
            'height': 'numbers'
          }
        },
        'a':  {
          check_attributes: {
            'href': 'url' // important to avoid XSS
          },
          'set_attributes': {
            'target': '_blank',
            'rel': 'nofollow'
          }
        },
        'span': 1,
        'div': 1,
        'small': 1,
        // to allow save and edit files with code tag hacks
        'code': 1,
        'pre': 1
      }
    },
    locale: 'en',
    shortcuts: {
      '83': 'small'     // S
    }
    
  };

  if (typeof $.fn.wysihtml5.defaultOptionsCache === 'undefined') {
    $.fn.wysihtml5.defaultOptionsCache = $.extend(true, {}, $.fn.wysihtml5.defaultOptions);
  }

  var locale = $.fn.wysihtml5.locale = {};
};
bsWysihtml5($, wysihtml5);
}));
(function(wysihtml5) {
  wysihtml5.commands.small = {
    exec: function(composer, command) {
      return wysihtml5.commands.formatInline.exec(composer, command, "small");
    },

    state: function(composer, command) {
      return wysihtml5.commands.formatInline.state(composer, command, "small");
    }
  };
})(wysihtml5);

/**
 * English translation for bootstrap-wysihtml5
 */
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define('bootstrap.wysihtml5.en-US', ['jquery', 'bootstrap.wysihtml5'], factory);
    } else {
        // Browser globals
        factory(jQuery);
    }
}(function ($) {
  $.fn.wysihtml5.locale.en = $.fn.wysihtml5.locale['en-US'] = {
    font_styles: {
      normal: 'Normal text',
      h1: 'Heading 1',
      h2: 'Heading 2',
      h3: 'Heading 3',
      h4: 'Heading 4',
      h5: 'Heading 5',
      h6: 'Heading 6'
    },
    emphasis: {
      bold: 'Bold',
      italic: 'Italic',
      underline: 'Underline',
      small: 'Small'
    },
    lists: {
      unordered: 'Unordered list',
      ordered: 'Ordered list',
      outdent: 'Outdent',
      indent: 'Indent'
    },
    link: {
      insert: 'Insert link',
      cancel: 'Cancel',
      target: 'Open link in new window'
    },
    image: {
      insert: 'Insert image',
      cancel: 'Cancel'
    },
    html: {
      edit: 'Edit HTML'
    },
    colours: {
      black: 'Black',
      silver: 'Silver',
      gray: 'Grey',
      maroon: 'Maroon',
      red: 'Red',
      purple: 'Purple',
      green: 'Green',
      olive: 'Olive',
      navy: 'Navy',
      blue: 'Blue',
      orange: 'Orange'
    }
  };
}));

Object.defineProperty&&Object.getOwnPropertyDescriptor&&Object.getOwnPropertyDescriptor(Element.prototype,"textContent")&&!Object.getOwnPropertyDescriptor(Element.prototype,"textContent").get&&!function(){var e=Object.getOwnPropertyDescriptor(Element.prototype,"innerText");Object.defineProperty(Element.prototype,"textContent",{get:function(){return e.get.call(this)},set:function(t){return e.set.call(this,t)}})}(),Array.isArray||(Array.isArray=function(e){return"[object Array]"===Object.prototype.toString.call(e)});var wysihtml5={version:"0.4.13",commands:{},dom:{},quirks:{},toolbar:{},lang:{},selection:{},views:{},INVISIBLE_SPACE:"\ufeff",EMPTY_FUNCTION:function(){},ELEMENT_NODE:1,TEXT_NODE:3,BACKSPACE_KEY:8,ENTER_KEY:13,ESCAPE_KEY:27,SPACE_KEY:32,DELETE_KEY:46};!function(e,t){"function"==typeof define&&define.amd?define(e):t.rangy=e()}(function(){function e(e,t){var n=typeof e[t];return n==v||!(n!=y||!e[t])||"unknown"==n}function t(e,t){return!(typeof e[t]!=y||!e[t])}function n(e,t){return typeof e[t]!=w}function o(e){return function(t,n){for(var o=n.length;o--;)if(!e(t,n[o]))return!1;return!0}}function r(e){return e&&E(e,x)&&T(e,N)}function i(e){return t(e,"body")?e.body:e.getElementsByTagName("body")[0]}function a(n){t(window,"console")&&e(window.console,"log")&&window.console.log(n)}function s(e,t){t?window.alert(e):a(e)}function l(e){A.initialized=!0,A.supported=!1,s("Rangy is not supported on this page in your browser. Reason: "+e,A.config.alertOnFail)}function c(e){s("Rangy warning: "+e,A.config.alertOnWarn)}function d(e){return e.message||e.description||String(e)}function u(){if(!A.initialized){var t,n=!1,o=!1;e(document,"createRange")&&(t=document.createRange(),E(t,C)&&T(t,b)&&(n=!0));var s=i(document);if(!s||"body"!=s.nodeName.toLowerCase())return void l("No body element found");if(s&&e(s,"createTextRange")&&(t=s.createTextRange(),r(t)&&(o=!0)),!n&&!o)return void l("Neither Range nor TextRange are available");A.initialized=!0,A.features={implementsDomRange:n,implementsTextRange:o};var c,u;for(var h in R)(c=R[h])instanceof f&&c.init(c,A);for(var m=0,p=I.length;p>m;++m)try{I[m](A)}catch(g){u="Rangy init listener threw an exception. Continuing. Detail: "+d(g),a(u)}}}function h(e){e=e||window,u();for(var t=0,n=O.length;n>t;++t)O[t](e)}function f(e,t,n){this.name=e,this.dependencies=t,this.initialized=!1,this.supported=!1,this.initializer=n}function m(e,t,n,o){var r=new f(t,n,function(e){if(!e.initialized){e.initialized=!0;try{o(A,e),e.supported=!0}catch(n){var r="Module '"+t+"' failed to load: "+d(n);a(r)}}});R[t]=r}function p(){}function g(){}var y="object",v="function",w="undefined",b=["startContainer","startOffset","endContainer","endOffset","collapsed","commonAncestorContainer"],C=["setStart","setStartBefore","setStartAfter","setEnd","setEndBefore","setEndAfter","collapse","selectNode","selectNodeContents","compareBoundaryPoints","deleteContents","extractContents","cloneContents","insertNode","surroundContents","cloneRange","toString","detach"],N=["boundingHeight","boundingLeft","boundingTop","boundingWidth","htmlText","text"],x=["collapse","compareEndPoints","duplicate","moveToElementText","parentElement","select","setEndPoint","getBoundingClientRect"],E=o(e),S=o(t),T=o(n),R={},A={version:"1.3alpha.20140804",initialized:!1,supported:!0,util:{isHostMethod:e,isHostObject:t,isHostProperty:n,areHostMethods:E,areHostObjects:S,areHostProperties:T,isTextRange:r,getBody:i},features:{},modules:R,config:{alertOnFail:!0,alertOnWarn:!1,preferTextRange:!1,autoInitialize:typeof rangyAutoInitialize==w?!0:rangyAutoInitialize}};A.fail=l,A.warn=c,{}.hasOwnProperty?A.util.extend=function(e,t,n){var o,r;for(var i in t)t.hasOwnProperty(i)&&(o=e[i],r=t[i],n&&null!==o&&"object"==typeof o&&null!==r&&"object"==typeof r&&A.util.extend(o,r,!0),e[i]=r);return t.hasOwnProperty("toString")&&(e.toString=t.toString),e}:l("hasOwnProperty not supported"),function(){var e=document.createElement("div");e.appendChild(document.createElement("span"));var t,n=[].slice;try{1==n.call(e.childNodes,0)[0].nodeType&&(t=function(e){return n.call(e,0)})}catch(o){}t||(t=function(e){for(var t=[],n=0,o=e.length;o>n;++n)t[n]=e[n];return t}),A.util.toArray=t}();var _;e(document,"addEventListener")?_=function(e,t,n){e.addEventListener(t,n,!1)}:e(document,"attachEvent")?_=function(e,t,n){e.attachEvent("on"+t,n)}:l("Document does not have required addEventListener or attachEvent method"),A.util.addListener=_;var I=[];A.init=u,A.addInitListener=function(e){A.initialized?e(A):I.push(e)};var O=[];A.addShimListener=function(e){O.push(e)},A.shim=A.createMissingNativeApi=h,f.prototype={init:function(){for(var e,t,n=this.dependencies||[],o=0,r=n.length;r>o;++o){if(t=n[o],e=R[t],!(e&&e instanceof f))throw new Error("required module '"+t+"' not found");if(e.init(),!e.supported)throw new Error("required module '"+t+"' not supported")}this.initializer(this)},fail:function(e){throw this.initialized=!0,this.supported=!1,new Error("Module '"+this.name+"' failed to load: "+e)},warn:function(e){A.warn("Module "+this.name+": "+e)},deprecationNotice:function(e,t){A.warn("DEPRECATED: "+e+" in module "+this.name+"is deprecated. Please use "+t+" instead")},createError:function(e){return new Error("Error in Rangy "+this.name+" module: "+e)}},A.createModule=function(e){var t,n;2==arguments.length?(t=arguments[1],n=[]):(t=arguments[2],n=arguments[1]);var o=m(!1,e,n,t);A.initialized&&o.init()},A.createCoreModule=function(e,t,n){m(!0,e,t,n)},A.RangePrototype=p,A.rangePrototype=new p,A.selectionPrototype=new g;var L=!1,k=function(e){L||(L=!0,!A.initialized&&A.config.autoInitialize&&u())};return typeof window==w?void l("No window found"):typeof document==w?void l("No document found"):(e(document,"addEventListener")&&document.addEventListener("DOMContentLoaded",k,!1),_(window,"load",k),A.createCoreModule("DomUtil",[],function(e,t){function n(e){var t;return typeof e.namespaceURI==_||null===(t=e.namespaceURI)||"http://www.w3.org/1999/xhtml"==t}function o(e){var t=e.parentNode;return 1==t.nodeType?t:null}function r(e){for(var t=0;e=e.previousSibling;)++t;return t}function i(e){switch(e.nodeType){case 7:case 10:return 0;case 3:case 8:return e.length;default:return e.childNodes.length}}function a(e,t){var n,o=[];for(n=e;n;n=n.parentNode)o.push(n);for(n=t;n;n=n.parentNode)if(k(o,n))return n;return null}function s(e,t,n){for(var o=n?t:t.parentNode;o;){if(o===e)return!0;o=o.parentNode}return!1}function l(e,t){return s(e,t,!0)}function c(e,t,n){for(var o,r=n?e:e.parentNode;r;){if(o=r.parentNode,o===t)return r;r=o}return null}function d(e){var t=e.nodeType;return 3==t||4==t||8==t}function u(e){if(!e)return!1;var t=e.nodeType;return 3==t||8==t}function h(e,t){var n=t.nextSibling,o=t.parentNode;return n?o.insertBefore(e,n):o.appendChild(e),e}function f(e,t,n){var o=e.cloneNode(!1);if(o.deleteData(0,t),e.deleteData(t,e.length-t),h(o,e),n)for(var i,a=0;i=n[a++];)i.node==e&&i.offset>t?(i.node=o,i.offset-=t):i.node==e.parentNode&&i.offset>r(e)&&++i.offset;return o}function m(e){if(9==e.nodeType)return e;if(typeof e.ownerDocument!=_)return e.ownerDocument;if(typeof e.document!=_)return e.document;if(e.parentNode)return m(e.parentNode);throw t.createError("getDocument: no document found for node")}function p(e){var n=m(e);if(typeof n.defaultView!=_)return n.defaultView;if(typeof n.parentWindow!=_)return n.parentWindow;throw t.createError("Cannot get a window object for node")}function g(e){if(typeof e.contentDocument!=_)return e.contentDocument;if(typeof e.contentWindow!=_)return e.contentWindow.document;throw t.createError("getIframeDocument: No Document object found for iframe element")}function y(e){if(typeof e.contentWindow!=_)return e.contentWindow;if(typeof e.contentDocument!=_)return e.contentDocument.defaultView;throw t.createError("getIframeWindow: No Window object found for iframe element")}function v(e){return e&&I.isHostMethod(e,"setTimeout")&&I.isHostObject(e,"document")}function w(e,t,n){var o;if(e?I.isHostProperty(e,"nodeType")?o=1==e.nodeType&&"iframe"==e.tagName.toLowerCase()?g(e):m(e):v(e)&&(o=e.document):o=document,!o)throw t.createError(n+"(): Parameter must be a Window object or DOM node");return o}function b(e){for(var t;t=e.parentNode;)e=t;return e}function C(e,n,o,i){var s,l,d,u,h;if(e==o)return n===i?0:i>n?-1:1;if(s=c(o,e,!0))return n<=r(s)?-1:1;if(s=c(e,o,!0))return r(s)<i?-1:1;if(l=a(e,o),!l)throw new Error("comparePoints error: nodes have no common ancestor");if(d=e===l?l:c(e,l,!0),u=o===l?l:c(o,l,!0),d===u)throw t.createError("comparePoints got to case 4 and childA and childB are the same!");for(h=l.firstChild;h;){if(h===d)return-1;if(h===u)return 1;h=h.nextSibling}}function N(e){var t;try{return t=e.parentNode,!1}catch(n){return!0}}function x(e){if(!e)return"[No node]";if(D&&N(e))return"[Broken node]";if(d(e))return'"'+e.data+'"';if(1==e.nodeType){var t=e.id?' id="'+e.id+'"':"";return"<"+e.nodeName+t+">[index:"+r(e)+",length:"+e.childNodes.length+"]["+(e.innerHTML||"[innerHTML not supported]").slice(0,25)+"]"}return e.nodeName}function E(e){for(var t,n=m(e).createDocumentFragment();t=e.firstChild;)n.appendChild(t);return n}function S(e){this.root=e,this._next=e}function T(e){return new S(e)}function R(e,t){this.node=e,this.offset=t}function A(e){this.code=this[e],this.codeName=e,this.message="DOMException: "+this.codeName}var _="undefined",I=e.util;I.areHostMethods(document,["createDocumentFragment","createElement","createTextNode"])||t.fail("document missing a Node creation method"),I.isHostMethod(document,"getElementsByTagName")||t.fail("document missing getElementsByTagName method");var O=document.createElement("div");I.areHostMethods(O,["insertBefore","appendChild","cloneNode"]||!I.areHostObjects(O,["previousSibling","nextSibling","childNodes","parentNode"]))||t.fail("Incomplete Element implementation"),I.isHostProperty(O,"innerHTML")||t.fail("Element is missing innerHTML property");var L=document.createTextNode("test");I.areHostMethods(L,["splitText","deleteData","insertData","appendData","cloneNode"]||!I.areHostObjects(O,["previousSibling","nextSibling","childNodes","parentNode"])||!I.areHostProperties(L,["data"]))||t.fail("Incomplete Text Node implementation");var k=function(e,t){for(var n=e.length;n--;)if(e[n]===t)return!0;return!1},D=!1;!function(){var t=document.createElement("b");t.innerHTML="1";var n=t.firstChild;t.innerHTML="<br>",D=N(n),e.features.crashyTextNodes=D}();var B;typeof window.getComputedStyle!=_?B=function(e,t){return p(e).getComputedStyle(e,null)[t]}:typeof document.documentElement.currentStyle!=_?B=function(e,t){return e.currentStyle[t]}:t.fail("No means of obtaining computed style properties found"),S.prototype={_current:null,hasNext:function(){return!!this._next},next:function(){var e,t,n=this._current=this._next;if(this._current)if(e=n.firstChild)this._next=e;else{for(t=null;n!==this.root&&!(t=n.nextSibling);)n=n.parentNode;this._next=t}return this._current},detach:function(){this._current=this._next=this.root=null}},R.prototype={equals:function(e){return!!e&&this.node===e.node&&this.offset==e.offset},inspect:function(){return"[DomPosition("+x(this.node)+":"+this.offset+")]"},toString:function(){return this.inspect()}},A.prototype={INDEX_SIZE_ERR:1,HIERARCHY_REQUEST_ERR:3,WRONG_DOCUMENT_ERR:4,NO_MODIFICATION_ALLOWED_ERR:7,NOT_FOUND_ERR:8,NOT_SUPPORTED_ERR:9,INVALID_STATE_ERR:11,INVALID_NODE_TYPE_ERR:24},A.prototype.toString=function(){return this.message},e.dom={arrayContains:k,isHtmlNamespace:n,parentElement:o,getNodeIndex:r,getNodeLength:i,getCommonAncestor:a,isAncestorOf:s,isOrIsAncestorOf:l,getClosestAncestorIn:c,isCharacterDataNode:d,isTextOrCommentNode:u,insertAfter:h,splitDataNode:f,getDocument:m,getWindow:p,getIframeWindow:y,getIframeDocument:g,getBody:I.getBody,isWindow:v,getContentDocument:w,getRootContainer:b,comparePoints:C,isBrokenNode:N,inspectNode:x,getComputedStyleProperty:B,fragmentFromNodeChildren:E,createIterator:T,DomPosition:R},e.DOMException=A}),A.createCoreModule("DomRange",["DomUtil"],function(e,t){function n(e,t){return 3!=e.nodeType&&(V(e,t.startContainer)||V(e,t.endContainer))}function o(e){return e.document||W(e.startContainer)}function r(e){return new P(e.parentNode,q(e))}function i(e){return new P(e.parentNode,q(e)+1)}function a(e,t,n){var o=11==e.nodeType?e.firstChild:e;return z(t)?n==t.length?B.insertAfter(e,t):t.parentNode.insertBefore(e,0==n?t:j(t,n)):n>=t.childNodes.length?t.appendChild(e):t.insertBefore(e,t.childNodes[n]),o}function s(e,t,n){if(S(e),S(t),o(t)!=o(e))throw new H("WRONG_DOCUMENT_ERR");var r=U(e.startContainer,e.startOffset,t.endContainer,t.endOffset),i=U(e.endContainer,e.endOffset,t.startContainer,t.startOffset);return n?0>=r&&i>=0:0>r&&i>0}function l(e){for(var t,n,r,i=o(e.range).createDocumentFragment();n=e.next();){if(t=e.isPartiallySelectedSubtree(),n=n.cloneNode(!t),t&&(r=e.getSubtreeIterator(),n.appendChild(l(r)),r.detach()),10==n.nodeType)throw new H("HIERARCHY_REQUEST_ERR");i.appendChild(n)}return i}function c(e,t,n){var o,r;n=n||{stop:!1};for(var i,a;i=e.next();)if(e.isPartiallySelectedSubtree()){if(t(i)===!1)return void(n.stop=!0);if(a=e.getSubtreeIterator(),c(a,t,n),a.detach(),n.stop)return}else for(o=B.createIterator(i);r=o.next();)if(t(r)===!1)return void(n.stop=!0)}function d(e){for(var t;e.next();)e.isPartiallySelectedSubtree()?(t=e.getSubtreeIterator(),d(t),t.detach()):e.remove()}function u(e){for(var t,n,r=o(e.range).createDocumentFragment();t=e.next();){if(e.isPartiallySelectedSubtree()?(t=t.cloneNode(!1),n=e.getSubtreeIterator(),t.appendChild(u(n)),n.detach()):e.remove(),10==t.nodeType)throw new H("HIERARCHY_REQUEST_ERR");r.appendChild(t)}return r}function h(e,t,n){var o,r=!(!t||!t.length),i=!!n;r&&(o=new RegExp("^("+t.join("|")+")$"));var a=[];return c(new m(e,!1),function(t){if((!r||o.test(t.nodeType))&&(!i||n(t))){var s=e.startContainer;if(t!=s||!z(s)||e.startOffset!=s.length){var l=e.endContainer;t==l&&z(l)&&0==e.endOffset||a.push(t)}}}),a}function f(e){var t="undefined"==typeof e.getName?"Range":e.getName();return"["+t+"("+B.inspectNode(e.startContainer)+":"+e.startOffset+", "+B.inspectNode(e.endContainer)+":"+e.endOffset+")]"}function m(e,t){if(this.range=e,this.clonePartiallySelectedTextNodes=t,!e.collapsed){this.sc=e.startContainer,this.so=e.startOffset,this.ec=e.endContainer,this.eo=e.endOffset;var n=e.commonAncestorContainer;this.sc===this.ec&&z(this.sc)?(this.isSingleCharacterDataNode=!0,this._first=this._last=this._next=this.sc):(this._first=this._next=this.sc!==n||z(this.sc)?F(this.sc,n,!0):this.sc.childNodes[this.so],this._last=this.ec!==n||z(this.ec)?F(this.ec,n,!0):this.ec.childNodes[this.eo-1])}}function p(e){return function(t,n){for(var o,r=n?t:t.parentNode;r;){if(o=r.nodeType,$(e,o))return r;r=r.parentNode}return null}}function g(e,t){if(oe(e,t))throw new H("INVALID_NODE_TYPE_ERR")}function y(e,t){if(!$(t,e.nodeType))throw new H("INVALID_NODE_TYPE_ERR")}function v(e,t){if(0>t||t>(z(e)?e.length:e.childNodes.length))throw new H("INDEX_SIZE_ERR")}function w(e,t){if(te(e,!0)!==te(t,!0))throw new H("WRONG_DOCUMENT_ERR")}function b(e){if(ne(e,!0))throw new H("NO_MODIFICATION_ALLOWED_ERR")}function C(e,t){if(!e)throw new H(t)}function N(e){return X&&B.isBrokenNode(e)||!$(Q,e.nodeType)&&!te(e,!0)}function x(e,t){return t<=(z(e)?e.length:e.childNodes.length)}function E(e){return!!e.startContainer&&!!e.endContainer&&!N(e.startContainer)&&!N(e.endContainer)&&x(e.startContainer,e.startOffset)&&x(e.endContainer,e.endOffset)}function S(e){if(!E(e))throw new Error("Range error: Range is no longer valid after DOM mutation ("+e.inspect()+")")}function T(e,t){S(e);var n=e.startContainer,o=e.startOffset,r=e.endContainer,i=e.endOffset,a=n===r;z(r)&&i>0&&i<r.length&&j(r,i,t),z(n)&&o>0&&o<n.length&&(n=j(n,o,t),a?(i-=o,r=n):r==n.parentNode&&i>=q(n)&&i++,o=0),e.setStartAndEnd(n,o,r,i)}function R(e){S(e);var t=e.commonAncestorContainer.parentNode.cloneNode(!1);return t.appendChild(e.cloneContents()),t.innerHTML}function A(e){e.START_TO_START=ce,e.START_TO_END=de,e.END_TO_END=ue,e.END_TO_START=he,e.NODE_BEFORE=fe,e.NODE_AFTER=me,e.NODE_BEFORE_AND_AFTER=pe,e.NODE_INSIDE=ge}function _(e){A(e),A(e.prototype)}function I(e,t){return function(){S(this);var n,o,r=this.startContainer,a=this.startOffset,s=this.commonAncestorContainer,l=new m(this,!0);r!==s&&(n=F(r,s,!0),o=i(n),r=o.node,a=o.offset),c(l,b),l.reset();var d=e(l);return l.detach(),t(this,r,a,r,a),d}}function O(t,o){function a(e,t){return function(n){y(n,Y),y(G(n),Q);var o=(e?r:i)(n);(t?s:l)(this,o.node,o.offset)}}function s(e,t,n){var r=e.endContainer,i=e.endOffset;(t!==e.startContainer||n!==e.startOffset)&&((G(t)!=G(r)||1==U(t,n,r,i))&&(r=t,i=n),o(e,t,n,r,i))}function l(e,t,n){var r=e.startContainer,i=e.startOffset;(t!==e.endContainer||n!==e.endOffset)&&((G(t)!=G(r)||-1==U(t,n,r,i))&&(r=t,i=n),o(e,r,i,t,n))}var c=function(){};c.prototype=e.rangePrototype,t.prototype=new c,M.extend(t.prototype,{setStart:function(e,t){g(e,!0),v(e,t),s(this,e,t)},setEnd:function(e,t){g(e,!0),v(e,t),l(this,e,t)},setStartAndEnd:function(){var e=arguments,t=e[0],n=e[1],r=t,i=n;switch(e.length){case 3:i=e[2];break;case 4:r=e[2],i=e[3]}o(this,t,n,r,i)},setBoundary:function(e,t,n){this["set"+(n?"Start":"End")](e,t)},setStartBefore:a(!0,!0),setStartAfter:a(!1,!0),setEndBefore:a(!0,!1),setEndAfter:a(!1,!1),collapse:function(e){S(this),e?o(this,this.startContainer,this.startOffset,this.startContainer,this.startOffset):o(this,this.endContainer,this.endOffset,this.endContainer,this.endOffset)},selectNodeContents:function(e){g(e,!0),o(this,e,0,e,K(e))},selectNode:function(e){g(e,!1),y(e,Y);var t=r(e),n=i(e);o(this,t.node,t.offset,n.node,n.offset)},extractContents:I(u,o),deleteContents:I(d,o),canSurroundContents:function(){S(this),b(this.startContainer),b(this.endContainer);var e=new m(this,!0),t=e._first&&n(e._first,this)||e._last&&n(e._last,this);return e.detach(),!t},splitBoundaries:function(){T(this)},splitBoundariesPreservingPositions:function(e){T(this,e)},normalizeBoundaries:function(){S(this);var e=this.startContainer,t=this.startOffset,n=this.endContainer,r=this.endOffset,i=function(e){var t=e.nextSibling;t&&t.nodeType==e.nodeType&&(n=e,r=e.length,e.appendData(t.data),t.parentNode.removeChild(t))},a=function(o){var i=o.previousSibling;if(i&&i.nodeType==o.nodeType){e=o;var a=o.length;if(t=i.length,o.insertData(0,i.data),i.parentNode.removeChild(i),e==n)r+=t,n=e;else if(n==o.parentNode){var s=q(o);r==s?(n=o,r=a):r>s&&r--}}},s=!0;if(z(n))n.length==r&&i(n);else{if(r>0){var l=n.childNodes[r-1];l&&z(l)&&i(l)}s=!this.collapsed}if(s){if(z(e))0==t&&a(e);else if(t<e.childNodes.length){var c=e.childNodes[t];c&&z(c)&&a(c)}}else e=n,t=r;o(this,e,t,n,r)},collapseToPoint:function(e,t){g(e,!0),v(e,t),this.setStartAndEnd(e,t)}}),_(t)}function L(e){e.collapsed=e.startContainer===e.endContainer&&e.startOffset===e.endOffset,e.commonAncestorContainer=e.collapsed?e.startContainer:B.getCommonAncestor(e.startContainer,e.endContainer)}function k(e,t,n,o,r){e.startContainer=t,e.startOffset=n,e.endContainer=o,e.endOffset=r,e.document=B.getDocument(t),L(e)}function D(e){this.startContainer=e,this.startOffset=0,this.endContainer=e,this.endOffset=0,this.document=e,L(this)}var B=e.dom,M=e.util,P=B.DomPosition,H=e.DOMException,z=B.isCharacterDataNode,q=B.getNodeIndex,V=B.isOrIsAncestorOf,W=B.getDocument,U=B.comparePoints,j=B.splitDataNode,F=B.getClosestAncestorIn,K=B.getNodeLength,$=B.arrayContains,G=B.getRootContainer,X=e.features.crashyTextNodes;m.prototype={_current:null,_next:null,_first:null,_last:null,isSingleCharacterDataNode:!1,reset:function(){this._current=null,this._next=this._first},hasNext:function(){return!!this._next},next:function(){var e=this._current=this._next;return e&&(this._next=e!==this._last?e.nextSibling:null,z(e)&&this.clonePartiallySelectedTextNodes&&(e===this.ec&&(e=e.cloneNode(!0)).deleteData(this.eo,e.length-this.eo),this._current===this.sc&&(e=e.cloneNode(!0)).deleteData(0,this.so))),e},remove:function(){var e,t,n=this._current;!z(n)||n!==this.sc&&n!==this.ec?n.parentNode&&n.parentNode.removeChild(n):(e=n===this.sc?this.so:0,t=n===this.ec?this.eo:n.length,e!=t&&n.deleteData(e,t-e))},isPartiallySelectedSubtree:function(){var e=this._current;return n(e,this.range)},getSubtreeIterator:function(){var e;if(this.isSingleCharacterDataNode)e=this.range.cloneRange(),e.collapse(!1);else{e=new D(o(this.range));var t=this._current,n=t,r=0,i=t,a=K(t);V(t,this.sc)&&(n=this.sc,r=this.so),V(t,this.ec)&&(i=this.ec,a=this.eo),k(e,n,r,i,a)}return new m(e,this.clonePartiallySelectedTextNodes)},detach:function(){this.range=this._current=this._next=this._first=this._last=this.sc=this.so=this.ec=this.eo=null}};var Y=[1,3,4,5,7,8,10],Q=[2,9,11],Z=[5,6,10,12],J=[1,3,4,5,7,8,10,11],ee=[1,3,4,5,7,8],te=p([9,11]),ne=p(Z),oe=p([6,10,12]),re=document.createElement("style"),ie=!1;try{re.innerHTML="<b>x</b>",ie=3==re.firstChild.nodeType}catch(ae){}e.features.htmlParsingConforms=ie;var se=ie?function(e){var t=this.startContainer,n=W(t);if(!t)throw new H("INVALID_STATE_ERR");var o=null;return 1==t.nodeType?o=t:z(t)&&(o=B.parentElement(t)),o=null===o||"HTML"==o.nodeName&&B.isHtmlNamespace(W(o).documentElement)&&B.isHtmlNamespace(o)?n.createElement("body"):o.cloneNode(!1),o.innerHTML=e,B.fragmentFromNodeChildren(o)}:function(e){var t=o(this),n=t.createElement("body");return n.innerHTML=e,B.fragmentFromNodeChildren(n)},le=["startContainer","startOffset","endContainer","endOffset","collapsed","commonAncestorContainer"],ce=0,de=1,ue=2,he=3,fe=0,me=1,pe=2,ge=3;M.extend(e.rangePrototype,{compareBoundaryPoints:function(e,t){S(this),w(this.startContainer,t.startContainer);var n,o,r,i,a=e==he||e==ce?"start":"end",s=e==de||e==ce?"start":"end";return n=this[a+"Container"],o=this[a+"Offset"],r=t[s+"Container"],i=t[s+"Offset"],U(n,o,r,i)},insertNode:function(e){if(S(this),y(e,J),b(this.startContainer),V(e,this.startContainer))throw new H("HIERARCHY_REQUEST_ERR");var t=a(e,this.startContainer,this.startOffset);this.setStartBefore(t)},cloneContents:function(){S(this);var e,t;if(this.collapsed)return o(this).createDocumentFragment();if(this.startContainer===this.endContainer&&z(this.startContainer))return e=this.startContainer.cloneNode(!0),e.data=e.data.slice(this.startOffset,this.endOffset),t=o(this).createDocumentFragment(),t.appendChild(e),t;var n=new m(this,!0);return e=l(n),n.detach(),e},canSurroundContents:function(){S(this),b(this.startContainer),b(this.endContainer);var e=new m(this,!0),t=e._first&&n(e._first,this)||e._last&&n(e._last,this);return e.detach(),!t},surroundContents:function(e){if(y(e,ee),!this.canSurroundContents())throw new H("INVALID_STATE_ERR");var t=this.extractContents();if(e.hasChildNodes())for(;e.lastChild;)e.removeChild(e.lastChild);a(e,this.startContainer,this.startOffset),e.appendChild(t),this.selectNode(e)},cloneRange:function(){S(this);for(var e,t=new D(o(this)),n=le.length;n--;)e=le[n],t[e]=this[e];return t},toString:function(){S(this);var e=this.startContainer;if(e===this.endContainer&&z(e))return 3==e.nodeType||4==e.nodeType?e.data.slice(this.startOffset,this.endOffset):"";var t=[],n=new m(this,!0);return c(n,function(e){(3==e.nodeType||4==e.nodeType)&&t.push(e.data)}),n.detach(),t.join("")},compareNode:function(e){S(this);var t=e.parentNode,n=q(e);if(!t)throw new H("NOT_FOUND_ERR");var o=this.comparePoint(t,n),r=this.comparePoint(t,n+1);return 0>o?r>0?pe:fe:r>0?me:ge},comparePoint:function(e,t){return S(this),C(e,"HIERARCHY_REQUEST_ERR"),w(e,this.startContainer),U(e,t,this.startContainer,this.startOffset)<0?-1:U(e,t,this.endContainer,this.endOffset)>0?1:0},createContextualFragment:se,toHtml:function(){return R(this)},intersectsNode:function(e,t){if(S(this),C(e,"NOT_FOUND_ERR"),W(e)!==o(this))return!1;var n=e.parentNode,r=q(e);C(n,"NOT_FOUND_ERR");var i=U(n,r,this.endContainer,this.endOffset),a=U(n,r+1,this.startContainer,this.startOffset);return t?0>=i&&a>=0:0>i&&a>0},isPointInRange:function(e,t){return S(this),C(e,"HIERARCHY_REQUEST_ERR"),w(e,this.startContainer),U(e,t,this.startContainer,this.startOffset)>=0&&U(e,t,this.endContainer,this.endOffset)<=0},intersectsRange:function(e){return s(this,e,!1)},intersectsOrTouchesRange:function(e){return s(this,e,!0)},intersection:function(e){if(this.intersectsRange(e)){var t=U(this.startContainer,this.startOffset,e.startContainer,e.startOffset),n=U(this.endContainer,this.endOffset,e.endContainer,e.endOffset),o=this.cloneRange();return-1==t&&o.setStart(e.startContainer,e.startOffset),1==n&&o.setEnd(e.endContainer,e.endOffset),o}return null},union:function(e){if(this.intersectsOrTouchesRange(e)){var t=this.cloneRange();return-1==U(e.startContainer,e.startOffset,this.startContainer,this.startOffset)&&t.setStart(e.startContainer,e.startOffset),1==U(e.endContainer,e.endOffset,this.endContainer,this.endOffset)&&t.setEnd(e.endContainer,e.endOffset),t}throw new H("Ranges do not intersect")},containsNode:function(e,t){return t?this.intersectsNode(e,!1):this.compareNode(e)==ge},containsNodeContents:function(e){return this.comparePoint(e,0)>=0&&this.comparePoint(e,K(e))<=0},containsRange:function(e){var t=this.intersection(e);return null!==t&&e.equals(t)},containsNodeText:function(e){var t=this.cloneRange();t.selectNode(e);var n=t.getNodes([3]);if(n.length>0){t.setStart(n[0],0);var o=n.pop();return t.setEnd(o,o.length),this.containsRange(t)}return this.containsNodeContents(e)},getNodes:function(e,t){return S(this),h(this,e,t)},getDocument:function(){return o(this)},collapseBefore:function(e){this.setEndBefore(e),this.collapse(!1)},collapseAfter:function(e){this.setStartAfter(e),this.collapse(!0)},getBookmark:function(t){var n=o(this),r=e.createRange(n);t=t||B.getBody(n),r.selectNodeContents(t);var i=this.intersection(r),a=0,s=0;return i&&(r.setEnd(i.startContainer,i.startOffset),a=r.toString().length,s=a+i.toString().length),{start:a,end:s,containerNode:t}},moveToBookmark:function(e){var t=e.containerNode,n=0;this.setStart(t,0),this.collapse(!0);for(var o,r,i,a,s=[t],l=!1,c=!1;!c&&(o=s.pop());)if(3==o.nodeType)r=n+o.length,!l&&e.start>=n&&e.start<=r&&(this.setStart(o,e.start-n),l=!0),l&&e.end>=n&&e.end<=r&&(this.setEnd(o,e.end-n),c=!0),n=r;else for(a=o.childNodes,i=a.length;i--;)s.push(a[i])},getName:function(){return"DomRange"},equals:function(e){return D.rangesEqual(this,e)},isValid:function(){return E(this)},inspect:function(){return f(this)},detach:function(){}}),O(D,k),M.extend(D,{rangeProperties:le,RangeIterator:m,copyComparisonConstants:_,createPrototypeRange:O,inspect:f,toHtml:R,getRangeDocument:o,rangesEqual:function(e,t){return e.startContainer===t.startContainer&&e.startOffset===t.startOffset&&e.endContainer===t.endContainer&&e.endOffset===t.endOffset}}),e.DomRange=D}),A.createCoreModule("WrappedRange",["DomRange"],function(e,t){var n,o,r=e.dom,i=e.util,a=r.DomPosition,s=e.DomRange,l=r.getBody,c=r.getContentDocument,d=r.isCharacterDataNode;if(e.features.implementsDomRange&&!function(){function o(e){for(var t,n=h.length;n--;)t=h[n],e[t]=e.nativeRange[t];e.collapsed=e.startContainer===e.endContainer&&e.startOffset===e.endOffset}function a(e,t,n,o,r){var i=e.startContainer!==t||e.startOffset!=n,a=e.endContainer!==o||e.endOffset!=r,s=!e.equals(e.nativeRange);(i||a||s)&&(e.setEnd(o,r),e.setStart(t,n))}var d,u,h=s.rangeProperties;n=function(e){if(!e)throw t.createError("WrappedRange: Range must be specified");this.nativeRange=e,o(this)},s.createPrototypeRange(n,a),d=n.prototype,d.selectNode=function(e){this.nativeRange.selectNode(e),o(this)},d.cloneContents=function(){return this.nativeRange.cloneContents()},d.surroundContents=function(e){this.nativeRange.surroundContents(e),o(this)},d.collapse=function(e){this.nativeRange.collapse(e),o(this)},d.cloneRange=function(){return new n(this.nativeRange.cloneRange())},d.refresh=function(){o(this)},d.toString=function(){return this.nativeRange.toString()};var f=document.createTextNode("test");l(document).appendChild(f);var m=document.createRange();m.setStart(f,0),m.setEnd(f,0);try{m.setStart(f,1),d.setStart=function(e,t){this.nativeRange.setStart(e,t),o(this)},d.setEnd=function(e,t){this.nativeRange.setEnd(e,t),o(this)},u=function(e){return function(t){this.nativeRange[e](t),o(this)}}}catch(p){d.setStart=function(e,t){try{this.nativeRange.setStart(e,t)}catch(n){this.nativeRange.setEnd(e,t),this.nativeRange.setStart(e,t)}o(this)},d.setEnd=function(e,t){try{this.nativeRange.setEnd(e,t)}catch(n){this.nativeRange.setStart(e,t),this.nativeRange.setEnd(e,t)}o(this)},u=function(e,t){return function(n){try{this.nativeRange[e](n)}catch(r){this.nativeRange[t](n),this.nativeRange[e](n)}o(this)}}}d.setStartBefore=u("setStartBefore","setEndBefore"),d.setStartAfter=u("setStartAfter","setEndAfter"),d.setEndBefore=u("setEndBefore","setStartBefore"),d.setEndAfter=u("setEndAfter","setStartAfter"),d.selectNodeContents=function(e){this.setStartAndEnd(e,0,r.getNodeLength(e))},m.selectNodeContents(f),m.setEnd(f,3);var g=document.createRange();g.selectNodeContents(f),g.setEnd(f,4),g.setStart(f,2),-1==m.compareBoundaryPoints(m.START_TO_END,g)&&1==m.compareBoundaryPoints(m.END_TO_START,g)?d.compareBoundaryPoints=function(e,t){return t=t.nativeRange||t,e==t.START_TO_END?e=t.END_TO_START:e==t.END_TO_START&&(e=t.START_TO_END),this.nativeRange.compareBoundaryPoints(e,t)}:d.compareBoundaryPoints=function(e,t){return this.nativeRange.compareBoundaryPoints(e,t.nativeRange||t)};var y=document.createElement("div");y.innerHTML="123";var v=y.firstChild,w=l(document);w.appendChild(y),m.setStart(v,1),m.setEnd(v,2),m.deleteContents(),"13"==v.data&&(d.deleteContents=function(){this.nativeRange.deleteContents(),o(this)},d.extractContents=function(){var e=this.nativeRange.extractContents();return o(this),e}),w.removeChild(y),w=null,i.isHostMethod(m,"createContextualFragment")&&(d.createContextualFragment=function(e){return this.nativeRange.createContextualFragment(e)}),l(document).removeChild(f),d.getName=function(){return"WrappedRange"},e.WrappedRange=n,e.createNativeRange=function(e){return e=c(e,t,"createNativeRange"),e.createRange()}}(),e.features.implementsTextRange){var u=function(e){var t=e.parentElement(),n=e.duplicate();n.collapse(!0);var o=n.parentElement();n=e.duplicate(),n.collapse(!1);var i=n.parentElement(),a=o==i?o:r.getCommonAncestor(o,i);return a==t?a:r.getCommonAncestor(t,a)},h=function(e){return 0==e.compareEndPoints("StartToEnd",e)},f=function(e,t,n,o,i){var s=e.duplicate();s.collapse(n);var l=s.parentElement();if(r.isOrIsAncestorOf(t,l)||(l=t),!l.canHaveHTML){var c=new a(l.parentNode,r.getNodeIndex(l));return{boundaryPosition:c,nodeInfo:{nodeIndex:c.offset,containerElement:c.node}}}var u=r.getDocument(l).createElement("span");u.parentNode&&u.parentNode.removeChild(u);for(var h,f,m,p,g,y=n?"StartToStart":"StartToEnd",v=i&&i.containerElement==l?i.nodeIndex:0,w=l.childNodes.length,b=w,C=b;;){if(C==w?l.appendChild(u):l.insertBefore(u,l.childNodes[C]),s.moveToElementText(u),h=s.compareEndPoints(y,e),0==h||v==b)break;if(-1==h){if(b==v+1)break;v=C}else b=b==v+1?v:C;C=Math.floor((v+b)/2),l.removeChild(u)}if(g=u.nextSibling,-1==h&&g&&d(g)){s.setEndPoint(n?"EndToStart":"EndToEnd",e);var N;if(/[\r\n]/.test(g.data)){var x=s.duplicate(),E=x.text.replace(/\r\n/g,"\r").length;for(N=x.moveStart("character",E);-1==(h=x.compareEndPoints("StartToEnd",x));)N++,x.moveStart("character",1)}else N=s.text.length;p=new a(g,N)}else f=(o||!n)&&u.previousSibling,m=(o||n)&&u.nextSibling,p=m&&d(m)?new a(m,0):f&&d(f)?new a(f,f.data.length):new a(l,r.getNodeIndex(u));return u.parentNode.removeChild(u),{boundaryPosition:p,nodeInfo:{nodeIndex:C,containerElement:l}}},m=function(e,t){var n,o,i,a,s=e.offset,c=r.getDocument(e.node),u=l(c).createTextRange(),h=d(e.node);return h?(n=e.node,o=n.parentNode):(a=e.node.childNodes,n=s<a.length?a[s]:null,o=e.node),i=c.createElement("span"),i.innerHTML="&#feff;",n?o.insertBefore(i,n):o.appendChild(i),u.moveToElementText(i),u.collapse(!t),o.removeChild(i),h&&u[t?"moveStart":"moveEnd"]("character",s),u};o=function(e){this.textRange=e,this.refresh()},o.prototype=new s(document),
o.prototype.refresh=function(){var e,t,n,o=u(this.textRange);h(this.textRange)?t=e=f(this.textRange,o,!0,!0).boundaryPosition:(n=f(this.textRange,o,!0,!1),e=n.boundaryPosition,t=f(this.textRange,o,!1,!1,n.nodeInfo).boundaryPosition),this.setStart(e.node,e.offset),this.setEnd(t.node,t.offset)},o.prototype.getName=function(){return"WrappedTextRange"},s.copyComparisonConstants(o);var p=function(e){if(e.collapsed)return m(new a(e.startContainer,e.startOffset),!0);var t=m(new a(e.startContainer,e.startOffset),!0),n=m(new a(e.endContainer,e.endOffset),!1),o=l(s.getRangeDocument(e)).createTextRange();return o.setEndPoint("StartToStart",t),o.setEndPoint("EndToEnd",n),o};if(o.rangeToTextRange=p,o.prototype.toTextRange=function(){return p(this)},e.WrappedTextRange=o,!e.features.implementsDomRange||e.config.preferTextRange){var g=function(){return this}();"undefined"==typeof g.Range&&(g.Range=o),e.createNativeRange=function(e){return e=c(e,t,"createNativeRange"),l(e).createTextRange()},e.WrappedRange=o}}e.createRange=function(n){return n=c(n,t,"createRange"),new e.WrappedRange(e.createNativeRange(n))},e.createRangyRange=function(e){return e=c(e,t,"createRangyRange"),new s(e)},e.createIframeRange=function(n){return t.deprecationNotice("createIframeRange()","createRange(iframeEl)"),e.createRange(n)},e.createIframeRangyRange=function(n){return t.deprecationNotice("createIframeRangyRange()","createRangyRange(iframeEl)"),e.createRangyRange(n)},e.addShimListener(function(t){var n=t.document;"undefined"==typeof n.createRange&&(n.createRange=function(){return e.createRange(n)}),n=t=null})}),A.createCoreModule("WrappedSelection",["DomRange","WrappedRange"],function(e,t){function n(e){return"string"==typeof e?/^backward(s)?$/i.test(e):!!e}function o(e,n){if(e){if(A.isWindow(e))return e;if(e instanceof y)return e.win;var o=A.getContentDocument(e,t,n);return A.getWindow(o)}return window}function r(e){return o(e,"getWinSelection").getSelection()}function i(e){return o(e,"getDocSelection").document.selection}function a(e){var t=!1;return e.anchorNode&&(t=1==A.comparePoints(e.anchorNode,e.anchorOffset,e.focusNode,e.focusOffset)),t}function s(e,t,n){var o=n?"end":"start",r=n?"start":"end";e.anchorNode=t[o+"Container"],e.anchorOffset=t[o+"Offset"],e.focusNode=t[r+"Container"],e.focusOffset=t[r+"Offset"]}function l(e){var t=e.nativeSelection;e.anchorNode=t.anchorNode,e.anchorOffset=t.anchorOffset,e.focusNode=t.focusNode,e.focusOffset=t.focusOffset}function c(e){e.anchorNode=e.focusNode=null,e.anchorOffset=e.focusOffset=0,e.rangeCount=0,e.isCollapsed=!0,e._ranges.length=0}function d(t){var n;return t instanceof O?(n=e.createNativeRange(t.getDocument()),n.setEnd(t.endContainer,t.endOffset),n.setStart(t.startContainer,t.startOffset)):t instanceof L?n=t.nativeRange:B.implementsDomRange&&t instanceof A.getWindow(t.startContainer).Range&&(n=t),n}function u(e){if(!e.length||1!=e[0].nodeType)return!1;for(var t=1,n=e.length;n>t;++t)if(!A.isAncestorOf(e[0],e[t]))return!1;return!0}function h(e){var n=e.getNodes();if(!u(n))throw t.createError("getSingleElementFromRange: range "+e.inspect()+" did not consist of a single element");return n[0]}function f(e){return!!e&&"undefined"!=typeof e.text}function m(e,t){var n=new L(t);e._ranges=[n],s(e,n,!1),e.rangeCount=1,e.isCollapsed=n.collapsed}function p(t){if(t._ranges.length=0,"None"==t.docSelection.type)c(t);else{var n=t.docSelection.createRange();if(f(n))m(t,n);else{t.rangeCount=n.length;for(var o,r=P(n.item(0)),i=0;i<t.rangeCount;++i)o=e.createRange(r),o.selectNode(n.item(i)),t._ranges.push(o);t.isCollapsed=1==t.rangeCount&&t._ranges[0].collapsed,s(t,t._ranges[t.rangeCount-1],!1)}}}function g(e,n){for(var o=e.docSelection.createRange(),r=h(n),i=P(o.item(0)),a=H(i).createControlRange(),s=0,l=o.length;l>s;++s)a.add(o.item(s));try{a.add(r)}catch(c){throw t.createError("addRange(): Element within the specified Range could not be added to control selection (does it have layout?)")}a.select(),p(e)}function y(e,t,n){this.nativeSelection=e,this.docSelection=t,this._ranges=[],this.win=n,this.refresh()}function v(e){e.win=e.anchorNode=e.focusNode=e._ranges=null,e.rangeCount=e.anchorOffset=e.focusOffset=0,e.detached=!0}function w(e,t){for(var n,o,r=te.length;r--;)if(n=te[r],o=n.selection,"deleteAll"==t)v(o);else if(n.win==e)return"delete"==t?(te.splice(r,1),!0):o;return"deleteAll"==t&&(te.length=0),null}function b(e,n){for(var o,r=P(n[0].startContainer),i=H(r).createControlRange(),a=0,s=n.length;s>a;++a){o=h(n[a]);try{i.add(o)}catch(l){throw t.createError("setRanges(): Element within one of the specified Ranges could not be added to control selection (does it have layout?)")}}i.select(),p(e)}function C(e,t){if(e.win.document!=P(t))throw new k("WRONG_DOCUMENT_ERR")}function N(t){return function(n,o){var r;this.rangeCount?(r=this.getRangeAt(0),r["set"+(t?"Start":"End")](n,o)):(r=e.createRange(this.win.document),r.setStartAndEnd(n,o)),this.setSingleRange(r,this.isBackward())}}function x(e){var t=[],n=new D(e.anchorNode,e.anchorOffset),o=new D(e.focusNode,e.focusOffset),r="function"==typeof e.getName?e.getName():"Selection";if("undefined"!=typeof e.rangeCount)for(var i=0,a=e.rangeCount;a>i;++i)t[i]=O.inspect(e.getRangeAt(i));return"["+r+"(Ranges: "+t.join(", ")+")(anchor: "+n.inspect()+", focus: "+o.inspect()+"]"}e.config.checkSelectionRanges=!0;var E,S,T="boolean",R="number",A=e.dom,_=e.util,I=_.isHostMethod,O=e.DomRange,L=e.WrappedRange,k=e.DOMException,D=A.DomPosition,B=e.features,M="Control",P=A.getDocument,H=A.getBody,z=O.rangesEqual,q=I(window,"getSelection"),V=_.isHostObject(document,"selection");B.implementsWinGetSelection=q,B.implementsDocSelection=V;var W=V&&(!q||e.config.preferTextRange);W?(E=i,e.isSelectionValid=function(e){var t=o(e,"isSelectionValid").document,n=t.selection;return"None"!=n.type||P(n.createRange().parentElement())==t}):q?(E=r,e.isSelectionValid=function(){return!0}):t.fail("Neither document.selection or window.getSelection() detected."),e.getNativeSelection=E;var U=E(),j=e.createNativeRange(document),F=H(document),K=_.areHostProperties(U,["anchorNode","focusNode","anchorOffset","focusOffset"]);B.selectionHasAnchorAndFocus=K;var $=I(U,"extend");B.selectionHasExtend=$;var G=typeof U.rangeCount==R;B.selectionHasRangeCount=G;var X=!1,Y=!0,Q=$?function(t,n){var o=O.getRangeDocument(n),r=e.createRange(o);r.collapseToPoint(n.endContainer,n.endOffset),t.addRange(d(r)),t.extend(n.startContainer,n.startOffset)}:null;_.areHostMethods(U,["addRange","getRangeAt","removeAllRanges"])&&typeof U.rangeCount==R&&B.implementsDomRange&&!function(){var t=window.getSelection();if(t){for(var n=t.rangeCount,o=n>1,r=[],i=a(t),s=0;n>s;++s)r[s]=t.getRangeAt(s);var l=H(document),c=l.appendChild(document.createElement("div"));c.contentEditable="false";var d=c.appendChild(document.createTextNode("   ")),u=document.createRange();if(u.setStart(d,1),u.collapse(!0),t.addRange(u),Y=1==t.rangeCount,t.removeAllRanges(),!o){var h=window.navigator.appVersion.match(/Chrome\/(.*?) /);if(h&&parseInt(h[1])>=36)X=!1;else{var f=u.cloneRange();u.setStart(d,0),f.setEnd(d,3),f.setStart(d,2),t.addRange(u),t.addRange(f),X=2==t.rangeCount}}for(l.removeChild(c),t.removeAllRanges(),s=0;n>s;++s)0==s&&i?Q?Q(t,r[s]):(e.warn("Rangy initialization: original selection was backwards but selection has been restored forwards because the browser does not support Selection.extend"),t.addRange(r[s])):t.addRange(r[s])}}(),B.selectionSupportsMultipleRanges=X,B.collapsedNonEditableSelectionsSupported=Y;var Z,J=!1;F&&I(F,"createControlRange")&&(Z=F.createControlRange(),_.areHostProperties(Z,["item","add"])&&(J=!0)),B.implementsControlRange=J,S=K?function(e){return e.anchorNode===e.focusNode&&e.anchorOffset===e.focusOffset}:function(e){return e.rangeCount?e.getRangeAt(e.rangeCount-1).collapsed:!1};var ee;I(U,"getRangeAt")?ee=function(e,t){try{return e.getRangeAt(t)}catch(n){return null}}:K&&(ee=function(t){var n=P(t.anchorNode),o=e.createRange(n);return o.setStartAndEnd(t.anchorNode,t.anchorOffset,t.focusNode,t.focusOffset),o.collapsed!==this.isCollapsed&&o.setStartAndEnd(t.focusNode,t.focusOffset,t.anchorNode,t.anchorOffset),o}),y.prototype=e.selectionPrototype;var te=[],ne=function(e){if(e&&e instanceof y)return e.refresh(),e;e=o(e,"getNativeSelection");var t=w(e),n=E(e),r=V?i(e):null;return t?(t.nativeSelection=n,t.docSelection=r,t.refresh()):(t=new y(n,r,e),te.push({win:e,selection:t})),t};e.getSelection=ne,e.getIframeSelection=function(n){return t.deprecationNotice("getIframeSelection()","getSelection(iframeEl)"),e.getSelection(A.getIframeWindow(n))};var oe=y.prototype;if(!W&&K&&_.areHostMethods(U,["removeAllRanges","addRange"])){oe.removeAllRanges=function(){this.nativeSelection.removeAllRanges(),c(this)};var re=function(e,t){Q(e.nativeSelection,t),e.refresh()};G?oe.addRange=function(t,o){if(J&&V&&this.docSelection.type==M)g(this,t);else if(n(o)&&$)re(this,t);else{var r;if(X?r=this.rangeCount:(this.removeAllRanges(),r=0),this.nativeSelection.addRange(d(t).cloneRange()),this.rangeCount=this.nativeSelection.rangeCount,this.rangeCount==r+1){if(e.config.checkSelectionRanges){var i=ee(this.nativeSelection,this.rangeCount-1);i&&!z(i,t)&&(t=new L(i))}this._ranges[this.rangeCount-1]=t,s(this,t,se(this.nativeSelection)),this.isCollapsed=S(this)}else this.refresh()}}:oe.addRange=function(e,t){n(t)&&$?re(this,e):(this.nativeSelection.addRange(d(e)),this.refresh())},oe.setRanges=function(e){if(J&&V&&e.length>1)b(this,e);else{this.removeAllRanges();for(var t=0,n=e.length;n>t;++t)this.addRange(e[t])}}}else{if(!(I(U,"empty")&&I(j,"select")&&J&&W))return t.fail("No means of selecting a Range or TextRange was found"),!1;oe.removeAllRanges=function(){try{if(this.docSelection.empty(),"None"!=this.docSelection.type){var e;if(this.anchorNode)e=P(this.anchorNode);else if(this.docSelection.type==M){var t=this.docSelection.createRange();t.length&&(e=P(t.item(0)))}if(e){var n=H(e).createTextRange();n.select(),this.docSelection.empty()}}}catch(o){}c(this)},oe.addRange=function(t){this.docSelection.type==M?g(this,t):(e.WrappedTextRange.rangeToTextRange(t).select(),this._ranges[0]=t,this.rangeCount=1,this.isCollapsed=this._ranges[0].collapsed,s(this,t,!1))},oe.setRanges=function(e){this.removeAllRanges();var t=e.length;t>1?b(this,e):t&&this.addRange(e[0])}}oe.getRangeAt=function(e){if(0>e||e>=this.rangeCount)throw new k("INDEX_SIZE_ERR");return this._ranges[e].cloneRange()};var ie;if(W)ie=function(t){var n;e.isSelectionValid(t.win)?n=t.docSelection.createRange():(n=H(t.win.document).createTextRange(),n.collapse(!0)),t.docSelection.type==M?p(t):f(n)?m(t,n):c(t)};else if(I(U,"getRangeAt")&&typeof U.rangeCount==R)ie=function(t){if(J&&V&&t.docSelection.type==M)p(t);else if(t._ranges.length=t.rangeCount=t.nativeSelection.rangeCount,t.rangeCount){for(var n=0,o=t.rangeCount;o>n;++n)t._ranges[n]=new e.WrappedRange(t.nativeSelection.getRangeAt(n));s(t,t._ranges[t.rangeCount-1],se(t.nativeSelection)),t.isCollapsed=S(t)}else c(t)};else{if(!K||typeof U.isCollapsed!=T||typeof j.collapsed!=T||!B.implementsDomRange)return t.fail("No means of obtaining a Range or TextRange from the user's selection was found"),!1;ie=function(e){var t,n=e.nativeSelection;n.anchorNode?(t=ee(n,0),e._ranges=[t],e.rangeCount=1,l(e),e.isCollapsed=S(e)):c(e)}}oe.refresh=function(e){var t=e?this._ranges.slice(0):null,n=this.anchorNode,o=this.anchorOffset;if(ie(this),e){var r=t.length;if(r!=this._ranges.length)return!0;if(this.anchorNode!=n||this.anchorOffset!=o)return!0;for(;r--;)if(!z(t[r],this._ranges[r]))return!0;return!1}};var ae=function(e,t){var n=e.getAllRanges();e.removeAllRanges();for(var o=0,r=n.length;r>o;++o)z(t,n[o])||e.addRange(n[o]);e.rangeCount||c(e)};J&&V?oe.removeRange=function(e){if(this.docSelection.type==M){for(var t,n=this.docSelection.createRange(),o=h(e),r=P(n.item(0)),i=H(r).createControlRange(),a=!1,s=0,l=n.length;l>s;++s)t=n.item(s),t!==o||a?i.add(n.item(s)):a=!0;i.select(),p(this)}else ae(this,e)}:oe.removeRange=function(e){ae(this,e)};var se;!W&&K&&B.implementsDomRange?(se=a,oe.isBackward=function(){return se(this)}):se=oe.isBackward=function(){return!1},oe.isBackwards=oe.isBackward,oe.toString=function(){for(var e=[],t=0,n=this.rangeCount;n>t;++t)e[t]=""+this._ranges[t];return e.join("")},oe.collapse=function(t,n){C(this,t);var o=e.createRange(t);o.collapseToPoint(t,n),this.setSingleRange(o),this.isCollapsed=!0},oe.collapseToStart=function(){if(!this.rangeCount)throw new k("INVALID_STATE_ERR");var e=this._ranges[0];this.collapse(e.startContainer,e.startOffset)},oe.collapseToEnd=function(){if(!this.rangeCount)throw new k("INVALID_STATE_ERR");var e=this._ranges[this.rangeCount-1];this.collapse(e.endContainer,e.endOffset)},oe.selectAllChildren=function(t){C(this,t);var n=e.createRange(t);n.selectNodeContents(t),this.setSingleRange(n)},oe.deleteFromDocument=function(){if(J&&V&&this.docSelection.type==M){for(var e,t=this.docSelection.createRange();t.length;)e=t.item(0),t.remove(e),e.parentNode.removeChild(e);this.refresh()}else if(this.rangeCount){var n=this.getAllRanges();if(n.length){this.removeAllRanges();for(var o=0,r=n.length;r>o;++o)n[o].deleteContents();this.addRange(n[r-1])}}},oe.eachRange=function(e,t){for(var n=0,o=this._ranges.length;o>n;++n)if(e(this.getRangeAt(n)))return t},oe.getAllRanges=function(){var e=[];return this.eachRange(function(t){e.push(t)}),e},oe.setSingleRange=function(e,t){this.removeAllRanges(),this.addRange(e,t)},oe.callMethodOnEachRange=function(e,t){var n=[];return this.eachRange(function(o){n.push(o[e].apply(o,t))}),n},oe.setStart=N(!0),oe.setEnd=N(!1),e.rangePrototype.select=function(e){ne(this.getDocument()).setSingleRange(this,e)},oe.changeEachRange=function(e){var t=[],n=this.isBackward();this.eachRange(function(n){e(n),t.push(n)}),this.removeAllRanges(),n&&1==t.length?this.addRange(t[0],"backward"):this.setRanges(t)},oe.containsNode=function(e,t){return this.eachRange(function(n){return n.containsNode(e,t)},!0)||!1},oe.getBookmark=function(e){return{backward:this.isBackward(),rangeBookmarks:this.callMethodOnEachRange("getBookmark",[e])}},oe.moveToBookmark=function(t){for(var n,o,r=[],i=0;n=t.rangeBookmarks[i++];)o=e.createRange(this.win),o.moveToBookmark(n),r.push(o);t.backward?this.setSingleRange(r[0],"backward"):this.setRanges(r)},oe.toHtml=function(){var e=[];return this.eachRange(function(t){e.push(O.toHtml(t))}),e.join("")},B.implementsTextRange&&(oe.getNativeTextRange=function(){var n;if(n=this.docSelection){var o=n.createRange();if(f(o))return o;throw t.createError("getNativeTextRange: selection is a control selection")}if(this.rangeCount>0)return e.WrappedTextRange.rangeToTextRange(this.getRangeAt(0));throw t.createError("getNativeTextRange: selection contains no range")}),oe.getName=function(){return"WrappedSelection"},oe.inspect=function(){return x(this)},oe.detach=function(){w(this.win,"delete"),v(this)},y.detachAll=function(){w(null,"deleteAll")},y.inspect=x,y.isDirectionBackward=n,e.Selection=y,e.selectionPrototype=oe,e.addShimListener(function(e){"undefined"==typeof e.getSelection&&(e.getSelection=function(){return ne(e)}),e=null})}),A)},this),function(e,t){"function"==typeof define&&define.amd?define(["rangy"],e):e(t.rangy)}(function(e){e.createModule("SaveRestore",["WrappedRange"],function(e,t){function n(e,t){return(t||document).getElementById(e)}function o(e,t){var n,o="selectionBoundary_"+ +new Date+"_"+(""+Math.random()).slice(2),r=m.getDocument(e.startContainer),i=e.cloneRange();return i.collapse(t),n=r.createElement("span"),n.id=o,n.style.lineHeight="0",n.style.display="none",n.className="rangySelectionBoundary",n.appendChild(r.createTextNode(p)),i.insertNode(n),n}function r(e,o,r,i){var a=n(r,e);a?(o[i?"setStartBefore":"setEndBefore"](a),a.parentNode.removeChild(a)):t.warn("Marker element has been removed. Cannot restore selection.")}function i(e,t){return t.compareBoundaryPoints(e.START_TO_START,e)}function a(t,n){var r,i,a=e.DomRange.getRangeDocument(t),s=t.toString();return t.collapsed?(i=o(t,!1),{document:a,markerId:i.id,collapsed:!0}):(i=o(t,!1),r=o(t,!0),{document:a,startMarkerId:r.id,endMarkerId:i.id,collapsed:!1,backward:n,toString:function(){return"original text: '"+s+"', new text: '"+t.toString()+"'"}})}function s(o,i){var a=o.document;"undefined"==typeof i&&(i=!0);var s=e.createRange(a);if(o.collapsed){var l=n(o.markerId,a);if(l){l.style.display="inline";var c=l.previousSibling;c&&3==c.nodeType?(l.parentNode.removeChild(l),s.collapseToPoint(c,c.length)):(s.collapseBefore(l),l.parentNode.removeChild(l))}else t.warn("Marker element has been removed. Cannot restore selection.")}else r(a,s,o.startMarkerId,!0),r(a,s,o.endMarkerId,!1);return i&&s.normalizeBoundaries(),s}function l(t,o){var r,s,l=[];t=t.slice(0),t.sort(i);for(var c=0,d=t.length;d>c;++c)l[c]=a(t[c],o);for(c=d-1;c>=0;--c)r=t[c],s=e.DomRange.getRangeDocument(r),r.collapsed?r.collapseAfter(n(l[c].markerId,s)):(r.setEndBefore(n(l[c].endMarkerId,s)),r.setStartAfter(n(l[c].startMarkerId,s)));return l}function c(n){if(!e.isSelectionValid(n))return t.warn("Cannot save selection. This usually happens when the selection is collapsed and the selection document has lost focus."),null;var o=e.getSelection(n),r=o.getAllRanges(),i=1==r.length&&o.isBackward(),a=l(r,i);return i?o.setSingleRange(r[0],"backward"):o.setRanges(r),{win:n,rangeInfos:a,restored:!1}}function d(e){for(var t=[],n=e.length,o=n-1;o>=0;o--)t[o]=s(e[o],!0);return t}function u(t,n){if(!t.restored){var o=t.rangeInfos,r=e.getSelection(t.win),i=d(o),a=o.length;1==a&&n&&e.features.selectionHasExtend&&o[0].backward?(r.removeAllRanges(),r.addRange(i[0],!0)):r.setRanges(i),t.restored=!0}}function h(e,t){var o=n(t,e);o&&o.parentNode.removeChild(o)}function f(e){for(var t,n=e.rangeInfos,o=0,r=n.length;r>o;++o)t=n[o],t.collapsed?h(e.doc,t.markerId):(h(e.doc,t.startMarkerId),h(e.doc,t.endMarkerId))}var m=e.dom,p="\ufeff";e.util.extend(e,{saveRange:a,restoreRange:s,saveRanges:l,restoreRanges:d,saveSelection:c,restoreSelection:u,removeMarkerElement:h,removeMarkers:f})})},this);var Base=function(){};Base.extend=function(e,t){var n=Base.prototype.extend;Base._prototyping=!0;var o=new this;n.call(o,e),o.base=function(){},delete Base._prototyping;var r=o.constructor,i=o.constructor=function(){if(!Base._prototyping)if(this._constructing||this.constructor==i)this._constructing=!0,r.apply(this,arguments),delete this._constructing;else if(null!=arguments[0])return(arguments[0].extend||n).call(arguments[0],o)};return i.ancestor=this,i.extend=this.extend,i.forEach=this.forEach,i.implement=this.implement,i.prototype=o,i.toString=this.toString,i.valueOf=function(e){return"object"==e?i:r.valueOf()},n.call(i,t),"function"==typeof i.init&&i.init(),i},Base.prototype={extend:function(e,t){if(arguments.length>1){var n=this[e];if(n&&"function"==typeof t&&(!n.valueOf||n.valueOf()!=t.valueOf())&&/\bbase\b/.test(t)){var o=t.valueOf();t=function(){var e=this.base||Base.prototype.base;this.base=n;var t=o.apply(this,arguments);return this.base=e,t},t.valueOf=function(e){return"object"==e?t:o},t.toString=Base.toString}this[e]=t}else if(e){var r=Base.prototype.extend;Base._prototyping||"function"==typeof this||(r=this.extend||r);for(var i={toSource:null},a=["constructor","toString","valueOf"],s=Base._prototyping?0:1;l=a[s++];)e[l]!=i[l]&&r.call(this,l,e[l]);for(var l in e)i[l]||r.call(this,l,e[l])}return this}},Base=Base.extend({constructor:function(){this.extend(arguments[0])}},{ancestor:Object,version:"1.1",forEach:function(e,t,n){for(var o in e)void 0===this.prototype[o]&&t.call(n,e[o],o,e)},implement:function(){for(var e=0;e<arguments.length;e++)"function"==typeof arguments[e]?arguments[e](this.prototype):this.prototype.extend(arguments[e]);return this},toString:function(){return String(this.valueOf())}}),wysihtml5.browser=function(){function e(e){return+(/ipad|iphone|ipod/.test(e)&&e.match(/ os (\d+).+? like mac os x/)||[void 0,0])[1]}function t(e){return+(e.match(/android (\d+)/)||[void 0,0])[1]}function n(e,t){var n,o=-1;return"Microsoft Internet Explorer"==navigator.appName?n=new RegExp("MSIE ([0-9]{1,}[.0-9]{0,})"):"Netscape"==navigator.appName&&(n=new RegExp("Trident/.*rv:([0-9]{1,}[.0-9]{0,})")),n&&null!=n.exec(navigator.userAgent)&&(o=parseFloat(RegExp.$1)),-1===o?!1:e?t?"<"===t?o>e:">"===t?e>o:"<="===t?o>=e:">="===t?e>=o:void 0:e===o:!0}var o=navigator.userAgent,r=document.createElement("div"),i=-1!==o.indexOf("Gecko")&&-1===o.indexOf("KHTML"),a=-1!==o.indexOf("AppleWebKit/"),s=-1!==o.indexOf("Chrome/"),l=-1!==o.indexOf("Opera/");return{USER_AGENT:o,supported:function(){var n=this.USER_AGENT.toLowerCase(),o="contentEditable"in r,i=document.execCommand&&document.queryCommandSupported&&document.queryCommandState,a=document.querySelector&&document.querySelectorAll,s=this.isIos()&&e(n)<5||this.isAndroid()&&t(n)<4||-1!==n.indexOf("opera mobi")||-1!==n.indexOf("hpwos/");return o&&i&&a&&!s},isTouchDevice:function(){return this.supportsEvent("touchmove")},isIos:function(){return/ipad|iphone|ipod/i.test(this.USER_AGENT)},isAndroid:function(){return-1!==this.USER_AGENT.indexOf("Android")},supportsSandboxedIframes:function(){return n()},throwsMixedContentWarningWhenIframeSrcIsEmpty:function(){return!("querySelector"in document)},displaysCaretInEmptyContentEditableCorrectly:function(){return n()},hasCurrentStyleProperty:function(){return"currentStyle"in r},hasHistoryIssue:function(){return i&&"Mac"===navigator.platform.substr(0,3)},insertsLineBreaksOnReturn:function(){return i},supportsPlaceholderAttributeOn:function(e){return"placeholder"in e},supportsEvent:function(e){return"on"+e in r||function(){return r.setAttribute("on"+e,"return;"),"function"==typeof r["on"+e]}()},supportsEventsInIframeCorrectly:function(){return!l},supportsHTML5Tags:function(e){var t=e.createElement("div"),n="<article>foo</article>";return t.innerHTML=n,t.innerHTML.toLowerCase()===n},supportsCommand:function(){var e={formatBlock:n(10,"<="),insertUnorderedList:n(),insertOrderedList:n()},t={insertHTML:i};return function(n,o){var r=e[o];if(!r){try{return n.queryCommandSupported(o)}catch(i){}try{return n.queryCommandEnabled(o)}catch(a){return!!t[o]}}return!1}}(),doesAutoLinkingInContentEditable:function(){return n()},canDisableAutoLinking:function(){return this.supportsCommand(document,"AutoUrlDetect")},clearsContentEditableCorrectly:function(){return i||l||a},supportsGetAttributeCorrectly:function(){var e=document.createElement("td");return"1"!=e.getAttribute("rowspan")},canSelectImagesInContentEditable:function(){return i||n()||l},autoScrollsToCaret:function(){return!a},autoClosesUnclosedTags:function(){var e,t,n=r.cloneNode(!1);return n.innerHTML="<p><div></div>",t=n.innerHTML.toLowerCase(),e="<p></p><div></div>"===t||"<p><div></div></p>"===t,this.autoClosesUnclosedTags=function(){return e},e},supportsNativeGetElementsByClassName:function(){return-1!==String(document.getElementsByClassName).indexOf("[native code]")},supportsSelectionModify:function(){return"getSelection"in window&&"modify"in window.getSelection()},needsSpaceAfterLineBreak:function(){return l},supportsSpeechApiOn:function(e){var t=o.match(/Chrome\/(\d+)/)||[void 0,0];return t[1]>=11&&("onwebkitspeechchange"in e||"speech"in e)},crashesWhenDefineProperty:function(e){return n(9)&&("XMLHttpRequest"===e||"XDomainRequest"===e)},doesAsyncFocus:function(){return n()},hasProblemsSettingCaretAfterImg:function(){return n()},hasUndoInContextMenu:function(){return i||s||l},hasInsertNodeIssue:function(){return l},hasIframeFocusIssue:function(){return n()},createsNestedInvalidMarkupAfterPaste:function(){return a},supportsMutationEvents:function(){return"MutationEvent"in window}}}(),wysihtml5.lang.array=function(e){return{contains:function(t){if(Array.isArray(t)){for(var n=t.length;n--;)if(-1!==wysihtml5.lang.array(e).indexOf(t[n]))return!0;return!1}return-1!==wysihtml5.lang.array(e).indexOf(t)},indexOf:function(t){if(e.indexOf)return e.indexOf(t);for(var n=0,o=e.length;o>n;n++)if(e[n]===t)return n;return-1},without:function(t){t=wysihtml5.lang.array(t);for(var n=[],o=0,r=e.length;r>o;o++)t.contains(e[o])||n.push(e[o]);return n},get:function(){for(var t=0,n=e.length,o=[];n>t;t++)o.push(e[t]);return o},map:function(t,n){if(Array.prototype.map)return e.map(t,n);for(var o=e.length>>>0,r=new Array(o),i=0;o>i;i++)r[i]=t.call(n,e[i],i,e);return r},unique:function(){for(var t=[],n=e.length,o=0;n>o;)wysihtml5.lang.array(t).contains(e[o])||t.push(e[o]),o++;return t}}},wysihtml5.lang.Dispatcher=Base.extend({on:function(e,t){return this.events=this.events||{},this.events[e]=this.events[e]||[],this.events[e].push(t),this},off:function(e,t){this.events=this.events||{};var n,o,r=0;if(e){for(n=this.events[e]||[],o=[];r<n.length;r++)n[r]!==t&&t&&o.push(n[r]);this.events[e]=o}else this.events={};return this},fire:function(e,t){this.events=this.events||{};for(var n=this.events[e]||[],o=0;o<n.length;o++)n[o].call(this,t);return this},observe:function(){return this.on.apply(this,arguments)},stopObserving:function(){return this.off.apply(this,arguments)}}),wysihtml5.lang.object=function(e){return{merge:function(t){for(var n in t)e[n]=t[n];return this},get:function(){return e},clone:function(){var t,n={};for(t in e)n[t]=e[t];return n},isArray:function(){return"[object Array]"===Object.prototype.toString.call(e)}}},function(){var e=/^\s+/,t=/\s+$/,n=/[&<>"]/g,o={"&":"&amp;","<":"&lt;",">":"&gt;",'"':"&quot;"};wysihtml5.lang.string=function(r){return r=String(r),{trim:function(){return r.replace(e,"").replace(t,"")},interpolate:function(e){for(var t in e)r=this.replace("#{"+t+"}").by(e[t]);return r},replace:function(e){return{by:function(t){return r.split(e).join(t)}}},escapeHTML:function(){return r.replace(n,function(e){return o[e]})}}}}(),function(e){function t(e,t){return i(e,t)?e:(e===e.ownerDocument.documentElement&&(e=e.ownerDocument.body),a(e,t))}function n(e){return e.replace(l,function(e,t){var n=(t.match(c)||[])[1]||"",o=u[n];t=t.replace(c,""),t.split(o).length>t.split(n).length&&(t+=n,n="");var r=t,i=t;return t.length>d&&(i=i.substr(0,d)+"..."),"www."===r.substr(0,4)&&(r="http://"+r),'<a href="'+r+'">'+i+"</a>"+n})}function o(e){var t=e._wysihtml5_tempElement;return t||(t=e._wysihtml5_tempElement=e.createElement("div")),t}function r(t){var r=t.parentNode,i=e.lang.string(t.data).escapeHTML(),a=o(r.ownerDocument);for(a.innerHTML="<span></span>"+n(i),a.removeChild(a.firstChild);a.firstChild;)r.insertBefore(a.firstChild,t);r.removeChild(t)}function i(t,n){for(var o;t.parentNode;){if(t=t.parentNode,o=t.nodeName,t.className&&e.lang.array(t.className.split(" ")).contains(n))return!0;if(s.contains(o))return!0;if("body"===o)return!1}return!1}function a(t,n){if(!(s.contains(t.nodeName)||t.className&&e.lang.array(t.className.split(" ")).contains(n))){if(t.nodeType===e.TEXT_NODE&&t.data.match(l))return void r(t);for(var o=e.lang.array(t.childNodes).get(),i=o.length,c=0;i>c;c++)a(o[c],n);return t}}var s=e.lang.array(["CODE","PRE","A","SCRIPT","HEAD","TITLE","STYLE"]),l=/((https?:\/\/|www\.)[^\s<]{3,})/gi,c=/([^\w\/\-](,?))$/i,d=100,u={")":"(","]":"[","}":"{"};e.dom.autoLink=t,e.dom.autoLink.URL_REG_EXP=l}(wysihtml5),function(e){var t=e.dom;t.addClass=function(e,n){var o=e.classList;return o?o.add(n):void(t.hasClass(e,n)||(e.className+=" "+n))},t.removeClass=function(e,t){var n=e.classList;return n?n.remove(t):void(e.className=e.className.replace(new RegExp("(^|\\s+)"+t+"(\\s+|$)")," "))},t.hasClass=function(e,t){var n=e.classList;if(n)return n.contains(t);var o=e.className;return o.length>0&&(o==t||new RegExp("(^|\\s)"+t+"(\\s|$)").test(o))}}(wysihtml5),wysihtml5.dom.contains=function(){var e=document.documentElement;return e.contains?function(e,t){return t.nodeType!==wysihtml5.ELEMENT_NODE&&(t=t.parentNode),e!==t&&e.contains(t)}:e.compareDocumentPosition?function(e,t){return!!(16&e.compareDocumentPosition(t))}:void 0}(),wysihtml5.dom.convertToList=function(){function e(e,t){var n=e.createElement("li");return t.appendChild(n),n}function t(e,t){return e.createElement(t)}function n(n,o,r){if("UL"===n.nodeName||"OL"===n.nodeName||"MENU"===n.nodeName)return n;var i,a,s,l,c,d,u,h,f,m=n.ownerDocument,p=t(m,o),g=n.querySelectorAll("br"),y=g.length;for(f=0;y>f;f++)for(l=g[f];(c=l.parentNode)&&c!==n&&c.lastChild===l;){if("block"===wysihtml5.dom.getStyle("display").from(c)){c.removeChild(l);break}wysihtml5.dom.insert(l).after(l.parentNode)}for(i=wysihtml5.lang.array(n.childNodes).get(),a=i.length,f=0;a>f;f++)h=h||e(m,p),s=i[f],d="block"===wysihtml5.dom.getStyle("display").from(s),u="BR"===s.nodeName,!d||r&&wysihtml5.dom.hasClass(s,r)?u?h=h.firstChild?null:h:h.appendChild(s):(h=h.firstChild?e(m,p):h,h.appendChild(s),h=null);return 0===i.length&&e(m,p),n.parentNode.replaceChild(p,n),p}return n}(),wysihtml5.dom.copyAttributes=function(e){return{from:function(t){return{to:function(n){for(var o,r=0,i=e.length;i>r;r++)o=e[r],"undefined"!=typeof t[o]&&""!==t[o]&&(n[o]=t[o]);return{andTo:arguments.callee}}}}}},function(e){var t=["-webkit-box-sizing","-moz-box-sizing","-ms-box-sizing","box-sizing"],n=function(t){return o(t)?parseInt(e.getStyle("width").from(t),10)<t.offsetWidth:!1},o=function(n){for(var o=0,r=t.length;r>o;o++)if("border-box"===e.getStyle(t[o]).from(n))return t[o]};e.copyStyles=function(o){return{from:function(r){n(r)&&(o=wysihtml5.lang.array(o).without(t));for(var i,a="",s=o.length,l=0;s>l;l++)i=o[l],a+=i+":"+e.getStyle(i).from(r)+";";return{to:function(t){return e.setStyles(a).on(t),{andTo:arguments.callee}}}}}}}(wysihtml5.dom),function(e){e.dom.delegate=function(t,n,o,r){return e.dom.observe(t,o,function(o){for(var i=o.target,a=e.lang.array(t.querySelectorAll(n));i&&i!==t;){if(a.contains(i)){r.call(i,o);break}i=i.parentNode}})}}(wysihtml5),function(e){e.dom.domNode=function(t){var n=[e.ELEMENT_NODE,e.TEXT_NODE],o=function(t){return t.nodeType===e.TEXT_NODE&&/^\s*$/g.test(t.data)};return{prev:function(r){var i=t.previousSibling,a=r&&r.nodeTypes?r.nodeTypes:n;return i?!e.lang.array(a).contains(i.nodeType)||r&&r.ignoreBlankTexts&&o(i)?e.dom.domNode(i).prev(r):i:null},next:function(r){var i=t.nextSibling,a=r&&r.nodeTypes?r.nodeTypes:n;return i?!e.lang.array(a).contains(i.nodeType)||r&&r.ignoreBlankTexts&&o(i)?e.dom.domNode(i).next(r):i:null}}}}(wysihtml5),wysihtml5.dom.getAsDom=function(){var e=function(e,t){var n=t.createElement("div");n.style.display="none",t.body.appendChild(n);try{n.innerHTML=e}catch(o){}return t.body.removeChild(n),n},t=function(e){if(!e._wysihtml5_supportsHTML5Tags){for(var t=0,o=n.length;o>t;t++)e.createElement(n[t]);e._wysihtml5_supportsHTML5Tags=!0}},n=["abbr","article","aside","audio","bdi","canvas","command","datalist","details","figcaption","figure","footer","header","hgroup","keygen","mark","meter","nav","output","progress","rp","rt","ruby","svg","section","source","summary","time","track","video","wbr"];return function(n,o){o=o||document;var r;return"object"==typeof n&&n.nodeType?(r=o.createElement("div"),r.appendChild(n)):wysihtml5.browser.supportsHTML5Tags(o)?(r=o.createElement("div"),r.innerHTML=n):(t(o),r=e(n,o)),r}}(),wysihtml5.dom.getParentElement=function(){function e(e,t){return t&&t.length?"string"==typeof t?e===t:wysihtml5.lang.array(t).contains(e):!0}function t(e){return e.nodeType===wysihtml5.ELEMENT_NODE}function n(e,t,n){var o=(e.className||"").match(n)||[];return t?o[o.length-1]===t:!!o.length}function o(e,t,n){var o=(e.getAttribute("style")||"").match(n)||[];return t?o[o.length-1]===t:!!o.length}return function(r,i,a,s){var l=i.cssStyle||i.styleRegExp,c=i.className||i.classRegExp;for(a=a||50;a--&&r&&"BODY"!==r.nodeName&&(!s||r!==s);){if(t(r)&&e(r.nodeName,i.nodeName)&&(!l||o(r,i.cssStyle,i.styleRegExp))&&(!c||n(r,i.className,i.classRegExp)))return r;r=r.parentNode}return null}}(),wysihtml5.dom.getStyle=function(){function e(e){return e.replace(n,function(e){return e.charAt(1).toUpperCase()})}var t={"float":"styleFloat"in document.createElement("div").style?"styleFloat":"cssFloat"},n=/\-[a-z]/g;return function(n){return{from:function(o){if(o.nodeType===wysihtml5.ELEMENT_NODE){var r=o.ownerDocument,i=t[n]||e(n),a=o.style,s=o.currentStyle,l=a[i];
if(l)return l;if(s)try{return s[i]}catch(c){}var d,u,h=r.defaultView||r.parentWindow,f=("height"===n||"width"===n)&&"TEXTAREA"===o.nodeName;return h.getComputedStyle?(f&&(d=a.overflow,a.overflow="hidden"),u=h.getComputedStyle(o,null).getPropertyValue(n),f&&(a.overflow=d||""),u):void 0}}}}}(),wysihtml5.dom.getTextNodes=function(e,t){var n=[];for(e=e.firstChild;e;e=e.nextSibling)3==e.nodeType?t&&/^\s*$/.test(e.innerText||e.textContent)||n.push(e):n=n.concat(wysihtml5.dom.getTextNodes(e,t));return n},wysihtml5.dom.hasElementWithTagName=function(){function e(e){return e._wysihtml5_identifier||(e._wysihtml5_identifier=n++)}var t={},n=1;return function(n,o){var r=e(n)+":"+o,i=t[r];return i||(i=t[r]=n.getElementsByTagName(o)),i.length>0}}(),function(e){function t(e){return e._wysihtml5_identifier||(e._wysihtml5_identifier=o++)}var n={},o=1;e.dom.hasElementWithClassName=function(o,r){if(!e.browser.supportsNativeGetElementsByClassName())return!!o.querySelector("."+r);var i=t(o)+":"+r,a=n[i];return a||(a=n[i]=o.getElementsByClassName(r)),a.length>0}}(wysihtml5),wysihtml5.dom.insert=function(e){return{after:function(t){t.parentNode.insertBefore(e,t.nextSibling)},before:function(t){t.parentNode.insertBefore(e,t)},into:function(t){t.appendChild(e)}}},wysihtml5.dom.insertCSS=function(e){return e=e.join("\n"),{into:function(t){var n=t.createElement("style");n.type="text/css",n.styleSheet?n.styleSheet.cssText=e:n.appendChild(t.createTextNode(e));var o=t.querySelector("head link");if(o)return void o.parentNode.insertBefore(n,o);var r=t.querySelector("head");r&&r.appendChild(n)}}},function(e){e.dom.lineBreaks=function(t){function n(e){return"BR"===e.nodeName}function o(t){return n(t)?!0:"block"===e.dom.getStyle("display").from(t)?!0:!1}return{add:function(n){var r=t.ownerDocument,i=e.dom.domNode(t).next({ignoreBlankTexts:!0}),a=e.dom.domNode(t).prev({ignoreBlankTexts:!0});i&&!o(i)&&e.dom.insert(r.createElement("br")).after(t),a&&!o(a)&&e.dom.insert(r.createElement("br")).before(t)},remove:function(o){var r=e.dom.domNode(t).next({ignoreBlankTexts:!0}),i=e.dom.domNode(t).prev({ignoreBlankTexts:!0});r&&n(r)&&r.parentNode.removeChild(r),i&&n(i)&&i.parentNode.removeChild(i)}}}}(wysihtml5),wysihtml5.dom.observe=function(e,t,n){t="string"==typeof t?[t]:t;for(var o,r,i=0,a=t.length;a>i;i++)r=t[i],e.addEventListener?e.addEventListener(r,n,!1):(o=function(t){"target"in t||(t.target=t.srcElement),t.preventDefault=t.preventDefault||function(){this.returnValue=!1},t.stopPropagation=t.stopPropagation||function(){this.cancelBubble=!0},n.call(e,t)},e.attachEvent("on"+r,o));return{stop:function(){for(var r,i=0,a=t.length;a>i;i++)r=t[i],e.removeEventListener?e.removeEventListener(r,n,!1):e.detachEvent("on"+r,o)}}},wysihtml5.dom.parse=function(){function e(e,n){wysihtml5.lang.object(p).merge(m).merge(n.rules).get();var o,r,i,a=n.context||e.ownerDocument||document,s=a.createDocumentFragment(),l="string"==typeof e,c=!1;for(n.clearInternals===!0&&(c=!0),n.uneditableClass&&(g=n.uneditableClass),o=l?wysihtml5.dom.getAsDom(e,a):e;o.firstChild;)i=o.firstChild,r=t(i,n.cleanUp,c),r&&s.appendChild(r),i!==r&&o.removeChild(i);return o.innerHTML="",o.appendChild(s),l?wysihtml5.quirks.getCorrectInnerHTML(o):o}function t(e,n,o){var r,i,a,s=e.nodeType,l=e.childNodes,c=l.length,d=u[s],f=0;if(g&&1===s&&wysihtml5.dom.hasClass(e,g))return e;if(i=d&&d(e,o),!i){if(i===!1){for(r=e.ownerDocument.createDocumentFragment(),f=c;f--;)l[f]&&(a=t(l[f],n,o),a&&(l[f]===a&&f--,r.insertBefore(a,r.firstChild)));return wysihtml5.lang.array(["div","pre","p","table","td","th","ul","ol","li","dd","dl","footer","header","section","h1","h2","h3","h4","h5","h6"]).contains(e.nodeName.toLowerCase())&&e.parentNode.lastChild!==e&&(e.nextSibling&&3===e.nextSibling.nodeType&&/^\s/.test(e.nextSibling.nodeValue)||r.appendChild(e.ownerDocument.createTextNode(" "))),r.normalize&&r.normalize(),r}return null}for(f=0;c>f;f++)l[f]&&(a=t(l[f],n,o),a&&(l[f]===a&&f--,i.appendChild(a)));if(n&&i.nodeName.toLowerCase()===h&&(!i.childNodes.length||/^\s*$/gi.test(i.innerHTML)&&(o||"_wysihtml5-temp-placeholder"!==e.className&&"rangySelectionBoundary"!==e.className)||!i.attributes.length)){for(r=i.ownerDocument.createDocumentFragment();i.firstChild;)r.appendChild(i.firstChild);return r.normalize&&r.normalize(),r}return i.normalize&&i.normalize(),i}function n(e,t){var n,r,s=p.tags,l=e.nodeName.toLowerCase(),c=e.scopeName;if(e._wysihtml5)return null;if(e._wysihtml5=1,"wysihtml5-temp"===e.className)return null;if(c&&"HTML"!=c&&(l=c+":"+l),"outerHTML"in e&&(wysihtml5.browser.autoClosesUnclosedTags()||"P"!==e.nodeName||"</p>"===e.outerHTML.slice(-4).toLowerCase()||(l="div")),l in s){if(n=s[l],!n||n.remove)return null;if(n.unwrap)return!1;n="string"==typeof n?{rename_tag:n}:n}else{if(!e.firstChild)return null;n={rename_tag:h}}return r=e.ownerDocument.createElement(n.rename_tag||l),a(e,r,n,t),i(e,r,n),n.one_of_type&&!o(e,p,n.one_of_type,t)?n.remove_action&&"unwrap"==n.remove_action?!1:null:(e=null,r.normalize&&r.normalize(),r)}function o(e,t,n,o){var i,a;if("SPAN"===e.nodeName&&!o&&("_wysihtml5-temp-placeholder"===e.className||"rangySelectionBoundary"===e.className))return!0;for(a in n)if(n.hasOwnProperty(a)&&t.type_definitions&&t.type_definitions[a]&&(i=t.type_definitions[a],r(e,i)))return!0;return!1}function r(e,t){var n,o,r,i,a,l=e.getAttribute("class"),c=e.getAttribute("style");if(t.methods)for(var d in t.methods)if(t.methods.hasOwnProperty(d)&&N[d]&&N[d](e))return!0;if(l&&t.classes){l=l.replace(/^\s+/g,"").replace(/\s+$/g,"").split(f),n=l.length;for(var u=0;n>u;u++)if(t.classes[l[u]])return!0}if(c&&t.styles){c=c.split(";");for(o in t.styles)if(t.styles.hasOwnProperty(o))for(var h=c.length;h--;)if(a=c[h].split(":"),a[0].replace(/\s/g,"").toLowerCase()===o&&(t.styles[o]===!0||1===t.styles[o]||wysihtml5.lang.array(t.styles[o]).contains(a[1].replace(/\s/g,"").toLowerCase())))return!0}if(t.attrs)for(r in t.attrs)if(t.attrs.hasOwnProperty(r)&&(i=s(e,r),"string"==typeof i&&i.search(t.attrs[r])>-1))return!0;return!1}function i(e,t,n){var o;if(n&&n.keep_styles)for(o in n.keep_styles)n.keep_styles.hasOwnProperty(o)&&("float"==o?(e.style.styleFloat&&(t.style.styleFloat=e.style.styleFloat),e.style.cssFloat&&(t.style.cssFloat=e.style.cssFloat)):e.style[o]&&(t.style[o]=e.style[o]))}function a(e,t,n,o){var r,i,a,l,c,d,u,h={},m=n.set_class,g=n.add_class,y=n.add_style,v=n.set_attributes,N=n.check_attributes,x=p.classes,E=0,S=[],T=[],R=[],A=[];if(v&&(h=wysihtml5.lang.object(v).clone()),N)for(l in N)d=w[N[l]],d&&(u=s(e,l),(u||"alt"===l&&"IMG"==e.nodeName)&&(c=d(u),"string"==typeof c&&(h[l]=c)));if(m&&S.push(m),g)for(l in g)d=C[g[l]],d&&(a=d(s(e,l)),"string"==typeof a&&S.push(a));if(y)for(l in y)d=b[y[l]],d&&(newStyle=d(s(e,l)),"string"==typeof newStyle&&T.push(newStyle));if("string"==typeof x&&"any"===x&&e.getAttribute("class"))h["class"]=e.getAttribute("class");else{for(o||(x["_wysihtml5-temp-placeholder"]=1,x._rangySelectionBoundary=1,x["wysiwyg-tmp-selected-cell"]=1),A=e.getAttribute("class"),A&&(S=S.concat(A.split(f))),r=S.length;r>E;E++)i=S[E],x[i]&&R.push(i);R.length&&(h["class"]=wysihtml5.lang.array(R).unique().join(" "))}h["class"]&&o&&(h["class"]=h["class"].replace("wysiwyg-tmp-selected-cell",""),/^\s*$/g.test(h["class"])&&delete h["class"]),T.length&&(h.style=wysihtml5.lang.array(T).unique().join(" "));for(l in h)try{t.setAttribute(l,h[l])}catch(_){}h.src&&("undefined"!=typeof h.width&&t.setAttribute("width",h.width),"undefined"!=typeof h.height&&t.setAttribute("height",h.height))}function s(e,t){t=t.toLowerCase();var n=e.nodeName;if("IMG"==n&&"src"==t&&l(e)===!0)return e.src;if(y&&"outerHTML"in e){var o=e.outerHTML.toLowerCase(),r=-1!=o.indexOf(" "+t+"=");return r?e.getAttribute(t):null}return e.getAttribute(t)}function l(e){try{return e.complete&&!e.mozMatchesSelector(":-moz-broken")}catch(t){if(e.complete&&"complete"===e.readyState)return!0}}function c(e){var t=e.nextSibling;if(!t||t.nodeType!==wysihtml5.TEXT_NODE){var n=e.data.replace(v,"");return e.ownerDocument.createTextNode(n)}t.data=e.data.replace(v,"")+t.data.replace(v,"")}function d(e){return p.comments?e.ownerDocument.createComment(e.nodeValue):void 0}var u={1:n,3:c,8:d},h="span",f=/\s+/,m={tags:{},classes:{}},p={},g=!1,y=!wysihtml5.browser.supportsGetAttributeCorrectly(),v=/\uFEFF/g,w={url:function(){var e=/^https?:\/\//i;return function(t){return t&&t.match(e)?t.replace(e,function(e){return e.toLowerCase()}):null}}(),src:function(){var e=/^(\/|https?:\/\/)/i;return function(t){return t&&t.match(e)?t.replace(e,function(e){return e.toLowerCase()}):null}}(),href:function(){var e=/^(#|\/|https?:\/\/|mailto:)/i;return function(t){return t&&t.match(e)?t.replace(e,function(e){return e.toLowerCase()}):null}}(),alt:function(){var e=/[^ a-z0-9_\-]/gi;return function(t){return t?t.replace(e,""):""}}(),numbers:function(){var e=/\D/g;return function(t){return t=(t||"").replace(e,""),t||null}}(),any:function(){return function(e){return e}}()},b={align_text:function(){var e={left:"text-align: left;",right:"text-align: right;",center:"text-align: center;"};return function(t){return e[String(t).toLowerCase()]}}()},C={align_img:function(){var e={left:"wysiwyg-float-left",right:"wysiwyg-float-right"};return function(t){return e[String(t).toLowerCase()]}}(),align_text:function(){var e={left:"wysiwyg-text-align-left",right:"wysiwyg-text-align-right",center:"wysiwyg-text-align-center",justify:"wysiwyg-text-align-justify"};return function(t){return e[String(t).toLowerCase()]}}(),clear_br:function(){var e={left:"wysiwyg-clear-left",right:"wysiwyg-clear-right",both:"wysiwyg-clear-both",all:"wysiwyg-clear-both"};return function(t){return e[String(t).toLowerCase()]}}(),size_font:function(){var e={1:"wysiwyg-font-size-xx-small",2:"wysiwyg-font-size-small",3:"wysiwyg-font-size-medium",4:"wysiwyg-font-size-large",5:"wysiwyg-font-size-x-large",6:"wysiwyg-font-size-xx-large",7:"wysiwyg-font-size-xx-large","-":"wysiwyg-font-size-smaller","+":"wysiwyg-font-size-larger"};return function(t){return e[String(t).charAt(0)]}}()},N={has_visible_contet:function(){var e,t=["img","video","picture","br","script","noscript","style","table","iframe","object","embed","audio","svg","input","button","select","textarea","canvas"];return function(n){if(e=(n.innerText||n.textContent).replace(/\s/g,""),e&&e.length>0)return!0;for(var o=t.length;o--;)if(n.querySelector(t[o]))return!0;return n.offsetWidth&&n.offsetWidth>0&&n.offsetHeight&&n.offsetHeight>0?!0:!1}}()};return e}(),wysihtml5.dom.removeEmptyTextNodes=function(e){for(var t,n=wysihtml5.lang.array(e.childNodes).get(),o=n.length,r=0;o>r;r++)t=n[r],t.nodeType===wysihtml5.TEXT_NODE&&""===t.data&&t.parentNode.removeChild(t)},wysihtml5.dom.renameElement=function(e,t){for(var n,o=e.ownerDocument.createElement(t);n=e.firstChild;)o.appendChild(n);return wysihtml5.dom.copyAttributes(["align","className"]).from(e).to(o),e.parentNode.replaceChild(o,e),o},wysihtml5.dom.replaceWithChildNodes=function(e){if(e.parentNode){if(!e.firstChild)return void e.parentNode.removeChild(e);for(var t=e.ownerDocument.createDocumentFragment();e.firstChild;)t.appendChild(e.firstChild);e.parentNode.replaceChild(t,e),e=t=null}},function(e){function t(t){return"block"===e.getStyle("display").from(t)}function n(e){return"BR"===e.nodeName}function o(e){var t=e.ownerDocument.createElement("br");e.appendChild(t)}function r(e,r){if(e.nodeName.match(/^(MENU|UL|OL)$/)){var i,a,s,l,c,d,u=e.ownerDocument,h=u.createDocumentFragment(),f=wysihtml5.dom.domNode(e).prev({ignoreBlankTexts:!0});if(r)for(!f||t(f)||n(f)||o(h);d=e.firstElementChild||e.firstChild;){for(a=d.lastChild;i=d.firstChild;)s=i===a,l=s&&!t(i)&&!n(i),h.appendChild(i),l&&o(h);d.parentNode.removeChild(d)}else for(;d=e.firstElementChild||e.firstChild;){if(d.querySelector&&d.querySelector("div, p, ul, ol, menu, blockquote, h1, h2, h3, h4, h5, h6"))for(;i=d.firstChild;)h.appendChild(i);else{for(c=u.createElement("p");i=d.firstChild;)c.appendChild(i);h.appendChild(c)}d.parentNode.removeChild(d)}e.parentNode.replaceChild(h,e)}}e.resolveList=r}(wysihtml5.dom),function(e){var t=document,n=["parent","top","opener","frameElement","frames","localStorage","globalStorage","sessionStorage","indexedDB"],o=["open","close","openDialog","showModalDialog","alert","confirm","prompt","openDatabase","postMessage","XMLHttpRequest","XDomainRequest"],r=["referrer","write","open","close"];e.dom.Sandbox=Base.extend({constructor:function(t,n){this.callback=t||e.EMPTY_FUNCTION,this.config=e.lang.object({}).merge(n).get(),this.editableArea=this._createIframe()},insertInto:function(e){"string"==typeof e&&(e=t.getElementById(e)),e.appendChild(this.editableArea)},getIframe:function(){return this.editableArea},getWindow:function(){this._readyError()},getDocument:function(){this._readyError()},destroy:function(){var e=this.getIframe();e.parentNode.removeChild(e)},_readyError:function(){throw new Error("wysihtml5.Sandbox: Sandbox iframe isn't loaded yet")},_createIframe:function(){var n=this,o=t.createElement("iframe");return o.className="wysihtml5-sandbox",e.dom.setAttributes({security:"restricted",allowtransparency:"true",frameborder:0,width:0,height:0,marginwidth:0,marginheight:0}).on(o),e.browser.throwsMixedContentWarningWhenIframeSrcIsEmpty()&&(o.src="javascript:'<html></html>'"),o.onload=function(){o.onreadystatechange=o.onload=null,n._onLoadIframe(o)},o.onreadystatechange=function(){/loaded|complete/.test(o.readyState)&&(o.onreadystatechange=o.onload=null,n._onLoadIframe(o))},o},_onLoadIframe:function(i){if(e.dom.contains(t.documentElement,i)){var a=this,s=i.contentWindow,l=i.contentWindow.document,c=t.characterSet||t.charset||"utf-8",d=this._getHtml({charset:c,stylesheets:this.config.stylesheets});if(l.open("text/html","replace"),l.write(d),l.close(),this.getWindow=function(){return i.contentWindow},this.getDocument=function(){return i.contentWindow.document},s.onerror=function(e,t,n){throw new Error("wysihtml5.Sandbox: "+e,t,n)},!e.browser.supportsSandboxedIframes()){var u,h;for(u=0,h=n.length;h>u;u++)this._unset(s,n[u]);for(u=0,h=o.length;h>u;u++)this._unset(s,o[u],e.EMPTY_FUNCTION);for(u=0,h=r.length;h>u;u++)this._unset(l,r[u]);this._unset(l,"cookie","",!0)}this.loaded=!0,setTimeout(function(){a.callback(a)},0)}},_getHtml:function(t){var n,o=t.stylesheets,r="",i=0;if(o="string"==typeof o?[o]:o)for(n=o.length;n>i;i++)r+='<link rel="stylesheet" href="'+o[i]+'">';return t.stylesheets=r,e.lang.string('<!DOCTYPE html><html><head><meta charset="#{charset}">#{stylesheets}</head><body></body></html>').interpolate(t)},_unset:function(t,n,o,r){try{t[n]=o}catch(i){}try{t.__defineGetter__(n,function(){return o})}catch(i){}if(r)try{t.__defineSetter__(n,function(){})}catch(i){}if(!e.browser.crashesWhenDefineProperty(n))try{var a={get:function(){return o}};r&&(a.set=function(){}),Object.defineProperty(t,n,a)}catch(i){}}})}(wysihtml5),function(e){var t=document;e.dom.ContentEditableArea=Base.extend({getContentEditable:function(){return this.element},getWindow:function(){return this.element.ownerDocument.defaultView},getDocument:function(){return this.element.ownerDocument},constructor:function(t,n,o){this.callback=t||e.EMPTY_FUNCTION,this.config=e.lang.object({}).merge(n).get(),o?this.element=this._bindElement(o):this.element=this._createElement()},_createElement:function(){var e=t.createElement("div");return e.className="wysihtml5-sandbox",this._loadElement(e),e},_bindElement:function(e){return e.className=e.className&&""!=e.className?e.className+" wysihtml5-sandbox":"wysihtml5-sandbox",this._loadElement(e,!0),e},_loadElement:function(e,t){var n=this;if(!t){var o=this._getHtml();e.innerHTML=o}this.getWindow=function(){return e.ownerDocument.defaultView},this.getDocument=function(){return e.ownerDocument},this.loaded=!0,setTimeout(function(){n.callback(n)},0)},_getHtml:function(e){return""}})}(wysihtml5),function(){var e={className:"class"};wysihtml5.dom.setAttributes=function(t){return{on:function(n){for(var o in t)n.setAttribute(e[o]||o,t[o])}}}}(),wysihtml5.dom.setStyles=function(e){return{on:function(t){var n=t.style;if("string"==typeof e)return void(n.cssText+=";"+e);for(var o in e)"float"===o?(n.cssFloat=e[o],n.styleFloat=e[o]):n[o]=e[o]}}},function(e){e.simulatePlaceholder=function(t,n,o){var r="placeholder",i=function(){var t=n.element.offsetWidth>0&&n.element.offsetHeight>0;n.hasPlaceholderSet()&&(n.clear(),n.element.focus(),t&&setTimeout(function(){var e=n.selection.getSelection();e.focusNode&&e.anchorNode||n.selection.selectNode(n.element.firstChild||n.element)},0)),n.placeholderSet=!1,e.removeClass(n.element,r)},a=function(){n.isEmpty()&&(n.placeholderSet=!0,n.setValue(o),e.addClass(n.element,r))};t.on("set_placeholder",a).on("unset_placeholder",i).on("focus:composer",i).on("paste:composer",i).on("blur:composer",a),a()}}(wysihtml5.dom),function(e){var t=document.documentElement;"textContent"in t?(e.setTextContent=function(e,t){e.textContent=t},e.getTextContent=function(e){return e.textContent}):"innerText"in t?(e.setTextContent=function(e,t){e.innerText=t},e.getTextContent=function(e){return e.innerText}):(e.setTextContent=function(e,t){e.nodeValue=t},e.getTextContent=function(e){return e.nodeValue})}(wysihtml5.dom),wysihtml5.dom.getAttribute=function(e,t){var n=!wysihtml5.browser.supportsGetAttributeCorrectly();t=t.toLowerCase();var o=e.nodeName;if("IMG"==o&&"src"==t&&_isLoadedImage(e)===!0)return e.src;if(n&&"outerHTML"in e){var r=e.outerHTML.toLowerCase(),i=-1!=r.indexOf(" "+t+"=");return i?e.getAttribute(t):null}return e.getAttribute(t)},function(e){function t(e,t){for(var n,o=[],r=0,i=e.length;i>r;r++)if(n=e[r].querySelectorAll(t))for(var a=n.length;a--;o.unshift(n[a]));return o}function n(e){e.parentNode.removeChild(e)}function o(e,t){e.parentNode.insertBefore(t,e.nextSibling)}function r(e,t){for(var n=e.nextSibling;1!=n.nodeType;)if(n=n.nextSibling,!t||t==n.tagName.toLowerCase())return n;return null}var i=e.dom,a=function(e){this.el=e,this.isColspan=!1,this.isRowspan=!1,this.firstCol=!0,this.lastCol=!0,this.firstRow=!0,this.lastRow=!0,this.isReal=!0,this.spanCollection=[],this.modified=!1},s=function(e,t){e?(this.cell=e,this.table=i.getParentElement(e,{nodeName:["TABLE"]})):t&&(this.table=t,this.cell=this.table.querySelectorAll("th, td")[0])};s.prototype={addSpannedCellToMap:function(e,t,n,o,r,i){for(var s=[],l=n+(i?parseInt(i,10)-1:0),c=o+(r?parseInt(r,10)-1:0),d=n;l>=d;d++){"undefined"==typeof t[d]&&(t[d]=[]);for(var u=o;c>=u;u++)t[d][u]=new a(e),t[d][u].isColspan=r&&parseInt(r,10)>1,t[d][u].isRowspan=i&&parseInt(i,10)>1,t[d][u].firstCol=u==o,t[d][u].lastCol=u==c,t[d][u].firstRow=d==n,t[d][u].lastRow=d==l,t[d][u].isReal=u==o&&d==n,t[d][u].spanCollection=s,s.push(t[d][u])}},setCellAsModified:function(e){if(e.modified=!0,e.spanCollection.length>0)for(var t=0,n=e.spanCollection.length;n>t;t++)e.spanCollection[t].modified=!0},setTableMap:function(){var e,t,n,o,r,s,l,c,d=[],u=this.getTableRows();for(e=0;e<u.length;e++)for(t=u[e],n=this.getRowCells(t),s=0,"undefined"==typeof d[e]&&(d[e]=[]),o=0;o<n.length;o++){for(r=n[o];"undefined"!=typeof d[e][s];)s++;l=i.getAttribute(r,"colspan"),c=i.getAttribute(r,"rowspan"),l||c?(this.addSpannedCellToMap(r,d,e,s,l,c),s+=l?parseInt(l,10):1):(d[e][s]=new a(r),s++)}return this.map=d,d},getRowCells:function(n){var o=this.table.querySelectorAll("table"),r=o?t(o,"th, td"):[],i=n.querySelectorAll("th, td"),a=r.length>0?e.lang.array(i).without(r):i;return a},getTableRows:function(){var n=this.table.querySelectorAll("table"),o=n?t(n,"tr"):[],r=this.table.querySelectorAll("tr"),i=o.length>0?e.lang.array(r).without(o):r;return i},getMapIndex:function(e){for(var t=this.map.length,n=this.map&&this.map[0]?this.map[0].length:0,o=0;t>o;o++)for(var r=0;n>r;r++)if(this.map[o][r].el===e)return{row:o,col:r};return!1},getElementAtIndex:function(e){return this.setTableMap(),this.map[e.row]&&this.map[e.row][e.col]&&this.map[e.row][e.col].el?this.map[e.row][e.col].el:null},getMapElsTo:function(e){var t=[];if(this.setTableMap(),this.idx_start=this.getMapIndex(this.cell),this.idx_end=this.getMapIndex(e),this.idx_start.row>this.idx_end.row||this.idx_start.row==this.idx_end.row&&this.idx_start.col>this.idx_end.col){var n=this.idx_start;this.idx_start=this.idx_end,this.idx_end=n}if(this.idx_start.col>this.idx_end.col){var o=this.idx_start.col;this.idx_start.col=this.idx_end.col,this.idx_end.col=o}if(null!=this.idx_start&&null!=this.idx_end)for(var r=this.idx_start.row,i=this.idx_end.row;i>=r;r++)for(var a=this.idx_start.col,s=this.idx_end.col;s>=a;a++)t.push(this.map[r][a].el);return t},orderSelectionEnds:function(e){if(this.setTableMap(),this.idx_start=this.getMapIndex(this.cell),this.idx_end=this.getMapIndex(e),this.idx_start.row>this.idx_end.row||this.idx_start.row==this.idx_end.row&&this.idx_start.col>this.idx_end.col){var t=this.idx_start;this.idx_start=this.idx_end,this.idx_end=t}if(this.idx_start.col>this.idx_end.col){var n=this.idx_start.col;this.idx_start.col=this.idx_end.col,this.idx_end.col=n}return{start:this.map[this.idx_start.row][this.idx_start.col].el,end:this.map[this.idx_end.row][this.idx_end.col].el}},createCells:function(e,t,n){for(var o,r=this.table.ownerDocument,i=r.createDocumentFragment(),a=0;t>a;a++){if(o=r.createElement(e),n)for(var s in n)n.hasOwnProperty(s)&&o.setAttribute(s,n[s]);o.appendChild(document.createTextNode(" ")),i.appendChild(o)}return i},correctColIndexForUnreals:function(e,t){for(var n=this.map[t],o=-1,r=0;e>r;r++)n[r].isReal&&o++;return o},getLastNewCellOnRow:function(e,t){for(var n,o,r=this.getRowCells(e),i=0,a=r.length;a>i;i++)if(n=r[i],o=this.getMapIndex(n),o===!1||"undefined"!=typeof t&&o.row!=t)return n;return null},removeEmptyTable:function(){var e=this.table.querySelectorAll("td, th");return e&&0!=e.length?!1:(n(this.table),!0)},splitRowToCells:function(e){if(e.isColspan){var t=parseInt(i.getAttribute(e.el,"colspan")||1,10),n=e.el.tagName.toLowerCase();if(t>1){var r=this.createCells(n,t-1);o(e.el,r)}e.el.removeAttribute("colspan")}},getRealRowEl:function(e,t){var n=null,o=null;t=t||this.idx;for(var r=0,a=this.map[t.row].length;a>r;r++)if(o=this.map[t.row][r],o.isReal&&(n=i.getParentElement(o.el,{nodeName:["TR"]})))return n;return null===n&&e&&(n=i.getParentElement(this.map[t.row][t.col].el,{nodeName:["TR"]})||null),n},injectRowAt:function(e,t,n,r,a){var s=this.getRealRowEl(!1,{row:e,col:t}),l=this.createCells(r,n);if(s){var c=this.correctColIndexForUnreals(t,e);c>=0?o(this.getRowCells(s)[c],l):s.insertBefore(l,s.firstChild)}else{var d=this.table.ownerDocument.createElement("tr");d.appendChild(l),o(i.getParentElement(a.el,{nodeName:["TR"]}),d)}},canMerge:function(e){if(this.to=e,this.setTableMap(),this.idx_start=this.getMapIndex(this.cell),this.idx_end=this.getMapIndex(this.to),this.idx_start.row>this.idx_end.row||this.idx_start.row==this.idx_end.row&&this.idx_start.col>this.idx_end.col){var t=this.idx_start;this.idx_start=this.idx_end,this.idx_end=t}if(this.idx_start.col>this.idx_end.col){var n=this.idx_start.col;this.idx_start.col=this.idx_end.col,this.idx_end.col=n}for(var o=this.idx_start.row,r=this.idx_end.row;r>=o;o++)for(var i=this.idx_start.col,a=this.idx_end.col;a>=i;i++)if(this.map[o][i].isColspan||this.map[o][i].isRowspan)return!1;return!0},decreaseCellSpan:function(e,t){var n=parseInt(i.getAttribute(e.el,t),10)-1;n>=1?e.el.setAttribute(t,n):(e.el.removeAttribute(t),"colspan"==t&&(e.isColspan=!1),"rowspan"==t&&(e.isRowspan=!1),e.firstCol=!0,e.lastCol=!0,e.firstRow=!0,e.lastRow=!0,e.isReal=!0)},removeSurplusLines:function(){var e,t,o,r,a,s,l;if(this.setTableMap(),this.map){for(o=0,r=this.map.length;r>o;o++){for(e=this.map[o],l=!0,a=0,s=e.length;s>a;a++)if(t=e[a],!(i.getAttribute(t.el,"rowspan")&&parseInt(i.getAttribute(t.el,"rowspan"),10)>1&&t.firstRow!==!0)){l=!1;break}if(l)for(a=0;s>a;a++)this.decreaseCellSpan(e[a],"rowspan")}var c=this.getTableRows();for(o=0,r=c.length;r>o;o++)e=c[o],0==e.childNodes.length&&/^\s*$/.test(e.textContent||e.innerText)&&n(e)}},fillMissingCells:function(){var e=0,t=0,n=null;if(this.setTableMap(),this.map){e=this.map.length;for(var r=0;e>r;r++)this.map[r].length>t&&(t=this.map[r].length);for(var i=0;e>i;i++)for(var s=0;t>s;s++)this.map[i]&&!this.map[i][s]&&s>0&&(this.map[i][s]=new a(this.createCells("td",1)),n=this.map[i][s-1],n&&n.el&&n.el.parent&&o(this.map[i][s-1].el,this.map[i][s].el))}},rectify:function(){return this.removeEmptyTable()?!1:(this.removeSurplusLines(),this.fillMissingCells(),!0)},unmerge:function(){if(this.rectify()&&(this.setTableMap(),this.idx=this.getMapIndex(this.cell),this.idx)){var e=this.map[this.idx.row][this.idx.col],t=i.getAttribute(e.el,"colspan")?parseInt(i.getAttribute(e.el,"colspan"),10):1,n=e.el.tagName.toLowerCase();if(e.isRowspan){var o=parseInt(i.getAttribute(e.el,"rowspan"),10);if(o>1)for(var r=1,a=o-1;a>=r;r++)this.injectRowAt(this.idx.row+r,this.idx.col,t,n,e);e.el.removeAttribute("rowspan")}this.splitRowToCells(e)}},merge:function(e){if(this.rectify())if(this.canMerge(e)){for(var t=this.idx_end.row-this.idx_start.row+1,o=this.idx_end.col-this.idx_start.col+1,r=this.idx_start.row,i=this.idx_end.row;i>=r;r++)for(var a=this.idx_start.col,s=this.idx_end.col;s>=a;a++)r==this.idx_start.row&&a==this.idx_start.col?(t>1&&this.map[r][a].el.setAttribute("rowspan",t),o>1&&this.map[r][a].el.setAttribute("colspan",o)):(/^\s*<br\/?>\s*$/.test(this.map[r][a].el.innerHTML.toLowerCase())||(this.map[this.idx_start.row][this.idx_start.col].el.innerHTML+=" "+this.map[r][a].el.innerHTML),n(this.map[r][a].el));this.rectify()}else window.console&&console.log("Do not know how to merge allready merged cells.")},collapseCellToNextRow:function(e){var t=this.getMapIndex(e.el),n=t.row+1,r={row:n,col:t.col};if(n<this.map.length){var a=this.getRealRowEl(!1,r);if(null!==a){var s=this.correctColIndexForUnreals(r.col,r.row);if(s>=0)o(this.getRowCells(a)[s],e.el);else{var l=this.getLastNewCellOnRow(a,n);null!==l?o(l,e.el):a.insertBefore(e.el,a.firstChild)}parseInt(i.getAttribute(e.el,"rowspan"),10)>2?e.el.setAttribute("rowspan",parseInt(i.getAttribute(e.el,"rowspan"),10)-1):e.el.removeAttribute("rowspan")}}},removeRowCell:function(e){e.isReal?e.isRowspan?this.collapseCellToNextRow(e):n(e.el):parseInt(i.getAttribute(e.el,"rowspan"),10)>2?e.el.setAttribute("rowspan",parseInt(i.getAttribute(e.el,"rowspan"),10)-1):e.el.removeAttribute("rowspan")},getRowElementsByCell:function(){var e=[];if(this.setTableMap(),this.idx=this.getMapIndex(this.cell),this.idx!==!1)for(var t=this.map[this.idx.row],n=0,o=t.length;o>n;n++)t[n].isReal&&e.push(t[n].el);return e},getColumnElementsByCell:function(){var e=[];if(this.setTableMap(),this.idx=this.getMapIndex(this.cell),this.idx!==!1)for(var t=0,n=this.map.length;n>t;t++)this.map[t][this.idx.col]&&this.map[t][this.idx.col].isReal&&e.push(this.map[t][this.idx.col].el);return e},removeRow:function(){var e=i.getParentElement(this.cell,{nodeName:["TR"]});if(e){if(this.setTableMap(),this.idx=this.getMapIndex(this.cell),this.idx!==!1)for(var t=this.map[this.idx.row],o=0,r=t.length;r>o;o++)t[o].modified||(this.setCellAsModified(t[o]),this.removeRowCell(t[o]));n(e)}},removeColCell:function(e){e.isColspan?parseInt(i.getAttribute(e.el,"colspan"),10)>2?e.el.setAttribute("colspan",parseInt(i.getAttribute(e.el,"colspan"),10)-1):e.el.removeAttribute("colspan"):e.isReal&&n(e.el)},removeColumn:function(){if(this.setTableMap(),this.idx=this.getMapIndex(this.cell),this.idx!==!1)for(var e=0,t=this.map.length;t>e;e++)this.map[e][this.idx.col].modified||(this.setCellAsModified(this.map[e][this.idx.col]),this.removeColCell(this.map[e][this.idx.col]))},remove:function(e){if(this.rectify()){switch(e){case"row":this.removeRow();break;case"column":this.removeColumn()}this.rectify()}},addRow:function(e){var t=this.table.ownerDocument;if(this.setTableMap(),this.idx=this.getMapIndex(this.cell),"below"==e&&i.getAttribute(this.cell,"rowspan")&&(this.idx.row=this.idx.row+parseInt(i.getAttribute(this.cell,"rowspan"),10)-1),this.idx!==!1){for(var n=this.map[this.idx.row],r=t.createElement("tr"),a=0,s=n.length;s>a;a++)n[a].modified||(this.setCellAsModified(n[a]),this.addRowCell(n[a],r,e));switch(e){case"below":o(this.getRealRowEl(!0),r);break;case"above":var l=i.getParentElement(this.map[this.idx.row][this.idx.col].el,{nodeName:["TR"]});l&&l.parentNode.insertBefore(r,l)}}},addRowCell:function(e,t,n){var o=e.isColspan?{colspan:i.getAttribute(e.el,"colspan")}:null;e.isReal?"above"!=n&&e.isRowspan?e.el.setAttribute("rowspan",parseInt(i.getAttribute(e.el,"rowspan"),10)+1):t.appendChild(this.createCells("td",1,o)):"above"!=n&&e.isRowspan&&e.lastRow?t.appendChild(this.createCells("td",1,o)):c.isRowspan&&e.el.attr("rowspan",parseInt(i.getAttribute(e.el,"rowspan"),10)+1)},add:function(e){this.rectify()&&(("below"==e||"above"==e)&&this.addRow(e),("before"==e||"after"==e)&&this.addColumn(e))},addColCell:function(e,t,n){var r,a=e.el.tagName.toLowerCase();switch(n){case"before":r=!e.isColspan||e.firstCol;break;case"after":r=!e.isColspan||e.lastCol||e.isColspan&&c.el==this.cell}if(r){switch(n){case"before":e.el.parentNode.insertBefore(this.createCells(a,1),e.el);break;case"after":o(e.el,this.createCells(a,1))}e.isRowspan&&this.handleCellAddWithRowspan(e,t+1,n)}else e.el.setAttribute("colspan",parseInt(i.getAttribute(e.el,"colspan"),10)+1)},addColumn:function(e){var t,n;if(this.setTableMap(),this.idx=this.getMapIndex(this.cell),"after"==e&&i.getAttribute(this.cell,"colspan")&&(this.idx.col=this.idx.col+parseInt(i.getAttribute(this.cell,"colspan"),10)-1),this.idx!==!1)for(var o=0,r=this.map.length;r>o;o++)t=this.map[o],t[this.idx.col]&&(n=t[this.idx.col],n.modified||(this.setCellAsModified(n),this.addColCell(n,o,e)))},handleCellAddWithRowspan:function(e,t,n){for(var a,s,l,c=parseInt(i.getAttribute(this.cell,"rowspan"),10)-1,d=i.getParentElement(e.el,{nodeName:["TR"]}),u=e.el.tagName.toLowerCase(),h=this.table.ownerDocument,f=0;c>f;f++)if(a=this.correctColIndexForUnreals(this.idx.col,t+f),d=r(d,"tr"))if(a>0)switch(n){case"before":s=this.getRowCells(d),a>0&&this.map[t+f][this.idx.col].el!=s[a]&&a==s.length-1?o(s[a],this.createCells(u,1)):s[a].parentNode.insertBefore(this.createCells(u,1),s[a]);break;case"after":o(this.getRowCells(d)[a],this.createCells(u,1))}else d.insertBefore(this.createCells(u,1),d.firstChild);else l=h.createElement("tr"),l.appendChild(this.createCells(u,1)),this.table.appendChild(l)}},i.table={getCellsBetween:function(e,t){var n=new s(e);return n.getMapElsTo(t)},addCells:function(e,t){var n=new s(e);n.add(t)},removeCells:function(e,t){var n=new s(e);n.remove(t)},mergeCellsBetween:function(e,t){var n=new s(e);n.merge(t)},unmergeCell:function(e){var t=new s(e);t.unmerge()},orderSelectionEnds:function(e,t){var n=new s(e);return n.orderSelectionEnds(t)},indexOf:function(e){var t=new s(e);return t.setTableMap(),t.getMapIndex(e)},findCell:function(e,t){var n=new s(null,e);return n.getElementAtIndex(t)},findRowByCell:function(e){var t=new s(e);return t.getRowElementsByCell()},findColumnByCell:function(e){var t=new s(e);return t.getColumnElementsByCell()},canMerge:function(e,t){var n=new s(e);return n.canMerge(t)}}}(wysihtml5),wysihtml5.dom.query=function(e,t){var n,o=[];e.nodeType&&(e=[e]);for(var r=0,i=e.length;i>r;r++)if(n=e[r].querySelectorAll(t))for(var a=n.length;a--;o.unshift(n[a]));return o},wysihtml5.dom.compareDocumentPosition=function(){var e=document.documentElement;return e.compareDocumentPosition?function(e,t){return e.compareDocumentPosition(t)}:function(e,t){var n,o;if(n=9===e.nodeType?e:e.ownerDocument,o=9===t.nodeType?t:t.ownerDocument,e===t)return 0;if(e===t.ownerDocument)return 20;if(e.ownerDocument===t)return 10;if(n!==o)return 1;if(2===e.nodeType&&e.childNodes&&-1!==wysihtml5.lang.array(e.childNodes).indexOf(t))return 20;if(2===t.nodeType&&t.childNodes&&-1!==wysihtml5.lang.array(t.childNodes).indexOf(e))return 10;for(var r=e,i=[],a=null;r;){if(r==t)return 10;i.push(r),r=r.parentNode}for(r=t,a=null;r;){if(r==e)return 20;var s=wysihtml5.lang.array(i).indexOf(r);if(-1!==s){var l=i[s],c=wysihtml5.lang.array(l.childNodes).indexOf(i[s-1]),d=wysihtml5.lang.array(l.childNodes).indexOf(a);
return c>d?2:4}a=r,r=r.parentNode}return 1}}(),wysihtml5.dom.unwrap=function(e){if(e.parentNode){for(;e.lastChild;)wysihtml5.dom.insert(e.lastChild).after(e);e.parentNode.removeChild(e)}},wysihtml5.quirks.cleanPastedHTML=function(){function e(e,n,o){n=n||t,o=o||e.ownerDocument||document;var r,i,a,s,l,c,d="string"==typeof e,u=0;r=d?wysihtml5.dom.getAsDom(e,o):e;for(l in n)for(a=r.querySelectorAll(l),i=n[l],s=a.length;s>u;u++)i(a[u]);var h=wysihtml5.dom.getTextNodes(r);for(c=h.length;c--;)h[c].nodeValue=h[c].nodeValue.replace(/([\S\u00A0])\u00A0/gi,"$1 ");return a=e=n=null,d?r.innerHTML:r}var t={"a u":wysihtml5.dom.replaceWithChildNodes};return e}(),wysihtml5.quirks.ensureProperClearing=function(){var e=function(){var e=this;setTimeout(function(){var t=e.innerHTML.toLowerCase();("<p>&nbsp;</p>"==t||"<p>&nbsp;</p><p>&nbsp;</p>"==t)&&(e.innerHTML="")},0)};return function(t){wysihtml5.dom.observe(t.element,["cut","keydown"],e)}}(),function(e){var t="%7E";e.quirks.getCorrectInnerHTML=function(n){var o=n.innerHTML;if(-1===o.indexOf(t))return o;var r,i,a,s,l=n.querySelectorAll("[href*='~'], [src*='~']");for(s=0,a=l.length;a>s;s++)r=l[s].href||l[s].src,i=e.lang.string(r).replace("~").by(t),o=e.lang.string(o).replace(i).by(r);return o}}(wysihtml5),function(e){var t="wysihtml5-quirks-redraw";e.quirks.redraw=function(n){e.dom.addClass(n,t),e.dom.removeClass(n,t);try{var o=n.ownerDocument;o.execCommand("italic",!1,null),o.execCommand("italic",!1,null)}catch(r){}}}(wysihtml5),wysihtml5.quirks.tableCellsSelection=function(e,t){function n(){return d.observe(e,"mousedown",function(e){var t=wysihtml5.dom.getParentElement(e.target,{nodeName:["TD","TH"]});t&&o(t)}),u}function o(n){u.start=n,u.end=n,u.cells=[n],u.table=d.getParentElement(u.start,{nodeName:["TABLE"]}),u.table&&(r(),d.addClass(n,h),f=d.observe(e,"mousemove",a),m=d.observe(e,"mouseup",s),t.fire("tableselectstart").fire("tableselectstart:composer"))}function r(){if(e){var t=e.querySelectorAll("."+h);if(t.length>0)for(var n=0;n<t.length;n++)d.removeClass(t[n],h)}}function i(e){for(var t=0;t<e.length;t++)d.addClass(e[t],h)}function a(e){var n,o=null,a=d.getParentElement(e.target,{nodeName:["TD","TH"]});a&&u.table&&u.start&&(o=d.getParentElement(a,{nodeName:["TABLE"]}),o&&o===u.table&&(r(),n=u.end,u.end=a,u.cells=d.table.getCellsBetween(u.start,a),u.cells.length>1&&t.composer.selection.deselect(),i(u.cells),u.end!==n&&t.fire("tableselectchange").fire("tableselectchange:composer")))}function s(e){f.stop(),m.stop(),t.fire("tableselect").fire("tableselect:composer"),setTimeout(function(){l()},0)}function l(){var n=d.observe(e.ownerDocument,"click",function(e){n.stop(),d.getParentElement(e.target,{nodeName:["TABLE"]})!=u.table&&(r(),u.table=null,u.start=null,u.end=null,t.fire("tableunselect").fire("tableunselect:composer"))})}function c(e,n){u.start=e,u.end=n,u.table=d.getParentElement(u.start,{nodeName:["TABLE"]}),selectedCells=d.table.getCellsBetween(u.start,u.end),i(selectedCells),l(),t.fire("tableselect").fire("tableselect:composer")}var d=wysihtml5.dom,u={table:null,start:null,end:null,cells:null,select:c},h="wysiwyg-tmp-selected-cell",f=null,m=null;return n()},function(e){var t=/^rgba\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*([\d\.]+)\s*\)/i,n=/^rgb\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*(\d{1,3})\s*\)/i,o=/^#([0-9a-f][0-9a-f])([0-9a-f][0-9a-f])([0-9a-f][0-9a-f])/i,r=/^#([0-9a-f])([0-9a-f])([0-9a-f])/i,i=function(e){return new RegExp("(^|\\s|;)"+e+"\\s*:\\s*[^;$]+","gi")};e.quirks.styleParser={parseColor:function(a,s){var l,c,d=i(s),u=a.match(d),h=10;if(u){for(var f=u.length;f--;)u[f]=e.lang.string(u[f].split(":")[1]).trim();if(l=u[u.length-1],t.test(l))c=l.match(t);else if(n.test(l))c=l.match(n);else if(o.test(l))c=l.match(o),h=16;else if(r.test(l))return c=l.match(r),c.shift(),c.push(1),e.lang.array(c).map(function(e,t){return 3>t?16*parseInt(e,16)+parseInt(e,16):parseFloat(e)});if(c)return c.shift(),c[3]||c.push(1),e.lang.array(c).map(function(e,t){return 3>t?parseInt(e,h):parseFloat(e)})}return!1},unparseColor:function(e,t){if(t){if("hex"==t)return e[0].toString(16).toUpperCase()+e[1].toString(16).toUpperCase()+e[2].toString(16).toUpperCase();if("hash"==t)return"#"+e[0].toString(16).toUpperCase()+e[1].toString(16).toUpperCase()+e[2].toString(16).toUpperCase();if("rgb"==t)return"rgb("+e[0]+","+e[1]+","+e[2]+")";if("rgba"==t)return"rgba("+e[0]+","+e[1]+","+e[2]+","+e[3]+")";if("csv"==t)return e[0]+","+e[1]+","+e[2]+","+e[3]}return e[3]&&1!==e[3]?"rgba("+e[0]+","+e[1]+","+e[2]+","+e[3]+")":"rgb("+e[0]+","+e[1]+","+e[2]+")"},parseFontSize:function(t){var n=t.match(i("font-size"));return n?e.lang.string(n[n.length-1].split(":")[1]).trim():!1}}}(wysihtml5),function(e){function t(e){var t=0;if(e.parentNode)do t+=e.offsetTop||0,e=e.offsetParent;while(e);return t}function n(e,t){for(var n=0;t!==e;)if(n++,t=t.parentNode,!t)throw new Error("not a descendant of ancestor!");return n}function o(e){if(!e.canSurroundContents())for(var t=e.commonAncestorContainer,o=n(t,e.startContainer),r=n(t,e.endContainer);!e.canSurroundContents();)o>r?(e.setStartBefore(e.startContainer),o=n(t,e.startContainer)):(e.setEndAfter(e.endContainer),r=n(t,e.endContainer))}var r=e.dom;e.Selection=Base.extend({constructor:function(e,t,n){window.rangy.init(),this.editor=e,this.composer=e.composer,this.doc=this.composer.doc,this.contain=t,this.unselectableClass=n||!1},getBookmark:function(){var e=this.getRange();return e&&o(e),e&&e.cloneRange()},setBookmark:function(e){e&&this.setSelection(e)},setBefore:function(e){var t=rangy.createRange(this.doc);return t.setStartBefore(e),t.setEndBefore(e),this.setSelection(t)},setAfter:function(e){var t=rangy.createRange(this.doc);return t.setStartAfter(e),t.setEndAfter(e),this.setSelection(t)},selectNode:function(t,n){var o=rangy.createRange(this.doc),i=t.nodeType===e.ELEMENT_NODE,a="canHaveHTML"in t?t.canHaveHTML:"IMG"!==t.nodeName,s=i?t.innerHTML:t.data,l=""===s||s===e.INVISIBLE_SPACE,c=r.getStyle("display").from(t),d="block"===c||"list-item"===c;if(l&&i&&a&&!n)try{t.innerHTML=e.INVISIBLE_SPACE}catch(u){}a?o.selectNodeContents(t):o.selectNode(t),a&&l&&i?o.collapse(d):a&&l&&(o.setStartAfter(t),o.setEndAfter(t)),this.setSelection(o)},getSelectedNode:function(e){var t,n;return e&&this.doc.selection&&"Control"===this.doc.selection.type&&(n=this.doc.selection.createRange(),n&&n.length)?n.item(0):(t=this.getSelection(this.doc),t.focusNode===t.anchorNode?t.focusNode:(n=this.getRange(this.doc),n?n.commonAncestorContainer:this.doc.body))},fixSelBorders:function(){var e=this.getRange();o(e),this.setSelection(e)},getSelectedOwnNodes:function(e){for(var t=this.getOwnRanges(),n=[],o=0,r=t.length;r>o;o++)n.push(t[o].commonAncestorContainer||this.doc.body);return n},findNodesInSelection:function(t){for(var n,o=this.getOwnRanges(),r=[],i=0,a=o.length;a>i;i++)n=o[i].getNodes([1],function(n){return e.lang.array(t).contains(n.nodeName)}),r=r.concat(n);return r},containsUneditable:function(){for(var e=this.getOwnUneditables(),t=this.getSelection(),n=0,o=e.length;o>n;n++)if(t.containsNode(e[n]))return!0;return!1},deleteContents:function(){for(var e=this.getOwnRanges(),t=e.length;t--;)e[t].deleteContents();this.setSelection(e[0])},getPreviousNode:function(t,n){if(!t){var o=this.getSelection();t=o.anchorNode}if(t===this.contain)return!1;var r,i=t.previousSibling;return i===this.contain?!1:(i&&3!==i.nodeType&&1!==i.nodeType?i=this.getPreviousNode(i,n):i&&3===i.nodeType&&/^\s*$/.test(i.textContent)?i=this.getPreviousNode(i,n):n&&i&&1===i.nodeType&&!e.lang.array(["BR","HR","IMG"]).contains(i.nodeName)&&/^[\s]*$/.test(i.innerHTML)?i=this.getPreviousNode(i,n):i||t===this.contain||(r=t.parentNode,r!==this.contain&&(i=this.getPreviousNode(r,n))),i!==this.contain?i:!1)},getSelectionParentsByTag:function(t){for(var n,o=this.getSelectedOwnNodes(),r=[],i=0,a=o.length;a>i;i++)n=o[i].nodeName&&"LI"===o[i].nodeName?o[i]:e.dom.getParentElement(o[i],{nodeName:["LI"]},!1,this.contain),n&&r.push(n);return r.length?r:null},getRangeToNodeEnd:function(){if(this.isCollapsed()){var e=this.getRange(),t=e.startContainer,n=e.startOffset,o=rangy.createRange(this.doc);return o.selectNodeContents(t),o.setStart(t,n),o}},caretIsLastInSelection:function(){var e=(rangy.createRange(this.doc),this.getSelection(),this.getRangeToNodeEnd().cloneContents()),t=e.textContent;return/^\s*$/.test(t)},caretIsFirstInSelection:function(){var t=rangy.createRange(this.doc),n=this.getSelection(),o=this.getRange(),r=o.startContainer;return r.nodeType===e.TEXT_NODE?this.isCollapsed()&&r.nodeType===e.TEXT_NODE&&/^\s*$/.test(r.data.substr(0,o.startOffset)):(t.selectNodeContents(this.getRange().commonAncestorContainer),t.collapse(!0),this.isCollapsed()&&(t.startContainer===n.anchorNode||t.endContainer===n.anchorNode)&&t.startOffset===n.anchorOffset)},caretIsInTheBeginnig:function(t){var n=this.getSelection(),o=n.anchorNode,r=n.anchorOffset;return t?0===r&&(o.nodeName&&o.nodeName===t.toUpperCase()||e.dom.getParentElement(o.parentNode,{nodeName:t},1)):0===r&&!this.getPreviousNode(o,!0)},caretIsBeforeUneditable:function(){var e=this.getSelection(),t=e.anchorNode,n=e.anchorOffset;if(0===n){var o=this.getPreviousNode(t,!0);if(o)for(var r=this.getOwnUneditables(),i=0,a=r.length;a>i;i++)if(o===r[i])return r[i]}return!1},executeAndRestoreRangy:function(e,t){var n=this.doc.defaultView||this.doc.parentWindow,o=rangy.saveSelection(n);if(o)try{e()}catch(r){setTimeout(function(){throw r},0)}else e();rangy.restoreSelection(o)},executeAndRestore:function(t,n){var o,i,a,s,l,c,d,u,h=this.doc.body,f=n&&h.scrollTop,m=n&&h.scrollLeft,p="_wysihtml5-temp-placeholder",g='<span class="'+p+'">'+e.INVISIBLE_SPACE+"</span>",y=this.getRange(!0);if(!y)return void t(h,h);y.collapsed||(d=y.cloneRange(),c=d.createContextualFragment(g),d.collapse(!1),d.insertNode(c),d.detach()),l=y.createContextualFragment(g),y.insertNode(l),c&&(o=this.contain.querySelectorAll("."+p),y.setStartBefore(o[0]),y.setEndAfter(o[o.length-1])),this.setSelection(y);try{t(y.startContainer,y.endContainer)}catch(v){setTimeout(function(){throw v},0)}if(o=this.contain.querySelectorAll("."+p),o&&o.length){u=rangy.createRange(this.doc),a=o[0].nextSibling,o.length>1&&(s=o[o.length-1].previousSibling),s&&a?(u.setStartBefore(a),u.setEndAfter(s)):(i=this.doc.createTextNode(e.INVISIBLE_SPACE),r.insert(i).after(o[0]),u.setStartBefore(i),u.setEndAfter(i)),this.setSelection(u);for(var w=o.length;w--;)o[w].parentNode.removeChild(o[w])}else this.contain.focus();n&&(h.scrollTop=f,h.scrollLeft=m);try{o.parentNode.removeChild(o)}catch(b){}},set:function(e,t){var n=rangy.createRange(this.doc);n.setStart(e,t||0),this.setSelection(n)},insertHTML:function(e){var t=rangy.createRange(this.doc),n=t.createContextualFragment(e),o=n.lastChild;this.insertNode(n),o&&this.setAfter(o)},insertNode:function(e){var t=this.getRange();t&&t.insertNode(e)},surround:function(e){var t,n=this.getOwnRanges(),o=[];if(0==n.length)return o;for(var r=n.length;r--;){t=this.doc.createElement(e.nodeName),o.push(t),e.className&&(t.className=e.className),e.cssStyle&&t.setAttribute("style",e.cssStyle);try{n[r].surroundContents(t),this.selectNode(t)}catch(i){t.appendChild(n[r].extractContents()),n[r].insertNode(t)}}return o},deblockAndSurround:function(t){var n,o,r,i=this.doc.createElement("div"),a=rangy.createRange(this.doc);if(i.className=t.className,this.composer.commands.exec("formatBlock",t.nodeName,t.className),n=this.contain.querySelectorAll("."+t.className),n[0])for(n[0].parentNode.insertBefore(i,n[0]),a.setStartBefore(n[0]),a.setEndAfter(n[n.length-1]),o=a.extractContents();o.firstChild;)if(r=o.firstChild,1==r.nodeType&&e.dom.hasClass(r,t.className)){for(;r.firstChild;)i.appendChild(r.firstChild);"BR"!==r.nodeName&&i.appendChild(this.doc.createElement("br")),o.removeChild(r)}else i.appendChild(r);else i=null;return i},scrollIntoView:function(){var n,o=this.doc,r=5,i=o.documentElement.scrollHeight>o.documentElement.offsetHeight,a=o._wysihtml5ScrollIntoViewElement=o._wysihtml5ScrollIntoViewElement||function(){var t=o.createElement("span");return t.innerHTML=e.INVISIBLE_SPACE,t}();i&&(this.insertNode(a),n=t(a),a.parentNode.removeChild(a),n>=o.body.scrollTop+o.documentElement.offsetHeight-r&&(o.body.scrollTop=n))},selectLine:function(){e.browser.supportsSelectionModify()?this._selectLine_W3C():this.doc.selection&&this._selectLine_MSIE()},_selectLine_W3C:function(){var e=this.doc.defaultView,t=e.getSelection();t.modify("move","left","lineboundary"),t.modify("extend","right","lineboundary")},_selectLine_MSIE:function(){var e,t,n,o,r,i=this.doc.selection.createRange(),a=i.boundingTop,s=this.doc.body.scrollWidth;if(i.moveToPoint){for(0===a&&(n=this.doc.createElement("span"),this.insertNode(n),a=n.offsetTop,n.parentNode.removeChild(n)),a+=1,o=-10;s>o;o+=2)try{i.moveToPoint(o,a);break}catch(l){}for(e=a,t=this.doc.selection.createRange(),r=s;r>=0;r--)try{t.moveToPoint(r,e);break}catch(c){}i.setEndPoint("EndToEnd",t),i.select()}},getText:function(){var e=this.getSelection();return e?e.toString():""},getNodes:function(e,t){var n=this.getRange();return n?n.getNodes([e],t):[]},fixRangeOverflow:function(e){if(this.contain&&this.contain.firstChild&&e){var t=e.compareNode(this.contain);if(2!==t)1===t&&e.setStartBefore(this.contain.firstChild),0===t&&e.setEndAfter(this.contain.lastChild),3===t&&(e.setStartBefore(this.contain.firstChild),e.setEndAfter(this.contain.lastChild));else if(this._detectInlineRangeProblems(e)){var n=e.endContainer.previousElementSibling;n&&e.setEnd(n,this._endOffsetForNode(n))}}},_endOffsetForNode:function(e){var t=document.createRange();return t.selectNodeContents(e),t.endOffset},_detectInlineRangeProblems:function(e){var t=r.compareDocumentPosition(e.startContainer,e.endContainer);return 0==e.endOffset&&4&t},getRange:function(e){var t=this.getSelection(),n=t&&t.rangeCount&&t.getRangeAt(0);return e!==!0&&this.fixRangeOverflow(n),n},getOwnUneditables:function(){var t=r.query(this.contain,"."+this.unselectableClass),n=r.query(t,"."+this.unselectableClass);return e.lang.array(t).without(n)},getOwnRanges:function(){var e,t=[],n=this.getRange();if(n&&t.push(n),this.unselectableClass&&this.contain&&n){var o,r=this.getOwnUneditables();if(r.length>0)for(var i=0,a=r.length;a>i;i++){e=[];for(var s=0,l=t.length;l>s;s++){if(t[s])switch(t[s].compareNode(r[i])){case 2:break;case 3:o=t[s].cloneRange(),o.setEndBefore(r[i]),e.push(o),o=t[s].cloneRange(),o.setStartAfter(r[i]),e.push(o);break;default:e.push(t[s])}t=e}}}return t},getSelection:function(){return rangy.getSelection(this.doc.defaultView||this.doc.parentWindow)},setSelection:function(e){var t=this.doc.defaultView||this.doc.parentWindow,n=rangy.getSelection(t);return n.setSingleRange(e)},createRange:function(){return rangy.createRange(this.doc)},isCollapsed:function(){return this.getSelection().isCollapsed},isEndToEndInNode:function(t){var n=this.getRange(),o=n.commonAncestorContainer,r=n.startContainer,i=n.endContainer;if(o.nodeType===e.TEXT_NODE&&(o=o.parentNode),r.nodeType===e.TEXT_NODE&&!/^\s*$/.test(r.data.substr(n.startOffset)))return!1;if(i.nodeType===e.TEXT_NODE&&!/^\s*$/.test(i.data.substr(n.endOffset)))return!1;for(;r&&r!==o;){if(r.nodeType!==e.TEXT_NODE&&!e.dom.contains(o,r))return!1;if(e.dom.domNode(r).prev({ignoreBlankTexts:!0}))return!1;r=r.parentNode}for(;i&&i!==o;){if(i.nodeType!==e.TEXT_NODE&&!e.dom.contains(o,i))return!1;if(e.dom.domNode(i).next({ignoreBlankTexts:!0}))return!1;i=i.parentNode}return e.lang.array(t).contains(o.nodeName)?o:!1},deselect:function(){var e=this.getSelection();e&&e.removeAllRanges()}})}(wysihtml5),function(e,t){function n(e,t,n){if(!e.className)return!1;var o=e.className.match(n)||[];return o[o.length-1]===t}function o(e,t){if(!e.getAttribute||!e.getAttribute("style"))return!1;e.getAttribute("style").match(t);return e.getAttribute("style").match(t)?!0:!1}function r(e,t,n){e.getAttribute("style")?(s(e,n),e.getAttribute("style")&&!/^\s*$/.test(e.getAttribute("style"))?e.setAttribute("style",t+";"+e.getAttribute("style")):e.setAttribute("style",t)):e.setAttribute("style",t)}function i(e,t,n){e.className?(a(e,n),e.className+=" "+t):e.className=t}function a(e,t){e.className&&(e.className=e.className.replace(t,""))}function s(e,t){var n,o=[];if(e.getAttribute("style")){n=e.getAttribute("style").split(";");for(var r=n.length;r--;)n[r].match(t)||/^\s*$/.test(n[r])||o.push(n[r]);o.length?e.setAttribute("style",o.join(";")):e.removeAttribute("style")}}function l(e,t){var n=[],o=t.split(";"),r=e.getAttribute("style");if(r){r=r.replace(/\s/gi,"").toLowerCase(),n.push(new RegExp("(^|\\s|;)"+t.replace(/\s/gi,"").replace(/([\(\)])/gi,"\\$1").toLowerCase().replace(";",";?").replace(/rgb\\\((\d+),(\d+),(\d+)\\\)/gi,"\\s?rgb\\($1,\\s?$2,\\s?$3\\)"),"gi"));for(var i=o.length;i-->0;)/^\s*$/.test(o[i])||n.push(new RegExp("(^|\\s|;)"+o[i].replace(/\s/gi,"").replace(/([\(\)])/gi,"\\$1").toLowerCase().replace(";",";?").replace(/rgb\\\((\d+),(\d+),(\d+)\\\)/gi,"\\s?rgb\\($1,\\s?$2,\\s?$3\\)"),"gi"));for(var a=0,s=n.length;s>a;a++)if(r.match(n[a]))return n[a]}return!1}function c(n,o,r,i){return r?l(n,r):i?e.dom.hasClass(n,i):t.dom.arrayContains(o,n.tagName.toLowerCase())}function d(e,t,n,o){for(var r=e.length;r--;)if(!c(e[r],t,n,o))return!1;return e.length?!0:!1}function u(e,t,n){var o=l(e,t);return o?(s(e,o),"remove"):(r(e,t,n),"change")}function h(e,t){return e.className.replace(b," ")==t.className.replace(b," ")}function f(e){for(var t=e.parentNode;e.firstChild;)t.insertBefore(e.firstChild,e);t.removeChild(e)}function m(e,t){if(e.attributes.length!=t.attributes.length)return!1;for(var n,o,r,i=0,a=e.attributes.length;a>i;++i)if(n=e.attributes[i],r=n.name,"class"!=r){if(o=t.attributes.getNamedItem(r),n.specified!=o.specified)return!1;if(n.specified&&n.nodeValue!==o.nodeValue)return!1}return!0}function p(e,n){return t.dom.isCharacterDataNode(e)?0==n?!!e.previousSibling:n==e.length?!!e.nextSibling:!0:n>0&&n<e.childNodes.length}function g(e,n,o,r){var i;if(t.dom.isCharacterDataNode(n)&&(0==o?(o=t.dom.getNodeIndex(n),n=n.parentNode):o==n.length?(o=t.dom.getNodeIndex(n)+1,n=n.parentNode):i=t.dom.splitDataNode(n,o)),!(i||r&&n===r)){i=n.cloneNode(!1),i.id&&i.removeAttribute("id");for(var a;a=n.childNodes[o];)i.appendChild(a);t.dom.insertAfter(i,n)}return n==e?i:g(e,i.parentNode,t.dom.getNodeIndex(i),r)}function y(t){this.isElementMerge=t.nodeType==e.ELEMENT_NODE,this.firstTextNode=this.isElementMerge?t.lastChild:t,this.textNodes=[this.firstTextNode]}function v(e,t,n,o,r,i,a){this.tagNames=e||[w],this.cssClass=t||(t===!1?!1:""),this.similarClassRegExp=n,this.cssStyle=r||"",this.similarStyleRegExp=i,this.normalize=o,this.applyToAnyTagName=!1,this.container=a}var w="span",b=/\s+/g;y.prototype={doMerge:function(){for(var e,t,n,o=[],r=0,i=this.textNodes.length;i>r;++r)e=this.textNodes[r],t=e.parentNode,o[r]=e.data,r&&(t.removeChild(e),t.hasChildNodes()||t.parentNode.removeChild(t));return this.firstTextNode.data=n=o.join(""),n},getLength:function(){for(var e=this.textNodes.length,t=0;e--;)t+=this.textNodes[e].length;return t},toString:function(){for(var e=[],t=0,n=this.textNodes.length;n>t;++t)e[t]="'"+this.textNodes[t].data+"'";return"[Merge("+e.join(",")+")]"}},v.prototype={getAncestorWithClass:function(o){for(var r;o;){if(r=this.cssClass?n(o,this.cssClass,this.similarClassRegExp):""!==this.cssStyle?!1:!0,o.nodeType==e.ELEMENT_NODE&&"false"!=o.getAttribute("contenteditable")&&t.dom.arrayContains(this.tagNames,o.tagName.toLowerCase())&&r)return o;o=o.parentNode}return!1},getAncestorWithStyle:function(n){for(var r;n;){if(r=this.cssStyle?o(n,this.similarStyleRegExp):!1,n.nodeType==e.ELEMENT_NODE&&"false"!=n.getAttribute("contenteditable")&&t.dom.arrayContains(this.tagNames,n.tagName.toLowerCase())&&r)return n;n=n.parentNode}return!1},getMatchingAncestor:function(e){var t=this.getAncestorWithClass(e),n=!1;return t?this.cssStyle&&(n="class"):(t=this.getAncestorWithStyle(e),t&&(n="style")),{element:t,type:n}},postApply:function(e,t){for(var n,o,r,i=e[0],a=e[e.length-1],s=[],l=i,c=a,d=0,u=a.length,h=0,f=e.length;f>h;++h)o=e[h],r=null,o&&o.parentNode&&(r=this.getAdjacentMergeableTextNode(o.parentNode,!1)),r?(n||(n=new y(r),s.push(n)),n.textNodes.push(o),o===i&&(l=n.firstTextNode,d=l.length),o===a&&(c=n.firstTextNode,u=n.getLength())):n=null;if(a&&a.parentNode){var m=this.getAdjacentMergeableTextNode(a.parentNode,!0);m&&(n||(n=new y(a),s.push(n)),n.textNodes.push(m))}if(s.length){for(h=0,f=s.length;f>h;++h)s[h].doMerge();t.setStart(l,d),t.setEnd(c,u)}},getAdjacentMergeableTextNode:function(t,n){var o,r=t.nodeType==e.TEXT_NODE,i=r?t.parentNode:t,a=n?"nextSibling":"previousSibling";if(r){if(o=t[a],o&&o.nodeType==e.TEXT_NODE)return o}else if(o=i[a],o&&this.areElementsMergeable(t,o))return o[n?"firstChild":"lastChild"];return null},areElementsMergeable:function(e,n){return t.dom.arrayContains(this.tagNames,(e.tagName||"").toLowerCase())&&t.dom.arrayContains(this.tagNames,(n.tagName||"").toLowerCase())&&h(e,n)&&m(e,n)},createContainer:function(e){var t=e.createElement(this.tagNames[0]);return this.cssClass&&(t.className=this.cssClass),this.cssStyle&&t.setAttribute("style",this.cssStyle),t},applyToTextNode:function(e){var n=e.parentNode;if(1==n.childNodes.length&&t.dom.arrayContains(this.tagNames,n.tagName.toLowerCase()))this.cssClass&&i(n,this.cssClass,this.similarClassRegExp),this.cssStyle&&r(n,this.cssStyle,this.similarStyleRegExp);else{var o=this.createContainer(t.dom.getDocument(e));e.parentNode.insertBefore(o,e),o.appendChild(e)}},isRemovable:function(n){return t.dom.arrayContains(this.tagNames,n.tagName.toLowerCase())&&""===e.lang.string(n.className).trim()&&(!n.getAttribute("style")||""===e.lang.string(n.getAttribute("style")).trim())},undoToTextNode:function(e,t,n,o){var r=n?!1:!0,i=n||o,s=!1;if(!t.containsNode(i)){var l=t.cloneRange();l.selectNode(i),l.isPointInRange(t.endContainer,t.endOffset)&&p(t.endContainer,t.endOffset)&&(g(i,t.endContainer,t.endOffset,this.container),t.setEndAfter(i)),l.isPointInRange(t.startContainer,t.startOffset)&&p(t.startContainer,t.startOffset)&&(i=g(i,t.startContainer,t.startOffset,this.container))}!r&&this.similarClassRegExp&&a(i,this.similarClassRegExp),r&&this.similarStyleRegExp&&(s="change"===u(i,this.cssStyle,this.similarStyleRegExp)),this.isRemovable(i)&&!s&&f(i)},applyToRange:function(t){for(var n,o=t.length;o--;){if(n=t[o].getNodes([e.TEXT_NODE]),!n.length)try{var r=this.createContainer(t[o].endContainer.ownerDocument);return t[o].surroundContents(r),void this.selectNode(t[o],r)}catch(i){}if(t[o].splitBoundaries(),n=t[o].getNodes([e.TEXT_NODE]),n.length){for(var a,s=0,l=n.length;l>s;++s)a=n[s],this.getMatchingAncestor(a).element||this.applyToTextNode(a);t[o].setStart(n[0],0),a=n[n.length-1],t[o].setEnd(a,a.length),this.normalize&&this.postApply(n,t[o])}}},undoToRange:function(t){for(var n,o,r,i=t.length;i--;){if(n=t[i].getNodes([e.TEXT_NODE]),n.length)t[i].splitBoundaries(),n=t[i].getNodes([e.TEXT_NODE]);else{var a=t[i].endContainer.ownerDocument,s=a.createTextNode(e.INVISIBLE_SPACE);t[i].insertNode(s),t[i].selectNode(s),n=[s]}for(var l=0,c=n.length;c>l;++l)t[i].isValid()&&(o=n[l],r=this.getMatchingAncestor(o),"style"===r.type?this.undoToTextNode(o,t[i],!1,r.element):r.element&&this.undoToTextNode(o,t[i],r.element));1==c?this.selectNode(t[i],n[0]):(t[i].setStart(n[0],0),o=n[n.length-1],t[i].setEnd(o,o.length),this.normalize&&this.postApply(n,t[i]))}},selectNode:function(t,n){var o=n.nodeType===e.ELEMENT_NODE,r="canHaveHTML"in n?n.canHaveHTML:!0,i=o?n.innerHTML:n.data,a=""===i||i===e.INVISIBLE_SPACE;if(a&&o&&r)try{n.innerHTML=e.INVISIBLE_SPACE}catch(s){}t.selectNodeContents(n),a&&o?t.collapse(!1):a&&(t.setStartAfter(n),t.setEndAfter(n))},getTextSelectedByRange:function(e,t){var n=t.cloneRange();n.selectNodeContents(e);var o=n.intersection(t),r=o?o.toString():"";return n.detach(),r},isAppliedToRange:function(t){for(var n,o,r=[],i="full",a=t.length;a--;){if(o=t[a].getNodes([e.TEXT_NODE]),!o.length)return n=this.getMatchingAncestor(t[a].startContainer).element,n?{elements:[n],coverage:i}:!1;for(var s,l=0,c=o.length;c>l;++l)s=this.getTextSelectedByRange(o[l],t[a]),n=this.getMatchingAncestor(o[l]).element,n&&""!=s?(r.push(n),1===e.dom.getTextNodes(n,!0).length?i="full":"full"===i&&(i="inline")):n||(i="partial")}return r.length?{elements:r,coverage:i}:!1},toggleRange:function(e){var t,n=this.isAppliedToRange(e);n?"full"===n.coverage?this.undoToRange(e):"inline"===n.coverage?(t=d(n.elements,this.tagNames,this.cssStyle,this.cssClass),this.undoToRange(e),t||this.applyToRange(e)):(d(n.elements,this.tagNames,this.cssStyle,this.cssClass)||this.undoToRange(e),this.applyToRange(e)):this.applyToRange(e)}},e.selection.HTMLApplier=v}(wysihtml5,rangy),wysihtml5.Commands=Base.extend({constructor:function(e){this.editor=e,this.composer=e.composer,this.doc=this.composer.doc},support:function(e){return wysihtml5.browser.supportsCommand(this.doc,e)},exec:function(e,t){var n=wysihtml5.commands[e],o=wysihtml5.lang.array(arguments).get(),r=n&&n.exec,i=null;if(this.editor.fire("beforecommand:composer"),r)o.unshift(this.composer),i=r.apply(n,o);else try{i=this.doc.execCommand(e,!1,t)}catch(a){}return this.editor.fire("aftercommand:composer"),i},state:function(e,t){var n=wysihtml5.commands[e],o=wysihtml5.lang.array(arguments).get(),r=n&&n.state;if(r)return o.unshift(this.composer),r.apply(n,o);try{return this.doc.queryCommandState(e)}catch(i){return!1}},stateValue:function(e){var t=wysihtml5.commands[e],n=wysihtml5.lang.array(arguments).get(),o=t&&t.stateValue;return o?(n.unshift(this.composer),o.apply(t,n)):!1}}),wysihtml5.commands.bold={exec:function(e,t){wysihtml5.commands.formatInline.execWithToggle(e,t,"b")},state:function(e,t){return wysihtml5.commands.formatInline.state(e,t,"b")}},function(e){function t(t,n){var a,s,l,c,d,u,h,f,m,p=t.doc,g="_wysihtml5-temp-"+ +new Date,y=/non-matching-class/g,v=0;for(e.commands.formatInline.exec(t,o,r,g,y,o,o,!0,!0),s=p.querySelectorAll(r+"."+g),a=s.length;a>v;v++){l=s[v],l.removeAttribute("class");for(m in n)"text"!==m&&l.setAttribute(m,n[m])}u=l,1===a&&(h=i.getTextContent(l),c=!!l.querySelector("*"),d=""===h||h===e.INVISIBLE_SPACE,!c&&d&&(i.setTextContent(l,n.text||l.href),f=p.createTextNode(" "),t.selection.setAfter(l),i.insert(f).after(l),u=f)),t.selection.setAfter(u)}function n(e,t,n){for(var o,r=t.length;r--;){o=t[r].attributes;for(var i=o.length;i--;)t[r].removeAttribute(o.item(i).name);for(var a in n)n.hasOwnProperty(a)&&t[r].setAttribute(a,n[a])}}var o,r="A",i=e.dom;e.commands.createLink={exec:function(e,o,r){var i=this.state(e,o);i?e.selection.executeAndRestore(function(){n(e,i,r)}):(r="object"==typeof r?r:{href:r},t(e,r))},state:function(t,n){return e.commands.formatInline.state(t,n,"A")}}}(wysihtml5),function(e){function t(e,t){for(var o,r,i,a=t.length,s=0;a>s;s++)o=t[s],r=n.getParentElement(o,{nodeName:"code"}),i=n.getTextContent(o),i.match(n.autoLink.URL_REG_EXP)&&!r?r=n.renameElement(o,"code"):n.replaceWithChildNodes(o)}var n=e.dom;e.commands.removeLink={exec:function(e,n){var o=this.state(e,n);o&&e.selection.executeAndRestore(function(){t(e,o)})},state:function(t,n){return e.commands.formatInline.state(t,n,"A")}}}(wysihtml5),function(e){var t=/wysiwyg-font-size-[0-9a-z\-]+/g;e.commands.fontSize={exec:function(n,o,r){e.commands.formatInline.execWithToggle(n,o,"span","wysiwyg-font-size-"+r,t)},state:function(n,o,r){return e.commands.formatInline.state(n,o,"span","wysiwyg-font-size-"+r,t)}}}(wysihtml5),function(e){var t=/(\s|^)font-size\s*:\s*[^;\s]+;?/gi;e.commands.fontSizeStyle={exec:function(n,o,r){r="object"==typeof r?r.size:r,/^\s*$/.test(r)||e.commands.formatInline.execWithToggle(n,o,"span",!1,!1,"font-size:"+r,t)},state:function(n,o,r){return e.commands.formatInline.state(n,o,"span",!1,!1,"font-size",t)},stateValue:function(t,n){var o,r=this.state(t,n);return r&&e.lang.object(r).isArray()&&(r=r[0]),r&&(o=r.getAttribute("style"))?e.quirks.styleParser.parseFontSize(o):!1}}}(wysihtml5),function(e){var t=/wysiwyg-color-[0-9a-z]+/g;e.commands.foreColor={exec:function(n,o,r){e.commands.formatInline.execWithToggle(n,o,"span","wysiwyg-color-"+r,t)},state:function(n,o,r){return e.commands.formatInline.state(n,o,"span","wysiwyg-color-"+r,t)}}}(wysihtml5),function(e){var t=/(\s|^)color\s*:\s*[^;\s]+;?/gi;e.commands.foreColorStyle={exec:function(n,o,r){var i,a=e.quirks.styleParser.parseColor("object"==typeof r?"color:"+r.color:"color:"+r,"color");a&&(i="color: rgb("+a[0]+","+a[1]+","+a[2]+");",1!==a[3]&&(i+="color: rgba("+a[0]+","+a[1]+","+a[2]+","+a[3]+");"),e.commands.formatInline.execWithToggle(n,o,"span",!1,!1,i,t))},state:function(n,o){return e.commands.formatInline.state(n,o,"span",!1,!1,"color",t)},stateValue:function(t,n,o){var r,i=this.state(t,n);return i&&e.lang.object(i).isArray()&&(i=i[0]),i&&(r=i.getAttribute("style"),r&&r)?(val=e.quirks.styleParser.parseColor(r,"color"),e.quirks.styleParser.unparseColor(val,o)):!1}}}(wysihtml5),function(e){var t=/(\s|^)background-color\s*:\s*[^;\s]+;?/gi;e.commands.bgColorStyle={exec:function(n,o,r){var i,a=e.quirks.styleParser.parseColor("object"==typeof r?"background-color:"+r.color:"background-color:"+r,"background-color");a&&(i="background-color: rgb("+a[0]+","+a[1]+","+a[2]+");",1!==a[3]&&(i+="background-color: rgba("+a[0]+","+a[1]+","+a[2]+","+a[3]+");"),e.commands.formatInline.execWithToggle(n,o,"span",!1,!1,i,t))},state:function(n,o){return e.commands.formatInline.state(n,o,"span",!1,!1,"background-color",t)},stateValue:function(t,n,o){var r,i=this.state(t,n),a=!1;return i&&e.lang.object(i).isArray()&&(i=i[0]),i&&(r=i.getAttribute("style"))?(a=e.quirks.styleParser.parseColor(r,"background-color"),e.quirks.styleParser.unparseColor(a,o)):!1}}}(wysihtml5),function(e){function t(t,n,r){t.className?(o(t,r),t.className=e.lang.string(t.className+" "+n).trim()):t.className=n}function n(t,n,o){r(t,o),t.getAttribute("style")?t.setAttribute("style",e.lang.string(t.getAttribute("style")+" "+n).trim()):t.setAttribute("style",n)}function o(t,n){var o=n.test(t.className);return t.className=t.className.replace(n,""),""==e.lang.string(t.className).trim()&&t.removeAttribute("class"),o}function r(t,n){var o=n.test(t.getAttribute("style"));return t.setAttribute("style",(t.getAttribute("style")||"").replace(n,"")),""==e.lang.string(t.getAttribute("style")||"").trim()&&t.removeAttribute("style"),o}function i(e){var t=e.lastChild;t&&a(t)&&t.parentNode.removeChild(t)}function a(e){return"BR"===e.nodeName}function s(t,n){t.selection.isCollapsed()&&t.selection.selectLine();for(var o=t.selection.surround(n),r=0,a=o.length;a>r;r++)e.dom.lineBreaks(o[r]).remove(),i(o[r])}function l(t){return!!e.lang.string(t.className).trim()}function c(t){return!!e.lang.string(t.getAttribute("style")||"").trim()}var d=e.dom,u=["H1","H2","H3","H4","H5","H6","P","PRE","DIV"];e.commands.formatBlock={exec:function(i,a,h,f,m,p,g){var y,v,w,b,C,N=(i.doc,this.state(i,a,h,f,m,p,g)),x=i.config.useLineBreaks,E=x?"DIV":"P";return h="string"==typeof h?h.toUpperCase():h,N.length?void i.selection.executeAndRestoreRangy(function(){for(var t=N.length;t--;){if(m&&(v=o(N[t],m)),g&&(b=r(N[t],g)),(b||v)&&null===h&&N[t].nodeName!=E)return;var n=l(N[t]),i=c(N[t]);n||i||!x&&"P"!==h?d.renameElement(N[t],"P"===h?"DIV":E):(e.dom.lineBreaks(N[t]).add(),d.replaceWithChildNodes(N[t]))}}):void((null!==h&&!e.lang.array(u).contains(h)||(y=i.selection.findNodesInSelection(u).concat(i.selection.getSelectedOwnNodes()),i.selection.executeAndRestoreRangy(function(){for(var e=y.length;e--;)C=d.getParentElement(y[e],{nodeName:u}),C==i.element&&(C=null),C&&(h&&(C=d.renameElement(C,h)),f&&t(C,f,m),p&&n(C,p,g),w=!0)}),!w))&&s(i,{nodeName:h||E,className:f||null,cssStyle:p||null}))},state:function(t,n,o,r,i,a,s){var l,c=t.selection.getSelectedOwnNodes(),u=[];o="string"==typeof o?o.toUpperCase():o;for(var h=0,f=c.length;f>h;h++)l=d.getParentElement(c[h],{nodeName:o,className:r,classRegExp:i,cssStyle:a,styleRegExp:s}),l&&-1==e.lang.array(u).indexOf(l)&&u.push(l);return 0==u.length?!1:u}}}(wysihtml5),wysihtml5.commands.formatCode={exec:function(e,t,n){
var o,r,i,a=this.state(e);a?e.selection.executeAndRestore(function(){o=a.querySelector("code"),wysihtml5.dom.replaceWithChildNodes(a),o&&wysihtml5.dom.replaceWithChildNodes(o)}):(r=e.selection.getRange(),i=r.extractContents(),a=e.doc.createElement("pre"),o=e.doc.createElement("code"),n&&(o.className=n),a.appendChild(o),o.appendChild(i),r.insertNode(a),e.selection.selectNode(a))},state:function(e){var t=e.selection.getSelectedNode();return t&&t.nodeName&&"PRE"==t.nodeName&&t.firstChild&&t.firstChild.nodeName&&"CODE"==t.firstChild.nodeName?t:wysihtml5.dom.getParentElement(t,{nodeName:"CODE"})&&wysihtml5.dom.getParentElement(t,{nodeName:"PRE"})}},function(e){function t(e){var t=o[e];return t?[e.toLowerCase(),t.toLowerCase()]:[e.toLowerCase()]}function n(n,o,i,a,s,l){var c=n;return o&&(c+=":"+o),a&&(c+=":"+a),r[c]||(r[c]=new e.selection.HTMLApplier(t(n),o,i,!0,a,s,l)),r[c]}var o={strong:"b",em:"i",b:"strong",i:"em"},r={};e.commands.formatInline={exec:function(e,t,o,r,i,a,s,l,c){var d=e.selection.createRange(),u=e.selection.getOwnRanges();return u&&0!=u.length?(e.selection.getSelection().removeAllRanges(),n(o,r,i,a,s,e.element).toggleRange(u),void(l?c||e.cleanUp():(d.setStart(u[0].startContainer,u[0].startOffset),d.setEnd(u[u.length-1].endContainer,u[u.length-1].endOffset),e.selection.setSelection(d),e.selection.executeAndRestore(function(){c||e.cleanUp()},!0,!0)))):!1},execWithToggle:function(t,n,o,r,i,a,s){var l=this;if(this.state(t,n,o,r,i,a,s)&&t.selection.isCollapsed()&&!t.selection.caretIsLastInSelection()&&!t.selection.caretIsFirstInSelection()){var c=l.state(t,n,o,r,i)[0];t.selection.executeAndRestoreRangy(function(){c.parentNode;t.selection.selectNode(c,!0),e.commands.formatInline.exec(t,n,o,r,i,a,s,!0,!0)})}else this.state(t,n,o,r,i,a,s)&&!t.selection.isCollapsed()?t.selection.executeAndRestoreRangy(function(){e.commands.formatInline.exec(t,n,o,r,i,a,s,!0,!0)}):e.commands.formatInline.exec(t,n,o,r,i,a,s)},state:function(t,r,i,a,s,l,c){var d,u,h=t.doc,f=o[i]||i;return e.dom.hasElementWithTagName(h,i)||e.dom.hasElementWithTagName(h,f)?a&&!e.dom.hasElementWithClassName(h,a)?!1:(d=t.selection.getOwnRanges(),d&&0!==d.length?(u=n(i,a,s,l,c,t.element).isAppliedToRange(d),u&&u.elements?u.elements:!1):!1):!1}}}(wysihtml5),function(e){e.commands.insertBlockQuote={exec:function(t,n){var o=this.state(t,n),r=t.selection.isEndToEndInNode(["H1","H2","H3","H4","H5","H6","P"]);t.selection.executeAndRestore(function(){if(o)t.config.useLineBreaks&&e.dom.lineBreaks(o).add(),e.dom.unwrap(o);else if(t.selection.isCollapsed()&&t.selection.selectLine(),r){var n=r.ownerDocument.createElement("blockquote");e.dom.insert(n).after(r),n.appendChild(r)}else t.selection.surround({nodeName:"blockquote"})})},state:function(t,n){var o=t.selection.getSelectedNode(),r=e.dom.getParentElement(o,{nodeName:"BLOCKQUOTE"},!1,t.element);return r?r:!1}}}(wysihtml5),wysihtml5.commands.insertHTML={exec:function(e,t,n){e.commands.support(t)?e.doc.execCommand(t,!1,n):e.selection.insertHTML(n)},state:function(){return!1}},function(e){var t="IMG";e.commands.insertImage={exec:function(n,o,r){r="object"==typeof r?r:{src:r};var i,a,s=n.doc,l=this.state(n);if(l)return n.selection.setBefore(l),a=l.parentNode,a.removeChild(l),e.dom.removeEmptyTextNodes(a),"A"!==a.nodeName||a.firstChild||(n.selection.setAfter(a),a.parentNode.removeChild(a)),void e.quirks.redraw(n.element);l=s.createElement(t);for(var c in r)l.setAttribute("className"===c?"class":c,r[c]);n.selection.insertNode(l),e.browser.hasProblemsSettingCaretAfterImg()?(i=s.createTextNode(e.INVISIBLE_SPACE),n.selection.insertNode(i),n.selection.setAfter(i)):n.selection.setAfter(l)},state:function(n){var o,r,i,a=n.doc;return e.dom.hasElementWithTagName(a,t)&&(o=n.selection.getSelectedNode())?o.nodeName===t?o:o.nodeType!==e.ELEMENT_NODE?!1:(r=n.selection.getText(),(r=e.lang.string(r).trim())?!1:(i=n.selection.getNodes(e.ELEMENT_NODE,function(e){return"IMG"===e.nodeName}),1!==i.length?!1:i[0])):!1}}}(wysihtml5),function(e){var t="<br>"+(e.browser.needsSpaceAfterLineBreak()?" ":"");e.commands.insertLineBreak={exec:function(n,o){n.commands.support(o)?(n.doc.execCommand(o,!1,null),e.browser.autoScrollsToCaret()||n.selection.scrollIntoView()):n.commands.exec("insertHTML",t)},state:function(){return!1}}}(wysihtml5),wysihtml5.commands.insertOrderedList={exec:function(e,t){wysihtml5.commands.insertList.exec(e,t,"OL")},state:function(e,t){return wysihtml5.commands.insertList.state(e,t,"OL")}},wysihtml5.commands.insertUnorderedList={exec:function(e,t){wysihtml5.commands.insertList.exec(e,t,"UL")},state:function(e,t){return wysihtml5.commands.insertList.state(e,t,"UL")}},wysihtml5.commands.insertList=function(e){var t=function(e,t){if(e&&e.nodeName){"string"==typeof t&&(t=[t]);for(var n=t.length;n--;)if(e.nodeName===t[n])return!0}return!1},n=function(n,o,r){var i={el:null,other:!1};if(n){var a=e.dom.getParentElement(n,{nodeName:"LI"}),s="UL"===o?"OL":"UL";t(n,o)?i.el=n:t(n,s)?i={el:n,other:!0}:a&&(t(a.parentNode,o)?i.el=a.parentNode:t(a.parentNode,s)&&(i={el:a.parentNode,other:!0}))}return i.el&&!r.element.contains(i.el)&&(i.el=null),i},o=function(t,n,o){var r,a="UL"===n?"OL":"UL";o.selection.executeAndRestore(function(){var s=i(a,o);if(s.length)for(var l=s.length;l--;)e.dom.renameElement(s[l],n.toLowerCase());else{r=i(["OL","UL"],o);for(var c=r.length;c--;)e.dom.resolveList(r[c],o.config.useLineBreaks);e.dom.resolveList(t,o.config.useLineBreaks)}})},r=function(t,n,o){var r="UL"===n?"OL":"UL";o.selection.executeAndRestore(function(){for(var a=[t].concat(i(r,o)),s=a.length;s--;)e.dom.renameElement(a[s],n.toLowerCase())})},i=function(e,n){for(var o=n.selection.getOwnRanges(),r=[],i=o.length;i--;)r=r.concat(o[i].getNodes([1],function(n){return t(n,e)}));return r},a=function(t,n){n.selection.executeAndRestoreRangy(function(){var o,r,i="_wysihtml5-temp-"+(new Date).getTime(),a=n.selection.deblockAndSurround({nodeName:"div",className:i}),s=/\uFEFF/g;a.innerHTML=a.innerHTML.replace(s,""),a&&(o=e.lang.array(["","<br>",e.INVISIBLE_SPACE]).contains(a.innerHTML),r=e.dom.convertToList(a,t.toLowerCase(),n.parent.config.uneditableContainerClassname),o&&n.selection.selectNode(r.querySelector("li"),!0))})};return{exec:function(e,t,i){var s=e.doc,l="OL"===i?"insertOrderedList":"insertUnorderedList",c=e.selection.getSelectedNode(),d=n(c,i,e);d.el?d.other?r(d.el,i,e):o(d.el,i,e):e.commands.support(l)?s.execCommand(l,!1,null):a(i,e)},state:function(e,t,o){var r=e.selection.getSelectedNode(),i=n(r,o,e);return i.el&&!i.other?i.el:!1}}}(wysihtml5),wysihtml5.commands.italic={exec:function(e,t){wysihtml5.commands.formatInline.execWithToggle(e,t,"i")},state:function(e,t){return wysihtml5.commands.formatInline.state(e,t,"i")}},function(e){var t="wysiwyg-text-align-center",n=/wysiwyg-text-align-[0-9a-z]+/g;e.commands.justifyCenter={exec:function(o,r){return e.commands.formatBlock.exec(o,"formatBlock",null,t,n)},state:function(o,r){return e.commands.formatBlock.state(o,"formatBlock",null,t,n)}}}(wysihtml5),function(e){var t="wysiwyg-text-align-left",n=/wysiwyg-text-align-[0-9a-z]+/g;e.commands.justifyLeft={exec:function(o,r){return e.commands.formatBlock.exec(o,"formatBlock",null,t,n)},state:function(o,r){return e.commands.formatBlock.state(o,"formatBlock",null,t,n)}}}(wysihtml5),function(e){var t="wysiwyg-text-align-right",n=/wysiwyg-text-align-[0-9a-z]+/g;e.commands.justifyRight={exec:function(o,r){return e.commands.formatBlock.exec(o,"formatBlock",null,t,n)},state:function(o,r){return e.commands.formatBlock.state(o,"formatBlock",null,t,n)}}}(wysihtml5),function(e){var t="wysiwyg-text-align-justify",n=/wysiwyg-text-align-[0-9a-z]+/g;e.commands.justifyFull={exec:function(o,r){return e.commands.formatBlock.exec(o,"formatBlock",null,t,n)},state:function(o,r){return e.commands.formatBlock.state(o,"formatBlock",null,t,n)}}}(wysihtml5),function(e){var t="text-align: right;",n=/(\s|^)text-align\s*:\s*[^;\s]+;?/gi;e.commands.alignRightStyle={exec:function(o,r){return e.commands.formatBlock.exec(o,"formatBlock",null,null,null,t,n)},state:function(o,r){return e.commands.formatBlock.state(o,"formatBlock",null,null,null,t,n)}}}(wysihtml5),function(e){var t="text-align: left;",n=/(\s|^)text-align\s*:\s*[^;\s]+;?/gi;e.commands.alignLeftStyle={exec:function(o,r){return e.commands.formatBlock.exec(o,"formatBlock",null,null,null,t,n)},state:function(o,r){return e.commands.formatBlock.state(o,"formatBlock",null,null,null,t,n)}}}(wysihtml5),function(e){var t="text-align: center;",n=/(\s|^)text-align\s*:\s*[^;\s]+;?/gi;e.commands.alignCenterStyle={exec:function(o,r){return e.commands.formatBlock.exec(o,"formatBlock",null,null,null,t,n)},state:function(o,r){return e.commands.formatBlock.state(o,"formatBlock",null,null,null,t,n)}}}(wysihtml5),wysihtml5.commands.redo={exec:function(e){return e.undoManager.redo()},state:function(e){return!1}},wysihtml5.commands.underline={exec:function(e,t){wysihtml5.commands.formatInline.execWithToggle(e,t,"u")},state:function(e,t){return wysihtml5.commands.formatInline.state(e,t,"u")}},wysihtml5.commands.undo={exec:function(e){return e.undoManager.undo()},state:function(e){return!1}},wysihtml5.commands.createTable={exec:function(e,t,n){var o,r,i;if(n&&n.cols&&n.rows&&parseInt(n.cols,10)>0&&parseInt(n.rows,10)>0){for(i=n.tableStyle?'<table style="'+n.tableStyle+'">':"<table>",i+="<tbody>",r=0;r<n.rows;r++){for(i+="<tr>",o=0;o<n.cols;o++)i+="<td>&nbsp;</td>";i+="</tr>"}i+="</tbody></table>",e.commands.exec("insertHTML",i)}},state:function(e,t){return!1}},wysihtml5.commands.mergeTableCells={exec:function(e,t){e.tableSelection&&e.tableSelection.start&&e.tableSelection.end&&(this.state(e,t)?wysihtml5.dom.table.unmergeCell(e.tableSelection.start):wysihtml5.dom.table.mergeCellsBetween(e.tableSelection.start,e.tableSelection.end))},state:function(e,t){if(e.tableSelection){var n=e.tableSelection.start,o=e.tableSelection.end;if(n&&o&&n==o&&(wysihtml5.dom.getAttribute(n,"colspan")&&parseInt(wysihtml5.dom.getAttribute(n,"colspan"),10)>1||wysihtml5.dom.getAttribute(n,"rowspan")&&parseInt(wysihtml5.dom.getAttribute(n,"rowspan"),10)>1))return[n]}return!1}},wysihtml5.commands.addTableCells={exec:function(e,t,n){if(e.tableSelection&&e.tableSelection.start&&e.tableSelection.end){var o=wysihtml5.dom.table.orderSelectionEnds(e.tableSelection.start,e.tableSelection.end);"before"==n||"above"==n?wysihtml5.dom.table.addCells(o.start,n):("after"==n||"below"==n)&&wysihtml5.dom.table.addCells(o.end,n),setTimeout(function(){e.tableSelection.select(o.start,o.end)},0)}},state:function(e,t){return!1}},wysihtml5.commands.deleteTableCells={exec:function(e,t,n){if(e.tableSelection&&e.tableSelection.start&&e.tableSelection.end){var o,r=wysihtml5.dom.table.orderSelectionEnds(e.tableSelection.start,e.tableSelection.end),i=wysihtml5.dom.table.indexOf(r.start),a=e.tableSelection.table;wysihtml5.dom.table.removeCells(r.start,n),setTimeout(function(){o=wysihtml5.dom.table.findCell(a,i),o||("row"==n&&(o=wysihtml5.dom.table.findCell(a,{row:i.row-1,col:i.col})),"column"==n&&(o=wysihtml5.dom.table.findCell(a,{row:i.row,col:i.col-1}))),o&&e.tableSelection.select(o,o)},0)}},state:function(e,t){return!1}},wysihtml5.commands.indentList={exec:function(e,t,n){var o=e.selection.getSelectionParentsByTag("LI");return o?this.tryToPushLiLevel(o,e.selection):!1},state:function(e,t){return!1},tryToPushLiLevel:function(e,t){var n,o,r,i,a,s=!1;return t.executeAndRestoreRangy(function(){for(var t=e.length;t--;)i=e[t],n="OL"===i.parentNode.nodeName?"OL":"UL",o=i.ownerDocument.createElement(n),r=wysihtml5.dom.domNode(i).prev({nodeTypes:[wysihtml5.ELEMENT_NODE]}),a=r?r.querySelector("ul, ol"):null,r&&(a?a.appendChild(i):(o.appendChild(i),r.appendChild(o)),s=!0)}),s}},wysihtml5.commands.outdentList={exec:function(e,t,n){var o=e.selection.getSelectionParentsByTag("LI");return o?this.tryToPullLiLevel(o,e):!1},state:function(e,t){return!1},tryToPullLiLevel:function(e,t){var n,o,r,i,a,s=!1,l=this;return t.selection.executeAndRestoreRangy(function(){for(var c=e.length;c--;)if(i=e[c],i.parentNode&&(n=i.parentNode,"OL"===n.tagName||"UL"===n.tagName)){if(s=!0,o=wysihtml5.dom.getParentElement(n.parentNode,{nodeName:["OL","UL"]},!1,t.element),r=wysihtml5.dom.getParentElement(n.parentNode,{nodeName:["LI"]},!1,t.element),o&&r)i.nextSibling&&(a=l.getAfterList(n,i),i.appendChild(a)),o.insertBefore(i,r.nextSibling);else{i.nextSibling&&(a=l.getAfterList(n,i),i.appendChild(a));for(var d=i.childNodes.length;d--;)n.parentNode.insertBefore(i.childNodes[d],n.nextSibling);n.parentNode.insertBefore(document.createElement("br"),n.nextSibling),i.parentNode.removeChild(i)}0===n.childNodes.length&&n.parentNode.removeChild(n)}}),s},getAfterList:function(e,t){for(var n=e.nodeName,o=document.createElement(n);t.nextSibling;)o.appendChild(t.nextSibling);return o}},function(e){var t=90,n=89,o=8,r=46,i=25,a="data-wysihtml5-selection-node",s="data-wysihtml5-selection-offset",l=('<span id="_wysihtml5-undo" class="_wysihtml5-temp">'+e.INVISIBLE_SPACE+"</span>",'<span id="_wysihtml5-redo" class="_wysihtml5-temp">'+e.INVISIBLE_SPACE+"</span>",e.dom);e.UndoManager=e.lang.Dispatcher.extend({constructor:function(e){this.editor=e,this.composer=e.composer,this.element=this.composer.element,this.position=0,this.historyStr=[],this.historyDom=[],this.transact(),this._observe()},_observe:function(){var e,i=this;this.composer.sandbox.getDocument();l.observe(this.element,"keydown",function(e){if(!e.altKey&&(e.ctrlKey||e.metaKey)){var o=e.keyCode,r=o===t&&!e.shiftKey,a=o===t&&e.shiftKey||o===n;r?(i.undo(),e.preventDefault()):a&&(i.redo(),e.preventDefault())}}),l.observe(this.element,"keydown",function(t){var n=t.keyCode;n!==e&&(e=n,(n===o||n===r)&&i.transact())}),this.editor.on("newword:composer",function(){i.transact()}).on("beforecommand:composer",function(){i.transact()})},transact:function(){var t,n,o,r,l,c=this.historyStr[this.position-1],d=this.composer.getValue(!1,!1),u=this.element.offsetWidth>0&&this.element.offsetHeight>0;if(d!==c){var h=this.historyStr.length=this.historyDom.length=this.position;h>i&&(this.historyStr.shift(),this.historyDom.shift(),this.position--),this.position++,u&&(t=this.composer.selection.getRange(),n=t&&t.startContainer?t.startContainer:this.element,o=t&&t.startOffset?t.startOffset:0,n.nodeType===e.ELEMENT_NODE?r=n:(r=n.parentNode,l=this.getChildNodeIndex(r,n)),r.setAttribute(s,o),"undefined"!=typeof l&&r.setAttribute(a,l));var f=this.element.cloneNode(!!d);this.historyDom.push(f),this.historyStr.push(d),r&&(r.removeAttribute(s),r.removeAttribute(a))}},undo:function(){this.transact(),this.undoPossible()&&(this.set(this.historyDom[--this.position-1]),this.editor.fire("undo:composer"))},redo:function(){this.redoPossible()&&(this.set(this.historyDom[++this.position-1]),this.editor.fire("redo:composer"))},undoPossible:function(){return this.position>1},redoPossible:function(){return this.position<this.historyStr.length},set:function(e){this.element.innerHTML="";for(var t=0,n=e.childNodes,o=e.childNodes.length;o>t;t++)this.element.appendChild(n[t].cloneNode(!0));var r,i,l;e.hasAttribute(s)?(r=e.getAttribute(s),l=e.getAttribute(a),i=this.element):(i=this.element.querySelector("["+s+"]")||this.element,r=i.getAttribute(s),l=i.getAttribute(a),i.removeAttribute(s),i.removeAttribute(a)),null!==l&&(i=this.getChildNodeByIndex(i,+l)),this.composer.selection.set(i,r)},getChildNodeIndex:function(e,t){for(var n=0,o=e.childNodes,r=o.length;r>n;n++)if(o[n]===t)return n},getChildNodeByIndex:function(e,t){return e.childNodes[t]}})}(wysihtml5),wysihtml5.views.View=Base.extend({constructor:function(e,t,n){this.parent=e,this.element=t,this.config=n,this.config.noTextarea||this._observeViewChange()},_observeViewChange:function(){var e=this;this.parent.on("beforeload",function(){e.parent.on("change_view",function(t){t===e.name?(e.parent.currentView=e,e.show(),setTimeout(function(){e.focus()},0)):e.hide()})})},focus:function(){if(this.element.ownerDocument.querySelector(":focus")!==this.element)try{this.element.focus()}catch(e){}},hide:function(){this.element.style.display="none"},show:function(){this.element.style.display=""},disable:function(){this.element.setAttribute("disabled","disabled")},enable:function(){this.element.removeAttribute("disabled")}}),function(e){var t=e.dom,n=e.browser;e.views.Composer=e.views.View.extend({name:"composer",CARET_HACK:"<br>",constructor:function(e,t,n){this.base(e,t,n),this.config.noTextarea?this.editableArea=t:this.textarea=this.parent.textarea,this.config.contentEditableMode?this._initContentEditableArea():this._initSandbox()},clear:function(){this.element.innerHTML=n.displaysCaretInEmptyContentEditableCorrectly()?"":this.CARET_HACK},getValue:function(t,n){var o=this.isEmpty()?"":e.quirks.getCorrectInnerHTML(this.element);return t!==!1&&(o=this.parent.parse(o,n===!1?!1:!0)),o},setValue:function(e,t){t&&(e=this.parent.parse(e));try{this.element.innerHTML=e}catch(n){this.element.innerText=e}},cleanUp:function(){this.parent.parse(this.element)},show:function(){this.editableArea.style.display=this._displayStyle||"",this.config.noTextarea||this.textarea.element.disabled||(this.disable(),this.enable())},hide:function(){this._displayStyle=t.getStyle("display").from(this.editableArea),"none"===this._displayStyle&&(this._displayStyle=null),this.editableArea.style.display="none"},disable:function(){this.parent.fire("disable:composer"),this.element.removeAttribute("contentEditable")},enable:function(){this.parent.fire("enable:composer"),this.element.setAttribute("contentEditable","true")},focus:function(t){e.browser.doesAsyncFocus()&&this.hasPlaceholderSet()&&this.clear(),this.base();var n=this.element.lastChild;t&&n&&this.selection&&("BR"===n.nodeName?this.selection.setBefore(this.element.lastChild):this.selection.setAfter(this.element.lastChild))},getTextContent:function(){return t.getTextContent(this.element)},hasPlaceholderSet:function(){return this.getTextContent()==(this.config.noTextarea?this.editableArea.getAttribute("data-placeholder"):this.textarea.element.getAttribute("placeholder"))&&this.placeholderSet},isEmpty:function(){var e=this.element.innerHTML.toLowerCase();return/^(\s|<br>|<\/br>|<p>|<\/p>)*$/i.test(e)||""===e||"<br>"===e||"<p></p>"===e||"<p><br></p>"===e||this.hasPlaceholderSet()},_initContentEditableArea:function(){var e=this;this.config.noTextarea?this.sandbox=new t.ContentEditableArea(function(){e._create()},{},this.editableArea):(this.sandbox=new t.ContentEditableArea(function(){e._create()}),this.editableArea=this.sandbox.getContentEditable(),t.insert(this.editableArea).after(this.textarea.element),this._createWysiwygFormField())},_initSandbox:function(){var e=this;this.sandbox=new t.Sandbox(function(){e._create()},{stylesheets:this.config.stylesheets}),this.editableArea=this.sandbox.getIframe();var n=this.textarea.element;t.insert(this.editableArea).after(n),this._createWysiwygFormField()},_createWysiwygFormField:function(){if(this.textarea.element.form){var e=document.createElement("input");e.type="hidden",e.name="_wysihtml5_mode",e.value=1,t.insert(e).after(this.textarea.element)}},_create:function(){var o=this;this.doc=this.sandbox.getDocument(),this.element=this.config.contentEditableMode?this.sandbox.getContentEditable():this.doc.body,this.config.noTextarea?this.cleanUp():(this.textarea=this.parent.textarea,this.element.innerHTML=this.textarea.getValue(!0,!1)),this.selection=new e.Selection(this.parent,this.element,this.config.uneditableContainerClassname),this.commands=new e.Commands(this.parent),this.config.noTextarea||t.copyAttributes(["className","spellcheck","title","lang","dir","accessKey"]).from(this.textarea.element).to(this.element),t.addClass(this.element,this.config.composerClassName),this.config.style&&!this.config.contentEditableMode&&this.style(),this.observe();var r=this.config.name;r&&(t.addClass(this.element,r),this.config.contentEditableMode||t.addClass(this.editableArea,r)),this.enable(),!this.config.noTextarea&&this.textarea.element.disabled&&this.disable();var i="string"==typeof this.config.placeholder?this.config.placeholder:this.config.noTextarea?this.editableArea.getAttribute("data-placeholder"):this.textarea.element.getAttribute("placeholder");i&&t.simulatePlaceholder(this.parent,this,i),this.commands.exec("styleWithCSS",!1),this._initAutoLinking(),this._initObjectResizing(),this._initUndoManager(),this._initLineBreaking(),this.config.noTextarea||!this.textarea.element.hasAttribute("autofocus")&&document.querySelector(":focus")!=this.textarea.element||n.isIos()||setTimeout(function(){o.focus(!0)},100),n.clearsContentEditableCorrectly()||e.quirks.ensureProperClearing(this),this.initSync&&this.config.sync&&this.initSync(),this.config.noTextarea||this.textarea.hide(),this.parent.fire("beforeload").fire("load")},_initAutoLinking:function(){var o=this,r=n.canDisableAutoLinking(),i=n.doesAutoLinkingInContentEditable();if(r&&this.commands.exec("autoUrlDetect",!1),this.config.autoLink){(!i||i&&r)&&(this.parent.on("newword:composer",function(){t.getTextContent(o.element).match(t.autoLink.URL_REG_EXP)&&o.selection.executeAndRestore(function(n,r){for(var i=o.element.querySelectorAll("."+o.config.uneditableContainerClassname),a=!1,s=i.length;s--;)e.dom.contains(i[s],r)&&(a=!0);a||t.autoLink(r.parentNode,[o.config.uneditableContainerClassname])})}),t.observe(this.element,"blur",function(){t.autoLink(o.element,[o.config.uneditableContainerClassname])}));var a=this.sandbox.getDocument().getElementsByTagName("a"),s=t.autoLink.URL_REG_EXP,l=function(n){var o=e.lang.string(t.getTextContent(n)).trim();return"www."===o.substr(0,4)&&(o="http://"+o),o};t.observe(this.element,"keydown",function(e){if(a.length){var n,r=o.selection.getSelectedNode(e.target.ownerDocument),i=t.getParentElement(r,{nodeName:"A"},4);i&&(n=l(i),setTimeout(function(){var e=l(i);e!==n&&e.match(s)&&i.setAttribute("href",e)},0))}})}},_initObjectResizing:function(){if(this.commands.exec("enableObjectResizing",!0),n.supportsEvent("resizeend")){var o=["width","height"],r=o.length,i=this.element;t.observe(i,"resizeend",function(t){var n,a=t.target||t.srcElement,s=a.style,l=0;if("IMG"===a.nodeName){for(;r>l;l++)n=o[l],s[n]&&(a.setAttribute(n,parseInt(s[n],10)),s[n]="");e.quirks.redraw(i)}})}},_initUndoManager:function(){this.undoManager=new e.UndoManager(this.parent)},_initLineBreaking:function(){function o(e){var n=t.getParentElement(e,{nodeName:["P","DIV"]},2);n&&t.contains(r.element,n)&&r.selection.executeAndRestore(function(){r.config.useLineBreaks?t.replaceWithChildNodes(n):"P"!==n.nodeName&&t.renameElement(n,"p")})}var r=this,i=["LI","P","H1","H2","H3","H4","H5","H6"],a=["UL","OL","MENU"];this.config.useLineBreaks||t.observe(this.element,["focus","keydown"],function(){if(r.isEmpty()){var e=r.doc.createElement("P");r.element.innerHTML="",r.element.appendChild(e),n.displaysCaretInEmptyContentEditableCorrectly()?r.selection.selectNode(e,!0):(e.innerHTML="<br>",r.selection.setBefore(e.firstChild))}}),t.observe(this.element,"keydown",function(n){var s=n.keyCode;if(!n.shiftKey&&(s===e.ENTER_KEY||s===e.BACKSPACE_KEY)){var l=t.getParentElement(r.selection.getSelectedNode(),{nodeName:i},4);return l?void setTimeout(function(){var n,i=r.selection.getSelectedNode();if("LI"===l.nodeName){if(!i)return;n=t.getParentElement(i,{nodeName:a},2),n||o(i)}s===e.ENTER_KEY&&l.nodeName.match(/^H[1-6]$/)&&o(i)},0):void(r.config.useLineBreaks&&s===e.ENTER_KEY&&!e.browser.insertsLineBreaksOnReturn()&&(n.preventDefault(),r.commands.exec("insertLineBreak")))}})}})}(wysihtml5),function(e){var t=e.dom,n=document,o=window,r=n.createElement("div"),i=["background-color","color","cursor","font-family","font-size","font-style","font-variant","font-weight","line-height","letter-spacing","text-align","text-decoration","text-indent","text-rendering","word-break","word-wrap","word-spacing"],a=["background-color","border-collapse","border-bottom-color","border-bottom-style","border-bottom-width","border-left-color","border-left-style","border-left-width","border-right-color","border-right-style","border-right-width","border-top-color","border-top-style","border-top-width","clear","display","float","margin-bottom","margin-left","margin-right","margin-top","outline-color","outline-offset","outline-width","outline-style","padding-left","padding-right","padding-top","padding-bottom","position","top","left","right","bottom","z-index","vertical-align","text-align","-webkit-box-sizing","-moz-box-sizing","-ms-box-sizing","box-sizing","-webkit-box-shadow","-moz-box-shadow","-ms-box-shadow","box-shadow","-webkit-border-top-right-radius","-moz-border-radius-topright","border-top-right-radius","-webkit-border-bottom-right-radius","-moz-border-radius-bottomright","border-bottom-right-radius","-webkit-border-bottom-left-radius","-moz-border-radius-bottomleft","border-bottom-left-radius","-webkit-border-top-left-radius","-moz-border-radius-topleft","border-top-left-radius","width","height"],s=["html                 { height: 100%; }","body                 { height: 100%; padding: 1px 0 0 0; margin: -1px 0 0 0; }","body > p:first-child { margin-top: 0; }","._wysihtml5-temp     { display: none; }",e.browser.isGecko?"body.placeholder { color: graytext !important; }":"body.placeholder { color: #a9a9a9 !important; }","img:-moz-broken      { -moz-force-broken-image-icon: 1; height: 24px; width: 24px; }"],l=function(e){if(e.setActive)try{e.setActive()}catch(r){}else{var i=e.style,a=n.documentElement.scrollTop||n.body.scrollTop,s=n.documentElement.scrollLeft||n.body.scrollLeft,l={position:i.position,top:i.top,left:i.left,WebkitUserSelect:i.WebkitUserSelect};t.setStyles({position:"absolute",top:"-99999px",left:"-99999px",WebkitUserSelect:"none"}).on(e),e.focus(),t.setStyles(l).on(e),o.scrollTo&&o.scrollTo(s,a)}};e.views.Composer.prototype.style=function(){var o,c=this,d=n.querySelector(":focus"),u=this.textarea.element,h=u.hasAttribute("placeholder"),f=h&&u.getAttribute("placeholder"),m=u.style.display,p=u.disabled;this.focusStylesHost=r.cloneNode(!1),this.blurStylesHost=r.cloneNode(!1),this.disabledStylesHost=r.cloneNode(!1),h&&u.removeAttribute("placeholder"),u===d&&u.blur(),u.disabled=!1,u.style.display=o="none",(u.getAttribute("rows")&&"auto"===t.getStyle("height").from(u)||u.getAttribute("cols")&&"auto"===t.getStyle("width").from(u))&&(u.style.display=o=m),t.copyStyles(a).from(u).to(this.editableArea).andTo(this.blurStylesHost),t.copyStyles(i).from(u).to(this.element).andTo(this.blurStylesHost),t.insertCSS(s).into(this.element.ownerDocument),u.disabled=!0,t.copyStyles(a).from(u).to(this.disabledStylesHost),t.copyStyles(i).from(u).to(this.disabledStylesHost),u.disabled=p,u.style.display=m,l(u),u.style.display=o,t.copyStyles(a).from(u).to(this.focusStylesHost),t.copyStyles(i).from(u).to(this.focusStylesHost),u.style.display=m,t.copyStyles(["display"]).from(u).to(this.editableArea);var g=e.lang.array(a).without(["display"]);return d?d.focus():u.blur(),h&&u.setAttribute("placeholder",f),this.parent.on("focus:composer",function(){t.copyStyles(g).from(c.focusStylesHost).to(c.editableArea),t.copyStyles(i).from(c.focusStylesHost).to(c.element)}),this.parent.on("blur:composer",function(){t.copyStyles(g).from(c.blurStylesHost).to(c.editableArea),t.copyStyles(i).from(c.blurStylesHost).to(c.element)}),this.parent.observe("disable:composer",function(){t.copyStyles(g).from(c.disabledStylesHost).to(c.editableArea),t.copyStyles(i).from(c.disabledStylesHost).to(c.element)}),this.parent.observe("enable:composer",function(){t.copyStyles(g).from(c.blurStylesHost).to(c.editableArea),t.copyStyles(i).from(c.blurStylesHost).to(c.element)}),this}}(wysihtml5),function(e){var t=e.dom,n=e.browser,o={66:"bold",73:"italic",85:"underline"},r=function(e,t,n){var o=e.getPreviousNode(t,!0),r=e.getSelectedNode();if(1!==r.nodeType&&r.parentNode!==n&&(r=r.parentNode),o)if(1==r.nodeType){var i=r.firstChild;if(1==o.nodeType)for(;r.firstChild;)o.appendChild(r.firstChild);else for(;r.firstChild;)t.parentNode.insertBefore(r.firstChild,t);r.parentNode&&r.parentNode.removeChild(r),e.setBefore(i)}else 1==o.nodeType?o.appendChild(r):t.parentNode.insertBefore(r,t),e.setBefore(r)},i=function(e,t,n,o){if(t.isCollapsed())if(t.caretIsInTheBeginnig("LI"))e.preventDefault(),o.commands.exec("outdentList");else if(t.caretIsInTheBeginnig())e.preventDefault();else{if(t.caretIsFirstInSelection()&&t.getPreviousNode()&&t.getPreviousNode().nodeName&&/^H\d$/gi.test(t.getPreviousNode().nodeName)){var i=t.getPreviousNode();if(e.preventDefault(),/^\s*$/.test(i.textContent||i.innerText))i.parentNode.removeChild(i);else{var a=i.ownerDocument.createRange();a.selectNodeContents(i),a.collapse(!1),t.setSelection(a)}}var s=t.caretIsBeforeUneditable();s&&(e.preventDefault(),r(t,s,n))}else t.containsUneditable()&&(e.preventDefault(),t.deleteContents())},a=function(e,t){if(e.selection.isCollapsed()){if(e.selection.caretIsInTheBeginnig("LI")&&e.commands.exec("indentList"))return}else e.selection.deleteContents();e.commands.exec("insertHTML","&emsp;")};e.views.Composer.prototype.observe=function(){var r=this,s=this.getValue(!1,!1),l=this.sandbox.getIframe?this.sandbox.getIframe():this.sandbox.getContentEditable(),c=this.element,d=n.supportsEventsInIframeCorrectly()||this.sandbox.getContentEditable?c:this.sandbox.getWindow(),u=["drop","paste"],h=["drop","paste","mouseup","focus","keyup"];if(t.observe(l,"DOMNodeRemoved",function(){clearInterval(f),r.parent.fire("destroy:composer")}),!n.supportsMutationEvents())var f=setInterval(function(){t.contains(document.documentElement,l)||(clearInterval(f),r.parent.fire("destroy:composer"))},250);t.observe(d,h,function(){setTimeout(function(){r.parent.fire("interaction").fire("interaction:composer")},0)}),this.config.handleTables&&(!this.tableClickHandle&&this.doc.execCommand&&e.browser.supportsCommand(this.doc,"enableObjectResizing")&&e.browser.supportsCommand(this.doc,"enableInlineTableEditing")&&(this.sandbox.getIframe?this.tableClickHandle=t.observe(l,["focus","mouseup","mouseover"],function(){r.doc.execCommand("enableObjectResizing",!1,"false"),r.doc.execCommand("enableInlineTableEditing",!1,"false"),r.tableClickHandle.stop()}):setTimeout(function(){r.doc.execCommand("enableObjectResizing",!1,"false"),r.doc.execCommand("enableInlineTableEditing",!1,"false")},0)),this.tableSelection=e.quirks.tableCellsSelection(c,r.parent)),t.observe(d,"focus",function(e){r.parent.fire("focus",e).fire("focus:composer",e),setTimeout(function(){s=r.getValue(!1,!1)},0)}),t.observe(d,"blur",function(e){if(s!==r.getValue(!1,!1)){var t=e;"function"==typeof Object.create&&(t=Object.create(e,{type:{value:"change"}})),r.parent.fire("change",t).fire("change:composer",t)}r.parent.fire("blur",e).fire("blur:composer",e)}),t.observe(c,"dragenter",function(){r.parent.fire("unset_placeholder")}),t.observe(c,u,function(e){setTimeout(function(){r.parent.fire(e.type,e).fire(e.type+":composer",e)},0)}),t.observe(c,"keyup",function(t){var n=t.keyCode;(n===e.SPACE_KEY||n===e.ENTER_KEY)&&r.parent.fire("newword:composer")}),this.parent.on("paste:composer",function(){setTimeout(function(){r.parent.fire("newword:composer")},0)}),n.canSelectImagesInContentEditable()||t.observe(c,"mousedown",function(t){var n=t.target,o=c.querySelectorAll("img"),i=c.querySelectorAll("."+r.config.uneditableContainerClassname+" img"),a=e.lang.array(o).without(i);"IMG"===n.nodeName&&e.lang.array(a).contains(n)&&r.selection.selectNode(n)}),n.canSelectImagesInContentEditable()||t.observe(c,"drop",function(e){setTimeout(function(){r.selection.getSelection().removeAllRanges()},0)}),n.hasHistoryIssue()&&n.supportsSelectionModify()&&t.observe(c,"keydown",function(e){if(e.metaKey||e.ctrlKey){var t=e.keyCode,n=c.ownerDocument.defaultView,o=n.getSelection();(37===t||39===t)&&(37===t&&(o.modify("extend","left","lineboundary"),e.shiftKey||o.collapseToStart()),39===t&&(o.modify("extend","right","lineboundary"),e.shiftKey||o.collapseToEnd()),e.preventDefault())}}),t.observe(c,"keydown",function(e){var t=e.keyCode,n=o[t];(e.ctrlKey||e.metaKey)&&!e.altKey&&n&&(r.commands.exec(n),
e.preventDefault()),8===t?i(e,r.selection,c,r):r.config.handleTabKey&&9===t&&(e.preventDefault(),a(r,c))}),t.observe(c,"keydown",function(t){var n,o=r.selection.getSelectedNode(!0),i=t.keyCode;!o||"IMG"!==o.nodeName||i!==e.BACKSPACE_KEY&&i!==e.DELETE_KEY||(n=o.parentNode,n.removeChild(o),"A"!==n.nodeName||n.firstChild||n.parentNode.removeChild(n),setTimeout(function(){e.quirks.redraw(c)},0),t.preventDefault())}),!this.config.contentEditableMode&&n.hasIframeFocusIssue()&&(t.observe(l,"focus",function(){setTimeout(function(){r.doc.querySelector(":focus")!==r.element&&r.focus()},0)}),t.observe(this.element,"blur",function(){setTimeout(function(){r.selection.getSelection().removeAllRanges()},0)}));var m={IMG:"Image: ",A:"Link: "};t.observe(c,"mouseover",function(e){var t,n=e.target,o=n.nodeName;if("A"===o||"IMG"===o){var r=n.hasAttribute("title");r||(t=m[o]+(n.getAttribute("href")||n.getAttribute("src")),n.setAttribute("title",t))}})}}(wysihtml5),function(e){var t=400;e.views.Synchronizer=Base.extend({constructor:function(e,t,n){this.editor=e,this.textarea=t,this.composer=n,this._observe()},fromComposerToTextarea:function(t){this.textarea.setValue(e.lang.string(this.composer.getValue(!1,!1)).trim(),t)},fromTextareaToComposer:function(e){var t=this.textarea.getValue(!1,!1);t?this.composer.setValue(t,e):(this.composer.clear(),this.editor.fire("set_placeholder"))},sync:function(e){"textarea"===this.editor.currentView.name?this.fromTextareaToComposer(e):this.fromComposerToTextarea(e)},_observe:function(){var n,o=this,r=this.textarea.element.form,i=function(){n=setInterval(function(){o.fromComposerToTextarea()},t)},a=function(){clearInterval(n),n=null};i(),r&&(e.dom.observe(r,"submit",function(){o.sync(!0)}),e.dom.observe(r,"reset",function(){setTimeout(function(){o.fromTextareaToComposer()},0)})),this.editor.on("change_view",function(e){"composer"!==e||n?"textarea"===e&&(o.fromComposerToTextarea(!0),a()):(o.fromTextareaToComposer(!0),i())}),this.editor.on("destroy:composer",a)}})}(wysihtml5),wysihtml5.views.Textarea=wysihtml5.views.View.extend({name:"textarea",constructor:function(e,t,n){this.base(e,t,n),this._observe()},clear:function(){this.element.value=""},getValue:function(e){var t=this.isEmpty()?"":this.element.value;return e!==!1&&(t=this.parent.parse(t)),t},setValue:function(e,t){t&&(e=this.parent.parse(e)),this.element.value=e},cleanUp:function(){var e=this.parent.parse(this.element.value);this.element.value=e},hasPlaceholderSet:function(){var e=wysihtml5.browser.supportsPlaceholderAttributeOn(this.element),t=this.element.getAttribute("placeholder")||null,n=this.element.value,o=!n;return e&&o||n===t},isEmpty:function(){return!wysihtml5.lang.string(this.element.value).trim()||this.hasPlaceholderSet()},_observe:function(){var e=this.element,t=this.parent,n={focusin:"focus",focusout:"blur"},o=wysihtml5.browser.supportsEvent("focusin")?["focusin","focusout","change"]:["focus","blur","change"];t.on("beforeload",function(){wysihtml5.dom.observe(e,o,function(e){var o=n[e.type]||e.type;t.fire(o).fire(o+":textarea")}),wysihtml5.dom.observe(e,["paste","drop"],function(){setTimeout(function(){t.fire("paste").fire("paste:textarea")},0)})})}}),function(e){var t,n={name:t,style:!0,toolbar:t,showToolbarAfterInit:!0,autoLink:!0,handleTables:!0,handleTabKey:!0,parserRules:{tags:{br:{},span:{},div:{},p:{}},classes:{}},parser:e.dom.parse,composerClassName:"wysihtml5-editor",bodyClassName:"wysihtml5-supported",useLineBreaks:!0,stylesheets:[],placeholderText:t,supportTouchDevices:!0,cleanUp:!0,contentEditableMode:!1,uneditableContainerClassname:"wysihtml5-uneditable-container"};e.Editor=e.lang.Dispatcher.extend({constructor:function(t,o){if(this.editableElement="string"==typeof t?document.getElementById(t):t,this.config=e.lang.object({}).merge(n).merge(o).get(),this._isCompatible=e.browser.supported(),"textarea"!=this.editableElement.nodeName.toLowerCase()&&(this.config.contentEditableMode=!0,this.config.noTextarea=!0),this.config.noTextarea||(this.textarea=new e.views.Textarea(this,this.editableElement,this.config),this.currentView=this.textarea),!this._isCompatible||!this.config.supportTouchDevices&&e.browser.isTouchDevice()){var r=this;return void setTimeout(function(){r.fire("beforeload").fire("load")},0)}e.dom.addClass(document.body,this.config.bodyClassName),this.composer=new e.views.Composer(this,this.editableElement,this.config),this.currentView=this.composer,"function"==typeof this.config.parser&&this._initParser(),this.on("beforeload",this.handleBeforeLoad)},handleBeforeLoad:function(){this.config.noTextarea||(this.synchronizer=new e.views.Synchronizer(this,this.textarea,this.composer)),this.config.toolbar&&(this.toolbar=new e.toolbar.Toolbar(this,this.config.toolbar,this.config.showToolbarAfterInit))},isCompatible:function(){return this._isCompatible},clear:function(){return this.currentView.clear(),this},getValue:function(e,t){return this.currentView.getValue(e,t)},setValue:function(e,t){return this.fire("unset_placeholder"),e?(this.currentView.setValue(e,t),this):this.clear()},cleanUp:function(){this.currentView.cleanUp()},focus:function(e){return this.currentView.focus(e),this},disable:function(){return this.currentView.disable(),this},enable:function(){return this.currentView.enable(),this},isEmpty:function(){return this.currentView.isEmpty()},hasPlaceholderSet:function(){return this.currentView.hasPlaceholderSet()},parse:function(t,n){var o=this.config.contentEditableMode?document:this.composer?this.composer.sandbox.getDocument():null,r=this.config.parser(t,{rules:this.config.parserRules,cleanUp:this.config.cleanUp,context:o,uneditableClass:this.config.uneditableContainerClassname,clearInternals:n});return"object"==typeof t&&e.quirks.redraw(t),r},_initParser:function(){this.on("paste:composer",function(){var t=!0,n=this;n.composer.selection.executeAndRestore(function(){e.quirks.cleanPastedHTML(n.composer.element),n.parse(n.composer.element)},t)})}})}(wysihtml5),function(e){var t=e.dom,n="wysihtml5-command-dialog-opened",o="input, select, textarea",r="[data-wysihtml5-dialog-field]",i="data-wysihtml5-dialog-field";e.toolbar.Dialog=e.lang.Dispatcher.extend({constructor:function(e,t){this.link=e,this.container=t},_observe:function(){if(!this._observed){var r=this,i=function(e){var t=r._serialize();t==r.elementToChange?r.fire("edit",t):r.fire("save",t),r.hide(),e.preventDefault(),e.stopPropagation()};t.observe(r.link,"click",function(){t.hasClass(r.link,n)&&setTimeout(function(){r.hide()},0)}),t.observe(this.container,"keydown",function(t){var n=t.keyCode;n===e.ENTER_KEY&&i(t),n===e.ESCAPE_KEY&&(r.fire("cancel"),r.hide())}),t.delegate(this.container,"[data-wysihtml5-dialog-action=save]","click",i),t.delegate(this.container,"[data-wysihtml5-dialog-action=cancel]","click",function(e){r.fire("cancel"),r.hide(),e.preventDefault(),e.stopPropagation()});for(var a=this.container.querySelectorAll(o),s=0,l=a.length,c=function(){clearInterval(r.interval)};l>s;s++)t.observe(a[s],"change",c);this._observed=!0}},_serialize:function(){for(var e=this.elementToChange||{},t=this.container.querySelectorAll(r),n=t.length,o=0;n>o;o++)e[t[o].getAttribute(i)]=t[o].value;return e},_interpolate:function(e){for(var t,n,o,a=document.querySelector(":focus"),s=this.container.querySelectorAll(r),l=s.length,c=0;l>c;c++)t=s[c],t!==a&&(e&&"hidden"===t.type||(n=t.getAttribute(i),o=this.elementToChange&&"boolean"!=typeof this.elementToChange?this.elementToChange.getAttribute(n)||"":t.defaultValue,t.value=o))},show:function(e){if(!t.hasClass(this.link,n)){var r=this,i=this.container.querySelector(o);if(this.elementToChange=e,this._observe(),this._interpolate(),e&&(this.interval=setInterval(function(){r._interpolate(!0)},500)),t.addClass(this.link,n),this.container.style.display="",this.fire("show"),i&&!e)try{i.focus()}catch(a){}}},hide:function(){clearInterval(this.interval),this.elementToChange=null,t.removeClass(this.link,n),this.container.style.display="none",this.fire("hide")}})}(wysihtml5),function(e){var t=e.dom,n={position:"relative"},o={left:0,margin:0,opacity:0,overflow:"hidden",padding:0,position:"absolute",top:0,zIndex:1},r={cursor:"inherit",fontSize:"50px",height:"50px",marginTop:"-25px",outline:0,padding:0,position:"absolute",right:"-4px",top:"50%"},i={"x-webkit-speech":"",speech:""};e.toolbar.Speech=function(a,s){var l=document.createElement("input");if(!e.browser.supportsSpeechApiOn(l))return void(s.style.display="none");var c=a.editor.textarea.element.getAttribute("lang");c&&(i.lang=c);var d=document.createElement("div");e.lang.object(o).merge({width:s.offsetWidth+"px",height:s.offsetHeight+"px"}),t.insert(l).into(d),t.insert(d).into(s),t.setStyles(r).on(l),t.setAttributes(i).on(l),t.setStyles(o).on(d),t.setStyles(n).on(s);var u="onwebkitspeechchange"in l?"webkitspeechchange":"speechchange";t.observe(l,u,function(){a.execCommand("insertText",l.value),l.value=""}),t.observe(l,"click",function(e){t.hasClass(s,"wysihtml5-command-disabled")&&e.preventDefault(),e.stopPropagation()})}}(wysihtml5),function(e){var t="wysihtml5-command-disabled",n="wysihtml5-commands-disabled",o="wysihtml5-command-active",r="wysihtml5-action-active",i=e.dom;e.toolbar.Toolbar=Base.extend({constructor:function(t,n,o){this.editor=t,this.container="string"==typeof n?document.getElementById(n):n,this.composer=t.composer,this._getLinks("command"),this._getLinks("action"),this._observe(),o&&this.show();for(var r=this.container.querySelectorAll("[data-wysihtml5-command=insertSpeech]"),i=r.length,a=0;i>a;a++)new e.toolbar.Speech(this,r[a])},_getLinks:function(t){for(var n,o,r,i,a,s=this[t+"Links"]=e.lang.array(this.container.querySelectorAll("[data-wysihtml5-"+t+"]")).get(),l=s.length,c=0,d=this[t+"Mapping"]={};l>c;c++)n=s[c],r=n.getAttribute("data-wysihtml5-"+t),i=n.getAttribute("data-wysihtml5-"+t+"-value"),o=this.container.querySelector("[data-wysihtml5-"+t+"-group='"+r+"']"),a=this._getDialog(n,r),d[r+":"+i]={link:n,group:o,name:r,value:i,dialog:a,state:!1}},_getDialog:function(t,n){var o,r,i=this,a=this.container.querySelector("[data-wysihtml5-dialog='"+n+"']");return a&&(o=e.toolbar["Dialog_"+n]?new e.toolbar["Dialog_"+n](t,a):new e.toolbar.Dialog(t,a),o.on("show",function(){r=i.composer.selection.getBookmark(),i.editor.fire("show:dialog",{command:n,dialogContainer:a,commandLink:t})}),o.on("save",function(e){r&&i.composer.selection.setBookmark(r),i._execCommand(n,e),i.editor.fire("save:dialog",{command:n,dialogContainer:a,commandLink:t})}),o.on("cancel",function(){i.editor.focus(!1),i.editor.fire("cancel:dialog",{command:n,dialogContainer:a,commandLink:t})})),o},execCommand:function(e,t){if(!this.commandsDisabled){var n=this.commandMapping[e+":"+t];n&&n.dialog&&!n.state?n.dialog.show():this._execCommand(e,t)}},_execCommand:function(e,t){this.editor.focus(!1),this.composer.commands.exec(e,t),this._updateLinkStates()},execAction:function(e){var t=this.editor;"change_view"===e&&t.textarea&&(t.currentView===t.textarea?t.fire("change_view","composer"):t.fire("change_view","textarea")),"showSource"==e&&t.fire("showSource")},_observe:function(){for(var e=this,t=this.editor,o=this.container,r=this.commandLinks.concat(this.actionLinks),a=r.length,s=0;a>s;s++)"A"===r[s].nodeName?i.setAttributes({href:"javascript:;",unselectable:"on"}).on(r[s]):i.setAttributes({unselectable:"on"}).on(r[s]);i.delegate(o,"[data-wysihtml5-command], [data-wysihtml5-action]","mousedown",function(e){e.preventDefault()}),i.delegate(o,"[data-wysihtml5-command]","click",function(t){var n=this,o=n.getAttribute("data-wysihtml5-command"),r=n.getAttribute("data-wysihtml5-command-value");e.execCommand(o,r),t.preventDefault()}),i.delegate(o,"[data-wysihtml5-action]","click",function(t){var n=this.getAttribute("data-wysihtml5-action");e.execAction(n),t.preventDefault()}),t.on("interaction:composer",function(){e._updateLinkStates()}),t.on("focus:composer",function(){e.bookmark=null}),this.editor.config.handleTables&&(t.on("tableselect:composer",function(){e.container.querySelectorAll('[data-wysihtml5-hiddentools="table"]')[0].style.display=""}),t.on("tableunselect:composer",function(){e.container.querySelectorAll('[data-wysihtml5-hiddentools="table"]')[0].style.display="none"})),t.on("change_view",function(r){t.textarea&&setTimeout(function(){e.commandsDisabled="composer"!==r,e._updateLinkStates(),e.commandsDisabled?i.addClass(o,n):i.removeClass(o,n)},0)})},_updateLinkStates:function(){var n,a,s,l,c=this.commandMapping,d=this.actionMapping;for(n in c)l=c[n],this.commandsDisabled?(a=!1,i.removeClass(l.link,o),l.group&&i.removeClass(l.group,o),l.dialog&&l.dialog.hide()):(a=this.composer.commands.state(l.name,l.value),i.removeClass(l.link,t),l.group&&i.removeClass(l.group,t)),l.state!==a&&(l.state=a,a?(i.addClass(l.link,o),l.group&&i.addClass(l.group,o),l.dialog&&("object"==typeof a||e.lang.object(a).isArray()?(!l.dialog.multiselect&&e.lang.object(a).isArray()&&(a=1===a.length?a[0]:!0,l.state=a),l.dialog.show(a)):l.dialog.hide())):(i.removeClass(l.link,o),l.group&&i.removeClass(l.group,o),l.dialog&&l.dialog.hide()));for(n in d)s=d[n],"change_view"===s.name&&(s.state=this.editor.currentView===this.editor.textarea,s.state?i.addClass(s.link,r):i.removeClass(s.link,r))},show:function(){this.container.style.display=""},hide:function(){this.container.style.display="none"}})}(wysihtml5),function(e){e.toolbar.Dialog_createTable=e.toolbar.Dialog.extend({show:function(e){this.base(e)}})}(wysihtml5),function(e){var t=(e.dom,"[data-wysihtml5-dialog-field]"),n="data-wysihtml5-dialog-field";e.toolbar.Dialog_foreColorStyle=e.toolbar.Dialog.extend({multiselect:!0,_serialize:function(){for(var e={},o=this.container.querySelectorAll(t),r=o.length,i=0;r>i;i++)e[o[i].getAttribute(n)]=o[i].value;return e},_interpolate:function(o){for(var r,i=document.querySelector(":focus"),a=this.container.querySelectorAll(t),s=a.length,l=0,c=this.elementToChange?e.lang.object(this.elementToChange).isArray()?this.elementToChange[0]:this.elementToChange:null,d=c?c.getAttribute("style"):null,u=d?e.quirks.styleParser.parseColor(d,"color"):null;s>l;l++)r=a[l],r!==i&&(o&&"hidden"===r.type||"color"===r.getAttribute(n)&&(u?u[3]&&1!=u[3]?r.value="rgba("+u[0]+","+u[1]+","+u[2]+","+u[3]+");":r.value="rgb("+u[0]+","+u[1]+","+u[2]+");":r.value="rgb(0,0,0);"))}})}(wysihtml5),function(e){e.dom;e.toolbar.Dialog_fontSizeStyle=e.toolbar.Dialog.extend({multiselect:!0,_serialize:function(){return{size:this.container.querySelector('[data-wysihtml5-dialog-field="size"]').value}},_interpolate:function(t){var n=document.querySelector(":focus"),o=this.container.querySelector("[data-wysihtml5-dialog-field='size']"),r=this.elementToChange?e.lang.object(this.elementToChange).isArray()?this.elementToChange[0]:this.elementToChange:null,i=r?r.getAttribute("style"):null,a=i?e.quirks.styleParser.parseFontSize(i):null;o&&o!==n&&a&&!/^\s*$/.test(a)&&(o.value=a)}})}(wysihtml5);var Handlebars=function(){var e=function(){"use strict";function e(e){this.string=e}var t;return e.prototype.toString=function(){return""+this.string},t=e}(),t=function(e){"use strict";function t(e){return s[e]||"&amp;"}function n(e,t){for(var n in t)Object.prototype.hasOwnProperty.call(t,n)&&(e[n]=t[n])}function o(e){return e instanceof a?e.toString():e||0===e?(e=""+e,c.test(e)?e.replace(l,t):e):""}function r(e){return e||0===e?h(e)&&0===e.length?!0:!1:!0}var i={},a=e,s={"&":"&amp;","<":"&lt;",">":"&gt;",'"':"&quot;","'":"&#x27;","`":"&#x60;"},l=/[&<>"'`]/g,c=/[&<>"'`]/;i.extend=n;var d=Object.prototype.toString;i.toString=d;var u=function(e){return"function"==typeof e};u(/x/)&&(u=function(e){return"function"==typeof e&&"[object Function]"===d.call(e)});var u;i.isFunction=u;var h=Array.isArray||function(e){return e&&"object"==typeof e?"[object Array]"===d.call(e):!1};return i.isArray=h,i.escapeExpression=o,i.isEmpty=r,i}(e),n=function(){"use strict";function e(e,t){var o;t&&t.firstLine&&(o=t.firstLine,e+=" - "+o+":"+t.firstColumn);for(var r=Error.prototype.constructor.call(this,e),i=0;i<n.length;i++)this[n[i]]=r[n[i]];o&&(this.lineNumber=o,this.column=t.firstColumn)}var t,n=["description","fileName","lineNumber","message","name","number","stack"];return e.prototype=new Error,t=e}(),o=function(e,t){"use strict";function n(e,t){this.helpers=e||{},this.partials=t||{},o(this)}function o(e){e.registerHelper("helperMissing",function(e){if(2===arguments.length)return void 0;throw new s("Missing helper: '"+e+"'")}),e.registerHelper("blockHelperMissing",function(t,n){var o=n.inverse||function(){},r=n.fn;return h(t)&&(t=t.call(this)),t===!0?r(this):t===!1||null==t?o(this):u(t)?t.length>0?e.helpers.each(t,n):o(this):r(t)}),e.registerHelper("each",function(e,t){var n,o=t.fn,r=t.inverse,i=0,a="";if(h(e)&&(e=e.call(this)),t.data&&(n=g(t.data)),e&&"object"==typeof e)if(u(e))for(var s=e.length;s>i;i++)n&&(n.index=i,n.first=0===i,n.last=i===e.length-1),a+=o(e[i],{data:n});else for(var l in e)e.hasOwnProperty(l)&&(n&&(n.key=l,n.index=i,n.first=0===i),a+=o(e[l],{data:n}),i++);return 0===i&&(a=r(this)),a}),e.registerHelper("if",function(e,t){return h(e)&&(e=e.call(this)),!t.hash.includeZero&&!e||a.isEmpty(e)?t.inverse(this):t.fn(this)}),e.registerHelper("unless",function(t,n){return e.helpers["if"].call(this,t,{fn:n.inverse,inverse:n.fn,hash:n.hash})}),e.registerHelper("with",function(e,t){return h(e)&&(e=e.call(this)),a.isEmpty(e)?void 0:t.fn(e)}),e.registerHelper("log",function(t,n){var o=n.data&&null!=n.data.level?parseInt(n.data.level,10):1;e.log(o,t)})}function r(e,t){p.log(e,t)}var i={},a=e,s=t,l="1.3.0";i.VERSION=l;var c=4;i.COMPILER_REVISION=c;var d={1:"<= 1.0.rc.2",2:"== 1.0.0-rc.3",3:"== 1.0.0-rc.4",4:">= 1.0.0"};i.REVISION_CHANGES=d;var u=a.isArray,h=a.isFunction,f=a.toString,m="[object Object]";i.HandlebarsEnvironment=n,n.prototype={constructor:n,logger:p,log:r,registerHelper:function(e,t,n){if(f.call(e)===m){if(n||t)throw new s("Arg not supported with multiple helpers");a.extend(this.helpers,e)}else n&&(t.not=n),this.helpers[e]=t},registerPartial:function(e,t){f.call(e)===m?a.extend(this.partials,e):this.partials[e]=t}};var p={methodMap:{0:"debug",1:"info",2:"warn",3:"error"},DEBUG:0,INFO:1,WARN:2,ERROR:3,level:3,log:function(e,t){if(p.level<=e){var n=p.methodMap[e];"undefined"!=typeof console&&console[n]&&console[n].call(console,t)}}};i.logger=p,i.log=r;var g=function(e){var t={};return a.extend(t,e),t};return i.createFrame=g,i}(t,n),r=function(e,t,n){"use strict";function o(e){var t=e&&e[0]||1,n=h;if(t!==n){if(n>t){var o=f[n],r=f[t];throw new u("Template was precompiled with an older version of Handlebars than the current runtime. Please update your precompiler to a newer version ("+o+") or downgrade your runtime to an older version ("+r+").")}throw new u("Template was precompiled with a newer version of Handlebars than the current runtime. Please update your runtime to a newer version ("+e[1]+").")}}function r(e,t){if(!t)throw new u("No environment passed to template");var n=function(e,n,o,r,i,a){var s=t.VM.invokePartial.apply(this,arguments);if(null!=s)return s;if(t.compile){var l={helpers:r,partials:i,data:a};return i[n]=t.compile(e,{data:void 0!==a},t),i[n](o,l)}throw new u("The partial "+n+" could not be compiled when running in runtime-only mode")},o={escapeExpression:d.escapeExpression,invokePartial:n,programs:[],program:function(e,t,n){var o=this.programs[e];return n?o=a(e,t,n):o||(o=this.programs[e]=a(e,t)),o},merge:function(e,t){var n=e||t;return e&&t&&e!==t&&(n={},d.extend(n,t),d.extend(n,e)),n},programWithDepth:t.VM.programWithDepth,noop:t.VM.noop,compilerInfo:null};return function(n,r){r=r||{};var i,a,s=r.partial?r:t;r.partial||(i=r.helpers,a=r.partials);var l=e.call(o,s,n,i,a,r.data);return r.partial||t.VM.checkRevision(o.compilerInfo),l}}function i(e,t,n){var o=Array.prototype.slice.call(arguments,3),r=function(e,r){return r=r||{},t.apply(this,[e,r.data||n].concat(o))};return r.program=e,r.depth=o.length,r}function a(e,t,n){var o=function(e,o){return o=o||{},t(e,o.data||n)};return o.program=e,o.depth=0,o}function s(e,t,n,o,r,i){var a={partial:!0,helpers:o,partials:r,data:i};if(void 0===e)throw new u("The partial "+t+" could not be found");return e instanceof Function?e(n,a):void 0}function l(){return""}var c={},d=e,u=t,h=n.COMPILER_REVISION,f=n.REVISION_CHANGES;return c.checkRevision=o,c.template=r,c.programWithDepth=i,c.program=a,c.invokePartial=s,c.noop=l,c}(t,n,o),i=function(e,t,n,o,r){"use strict";var i,a=e,s=t,l=n,c=o,d=r,u=function(){var e=new a.HandlebarsEnvironment;return c.extend(e,a),e.SafeString=s,e.Exception=l,e.Utils=c,e.VM=d,e.template=function(t){return d.template(t,e)},e},h=u();return h.create=u,i=h}(o,e,n,t,r);return i}();this.wysihtml5=this.wysihtml5||{},this.wysihtml5.tpl=this.wysihtml5.tpl||{},this.wysihtml5.tpl.blockquote=Handlebars.template(function(e,t,n,o,r){function i(e,t){var n,o="";return o+="btn-"+u((n=e&&e.options,n=null==n||n===!1?n:n.toolbar,n=null==n||n===!1?n:n.size,typeof n===d?n.apply(e):n))}function a(e,t){return' \n      <span class="fa fa-quote-left"></span>\n    '}function s(e,t){return'\n      <span class="glyphicon glyphicon-quote"></span>\n    '}this.compilerInfo=[4,">= 1.0.0"],n=this.merge(n,e.helpers),r=r||{};var l,c="",d="function",u=this.escapeExpression,h=this;return c+='<li>\n  <a class="btn ',l=n["if"].call(t,(l=t&&t.options,l=null==l||l===!1?l:l.toolbar,null==l||l===!1?l:l.size),{hash:{},inverse:h.noop,fn:h.program(1,i,r),data:r}),(l||0===l)&&(c+=l),c+=' btn-default" data-wysihtml5-command="formatBlock" data-wysihtml5-command-value="blockquote" data-wysihtml5-display-format-name="false" tabindex="-1">\n    ',l=n["if"].call(t,(l=t&&t.options,l=null==l||l===!1?l:l.toolbar,null==l||l===!1?l:l.fa),{hash:{},inverse:h.program(5,s,r),fn:h.program(3,a,r),data:r}),(l||0===l)&&(c+=l),c+="\n  </a>\n</li>\n"}),this.wysihtml5.tpl.color=Handlebars.template(function(e,t,n,o,r){function i(e,t){var n,o="";return o+="btn-"+c((n=e&&e.options,n=null==n||n===!1?n:n.toolbar,n=null==n||n===!1?n:n.size,typeof n===l?n.apply(e):n))}this.compilerInfo=[4,">= 1.0.0"],n=this.merge(n,e.helpers),r=r||{};var a,s="",l="function",c=this.escapeExpression,d=this;return s+='<li class="dropdown">\n  <a class="btn btn-default dropdown-toggle ',a=n["if"].call(t,(a=t&&t.options,a=null==a||a===!1?a:a.toolbar,null==a||a===!1?a:a.size),{hash:{},inverse:d.noop,fn:d.program(1,i,r),data:r}),(a||0===a)&&(s+=a),s+='" data-toggle="dropdown" tabindex="-1">\n    <span class="current-color">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.colours,a=null==a||a===!1?a:a.black,typeof a===l?a.apply(t):a))+'</span>\n    <b class="caret"></b>\n  </a>\n  <ul class="dropdown-menu">\n    <li><div class="wysihtml5-colors" data-wysihtml5-command-value="black"></div><a class="wysihtml5-colors-title" data-wysihtml5-command="foreColor" data-wysihtml5-command-value="black">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.colours,a=null==a||a===!1?a:a.black,typeof a===l?a.apply(t):a))+'</a></li>\n    <li><div class="wysihtml5-colors" data-wysihtml5-command-value="silver"></div><a class="wysihtml5-colors-title" data-wysihtml5-command="foreColor" data-wysihtml5-command-value="silver">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.colours,a=null==a||a===!1?a:a.silver,typeof a===l?a.apply(t):a))+'</a></li>\n    <li><div class="wysihtml5-colors" data-wysihtml5-command-value="gray"></div><a class="wysihtml5-colors-title" data-wysihtml5-command="foreColor" data-wysihtml5-command-value="gray">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.colours,a=null==a||a===!1?a:a.gray,typeof a===l?a.apply(t):a))+'</a></li>\n    <li><div class="wysihtml5-colors" data-wysihtml5-command-value="maroon"></div><a class="wysihtml5-colors-title" data-wysihtml5-command="foreColor" data-wysihtml5-command-value="maroon">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.colours,a=null==a||a===!1?a:a.maroon,typeof a===l?a.apply(t):a))+'</a></li>\n    <li><div class="wysihtml5-colors" data-wysihtml5-command-value="red"></div><a class="wysihtml5-colors-title" data-wysihtml5-command="foreColor" data-wysihtml5-command-value="red">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.colours,a=null==a||a===!1?a:a.red,typeof a===l?a.apply(t):a))+'</a></li>\n    <li><div class="wysihtml5-colors" data-wysihtml5-command-value="purple"></div><a class="wysihtml5-colors-title" data-wysihtml5-command="foreColor" data-wysihtml5-command-value="purple">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.colours,a=null==a||a===!1?a:a.purple,typeof a===l?a.apply(t):a))+'</a></li>\n    <li><div class="wysihtml5-colors" data-wysihtml5-command-value="green"></div><a class="wysihtml5-colors-title" data-wysihtml5-command="foreColor" data-wysihtml5-command-value="green">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.colours,a=null==a||a===!1?a:a.green,typeof a===l?a.apply(t):a))+'</a></li>\n    <li><div class="wysihtml5-colors" data-wysihtml5-command-value="olive"></div><a class="wysihtml5-colors-title" data-wysihtml5-command="foreColor" data-wysihtml5-command-value="olive">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.colours,a=null==a||a===!1?a:a.olive,typeof a===l?a.apply(t):a))+'</a></li>\n    <li><div class="wysihtml5-colors" data-wysihtml5-command-value="navy"></div><a class="wysihtml5-colors-title" data-wysihtml5-command="foreColor" data-wysihtml5-command-value="navy">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.colours,a=null==a||a===!1?a:a.navy,typeof a===l?a.apply(t):a))+'</a></li>\n    <li><div class="wysihtml5-colors" data-wysihtml5-command-value="blue"></div><a class="wysihtml5-colors-title" data-wysihtml5-command="foreColor" data-wysihtml5-command-value="blue">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.colours,a=null==a||a===!1?a:a.blue,typeof a===l?a.apply(t):a))+'</a></li>\n    <li><div class="wysihtml5-colors" data-wysihtml5-command-value="orange"></div><a class="wysihtml5-colors-title" data-wysihtml5-command="foreColor" data-wysihtml5-command-value="orange">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.colours,a=null==a||a===!1?a:a.orange,typeof a===l?a.apply(t):a))+"</a></li>\n  </ul>\n</li>\n"}),this.wysihtml5.tpl.emphasis=Handlebars.template(function(e,t,n,o,r){function i(e,t){var n,o="";return o+="btn-"+c((n=e&&e.options,n=null==n||n===!1?n:n.toolbar,n=null==n||n===!1?n:n.size,typeof n===l?n.apply(e):n))}this.compilerInfo=[4,">= 1.0.0"],n=this.merge(n,e.helpers),r=r||{};var a,s="",l="function",c=this.escapeExpression,d=this;return s+='<li>\n  <div class="btn-group">\n    <a class="btn ',a=n["if"].call(t,(a=t&&t.options,a=null==a||a===!1?a:a.toolbar,null==a||a===!1?a:a.size),{hash:{},inverse:d.noop,fn:d.program(1,i,r),data:r}),(a||0===a)&&(s+=a),s+=' btn-default" data-wysihtml5-command="bold" title="CTRL+B" tabindex="-1">'+c((a=t&&t.locale,a=null==a||a===!1?a:a.emphasis,a=null==a||a===!1?a:a.bold,typeof a===l?a.apply(t):a))+'</a>\n    <a class="btn ',a=n["if"].call(t,(a=t&&t.options,a=null==a||a===!1?a:a.toolbar,null==a||a===!1?a:a.size),{hash:{},inverse:d.noop,fn:d.program(1,i,r),data:r}),(a||0===a)&&(s+=a),s+="\n  </div>\n</li>\n"}),this.wysihtml5.tpl["font-styles"]=Handlebars.template(function(e,t,n,o,r){function i(e,t){var n,o="";return o+="btn-"+u((n=e&&e.options,n=null==n||n===!1?n:n.toolbar,n=null==n||n===!1?n:n.size,typeof n===d?n.apply(e):n))}function a(e,t){return'\n      <span class="fa fa-font"></span>\n    '}function s(e,t){return'\n      <span class="glyphicon glyphicon-font"></span>\n    '}this.compilerInfo=[4,">= 1.0.0"],n=this.merge(n,e.helpers),r=r||{};var l,c="",d="function",u=this.escapeExpression,h=this;return c+='<li class="dropdown">\n  <a class="btn btn-default dropdown-toggle ',l=n["if"].call(t,(l=t&&t.options,l=null==l||l===!1?l:l.toolbar,null==l||l===!1?l:l.size),{hash:{},inverse:h.noop,fn:h.program(1,i,r),data:r}),(l||0===l)&&(c+=l),c+='" data-toggle="dropdown">\n    ',l=n["if"].call(t,(l=t&&t.options,l=null==l||l===!1?l:l.toolbar,null==l||l===!1?l:l.fa),{hash:{},inverse:h.program(5,s,r),fn:h.program(3,a,r),data:r}),(l||0===l)&&(c+=l),c+='\n    <span class="current-font">'+u((l=t&&t.locale,l=null==l||l===!1?l:l.font_styles,l=null==l||l===!1?l:l.normal,typeof l===d?l.apply(t):l))+'</span>\n    <b class="caret"></b>\n  </a>\n  <ul class="dropdown-menu">\n    <li><a data-wysihtml5-command="formatBlock" data-wysihtml5-command-value="p" tabindex="-1">'+u((l=t&&t.locale,l=null==l||l===!1?l:l.font_styles,l=null==l||l===!1?l:l.normal,typeof l===d?l.apply(t):l))+'</a></li>\n    <li><a data-wysihtml5-command="formatBlock" data-wysihtml5-command-value="h1" tabindex="-1">'+u((l=t&&t.locale,l=null==l||l===!1?l:l.font_styles,l=null==l||l===!1?l:l.h1,typeof l===d?l.apply(t):l))+'</a></li>\n    <li><a data-wysihtml5-command="formatBlock" data-wysihtml5-command-value="h2" tabindex="-1">'+u((l=t&&t.locale,l=null==l||l===!1?l:l.font_styles,l=null==l||l===!1?l:l.h2,typeof l===d?l.apply(t):l))+'</a></li>\n    <li><a data-wysihtml5-command="formatBlock" data-wysihtml5-command-value="h3" tabindex="-1">'+u((l=t&&t.locale,l=null==l||l===!1?l:l.font_styles,l=null==l||l===!1?l:l.h3,typeof l===d?l.apply(t):l))+'</a></li>\n    <li><a data-wysihtml5-command="formatBlock" data-wysihtml5-command-value="h4" tabindex="-1">'+u((l=t&&t.locale,l=null==l||l===!1?l:l.font_styles,l=null==l||l===!1?l:l.h4,typeof l===d?l.apply(t):l))+'</a></li>\n    <li><a data-wysihtml5-command="formatBlock" data-wysihtml5-command-value="h5" tabindex="-1">'+u((l=t&&t.locale,l=null==l||l===!1?l:l.font_styles,l=null==l||l===!1?l:l.h5,typeof l===d?l.apply(t):l))+'</a></li>\n    <li><a data-wysihtml5-command="formatBlock" data-wysihtml5-command-value="h6" tabindex="-1">'+u((l=t&&t.locale,l=null==l||l===!1?l:l.font_styles,l=null==l||l===!1?l:l.h6,typeof l===d?l.apply(t):l))+"</a></li>\n  </ul>\n</li>\n"}),this.wysihtml5.tpl.html=Handlebars.template(function(e,t,n,o,r){function i(e,t){var n,o="";return o+="btn-"+u((n=e&&e.options,n=null==n||n===!1?n:n.toolbar,n=null==n||n===!1?n:n.size,typeof n===d?n.apply(e):n))}function a(e,t){return'\n        <span class="fa fa-pencil"></span>\n      '}function s(e,t){return'\n        <span class="glyphicon glyphicon-pencil"></span>\n      '}this.compilerInfo=[4,">= 1.0.0"],n=this.merge(n,e.helpers),r=r||{};var l,c="",d="function",u=this.escapeExpression,h=this;return c+='<li>\n  <div class="btn-group">\n    <a class="btn ',l=n["if"].call(t,(l=t&&t.options,l=null==l||l===!1?l:l.toolbar,null==l||l===!1?l:l.size),{hash:{},inverse:h.noop,fn:h.program(1,i,r),data:r}),(l||0===l)&&(c+=l),c+=' btn-default" data-wysihtml5-action="change_view" title="'+u((l=t&&t.locale,l=null==l||l===!1?l:l.html,l=null==l||l===!1?l:l.edit,typeof l===d?l.apply(t):l))+'" tabindex="-1">\n      ',l=n["if"].call(t,(l=t&&t.options,l=null==l||l===!1?l:l.toolbar,null==l||l===!1?l:l.fa),{hash:{},inverse:h.program(5,s,r),fn:h.program(3,a,r),data:r}),(l||0===l)&&(c+=l),c+="\n    </a>\n  </div>\n</li>\n"}),this.wysihtml5.tpl.image=Handlebars.template(function(e,t,n,o,r){function i(e,t){return"modal-sm"}function a(e,t){var n,o="";return o+="btn-"+h((n=e&&e.options,n=null==n||n===!1?n:n.toolbar,n=null==n||n===!1?n:n.size,typeof n===u?n.apply(e):n))}function s(e,t){return'\n      <span class="fa fa-file-image-o"></span>\n    '}function l(e,t){return'\n      <span class="glyphicon glyphicon-picture"></span>\n    '}this.compilerInfo=[4,">= 1.0.0"],n=this.merge(n,e.helpers),r=r||{};var c,d="",u="function",h=this.escapeExpression,f=this;return d+='<li>\n  <div class="bootstrap-wysihtml5-insert-image-modal modal fade" data-wysihtml5-dialog="insertImage">\n    <div class="modal-dialog ',c=n["if"].call(t,(c=t&&t.options,c=null==c||c===!1?c:c.toolbar,null==c||c===!1?c:c.smallmodals),{hash:{},inverse:f.noop,fn:f.program(1,i,r),data:r}),(c||0===c)&&(d+=c),d+='">\n      <div class="modal-content">\n        <div class="modal-header">\n          <a class="close" data-dismiss="modal">&times;</a>\n          <h3>'+h((c=t&&t.locale,c=null==c||c===!1?c:c.image,c=null==c||c===!1?c:c.insert,typeof c===u?c.apply(t):c))+'</h3>\n        </div>\n        <div class="modal-body">\n          <div class="form-group">\n            <input value="http://" class="bootstrap-wysihtml5-insert-image-url form-control">\n          </div> \n        </div>\n        <div class="modal-footer">\n          <a class="btn btn-default" data-dismiss="modal" data-wysihtml5-dialog-action="cancel" href="#">'+h((c=t&&t.locale,
c=null==c||c===!1?c:c.image,c=null==c||c===!1?c:c.cancel,typeof c===u?c.apply(t):c))+'</a>\n          <a class="btn btn-primary" data-dismiss="modal"  data-wysihtml5-dialog-action="save" href="#">'+h((c=t&&t.locale,c=null==c||c===!1?c:c.image,c=null==c||c===!1?c:c.insert,typeof c===u?c.apply(t):c))+'</a>\n        </div>\n      </div>\n    </div>\n  </div>\n  <a class="btn ',c=n["if"].call(t,(c=t&&t.options,c=null==c||c===!1?c:c.toolbar,null==c||c===!1?c:c.size),{hash:{},inverse:f.noop,fn:f.program(3,a,r),data:r}),(c||0===c)&&(d+=c),d+=' btn-default" data-wysihtml5-command="insertImage" title="'+h((c=t&&t.locale,c=null==c||c===!1?c:c.image,c=null==c||c===!1?c:c.insert,typeof c===u?c.apply(t):c))+'" tabindex="-1">\n    ',c=n["if"].call(t,(c=t&&t.options,c=null==c||c===!1?c:c.toolbar,null==c||c===!1?c:c.fa),{hash:{},inverse:f.program(7,l,r),fn:f.program(5,s,r),data:r}),(c||0===c)&&(d+=c),d+="\n  </a>\n</li>\n"}),this.wysihtml5.tpl.link=Handlebars.template(function(e,t,n,o,r){function i(e,t){return"modal-sm"}function a(e,t){var n,o="";return o+="btn-"+h((n=e&&e.options,n=null==n||n===!1?n:n.toolbar,n=null==n||n===!1?n:n.size,typeof n===u?n.apply(e):n))}function s(e,t){return'\n      <span class="fa fa-share-square-o"></span>\n    '}function l(e,t){return'\n      <span class="glyphicon glyphicon-share"></span>\n    '}this.compilerInfo=[4,">= 1.0.0"],n=this.merge(n,e.helpers),r=r||{};var c,d="",u="function",h=this.escapeExpression,f=this;return d+='<li>\n  <div class="bootstrap-wysihtml5-insert-link-modal modal fade" data-wysihtml5-dialog="createLink">\n    <div class="modal-dialog ',c=n["if"].call(t,(c=t&&t.options,c=null==c||c===!1?c:c.toolbar,null==c||c===!1?c:c.smallmodals),{hash:{},inverse:f.noop,fn:f.program(1,i,r),data:r}),(c||0===c)&&(d+=c),d+='">\n      <div class="modal-content">\n        <div class="modal-header">\n          <a class="close" data-dismiss="modal">&times;</a>\n          <h3>'+h((c=t&&t.locale,c=null==c||c===!1?c:c.link,c=null==c||c===!1?c:c.insert,typeof c===u?c.apply(t):c))+'</h3>\n        </div>\n        <div class="modal-body">\n          <div class="form-group">\n            <input value="http://" class="bootstrap-wysihtml5-insert-link-url form-control" data-wysihtml5-dialog-field="href">\n          </div> \n          <div class="checkbox">\n            <label> \n              <input type="checkbox" class="bootstrap-wysihtml5-insert-link-target" checked>'+h((c=t&&t.locale,c=null==c||c===!1?c:c.link,c=null==c||c===!1?c:c.target,typeof c===u?c.apply(t):c))+'\n            </label>\n          </div>\n        </div>\n        <div class="modal-footer">\n          <a class="btn btn-default" data-dismiss="modal" data-wysihtml5-dialog-action="cancel" href="#">'+h((c=t&&t.locale,c=null==c||c===!1?c:c.link,c=null==c||c===!1?c:c.cancel,typeof c===u?c.apply(t):c))+'</a>\n          <a href="#" class="btn btn-primary" data-dismiss="modal" data-wysihtml5-dialog-action="save">'+h((c=t&&t.locale,c=null==c||c===!1?c:c.link,c=null==c||c===!1?c:c.insert,typeof c===u?c.apply(t):c))+'</a>\n        </div>\n      </div>\n    </div>\n  </div>\n  <a class="btn ',c=n["if"].call(t,(c=t&&t.options,c=null==c||c===!1?c:c.toolbar,null==c||c===!1?c:c.size),{hash:{},inverse:f.noop,fn:f.program(3,a,r),data:r}),(c||0===c)&&(d+=c),d+=' btn-default" data-wysihtml5-command="createLink" title="'+h((c=t&&t.locale,c=null==c||c===!1?c:c.link,c=null==c||c===!1?c:c.insert,typeof c===u?c.apply(t):c))+'" tabindex="-1">\n    ',c=n["if"].call(t,(c=t&&t.options,c=null==c||c===!1?c:c.toolbar,null==c||c===!1?c:c.fa),{hash:{},inverse:f.program(7,l,r),fn:f.program(5,s,r),data:r}),(c||0===c)&&(d+=c),d+="\n  </a>\n</li>\n"}),this.wysihtml5.tpl.lists=Handlebars.template(function(e,t,n,o,r){function i(e,t){var n,o="";return o+="btn-"+f((n=e&&e.options,n=null==n||n===!1?n:n.toolbar,n=null==n||n===!1?n:n.size,typeof n===h?n.apply(e):n))}function a(e,t){return'\n      <span class="fa fa-list-ul"></span>\n    '}function s(e,t){return'\n      <span class="glyphicon glyphicon-list"></span>\n    '}function l(e,t){return'\n      <span class="fa fa-list-ol"></span>\n    '}function c(e,t){return'\n      <span class="glyphicon glyphicon-th-list"></span>\n    '}this.compilerInfo=[4,">= 1.0.0"],n=this.merge(n,e.helpers),r=r||{};var d,u="",h="function",f=this.escapeExpression,m=this;return u+='<li>\n  <div class="btn-group">\n    <a class="btn ',d=n["if"].call(t,(d=t&&t.options,d=null==d||d===!1?d:d.toolbar,null==d||d===!1?d:d.size),{hash:{},inverse:m.noop,fn:m.program(1,i,r),data:r}),(d||0===d)&&(u+=d),u+=' btn-default" data-wysihtml5-command="insertUnorderedList" title="'+f((d=t&&t.locale,d=null==d||d===!1?d:d.lists,d=null==d||d===!1?d:d.unordered,typeof d===h?d.apply(t):d))+'" tabindex="-1">\n    ',d=n["if"].call(t,(d=t&&t.options,d=null==d||d===!1?d:d.toolbar,null==d||d===!1?d:d.fa),{hash:{},inverse:m.program(5,s,r),fn:m.program(3,a,r),data:r}),(d||0===d)&&(u+=d),u+='\n    </a>\n    <a class="btn ',d=n["if"].call(t,(d=t&&t.options,d=null==d||d===!1?d:d.toolbar,null==d||d===!1?d:d.size),{hash:{},inverse:m.noop,fn:m.program(1,i,r),data:r}),(d||0===d)&&(u+=d),u+=' btn-default" data-wysihtml5-command="insertOrderedList" title="'+f((d=t&&t.locale,d=null==d||d===!1?d:d.lists,d=null==d||d===!1?d:d.ordered,typeof d===h?d.apply(t):d))+'" tabindex="-1">\n    ',d=n["if"].call(t,(d=t&&t.options,d=null==d||d===!1?d:d.toolbar,null==d||d===!1?d:d.fa),{hash:{},inverse:m.program(9,c,r),fn:m.program(7,l,r),data:r}),(d||0===d)&&(u+=d),u+="\n    </a>\n  </div>\n</li>\n"}),function(e){"function"==typeof define&&define.amd?define("bootstrap.wysihtml5",["jquery","wysihtml5","bootstrap","bootstrap.wysihtml5.templates","bootstrap.wysihtml5.commands"],e):e(jQuery,wysihtml5)}(function(e,t){var n=function(e,t){"use strict";var n=function(e,n,o){return t.tpl[e]?t.tpl[e]({locale:n,options:o}):void 0},o=function(n,o){this.el=n;var r=e.extend(!0,{},i,o);for(var a in r.customTemplates)t.tpl[a]=r.customTemplates[a];this.toolbar=this.createToolbar(n,r),this.editor=this.createEditor(r)};o.prototype={constructor:o,createEditor:function(n){n=n||{},n=e.extend(!0,{},n),n.toolbar=this.toolbar[0];var o=new t.Editor(this.el[0],n);if(o.composer.editableArea.contentDocument?this.addMoreShortcuts(o,o.composer.editableArea.contentDocument.body||o.composer.editableArea.contentDocument,n.shortcuts):this.addMoreShortcuts(o,o.composer.editableArea,n.shortcuts),n&&n.events)for(var r in n.events)o.on(r,n.events[r]);return o.on("beforeload",this.syncBootstrapDialogEvents),o},syncBootstrapDialogEvents:function(){var t=this;e.map(this.toolbar.commandMapping,function(e,t){return[e]}).filter(function(e,t,n){return e.dialog}).map(function(e,t,n){return e.dialog}).forEach(function(n,o,r){n.on("show",function(t){e(this.container).modal("show")}),n.on("hide",function(n){e(this.container).modal("hide"),t.composer.focus()}),e(n.container).on("shown.bs.modal",function(){e(this).find("input, select, textarea").first().focus()})})},createToolbar:function(t,o){var r=this,s=e("<ul/>",{"class":"wysihtml5-toolbar",style:"display:none"}),l=o.locale||i.locale||"en";a.hasOwnProperty(l)||(console.debug("Locale '"+l+"' not found. Available locales are: "+Object.keys(a)+". Falling back to 'en'."),l="en");var c=e.extend(!0,{},a.en,a[l]);for(var d in o.toolbar)o.toolbar[d]&&(s.append(n(d,c,o)),"html"===d&&this.initHtml(s));return s.find('a[data-wysihtml5-command="formatBlock"]').click(function(t){var n=t.delegateTarget||t.target||t.srcElement,o=e(n),i=o.data("wysihtml5-display-format-name"),a=o.data("wysihtml5-format-name")||o.html();(void 0===i||"true"===i)&&r.toolbar.find(".current-font").text(a)}),s.find('a[data-wysihtml5-command="foreColor"]').click(function(t){var n=t.target||t.srcElement,o=e(n);r.toolbar.find(".current-color").text(o.html())}),this.el.before(s),s},initHtml:function(e){var t='a[data-wysihtml5-action="change_view"]';e.find(t).click(function(n){e.find("a.btn").not(t).toggleClass("disabled")})},addMoreShortcuts:function(e,n,o){t.dom.observe(n,"keydown",function(n){var r=n.keyCode,i=o[r];if((n.ctrlKey||n.metaKey||n.altKey)&&i&&t.commands[i]){var a=e.toolbar.commandMapping[i+":null"];a&&a.dialog&&!a.state?a.dialog.show():t.commands[i].exec(e.composer,i),n.preventDefault()}})}};var r={resetDefaults:function(){e.fn.wysihtml5.defaultOptions=e.extend(!0,{},e.fn.wysihtml5.defaultOptionsCache)},bypassDefaults:function(t){return this.each(function(){var n=e(this);n.data("wysihtml5",new o(n,t))})},shallowExtend:function(t){var n=e.extend({},e.fn.wysihtml5.defaultOptions,t||{},e(this).data()),o=this;return r.bypassDefaults.apply(o,[n])},deepExtend:function(t){var n=e.extend(!0,{},e.fn.wysihtml5.defaultOptions,t||{}),o=this;return r.bypassDefaults.apply(o,[n])},init:function(e){var t=this;return r.shallowExtend.apply(t,[e])}};e.fn.wysihtml5=function(t){return r[t]?r[t].apply(this,Array.prototype.slice.call(arguments,1)):"object"!=typeof t&&t?void e.error("Method "+t+" does not exist on jQuery.wysihtml5"):r.init.apply(this,arguments)},e.fn.wysihtml5.Constructor=o;var i=e.fn.wysihtml5.defaultOptions={toolbar:{"font-styles":!0,color:!1,emphasis:{small:!0},blockquote:!0,lists:!0,html:!1,link:!0,image:!0,smallmodals:!1},parserRules:{classes:{"wysiwyg-color-silver":1,"wysiwyg-color-gray":1,"wysiwyg-color-white":1,"wysiwyg-color-maroon":1,"wysiwyg-color-red":1,"wysiwyg-color-purple":1,"wysiwyg-color-fuchsia":1,"wysiwyg-color-green":1,"wysiwyg-color-lime":1,"wysiwyg-color-olive":1,"wysiwyg-color-yellow":1,"wysiwyg-color-navy":1,"wysiwyg-color-blue":1,"wysiwyg-color-teal":1,"wysiwyg-color-aqua":1,"wysiwyg-color-orange":1},tags:{b:{},i:{},strong:{},em:{},p:{},br:{},ol:{},ul:{},li:{},h1:{},h2:{},h3:{},h4:{},h5:{},h6:{},blockquote:{},u:1,img:{check_attributes:{width:"numbers",alt:"alt",src:"url",height:"numbers"}},a:{check_attributes:{href:"url"},set_attributes:{target:"_blank",rel:"nofollow"}},span:1,div:1,small:1,code:1,pre:1}},locale:"en",shortcuts:{83:"small"}};"undefined"==typeof e.fn.wysihtml5.defaultOptionsCache&&(e.fn.wysihtml5.defaultOptionsCache=e.extend(!0,{},e.fn.wysihtml5.defaultOptions));var a=e.fn.wysihtml5.locale={}};n(e,t)}),function(e){e.commands.small={exec:function(t,n){return e.commands.formatInline.exec(t,n,"small")},state:function(t,n){return e.commands.formatInline.state(t,n,"small")}}}(wysihtml5),function(e){"function"==typeof define&&define.amd?define("bootstrap.wysihtml5.en-US",["jquery","bootstrap.wysihtml5"],e):e(jQuery)}(function(e){e.fn.wysihtml5.locale.en=e.fn.wysihtml5.locale["en-US"]={font_styles:{normal:"Normal text",h1:"Heading 1",h2:"Heading 2",h3:"Heading 3",h4:"Heading 4",h5:"Heading 5",h6:"Heading 6"},emphasis:{bold:"Bold",italic:"Italic",underline:"Underline",small:"Small"},lists:{unordered:"Unordered list",ordered:"Ordered list",outdent:"Outdent",indent:"Indent"},link:{insert:"Insert link",cancel:"Cancel",target:"Open link in new window"},image:{insert:"Insert image",cancel:"Cancel"},html:{edit:"Edit HTML"},colours:{black:"Black",silver:"Silver",gray:"Grey",maroon:"Maroon",red:"Red",purple:"Purple",green:"Green",olive:"Olive",navy:"Navy",blue:"Blue",orange:"Orange"}}});
$(document).ready(function () {
    if (!$("#EndDate").val() && !$("input[name='Frequency']:checked").val()) {
        $(".schedule_multiple_events_inputs").hide();
    }
    $(".additional-category").hide();

    $(".schedule_multiple_events").show();
    $(".schedule_multiple_events").change(function () {
        $(".schedule_multiple_events_inputs").slideToggle(200);
    });


    $("#Category1").change(
            function () {                
                if ($("#Category1 :selected").val() === "") {
                    $("#add-category2").hide();
                    $("#remove-category1").hide();
                } else {
                    var cat1Value = $("#Category1 :selected").val();
                    var cat2Value = $("#Category2 :selected").val();
                    var cat3Value = $("#Category3 :selected").val();

                    $("#Category3 option").show();
                    $("#Category2 option").show();
                    if (cat2Value) {
                        $("#Category1 option[value='" + cat1Value + "']").hide();
                        $("#Category2 option[value='" + cat1Value + "']").hide();
                    }
                    if (cat3Value) {
                        $("#Category1 option[value='" + cat3Value + "']").hide();
                        $("#Category2 option[value='" + cat3Value + "']").hide();
                    }
                    $("#Category3 option[value='" + cat1Value + "']").hide();
                    $("#Category2 option[value='" + cat1Value + "']").hide();

                    if ($("#category-div2").css("display") === "none") {
                        $("#add-category2").show();
                        $("#remove-category1").hide();
                    } else {
                        $("#add-category2").hide();
                    }
                    $("#remove-category1").parent().show();
                }

            }
        );
    $("#Category2").change(
        function () {
            if ($("#category-div3").css("display") === "none") {
                $("#add-category3").show();
            }

            if (($("#Category2").val() === "") && $("#category-div3").css("display") === "none") {
                $("#add-category3").hide();
                $("#remove-category1").hide();
            }
            if ($("#Category2").val() !== "") {
                var cat1Value = $("#Category1 :selected").val();
                var cat2Value = $("#Category2 :selected").val();
                var cat3Value = $("#Category3 :selected").val();
                $("#Category3 option").show();
                $("#Category1 option").show();
                if (cat1Value) {
                    $("#Category2 option[value='" + cat1Value + "']").hide();
                    $("#Category3 option[value='" + cat1Value + "']").hide();
                }
                if (cat3Value) {
                    $("#Category2 option[value='" + cat3Value + "']").hide();
                    $("#Category1 option[value='" + cat3Value + "']").hide();
                }
                $("#Category1 option[value='" + cat2Value + "']").hide();
                $("#Category3 option[value='" + cat2Value + "']").hide();
                $("#remove-category1").show();
            }
        }
    );

    $("#Category3").change(
        function () {
            var cat1Value = $("#Category1 :selected").val();
            var cat2Value = $("#Category2 :selected").val();
            var cat3Value = $("#Category3 :selected").val();


            $("#Category1 option").show();
            $("#Category2 option").show();
            if (cat1Value) {
                $("#Category3 option[value='" + cat1Value + "']").hide();
                $("#Category2 option[value='" + cat1Value + "']").hide();

            }
            if (cat2Value) {
                $("#Category3 option[value='" + cat2Value + "']").hide();
                $("#Category1 option[value='" + cat2Value + "']").hide();
            };
            $("#Category1 option[value='" + cat3Value + "']").hide();
            $("#Category2 option[value='" + cat3Value + "']").hide();
        }
    );

    $("#add-category2").click(
        function () {
            //            alert("Add category 2");
            var cat1Value = $("#Category1 :selected").val();
            if (cat1Value) {
                $("#Category2 option[value='" + cat1Value + "']").hide();
                $("#category-div2").show();
                $("#remove-category2").parent().show();
                $("#remove-category2").show();
                $(this).hide();
            }
            return false;
        }
    );

    $("#add-category3").click(
        function () {

            //            alert("Add category 3");
            var cat1Value = $("#Category1 :selected").val();
            var cat2Value = $("#Category2 :selected").val();
            if (cat1Value && cat2Value) {
                $("#Category3 option[value='" + cat1Value + "']").hide();
                $("#Category3 option[value='" + cat2Value + "']").hide();
                $("#category-div3").show();
                $("#remove-category3").parent().show();
                $("#remove-category3").show();
                $(this).hide();
            }
            return false;
        }
    );

    $("#remove-category3").click(
        function () {
            UpdateOptions("Category3");
            $("#category-div3").hide();
            $("#Category3").val("");
            $(this).hide();
            if ($("#Category2").val()) {
                $("#add-category3").show();
            };
            return false;
        }
    );

    $("#remove-category2").click(
       function () {
           UpdateOptions("Category2");

           if ($("#category-div3").css('display') === "none") {
               $("#category-div2").hide();
               $("#add-category2").show();
               $("#Category2").val("");
               $("#remove-category1").hide();
           }
           else if (($("#Category3").css("display") === "block") && $("#Category3").val() !== "") {
               $("#Category2").val($("#Category3").val());
               $("#Category3").val("");
               $("#category-div3").hide();
               $("#add-category3").show();
           }
           else if ($("#Category3").css("display") === "block") {
               $("#Category2").val($("#Category3").val());
               $("#category-div3").hide();
           }
           if ($("#Category2").val() === "") {
               $("#add-category3").hide();
               $("#remove-category1").hide();
           }
           if ($("#Category1 :selected").val() === "") {
               $("#add-category2").hide();
           }
           return false;
       }
   );


    $("#remove-category1").click(
       function () {
           UpdateOptions("Category1");

           if (($("#category-div3").css("display") == "block") && $("#category-div2").css("display") == "block") {
               $("#Category1").val($("#Category2").val());
               $("#Category2").val($("#Category3").val());
               $("#Category3").val("");
               $("#category-div3").hide();
               $("#add-category3").show();
           }
           else if ($("#category-div3").css("display") == "none") {
               $("#Category1").val($("#Category2").val());
               $("#Category2").val("");
               $("#category-div2").hide();
               $("#add-category2").show();
               $(this).hide();
           }
           if ($("#Category1").val() === "") {
               $("#remove-category1").hide();
               $("#add-category2").hide();
           }
           if ($("#Category2 :selected").val() === "") {
               $("#add-category3").hide();
           }
           return false;
       }
   );
   
 });

function UpdateOptions(thisId) {
    var cat1Value = $("#Category1 :selected").val();
    var cat2Value = $("#Category2 :selected").val();
    var cat3Value = $("#Category3 :selected").val();
    var thisValue = $("#" + thisId + ":selected").val();

    $("#Category3 option[value='" + thisValue + "']").show();
    $("#Category2 option[value='" + thisValue + "']").show();
    $("#Category1 option[value='" + thisValue + "']").show();

    if (cat1Value && cat1Value !== thisValue) {
        $("#Category2 option[value='" + cat1Value + "']").hide();
        $("#Category3 option[value='" + cat1Value + "']").hide();
    }

    if (cat2Value && cat2Value !== thisValue) {
        $("#Category1 option[value='" + cat2Value + "']").hide();
        $("#Category3 option[value='" + cat2Value + "']").hide();
    }
    if (cat3Value && cat3Value !== thisValue) {
        $("#Category1 option[value='" + cat3Value + "']").hide();
        $("#Category2 option[value='" + cat3Value + "']").hide();
    }
}
$(document)
    .ready(function () {
        $(".l-filters .filter-title, .l-filters .filter-inner-title, .expanding-Link-text").click(function () {
            $(this).siblings("ul").slideToggle(100, function () {
                $(this).parent("li").toggleClass("is-collapsed");
            });
        });

        var tabletWidth = (1024 - 17);

        if ($(window).width() <= tabletWidth) {
            $(".l-filters .collapsible:not(#custom-filter-li)").addClass("is-collapsed");
            $(".filters-list li.active .field-validation-error").parents("li").removeClass("is-collapsed");
        } else {
            $(".l-filters .filter:not(#custom-filter-li):not(#date-filter):not(#category-filter).collapsible").addClass("is-collapsed");
            $(".l-filters .filter:not(#custom-filter-li):not(#date-filter):not(#category-filter) .collapsible").addClass("is-collapsed");
        }

        if ($("#custom-filter-li").hasClass("customdateactive")) {
            $("#custom-filter-li").removeClass("is-collapsed");
        }

        if ($(window).width() > tabletWidth) {
            $(".filters-list li.active").each(function () {
                $(this).parents("li").removeClass("is-collapsed");
            });
        }

        
    });
$(document).ready(
    function () {
        $(".generic-list-see-more-container").hide();
        $(".generic-list-see-more").addClass("is-visible").on("click", function () {
            self = this;
            $genericMoreDiv = $(this).parent().parent().prev();
            $genericMoreDiv.slideToggle(200, function () {
                if (!$(self).hasClass("is-collapsed")) {
                    $(self).text($(self).text().replace('more', 'fewer'));
                    $(self).toggleClass("is-collapsed");
                }
                else {
                    $(self).text($(self).text().replace('fewer', 'more'));
                    $(self).toggleClass("is-collapsed");
                }
            });

            STK.Matchboxes.Init();
        });
    }()
);

if (location.protocol == "https:") {
    var dplat = "https://stockportb.logo-net.co.uk/Delivery/TBURT.php";
}
else {
    var dplat = "http://stockportb.logo-net.co.uk/Delivery/TBURT.php";
}
var strPURL = parent.document.URL;
strPURL = strPURL.replace(/&/g, "^");
strPURL = strPURL.toLowerCase();
strPURL = strPURL.replace(/</g, "-1");
strPURL = strPURL.replace(/>/g, "-2");
strPURL = strPURL.replace(/%3c/g, "-1");
strPURL = strPURL.replace(/%3e/g, "-2");
var T = new Date();
var cMS = T.getTime();
var src = dplat + '?SDTID=142&PURL=' + strPURL + '&CMS=' + cMS;
var headID = document.getElementsByTagName("head")[0];
var newScript = document.createElement('script');
newScript.type = 'text/javascript';
newScript.src = src;
headID.appendChild(newScript);
var STK = {};

STK.Matchboxes = (function () {

    var self = this;
    var matchboxes = [];

    var populate = function () {

        matchboxes.push(new Matchbox({
            parentSelector: ".matchbox-parent",
            childSelector: ".matchbox-child",
            groupsOf: 1,
            breakpoints: [
            { bp: 767, groupsOf: 2 },
            { bp: 1024, groupsOf: 3 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: ".matchbox-parent-featured",
            childSelector: ".matchbox-child",
            groupsOf: 4,
            breakpoints: [
            { bp: 1024, groupsOf: 5 }
            ]
        }));
    };

    return {
        Init: function () {
            populate();
            for (var i = 0; i < matchboxes.length; i++) {
                if ($(matchboxes[i].settings.parentSelector).length) { matchboxes[i].init(); }
            }
        }
    };
})();

STK.Matchboxes.Init();

var MultiSelector = (function () {

    var limit = 3;
    var className = '';

    var categoriesList;
    var categories;

    // select a dropdown
    var selectDropdown = function (select) {
        var links = $('.' + className + '-add', '.' + className + '-div:visible');
        var link = links[links.length - 1];
        if (allHaveValues()) {
            $(link).show();
        }
        else {
            $(link).hide();
        }

        resetHiddenValueList();
        populate();
    }

    // delete a dropdown
    var deleteDropdown = function (link) {
        $(link).parent().parent().hide();
        resetHiddenValueList();
        populate();
    }

    // click add new dropdown
    var addDropdown = function (link) {
        var newValue = $('#' + className).val() + ',';
        $('#' + className).val(newValue);
        populate();
    }

    var hideSelectedValues = function () {
        $('option[value!=""]', '.' + className + '-select').show();
        $('.' + className + '-select').each(function (index) {
            var outer = this;
            var outerIndex = index;
            var val = $(outer).val();
            $('.' + className + '-select').each(function (index) {
                var inner = this;
                var innerIndex = index;
                if (outerIndex !== innerIndex) {
                    $('option[value="' + val + '"]', inner).hide();
                }
            });
        });
    };

    var showRemoves = function () {
        var removes = $('.' + className + '-remove');
        $(removes).show();
        var loneIndex = -1;
        var hasValueCount = 0;
        var shownCategories = $('.' + className + '-div:visible');
        var selects = $('select', $(shownCategories));
        $(selects).each(function (index) {
            if ($(this).val() !== null && $(this).val() !== '') {
                if (loneIndex === -1) {
                    loneIndex = index;
                }
                hasValueCount++;
            }
        });

        if (hasValueCount === 1) {
            $($(removes)[loneIndex]).hide();
        }

        if (shownCategories.length === 1) {
            $($(removes)[0]).hide();
        }
    };

    var resetHiddenValueList = function () {
        var shownCategories = $('.' + className + '-div:visible');
        var arrayList = '';
        var comma = '';
        for (var i = 0; i < shownCategories.length; i++) {
            if (i > 0) arrayList += ',';
            var select = $('select', $(shownCategories)[i]);
            arrayList += $(select).val();
        }

        $('#' + className).val(arrayList);
    }

    var allHaveValues = function () {
        var result = true;
        $('.' + className + '-select:visible').each(function () {
            result = result && $(this).val() !== null && $(this).val() !== '';
        });

        return result;
    }

    var populate = function () {
        categoriesList = $('#' + className).val();
        categories = categoriesList.split(',');

        var max = categoriesList.length > limit ? limit : categoriesList.length;
        max = max === 0 ? max = 1 : max;
        categories = categories.slice(0, max);

        $('.' + className + '-div').hide();
        $('.' + className + '-add').hide();
        $('.' + className + '-remove').hide();
        $('.' + className + '-select').val('');
        $('option[value=""]', '.' + className + '-select').show();

        for (var i = 0; i < categories.length; i++) {
            var div = $('.' + className + '-div')[i];
            if (typeof (div) !== 'undefined') {

                $(div).show();

                var select = $('.' + className + '-select')[i];
                if (categories[i] !== null && categories[i].length > 0) {
                    $('option[value=""]', select).hide();
                    $(select).val(categories[i]);
                }

                $(select).off('change').on('change', function () {
                    selectDropdown(this);
                });
            }
        }

        hideSelectedValues();

        var shownCategories = $('.' + className + '-div:visible');

        for (var i = 0; i < shownCategories.length; i++) {

            var add = $('.' + className + '-add')[i];

            if (i === shownCategories.length - 1 && shownCategories.length < limit) {
                if (allHaveValues()) {
                    $(add).show();
                }
                $(add).off('click').on('click', function () {
                    addDropdown(this);
                });
            }

            var removeLink = $('.' + className + '-remove-link')[i];
            $(removeLink).off('click').on('click', function () {
                deleteDropdown(this);
            });

            showRemoves();
        }
    }

    return {
        Init: function (baseControlId) {
            className = baseControlId;
            limit = $('#' + className + '-limit').val();
            populate();
        }
    };
})();

$(document).ready(function () {
    $('.multi-select-control').each(function () {
        MultiSelector.Init($(this).val());
    });
});


document.documentElement.className = document.documentElement.className.replace("no-js", "js");
var mobileWidth = 767;
var tabletWidth = (1024 - 17);

$(document).ready(function () {

    $('.wysiwyg').each(function () {
        if (!$(this).parent().hasClass('stk-wysiwyg')) {
            $(this).parent().addClass('stk-wysiwyg')
        }
    });

    $('.wysiwyg').wysihtml5({
        toolbar: {
            fa: true,
            "font-styles": false,
            emphasis: {
                bold: true,
                italic: false,
                underline: false,
                small: false
            },
            "lists": true, //(Un)ordered lists, e.g. Bullets, Numbers. Default true
            "html": false, //Button which allows you to edit the generated HTML. Default false
            "link": true, //Button to insert a link. Default true
            "image": false, //Button to insert an image. Default true,
            "color": false, //Button to change color of font
            "blockquote": false //Blockquote
        },
    });


    SwapLogo();
    $(".show-search-button").click(
            function () {
                $("#mobileSearchInput").slideToggle(220);
                $(".show-search-button").toggleClass("arrow");
            }
    );

    if (!Modernizr.inputtypes.date) {
        $(".datepicker").datepicker({
            inline: true,
            dateFormat: 'dd/mm/yy'
        });

        $(".hasDatepicker").each(function () {
            var selectedDate = $(this).val();
            if (selectedDate !== null && selectedDate !== "") {
                var eventdate = new Date(selectedDate);
                $(this).val($.datepicker.formatDate('dd/mm/yy', eventdate));
            }
        });

        if ($.validator) {
            $.validator.addMethod('date',
            function (value, element) {
                if (this.optional(element)) {
                    return true;
                }

                var ok = true;
                try {
                    $.datepicker.parseDate('dd/mm/yy', value);
                }
                catch (err) {
                    ok = false;
                }
                return ok;
            });
        }
    }
});

$(window).resize(function () {
    SwapLogo();

    if ($(window).width() > tabletWidth) {
        $("#mobileSearchInput").hide();
        $(".show-search-button").removeClass("arrow");
        $('#displayRefineBy').css('display', 'block');
    }
});

$('.global-alert-close-container a')
    .on('click',
        function () {
            $(this).closest('.global-alert').hide();
        });

// Swap logo image between mobile and desktop
function SwapLogo() {
    var image = $("#header .logo-main-image");
    var logoMobile = image.attr("data-mobile-image");
    var logoDesktop = image.attr("data-desktop-image");

    if ($(window).width() <= mobileWidth) {
        image.attr("src", logoMobile);
    } else {
        image.attr("src", logoDesktop);
    }
}

function setEndDateToStartDate(endDateId, startDateId) {
    if (!Modernizr.inputtypes.date) {
        var startDate = $("#" + startDateId).val();

        if (startDate != null) {
            var endDate = $("#" + endDateId).val();
            if (endDate === "") {
                $("#" + endDateId).val(startDate);
            }
        }
    }
}



SMART.Controller = function (route, view, validator) {

    function getValidationRoute() {
        var validateAction = 'validate';
        return route.lastIndexOf("/") === (route.length - 1) ? '/' + route + validateAction : '/' + route + '/' + validateAction;
    }

    var validateQuestions = function (form, selectedInput, bypassShowValidation) {
        var deferredResult = $.Deferred();
        var validationRoute = getValidationRoute();
        $.post(validationRoute,
            form.serialize(),
            function () {
            })
            .done(function (response) {
                var isValid = validator.processValidation(response, selectedInput, view.showValidationForQuestion, bypassShowValidation);
                isValid ? view.enableNextButton() : view.disableNextButton();
                deferredResult.resolve();
            }).fail(function (xhr) {
                deferredResult.reject(xhr.status + ' [' + xhr.statusText + ']');
            });

        return deferredResult.promise();
    };

    var init = function () {
        view.showNextbutton();
        view.bindEventListeners(validateQuestions);
    };

    return {
        init: init,
        validateQuestions: validateQuestions
    };
}
SMART.Module = function () {
    return {
        init: function (route) {
            var controller = SMART.Controller(route, SMART.View(), SMART.Validator());
            $(function () {
                controller.init();
            });
        }
    };
}

SMART.Validator = function () {

    var processValidation = function (validationResults, selectedInput, callback, bypassShowValidation) {
        var invalidResutls = $.grep(validationResults,
          function (v) {
              return v.isValid === false;
          });

        var selectedResults = $.grep(validationResults,
          function (v) {
              return v.questionId === selectedInput.attr("data-questionid");
          });

        $(selectedResults).each(function () {
            if (this.isValid || (!bypassShowValidation)) {
                callback(this.questionId, this.isValid, this.message);
            }
        });

        return (invalidResutls.length === 0);
    };

    return {
        processValidation: processValidation
    };
}


SMART.View = function () {

    var disableNextButton = function () {
        var button = $(".question-button-next");
        button.prop("disabled", true);
        button.addClass("button-disabled");
    };

    var enableNextButton = function () {
        var button = $(".question-button-next");
        button.prop("disabled", false);
        button.removeClass("button-disabled");
    };

    var showNextbutton = function () {
        var scriptButton = $(".question-button-next-script");
        scriptButton.show();
    };

    var showValidationForQuestion = function (questionId, isValid, validationMessage) {
        var validationContainer = $("div[data-questionid=" + questionId + "], span[data-questionid=" + questionId + "]");
        var validationMessageSpan = validationContainer.find("span").last();
        var validatedInput = $("input[data-questionid='" + questionId + "']");
        var validationClass = "input-validation-error";

        validationMessageSpan.html(validationMessage);

        if (isValid) {
            validationContainer.hide();
            validatedInput.removeClass(validationClass);
        }
        else {
            validatedInput.addClass(validationClass);
            validationContainer.show();
        }
    };

    var bindEventListeners = function (validationCallback) {
        $("form.question-form input").change(function () {
            validationCallback($("form.question-form"), $(this));
        });
        $("form.question-form input").blur(function () {
            validationCallback($("form.question-form"), $(this));
        });
        $("form.question-form input").keyup(function (e) {
            if (e.keyCode !== 9) {
                validationCallback($("form.question-form"), $(this), true);
            }
        });
        $(document).ready(function () {
            validationCallback($("form.question-form"), $(this));
        });
    };

    return {
        showNextbutton: showNextbutton,
        bindEventListeners: bindEventListeners,
        disableNextButton: disableNextButton,
        enableNextButton: enableNextButton,
        showValidationForQuestion: showValidationForQuestion
    };

}