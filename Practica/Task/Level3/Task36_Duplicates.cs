using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level3;
public class Task36_Duplicates : TaskBase
{
    public Task36_Duplicates() : base(
        "Дубликаты в List (HashSet + GroupBy)",
        "Найти дубликаты в списке чисел двумя способами.",
        new[]
        {
            "Способ 1: HashSet<int> — при добавлении проверять Contains",
            "Способ 2: LINQ GroupBy(x => x).Where(g => g.Count() > 1)",
            "Оба способа — отдельными методами",
            "Показать на списке с повторяющимися значениями",
        },
        "HashSet<T>.Contains/Add, GroupBy, Where, Count.")
    {
    }

    public override void Run()
    {
        var numbers = new List<int> { 1, 2, 3, 2, 4, 5, 3, 6, 2, 7, 7 };
        Console.WriteLine("Список: " + string.Join(", ", numbers));

        List<int> a = FindWithHashSet(numbers);
        List<int> b = FindWithGroupBy(numbers);

        Console.WriteLine("HashSet:  " + string.Join(", ", a));
        Console.WriteLine("GroupBy:  " + string.Join(", ", b));
        Console.WriteLine($"Наборы совпадают: {a.SequenceEqual(b)}");
    }

    public static List<int> FindWithHashSet(List<int> numbers)
    {
        var seen = new HashSet<int>();
        var duplicates = new HashSet<int>();
        foreach (int x in numbers)
        {
            if (seen.Contains(x))
            {
                duplicates.Add(x);
            }
            else
            {
                seen.Add(x);
            }
        }

        var result = duplicates.ToList();
        result.Sort();
        return result;
    }

    public static List<int> FindWithGroupBy(List<int> numbers) =>
        numbers.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).OrderBy(x => x).ToList();
}
