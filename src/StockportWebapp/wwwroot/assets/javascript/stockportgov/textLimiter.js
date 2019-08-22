define(["jquery"], function ($) {

  //  var restrictLength = function (inputId, validatorData) {
  //      $(inputId).on("keypress",
  //          function (event) {
  //              console.log("Hits restrictLength with validator value - " + validatorData.maxLength);
  //              if (event.target.value.length >= parseInt(validatorData.maxLength)) {
  //                  return false;
  //              }
  //          });
  //  };

  //  var init = function () {
		//restrictLength();
  //  };

  //  return {
		//Init: init
  //  };
      return {
	      restrictLength: function (inputId, maxLength) {
		      $(inputId).on("keypress",
			      function (event) {
				      if (event.target.value.length >= maxLength) {
					      return false;
				      }
			      });
	      }
      }

});
