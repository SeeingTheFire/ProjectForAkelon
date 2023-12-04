using ProjectForAkelon.ExcelTask.Services;
using Spectre.Console;

namespace ProjectForAkelon.ExcelTask;

public class Application(IDataController dataController) : IApplication
{
    public void Run()
    {
        while (true)
        {
            // Даем выбор пользователю
            var selectFunctions = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(10)
                    .WrapAround()
                    .HighlightStyle(new Style(Color.Aqua, Color.Black))
                    .Title("[green]Выберите действие:[/]")
                    .MoreChoicesText("[grey](Двигайте вверх и вниз стрелочками чтобы выбрать элемент)[/]")
                    .AddChoices("Получение пользователей заказавших выбранный продукт",
                        "Изменение контактного лица компании",
                        "Получение золотого клиента за выбранный период",
                        "Выйти"));

            // В зависимости от выбора пользователя запускаем функцию
            switch (selectFunctions)
            {
                case "Получение пользователей заказавших выбранный продукт":
                {
                    dataController.GetUsersWhoOrderedProduct(AnsiConsole.Ask<string>("Введите наименование товара"));
                    break;
                }
                case "Изменение контактного лица компании":
                {
                    dataController.ChangeClientName(AnsiConsole.Ask<string>("Введите название организации"));
                    break;
                }
                case "Получение золотого клиента за выбранный период":
                {
                    dataController.GetGoldenClient(AnsiConsole.Ask<int>("Введите год"), AnsiConsole.Ask<int>("Введите месяц"));
                    break;
                }
                case "Выйти":
                {
                    return;
                }
            }

            Emoji.Replace("[green]Увидимся еще [/] :birthday_cake:");
        }
    }
}