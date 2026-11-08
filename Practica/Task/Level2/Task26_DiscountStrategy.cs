using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;

public interface IDiscountStrategy
{
    decimal Apply(decimal price);
}

public class NoDiscount : IDiscountStrategy
{
    public decimal Apply(decimal price) => price;
}

public class VipDiscount : IDiscountStrategy
{
    public decimal Apply(decimal price) => price * 0.90m;
}

public class PromoDiscount : IDiscountStrategy
{
    public decimal Apply(decimal price) => price * 0.80m;
}

public class PriceCalculator
{
    private readonly IDiscountStrategy _strategy;

    public PriceCalculator(IDiscountStrategy strategy) => _strategy = strategy;

    public decimal Calculate(decimal price) => _strategy.Apply(price);
}

public class Task26_DiscountStrategy : TaskBase
{
    public Task26_DiscountStrategy() : base(
        "Паттерн «Стратегия» (скидки)",
        "Реализовать калькулятор скидок через паттерн «Стратегия».",
        new[]
        {
            "IDiscountStrategy.Apply(decimal price)",
            "NoDiscount, VipDiscount (10%), PromoDiscount (20%)",
            "PriceCalculator принимает стратегию в конструкторе",
            "Расчёт для каждой стратегии",
        },
        "interface, внедрение зависимости через конструктор, decimal.")
    {
    }

    public override void Run()
    {
        decimal price = 1000m;
        var strategies = new (string Name, IDiscountStrategy Strategy)[]
        {
            ("Без скидки", new NoDiscount()),
            ("VIP (10%)", new VipDiscount()),
            ("Промо (20%)", new PromoDiscount()),
        };

        foreach (var (name, strategy) in strategies)
        {
            var calc = new PriceCalculator(strategy);
            Console.WriteLine($"{name,-12}: {price} → {calc.Calculate(price)}");
        }
    }
}
