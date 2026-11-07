using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level1;


public class Task12_Palindrome : TaskBase
{

    public Task12_Palindrome() : base(
        "Палиндром",
        "Проверить, является ли строка палиндромом.",
        new[]
        {
            "Игнорировать регистр",
            "Игнорировать пробелы и знаки пунктуации",
            "Без LINQ Reverse",
            "Пустая строка и один символ — палиндром",
        },
        "char.IsLetterOrDigit, char.ToLower, два индекса (с начала и с конца).")
    {
    }


    public override void Run()
    {
        string s = ConsoleHelper.ReadLine("Введите строку: ");
        Console.WriteLine($"\"{s}\" → {IsPalindrome(s)}");
        Console.WriteLine($"Проверки: {IsPalindrome("А роза упала на лапу Азора")} (ожидается True), {IsPalindrome("hello")} (ожидается False)");
    }


    public static bool IsPalindrome(string s)
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
            else
            {
                if (char.ToLower(s[i]) != char.ToLower(s[j]))
                {
                    return false;
                }

                i++;
                j--;
            }
        }

        return true;
    }
}
