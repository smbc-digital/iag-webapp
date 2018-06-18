define(["jquery", "refinebybar"], function ($, refinebybar) {

    var locationDefaults = {
        Name: "Stockport",
        Latitude: 53.405817,
        Longitude: -2.158046,
        CurrentLocationError: "We couldn't find your current location -- please check the location settings on your device.",
        LocationLookupError: "We couldn't find this location -- please check the location and try again.",
        NoLocationError: "No location entered -- please enter a location and try again."
    };

    var location = {
        Name: locationDefaults.Name,
        Latitude: locationDefaults.Latitude,
        Longitude: locationDefaults.Longitude,
        HasBeenUpdated: false,
        ShowLocationError: function (error) {
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
            var controlId = $('#locationControlId').val();
            if ($("#" + controlId).length) {
                $("#" + controlId).val(this.Name);
            }
            $("#address").val(this.Name);

            var longitudeControlId = $('#longitudeControlId').val();
            longitudeControlId = typeof (longitudeControlId) === 'undefined' || longitudeControlId === '' ? 'longitude' : longitudeControlId;
            if ($("#" + longitudeControlId).length) {
                $("#" + longitudeControlId).val(this.Longitude);
            }

            var latitudeControlId = $('#latitudeControlId').val();
            latitudeControlId = typeof (latitudeControlId) === 'undefined' || latitudeControlId === '' ? 'latitude' : latitudeControlId;
            if ($("#" + latitudeControlId).length) {
                $("#" + latitudeControlId).val(this.Latitude);
            }

            if ($('#callback').val() === '') {
                updateLocationFieldSize();
                $("#getLocation").hide();
            }
            else {
                eval($('#callback').val());
            }
        },
        SetLocation: function (name, latitude, longitude) {
            this.Name = name;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.HasBeenUpdated = true;
        },
        GetLocationInputValue: function () {
            return $("#location-autocomplete").val();
        },
        SetLocationInputValue: function (value) {
            $("#location-autocomplete").val(value);
        }
    };

    var extractFromAdress = function (components, type) {
        for (var i = 0; i < components.length; i++)
            for (var j = 0; j < components[i].types.length; j++)
                if (components[i].types[j] === type) return components[i].long_name;
        return "";
    }

    var updateLocationFieldSize = function () {
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

    var locationLookupNonAutocomplete = function () {
        var address = location.GetLocationInputValue();
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': address + ", UK" }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                if ($('#callback').val() === '') {
                    location.SetLocation(buildLocation(results[0].address_components), results[0].geometry.location.lat(), results[0].geometry.location.lng());
                    location.SetLocationValues();
                    location.HideLocationError();
                }
                else {
                    eval($('#callback').val());
                }
            } else {
                location.ShowLocationError(locationDefaults.LocationLookupError);
            }
        });
    };

    var setSelectBoxes = function () {
        if ($(window).width() > 767) {
            $("#hiddenSelectCategory").html("<option>" + $("#selectCategory").find(":selected").text() + "</option>");
            $("#selectCategory").width($("#hiddenSelectCategory").width());
            $("#hiddenSelectOrder").html("<option>" + $("#selectOrder").find(":selected").text() + "</option>");
            $("#selectOrder").width($("#hiddenSelectOrder").width());
            $("#hiddenSelectLocation").html("<option>" + $("#postcode").val() + "</option>");
            $("#postcode").width($("#hiddenSelectLocation").width());
        }
    };

    var init = function () {
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

        $(".expandable-toggle").keypress(function (event) {
            event.preventDefault();

            if (event.which === 13 ||event.which === 32) {
                $(".expandable-box", $(this).parent()).toggle();
            }
        });

        if ($('.location-search-input-autoset').length) {
            var addressVal = $('#address').val();
            if (typeof (addressVal) === 'undefined' || !addressVal.length) {
                addressVal = $('#location').val();
            }
            $('.location-search-input-autoset').val(addressVal);
        }

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

                                location.SetLocationInputValue(jointLocation);
                                location.SetLocation(jointLocation, results[0].geometry.location.lat(), results[0].geometry.location.lng());
                                location.HideLocationError();
                            }
                            else {
                                location.ShowLocationError(locationDefaults.CurrentLocationError);
                            }
                        });
                },
                function () {
                    location.ShowLocationError(locationDefaults.CurrentLocationError);
                },
                { maximumAge: 10000, timeout: 6000, enableHighAccuracy: true });
            return false;
        });

        $(".events-refine-by-bar-container #allLocations").click(function () {
            $('#location').val('');
            $('#longitude').val('');
            $('#latitude').val('');
            $('.location-search-input').val('');
            location.SetLocation('', '', '');
        });

        $(".group-startpage-image #allLocations").click(function () {
            $('#location').val('Stockport');
            $('#longitude').val('-2.158046');
            $('#latitude').val('53.405817');
            $('.location-search-input').val('Stockport');
            location.SetLocation('Stockport', '53.405817', '-2.158046');
        });

        // use location click
        $("#btnLocationAutoComplete").click(function (event) {
            locationAutoComplete(event);
        });

        $("#btnLocationAutoComplete").keypress(function (event) {
            if (event.which == 13) {
                locationAutoComplete(event);
            } 
        });

        function locationAutoComplete(event) {
            event.preventDefault();
            var address = location.GetLocationInputValue();
            if (address === "") {
                location.ShowLocationError(locationDefaults.NoLocationError);
                return false;
            }
            // check if location values have been set via autocomplete
            if (location.HasBeenUpdated === true) {
                location.SetLocationValues();
                location.HideLocationError();
            } else {
                // perform a lookup on the location in the textbox
                locationLookupNonAutocomplete(); 
            }

            // after all is done set HasBeenUpdated to false so you can choose a location again
            location.HasBeenUpdated = false;
        }

        if ($('.location-search-input-autoset').length) {
            addValidationErrorToMeetingLocationField();

            $('.location-search-input-autoset').on('change', function () {
                $("#address").val('');

                var latitudeControlId = $('#latitudeControlId').val();
                latitudeControlId = typeof (latitudeControlId) === 'undefined' || latitudeControlId === '' ? 'latitude' : latitudeControlId;
                if ($("#" + latitudeControlId).length) {
                    $("#" + latitudeControlId).val(location.Latitude);
                }

                var longitudeControlId = $('#longitudeControlId').val();
                longitudeControlId = typeof (longitudeControlId) === 'undefined' || longitudeControlId === '' ? 'longitude' : longitudeControlId;
                if ($("#" + longitudeControlId).length) {
                    $("#" + longitudeControlId).val(location.Longitude);
                }
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
            // Set the default bounds to Stockport, this is a suggestion to google to look within these bounds
            var defaultBounds = new google.maps.LatLngBounds(
                new google.maps.LatLng(53.3435829465, -2.2532923571),
                new google.maps.LatLng(53.4410365421, -2.0231358656));

            var options = {
                bounds: defaultBounds,
                // the country to return results, the bounds above seemed to also be needed and not just this though
                // this isn't 100% though and is just a suggestion to first look in gb
                componentRestrictions: { country: 'gb' }
            };

            var searchBox = new google.maps.places.Autocomplete(document.getElementById('location-autocomplete'), options);

            // Listen for the event fired when the user selects a prediction and retrieve more details for that place.
            google.maps.event.addListener(searchBox, 'place_changed', function () {
                var place = searchBox.getPlace();

                if (place == null) {
                    return;
                }

                var autocompleteName = place.name;

                if ($('.location-search-input-autoset').length) {

                    if (typeof (place.formatted_address) !== 'undefined') {
                        autocompleteName = place.formatted_address.replace(', UK', '').replace(', United Kingdom', '');
                        if (autocompleteName.indexOf(place.name) < 0) {
                            autocompleteName = place.name + ', ' + autocompleteName;
                        }
                    }

                    $('#location-autocomplete').removeClass('input-validation-error');
                    $("[data-valmsg-for='Address']").hide();
                }

                location.SetLocation(autocompleteName, place.geometry.location.lat(), place.geometry.location.lng());
                location.HideLocationError();

                if ($('.location-search-input-autoset').length) {
                    location.SetLocationValues();
                }
            });
        }
    };

    setSelectBoxes();

    return {
        Init: function () {
            init();
            $(window).resize(function () {
                setSelectBoxes();
            });
        },
        BuildLocation: buildLocation
    }
});
