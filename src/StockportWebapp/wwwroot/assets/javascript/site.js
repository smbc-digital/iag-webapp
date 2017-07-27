var SMART = {};

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