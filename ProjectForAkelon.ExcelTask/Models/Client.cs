namespace ProjectForAkelon.ExcelTask.Models;

/// <summary>
/// Клиент
/// </summary>
/// <param name="clientId">Идентификатор клиента</param>
/// <param name="clientName">Имя клиента</param>
public class Client(int clientId, string clientName)
    : BaseClass(clientId)
{
    /// <summary>
    /// Имя клиента
    /// </summary>
    public string ClientName { get; } = clientName;

    /// <summary>
    /// Заявки клиента
    /// </summary>
    public List<PurchaseRequest> PurchaseRequests { get; } = new();
};