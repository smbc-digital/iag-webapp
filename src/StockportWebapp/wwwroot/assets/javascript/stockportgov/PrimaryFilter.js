$(document).ready(function () {

    $("#selectCategory").change(function () {
        $("#hiddenSelectCategory").html("<option>" + $("#selectCategory").find(":selected").text() + "</option>");
        $("#selectCategory").width($("#hiddenSelectCategory").width());
    });

    $("#selectOrder").change(function () {
        $("#hiddenSelectOrder").html("<option>" + $("#selectOrder").find(":selected").text() + "</option>");
        $("#selectOrder").width($("#hiddenSelectOrder").width());
    });

    $("#selectLocation").change(function () {
        $("#hiddenSelectLocation").html("<option>" + $("#selectOrder").find(":selected").text() + "</option>");
        $("#selectLocation").width($("#hiddenSelectLocation").width());
    });

    $("#selectOrderMobile").change(function () {
        var orderby = $("#selectOrderMobile").find(":selected").text();
        $("#sortBy").val(orderby);
        $("#filterButton").click();
    });
});

$(window).resize(function () {
    $("#hiddenSelectCategory").html("<option>" + $("#selectCategory").find(":selected").text() + "</option>");
    $("#selectCategory").width($("#hiddenSelectCategory").width());
    $("#hiddenSelectOrder").html("<option>" + $("#selectOrder").find(":selected").text() + "</option>");
    $("#selectOrder").width($("#hiddenSelectOrder").width());
    $("#hiddenSelectLocation").html("<option>" + $("#selectLocation").find(":selected").text() + "</option>");
    $("#selectLocation").width($("#hiddenSelectLocation").width());
});