using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StockportWebapp.Dtos;
using StockportWebapp.QuestionBuilder.Entities;
using StockportWebapp.QuestionBuilder.Maps;
using Xunit;

namespace StockportWebappTests.Unit.SmartAnswers
{
    public class QuestionMapperTests
    {
        [Fact]
        public void ShouldTrigger_ShouldReturnTrueWhenAnswerMatches()
        {
            var previousAnswersJson = "{\"buildWhat\":{\"QuestionId\":\"buildWhat\"," +
                                      "\"Question\":\"What to Build\",\"Response\":\"Garage\"}}";

            var answers = JsonConvert.DeserializeObject<Dictionary<string, Question>>(previousAnswersJson).Select(q => new Answer
            {
                QuestionId = q.Value.QuestionId,
                Response = q.Value.Response
            }).ToList();

            var mapper = new QuestionMapper<BuildingRegsModel>(new BuildingRegsMap());
            var smartAnswers = mapper.MapFromAnswers(answers);

//            Assert.Equal("John", smartAnswers.);
//            Assert.Equal("Jones", bookingRequest.LastName);
//            Assert.Equal("test@test.com", bookingRequest.Email);
//            Assert.Equal("01749865948", bookingRequest.TelephoneNumber);
//            Assert.Equal(appointmentDate, bookingRequest.AppointmentDateTime);
//            Assert.Equal("Susan Jones", bookingRequest.PartnerName);
//            Assert.Equal(EBookingRequestedBy.Father, bookingRequest.RequestedBy);
        }

    }
}
