using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level1;


public class Task08_FizzBuzz : TaskBase
{

    public Task08_FizzBuzz() : base(
        "FizzBuzz 1–100",
        "Вывести числа от 1 до 100 с заменами.",
        new[]
        {
            "Кратно 3 → Fizz, кратно 5 → Buzz, кратно 15 → FizzBuzz",
            "Иначе — само число",
            "Порядок проверок: сначала 15, потом 3 и 5",
        },
        "for, if / else if, оператор %.")
    {
    }


    public override void Run()
    {
        for (int i = 1; i <= 100; i++)
        {
            if (i % 15 == 0)
            {
                Console.WriteLine("FizzBuzz");
            }
            else if (i % 3 == 0)
            {
                Console.WriteLine("Fizz");
            }
            else if (i % 5 == 0)
            {
                Console.WriteLine("Buzz");
            }
            else
            {
                Console.WriteLine(i);
            }
        }
    }
}
