define(["jquery"], function ($) {
    return {
        Init: function () {
            $("#reciteMe").click(function(i,e){
                loadService();
                return false;
            });
        }
    };
});