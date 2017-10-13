var methods = {
    assertGroupHomepageTitleIsVisible: function (title) {
        this.waitForElementVisible('@groupsHomepageTitle', this.api.globals.timeOut)
            .expect.element('@groupsHomepageTitle')
            .text.to.equal(title);
    },
    assertGroupsHomepageFeaturedListContainerIsVisible: function () {
        this.waitForElementVisible("@groupsHomepageFeaturedList", this.api.globals.timeOut);
    },
    assertGroupsHomepageFeaturedContainerButtonForItemOneIsVisible: function (itemText) {
        this.waitForElementVisible("@groupsHomepageFeaturedContainerItemOne", this.api.globals.timeOut)
            .expect.element('@groupsHomepageFeaturedContainerItemOne')
            .text.to.equal(itemText);
    },
    assertGroupsHomepageFeaturedContainerButtonForItemTwoIsVisible: function (itemText) {
        this.waitForElementVisible("@groupsHomepageFeaturedContainerItemTwo", this.api.globals.timeOut)
            .expect.element('@groupsHomepageFeaturedContainerItemTwo')
            .text.to.equal(itemText);
    },
    assertGroupsHomepageFeaturedContainerButtonForItemThreeIsVisible: function (itemText) {
        this.waitForElementVisible("@groupsHomepageFeaturedContainerItemThree", this.api.globals.timeOut)
            .expect.element('@groupsHomepageFeaturedContainerItemThree')
            .text.to.equal(itemText);
    },
    assertGroupsHomepageFeaturedContainerButtonForItemFourIsVisible: function (itemText) {
        this.waitForElementVisible("@groupsHomepageFeaturedContainerItemFour", this.api.globals.timeOut)
            .expect.element('@groupsHomepageFeaturedContainerItemFour')
            .text.to.equal(itemText);
    },
    assertFeaturedGroupsArePresent: function (value, browser) {
        this.waitForElementVisible("#featured-groups", this.api.globals.timeOut)
            .expect.element('.group-title h3')
            .text.to.equal(value);

        // take back to the main /groups/ page for further testing using breadrumb
        browser.click(".group-card a[href='/groups/uitest-group']");
    },
    assertGroupExists: function () {
        this.waitForElementVisible("h1", this.api.globals.timeOut)
            .expect.element('h1')
            .text.to.equal("UITEST: Group");
        
        this.expect.element('.add-favourite').to.be.present;
        this.expect.element('.group-details').to.be.present;
        this.expect.element('.manage-group-section h3').to.be.present;
        this.expect.element('.manage-group-section h3').text.to.equal("Manage your group information");
        this.expect.element('.export-as-pdf').to.be.present;
    }
};

module.exports = {
    commands: [methods],
    url: function () {
        return this.api.globals.testUri + "/groups/";
    },
    elements: {
        groupsHomepageFeaturedList: ".group-homepage-list",
        groupsHomepageFeaturedContainerItemOne: ".group-homepage-container:nth-child(1)",
        groupsHomepageFeaturedContainerItemTwo: ".group-homepage-container:nth-child(2)",
        groupsHomepageFeaturedContainerItemThree: ".group-homepage-container:nth-child(3)",
        groupsHomepageFeaturedContainerItemFour: ".group-homepage-container:last-of-type",
        groupsHomepageTitle: ".group-homepage-title"
    }
};
