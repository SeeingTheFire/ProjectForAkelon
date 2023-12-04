using ProjectForAkelon.VacationTask.Models;

namespace ProjectForAkelon.VacationTask;

class Program
{
    static void Main(string[] args)
    {
        // Создаем список сотрудников
        var employees = new HashSet<Employee>
        {
            new("Иванов", "Иван", "Иванович"),
            new("Петров", "Петр", "Петрович"),
            new("Юлина", "Юлия", "Юлиановна"),
            new("Сидоров", "Сидор", "Сидорович"),
            new("Павлов", "Павел", "Павлович"),
            new("Георгиев", "Георг", "Георгиевич")
        };

        var gen = new Random();
        int[] vacationSteps = { 7, 14 };
        foreach (var employee in employees)
        {
            var start = new DateTime(DateTime.Now.Year, 1, 1);
            var end = new DateTime(DateTime.Today.Year, 12, 31);

            var vacationCount = 28;
            var range = (end - start).Days;

            while (vacationCount > 0)
            {
                // Генерируем случайную дату начала отпуска
                var startDate = start.AddDays(gen.Next(range));
                if (startDate.DayOfWeek is not (not DayOfWeek.Sunday and DayOfWeek.Saturday))
                {
                    continue;
                }
                    
                var vacIndex = gen.Next(vacationSteps.Length);
                var difference = vacationCount <= 7 ? 7 : vacationSteps[vacIndex]; 
                var endDate = startDate.AddDays(difference);

                // Проверка условий по отпуску
                if (employees.Any(e => e.HasVacation(startDate, endDate)))
                {
                    continue;
                }

                if (!employee.AddVacation(startDate, endDate))
                {
                    continue;
                }

                vacationCount -= difference;
            }
        }
        
        foreach (var employee in employees)
        {
            employee.PrintVacations();
        }

        Console.ReadKey();
    }
}