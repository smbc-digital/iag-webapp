define(["jquery"], function ($) {
    var init = function () {
        $(document).ready(function () {
            const observer = new MutationObserver(() => {
                const searchFilters = document.querySelector("#cludo-search-results > div > div.article--30 > div.search-filters");
                const searchFiltersTitle = document.querySelector("#cludo-search-results > div > div.article--30 > div.search-filters__title");
    
                if (searchFilters && searchFiltersTitle) {
                    if (!searchFilters.hasChildNodes()) {
                        searchFiltersTitle.style.display = "none";
                    } else {
                        searchFiltersTitle.style.display = "block";
                    }
                }
            });
    
            observer.observe(document.body, { childList: true, subtree: true });
        }
    )}

    return {
        Init: init
    }
});