using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level3;

public class Task39_YieldReturn : TaskBase
{
    public Task39_YieldReturn() : base(
        "yield return — генератор чётных чисел",
        "Реализовать генератор чётных чисел через yield return.",
        new[]
        {
            "IEnumerable<int> GetEvenNumbers(int max)",
            "Через yield return, без промежуточного списка",
            "Показать работу в foreach",
            "Показать ленивость: сообщение внутри генератора печатается по одному",
        },
        "yield return, IEnumerable<T>, отложенное выполнение.")
    {
    }

    public override void Run()
    {
        Console.WriteLine("Чётные до 10: " + string.Join(", ", GetEvenNumbers(10)));
        Console.WriteLine();
        Console.WriteLine("Ленивость (сообщения генератора чередуются с foreach):");
        foreach (int n in GetEvenNumbers(8, log: true))
        {
            Console.WriteLine($"  foreach получил {n}");
        }
    }

    public static IEnumerable<int> GetEvenNumbers(int max, bool log = false)
    {
        for (int i = 2; i <= max; i += 2)
        {
            if (log)
            {
                Console.WriteLine($"  [генератор] отдаю {i}");
            }

            yield return i;
        }
    }
}
