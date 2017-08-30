define(["jquery", "cludo"], function ($, cludo) {
    var CludoSearch;
    (function () {
        var cludoSettings = {
            customerId: 112,
            engineId: 1757,
            type: 'standardOverlay',
            hideSearchFilters: true,
            initSearchBoxText: '',
            searchInputs: ["cludo-search-form", "cludo-search-mobile-form", "cludo-search-hero-form"],
            theme: { themeColor: '#055c58', themeBannerColor: { textColor: '#333', backgroundColor: '#f2f2f2' }, borderRadius: 10 },
            language: 'en'
        };
        CludoSearch = new Cludo(cludoSettings);
        CludoSearch.init();
    })();
});