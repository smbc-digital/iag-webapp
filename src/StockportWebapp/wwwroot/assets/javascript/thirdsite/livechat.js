define(["jquery"], function ($) {

    var init = function () {
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
    };

    return {
        Init: function () {
            init();
        }
    }
});


