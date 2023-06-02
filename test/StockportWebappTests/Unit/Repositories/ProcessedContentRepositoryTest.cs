//namespace stockportwebapptests_unit.unit.repositories;

//public class processedcontentrepositorytest
//{
//    private readonly iprocessedcontentrepository _repository;
//    private readonly mock<ihttpclient> _mockhttpclient;
//    private readonly mock<istubtourlconverter> _mockurlgenerator;
//    private readonly mock<isimpletagparsercontainer> _tagparsercontainer;
//    private readonly mock<idynamictagparser<profile>> _profiletagparser;
//    private readonly mock<idynamictagparser<document>> _documenttagparser;
//    private readonly mock<idynamictagparser<alert>> _alertsinlinetagparser;
//    private readonly mock<markdownwrapper> _markdownwrapper;
//    private readonly mock<iapplicationconfiguration> appconfig;
//    private readonly mock<ihttpcontextaccessor> httpcontextaccessor;
//    private readonly mock<idynamictagparser<s3bucketsearch>> _s3bucketparser;
//    private readonly mock<idynamictagparser<privacynotice>> _privacynoticetagparser;

//    public processedcontentrepositorytest()
//    {
//        _tagparsercontainer = new mock<isimpletagparsercontainer>();
//        _profiletagparser = new mock<idynamictagparser<profile>>();
//        _markdownwrapper = new mock<markdownwrapper>();
//        _documenttagparser = new mock<idynamictagparser<document>>();
//        _alertsinlinetagparser = new mock<idynamictagparser<alert>>();
//        _mockurlgenerator = new mock<istubtourlconverter>();
//        appconfig = new mock<iapplicationconfiguration>();
//        httpcontextaccessor = new mock<ihttpcontextaccessor>();
//        _mockhttpclient = new mock<ihttpclient>();
//        _s3bucketparser = new mock<idynamictagparser<s3bucketsearch>>();
//        _privacynoticetagparser = new mock<idynamictagparser<privacynotice>>();

//        _s3bucketparser.setup(o => o.parse(it.isany<string>(), it.isany<ienumerable<s3bucketsearch>>())).returns("");

//        var contentfactory = new contenttypefactory(_tagparsercontainer.object, _profiletagparser.object, _markdownwrapper.object, _documenttagparser.object, _alertsinlinetagparser.object, httpcontextaccessor.object, _s3bucketparser.object, _privacynoticetagparser.object);
//        _repository = new processedcontentrepository(_mockurlgenerator.object, _mockhttpclient.object, contentfactory, appconfig.object);
//    }

//    /*
//     * article
//     */
//    [fact]
//    public async task getarticleforarticleslug()
//    {
//        var articleslug = "physical-activity";
//        const string url = "article-with-slug-url";
//        _mockurlgenerator.setup(o => o.urlfor<article>(articleslug, it.isany<list<query>>())).returns(url);

//        var body = "staying active and exercising is essential to reach and maintain a healthy lifestyle.";

//        _mockhttpclient.setup(o => o.get(url, it.isany<dictionary<string, string>>()))
//            .returnsasync(new httpresponse(200, jsonfilehelper.getstringresponsefromfile("article.json"), string.empty));

//        _tagparsercontainer.setup(o => o.parseall(it.isany<string>(), it.isany<string>())).returns(body);
//        _profiletagparser.setup(o => o.parse(it.isany<string>(), it.isany<ienumerable<profile>>())).returns(body);
//        _markdownwrapper.setup(o => o.converttohtml(it.isany<string>())).returns(body);
//        _documenttagparser.setup(o => o.parse(it.isany<string>(), it.isany<ienumerable<document>>())).returns(body);
//        _alertsinlinetagparser.setup(o => o.parse(it.isany<string>(), it.isany<ienumerable<alert>>())).returns(body);
//        _s3bucketparser.setup(o => o.parse(it.isany<string>(), it.isany<ienumerable<s3bucketsearch>>())).returns(body);
//        _privacynoticetagparser.setup(o => o.parse(it.isany<string>(), it.isany<ienumerable<privacynotice>>())).returns(body);

//        var httpresponse = await _repository.get<article>(articleslug);
//        var article = httpresponse.content as processedarticle;

//        article.title.should().notbenull();
//        article.navigationlink.should().notbenull();
//        article.body.should().be(body);
//        article.backgroundimage.should().notbenull();
//        article.icon.should().be("fa-icon");
//        article.breadcrumbs.should().havecount(1);
//        article.breadcrumbs.first().title.should().be("test topic");
//        article.breadcrumbs.first().navigationlink.should().contain("test-topic");

//        var section = article.sections.first();

//        section.title.should().be("overview ");
//        section.slug.should().be("physical-activity-overview");
//        section.body.should().contain("staying active and exercising is essential to reach and maintain a healthy lifestyle.");
//    }

//    [fact]
//    public async task getarticleforarticleslugwithoutbackgroundimage()
//    {
//        const string articleslug = "physical-activity";
//        const string url = "article-with-slug-url";
//        _mockurlgenerator.setup(o => o.urlfor<article>(articleslug, it.isany<list<query>>())).returns(url);

//        _mockhttpclient.setup(o => o.get(url, it.isany<dictionary<string, string>>()))
//            .returnsasync(new httpresponse(200, jsonfilehelper.getstringresponsefromfile("articlewithoutbackgroundimage.json"),
//                string.empty));

//        var httpresponse = await _repository.get<article>(articleslug);
//        var article = httpresponse.content as processedarticle;

//        article.backgroundimage.should().benull();
//        article.title.should().be("title");
//        article.sections.first().profiles.first().title.should().be("a pull out");
//    }

//    [fact]
//    public async task getsnotfoundforarticlenotfound()
//    {
//        const string nonexistentarticle = "pineapple";
//        const string articlenotfounderror = "no article found for pineapple";

//        const string url = "non-existent-article-with-slug-url";
//        _mockurlgenerator.setup(o => o.urlfor<article>(nonexistentarticle, it.isany<list<query>>())).returns(url);

//        _mockhttpclient.setup(o => o.get(url, it.isany<dictionary<string, string>>()))
//            .returnsasync(httpresponse.failure(404, articlenotfounderror));

//        var httpresponse = await _repository.get<article>(nonexistentarticle);

//        assert.equal(404, httpresponse.statuscode);
//        assert.equal("no article found for pineapple", httpresponse.error);
//    }

//    [fact]
//    public async task getsalertbyarticleslug()
//    {
//        const string articleslug = "article-with-alerts";
//        const string url = "get-articlewithalerts-with-slug-url";

//        _mockurlgenerator.setup(o => o.urlfor<article>(articleslug, it.isany<list<query>>())).returns(url);
//        _mockhttpclient.setup(o => o.get(url, it.isany<dictionary<string, string>>()))
//            .returnsasync(new httpresponse(200, jsonfilehelper.getstringresponsefromfile("articlewithalerts.json"),
//                string.empty));

//        var httpresponse = await _repository.get<article>(articleslug);
//        var topic = httpresponse.content as processedarticle;

//        topic.alerts.should().havecount(1);
//        topic.alerts.firstordefault().title.should().be("this is an alert");
//        topic.alerts.firstordefault().subheading.should().be("it has a subheading");
//        topic.alerts.firstordefault()
//            .body.should()
//            .be("<p>it also has a body text</p>\n");
//        topic.alerts.firstordefault().severity.should().be(severity.warning);
//    }

//    /*
//     * profile
//     */
//    [fact]
//    public async task shouldgetaprofile()
//    {
//        const string profileslug = "slug";
//        const string url = "get-getaprofile-with-slug-url";

//        _mockurlgenerator.setup(o => o.urlfor<profile>(profileslug, it.isany<list<query>>())).returns(url);

//        _mockhttpclient.setup(o => o.get(url, it.isany<dictionary<string, string>>()))
//            .returnsasync(new httpresponse(200, jsonfilehelper.getstringresponsefromfile("profile.json"), string.empty));

//        var httpresponse = await _repository.get<profile>(profileslug);
//        var profile = httpresponse.content as processedprofile;

//        profile.title.should().be("test profile");
//        profile.slug.should().be("test-profile");
//        profile.teaser.should().be("teaser");
//        profile.subtitle.should().be("test sub title");
//        profile.image.should().be("image");
//        profile.type.should().be("success story");
//        profile.backgroundimage.should().be("background-image");
//        profile.icon.should().be("fa-icon");
//        profile.breadcrumbs.should().havecount(2);
//        profile.breadcrumbs.first().navigationlink.should().contain("test-sub-topic-1");
//        profile.breadcrumbs.first().title.should().be("test sub topic 1");
//    }

//    /*
//     * news
//     */
//    [fact]
//    public async task getsnews()
//    {
//        const string slug = "news";
//        const string url = "get-news-with-slug-url";

//        _mockurlgenerator.setup(o => o.urlfor<news>(slug, it.isany<list<query>>())).returns(url);
//        var body = "lorem ipsum dolor sit amet, consectetur adipiscing elit.";

//        _mockhttpclient.setup(o => o.get(url, it.isany<dictionary<string, string>>())).returnsasync(new httpresponse(200, jsonfilehelper.getstringresponsefromfile("news.json"), string.empty));

//        _tagparsercontainer.setup(o => o.parseall(body, it.isany<string>())).returns(body);
//        _markdownwrapper.setup(o => o.converttohtml(body)).returns(body);
//        _documenttagparser.setup(o => o.parse(body, it.isany<list<document>>())).returns(body);

//        var httpresponse = await _repository.get<news>(slug);
//        var news = httpresponse.content as processednews;

//        news.title.should().be("another news article");
//        news.slug.should().be("another-news-article");
//        news.teaser.should().be("this is another news article");
//        news.image.should().be("image.jpg");
//        news.thumbnailimage.should().be("thumbnail.jpg");
//        news.body.should().be(body);
//    }

//    [fact]
//    public async task getsevent()
//    {
//        const string slug = "event";
//        const string url = "get-event-with-slug-url";

//        _mockurlgenerator.setup(o => o.urlfor<event>(slug, null)).returns(url);

//        var body = "the event description";

//        _mockhttpclient.setup(o => o.get(url, it.isany<dictionary<string, string>>())).returnsasync(new httpresponse(200, jsonfilehelper.getstringresponsefromfile("event.json"), string.empty));

//        _tagparsercontainer.setup(o => o.parseall(body, it.isany<string>())).returns(body);
//        _markdownwrapper.setup(o => o.converttohtml(body)).returns(body);
//        _documenttagparser.setup(o => o.parse(body, it.isany<list<document>>())).returns(body);

//        var httpresponse = await _repository.get<event>(slug);
//        var events = httpresponse.content as processedevents;

//        events.title.should().be("this is the event");
//        events.slug.should().be("event-of-the-century");
//        events.teaser.should().be("read more for the event");

//        events.description.should().be(body);
//    }

//    [fact]
//    public async task getseventwithspecificdate()
//    {
//        const string slug = "event";
//        const string url = "get-event-with-slug-url";
//        var date = new datetime();
//        _mockurlgenerator.setup(o => o.urlfor<event>(slug, it.is<list<query>>(q => q.contains(new query("date", date.tostring("yyyy-mm-dd")))))).returns(url);

//        var body = "the event description";

//        _mockhttpclient.setup(o => o.get(url, it.isany<dictionary<string, string>>())).returnsasync(new httpresponse(200, jsonfilehelper.getstringresponsefromfile("event.json"), string.empty));

//        _tagparsercontainer.setup(o => o.parseall(body, it.isany<string>())).returns(body);
//        _markdownwrapper.setup(o => o.converttohtml(body)).returns(body);
//        _documenttagparser.setup(o => o.parse(body, it.isany<list<document>>())).returns(body);

//        var httpresponse = await _repository.get<event>(slug, new list<query> { new query("date", date.tostring("yyyy-mm-dd")) });
//        var eventitem = httpresponse.content as processedevents;

//        eventitem.title.should().be("this is the event");
//        eventitem.slug.should().be("event-of-the-century");
//        eventitem.teaser.should().be("read more for the event");
//        eventitem.description.should().be(body);
//    }

//    [fact]
//    public async task getsgroup()
//    {
//        const string slug = "group";
//        const string url = "get-group-with-slug-url";

//        _mockurlgenerator.setup(o => o.urlfor<group>(slug, null)).returns(url);

//        var body = "the group description";

//        _mockhttpclient.setup(o => o.get(url, it.isany<dictionary<string, string>>())).returnsasync(new httpresponse(200, jsonfilehelper.getstringresponsefromfile("group.json"), string.empty));

//        _tagparsercontainer.setup(o => o.parseall(body, it.isany<string>())).returns(body);
//        _markdownwrapper.setup(o => o.converttohtml(body)).returns(body);

//        var httpresponse = await _repository.get<group>(slug);
//        var group = httpresponse.content as processedgroup;

//        group.name.should().be("zumba");
//        group.slug.should().be("test-zumba-slug");
//        group.phonenumber.should().be("00000000000");
//        group.email.should().be("hello@stockportzumba.whatever");
//        group.website.should().be("stockportzumba.io");
//        group.facebook.should().be("facebook.com/stockportzumba");
//        group.address.should().be("zumba house,\nzumba road,\nzumba zumba zumba");
//        group.description.should().be("the group description");
//    }

//    [fact]
//    public async task getsshowcase()
//    {
//        //arrange
//        const string slug = "showcase";
//        const string url = "url";

//        _mockurlgenerator.setup(o => o.urlfor<showcase>(slug, null)).returns(url);
//        _mockhttpclient.setup(o => o.get(url, it.isany<dictionary<string, string>>())).returnsasync(new httpresponse(200, jsonfilehelper.getstringresponsefromfile("showcase.json"), string.empty));

//        //act
//        var httpresponse = await _repository.get<showcase>(slug);
//        var showcase = httpresponse.content as processedshowcase;

//        //assert
//        showcase.title.should().be("test showcase");
//        showcase.slug.should().be("test-showcase");
//        showcase.teaser.should().be("just a test");
//        showcase.subheading.should().be("test subheading");
//        showcase.heroimageurl.should().be("heroimageurl.jpg");
//        showcase.featureditems.first().title.should().be("test title");
//        showcase.breadcrumbs.first().title.should().be("test-title");
//    }
//}

