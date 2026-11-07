using System.Diagnostics;
using System.Text;
using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;


public class Task17_StringBuilderVsConcat : TaskBase
{

    public Task17_StringBuilderVsConcat() : base(
        "StringBuilder vs конкатенация",
        "Сравнить производительность конкатенации через + и StringBuilder.",
        new[]
        {
            "Собрать строку из 10 000 (или 100 000) чисел двумя способами",
            "Замерить время через Stopwatch",
            "Вывести оба времени и разницу",
            "Объяснить в комментарии, почему StringBuilder быстрее",
        },
        "System.Text.StringBuilder, System.Diagnostics.Stopwatch.")
    {
    }

    public override void Run()
    {
        string input = ConsoleHelper.ReadLine("Количество чисел (Enter = 10000, можно 100000): ");
        int n = int.TryParse(input, out int parsed) && parsed > 0 ? parsed : 10_000;

        var sw = Stopwatch.StartNew();
        string concat = string.Empty;
        for (int i = 0; i < n; i++)
        {
            concat += i;
        }

        sw.Stop();
        long concatMs = sw.ElapsedMilliseconds;

        sw.Restart();
        var sb = new StringBuilder();
        for (int i = 0; i < n; i++)
        {
            sb.Append(i);
        }

        string built = sb.ToString();
        sw.Stop();
        long sbMs = sw.ElapsedMilliseconds;

        Console.WriteLine($"Конкатенация (+): {concatMs} мс");
        Console.WriteLine($"StringBuilder:   {sbMs} мс");
        Console.WriteLine($"Разница:         {concatMs - sbMs} мс");
        Console.WriteLine($"Строки равны:    {concat == built}");

        Console.WriteLine("Причина: string неизменяема, каждое += копирует всю строку (O(n²)); StringBuilder дописывает в буфер (O(n)).");
    }
}
