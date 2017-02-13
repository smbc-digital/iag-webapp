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
    if (!Modernizr.inputtypes.date) {
        $("#datepickerfrom").datepicker({
            minDate: new Date(),
            dateFormat: 'dd/mm/yy',
            onClose: function (selectedDate) {
                $("#datepickerto").datepicker("option", "minDate", selectedDate);
            }
        });

        $("#datepickerto").datepicker({
            dateFormat: 'dd/mm/yy',
            onClose: function (selectedDate) {
                $("#datepickerfrom").datepicker("option", "maxDate", selectedDate);
            }
        });

        if ($("#datepickerfrom").val() !== null && $("#datepickerfrom").val() !== "") {
            var startDate = new Date($("#datepickerfrom").val());
            $("#datepickerfrom").val($.datepicker.formatDate('dd/mm/yy', startDate));
        }

        if ($("#datepickerto").val() !== null && $("#datepickerto").val() !== "") {
            var endDate = new Date($("#datepickerto").val());
            $("#datepickerto").val($.datepicker.formatDate('dd/mm/yy', endDate));
        }


        
    }
});

