define(['jquery'], function ($) {
    return {
        Init: function () {
            const emailInput = document.getElementsByName('emailAddress')[0];
  
            emailInput.addEventListener('input', handler, false);
            emailInput.addEventListener('invalid', handler, false);
        
            function handler(event) {
                if (emailInput.validity.valueMissing) {
                    emailInput.setCustomValidity('Enter an email address');
                } else if (emailInput.validity.typeMismatch) {
                    emailInput.setCustomValidity('Enter a valid email address');
                } else {
                    emailInput.setCustomValidity('');
                }
            }
        }
    };
});
