define(["jquery", "utils", "primaryfilter"], function ($, utils, primaryfilter) {

    var handleVolunteering = function (input) {
        if ($(input).is(':checked') === true) {
            $("#volunteering-text").show();
        }
        else {
            $("#volunteering-text").hide();
        }
    };


    var handleDonations = function (input) {
        if ($(input).is(':checked') === true) {
            $("#donations-text").show();
        }
        else {
            $("#donations-text").hide();
        }
    };

    var changeText = function() {
        $('.upToDate').click(function () {
            var $this = $(this);
        var postUrl = window.location.href + "/up-to-date";
            postUrl = postUrl.replace(/#/g, '');
                $.post( postUrl,
                function (data) {
                 if (data == "200") {
                        $("#ConfirmedUpToDate").val(true);
                        $this.toggleClass('.upToDate');
                        if ($this.hasClass('.upToDate')) {
                            $this.text('Thanks for letting us know');
                        } 
                    } 

                });
            return false;
        });


    }


    var handleAdditionalInformation = function (input) {
        if ($(input).is(':checked') === true) {
            $("#additional-information-text").show();
        }
        else {
            $("#additional-information-text").hide();
        }
    };

    var init = function () {


         changeText();
        if ($(window).width() <= utils.MobileWidth) {
            $("#edit-search").hide();
            $(".result-arrow").addClass("result-search-down-arrow");

            $("#open-edit-search").click(function () {
                $("#edit-search").show();
                $(".result-arrow").toggleClass("result-search-down-arrow");
                $(".result-arrow").toggleClass("result-search-up-arrow");

                $(".result-search-down-arrow").parent().click(function () {
                    $("#edit-search").show();
                });
                $(".result-search-up-arrow").parent().click(function () {
                    $("#edit-search").hide();
                });
            });

            $(".result-search-up-arrow").parent().click(function () {
                $("#edit-search").hide();
            });
        }

        $(".print-this").click(function () {
            window.print();
        });

        handleVolunteering($("#volunteering-needed"));
        $("#volunteering-needed").on("change", function () {
            handleVolunteering(this);
        });

        handleDonations($("#donations-needed"));
        $("#donations-needed").on("change", function () {
            handleDonations(this);
        });

        handleAdditionalInformation($("#additional-information"));
        $("#additional-information").on("change", function () {
            handleAdditionalInformation(this);
        });

        $(".remove-favourite a,.add-favourite a").attr("href", "javascript:void(0)");
    };

    $("#currentLocationgroup").click(function () {
        var latitude = "";
        var logitude = "";
        var LocationLookupError = "We couldn't find this location -- please check the location and try again.";
        var CurrentLocationError = "We couldn't find your current location -- please check the location settings on your device.";
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
                            var jointLocation = primaryfilter.BuildLocation(results[0].address_components);
                            var url = window.location.href;
                            latitude = position.coords.latitude;
                            longitude = position.coords.longitude;

                            var fullurl = url + "/results?Category=all&latitude=" + latitude + "&longitude=" + longitude + "&Location=" + jointLocation + "&Order=Nearest";
                            window.location.href = fullurl;
                        }
                        else {
                            ShowLocationError(LocationLookupError);
                        }
                    });               
            },
            function () {
                ShowLocationError(CurrentLocationError);
            },
            { maximumAge: 10000, timeout: 6000, enableHighAccuracy: true });
        return false;
    });

    var ShowLocationError = function (errorMessage) {
        $("#currentLocationgrouperror").css("border", "1px solid #C83725");
        $("#currentLocationgrouperror").html(errorMessage);
        $(".group-homepage-container .form-field-validation-error").show();
        $("#currentLocationgrouperror").show();
        $("#currentLocationgroup").css("margin-bottom", "10px");
        $("#currentLocationgroup .group-block-content").addClass("group-block-content-margin");
    }

    return {
        Init: init
    }
});





