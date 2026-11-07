using System.Globalization;
using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level1;


public class Task14_Age : TaskBase
{

    public Task14_Age() : base(
        "Возраст по дате рождения",
        "По дате рождения вычислить возраст в годах, месяцах и днях.",
        new[]
        {
            "Ввод через DateTime.TryParse / TryParseExact",
            "Учесть, что день рождения в этом году мог ещё не наступить",
            "Вывести «X лет, Y месяцев, Z дней»",
            "Некорректная дата — сообщение, без падения",
        },
        "DateTime.TryParseExact, DateTime.AddMonths, CultureInfo.")
    {
    }

 
    public override void Run()
    {
        string input = ConsoleHelper.ReadLine("Дата рождения (дд.мм.гггг): ").Trim();
        if (!DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birth))
        {
            Console.WriteLine("Ошибка: некорректная дата.");
            return;
        }

        DateTime today = DateTime.Today;
        if (birth > today)
        {
            Console.WriteLine("Ошибка: дата рождения в будущем.");
            return;
        }

        var (years, months, days) = CalculateAge(birth, today);
        Console.WriteLine($"{years} лет, {months} месяцев, {days} дней");
    }

    public static (int Years, int Months, int Days) CalculateAge(DateTime birth, DateTime today)
    {
        int totalMonths = ((today.Year - birth.Year) * 12) + today.Month - birth.Month;
        if (birth.AddMonths(totalMonths) > today)
        {
            totalMonths--;
        }

        int days = (today - birth.AddMonths(totalMonths)).Days;
        return (totalMonths / 12, totalMonths % 12, days);
    }
}
