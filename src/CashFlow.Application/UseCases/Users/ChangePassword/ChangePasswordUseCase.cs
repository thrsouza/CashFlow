using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Services.AuthenticatedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.ChangePassword;

public class ChangePasswordUseCase(
    IAuthenticatedUserService authenticatedUserService,
    IUsersUpdateOnlyRepository userUpdateOnlyRepository,
    IPasswordEncryptor passwordEncryptor,
    IUnitOfWork unitOfWork) : IChangePasswordUseCase
{
    public async Task Execute(RequestChangePasswordJson request)
    {
        var authenticatedUser = await authenticatedUserService.Get();
        
        await Validate(request, authenticatedUser);
        
        var user = await userUpdateOnlyRepository.GetById(authenticatedUser.Id);
        
        user.Password = passwordEncryptor.Encrypt(request.NewPassword);
        
        userUpdateOnlyRepository.Update(user);
        
        await unitOfWork.Commit();
    }

    private async Task Validate(RequestChangePasswordJson request, User user)
    {
        var validator = new ChangePasswordValidator();
        
        var result = await validator.ValidateAsync(request);
        
        if (!passwordEncryptor.Verify(request.Password, user.Password))
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PasswordIncorrect));
        }
        
        if (!result.IsValid) 
        {
            var errorMessages = result.Errors.Select(err => err.ErrorMessage).ToList();
            
            throw new CashFlowValidationErrorException(errorMessages);
        }
    }
}