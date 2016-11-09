var methods = {
    assertSearchBarIsVisible: function () {
        this.waitForElementVisible('.grid-container-full-width', 10000)
            .assert.visible('@searchBar');
    },

    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@profileTitle', this.api.globals.timeOut)
            .expect.element('@profileTitle').text.to.equal(title);
    }
};

module.exports = {
    commands: [methods],
    url: function () {
        // This is giving us a page object for the index method
        return this.api.globals.testUri + 'profile';
    },
    elements: {
        searchBar: '.search-bar',
        profileTitle: 'h1'
    }
};
