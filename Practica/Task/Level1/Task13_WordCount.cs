using System.Text;
using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level1;


public class Task13_WordCount : TaskBase
{

    public Task13_WordCount() : base(
        "Счётчик слов (Dictionary)",
        "Посчитать, сколько раз каждое слово встречается в тексте.",
        new[]
        {
            "Dictionary<string, int>",
            "Разбить на слова по пробелам и знакам пунктуации",
            "Привести к нижнему регистру",
            "Вывести «слово → количество» по убыванию количества",
        },
        "Dictionary<TKey,TValue>, TryGetValue, OrderByDescending (System.Linq).")
    {
    }


    public override void Run()
    {
        string text = ConsoleHelper.ReadLine("Введите текст (Enter — тестовый текст): ");
        if (string.IsNullOrWhiteSpace(text))
        {
            text = "Мама мыла раму. Мама, мама! Рама чистая, а мама устала. Раму мыла мама.";
            Console.WriteLine($"Текст: {text}");
        }

        Dictionary<string, int> counts = CountWords(text);
        foreach (var pair in counts.OrderByDescending(p => p.Value).ThenBy(p => p.Key))
        {
            Console.WriteLine($"{pair.Key} → {pair.Value}");
        }
    }

    public static Dictionary<string, int> CountWords(string text)
    {
        var result = new Dictionary<string, int>();
        var word = new StringBuilder();

        void Flush()
        {
            if (word.Length == 0)
            {
                return;
            }

            string w = word.ToString();
            result[w] = result.TryGetValue(w, out int c) ? c + 1 : 1;
            word.Clear();
        }

        foreach (char ch in text.ToLower())
        {
            if (char.IsLetterOrDigit(ch))
            {
                word.Append(ch);
            }
            else
            {
                Flush();
            }
        }

        Flush();
        return result;
    }
}
