var methods = {
    // assertions go in here
    
    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@managePageTitle', this.api.globals.timeOut)
            .expect.element('@managePageTitle')
            .text.to.equal(title);
    },

    assertGroupIsVisibleAndGoToManagePage: function (browser) {
        browser.useXpath().waitForElementVisible(".//*[@id='group-item-uitest-a-group-for-ui-testing']",
            this.api.globals.timeOut);
        browser.useXpath().click(".//*[@id='group-item-uitest-a-group-for-ui-testing']");
    },

    assertManageGroupsEventsTitleIsVisible: function (title) {
        this.waitForElementVisible('@manageGroupPageTitle', this.api.globals.timeOut)
            .expect.element('@manageGroupPageTitle')
            .text.to.equal(title);
    }
};

module.exports = {
    commands: [methods],
    url: function () {
        // where to start the tests from in this file
        return this.api.globals.testUri + "/groups/manage";
    },
    elements: {
        managePageTitle: "#manage-groups-heading",
        groupId: "#group-item-uitest-a-group-for-ui-testing",
        manageGroupPageTitle: "#manage-group-title"
        
    }
};
