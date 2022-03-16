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

    var addHomepageLink = function () {
        const target = document.querySelector('#content');
        const config = { attributes: true, childList: true, subtree: true };
        const observerCallback = function (mutationsList, observer) {
            const cludoModule = document.querySelector('#cludo-404');

            for (const mutation of mutationsList) {
                if (mutation.type === 'attributes' && cludoModule != null && cludoModule.classList.contains("loaded")) {
                    observer.disconnect();

                    if (cludoModule.classList.contains("hide-module"))
                        document.querySelector("#homepage-link").classList.remove("invisible");
                }
            }     
        }

        const observer = new MutationObserver(observerCallback);
        observer.observe(target, config);
    }

    return {
        Init: init,
        AddHomepageLink: addHomepageLink
    }
});
