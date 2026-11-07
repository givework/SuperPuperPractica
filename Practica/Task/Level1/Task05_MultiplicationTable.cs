using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level1;


public class Task05_MultiplicationTable : TaskBase
{

    public Task05_MultiplicationTable() : base(
        "Таблица умножения 10×10",
        "Вывести таблицу умножения 10×10.",
        new[]
        {
            "Вложенные циклы for",
            "Выровнять столбцы",
            "Заголовки строк/столбцов приветствуются",
        },
        "Вложенный for, string.PadLeft или форматирование {0,4}.")
    {
    }


    public override void Run()
    {
        Console.Write("    |");
        for (int j = 1; j <= 10; j++)
        {
            Console.Write($"{j,4}");
        }

        Console.WriteLine();
        Console.WriteLine(new string('-', 5) + new string('-', 40));

        for (int i = 1; i <= 10; i++)
        {
            Console.Write($"{i,3} |");
            for (int j = 1; j <= 10; j++)
            {
                Console.Write($"{i * j,4}");
            }

            Console.WriteLine();
        }
    }
}
