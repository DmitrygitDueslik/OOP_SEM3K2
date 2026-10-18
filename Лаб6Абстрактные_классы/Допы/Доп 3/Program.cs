using System;
using System.Collections.Generic;
using System.Linq;

// Абстрактный класс единицы библиотечного фонда
public abstract class LibraryItem
{
    public string InventoryNumber { get; }
    public string Title { get; }
    public int Year { get; }

    public bool IsIssued { get; private set; }         // статус - изменяется только через Issue()/Return()
    public DateTime? DueDate { get; private set; }

    private readonly List<string> _history = new List<string>(); // история выдачи

    protected LibraryItem(string inventoryNumber, string title, int year)
    {
        InventoryNumber = inventoryNumber;
        Title = title;
        Year = year;
    }

    // Срок выдачи в днях и штраф за день просрочки - у каждого типа свои
    public abstract int GetLoanPeriod();
    public abstract decimal GetFinePerDay();

    public bool Issue(DateTime issueDate)
    {
        if (IsIssued)
        {
            Console.WriteLine($"«{Title}» уже выдана - повторная выдача невозможна");
            return false;
        }
        IsIssued = true;
        DueDate = issueDate.AddDays(GetLoanPeriod());
        _history.Add($"Выдана {issueDate:dd.MM.yyyy}, вернуть до {DueDate:dd.MM.yyyy}");
        Console.WriteLine($"«{Title}» выдана до {DueDate:dd.MM.yyyy}");
        return true;
    }

    public decimal Return(DateTime returnDate)
    {
        if (!IsIssued)
        {
            Console.WriteLine($"«{Title}» не была выдана");
            return 0;
        }
        decimal fine = 0;
        if (DueDate.HasValue && returnDate > DueDate.Value)
        {
            int overdueDays = (returnDate - DueDate.Value).Days;
            fine = overdueDays * GetFinePerDay();
            _history.Add($"Возвращена {returnDate:dd.MM.yyyy} с опозданием на {overdueDays} дн., штраф {fine:F2} руб.");
            Console.WriteLine($"«{Title}» возвращена с опозданием на {overdueDays} дн. Штраф: {fine:F2} руб.");
        }
        else
        {
            _history.Add($"Возвращена {returnDate:dd.MM.yyyy} вовремя");
            Console.WriteLine($"«{Title}» возвращена вовремя");
        }
        IsIssued = false;
        DueDate = null;
        return fine;
    }

    public void PrintHistory()
    {
        Console.WriteLine($"История «{Title}» ({InventoryNumber}):");
        foreach (var entry in _history)
            Console.WriteLine($"  {entry}");
    }
}

public class Book : LibraryItem
{
    public string Author { get; }
    public int Pages { get; }

    public Book(string inventoryNumber, string title, int year, string author, int pages)
        : base(inventoryNumber, title, year)
    {
        Author = author;
        Pages = pages;
    }

    public override int GetLoanPeriod() => 21;      // книги выдаются на 3 недели
    public override decimal GetFinePerDay() => 5m;
}

public class Magazine : LibraryItem
{
    public int IssueNumber { get; }
    public string Topic { get; }

    public Magazine(string inventoryNumber, string title, int year, int issueNumber, string topic)
        : base(inventoryNumber, title, year)
    {
        IssueNumber = issueNumber;
        Topic = topic;
    }

    public override int GetLoanPeriod() => 7;       // журналы - на неделю
    public override decimal GetFinePerDay() => 2m;
}

public class Dvd : LibraryItem
{
    public string Director { get; }
    public int DurationMinutes { get; }

    public Dvd(string inventoryNumber, string title, int year, string director, int durationMinutes)
        : base(inventoryNumber, title, year)
    {
        Director = director;
        DurationMinutes = durationMinutes;
    }

    public override int GetLoanPeriod() => 3;       // диски - на 3 дня
    public override decimal GetFinePerDay() => 10m;
}

// Простейший каталог с поиском
public class Library
{
    private readonly List<LibraryItem> _items = new List<LibraryItem>();

    public void Add(LibraryItem item) => _items.Add(item);

    public IEnumerable<LibraryItem> SearchByTitle(string part) =>
        _items.Where(i => i.Title.Contains(part, StringComparison.OrdinalIgnoreCase));
}

class Program
{
    static void Main()
    {
        Library library = new Library();

        Book book = new Book("B-001", "Война и мир", 1869, "Лев Толстой", 1225);
        Magazine magazine = new Magazine("M-010", "Наука и жизнь", 2024, 3, "Наука");
        Dvd dvd = new Dvd("D-050", "Интерстеллар", 2014, "Кристофер Нолан", 169);

        library.Add(book);
        library.Add(magazine);
        library.Add(dvd);

        DateTime issueDate = new DateTime(2026, 9, 1);
        book.Issue(issueDate);
        dvd.Issue(issueDate);

        Console.WriteLine("\nПопытка повторно выдать уже выданную книгу:");
        book.Issue(new DateTime(2026, 9, 5)); // отклонено

        Console.WriteLine("\nВозврат с опозданием:");
        book.Return(new DateTime(2026, 9, 30)); // просрочка книги (срок 21 день -> до 22.09)
        dvd.Return(new DateTime(2026, 9, 3));   // диск вовремя (срок 3 дня -> до 04.09)

        Console.WriteLine();
        book.PrintHistory();

        Console.WriteLine("\nПоиск по названию \"наука\":");
        foreach (var item in library.SearchByTitle("наука"))
            Console.WriteLine($"  Найдено: {item.Title} ({item.Year})");
    }
}
