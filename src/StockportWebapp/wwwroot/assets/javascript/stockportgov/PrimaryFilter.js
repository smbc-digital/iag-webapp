$(document).ready(function () {

    $("#hiddenSelectCategory").html("<option>" + $("#selectCategory").find(":selected").text() + "</option>");
    $("#selectCategory").width($("#hiddenSelectCategory").width());
    $("#selectCategory").change(function () {
        $("#hiddenSelectCategory").html("<option>" + $("#selectCategory").find(":selected").text() + "</option>");
        $("#selectCategory").width($("#hiddenSelectCategory").width());

        $("#selectCategory").css("content",'"."');
    });

    $("#hiddenSelectOrder").html("<option>" + $("#selectOrder").find(":selected").text() + "</option>");
    $("#selectOrder").width($("#hiddenSelectOrder").width());
    $("#selectOrder").change(function () {
        $("#hiddenSelectOrder").html("<option>" + $("#selectOrder").find(":selected").text() + "</option>");
        $("#selectOrder").width($("#hiddenSelectOrder").width());
    });

    $("#hiddenSelectLocation").html("<option>" + $("#selectLocation").find(":selected").text() + "</option>");
    $("#selectLocation").width($("#hiddenSelectLocation").width());
    $("#selectLocation").change(function () {
        $("#hiddenSelectLocation").html("<option>" + $("#selectOrder").find(":selected").text() + "</option>");
        $("#selectLocation").width($("#hiddenSelectLocation").width());
    });

});
