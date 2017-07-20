var mobileWidth = 767;
var tabletWidth = (1024 - 17);

$(document).ready(function () {

    $('.wysiwyg').each(function () {
        if (!$(this).parent().hasClass('stk-wysiwyg')) {
            $(this).parent().addClass('stk-wysiwyg')
        }
    });

    $('.wysiwyg').wysihtml5({
        toolbar: {
            fa: true,
            "font-styles": false,
            emphasis: {
                bold: true,
                italic: false,
                underline: false,
                small: false
            },
            "lists": true, //(Un)ordered lists, e.g. Bullets, Numbers. Default true
            "html": false, //Button which allows you to edit the generated HTML. Default false
            "link": true, //Button to insert a link. Default true
            "image": false, //Button to insert an image. Default true,
            "color": false, //Button to change color of font
            "blockquote": false //Blockquote
        },
    });


    SwapLogo();
    $(".show-search-button").click(
            function () {
                $("#mobileSearchInput").slideToggle(220);
                $(".show-search-button").toggleClass("arrow");
            }
    );

    if (!Modernizr.inputtypes.date) {
        $(".datepicker").datepicker({
            inline: true,
            dateFormat: 'dd/mm/yy'
        });

        $(".hasDatepicker").each(function () {
            var selectedDate = $(this).val();
            if (selectedDate !== null && selectedDate !== "") {
                var eventdate = new Date(selectedDate);
                $(this).val($.datepicker.formatDate('dd/mm/yy', eventdate));
            }
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
        $('#displayRefineBy').css('display', 'block');
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

function setEndDateToStartDate(endDateId, startDateId) {
    if (!Modernizr.inputtypes.date) {
        var startDate = $("#" + startDateId).val();

        if (startDate != null) {
            var endDate = $("#" + endDateId).val();
            if (endDate === "") {
                $("#" + endDateId).val(startDate);
            }
        }
    }
}

