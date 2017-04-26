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
        $("#postcode, #postcodeMobile").click(
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
                        var lat = results[0].geometry.location.lat();
                        var long = results[0].geometry.location.lng();
                        var street = extractFromAdress(results[0].address_components, "route");
                        var postcode = extractFromAdress(results[0].address_components, "postal_code");
                        var city = extractFromAdress(results[0].address_components, "locality");
                        var jointLocation = (street + " " + postcode + " " + city).trim();

                        $("#latitude, #latitudeMobile").val(lat);
                        $("#longitude, #longitudeMobile").val(long);
                        $("#postcode, #postcodeMobile").val(jointLocation);
                    }
                    else {
                        alert("We couldn't find your current location.");
                    }
                });
            }, function() {
                alert("An error has occurred -- please check your device's location settings.");
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
                    $("#latitude #postcodeMobile").val("53.40581278523235");
                    $("#longitude #latitudeMobile").val("-2.158041000366211");
                    UpdateLocationFieldSize();
                    $("#getLocation, #getLocationMobile").hide();
                }
            });
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
                    $("#latitude #postcodeMobile").val("53.40581278523235");
                    $("#longitude #latitudeMobile").val("-2.158041000366211");
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