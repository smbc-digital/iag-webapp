var mobileWidth = 767;
var tabletWidth = (1024 - 17);

var matchboxPrimary = new Matchbox({
    parentSelector: ".l-page-content .nav-card-list",
    childSelector: ".nav-card .nav-card-item",
    groupsOf: 3,
    breakpoints: [
    { bp: 1024, groupsOf: 3 }
    ]
});

$(document).ready(function () {
    SwapLogo();
    $(".show-search-button").click(
            function () {
                $("#mobileSearchInput").slideToggle(220);
                $(".show-search-button").toggleClass("arrow");
            }
    );

    if ($(".l-page-content .nav-card-list").length) {
        matchboxPrimary.init();
    }

    if (!Modernizr.inputtypes.date) {
        $(".datepicker").datepicker({
            inline: true,
            dateFormat: 'dd/mm/yy'
        });

        if ($.validator) {
            $.validator.addMethod('date',
            function (value, element) {
                if (this.optional(element)) {
                    return true;
                }

                var ok = true;
                try {
                    $.datepicker.parseDate('dd/mm/yy', value);
                }
                catch (err) {
                    ok = false;
                }
                return ok;
            });
        }
    }
});

$(window).resize(function () {
    SwapLogo();

    if ($(window).width() > tabletWidth) {
        $("#mobileSearchInput").hide();
        $(".show-search-button").removeClass("arrow");
        $('#DisplayRefineBy').css('display', 'block');
    }
});

$('.global-alert-close-container a')
    .on('click',
        function () {
            $(this).closest('.global-alert').hide();
        });

// Swap logo image between mobile and desktop
function SwapLogo() {
    var image = $("#header .logo-main-image");
    var logoMobile = image.attr("data-mobile-image");
    var logoDesktop = image.attr("data-desktop-image");

    if ($(window).width() <= mobileWidth) {
        image.attr("src", logoMobile);
    } else {
        image.attr("src", logoDesktop);
    }
}