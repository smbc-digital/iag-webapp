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
        $("#sortBy").val(orderby);
        $("#filterButtonMobile").click();
    });
});

$(window).resize(function () {
    $("#hiddenSelectCategory").html("<option>" + $("#selectCategory").find(":selected").text() + "</option>");
    $("#selectCategory").width($("#hiddenSelectCategory").width());
    $("#hiddenSelectOrder").html("<option>" + $("#selectOrder").find(":selected").text() + "</option>");
    $("#selectOrder").width($("#hiddenSelectOrder").width());
    $("#hiddenSelectLocation").html("<option>" + $("#postcode").val() + "</option>");
    $("#postcode").width($("#hiddenSelectLocation").width());
});

$(document).ready(
    function () {
        $("#postcode, #postcodeMobile, #postcode-autocomplete").click(
            function () {
                $("#getLocation, #getLocationMobile").toggle();
            }
        );

        $("#currentLocation, #currentLocationMobile").click(function () {
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

                        $("#location, #locationMobile").val(jointLocation);
                    }
                    else {
                        alert("We couldn't find your current location.");
                    }
                });
            }, function() {
                alert("We couldn't find your current location -- please check the location settings on your device.");
            });
            return false;
        });

        $("#btnLocation").click(function (event) {
            event.preventDefault();
            var address = $("#location").val();
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'address': address + ", UK" }, function (results, status) {
                if (address !== "") {
                    if (status === google.maps.GeocoderStatus.OK) {
                        var street = extractFromAdress(results[0].address_components, "route");
                        var postcode = extractFromAdress(results[0].address_components, "postal_code");
                        var city = extractFromAdress(results[0].address_components, "locality");
                        var jointLocation = (street + " " + postcode + " " + city).trim();

                        $("#postcode, #postcodeMobile").val(jointLocation);
                        $("#latitude, #latitudeMobile").val(results[0].geometry.location.lat());
                        $("#longitude, #longitudeMobile").val(results[0].geometry.location.lng());
                        UpdateLocationFieldSize();
                        $("#getLocation, #getLocationMobile").hide();
                    } else {
                        alert("We couldn't find that location.");
                    }
                } else {
                    $("#postcode, #postcodeMobile").val("Stockport");
                    $("#latitude #postcodeMobile").val("53.405817");
                    $("#longitude #latitudeMobile").val("-2.158046");
                    UpdateLocationFieldSize();
                    $("#getLocation, #getLocationMobile").hide();
                }
            });
        });

        $("#btnLocationAutoComplete").click(function (event) {
            event.preventDefault();
            $("#postcode, #postcodeMobile").val(autocompleteName);
            $("#latitude, #latitudeMobile").val(autocompleteLocationLatitude);
            $("#longitude, #longitudeMobile").val(autocompleteLocationLongitude);
            UpdateLocationFieldSize();
            $("#getLocation, #getLocationMobile").hide();
        });


        // set the default values
        var autocompleteName = "Stockport";
        var autocompleteLocationLatitude = 53.405817;
        var autocompleteLocationLongitude = -2.158046;

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
        var input = document.getElementById('location-autocomplete');
        var searchBox = new google.maps.places.SearchBox(input, options);

        // Listen for the event fired when the user selects a prediction and retrieve more details for that place.
        searchBox.addListener('places_changed', function () {
            var places = searchBox.getPlaces();

            if (places.length == 0) {
                return;
            }

            autocompleteName = places[0].name;
            autocompleteLocationLatitude = places[0].geometry.location.lat();
            autocompleteLocationLongitude = places[0].geometry.location.lng();
        });

        $("#btnLocationMobile").click(function (event) {
            event.preventDefault();
            var address = $("#locationMobile").val();
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'address': address + ", UK" }, function (results, status) {
                if (address !== "") {
                    if (status === google.maps.GeocoderStatus.OK) {
                        var street = extractFromAdress(results[0].address_components, "route");
                        var postcode = extractFromAdress(results[0].address_components, "postal_code");
                        var city = extractFromAdress(results[0].address_components, "locality");
                        var jointLocation = (street + " " + postcode + " " + city).trim();

                        $("#postcode, #postcodeMobile").val(jointLocation);
                        $("#latitude, #latitudeMobile").val(results[0].geometry.location.lat());
                        $("#longitude, #longitudeMobile").val(results[0].geometry.location.lng());
                        UpdateLocationFieldSize();
                        $("#getLocation, #getLocationMobile").hide();
                    } else {
                        alert("We couldn't find that location.");
                    }
                } else {
                    $("#postcode, #postcodeMobile").val("Stockport");
                    $("#latitude #postcodeMobile").val("53.405817");
                    $("#longitude #latitudeMobile").val("-2.158046");
                    UpdateLocationFieldSize();
                    $("#getLocation, #getLocationMobile").hide();
                }
            });
        });
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