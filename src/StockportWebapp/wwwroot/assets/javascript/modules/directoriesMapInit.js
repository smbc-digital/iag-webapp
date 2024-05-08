define(function () {
    return {
        Init: function () {
            window.onload = function () {
                var showMap = document.getElementById("showMap");
                var hideMap = document.getElementById("hideMap");
                var mapContainer = document.getElementById("map");
                var pins = document.getElementsByClassName("pin");

                showMap.onclick = function () {
                    mapContainer.style.display = "block";
                    hideMap.style.display = "inline-block";
                    showMap.style.display = "none";
                    SetPinVisibility("inline-block");
                }

                hideMap.onclick = function () {
                    mapContainer.style.display = "none";
                    showMap.style.display = "inline-block";
                    hideMap.style.display = "none";
                    SetPinVisibility("none");
                }

                function SetPinVisibility(value) {
                    for (let i = 0; i < pins.length; i++) {
                        pins[i].style.display = value;
                    }
                }
            }
        }
    };
});