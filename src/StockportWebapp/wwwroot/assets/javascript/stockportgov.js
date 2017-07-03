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
var matchboxFeaturedItemsShowcase = new Matchbox({
    parentSelector: ".group-listing-container",
    childSelector: ".group-li .group-card",
    groupsOf: 1,
    breakpoints: [
    { bp: 767, groupsOf: 2 },
    { bp: 1024, groupsOf: 3 }
    ]
});

$(document).ready(
    function () {
        if ($(".group-listing-container").length) { matchboxFeaturedItemsShowcase.init(); }
    }()
);
var matchboxFeaturedItemsShowcase = new Matchbox({
    parentSelector: ".featured-category-items-wrapper",
    childSelector: '.featured-group-category',
    groupsOf: 3,
    breakpoints: [
    { bp: 767, groupsOf: 5 },
    { bp: 1024, groupsOf: 5 }
    ]
});

$(document).ready(
    function () {
        if ($(".featured-category-items-wrapper").length) { matchboxFeaturedItemsShowcase.init(); }
    }()
);

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
});

$(window).resize(function () {
    // only apply on tablet and desktop
    var mobileWidth = 767;

    if ($(window).width() > mobileWidth) {
        $("#hiddenSelectCategory").html("<option>" + $("#selectCategory").find(":selected").text() + "</option>");
        $("#selectCategory").width($("#hiddenSelectCategory").width());
        $("#hiddenSelectOrder").html("<option>" + $("#selectOrder").find(":selected").text() + "</option>");
        $("#selectOrder").width($("#hiddenSelectOrder").width());
        $("#hiddenSelectLocation").html("<option>" + $("#postcode").val() + "</option>");
        $("#postcode").width($("#hiddenSelectLocation").width());
    }
});


$(document).ready(
    function () {
        // set the default values
        var autocompleteName = "Stockport";
        var autocompleteLocationLatitude = 53.405817;
        var autocompleteLocationLongitude = -2.158046;

        // open the "location search" box
        $("#postcode").click(
            function () {
                $("#getLocation").toggle();
            }
        );

        if ($('.location-search-input-autoset').length) {
            $('.location-search-input-autoset').val($('#address').val());
        }

        // get current location
        $("#currentLocation").click(function () {
            navigator.geolocation.getCurrentPosition(function (position) {
                var geocoder = new google.maps.Geocoder();
                var latLng = new google.maps.LatLng(
                    position.coords.latitude,
                    position.coords.longitude);
                geocoder.geocode({
                    'latLng': latLng
                },
                function (results, status) {
                    if (status === google.maps.GeocoderStatus.OK) {
                        var street = extractFromAdress(results[0].address_components, "route");
                        var postcode = extractFromAdress(results[0].address_components, "postal_code");
                        var city = extractFromAdress(results[0].address_components, "locality");
                        var jointLocation = (street + " " + postcode + " " + city).trim();

                        $("#location, #location-autocomplete").val(jointLocation);
                        autocompleteName = jointLocation;
                        autocompleteLocationLatitude = results[0].geometry.location.lat();
                        autocompleteLocationLongitude = results[0].geometry.location.lng();
                    }
                    else {
                        alert("We couldn't find your current location.");
                    }
                });
            }, function () {
                alert("We couldn't find your current location -- please check the location settings on your device.");
            });
            return false;
        });

        // get location lookup, non auto complete
        $("#btnLocation").click(function (event) {
            event.preventDefault();
            var address = $("#location").val();
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'address': address + ", UK" }, function (results, status) {
                if (address !== "") {
                    if (status === google.maps.GeocoderStatus.OK) {
                        UpdateLocation(results);
                    } else {
                        alert("We couldn't find that location.");
                    }
                } else {
                    $("#postcode").val("Stockport");
                    $("#latitude").val("53.405817");
                    $("#longitude").val("-2.158046");
                    UpdateLocationFieldSize();
                    $("#getLocation").hide();
                }
            });
        });

        function UpdateLocation(results) {
            var street = extractFromAdress(results[0].address_components, "route");
            var postcode = extractFromAdress(results[0].address_components, "postal_code");
            var city = extractFromAdress(results[0].address_components, "locality");
            var jointLocation = (street + " " + postcode + " " + city).trim();

            $("#postcode").val(jointLocation);
            $("#latitude").val(results[0].geometry.location.lat());
            $("#longitude").val(results[0].geometry.location.lng());
            UpdateLocationFieldSize();
            $("#getLocation").hide();
        }

        // auto complete
        $("#btnLocationAutoComplete").click(function (event) {
            event.preventDefault();
            setLocationValues();
        });

        var setLocationValues = function () {
            $("#postcode").val(autocompleteName);
            $("#address").val(autocompleteName);
            $("#latitude").val(autocompleteLocationLatitude);
            $("#longitude").val(autocompleteLocationLongitude);
            UpdateLocationFieldSize();
            $("#getLocation").hide();
        };

        if ($('.location-search-input-autoset').length) {

            var addValidationErrorToMeetingLocationField = function () {
                if ($("#address").hasClass('input-validation-error')) {
                    $('#location-autocomplete').addClass('input-validation-error');
                }
            }

            addValidationErrorToMeetingLocationField();

            $('.location-search-input-autoset').on('change', function () {
                $("#address").val('');
                $("#latitude").val(autocompleteLocationLatitude);
                $("#longitude").val(autocompleteLocationLongitude);
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

        // only run of the auto complete is toggled on
        if ($(".primary-filter-form-autocomplete").length) {
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

            // desktop
            var input = document.getElementById('location-autocomplete');
            var searchBox = new google.maps.places.SearchBox(input, options);

            // Listen for the event fired when the user selects a prediction and retrieve more details for that place.
            searchBox.addListener('places_changed', function () {

                var places = searchBox.getPlaces();

                if (places.length == 0) {
                    return;
                }

                autocompleteName = places[0].name;
                if ($('.location-search-input-autoset').length) {
                    if (typeof(places[0].formatted_address) == 'undefined') {
                        autocompleteName = places[0].formatted_address.replace(', UK', '').replace(', United Kingdom', '');
                        if (autocompleteName.indexOf(places[0].name) < 0) {
                            autocompleteName = places[0].name + ', ' + autocompleteName;
                        }
                    }

                    $('#location-autocomplete').removeClass('input-validation-error');
                    $("[data-valmsg-for='Address']").hide();
                }

                autocompleteLocationLatitude = places[0].geometry.location.lat();
                autocompleteLocationLongitude = places[0].geometry.location.lng();

                if ($('.location-search-input-autoset').length) {
                    setLocationValues();
                }
            });
        }
    });

function extractFromAdress(components, type) {
    for (var i = 0; i < components.length; i++)
        for (var j = 0; j < components[i].types.length; j++)
            if (components[i].types[j] == type) return components[i].long_name;
    return "";
}

function UpdateLocationFieldSize() {
    $("#hiddenSelectLocation").html("<option>" + $("#postcode").val() + "</option>");
    $("#postcode").width($("#hiddenSelectLocation").width());
};
var matchboxFeaturedItemsShowcase = new Matchbox({
    parentSelector: ".featured-items-wrapper",
    childSelector: '.featured-topic',
    groupsOf: 3,
    breakpoints: [
    { bp: 767, groupsOf: 4 },
    { bp: 1024, groupsOf: 5 }
    ]
});

$(document).ready(
    function () {
        if ($(".featured-items-wrapper").length) { matchboxFeaturedItemsShowcase.init(); }
    }()
);
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
var matchboxTopicsHomepagePrimary = new Matchbox({
    parentSelector: '.featured-topics .featured-topics-primary .featured-topic-list',
    childSelector: '.featured-topic',
    groupsOf: 3,
    breakpoints: [
    { bp: 767, groupsOf: 3 },
    { bp: 1024, groupsOf: 3 }
    ]
});

var matchboxTopicsHomepageSecond = new Matchbox({
    parentSelector: '.featured-topics .primary-topics .featured-topic-list',
    childSelector: '.featured-topic',
    groupsOf: 5,
    breakpoints: [
    { bp: 767, groupsOf: 5 },
    { bp: 1024, groupsOf: 5 }
    ]
});

var matchboxTopicsHomepageMore = new Matchbox({
    parentSelector: '.featured-topics #more-topics .featured-topic-list.hide-on-mobile',
    childSelector: '.featured-topic',
    groupsOf: 5,
    breakpoints: [
    { bp: 767, groupsOf: 5 },
    { bp: 1024, groupsOf: 5 }
    ]
});

var matchboxTopicsHomepageMoreMobile = new Matchbox({
    parentSelector: '.featured-topics #more-topics .featured-topic-list.hide-on-desktop',
    childSelector: '.featured-topic',
    groupsOf: 5,
    breakpoints: [
    { bp: 767, groupsOf: 4 },
    { bp: 1024, groupsOf: 4 }
    ]
});

var matchboxNewsHomepage = new Matchbox({
    parentSelector: '.homepage-news-items',
    childSelector: '.homepage-news-item',
    groupsOf: 2
});

var $seeMoreServicesButton = $("#see-more-services, #see-more-services-mobile");
var $moreFeaturedTopicsDiv = $("#more-topics");

$(document).ready(
    function () {
        if ($(".featured-topics .featured-topics-primary").length) { matchboxTopicsHomepagePrimary.init(); }
        if ($(".featured-topics .primary-topics").length) { matchboxTopicsHomepageSecond.init(); }
        if ($(".featured-topics #more-topics").length) { matchboxTopicsHomepageMore.init(); }
        if ($(".featured-topics #more-topics").length) { matchboxTopicsHomepageMoreMobile.init(); }
        if ($(".homepage-news-items").length) { matchboxNewsHomepage.init(); }

        $moreFeaturedTopicsDiv.hide();
        $seeMoreServicesButton.addClass("is-visible");
    }()
);

$seeMoreServicesButton.on(
    "click", function () {
        $moreFeaturedTopicsDiv.slideToggle(200, function () {
            if (!$seeMoreServicesButton.hasClass("is-collapsed")) {
                $seeMoreServicesButton.text("See fewer services");
                $seeMoreServicesButton.toggleClass("is-collapsed");

            }
            else {
                $seeMoreServicesButton.text("See more services");
                $seeMoreServicesButton.toggleClass("is-collapsed");
            }
        }
        );
        if ($(".featured-topics #more-topics").length) { matchboxTopicsHomepageMore.init(); }
        if ($(".featured-topics #more-topics").length) { matchboxTopicsHomepageMoreMobile.init(); }
    }
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

var matchboxPrimary = new Matchbox({
    parentSelector: ".l-page-content .nav-card-list",
    childSelector: ".nav-card .nav-card-item",
    groupsOf: 1,
    breakpoints: [
    { bp: 1024, groupsOf: 3 }
    ]
});

var matchboxEventCards = new Matchbox({
    parentSelector: ".event-listing-container",
    childSelector: ".event-card-information",
    groupsOf: 1,
    breakpoints: [
    { bp: 767, groupsOf: 2 },
    { bp: 1024, groupsOf: 3 }
    ]
});

$(document).ready(function () {
    SwapLogo();
    $(".show-search-button").click(
            function () {
                $("#mobileSearchInput").slideToggle(220);
                $(".show-search-button").toggleClass("arrow");
            }
    );

   

    if ($(".l-page-content .nav-card-list").length) {
        matchboxPrimary.init();
    }

    if ($(".event-listing-container").length) {
        matchboxEventCards.init();
    }

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

