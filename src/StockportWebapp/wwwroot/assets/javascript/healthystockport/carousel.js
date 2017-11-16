define(["jquery"], function ($) {
    $(".carousel a").css("display", "block");
    $(".carousel div").css("display", "block");
    $(".carousel").slick({
        arrows: true,
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        dots: true,
        autoplay: false,
        autoplaySpeed: 5000,
        focusOnChange: true
    });
});