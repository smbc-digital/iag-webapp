define(["jquery"], function () {
    return {
        Init: function (healthyStockport) {
            var cludoSettings;
            if (healthyStockport) {
                cludoSettings = {
                    customerId: 112,
                    engineId: 1757,
                    searchUrl: '/searchResults?query',
                    language: 'en',
                    searchInputs: ['cludo-search-form', 'cludo-search-mobile-form'],
                    template: 'InlineBasic',
                    focusOnResultsAfterSearch: true,
                    type: 'inline'
                };
            } else {
                // Stockport Gov
                cludoSettings = {
                    customerId: 112,
                    engineId: 1144,
                    searchUrl: '/searchResults?query',
                    language: 'en',
                    searchInputs: ['cludo-search-form', 'cludo-search-hero-form', 'cludo-search-mobile-form'],
                    template: 'InlineBasic',
                    focusOnResultsAfterSearch: true,
                    type: 'inline'
                };
            }
            CludoSearch = new Cludo(cludoSettings);
            CludoSearch.init();
        }
    }
});
