namespace StockportWebappTests_Unit.Unit.ViewDetails;

public class ContactUsDetailsTest
{
    [Fact]
    public void TestNameIsRequired()
    {
        // Arrange
        ContactUsDetails model = new();
        ValidationContext context = new(model, null, null);
        List<ValidationResult> result = new();
        
        // Act
        bool valid = Validator.TryValidateObject(model, context, result, true);

        // Assert
        Assert.False(valid);
        ValidationResult failure = Assert.Single(result, x => x.ErrorMessage.Equals("Enter your name"));
        Assert.Single(failure.MemberNames, x => x.Equals("Name"));
    }

    [Fact]
    public void TestSubjectIsRequired()
    {
        // Arrange
        ContactUsDetails model = new();
        ValidationContext context = new(model, null, null);
        List<ValidationResult> result = new();
        
        // Act
        bool valid = Validator.TryValidateObject(model, context, result, true);

        // Assert
        Assert.False(valid);
        ValidationResult failure = Assert.Single(result, x => x.ErrorMessage.Equals("Enter the subject of your enquiry"));
        Assert.Single(failure.MemberNames, x => x.Equals("Subject"));
    }

    [Fact]
    public void TestMessageIsRequired()
    {
        // Arrange
        ContactUsDetails model = new();
        ValidationContext context = new(model, null, null);
        List<ValidationResult> result = new();
        
        // Act
        bool valid = Validator.TryValidateObject(model, context, result, true);

        // Assert
        Assert.False(valid);
        ValidationResult failure = Assert.Single(result, x => x.ErrorMessage.Equals("Tell us about your enquiry"));
        Assert.Single(failure.MemberNames, x => x.Equals("Message"));
    }

    [Fact]
    public void TestEmailValidationFailsIfEmailIsNotRightFormat()
    {
        // Arrange
        ContactUsDetails model = new()
        {
            Name = "Name",
            Email = "email",
            Subject = "Subject",
            Message = "Message"
        };

        ValidationContext context = new(model, null, null);
        List<ValidationResult> result = new();
        
        // Act
        bool valid = Validator.TryValidateObject(model, context, result, true);

        // Assert
        Assert.False(valid);
        ValidationResult failure = Assert.Single(result, x => x.ErrorMessage.Equals("This is not a valid email address"));
        Assert.Single(failure.MemberNames, x => x.Equals("Email"));
    }

    [Fact]
    public void TestEmailValidationPassesIfEmailIsRightFormat()
    {
        // Arrange
        ContactUsDetails model = new()
        {
            Name = "Name",
            Email = "email@domain.com",
            Subject = "Subject",
            Message = "Message"
        };

        ValidationContext context = new(model, null, null);
        List<ValidationResult> result = new();
        
        // Act
        bool valid = Validator.TryValidateObject(model, context, result, true);

        // Assert
        Assert.True(valid);
    }
}