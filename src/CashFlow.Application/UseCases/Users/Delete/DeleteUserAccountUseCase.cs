using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.AuthenticatedUser;

namespace CashFlow.Application.UseCases.Users.Delete;

public class DeleteUserAccountUseCase(
    IAuthenticatedUserService authenticatedUserService,
    IUsersWriteOnlyRepository usersWriteOnlyRepository,
    IUnitOfWork unitOfWork) : IDeleteUserAccountUseCase
{
    public async Task Execute()
    {
        var authenticatedUser = authenticatedUserService.Get();
        
        await usersWriteOnlyRepository.Delete(authenticatedUser.Id);
        
        await unitOfWork.Commit();
    }
}