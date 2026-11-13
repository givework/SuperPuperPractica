using CSharpPracticum.Common;

namespace CSharpPracticum.Tasks.Level3;

public class Book
{
    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }

    public string Title { get; }

    public string Author { get; }

    public bool IsAvailable { get; internal set; } = true;

    public override string ToString() => $"«{Title}» — {Author}";
}

public class Reader
{
    internal List<Book> Borrowed { get; } = new();

    public Reader(string name) => Name = name;

    public string Name { get; }

    public IReadOnlyList<Book> BorrowedBooks => Borrowed;
}

public class BookNotAvailableException : InvalidOperationException
{
    public BookNotAvailableException(Book book) : base($"Книга {book} уже выдана.")
    {
    }
}

public class BorrowLimitExceededException : InvalidOperationException
{
    public BorrowLimitExceededException(Reader reader, int limit)
        : base($"Читатель {reader.Name} уже взял максимум книг ({limit}).")
    {
    }
}

public class Library
{
    public const int MaxBooksPerReader = 3;

    private readonly List<Book> _books = new();

    public IReadOnlyList<Book> Books => _books;

    public void AddBook(Book book) => _books.Add(book);

    public void Borrow(Reader reader, Book book)
    {
        if (!_books.Contains(book))
        {
            throw new ArgumentException("Такой книги нет в библиотеке.", nameof(book));
        }

        if (!book.IsAvailable)
        {
            throw new BookNotAvailableException(book);
        }

        if (reader.Borrowed.Count >= MaxBooksPerReader)
        {
            throw new BorrowLimitExceededException(reader, MaxBooksPerReader);
        }

        book.IsAvailable = false;
        reader.Borrowed.Add(book);
    }

    public void Return(Reader reader, Book book)
    {
        if (!reader.Borrowed.Remove(book))
        {
            throw new InvalidOperationException($"Читатель {reader.Name} не брал книгу {book}.");
        }

        book.IsAvailable = true;
    }
}

public class Task45_Library : TaskBase
{
    public Task45_Library() : base(
        "Мини-проект «Библиотека» + юнит-тесты",
        "Классы библиотеки с выдачей и возвратом книг; тесты лежат в проекте CSharpPracticum.Tests.",
        new[]
        {
            "Book (Title, Author, IsAvailable), Reader (Name, BorrowedBooks), Library (выдача/возврат)",
            "Лимит — не больше 3 книг на читателя",
            "Книга уже выдана / лимит превышен — исключения",
            "5+ юнит-тестов (xUnit), все проходят",
        },
        "Тесты: Test Explorer в Visual Studio или команда dotnet test (проект CSharpPracticum.Tests).")
    {
    }
    public override void Run()
    {
        var library = new Library();
        var books = new[]
        {
            new Book("Мастер и Маргарита", "Булгаков"),
            new Book("Война и мир", "Толстой"),
            new Book("Преступление и наказание", "Достоевский"),
            new Book("Анна Каренина", "Толстой"),
        };
        foreach (Book b in books)
        {
            library.AddBook(b);
        }

        var anna = new Reader("Анна");
        var boris = new Reader("Борис");

        Try("Анна берёт 3 книги", () =>
        {
            library.Borrow(anna, books[0]);
            library.Borrow(anna, books[1]);
            library.Borrow(anna, books[2]);
        });
        Try("Анна берёт 4-ю книгу (лимит)", () => library.Borrow(anna, books[3]));
        Try("Борис берёт уже выданную книгу", () => library.Borrow(boris, books[0]));
        Try("Анна возвращает «Мастера и Маргариту»", () => library.Return(anna, books[0]));
        Try("Борис берёт её же", () => library.Borrow(boris, books[0]));

        Console.WriteLine($"У Анны: {string.Join("; ", anna.BorrowedBooks)}");
        Console.WriteLine($"У Бориса: {string.Join("; ", boris.BorrowedBooks)}");
    }

    private static void Try(string title, Action action)
    {
        try
        {
            action();
            Console.WriteLine($"OK:     {title}");
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
        {
            Console.WriteLine($"ОШИБКА: {title} → {ex.Message}");
        }
    }
}
