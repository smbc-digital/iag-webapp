$(".carousel a").css("display", "block");
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
