var methods = {
    assertTitle: function (browser, title) {
        this.waitForElementVisible('@lettervisited', this.api.globals.timeOut)
            .expect.element('@lettervisited').text.to.equal(title);
    },
assertArticlelistedVisible: function (browser, articlename){
    this.waitForElementVisible('@atozListingItem', this.api.globals.timeOut)
        .expect.element('@atozListingItem').text.to.contain(articlename);
},
    goToLetter: function(browser, letter) {
        this.waitForElementVisible('@atozList', this.api.globals.timeOut);
        browser.useXpath()
            .click("(//a[contains(@href,'/atoz/" + letter + "')])[2]");
    }

};
module.exports = {
    commands: [methods],
    url: function () {
        // This is giving us a page object for the index method
        return this.api.globals.testUri + '/atoz';
    },
    elements: {
        lettervisited: 'h1',
        atozList: '.atoz',
        atozListingItem: '.atoz-listing-item'
    }};