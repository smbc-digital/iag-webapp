define(['jquery'], function ($) {
    let hasInitialized = false;

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
            if (!hasInitialized) {
                hasInitialized = true;
                
                $('#homepageFilters').on('submit', function (event) {
                    disableEmptyInputs(this);
                });
            }
        }
    };
});