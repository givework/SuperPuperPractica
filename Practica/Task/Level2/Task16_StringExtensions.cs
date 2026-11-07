using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;

public static class StringExtensions
{

    public static bool IsPalindrome(this string s)
    {
        int i = 0;
        int j = s.Length - 1;
        while (i < j)
        {
            if (!char.IsLetterOrDigit(s[i]))
            {
                i++;
            }
            else if (!char.IsLetterOrDigit(s[j]))
            {
                j--;
            }
            else if (char.ToLower(s[i++]) != char.ToLower(s[j--]))
            {
                return false;
            }
        }

        return true;
    }

    public static int WordCount(this string s) =>
        s.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;


    public static string ReverseString(this string s)
    {
        char[] chars = s.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}

public class Task16_StringExtensions : TaskBase
{

    public Task16_StringExtensions() : base(
        "Методы расширения для string",
        "Написать extension-методы IsPalindrome(), WordCount(), ReverseString().",
        new[]
        {
            "Статический класс StringExtensions",
            "public static, первый параметр this string",
            "Продемонстрировать вызов на 2–3 строках",
        },
        "static class, this string, Split с StringSplitOptions.RemoveEmptyEntries.")
    {
    }


    public override void Run()
    {
        foreach (string s in new[] { "abc", "А роза упала на лапу Азора", "Привет, мир и все остальные" })
        {
            Console.WriteLine($"\"{s}\": палиндром = {s.IsPalindrome()}, слов = {s.WordCount()}, реверс = \"{s.ReverseString()}\"");
        }
    }
}
