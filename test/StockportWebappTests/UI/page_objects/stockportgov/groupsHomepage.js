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
