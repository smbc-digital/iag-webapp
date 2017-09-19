var methods = {

  assertBreadcrumbIsVisible: function() {
    this.waitForElementVisible('@breadcrumb', this.api.globals.timeOut)
  },

  assertHeroImageIsVisible: function() {
    this.waitForElementVisible('@heroImage', this.api.globals.timeOut)
    .expect.element('@heroImage').to.have.css('background-image');

  },

  assertTitleIsVisible: function(title) {
    this.waitForElementVisible('@title', this.api.globals.timeOut)
        .expect.element('@title').text.to.equal(title);
  },

  assertTeaserIsVisible: function(browser, teaser) {
    this.waitForElementVisible('@teaser', this.api.globals.timeOut)
        .expect.element('@teaser').text.to.equal(teaser);
  },
  assertSubheadingIsVisible: function(browser, subheading) {
    this.waitForElementVisible('@subheading', this.api.globals.timeOut)
        .expect.element('@subheading').text.to.equal(subheading);
  },
  assertFeaturedTopicsAreVisible: function() {
    this.waitForElementVisible('@featuredtopics', this.api.globals.timeOut)
  },
  assertCurrentConsulationsIsVisible: function(browser) {
    this.waitForElementVisible('@consultaionSection', this.api.globals.timeOut)
        .expect.element('@consulationHeading').text.to.equal("Current Consultations");
        this.waitForElementVisible('@consultaionTable', this.api.globals.timeOut)
  },
  assertNewsSectionVisible: function() {
    this.waitForElementVisible('@newsSection', this.api.globals.timeOut)
  },
  assertEventsSectionVisible: function() {
    this.waitForElementVisible('@eventsSection', this.api.globals.timeOut)
  },
};

module.exports = {
  commands: [methods],
  url: function () {
    // This is giving us a page object for the index method
    return this.api.globals.testUri + "/showcase/uitest-showcase-page";
  },
  elements: {
    breadcrumb:'.breadcrumb',
    heroImage: '.showcase-hero-image',
    title: '.title-card>h1>span',
    teaser: '.title-card>div>div>h2>span',
    subheading: '.featured-items-wrapper>div>h2',
    featuredtopics:'.featured-topic-list',
    consultaionSection:'.grid-100.tablet-grid-100.mobile-grid-100.group-margin',
    consulationHeading: '.grid-100.sk-table-cell>h3',
    consultaionTable: '.grid-100.sk-table-row',
    newsSection: '.showcase-news.showcase-news-events',
    eventsSection:'.showcase-events.showcase-news-events'




  }
};
