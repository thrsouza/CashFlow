using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.AuthenticatedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Update;

public class UpdateUserUseCase(
    IAuthenticatedUserService authenticatedUserService,
    IUsersUpdateOnlyRepository userUpdateOnlyRepository,
    IUsersReadOnlyRepository usersReadOnlyRepository,
    IUnitOfWork unitOfWork) : IUpdateUserUseCase
{
    public async Task Execute(RequestUpdateUserJson request)
    {
        var authenticatedUser = await authenticatedUserService.Get();

        await Validate(request, authenticatedUser.Email);

        var user = await userUpdateOnlyRepository.GetById(authenticatedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;
        
        userUpdateOnlyRepository.Update(user);
        
        await unitOfWork.Commit();
    }
    
    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();
        
        var result = await validator.ValidateAsync(request);

        if (!currentEmail.Equals(request.Email))
        {
            var emailAlreadyExists = await usersReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (emailAlreadyExists)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EmailAlreadyRegistered));
        }
        
        if (!result.IsValid) 
        {
            var errorMessages = result.Errors.Select(err => err.ErrorMessage).ToList();
            
            throw new CashFlowValidationErrorException(errorMessages);
        }
    }   
}