using System.Diagnostics;
using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level1;


public class Task10_Sorting : TaskBase
{

    public Task10_Sorting() : base(
        "Сортировка пузырьком + вставками",
        "Отсортировать массив двумя алгоритмами и сравнить время.",
        new[]
        {
            "BubbleSort и InsertionSort — отдельные методы",
            "Без LINQ и Array.Sort",
            "Замер Stopwatch на массиве 10 000 элементов",
            "Вывести оба времени",
        },
        "System.Diagnostics.Stopwatch, Array.Clone.")
    {
    }


    public override void Run()
    {
        var rnd = new Random();
        int[] source = new int[10_000];
        for (int i = 0; i < source.Length; i++)
        {
            source[i] = rnd.Next(0, 100_000);
        }

        int[] a = (int[])source.Clone();
        int[] b = (int[])source.Clone();

        var sw = Stopwatch.StartNew();
        BubbleSort(a);
        sw.Stop();
        Console.WriteLine($"BubbleSort:    {sw.ElapsedMilliseconds} мс");

        sw.Restart();
        InsertionSort(b);
        sw.Stop();
        Console.WriteLine($"InsertionSort: {sw.ElapsedMilliseconds} мс");

        bool same = true;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
            {
                same = false;
                break;
            }
        }

        Console.WriteLine($"Массивы отсортированы одинаково: {same}");
    }


    public static void BubbleSort(int[] a)
    {
        for (int i = 0; i < a.Length - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < a.Length - 1 - i; j++)
            {
                if (a[j] > a[j + 1])
                {
                    (a[j], a[j + 1]) = (a[j + 1], a[j]);
                    swapped = true;
                }
            }

            if (!swapped)
            {
                break;
            }
        }
    }


    public static void InsertionSort(int[] a)
    {
        for (int i = 1; i < a.Length; i++)
        {
            int key = a[i];
            int j = i - 1;
            while (j >= 0 && a[j] > key)
            {
                a[j + 1] = a[j];
                j--;
            }

            a[j + 1] = key;
        }
    }
}
