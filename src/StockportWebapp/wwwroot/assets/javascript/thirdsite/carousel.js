﻿define(["jquery", "slick"], function ($, slick) {

    var init = function () {
        $(".carousel a").css("display", "block");
        $(".carousel div").css("display", "block");
        $(document).ready(
                function () {
                    $(".carousel").slick( 
                     {
                         arrows: true,
                         infinite: true,
                         slidesToShow: 1,
                         slidesToScroll: 1,
                         dots: true,
                         autoplay: true,
                         autoplaySpeed: 5000
                     });
                }
        );
    };

    return {
        Init: init
    }
});
