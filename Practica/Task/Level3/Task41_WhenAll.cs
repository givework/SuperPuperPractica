using System.Diagnostics;
using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level3;

public class Task41_WhenAll : TaskBase
{
    public Task41_WhenAll() : base(
        "Task.WhenAll — 10 задач параллельно",
        "Запустить 10 задач параллельно и сравнить с последовательным выполнением.",
        new[]
        {
            "Task<int> WorkAsync(int id) — Task.Delay(500), возвращает id",
            "Последовательный запуск в цикле с await",
            "Параллельный запуск через Task.WhenAll",
            "Замерить время обоих вариантов и объяснить разницу",
        },
        "Task.WhenAll, Task.Delay, Stopwatch.")
    {
    }

    public override void Run() => RunAsync().GetAwaiter().GetResult();

    private static async Task RunAsync()
    {
        var sw = Stopwatch.StartNew();
        var sequential = new List<int>();
        for (int i = 1; i <= 10; i++)
        {
            sequential.Add(await WorkAsync(i));
        }

        sw.Stop();
        long seqMs = sw.ElapsedMilliseconds;

        sw.Restart();
        int[] parallel = await Task.WhenAll(Enumerable.Range(1, 10).Select(WorkAsync));
        sw.Stop();
        long parMs = sw.ElapsedMilliseconds;

        Console.WriteLine($"Последовательно: {seqMs} мс → {string.Join(", ", sequential)}");
        Console.WriteLine($"Task.WhenAll:    {parMs} мс → {string.Join(", ", parallel)}");
        Console.WriteLine($"Результаты одинаковы: {sequential.SequenceEqual(parallel)}");
        Console.WriteLine("Разница: при await в цикле задержки складываются (10 × 500 мс), а WhenAll запускает все задачи сразу, и общее время ≈ самой долгой задаче (≈ 500 мс).");
    }

    public static async Task<int> WorkAsync(int id)
    {
        await Task.Delay(500);
        return id;
    }
}
