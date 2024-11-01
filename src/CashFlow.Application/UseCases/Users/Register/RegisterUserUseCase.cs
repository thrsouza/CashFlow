using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.AccessToken;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Register;

public class RegisterUserUseCase(
    IUsersReadOnlyRepository usersReadOnlyRepository, 
    IUsersWriteOnlyRepository usersWriteOnlyRepository,
    IAccessTokenGenerator accessTokenGenerator,
    IPasswordEncryptor passwordEncryptor, 
    IUnitOfWork unitOfWork,
    IMapper mapper) 
    : IRegisterUserUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson requestRegister)
    {
        await Validate(requestRegister);
        
        var entity = mapper.Map<User>(requestRegister);
        entity.Password = passwordEncryptor.Encrypt(requestRegister.Password);
        entity.UserIdentifier = Guid.NewGuid();

        await usersWriteOnlyRepository.Add(entity);

        await unitOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Name = entity.Name,
            Token = accessTokenGenerator.Generate(entity)
        };
    }

    private async Task Validate(RequestRegisterUserJson requestRegister)
    {
        var result = await new RequestRegisterUserValidator().ValidateAsync(requestRegister);

        var emailAlreadyExists = await usersReadOnlyRepository.ExistActiveUserWithEmail(requestRegister.Email);

        if (emailAlreadyExists)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EmailAlreadyRegistered));
        }

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(err => err.ErrorMessage).ToList();
            
        throw new CashFlowValidationErrorException(errorMessages);
    }
}