using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;

public abstract class Animal
{
    protected Animal(string name) => Name = name;

    public string Name { get; }

    public abstract string MakeSound();

    public override string ToString() => $"{GetType().Name} «{Name}»";
}

public class Dog : Animal
{
    public Dog(string name) : base(name)
    {
    }

    public override string MakeSound() => "Гав!";
}

public class Cat : Animal
{
    public Cat(string name) : base(name)
    {
    }

    public override string MakeSound() => "Мяу!";
}

public class Bird : Animal
{
    public Bird(string name) : base(name)
    {
    }

    public override string MakeSound() => "Чирик!";

    public string Fly() => $"{Name} летит";
}

public class Task24_Animals : TaskBase
{

    public Task24_Animals() : base(
        "ООП: Animal → Dog/Cat/Bird",
        "Построить иерархию животных и показать полиморфизм.",
        new[]
        {
            "Абстрактный Animal: Name, абстрактный MakeSound(), ToString()",
            "Dog, Cat, Bird — наследники; у Bird ещё Fly()",
            "List<Animal> и вызов MakeSound() в цикле",
        },
        "abstract, override, наследование, List<T>, pattern matching (is Bird).")
    {
    }

    public override void Run()
    {
        var animals = new List<Animal> { new Dog("Шарик"), new Cat("Мурка"), new Bird("Кеша") };
        foreach (Animal a in animals)
        {
            Console.WriteLine($"{a} говорит: {a.MakeSound()}");
            if (a is Bird bird)
            {
                Console.WriteLine($"  {bird.Fly()}");
            }
        }
    }
}
