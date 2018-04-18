define(["jquery", "cludo"], function ($, cludo) {

    var init = function () {
        var cludoSettings = {
            customerId: 112,
            engineId: 1144,
            type: 'standardOverlay',
            hideSearchFilters: true,
            initSearchBoxText: '',
            searchInputs: ["cludo-search-form", "cludo-search-mobile-form", "cludo-search-hero-form"],
            theme: { themeColor: '#055c58', themeBannerColor: { textColor: '#ffffff', backgroundColor: '#055c58' }, borderRadius: 10 },
            language: 'en'
        };

        CludoSearch = new Cludo(cludoSettings);
        CludoSearch.init();
    };

    return {
        Init: init
    }
});
