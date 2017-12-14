define(["jquery"], function ($) {

    var limit = 3;
    var className = '';

    var categoriesList;
    var categories;

    if ($('#categoryListError').css('display') == 'block') {
        $('.CategoriesList-select').addClass("input-validation-error");
        $('.CategoriesList-select').css("margin-bottom", "0px");      
    }

    var selectDropdown = function (select) {
        var links = $('.' + className + '-add', '.' + className + '-div:visible');
        var link = links[links.length - 1];
        if (allHaveValues()) {
            $(link).show();
            $("#categoryListError").hide();
            $('.CategoriesList-select').removeClass("input-validation-error");
            $('.CategoriesList-select').css("margin-bottom", "10px");
        }
        else {
            $(link).hide();
        }

        resetHiddenValueList();
        populate();
    }

    var deleteDropdown = function (link) {
        $(link).parent().parent().hide();
        resetHiddenValueList();
        populate();
    }

    var addDropdown = function (link) {
        var newValue = $('#' + className).val() + '|';
        $('#' + className).val(newValue);
        populate();
    }

    var hideSelectedValues = function () {
        $('option[value!=""]', '.' + className + '-select').show();
        $('.' + className + '-select').each(function (index) {
            var outer = this;
            var outerIndex = index;
            var val = $(outer).val();
            $('.' + className + '-select').each(function (index) {
                var inner = this;
                var innerIndex = index;
                if (outerIndex !== innerIndex) {
                    $('option[value="' + val + '"]', inner).hide();
                }
            });
        });
    };

    var showRemoves = function () {
        var removes = $('.' + className + '-remove');
        $(removes).show();
        var loneIndex = -1;
        var hasValueCount = 0;
        var shownCategories = $('.' + className + '-div:visible');
        var selects = $('select', $(shownCategories));
        $(selects).each(function (index) {
            if ($(this).val() !== null && $(this).val() !== '') {
                if (loneIndex === -1) {
                    loneIndex = index;
                }
                hasValueCount++;
            }
        });

        if (hasValueCount === 1) {
            $($(removes)[loneIndex]).hide();
        }

        if (shownCategories.length === 1) {
            $($(removes)[0]).hide();
        }
    };

    var resetHiddenValueList = function () {
        var shownCategories = $('.' + className + '-div:visible');
        var arrayList = '';
        var comma = '';
        for (var i = 0; i < shownCategories.length; i++) {
            if (i > 0) arrayList += '|';
            var select = $('select', $(shownCategories)[i]);
            arrayList += $(select).val();
        }

        $('#' + className).val(arrayList);
    }

    var allHaveValues = function () {
        var result = true;
        $('.' + className + '-select:visible').each(function () {
            result = result && $(this).val() !== null && $(this).val() !== '';
        });

        return result;
    }

    var populate = function () {
        categoriesList = $('#' + className).val();
        categories = categoriesList.split('|');

        var max = categoriesList.length > limit ? limit : categoriesList.length;
        max = max === 0 ? max = 1 : max;
        categories = categories.slice(0, max);

        $('.' + className + '-div').hide();
        $('.' + className + '-add').hide();
        $('.' + className + '-remove').hide();
        $('.' + className + '-select').val('');
        $('option[value=""]', '.' + className + '-select').show();

        for (var i = 0; i < categories.length; i++) {
            var div = $('.' + className + '-div')[i];
            if (typeof (div) !== 'undefined') {

                $(div).show();

                var select = $('.' + className + '-select')[i];
                if (categories[i] !== null && categories[i].length > 0) {
                    $('option[value=""]', select).hide();
                    $(select).val(categories[i]);
                }

                $(select).off('change').on('change', function () {
                    selectDropdown(this);
                });
            }
        }

        hideSelectedValues();

        var shownCategories = $('.' + className + '-div:visible');

        for (var i = 0; i < shownCategories.length; i++) {

            var add = $('.' + className + '-add')[i];

            if (i === shownCategories.length - 1 && shownCategories.length < limit) {
                if (allHaveValues()) {
                    $(add).show();
                }
                $(add).off('click').on('click', function () {
                    addDropdown(this);
                });
            }

            var removeLink = $('.' + className + '-remove-link')[i];
            $(removeLink).off('click').on('click', function () {
                deleteDropdown(this);
            });

            showRemoves();
        }
    }

    return {
        Init: function (baseControlId) {
            className = baseControlId;
            limit = $('#' + className + '-limit').val();
            populate();
        }
    };
});


