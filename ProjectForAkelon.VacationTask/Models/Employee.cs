namespace ProjectForAkelon.VacationTask.Models;

/// <summary>
/// Сотрудник
/// </summary>
public record Employee
{
    /// <summary>
    /// Уникальный идентификатор сотрудника
    /// </summary>
    private readonly Guid _id = Guid.NewGuid();

    private readonly List<Vacation> _vacations = new();

    /// <summary>
    /// Сотрудник
    /// </summary>
    /// <param name="firstName">Имя</param>
    /// <param name="lastName">Фамилия</param>
    /// <param name="surname">Отчество</param>
    public Employee(string firstName, string lastName, string surname)
    {
        FirstName = firstName;
        LastName = lastName;
        Surname = surname;
        FullName = $"{firstName} {lastName} {surname}";
    }

    /// <summary>
    /// Имя
    /// </summary>
    private string FirstName { get; }

    /// <summary>
    /// Фамилия
    /// </summary>
    private string LastName { get; }

    /// <summary>
    /// Отчество
    /// </summary>
    private string Surname { get; }

    /// <summary>
    /// Полное имя
    /// </summary>
    public string FullName { get; }

    /// <summary>
    /// Отпуска сотрулдника
    /// </summary>
    public List<Vacation> Vacations => _vacations;

    /// <summary>
    /// Проверка существования отпуска с датами
    /// </summary>
    /// <param name="startDate">Дата начала отпуска</param>
    /// <param name="endDate">Дата конца отпуска</param>
    /// <returns></returns>
    public bool HasVacation(DateTime startDate, DateTime endDate)
    {
        return _vacations.Exists(v =>
            (v.StarDay <= startDate && v.EndDay.AddDays(3) >= startDate)
            || (v.StarDay <= endDate && v.EndDay.AddDays(3) >= endDate));
    }

    /// <summary>
    /// Метод добавления нового отпуска
    /// </summary>
    /// <param name="startDate">Дата начала</param>
    /// <param name="endDate">Дата конца</param>
    /// <returns>Успешность выполнения операции</returns>
    public bool AddVacation(DateTime startDate, DateTime endDate)
    {
        // Проверяем есть ли уже существующий отпуск в границах +-1 месяц от нового
        var exist = _vacations.Exists(v =>
            (v.StarDay.AddMonths(-1) <= startDate && v.EndDay.AddMonths(1) >= startDate)
            || (v.StarDay.AddMonths(-1) <= endDate && v.EndDay.AddMonths(1) >= endDate));

        if (exist)
        {
            return false;
        }

        _vacations.Add(new Vacation(startDate, endDate));
        return true;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }


    /// <inheritdoc />
    public virtual bool Equals(Employee? other)
    {
        return other != null && other._id == _id && other.FirstName == FirstName && other.LastName == LastName &&
               other.Surname == Surname;
    }

    /// <summary>
    /// Вывод в консоль информации об отпусках
    /// </summary>
    public void PrintVacations()
    {
        Console.WriteLine("Дни отпуска " + FullName + " : ");
        foreach (var vacation in _vacations.OrderBy(x => x.StarDay))
        {
            Console.WriteLine($"Дата начала отпуска - {vacation.StarDay:d} дата окончания - {vacation.EndDay:d}");
        }
    }
}