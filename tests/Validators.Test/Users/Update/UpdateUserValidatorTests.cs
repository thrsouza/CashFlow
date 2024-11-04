using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Test.Users.Update;

public class UpdateUserValidatorTests
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Error_Name_Empty(string name)
    {
        // Arrange
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = name;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NameIsRequired));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Error_Email_Empty(string email)
    {
        // Arrange
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = email;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EmailIsRequired));
    }
    
    [Fact]
    public void Error_Email_Invalid()
    {
        // Arrange
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "email.email";

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EmailIsNotValid));
    }
}