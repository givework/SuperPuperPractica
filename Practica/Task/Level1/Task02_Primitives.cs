using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level1;

public class Task02_Primitives : TaskBase
{

    public Task02_Primitives() : base(
        "Переменные примитивных типов",
        "Объявить переменные примитивных типов и вывести их значения и типы.",
        new[]
        {
            "Минимум: int, long, double, float, decimal, bool, char, string",
            "Для каждой переменной вывести значение и GetType().Name",
            "decimal — с суффиксом m, float — с f",
        },
        "GetType().Name, суффиксы литералов: L, f, m.")
    {
    }

  
    public override void Run()
    {
        int i = 42;
        long l = 9_000_000_000L;
        double d = 3.14159;
        float f = 2.5f;
        decimal m = 1234.56m;
        bool b = true;
        char c = 'Z';
        string s = "Привет";

        Show(nameof(i), i, i.GetType().Name);
        Show(nameof(l), l, l.GetType().Name);
        Show(nameof(d), d, d.GetType().Name);
        Show(nameof(f), f, f.GetType().Name);
        Show(nameof(m), m, m.GetType().Name);
        Show(nameof(b), b, b.GetType().Name);
        Show(nameof(c), c, c.GetType().Name);
        Show(nameof(s), s, s.GetType().Name);
    }

    private static void Show(string name, object value, string type) =>
        Console.WriteLine($"{name} = {value} ({type})");
}
