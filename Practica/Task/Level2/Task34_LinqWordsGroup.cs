using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;

public class Task34_LinqWordsGroup : TaskBase
{
    public Task34_LinqWordsGroup() : base(
        "LINQ: группировка слов по первой букве",
        "Сгруппировать слова по первой букве.",
        new[]
        {
            "Список слов (10+)",
            "GroupBy(w => w[0])",
            "Группы — по первой букве, слова внутри — по алфавиту",
            "Вывод «Буква: слово1, слово2, ...»",
        },
        "GroupBy, OrderBy, string.Join.")
    {
    }

    public override void Run()
    {
        string[] words =
        {
            "яблоко", "арбуз", "банан", "виноград", "апельсин", "вишня",
            "груша", "брусника", "ананас", "голубика", "ежевика", "ягода",
        };

        foreach (var group in words.GroupBy(w => char.ToUpper(w[0])).OrderBy(g => g.Key))
        {
            Console.WriteLine($"{group.Key}: {string.Join(", ", group.OrderBy(w => w))}");
        }
    }
}
