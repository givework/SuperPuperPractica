using System.Diagnostics;
using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level3;

public class Task38_ListVsLinkedList : TaskBase
{
    private const int Size = 100_000;
    private const int Inserts = 1000;

    public Task38_ListVsLinkedList() : base(
        "List<T> vs LinkedList<T> (бенчмарк)",
        "Сравнить вставку в середину List<T> и LinkedList<T>.",
        new[]
        {
            "Создать List<int> и LinkedList<int> по 100 000 элементов",
            "Вставить 1000 элементов в середину каждой коллекции",
            "Замерить время через Stopwatch",
            "Вывести времена и объяснить результат в комментарии",
        },
        "List.Insert, LinkedList.AddBefore, LinkedListNode, Stopwatch.")
    {
    }

    public override void Run()
    {
        var list = Enumerable.Range(0, Size).ToList();
        var linked = new LinkedList<int>(Enumerable.Range(0, Size));

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Inserts; i++)
        {
            list.Insert(list.Count / 2, -1);
        }

        sw.Stop();
        double listInsert = sw.Elapsed.TotalMilliseconds;

        sw.Restart();
        LinkedListNode<int>? node = linked.First;
        for (int i = 0; i < linked.Count / 2 && node != null; i++)
        {
            node = node.Next;
        }

        if (node != null)
        {
            for (int i = 0; i < Inserts; i++)
            {
                linked.AddBefore(node, -1);
            }
        }

        sw.Stop();
        double linkedInsert = sw.Elapsed.TotalMilliseconds;

        sw.Restart();
        long sum = 0;
        for (int i = 0; i < Inserts; i++)
        {
            sum += list[list.Count / 2];
        }

        sw.Stop();
        double listAccess = sw.Elapsed.TotalMilliseconds;

        sw.Restart();
        for (int i = 0; i < Inserts; i++)
        {
            sum += linked.ElementAt(linked.Count / 2);
        }

        sw.Stop();
        double linkedAccess = sw.Elapsed.TotalMilliseconds;

        Console.WriteLine($"Вставка {Inserts} элементов в середину:");
        Console.WriteLine($"  List:       {listInsert:F3} мс");
        Console.WriteLine($"  LinkedList: {linkedInsert:F3} мс");
        Console.WriteLine($"Доступ по индексу к середине ({Inserts} раз):");
        Console.WriteLine($"  List:       {listAccess:F3} мс");
        Console.WriteLine($"  LinkedList: {linkedAccess:F3} мс (контрольная сумма {sum})");

        Console.WriteLine("Вывод: LinkedList быстрее на вставке (если узел известен), List — на доступе по индексу.");
    }
}
