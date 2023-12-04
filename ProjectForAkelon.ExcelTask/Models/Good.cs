namespace ProjectForAkelon.ExcelTask.Models;

/// <summary>
/// Товары
/// </summary>
/// <param name="goodId">Идентификатор товара</param>
/// <param name="name">Имя</param>
/// <param name="units">Единицы измерения</param>
/// <param name="price">Цена</param>
public class Good(int goodId, string name, string units, decimal price)
    : BaseClass(goodId)
{
    /// <summary>
    /// Имя товара
    /// </summary>
    public string Name { get; } = name;
    
    /// <summary>
    /// Единицы измерения
    /// </summary>
    public string Units { get; } = units;
    
    /// <summary>
    /// Цена
    /// </summary>
    public decimal Price { get; } = price;

    /// <summary>
    /// Заявки
    /// </summary>
    public List<PurchaseRequest> PurchaseRequests { get; } = new();
};