define(["jquery", "questioncontroller"], function ($, controller) {
    return {
        Init: function (route) {

            $(function () {
                controller.init(route);
            });

            $("#back-button").show().click(function () {
                window.history.back();
            });

            $("input[type='radio']").change(function () {
                var li = $(this).parent();
                var text = $(".tertiary-Information", li).text();
                $("#tertiary-Information-value").text(text);
            });
        }
    };
});
