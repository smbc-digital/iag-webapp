using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using StockportWebapp.Dtos;
using StockportWebapp.Enums;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.QuestionBuilder;
using StockportWebapp.QuestionBuilder.Entities;
using StockportWebapp.QuestionBuilder.Maps;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;
using Xunit;
using HttpResponse = StockportWebapp.Http.HttpResponse;

namespace StockportWebappTests_Unit.Unit.SmartAnswers
{
    internal class TestQuestionController : BaseQuestionController<GenericSmartAnswersModel, GenericSmartAnswersMap>
    {
        public TestQuestionController(IDictionary<int, Page> structure, QuestionLoader questionLoader, IHttpContextAccessor httpContextAccessor, FeatureToggles featuretoggle, IHttpClient _client, IConfiguration _config, ILogger<BaseQuestionController<GenericSmartAnswersModel, GenericSmartAnswersMap>> logger, ISmartAnswerStringHelper smartAnswerStringHelper) : base(httpContextAccessor, questionLoader, featuretoggle, _client, _config, logger, smartAnswerStringHelper)
        {
        }

        public override IActionResult ProcessResults(GenericSmartAnswersModel result, string endpointName)
        {
            return null;
        }
    }

    public class BaseQuestionControllerTests
    {
        private IDictionary<int, Page> _structure;
        private object _smartAnswersResponse;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private Mock<QuestionLoader> _questionLoader;
        private Mock<IRepository> _repository; 
        private FeatureToggles _featureToggles;
        private ISmartAnswerStringHelper _smartAnswerStringHelper;
        private readonly Mock<IHttpClient> _client;
        private readonly Mock<IConfiguration> _config;
        private readonly Mock<ILogger<BaseQuestionController<GenericSmartAnswersModel, GenericSmartAnswersMap>>> _logger;

        public BaseQuestionControllerTests()
        {
            _repository = new Mock<IRepository>();

            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _questionLoader = new Mock<QuestionLoader>(_repository.Object);
            _featureToggles = new FeatureToggles();
            _client = new Mock<IHttpClient>();
            _config = new Mock<IConfiguration>();
            _logger = new Mock<ILogger<BaseQuestionController<GenericSmartAnswersModel, GenericSmartAnswersMap>>>();
            _smartAnswerStringHelper = new SmartAnswerStringHelper();
            SetFakeQuestionStructure();
            SetFakeResponse();

            _repository.Setup(o => o.Get<StockportWebapp.Models.SmartAnswers>(It.IsAny<string>(), null))
                .ReturnsAsync(new HttpResponse(200, _smartAnswersResponse, string.Empty));

            _httpContextAccessor.Setup(_ => _.HttpContext.Request.Path).Returns("/smart-answers/pathUrl");
        }

        [Fact]
        public void GetPageForId_ShouldReturnFirstPage()
        {
            var navigator = new TestQuestionController(_structure, _questionLoader.Object, _httpContextAccessor.Object, _featureToggles, _client.Object, _config.Object, _logger.Object, _smartAnswerStringHelper);

            var page = navigator.GetPage(0);

            page.PageId.Should().Be(0);
            page.Description.Should().Be("Page 1");
            page.ButtonText.Should().Be("Next step");
        }

        [Fact]
        public void GetAllDetails_ForFirstPage_AndAreAllValid()
        {
            var navigator = new TestQuestionController(_structure, _questionLoader.Object, _httpContextAccessor.Object, _featureToggles, _client.Object, _config.Object, _logger.Object, _smartAnswerStringHelper);

            var page = navigator.GetPage(0);

            page.PageId.Should().Be(0);
            page.Description.Should().Be("Page 1");
            page.ButtonText.Should().Be("Next step");
            page.Questions.Count.Should().Be(1);
            page.Questions[0].Label.Should().Be("What are you proposing to build?");

            page.Behaviours.Count.Should().Be(3);
            page.Behaviours[0].BehaviourType.Should().Be(EQuestionType.GoToPage);

            page.Questions[0].Options.Count.Should().Be(3);
            page.Questions[0].Options[0].Label.Should().Be("Conservatory/Porch/Orangery");
            page.Questions[0].Options[1].Label.Should().Be("Garagee");
            page.Questions[0].Options[2].Label.Should().Be("Shed");
        }

        [Fact]
        public void RunBehaviours_ShouldRunDefaultBehaviourIfNoBehavioursAreDefined()
        {
            var navigator = new TestQuestionController(_structure, _questionLoader.Object, _httpContextAccessor.Object, _featureToggles, _client.Object, _config.Object, _logger.Object, _smartAnswerStringHelper);
            const int currentPage = 203;
            var actual = navigator.DefaultBehaviour(currentPage);

            actual.Should().NotBeNull();
            actual.Should().BeOfType<ViewResult>();
            var viewResult = (ViewResult)actual;

            viewResult.Model.Should().BeOfType<Page>();
            var model = (Page)viewResult.Model;

            model.PageId.Should().Be(204);
            model.IsLastPage.Should().BeTrue();
        }

        [Fact]
        public async void RunBehaviours_ShouldRunBehaviourForRedirect()
        {
            var navigator = new TestQuestionController(_structure, _questionLoader.Object, _httpContextAccessor.Object, _featureToggles, _client.Object, _config.Object, _logger.Object, _smartAnswerStringHelper);
            Page currentPage = navigator.GetPage(204);

            var actual = await navigator.RunBehaviours(currentPage);
            actual.Should().BeOfType<RedirectResult>();
            var model = (RedirectResult) actual;
            currentPage.Behaviours[0].Value.Should().Be(model.Url);
        }

        [Fact]
        public async void RunBehaviours_ShouldRunBehaviourForRedirectToAction()
        {
            var navigator = new TestQuestionController(_structure, _questionLoader.Object, _httpContextAccessor.Object, _featureToggles, _client.Object, _config.Object, _logger.Object, _smartAnswerStringHelper);
            Page currentPage = navigator.GetPage(101);

            var actual = await navigator.RunBehaviours(currentPage);
            actual.Should().BeOfType<RedirectToActionResult>();
            var model = (RedirectToActionResult) actual;
            currentPage.Behaviours[0].Value.Should().Be(model.ActionName);
        }


        [Fact]
        public void IndexShouldNotReturnCacheHeaderWhenPageShouldBeCached()
        {
            var httpContext = new DefaultHttpContext();
            _httpContextAccessor.SetupGet(_ => _.HttpContext).Returns(httpContext);
            var navigator = new TestQuestionController(_structure, _questionLoader.Object, _httpContextAccessor.Object, _featureToggles, _client.Object, _config.Object, _logger.Object, _smartAnswerStringHelper);
            var page = new Page { ShouldCache = true };

            var result = navigator.Index(page);

            httpContext.Response.Headers.ContainsKey("Cache-Control").Should().BeFalse();
            httpContext.Response.Headers.ContainsKey("Pragma").Should().BeFalse();
        }

        [Fact]
        public void IndexShouldReturnCacheHeaderWhenPageShouldNotBeCached()
        {
            var httpContext = new DefaultHttpContext();         
            _httpContextAccessor.SetupGet(_ => _.HttpContext).Returns(httpContext);

            var navigator = new TestQuestionController(_structure, _questionLoader.Object, _httpContextAccessor.Object, _featureToggles, _client.Object, _config.Object, _logger.Object, _smartAnswerStringHelper);
            var page = new Page { ShouldCache = false };

            var result = navigator.Index(page);

            httpContext.Response.Headers["Cache-Control"].Should().Equal("no-store, must-revalidate, max-age=0");
            httpContext.Response.Headers["Pragma"].Should().Equal("no-cache");
        }

        private void SetFakeResponse()
        {
            _smartAnswersResponse = new StockportWebapp.Models.SmartAnswers
            {
                Slug = "slug",
                Questionjson = "[{\"pageId\":0,\"buttonText\":\"Next step\",\"ShouldCache\":true,\"questions\":[{\"questionId\":\"buildWhat\",\"secondaryInfo\":\"\",\"questionType\":\"Radio\",\"label\":\"What are you proposing to build?\",\"options\":[{\"label\":\"Conservatory/Porch/Orangery\",\"value\":\"conservatory\"},{\"label\":\"Garagee\",\"value\":\"garage\"},{\"label\":\"Shed\",\"value\":\"shed\"}],\"validatorData\":[{\"type\":\"non-empty\",\"message\":\"This is a required answer\"}]}],\"behaviours\":[{\"behaviourType\":3,\"conditions\":[{\"questionId\":\"buildWhat\",\"equalTo\":\"conservatory\"}],\"value\":1},{\"behaviourType\":3,\"conditions\":[{\"questionId\":\"buildWhat\",\"equalTo\":\"garage\"}],\"value\":101},{\"behaviourType\":3,\"conditions\":[{\"questionId\":\"buildWhat\",\"equalTo\":\"shed\"}],\"value\":201}],\"description\":\"Page 1\"},{\"pageId\":1,\"buttonText\":\"Next step\",\"ShouldCache\":true,\"questions\":[{\"questionId\":\"30SqMetres\",\"secondaryInfo\":\"This would be the total square footage of the floor. e.g. 3m x 3m = 9m squared\",\"questionType\":\"Radio\",\"label\":\"Is the floor area less than 30 square metres?\",\"options\":[{\"label\":\"Yes\",\"value\":\"yes\"},{\"label\":\"No\",\"value\":\"no\"}],\"validatorData\":[{\"type\":\"non-empty\",\"message\":\"This is a required answer\"}]}],\"behaviours\":[{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"30SqMetres\",\"equalTo\":\"no\"}],\"value\":\"http://localhost:5000/you-need-building-regulations\"}],\"description\":\"Conservatory/Porch/Orangery\"},{\"pageId\":2,\"buttonText\":\"Next step\",\"ShouldCache\":true,\"questions\":[{\"questionId\":\"ExteriaDoorBetween\",\"secondaryInfo\":\"Do you leave the house to enter the conservatory?\",\"questionType\":\"Radio\",\"label\":\"Is there an exterior quality door between the conservatory and house?\",\"options\":[{\"label\":\"Yes\",\"value\":\"yes\"},{\"label\":\"No\",\"value\":\"no\"}],\"validatorData\":[{\"type\":\"non-empty\",\"message\":\"This is a required answer\"}]}],\"behaviours\":[{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"ExteriaDoorBetween\",\"equalTo\":\"no\"}],\"value\":\"http://localhost:5000/you-need-building-regulations\"}],\"description\":\"Conservatory/Porch/Orangery\"},{\"pageId\":3,\"buttonText\":\"Next step\",\"ShouldCache\":true,\"questions\":[{\"questionId\":\"ToughenedSafetyGlass\",\"secondaryInfo\":null,\"questionType\":\"Radio\",\"label\":\"Has the conservatory been contructed with toughened saftey glass?\",\"options\":[{\"label\":\"Yes\",\"value\":\"yes\"},{\"label\":\"No\",\"value\":\"no\"}],\"validatorData\":[{\"type\":\"non-empty\",\"message\":\"This is a required answer\"}]}],\"behaviours\":[{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"ToughenedSafetyGlass\",\"equalTo\":\"no\"}],\"value\":\"http://localhost:5000/you-need-building-regulations\"}],\"description\":\"Are the windows double-glazed or single-glazed/ Kite symbol marked.\"},{\"pageId\":4,\"buttonText\":\"Next step\",\"ShouldCache\":true,\"questions\":[{\"questionId\":\"SeperateHeat\",\"secondaryInfo\":null,\"questionType\":\"Radio\",\"label\":\"Does the conservatory have it's own seperate heating setup?\",\"options\":[{\"label\":\"Yes\",\"value\":\"yes\"},{\"label\":\"No\",\"value\":\"no\"}],\"validatorData\":[{\"type\":\"non-empty\",\"message\":\"This is a required answer\"}]}],\"behaviours\":[{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"SeperateHeat\",\"equalTo\":\"yes\"}],\"value\":\"http://localhost:5000/exempt-from-building-regulations\"},{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"SeperateHeat\",\"equalTo\":\"no\"}],\"value\":\"http://localhost:5000/you-need-building-regulations\"}],\"description\":\"<p></p>\"},{\"pageId\":101,\"buttonText\":\"Next step\",\"ShouldCache\":true,\"questions\":[{\"questionId\":\"SingleStory\",\"Response\":\"no\",\"secondaryInfo\":\"<p>Do you leave the house to enter the conservatory?</p>\",\"questionType\":\"Radio\",\"label\":\"Is the garage Single Story?\",\"options\":[{\"label\":\"Yes\",\"value\":\"yes\"},{\"label\":\"No\",\"value\":\"no\"}],\"validatorData\":[{\"type\":\"non-empty\",\"message\":\"This is a required answer\"}]}],\"behaviours\":[{\"behaviourType\":1,\"conditions\":[{\"questionId\":\"SingleStory\",\"equalTo\":\"no\"}],\"value\":\"http://localhost:5000/you-need-building-regulations\"}],\"description\":\"\"},{\"pageId\":102,\"buttonText\":\"Next step\",\"ShouldCache\":true,\"questions\":[{\"questionId\":\"30SquareMetres\",\"secondaryInfo\":null,\"questionType\":\"Radio\",\"label\":\"Is it more or less than 30 square metres?\",\"options\":[{\"label\":\"More\",\"value\":\"more\"},{\"label\":\"Less\",\"value\":\"less\"}],\"validatorData\":[{\"type\":\"non-empty\",\"message\":\"This is a required answer\"}]}],\"behaviours\":[{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"30SquareMetres\",\"equalTo\":\"no\"}],\"value\":\"http://localhost:5000/you-need-building-regulations\"}],\"description\":\"\"},{\"pageId\":103,\"buttonText\":\"Next step\",\"ShouldCache\":true,\"questions\":[{\"questionId\":\"Materials\",\"secondaryInfo\":\"<strong>ROOF</strong> - non-combustible cement based sheeting fixed to steel roof trusses / tiled or slated roof on timber roof trusses or timbers / timber flat roof covered with felt with applied bitumen bedded 12.5mm limestone chippings.</br><strong>WALLS</strong> – brickwork / blockwork / concrete panels / steel frame clad in non-combustible cement based boarding.</br><strong>FLOORS</strong> – concrete slab.\",\"questionType\":\"Radio\",\"label\":\"Is it made of substantially incombustable materials?\",\"options\":[{\"label\":\"Yes\",\"value\":\"yes\"},{\"label\":\"No\",\"value\":\"no\"}],\"validatorData\":[{\"type\":\"non-empty\",\"message\":\"This is a required answer\"}]}],\"behaviours\":[{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"Materials\",\"equalTo\":\"yes\"}],\"value\":\"http://localhost:5000/exempt-from-building-regulations\"},{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"Materials\",\"equalTo\":\"no\"}],\"value\":\"http://localhost:5000/you-need-building-regulations\"}],\"description\":null},{\"pageId\":201,\"buttonText\":\"Next step\",\"ShouldCache\":true,\"questions\":[{\"questionId\":\"shedSize\",\"secondaryInfo\":null,\"questionType\":\"Radio\",\"label\":\"How many square metres is the shed's footprint?\",\"options\":[{\"label\":\"More than 30 square metres\",\"value\":\"more\"},{\"label\":\"Between 15 and 30 square metres\",\"value\":\"between\"},{\"label\":\"Less than 15 square metres\",\"value\":\"less\"}],\"validatorData\":[{\"type\":\"non-empty\",\"message\":\"This is a required answer\"}]}],\"behaviours\":[{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"shedSize\",\"equalTo\":\"more\"}],\"value\":\"http://localhost:5000/you-need-building-regulations\"},{\"behaviourType\":3,\"conditions\":[{\"questionId\":\"shedSize\",\"equalTo\":\"between\"}],\"value\":202},{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"shedSize\",\"equalTo\":\"less\"}],\"value\":\"http://localhost:5000/exempt-from-building-regulations\"}],\"description\":\"Shed\"},{\"pageId\":202,\"buttonText\":\"Next step\",\"ShouldCache\":true,\"questions\":[{\"questionId\":\"boundary\",\"secondaryInfo\":null,\"questionType\":\"Radio\",\"label\":\"Is it within 1 metre of a boundary?\",\"options\":[{\"label\":\"Yes\",\"value\":\"yes\"},{\"label\":\"No\",\"value\":\"no\"}],\"validatorData\":[{\"type\":\"non-empty\",\"message\":\"This is a required answer\"}]}],\"behaviours\":[{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"boundary\",\"equalTo\":\"no\"}],\"value\":\"http://localhost:5000/exempt-from-building-regulations\"},{\"behaviourType\":3,\"conditions\":[{\"questionId\":\"boundary\",\"equalTo\":\"yes\"}],\"value\":203}],\"description\":null},{\"pageId\":203,\"buttonText\":\"Next step\",\"ShouldCache\":true,\"questions\":[{\"questionId\":\"materials\",\"secondaryInfo\":null,\"questionType\":\"Radio\",\"label\":\"Is it made of substantially incombustable materials?\",\"options\":[{\"label\":\"Yes\",\"value\":\"yes\"},{\"label\":\"No\",\"value\":\"no\"}],\"validatorData\":[{\"type\":\"non-empty\",\"message\":\"This is a required answer\"}]}],\"behaviours\":[{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"materials\",\"equalTo\":\"no\"}],\"value\":\"http://localhost:5000/you-need-building-regulations\"},{\"behaviourType\":3,\"conditions\":[{\"questionId\":\"materials\",\"equalTo\":\"yes\"}],\"value\":204}],\"description\":null},{\"pageId\":204,\"buttonText\":\"Next step\",\"ShouldCache\":true,\"questions\":[{\"questionId\":\"windowsDoors\",\"Response\":\"no\",\"secondaryInfo\":null,\"questionType\":\"Radio\",\"label\":\"Does it have windows or doors?\",\"options\":[{\"label\":\"Yes\",\"value\":\"yes\"},{\"label\":\"No\",\"value\":\"no\"}],\"validatorData\":[{\"type\":\"non-empty\",\"message\":\"This is a required answer\"}]}],\"behaviours\":[{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"windowsDoors\",\"Response\":\"no\",\"equalTo\":\"no\"}],\"value\":\"http://localhost:5000/exempt-from-building-regulations\"},{\"behaviourType\":0,\"conditions\":[{\"questionId\":\"windowsDoors\",\"equalTo\":\"yes\"}],\"value\":\"http://localhost:5000/you-need-building-regulations\"}],\"IsLastPage\":\"True\",\"Response\":\"no\",\"description\":null}]"
            };
        }


        private void SetFakeQuestionStructure()
        {
            _structure = new Dictionary<int, Page>
            {
                {
                    0,
                    new Page(
                        0,
                        "Analytics Event",
                        "Page 1",
                        new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "1",
                                Label = "Question 1"
                            }
                        },
                        new List<Behaviour>
                        {
                            new Behaviour
                            {
                                BehaviourType = EQuestionType.GoToPage,
                                Conditions = new List<Condition>
                                {
                                    new Condition
                                    {
                                        QuestionId = "1",
                                        EqualTo = "no"
                                    }
                                },
                                Value = "2"
                            }
                        }
                    )
                },
                {
                    1,
                    new Page(1,
                        "Analytics Event",
                        "Page 2",
                        new List<Question>
                        {
                            new Question
                            {
                                Label = "Question 2"
                            }
                        }
                    )
                },
                {
                    2,
                    new Page(2, "Analytics Event", "Page 3", isLastPage: true,
                        questions: new List<Question>
                        {
                            new Question
                            {
                                Label = "Question 3"
                            }
                        }
                    )
                },
                {
                    3,
                    new Page(3, "Analytics Event", "Page 4",
                        isLastPage: true,
                        questions: new List<Question>
                        {
                            new Question
                            {
                                Label = "Question 4"
                            }
                        },
                        behaviours: new List<Behaviour> {
                            new Behaviour {
                                BehaviourType = EQuestionType.Redirect,
                                Value = "https://www.stockport.gov.uk"
                            }
                        }
                    )
                },
                {
                    4,
                    new Page(4, "Analytics Event", "Page 5",
                        isLastPage: true,
                        questions: new List<Question>
                        {
                            new Question
                            {
                                Label = "Question 4"
                            }
                        },
                        behaviours: new List<Behaviour> {
                            new Behaviour {
                                BehaviourType = EQuestionType.RedirectToAction,
                                Value = "TestRedirectToAction"
                            }
                        }
                    )
                },
            };
        }
    }
}
