define(['jquery'], function ($) {
    var disableEmptyInputs = function (form) {
        const inputs = form.querySelectorAll('input, select');

        inputs.forEach(input => {
            if (!input.value) {
                input.disabled = true;
            }
        });
    };

    return {
        Init: function () {             
            $('#homepageFilters').on('submit', function (event) {
                disableEmptyInputs(this);
            });
        }
    };
});