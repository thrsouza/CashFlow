using CashFlow.Application.AutoMapper;
using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Application.UseCases.Users.Register;
using Microsoft.Extensions.DependencyInjection;
using PdfSharp.Fonts;

namespace CashFlow.Application;
public static class ApplicationServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMapper(services);
        AddUseCases(services);
        ApplyFontResolver();
    }

    private static void ApplyFontResolver()
    {
        GlobalFontSettings.FontResolver = new ExpensesReportPdfFontResolver();
    }


    private static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }

    private static void AddUseCases(this IServiceCollection services)
    {
        // Expenses
        services.AddScoped<IGetAllExpenseUseCase, GetAllExpenseUseCase>();
        services.AddScoped<IGetByIdExpenseUseCase, GetByIdExpenseUseCase>();
        services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
        services.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();
        services.AddScoped<IUpdateExpenseUseCase, UpdateExpenseUseCase>();
        services.AddScoped<IGenerateExpensesReportExcelUseCase, GenerateExpensesReportExcelUseCase>();
        services.AddScoped<IGenerateExpensesReportPdfUseCase, GenerateExpensesReportPdfUseCase>();
        
        // Users
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
    }
}
