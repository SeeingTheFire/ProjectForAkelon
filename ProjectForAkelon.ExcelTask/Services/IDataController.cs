namespace ProjectForAkelon.ExcelTask.Services;

/// <summary>
/// Контроллер данных
/// </summary>
public interface IDataController
{
    /// <summary>
    /// Получение пользователей заказавших выбранный продукт
    /// </summary>
    /// <param name="goodName">Имя товара</param>
    public void GetUsersWhoOrderedProduct(string goodName);

    /// <summary>
    /// Изменение контактного лица компании
    /// </summary>
    /// <param name="companyName">Наименование компании</param>
    public void ChangeClientName(string companyName);

    /// <summary>
    /// Получение золотого клиента за выбранный период
    /// </summary>
    /// <param name="year">Год</param>
    /// <param name="month">Месяц</param>
    public void GetGoldenClient(int year, int month);
}