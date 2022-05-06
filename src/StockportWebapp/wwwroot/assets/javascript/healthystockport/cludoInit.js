var CludoSearch;
var cludoSettings = {
    customerId: 112,
    engineId: 1757,
    searchUrl: '/searchResults?query',
    language: 'en',
    searchInputs: ['cludo-search-form'],
    template: 'InlineBasic',
    focusOnResultsAfterSearch: true,
    type: 'inline'
};

CludoSearch = new Cludo(cludoSettings);
CludoSearch.init();