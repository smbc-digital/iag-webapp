$(".l-filters .filter-title, .l-filters .filter-inner-title").click(function () {
    $(this).siblings("ul").slideToggle(100, function () {
        $(this).parent("li").toggleClass("is-collapsed");
    });
});

if ($("#custom-filter-li").hasClass("customdateactive")) {
    $("#custom-filter-li").removeClass().addClass("customdateactive").addClass("collapsible");
}
else
{
    $("#custom-filter-li").removeClass().addClass("filter").addClass("is-collapsed");
}

$(function () {
    $("#datepickerfrom").datepicker({
        minDate: new Date(),
        dateFormat: 'yy-mm-dd',
        onClose: function (selectedDate) {
            $("#datepickerto").datepicker("option", "minDate", selectedDate);
        }
    });

    $("#datepickerto").datepicker({
        dateFormat: 'yy-mm-dd',
        onClose: function (selectedDate) {
            $("#datepickerfrom").datepicker("option", "maxDate", selectedDate);
        }
    });
});

