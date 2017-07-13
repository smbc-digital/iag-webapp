define(['jquery', 'questionComponent/questionController', 'questionComponent/questionView', 'questionComponent/questionValidator'],
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
