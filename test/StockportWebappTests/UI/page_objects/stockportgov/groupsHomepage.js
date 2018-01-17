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
    assertGroupExists: function (browser) {
        browser.waitForElementVisible("h1", this.api.globals.timeOut)
            .expect.element('h1')
            .text.to.equal("UITEST: Group");
        
        this.expect.element('.add-favourite').to.be.present;
        this.expect.element('.group-details-heading').to.be.present;
        this.expect.element('.group-information-section h3').to.be.present;
        this.expect.element('.group-information-section h3').text.to.equal("Manage your group information");
        //this.expect.element('.export-as-pdf').to.be.present;
    },
    assertCanGetToCategoryResultsPage: function(browser) {
        this.waitForElementVisible("#see-more-services", this.api.globals.timeOut)
            .expect.element('#see-more-services')
            .text.to.equal("View more categories");
        
        browser.click("button[id='see-more-services']");

        browser.useXpath();
        browser.waitForElementVisible(".//*[@class='featured-topic-name'][text()='UITEST: Group Category']", this.api.globals.timeOut);
        
        browser.click(".//*[@class='featured-topic-link'][@href='/groups/results?category=uitest-group-category&order=Name+A-Z']");

        browser.useCss();
        this.expect.element("#hiddenSelectCategory").value.to.equal("UITEST: Group Category");

        browser.useXpath();
        this.expect.element(".//*[@id='item-card-uitest-a-group-for-ui-testing']/div/a[@href='/groups/uitest-a-group-for-ui-testing']").to.be.present;
    },
    assertCanUseRefineByFilter: function(browser) {
        browser.waitForElementVisible(".//div[@class='refine']/a[@class='link'][contains(text(), 'Get involved')]", this.api.globals.timeOut);

        browser.click(".//div[@class='refine']/a[@class='link'][contains(text(), 'Get involved')]");
        
        browser.waitForElementVisible(".//*[@id='refine-slider']//div[contains(@class, 'refine-filters') and contains(.//*, 'Volunteering opportunities')]", this.api.globals.timeOut);

        browser.click(".//input[@type='checkbox'][@name='getinvolved']");
        browser.click(".//a[@class='apply']");

        browser.expect.element(".//*[@id='item-card-uitest-a-group-for-ui-testing']/div/a[@href='/groups/uitest-a-group-for-ui-testing']").to.be.present;

        browser.expect.element(".//div[@class='badge']/span[text()='1']").to.be.present;
    },
    assertCanSearchNearMe: function(browser) {
        browser.useCss();

        browser.waitForElementVisible("#currentLocationgroup", this.api.globals.timeOut)
                .expect.element("#currentLocationgroup h3")
                .text.to.equal("What's near me?");
        
        browser.click("#currentLocationgroup");

        // can't rely on this bieng anything specific, so just check it isn't the default
        browser.waitForElementVisible("#postcode", this.api.globals.timeOut);
        browser.expect.element("#postcode").value.to.not.equal("Stockport");
    },
    assertCanSearchEverything: function(browser) {
        browser.useXpath();

        browser.waitForElementVisible(".//div[@id='search-everything']", this.api.globals.timeOut)
            .expect.element(".//div[@id='search-everything']//h3")
            .text.to.equal("Search everything");

        browser.click(".//div[@id='search-everything']//a");

        browser.expect.element(".//*[@id='hiddenSelectCategory']").value.to.equal("All");

        browser.useCss().expect.element(".group-card").to.be.present;

        browser.useXpath().click(".//ul[@class='breadcrumb']//a[@href='/groups']");
    },
    assertCanFindHelpAndSupport: function(browser) {
        browser.useXpath();

        browser.waitForElementVisible(".//div[@id='find-help-and-support']", this.api.globals.timeOut)
            .expect.element(".//div[@id='find-help-and-support']//h3")
            .text.to.equal("Find help and support");

        browser.click(".//div[@id='find-help-and-support']//a");

        browser.expect.element(".//*[@id='hiddenSelectCategory']").value.to.equal("Help and support");

        browser.useCss().expect.element(".group-card").to.be.present;
        
        browser.useXpath().click(".//ul[@class='breadcrumb']//a[@href='/groups']");
    }
};
module.exports = {
    commands: [methods],
    url: function () {
        return this.api.globals.testUri + "/groups";
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
