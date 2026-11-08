using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;

public enum NotificationType
{
    Email,

    Sms,

    Push,
}

public interface INotification
{
    void Send(string message);
}

public class EmailNotification : INotification
{
    public void Send(string message) => Console.WriteLine($"[Email] {message}");
}

public class SmsNotification : INotification
{
    public void Send(string message) => Console.WriteLine($"[SMS] {message}");
}

public class PushNotification : INotification
{
    public void Send(string message) => Console.WriteLine($"[Push] {message}");
}

public static class NotificationFactory
{
    public static INotification Create(NotificationType type) => type switch
    {
        NotificationType.Email => new EmailNotification(),
        NotificationType.Sms => new SmsNotification(),
        NotificationType.Push => new PushNotification(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Неизвестный тип уведомления."),
    };
}

public class Task29_NotificationFactory : TaskBase
{
    public Task29_NotificationFactory() : base(
        "Паттерн «Фабрика» (уведомления)",
        "Реализовать фабрику уведомлений.",
        new[]
        {
            "INotification.Send(string message)",
            "EmailNotification, SmsNotification, PushNotification",
            "NotificationFactory.Create(NotificationType type)",
            "enum NotificationType { Email, Sms, Push }",
        },
        "interface, enum, switch-выражение, статический фабричный метод.")
    {
    }

    public override void Run()
    {
        foreach (NotificationType type in Enum.GetValues<NotificationType>())
        {
            INotification n = NotificationFactory.Create(type);
            Console.Write($"{type} → {n.GetType().Name}: ");
            n.Send("Привет из фабрики!");
        }
    }
}
