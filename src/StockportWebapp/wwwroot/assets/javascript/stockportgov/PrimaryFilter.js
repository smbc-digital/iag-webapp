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
        $("#filterButton").click();
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
                                var address = results[0].address_components;
                                var lat = results[0].geometry.location.lat();
                                var long = results[0].geometry.location.lng();

                                $("#latitude, #latitudeMobile").val(lat);
                                $("#longitude, #longitudeMobile").val(long);
                                $("#location, #locationMobile").val(extractFromAdress(results[0].address_components, "route") + " " + extractFromAdress(results[0].address_components, "postal_code"));
                            } else {
                                alert("Unable to get your current location for the following reason: " + status);
                            }
                        });
                });
                return false;
            });

            $("#btnLocation").click(function (event) {
                event.preventDefault();
                var address = $("#location").val(); // THIS IS WHY IT ISN'T WORKING
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({ 'address': address+ ", UK"}, function (results, status) {
                    if (address !== "") {
                        if (status === google.maps.GeocoderStatus.OK) {
                            var address1 = results[0].address_components;                           
                            $("#postcode, #postcodeMobile").val(extractFromAdress(results[0].address_components, "route") + " " + extractFromAdress(address1, "postal_code"));
                            $("#latitude, #latitudeMobile").html(results[0].geometry.location.lat());
                            $("#longitude, #longitudeMobile").html(results[0].geometry.location.lng());
                            UpdateLocationFieldSize();
                            $("#getLocation, #getLocationMobile").hide();
                        } else {
                            alert("Geocode was not successful for the following reason: " + status);
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

$("#btnLocationMobile").click(function (event) {
    event.preventDefault();
    var address = $("#locationMobile").val(); // THIS IS WHY IT ISN'T WORKING
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'address': address+ ", UK"}, function (results, status) {
        if (address !== "") {
            if (status === google.maps.GeocoderStatus.OK) {
                var address1 = results[0].address_components;                           
                $("#postcode, #postcodeMobile").val(extractFromAdress(results[0].address_components, "route") + " " + extractFromAdress(address1, "postal_code"));
                $("#latitude, #latitudeMobile").html(results[0].geometry.location.lat());
                $("#longitude, #longitudeMobile").html(results[0].geometry.location.lng());
                UpdateLocationFieldSize();
                $("#getLocation, #getLocationMobile").hide();
            } else {
                alert("Geocode was not successful for the following reason: " + status);
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