using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level3;

public class Task40_AsyncAwait : TaskBase
{
    public Task40_AsyncAwait() : base(
        "async/await + обработка исключений",
        "Асинхронный метод с эмуляцией «скачивания».",
        new[]
        {
            "async Task<string> DownloadAsync(string url) с Task.Delay(1000)",
            "Пустой url — ArgumentException",
            "Вызов через await внутри try/catch",
            "Показать успешный и ошибочный вызов",
        },
        "async/await, Task.Delay, ArgumentException, try/catch.")
    {
    }

    public override void Run() => RunAsync().GetAwaiter().GetResult();

    private static async Task RunAsync()
    {
        foreach (string url in new[] { "https://example.com", string.Empty })
        {
            try
            {
                Console.WriteLine($"Скачиваю \"{url}\"...");
                string result = await DownloadAsync(url);
                Console.WriteLine($"Успех: {result}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    public static async Task<string> DownloadAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL не может быть пустым.", nameof(url));
        }

        await Task.Delay(1000);
        return $"<html>содержимое {url}</html>";
    }
}
