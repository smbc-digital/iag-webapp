// Launching fullscreen overlays
$('.launch-fullscreen-overlay[data-overlay]').on( 'click', function() {
   var overlayData = $(this).data('overlay'),
       $target = $('#'+overlayData),
       triggerEvent = 'launch-overlay-'+overlayData;
   $('body').addClass('overlay-open no-scroll').trigger(triggerEvent);

   if ( ! $('body').hasClass('container-layout-free') ) {
      var $wrapAll = $('#container .wrap-all').eq(0),
          width = $wrapAll.outerWidth(),
          left = $wrapAll.offset().left,
          css = { width: width, left: left, };
      $target.css(css);
      $target.find('.button-close').css(css);
   }

   $target.addClass('is-active');
   setTimeout( function() {
      $target.addClass('is-open');
   }, 10 );
});

// Closing Fullscreen Overlays
$('.fullscreen-overlay .button-close').on( 'click', function() {
   var $overlay = $(this).parent().trigger('close');
   $overlay.removeClass('is-open');
   setTimeout( function() {
      $overlay.removeClass('is-active');
      $('body').removeClass('no-scroll overlay-open');
   }, 500);
});

$('.alert-close a')
    .on('click',
        function() {
            $(this).closest('.alert').hide();            
            if ($('.alert:visible').length === 0) {
                $('.alert-container').css('margin-bottom', '0');
            }
        });

if ($('.alert:visible').length === 0) {
    $('.alert-container').css('margin-bottom', '0');
}

$(function () {

    // Find all YouTube videos
    var $allVideos = $("iframe[src^='https://www.youtube.com']"),

	    // The element that is fluid width
	    $fluidEl = $("article");

    // Figure out and save aspect ratio for each video
    $allVideos.each(function () {

        $(this)
			.data('aspectRatio', this.height / this.width)

			// and remove the hard coded width/height
			.removeAttr('height')
			.removeAttr('width');

    });

    // When the window is resized
    // (You'll probably want to debounce this)
    $(window).resize(function () {

        var newWidth = $fluidEl.width();

        // Resize all videos according to their own aspect ratio
        $allVideos.each(function () {

            var $el = $(this);
            $el
				.width(newWidth)
				.height(newWidth * $el.data('aspectRatio'));

        });

        // Kick off one resize to fix all videos on page load
    }).resize();

});
$(".carousel a").css("display", "block");
$(".carousel div").css("display", "block");
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

$(document).ready(function () {
    var CludoSearch;
    (function () {
        var cludoSettings = {
            customerId: 112,
            engineId: 1757,
            type: 'standardOverlay',
            hideSearchFilters: true,
            initSearchBoxText: '',
            searchInputs: ["cludo-search-form","cludo-search-mobile-form", "cludo-search-hero-form"],
            theme: { themeColor: '#055c58', themeBannerColor: {textColor: '#333', backgroundColor: '#f2f2f2'}, borderRadius: 10},
            language: 'en'
        };
        CludoSearch= new Cludo(cludoSettings);
        CludoSearch.init();
    })();
})
// Push content on presence of background image
var mobileWidth = 767;
var tabletWidth = (1024 - 17);
var windowHeight = $(window).height();
var pushHeight = (windowHeight / 4) + "px";
var groupsOf = 3;
var bp = 1024;
var childselector = ".topic-block-content";


$(document).ready(function () {

    STK.Matchboxes.Init();

    $(".show-search-button").click(
            function () {
                $("#mobileSearchInput").slideToggle(220);
                $(".show-search-button").toggleClass("arrow");               
            }
    );
});


$(window)
    .resize(function() {
       
        var matchboxPrimary = new Matchbox({
            parentSelector: ".topic-block-container",
            childSelector: childselector,
            groupsOf: 1,
            breakpoints: [
            { bp: 767, groupsOf: 2 },
            { bp: 1025, groupsOf: 3 }
            ]
        });

        if ($(window).width() > mobileWidth) {
            if ($(".topic-block-content").length > 0) {
                matchboxPrimary.init();
            } else {
                ($(".topic-block-content").css("height", "56px"));
        }
    }


    if ($(window).width() > tabletWidth) {

        $("#mobileSearchInput").hide();
        $(".show-search-button").removeClass("arrow");
        $('#displayRefineBy').css('display', 'block');
    }
});
document.documentElement.className = document.documentElement.className.replace("no-js", "js");