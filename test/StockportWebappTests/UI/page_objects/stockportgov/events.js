var methods = {
    assertEventTitleIsVisible: function (title) {
        this.waitForElementVisible('@pageTitle', this.api.globals.timeOut)
            .expect.element('@pageTitle').text.to.equal(title);
    }
    //assertNewsSideBarVisible: function (browser,title) {
    //    this.waitForElementVisible('@sideBar', this.api.globals.timeOut)
    //        .expect.element('@sideBarTitle').text.to.equal(title);
    //},

    //assertTimestampPresent: function (browser) {
    //    this.waitForElementVisible('@timeStamp', this.api.globals.timeOut)
    //        .expect.element('@timeStamp').text.to.contain("Last updated");
    //},

    //assertNewsSharePresent: function(browser) {
    //    this.waitForElementVisible('@shareIT', this.api.globals.timeOut)
    //        .expect.element('@shareThis')
    //        .text.to.equals("Share this");
    //    this.waitForElementVisible('@addThisIcons', this.api.globals.timeOut);
    //},
    //assertNewsTagIsVisible: function(tag) {
    //    this.waitForElementVisible('@newsTag', this.api.globals.timeOut)
    //        .expect.element('@newsTag').text.to.equal(tag);
    //},
    //assertDocumentIsVisible:function() {
    //    this.waitForElementVisible('@documentTag', this.api.globals.timeOut)
    //        .expect.element('@documentHeading')
    //        .text.to.equal("UITEST: Document");
    //},

};

module.exports = {
  commands: [methods],
  url: function () {
    // This is giving us a page object for the index method
      return this.api.globals.testUri + "/events/";
  },
  elements: {
      pageTitle: "h1",
      sideBar: ".l-left-side-bar.grid-30.tablet-grid-100.mobile-grid-100.grid-parent",
      sideBarTitle:".l-left-side-bar-section>h3",
      timeStamp: ".news-date.news-article-date",
      shareIT:".share",
      shareThis:".share>h6",
      addThisIcons: ".addthis_toolbox",
      newsTag: "li.news-tag a",    
  }
};
