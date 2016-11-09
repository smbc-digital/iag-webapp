var methods = {
    goToHome: function (browser) {
        this.waitForElementVisible('@breadcrumbContainer', this.api.globals.timeOut);
        browser.useXpath().click("//ul[@class='breadcrumb']/li/a[contains(@href,'/') and text()='Home']");
    }
};

module.exports = {
    commands: [methods],
    url: function () {
        // This is giving us a page object for the index method
        return this.api.globals.testUri;
    },
    elements: {
        breadcrumbContainer: '.breadcrumb-container'
    }
};
