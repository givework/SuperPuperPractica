using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level1;


public class Task11_MatrixDiagonal : TaskBase
{

    public Task11_MatrixDiagonal() : base(
        "Диагональ матрицы N×N",
        "Найти сумму элементов главной диагонали матрицы N×N.",
        new[]
        {
            "N вводит пользователь (минимум 2)",
            "Матрица заполняется случайными числами",
            "Сумма — без LINQ, в одном цикле for",
            "Вывести матрицу и сумму",
        },
        "Двумерный массив int[,], Random.")
    {
    }

   
    public override void Run()
    {
        int n = ConsoleHelper.ReadInt("Размер N (минимум 2): ", 2, 50);
        var rnd = new Random();
        int[,] m = new int[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                m[i, j] = rnd.Next(1, 100);
                Console.Write($"{m[i, j],4}");
            }

            Console.WriteLine();
        }

        int sum = 0;
        for (int i = 0; i < n; i++)
        {
            sum += m[i, i];
        }

        Console.WriteLine($"Сумма главной диагонали: {sum}");
    }
}
