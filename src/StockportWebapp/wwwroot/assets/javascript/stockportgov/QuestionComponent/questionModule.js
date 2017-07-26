define(['jquery', 'questionController', 'questionView', 'questionValidator'],
  function ($, controllerCtor, viewCtor, validatorCtor) {

    return {
      init: function (route) {

        var view = viewCtor();
        var validator = validatorCtor();
        var controller = controllerCtor(route, view, validator);

        $(function () {
          controller.init();
        });
      }
    };
  });
