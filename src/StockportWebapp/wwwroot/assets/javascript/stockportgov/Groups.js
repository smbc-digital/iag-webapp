var matchboxFeaturedItemsShowcase = new Matchbox({
    parentSelector: ".featured-category-items-wrapper",
    childSelector: '.featured-group-category',
    groupsOf: 3,
    breakpoints: [
    { bp: 767, groupsOf: 5 },
    { bp: 1024, groupsOf: 5 }
    ]
});

$(document).ready(
    function () {
        if ($(".featured-category-items-wrapper").length) { matchboxFeaturedItemsShowcase.init(); }
    }()
);

$(document).ready(
    function () {
        // only apply on mobile
        var mobileWidth = 767;

        if ($(window).width() <= mobileWidth) {
            $("#edit-search").hide();
            $(".result-arrow").addClass("result-search-down-arrow");

            $("#open-edit-search").click(function() {
                $("#edit-search").show();
                $(".result-arrow").toggleClass("result-search-down-arrow");
                $(".result-arrow").toggleClass("result-search-up-arrow");

                $(".result-search-down-arrow").parent().click(function() {
                    $("#edit-search").show();
                });
                $(".result-search-up-arrow").parent().click(function() {
                    $("#edit-search").hide();
                });
            });

            $(".result-search-up-arrow").parent().click(function () {
                $("#edit-search").hide();
            });
        }

        $(".print-this")
       .click(
           function () {
               window.print();
           }
       );

    }
);


