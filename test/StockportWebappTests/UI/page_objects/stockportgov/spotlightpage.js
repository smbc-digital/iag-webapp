var methods = {

  assertBreadcrumbIsVisible: function() {
    this.waitForElementVisible('@breadcrumb', this.api.globals.timeOut)
  },

  assertHeroImageIsVisible: function() {
    this.waitForElementVisible('@heroImage', this.api.globals.timeOut)

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
    teaser: '.title-card>h2>span',
    subheading: '.featured-items-wrapper>h2',
    featuredtopics:'.featured-topic-list'



  }
};
