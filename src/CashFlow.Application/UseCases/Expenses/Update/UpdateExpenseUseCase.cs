using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;
public class UpdateExpenseUseCase(
    IExpensesUpdateOnlyRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IUpdateExpenseUseCase
{
    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);

        var entity = await repository.GetById(id);

        if (entity is null)
        {
            throw new CashFlowNotFoundException(ResourceErrorMessages.ExpenseNotFound);
        }

        mapper.Map(request, entity);

        repository.Update(entity);

        await unitOfWork.Commit();
    }

    private static void Validate(RequestExpenseJson request)
    {
        var validator = new RequestExpenseValidator();

        var result = validator.Validate(request);

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(err => err.ErrorMessage).ToList();

        throw new CashFlowValidationErrorException(errorMessages: errorMessages);
    }
}
