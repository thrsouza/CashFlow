using CashFlow.Domain.Entities;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

internal class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    private const string CURRENCY_SYMBOL = "$";
    private readonly IExpensesReadOnlyRepository _repository;

    public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository)
    {
        this._repository = repository;
    }

    public async Task<byte[]> Execute(DateOnly date)
    {
        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        var expenses = await  _repository.GetAllByDate(startDate, endDate);

        if (expenses.Count == 0)
            return [];

        var title = $"{ResourceReportGenerationMessages.EXPENSES_FOR}  {date:Y}";

        return CreateReport(date: date, expenses: expenses);
    }

    private byte[] CreateReport(DateOnly date, IList<Expense> expenses)
    {
        using var workbook = new XLWorkbook();

        workbook.Author = "Thiago Souza";
        workbook.Style.Font.FontSize = 10;
        workbook.Style.Font.FontName = "Calibri";

        var title = string.Format(ResourceReportGenerationMessages.EXPENSES_FOR, date.ToString("Y"));
        var worksheet = workbook.Worksheets.Add(title);

        InsertHeader(worksheet);

        var row = 2;
        foreach (var expense in expenses)
        {
            worksheet.Cell($"A{row}").Value = expense.Title;
            worksheet.Cell($"B{row}").Value = expense.Date;
            worksheet.Cell($"C{row}").Value = expense.PaymentType.ParseToString();
            
            worksheet.Cell($"D{row}").Value = expense.Amount;
            worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"- {CURRENCY_SYMBOL} #,##0.00";

            worksheet.Cell($"E{row}").Value = expense.Description;

            row++;
        }

        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.Black;
        worksheet.Cells("A1:E1").Style.Font.FontColor = XLColor.White;
        worksheet.Cells("A1:E1").Style.Font.Bold = true;

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    }
}
