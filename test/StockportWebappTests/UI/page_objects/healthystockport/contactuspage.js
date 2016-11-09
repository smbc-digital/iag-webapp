var methods = {
    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@pageTitle', this.api.globals.timeOut)
            .expect.element('@pageTitle').text.to.equal(title);
    },

    goToHomePage: function (browser) {
        this.waitForElementVisible('@header', this.api.globals.timeOut);
        browser.useXpath().click("//a[@href='/']");
    }
};

module.exports = {
    commands: [methods],
    url: function () {
        return this.api.globals.testUri + '/contact-us';
    },
    elements: {
        pageTitle: 'h1',
        articleContainer: '.l-article-container',
        paginationNext: '.pagination-next',
        profile: '.profile',
        profileSubtitle: '.profile-subtitle',
        butoDiv: '#buto_kQl5D',
        baseVideoTitle: '.base-video-title',
        tableTag: 'table',
        tableHeader: 'th',
        tableCell: 'td',
        header: '#header'
    }
};
