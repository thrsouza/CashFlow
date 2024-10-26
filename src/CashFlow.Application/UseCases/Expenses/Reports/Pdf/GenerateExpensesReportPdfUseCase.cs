using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;
public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "$";
    private const int EXPENSE_TABLE_ROW_HEIGHT = 25;
    private const int EXPENSE_TABLE_LEFT_INDENT = 16;

    private readonly IExpensesReadOnlyRepository _repository;

    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository)
    {
        this._repository = repository;
    }

    public async Task<byte[]> Execute(DateOnly date)
    {
        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        var expenses = await _repository.GetAllByDate(startDate, endDate);

        if (expenses.Count == 0)
            return [];

        return CreateReport(date: date, expenses: expenses);
    }

    private byte[] CreateReport(DateOnly date, IList<Expense> expenses)
    {
        var title = string.Format(ResourceReportGenerationMessages.EXPENSES_FOR, date.ToString("Y"));

        var document = new Document();
        document.Info.Title = title;
        document.Info.Author = "Thiago Souza";

        var style = document.Styles["Normal"]!;
        style.Font.Name = FontHelper.Raleway.REGULAR;

        var page = CreatePage(document);
        CreateHeaderWithProfilePhotoAndName(page);
        CreateTotalSpentSection(page, date, expenses.Sum(expense => expense.Amount));

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);

            var row = table.AddRow();
            row.Height = EXPENSE_TABLE_ROW_HEIGHT;

            AddExpenseTitleHeader(row.Cells[0], expense.Title);
            AddExpenseAmountHeader(row.Cells[3]);

            row = table.AddRow();
            row.Height = EXPENSE_TABLE_ROW_HEIGHT;

            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpenseInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = EXPENSE_TABLE_LEFT_INDENT;

            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(expense.PaymentType.ParseToString());
            SetStyleBaseForExpenseInformation(row.Cells[2]);

            AddExpenseAmountValue(row.Cells[3], expense.Amount);
            if (!string.IsNullOrWhiteSpace(expense.Description))
            {
                row.Cells[3].MergeDown = 1;
                AddExpenseDescriptionRow(table, expense.Description);
            }

            AddWhiteSpace(table);
        }

        return DocumentToByteArray(document);
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();

        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;

        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.LeftMargin = 40;

        return section;
    }

    private void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        var header = page.AddTable();
        header.AddColumn();
        header.AddColumn("360");

        var headerRow = header.AddRow();
        
        var assembly = Assembly.GetExecutingAssembly();
        var dirName = Path.GetDirectoryName(assembly.Location)!;

        var logo = headerRow.Cells[0].AddImage(Path.Combine(dirName, "Images", "Logo.png"));
        logo.Width = new Unit(56, UnitType.Point);

        headerRow.Cells[1].AddParagraph(string.Format(ResourceReportGenerationMessages.HELLO, "Thiago Souza"));
        headerRow.Cells[1].Format.Font = new Font { Name = FontHelper.Raleway.BLACK, Size = 16 };
        headerRow.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
        headerRow.Cells[1].Format.LeftIndent = 8;
    }

    private void CreateTotalSpentSection(Section page, DateOnly date, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        paragraph.AddFormattedText(
            string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, date.ToString("Y")),
            new Font { Name = FontHelper.Raleway.REGULAR, Size = 15 });

        paragraph.AddLineBreak();

        paragraph.AddFormattedText(
            $"{totalExpenses:#,##0.00} {CURRENCY_SYMBOL}",
            new Font { Name = FontHelper.WorkSans.BLACK, Size = 50 });
    }

    private Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;
        return table;
    }

    private void AddExpenseTitleHeader(Cell cell, string title)
    {
        cell.AddParagraph(title);
        cell.Format.Font = new Font { Name = FontHelper.Raleway.BLACK, Size = 14, Color = ColorHelper.BLACK };
        cell.Shading.Color = ColorHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = EXPENSE_TABLE_LEFT_INDENT;
    }

    private void AddExpenseAmountHeader(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font { Name = FontHelper.Raleway.BLACK, Size = 14, Color = ColorHelper.WHITE };
        cell.Shading.Color = ColorHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddExpenseAmountValue(Cell cell, decimal value)
    {
        cell.AddParagraph($"-{value:#,##0.00} {CURRENCY_SYMBOL}");
        cell.Format.Font = new Font { Name = FontHelper.WorkSans.REGULAR, Size = 12, Color = ColorHelper.BLACK };
        cell.Shading.Color = ColorHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddExpenseDescriptionRow(Table table, string description)
    {
        var row = table.AddRow();
        row.Height = EXPENSE_TABLE_ROW_HEIGHT;
        row.Cells[0].AddParagraph(description);
        row.Cells[0].Format.Font = new Font { Name = FontHelper.WorkSans.REGULAR, Size = 10, Color = ColorHelper.BLACK };
        row.Cells[0].Shading.Color = ColorHelper.GREEN_LIGHT;
        row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
        row.Cells[0].Format.LeftIndent = EXPENSE_TABLE_LEFT_INDENT;
        row.Cells[0].MergeRight = 2;
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.WorkSans.REGULAR, Size = 12, Color = ColorHelper.BLACK };
        cell.Shading.Color = ColorHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private byte[] DocumentToByteArray(Document document)
    {
        var renderer = new PdfDocumentRenderer { Document = document };
        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}
