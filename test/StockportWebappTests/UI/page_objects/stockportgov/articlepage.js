var methods = {
    assertTitleIsVisible: function(title) {
        this.waitForElementVisible('@articleTitle', this.api.globals.timeOut)
            .expect.element('@articleTitle')
            .text.to.equal(title);
    },

    assertProfileIsVisible: function(title) {
        this.waitForElementVisible('@profile', this.api.globals.timeOut)
            .expect.element('@profileSubtitle')
            .text.to.equal(title);
    },

    assertButoVideoIsVisible: function() {
        this.waitForElementPresent('@butoDiv', 50000);
    },

    assertTableIsVisible: function() {
        this.waitForElementVisible('@tableTag', this.api.globals.timeOut)
            .expect.element('@tableHeader')
            .text.to.equal("Header");
    },

    assertDocumentIsVisible: function() {
        this.waitForElementVisible('@documentTag', this.api.globals.timeOut)
            .expect.element('@documentHeading')
            .text.to.equal("UITEST: Document");
    },

    goToProfile: function(browser, link) {
        this.waitForElementVisible('@profile', this.api.globals.timeOut);
        browser.useXpath().click("//a[contains(@href,'/profile/" + link + "')]");
    },

    goToNextSection: function(browser) {
        this.waitForElementVisible('@paginationNext', this.api.globals.timeOut);
        browser.useXpath().click("//div[contains(@class,'pagination-next')]/a");
    },

    goToStartPage: function(browser, link) {
        this.waitForElementVisible('@articleContainer', this.api.globals.timeOut);
        browser.useXpath().click("//a[contains(@href,'/start/" + link + "')]");
    },

    assertContactFormIsVisible: function() {
        this.waitForElementVisible('@contactUsForm', this.api.globals.timeOut);
        this.assert.visible('@Name');
        this.assert.visible('@emailField');
        this.assert.visible('@Subject');
        this.assert.visible('@Message');
    },

    assertTitleIsHiddenFieldOnContactForm: function(browser) {
        this.waitForElementVisible('@contactUsForm', this.api.globals.timeOut);
        browser.useCss();
        this.assert.hidden('#Title');
    },


    assertLiveChatIsVisible: function(browser) {
      this.waitForElementVisible('.l-right-side-bar-section.side-bar-section-live-chat', this.api.globals.timeOut);
      browser.expect.element(".l-right-side-bar-section.side-bar-section-live-chat>h2").text.to.equal("UITEST: Live Chat");
      browser.useXpath().expect.element("//div[@class='l-right-side-bar-section side-bar-section-live-chat']/p[2]").text.to.contain("this is a live chat text");

    },


    assertTitleOfArticleIsInTitleFieldOfContactFormForSection: function (browser) {
        this.waitForElementVisible('@contactUsForm', this.api.globals.timeOut);
        browser.useCss();
        var titleValue = this.waitForElementPresent("#Title", this.api.globals.timeOut)
            .verify.attributeEquals('#Title', 'value', 'UITEST: Article with Section for Contact Us form');
    },

    enterTextIntoFormField: function (browser, field, fieldInput) {
        this.waitForElementVisible('@contactUsForm', this.api.globals.timeOut);
        browser.useCss();
        browser.waitForElementPresent("#" + field, this.api.globals.timeOut)
            .setValue("#" + field, fieldInput)
            .click('#uitest-contact-form-submit');
    },

    submitContactUsForm: function (browser) {
        this.waitForElementVisible('@contactUsForm', this.api.globals.timeOut);
        browser.useCss().click('#uitest-contact-form-submit');
    },

    assertValidationMessageIsVisible: function (browser, field, expectedtext) {
        this.waitForElementVisible('@contactUsForm', this.api.globals.timeOut);
        browser.useCss();
        this.waitForElementVisible("#" + field, this.api.globals.timeOut);
        browser.useXpath()
            .assert.visible("//span[@class='' and text()='" + expectedtext + "']");
    },

    assertCarouselIsVisible: function(browser) {
      browser.useCss();
        this.waitForElementVisible('.carousel', this.api.globals.timeOut);
    },

    assertCarouselImagesAreVisible: function (browser) {
      browser.useCss();
      this.waitForElementVisible('.carousel-image.slick-slide.slick-current.slick-active', this.api.globals.timeOut);
    }
};

module.exports = {
    commands: [methods],
    url: function() {
        // This is giving us a page object for the index method
        return this.api.globals.testUri + '/article';
    },
    elements: {
        articleTitle: 'h1',
        articleContainer: '#uitest-page-has-loaded',
        paginationNext: '.pagination-next',
        profile: '.profile',
        profileSubtitle: '.summary',
        butoDiv: '#buto_kQl5D',
        baseVideoTitle: '.base-video-title',
        tableTag: 'table',
        tableHeader: 'th',
        tableCell: 'td',
        documentTag: '.test-documents',
        documentHeading: '.document-detail-title',
        contactUsForm: '#contactForm',
        Name : '#Name',
        emailField: '#Email',
        Subject: '#Subject',
        Message: '#Message'
  }
};
