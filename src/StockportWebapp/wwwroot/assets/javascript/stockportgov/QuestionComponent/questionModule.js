var SMART = SMART || {};

SMART.Module = function () {
    return {
        init: function (route) {
            var controller = SMART.Controller(route, SMART.View(), SMART.Validator());
            $(function () {
                controller.init();
            });
        }
    };
}
