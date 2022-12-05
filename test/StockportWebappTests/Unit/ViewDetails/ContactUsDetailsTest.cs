using System.ComponentModel.DataAnnotations;
using StockportWebapp.ViewDetails;
using Xunit;

namespace StockportWebappTests_Unit.Unit.ViewDetails
{
    public class ContactUsDetailsTest
    {
        [Fact]
        public void TestNameIsRequired()
        {
            var model = new ContactUsDetails();
            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(model, context, result, true);

            Assert.False(valid);
            var failure = Assert.Single(
            result,
            x => x.ErrorMessage == "Your name is required");
            Assert.Single(failure.MemberNames, x => x == "Name");
        }

        [Fact]
        public void TestSubjectIsRequired()
        {
            var model = new ContactUsDetails();
            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(model, context, result, true);

            Assert.False(valid);
            var failure = Assert.Single(
            result,
            x => x.ErrorMessage == "A subject is required");
            Assert.Single(failure.MemberNames, x => x == "Subject");
        }

        [Fact]
        public void TestMessageIsRequired()
        {
            var model = new ContactUsDetails();
            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(model, context, result, true);

            Assert.False(valid);
            var failure = Assert.Single(
            result,
            x => x.ErrorMessage == "An enquiry message is required");
            Assert.Single(failure.MemberNames, x => x == "Message");
        }

        [Fact]
        public void TestEmailValidationFailsIfEmailIsNotRightFormat()
        {
            var model = new ContactUsDetails
            {
                Name = "Name",
                Email = "email",
                Subject = "Subject",
                Message = "Message"
            };
            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(model, context, result, true);

            Assert.False(valid);
            var failure = Assert.Single(
            result,
            x => x.ErrorMessage == "This is not a valid email address");
            Assert.Single(failure.MemberNames, x => x == "Email");
        }

        [Fact]
        public void TestEmailValidationPassesIfEmailIsRightFormat()
        {
            var model = new ContactUsDetails
            {
                Name = "Name",
                Email = "email@domain.com",
                Subject = "Subject",
                Message = "Message"
            };
            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(model, context, result, true);

            Assert.True(valid);
        }
    }
}
