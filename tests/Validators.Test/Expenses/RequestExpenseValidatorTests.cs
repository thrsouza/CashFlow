using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exeption;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Test.Expenses;

public class RequestExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new RequestExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Error_Title_Empty(string title)
    {
        // Arrange
        var validator = new RequestExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = title;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_IS_REQUIRED));
    }


    [Fact]
    public void Error_Date_Future()
    {
        // Arrange
        var validator = new RequestExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.DATE_CONNOT_BE_FOR_THE_FUTURE));
    }


    [Fact]
    public void Error_PaymentType_Invalid()
    {
        // Arrange
        var validator = new RequestExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.PaymentType = (PaymentType)999;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_IS_NOT_VALID));
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-7)]
    public void Error_Amount_GreaterThanZero(decimal amount)
    {
        // Arrange
        var validator = new RequestExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Amount = amount;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }
}
