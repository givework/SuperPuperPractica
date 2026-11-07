using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;

public class Task23_Nullable : TaskBase
{
    private static readonly Dictionary<string, int> Storage = new()
    {
        ["age"] = 30,
        ["count"] = 5,
    };

    public Task23_Nullable() : base(
        "Nullable-типы, ??, ?.",
        "Продемонстрировать работу с nullable.",
        new[]
        {
            "int? FindValue(string key) — null, если ключа нет",
            "?? для значения по умолчанию",
            "?. для безопасного доступа к свойству",
            "HasValue / Value с проверкой",
        },
        "int?, оператор ??, оператор ?., HasValue, Value.")
    {
    }

    public override void Run()
    {
        foreach (string key in new[] { "age", "missing" })
        {
            int? value = FindValue(key);

            Console.WriteLine($"Ключ \"{key}\":");
            Console.WriteLine($"  ?? : {value ?? -1}");

            if (value.HasValue)
            {
                Console.WriteLine($"  HasValue/Value : {value.Value}");
            }
            else
            {
                Console.WriteLine("  HasValue/Value : значения нет");
            }
        }

        string? name = null;
        string? other = "Claude";
        Console.WriteLine($"name?.Length  = {name?.Length.ToString() ?? "null"}");
        Console.WriteLine($"other?.Length = {other?.Length.ToString() ?? "null"}");
    }

    public static int? FindValue(string key) => Storage.TryGetValue(key, out int v) ? v : null;
}
