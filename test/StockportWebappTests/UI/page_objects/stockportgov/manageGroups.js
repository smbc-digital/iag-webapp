var methods = {
    // assertions go in here

    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@managePageTitle', this.api.globals.timeOut)
            .expect.element('@managePageTitle')
            .text.to.equal(title);
    },

    assertGroupIsVisibleAndGoToManagePage: function (browser) {
        browser.useXpath().waitForElementVisible(".//*[@id='group-item-uitest-a-group-for-ui-testing']", this.api.globals.timeOut);
        browser.useXpath().click(".//*[@id='group-item-uitest-a-group-for-ui-testing']");
    },

    assertManageGroupsTitleIsVisible: function (title) {
        this.waitForElementVisible('@manageGroupPageTitle', this.api.globals.timeOut)
            .expect.element('@manageGroupPageTitle')
            .text.to.equal(title);
    },

    assertViewEventsButtonIsVisibleAndClickIt: function (browser) {
        browser.useXpath().waitForElementVisible(".//*[@id='view-group-events']", this.api.globals.timeOut);
        browser.useXpath().click(".//*[@id='view-group-events']");
    },
    assertManageGroupsEventsTitleIsVisible: function (title) {
        this.waitForElementVisible('@manageGroupPageTitle', this.api.globals.timeOut)
            .expect.element('@manageGroupPageTitle')
            .text.to.equal(title);
    },
    assertGroupsEventIsVisibleAndClickIt: function (browser) {
        browser.useXpath().waitForElementVisible(".//*[@id='event-item-hats-amazing']", this.api.globals.timeOut);
        browser.useXpath().click(".//*[@id='event-item-hats-amazing']");
    },
    assertEventsTitleIsVisible: function (title) {
        this.waitForElementVisible('@eventPageTitle', this.api.globals.timeOut)
            .expect.element('@eventPageTitle')
            .text.to.equal(title);
    }
};

module.exports = {
    commands: [methods],
    url: function () {
        return this.api.globals.testUri + "/groups/manage";
    },
    elements: {
        managePageTitle: "#manage-groups-heading",
        groupId: "#group-item-uitest-a-group-for-ui-testing",
        manageGroupPageTitle: "#manage-group-title",
        viewEventsForGroup: "#view-group-events",
        eventPageTitle: "#event-title"
    }
};
