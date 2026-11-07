using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;


public class Task21_EnumDays : TaskBase
{

    public enum DayOfWeek
    {
 
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
    }

    public Task21_EnumDays() : base(
        "enum дни недели",
        "Описать дни недели через enum и определить тип дня.",
        new[]
        {
            "enum DayOfWeek { Monday, ..., Sunday }",
            "IsWeekend(DayOfWeek) — true для субботы и воскресенья",
            "GetRussianName(DayOfWeek) — название на русском",
            "Показать на всех 7 днях",
        },
        "enum, Enum.GetValues, switch-выражение.")
    {
    }


    public override void Run()
    {
        foreach (DayOfWeek day in Enum.GetValues<DayOfWeek>())
        {
            Console.WriteLine($"{day,-10} {GetRussianName(day),-12} выходной: {(IsWeekend(day) ? "да" : "нет")}");
        }
    }

    public static bool IsWeekend(DayOfWeek day) => day == DayOfWeek.Saturday || day == DayOfWeek.Sunday;

    public static string GetRussianName(DayOfWeek day) => day switch
    {
        DayOfWeek.Monday => "Понедельник",
        DayOfWeek.Tuesday => "Вторник",
        DayOfWeek.Wednesday => "Среда",
        DayOfWeek.Thursday => "Четверг",
        DayOfWeek.Friday => "Пятница",
        DayOfWeek.Saturday => "Суббота",
        DayOfWeek.Sunday => "Воскресенье",
        _ => "Неизвестно",
    };
}
