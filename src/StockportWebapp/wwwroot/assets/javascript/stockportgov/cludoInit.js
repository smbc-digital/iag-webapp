var CludoSearch;
var cludoSettings = {
    customerId: 112,
    engineId: 1144,
    searchUrl: '/searchResults?query',
    language: 'en',
    searchInputs: ['cludo-search-form', 'cludo-search-hero-form', 'cludo-search-mobile-form'],
    template: 'InlineBasic',
    focusOnResultsAfterSearch: true,
    type: 'inline'
};

CludoSearch = new Cludo(cludoSettings);
CludoSearch.init();