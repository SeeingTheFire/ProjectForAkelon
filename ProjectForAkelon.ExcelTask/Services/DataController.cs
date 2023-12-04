using System.Globalization;
using OfficeOpenXml;
using ProjectForAkelon.ExcelTask.Models;
using Spectre.Console;

namespace ProjectForAkelon.ExcelTask.Services;

/// <inheritdoc />
public class DataController : IDataController
{
    /// <summary>
    /// Строка для подключения к документу
    /// </summary>
    private string? _answer;

    /// <summary>
    /// Клиенты
    /// </summary>
    private List<Client> Clients { get; } = new();

    /// <summary>
    /// Товары
    /// </summary>
    private List<Good> Goods { get; } = new();

    /// <summary>
    /// Конструктор контроллера данных
    /// </summary>
    public DataController()
    {
        AnsiConsole.Prompt(
            new TextPrompt<string>("Ведите путь до файла Excel:")
                .PromptStyle("green")
                .Validate(answer =>
                {
                    if (string.IsNullOrWhiteSpace(answer))
                    {
                        return ValidationResult.Error("[red]Путь не может быть пустой строкой[/]");
                    }

                    try
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using var package = new ExcelPackage(answer);
                        _answer = answer;
                        var workbook = package.Workbook;

                        var goodsSheet = workbook.Worksheets["Товары"];
                        var clientsSheet = workbook.Worksheets["Клиенты"];
                        var purchaseRequestsSheet = workbook.Worksheets["Заявки"];

                        GetGoods(goodsSheet);
                        GetClients(clientsSheet);
                        GetPurchaseRequests(purchaseRequestsSheet);
                    }
                    catch (Exception)
                    {
                        return ValidationResult.Error("[red]Некорректный путь[/]");
                    }

                    return ValidationResult.Success();
                }));
    }

    /// <summary>
    /// Метод для формирования коллекции товаров
    /// </summary>
    /// <param name="sheet">Лист с товарами</param>
    private void GetGoods(ExcelWorksheet sheet)
    {
        // Формируем товары из строк листа
        var i = 2;
        while (i < sheet.Rows.EndRow && sheet.Cells["A" + i].GetCellValue<int>() is var goodId && goodId != 0)
        {
            var goodName = sheet.Cells["B" + i].GetCellValue<string>();
            var goodUnits = sheet.Cells["C" + i].GetCellValue<string>();
            var goodPrice = sheet.Cells["D" + i].GetCellValue<decimal>();
            if (!Goods.Exists(g => g.Id == goodId) && !string.IsNullOrWhiteSpace(goodName) &&
                !string.IsNullOrWhiteSpace(goodUnits))
            {
                Goods.Add(new Good(goodId, goodName, goodUnits, goodPrice));
            }

            i++;
        }
    }

    /// <summary>
    /// Метод для формирования коллекции клиентов
    /// </summary>
    /// <param name="sheet">Лист с клиентами</param>
    private void GetClients(ExcelWorksheet sheet)
    {
        // Формируем клиентов из строк листа
        var i = 2;
        while (i < sheet.Rows.EndRow && sheet.Cells["A" + i].GetCellValue<int>() is var clientId && clientId != 0)
        {
            var address = sheet.Cells["C" + i].GetCellValue<string>();
            var clientName = sheet.Cells["D" + i].GetCellValue<string>();
            if (!Clients.Exists(g => g.Id == clientId) && !string.IsNullOrWhiteSpace(clientName) &&
                !string.IsNullOrWhiteSpace(address) && !string.IsNullOrWhiteSpace(clientName))
            {
                Clients.Add(new Client(clientId, clientName));
            }

            i++;
        }
    }

    /// <summary>
    /// Метод для формирования заявок
    /// </summary>
    /// <param name="sheet">Лист с заявками</param>
    private void GetPurchaseRequests(ExcelWorksheet sheet)
    {
        // Формируем заявки из строк листа
        var i = 2;
        while (i < sheet.Rows.EndRow && sheet.Cells["A" + i].GetCellValue<int>() is var purchaseRequestId &&
               purchaseRequestId != 0)
        {
            var goodId = sheet.Cells["B" + i].GetCellValue<int>();
            var clientId = sheet.Cells["C" + i].GetCellValue<int>();
            var quantity = sheet.Cells["E" + i].GetCellValue<int>();
            var postingDate = sheet.Cells["F" + i].GetCellValue<DateTime>();

            if (Goods.FirstOrDefault(g => g.Id == goodId) is { } good &&
                Clients.FirstOrDefault(c => c.Id == clientId) is { } client)
            {
                var purchaseRequest = new PurchaseRequest(purchaseRequestId, client, quantity, postingDate);
                good.PurchaseRequests.Add(purchaseRequest);
                client.PurchaseRequests.Add(purchaseRequest);
            }

            i++;
        }
    }


    /// <inheritdoc />
    public void GetUsersWhoOrderedProduct(string goodName)
    {
        // Находим товар
        var good = Goods.FirstOrDefault(x => x.Name == goodName);
        if (good is null)
        {
            AnsiConsole.MarkupLine("[red]Товара с таким именем нету в базе данных.[/]");
            return;
        }

        // Формируем таблицу
        var table = new Table();

        table.AddColumn("Клиент");
        table.AddColumn("Количество товара");
        table.AddColumn("Цена");
        table.AddColumn("Дата заказа");
        var price = good.Price;

        good.PurchaseRequests.ForEach(purchaseRequest =>
        {
            table.AddRow(
                purchaseRequest.Client.ClientName,
                $"{purchaseRequest.Quantity} {good.Units}",
                price.ToString(CultureInfo.InvariantCulture),
                purchaseRequest.PostingDate.ToString("d"));
        });


        // Отображаем таблицу с пользователями
        AnsiConsole.Write(table);
    }

    /// <inheritdoc />
    public void ChangeClientName(string companyName)
    {
        try
        {
            // Подключаемся к документу
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(_answer);

            var clientsSheet = package.Workbook.Worksheets["Клиенты"];
            var i = 2;

            // Находим необходимую компанию
            while (clientsSheet.Cells["B" + i] is var cell && cell.GetCellValue<string>() is var name &&
                   !string.IsNullOrWhiteSpace(name))
            {
                if (name != companyName)
                {
                    i++;
                    continue;
                }

                // Заменяем имя и выводим сообщение
                var newFullName = AnsiConsole.Ask<string>("Введите ФИО нового контактного лица");
                var oldName = clientsSheet.Cells["D" + i].GetCellValue<string>();
                clientsSheet.Cells["D" + i].Value = newFullName;
                package.SaveAsync();
                AnsiConsole.MarkupLine($"[green]Контактное лицо компании [yellow]{name}[/] [springgreen3_1]\"{oldName
                }\"[/] было заменено на [mediumspringgreen]\"{newFullName}\"[/][/]");
                return;
            }

            AnsiConsole.MarkupLine("[red]Не удалось найти компанию[/]");
        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine("[red]Не удалось подключиться к документу[/]");
        }
    }

    /// <inheritdoc />
    public void GetGoldenClient(int year, int month)
    {
        var goldenClients = Clients
            .Select(x => (Client: x,
                PurchaseRequestsCount: x.PurchaseRequests.Count(z => z.PostingDate.Year == year && z.PostingDate.Month == month)))
            .ToList();

        if (!goldenClients.Exists(x => x.PurchaseRequestsCount > 0))
        {
            AnsiConsole.MarkupLine("[red]За данный период не было найдено [gold3]золотого пользователя[/][/]");
            return;
        }

        AnsiConsole.MarkupLine($"[gold3]{goldenClients.MaxBy(x => x.Item2).Client.ClientName}[/]");
    }
}