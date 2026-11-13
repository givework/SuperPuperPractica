using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level3;

public class Task42_CancellationToken : TaskBase
{
    public Task42_CancellationToken() : base(
        "CancellationToken — отмена через 2 сек",
        "Реализовать долгую операцию с поддержкой отмены.",
        new[]
        {
            "async Task LongOperationAsync(CancellationToken token): 10 раз Task.Delay(500, token)",
            "CancellationTokenSource, отмена через 2 секунды",
            "Обработать OperationCanceledException",
            "Вывести «операция отменена»",
        },
        "CancellationTokenSource.CancelAfter, OperationCanceledException.")
    {
    }

    public override void Run() => RunAsync().GetAwaiter().GetResult();

    private static async Task RunAsync()
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(2));

        try
        {
            await LongOperationAsync(cts.Token);
            Console.WriteLine("Операция завершена.");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("операция отменена");
        }
    }

    public static async Task LongOperationAsync(CancellationToken token)
    {
        for (int i = 1; i <= 10; i++)
        {
            await Task.Delay(500, token);
            Console.WriteLine($"  шаг {i}/10");
        }
    }
}
