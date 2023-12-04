namespace ProjectForAkelon.ExcelTask.Models;

/// <summary>
/// Заявка
/// </summary>
public class PurchaseRequest(int applicationId, Client client, int quantity, DateTime postingDate)
    : BaseClass(applicationId)
{
    /// <summary>
    /// Клиент
    /// </summary>
    public Client Client { get; } = client;
    
    /// <summary>
    /// Количество
    /// </summary>
    public int Quantity { get; } = quantity;
    
    /// <summary>
    /// Дата заявки
    /// </summary>
    public DateTime PostingDate { get; } = postingDate;
};