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
        this.assert.visible('#Name');
        this.assert.visible('@emailField');
        this.assert.visible('#Subject');
        this.assert.visible('#Message');
    },

    assertTitleIsHiddenFieldOnContactForm: function () {
        this.waitForElementVisible('@contactUsForm', this.api.globals.timeOut);
        this.assert.hidden('#Title');
    },

    assertTitleOfArticleIsInTitleFieldOfContactFormForSection: function () {
        this.waitForElementVisible('@contactUsForm', this.api.globals.timeOut);
        //this.waitForElementVisible("#Title", this.api.globals.timeOut).getAttribute("value")
        //    .assert.isEqual("UITEST: Article with Section for Contact Us form");

        var titleValue = this.waitForElementPresent("#Title", this.api.globals.timeOut)
            .verify.attributeEquals('#Title', 'value', 'UITEST: Article with Section for Contact Us form');
    },

    enterTextIntoFormField: function (browser, field, fieldInput) {
        this.waitForElementVisible('@contactUsForm', this.api.globals.timeOut);
        browser.waitForElementPresent("#" + field, this.api.globals.timeOut)
            .setValue("#" + field, fieldInput)
            .click('#uitest-contact-form-submit');
    },
    
    submitContactUsForm: function() {
        this.waitForElementVisible('@contactUsForm', this.api.globals.timeOut)
            .click('#uitest-contact-form-submit');
    },

    assertValidationMessageIsVisible: function (browser, field, expectedtext) {
        this.waitForElementVisible('@contactUsForm', this.api.globals.timeOut);
        this.waitForElementVisible("#" + field, this.api.globals.timeOut);
        browser.useXpath()
            .assert.visible("//span[@class='' and text()='" + expectedtext + "']");
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
        profileSubtitle: '.profile-subtitle',
        butoDiv: '#buto_kQl5D',
        baseVideoTitle: '.base-video-title',
        tableTag: 'table',
        tableHeader: 'th',
        tableCell: 'td',
        documentTag: '.test-documents',
        documentHeading: '.test-document-detail',
        contactUsForm: '#contactForm',
        emailField: '#Email'
  }
};
