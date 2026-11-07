using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level2;

public class MyStack<T>
{
    private T[] _items = new T[4];

    public int Count { get; private set; }

    public bool IsEmpty => Count == 0;

    public void Push(T item)
    {
        if (Count == _items.Length)
        {
            Array.Resize(ref _items, _items.Length * 2);
        }

        _items[Count++] = item;
    }

    public T Pop()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Стек пуст.");
        }

        T item = _items[--Count];
        _items[Count] = default!;
        return item;
    }

    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Стек пуст.");
        }

        return _items[Count - 1];
    }
}

public class MyQueue<T>
{
    private T[] _items = new T[4];
    private int _head;

    public int Count { get; private set; }

    public bool IsEmpty => Count == 0;

    public void Enqueue(T item)
    {
        if (Count == _items.Length)
        {
            var bigger = new T[_items.Length * 2];
            for (int i = 0; i < Count; i++)
            {
                bigger[i] = _items[(_head + i) % _items.Length];
            }

            _items = bigger;
            _head = 0;
        }

        _items[(_head + Count) % _items.Length] = item;
        Count++;
    }

    public T Dequeue()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Очередь пуста.");
        }

        T item = _items[_head];
        _items[_head] = default!;
        _head = (_head + 1) % _items.Length;
        Count--;
        return item;
    }

    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Очередь пуста.");
        }

        return _items[_head];
    }
}

public class Task19_StackQueue : TaskBase
{

    public Task19_StackQueue() : base(
        "Свой стек/очередь на массиве",
        "Реализовать MyStack<T> и MyQueue<T> на массиве.",
        new[]
        {
            "Стек: Push, Pop, Peek, Count, IsEmpty",
            "Очередь: Enqueue, Dequeue, Peek, Count, IsEmpty",
            "При переполнении — удвоение массива",
            "Pop/Dequeue на пустой коллекции — InvalidOperationException",
        },
        "Обобщения <T>, Array.Resize, кольцевой буфер.")
    {
    }

    public override void Run()
    {
        var stack = new MyStack<int>();
        for (int i = 1; i <= 6; i++)
        {
            stack.Push(i * 10);
        }

        Console.WriteLine($"Стек: Count={stack.Count}, Peek={stack.Peek()}");
        Console.WriteLine($"Pop: {stack.Pop()}, {stack.Pop()}; Count={stack.Count}, IsEmpty={stack.IsEmpty}");

        var queue = new MyQueue<string>();
        foreach (string s in new[] { "a", "b", "c", "d", "e", "f" })
        {
            queue.Enqueue(s);
        }

        Console.WriteLine($"Очередь: Count={queue.Count}, Peek={queue.Peek()}");
        Console.WriteLine($"Dequeue: {queue.Dequeue()}, {queue.Dequeue()}; Count={queue.Count}");
        queue.Enqueue("g");
        Console.WriteLine($"После Enqueue(g): Count={queue.Count}, IsEmpty={queue.IsEmpty}");

        var empty = new MyStack<int>();
        try
        {
            empty.Pop();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Pop на пустом стеке: {ex.Message}");
        }

        var emptyQ = new MyQueue<int>();
        try
        {
            emptyQ.Dequeue();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Dequeue на пустой очереди: {ex.Message}");
        }
    }
}
