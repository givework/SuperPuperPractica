using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;


public class Task18_TryParseVsTryCatch : TaskBase
{

    public Task18_TryParseVsTryCatch() : base(
        "TryParse vs try/catch",
        "Сравнить два способа обработки некорректного ввода числа.",
        new[]
        {
            "Способ 1: int.TryParse — bool + значение через out",
            "Способ 2: int.Parse в try/catch (FormatException)",
            "Оба — отдельными методами",
            "Показать на корректном и некорректном вводе",
        },
        "int.TryParse, out-параметры, FormatException.")
    {
    }


    public override void Run()
    {
        foreach (string s in new[] { "123", "abc" })
        {
            bool ok = TryParseWay(s, out int v1);
            int? v2 = TryCatchWay(s);
            Console.WriteLine($"\"{s}\": TryParse → {(ok ? v1.ToString() : "ошибка")}, try/catch → {(v2?.ToString() ?? "ошибка")}");
        }

        string user = ConsoleHelper.ReadLine("Введите своё значение: ");
        bool okUser = TryParseWay(user, out int r);
        Console.WriteLine(okUser ? $"Получено число {r}" : "Это не число");
    }

    public static bool TryParseWay(string s, out int value) => int.TryParse(s, out value);

    public static int? TryCatchWay(string s)
    {
        try
        {
            return int.Parse(s);
        }
        catch (FormatException)
        {
            return null;
        }
        catch (OverflowException)
        {
            return null;
        }
    }
}
