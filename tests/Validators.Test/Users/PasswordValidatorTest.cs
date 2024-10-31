using CashFlow.Application.UseCases.Users;
using CashFlow.Communication.Requests;
using FluentAssertions;
using FluentValidation;

namespace Validators.Test.Users;

public class PasswordValidatorTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("Aaaaaaaa")]
    [InlineData("A1aaaaaa")]
    public void Error_Password_Invalid(string password)
    {
        // Arrange
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        // Act
        var result = validator.IsValid(
            new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()),
            password);

        // Assert
        result.Should().BeFalse();
    }
}