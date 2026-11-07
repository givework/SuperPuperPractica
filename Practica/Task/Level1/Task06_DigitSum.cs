using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level1;


public class Task06_DigitSum : TaskBase
{
 
    public Task06_DigitSum() : base(
        "Сумма цифр числа",
        "Найти сумму цифр введённого числа (1234 → 10) двумя способами.",
        new[]
        {
            "Способ 1: ToString() → символы → int.Parse",
            "Способ 2: while (n > 0) { sum += n % 10; n /= 10; }",
            "Оба способа — отдельными методами",
            "Отрицательные числа — брать модуль",
        },
        "Math.Abs, long.ToString(), int.Parse.")
    {
    }

  
    public override void Run()
    {
        long n = ConsoleHelper.ReadInt("Введите целое число: ");
        Print(n);

        Console.WriteLine("Дополнительные проверки:");
        foreach (long test in new long[] { 1234, -987, 0, 100500 })
        {
            Print(test);
        }
    }

    private static void Print(long n) =>
        Console.WriteLine($"{n}: строка = {SumByString(n)}, арифметика = {SumByMath(n)}");

    public static int SumByString(long n)
    {
        long abs = n == long.MinValue ? long.MaxValue : Math.Abs(n);
        int sum = 0;
        foreach (char c in abs.ToString())
        {
            sum += int.Parse(c.ToString());
        }

        return sum;
    }


    public static int SumByMath(long n)
    {
        long abs = n == long.MinValue ? long.MaxValue : Math.Abs(n);
        int sum = 0;
        while (abs > 0)
        {
            sum += (int)(abs % 10);
            abs /= 10;
        }

        return sum;
    }
}
