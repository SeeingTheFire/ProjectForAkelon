namespace ProjectForAkelon.VacationTask.Models;

/// <summary>
/// Отпуск
/// </summary>
/// <param name="starDay">Дата начала</param>
/// <param name="endDay">Дата конца</param>
public readonly struct Vacation(DateTime starDay, DateTime endDay)
{
    /// <summary>
    /// Дата начала
    /// </summary>
    public DateTime StarDay { get; } = starDay;

    /// <summary>
    /// Дата конца
    /// </summary>
    public DateTime EndDay { get; } = endDay;
}