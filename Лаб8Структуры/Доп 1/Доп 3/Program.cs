using System;
using System.Collections.Generic;

enum WorkType { Book, Audiobook, Magazine, Article }
enum AuthorContribution { Author, CoAuthor, Translator }
enum SearchCriterion { Title, Author, Type }
enum SortOrder { Ascending, Descending }
enum AccessLevel { Basic = 1, Premium = 2 }

struct Author
{
    public string Name;
    public AuthorContribution Contribution;
    public Author(string name, AuthorContribution contribution) { Name = name; Contribution = contribution; }
}

struct Work
{
    public string Title;
    public WorkType Type;
    public Author[] Authors;
    public int Year;
    public AccessLevel Access;
    public int RatingSum, RatingCount, Borrowed;

    public Work(string title, WorkType type, Author[] authors, int year, AccessLevel access)
    {
        Title = title; Type = type; Authors = authors; Year = year; Access = access;
        RatingSum = RatingCount = Borrowed = 0;
    }

    public double Rating => RatingCount == 0 ? 0 : (double)RatingSum / RatingCount;
}

struct LibraryCard
{
    public string Reader;
    public AccessLevel Level;
    public List<int> History;   // индексы прочитанных произведений
    public int Fine;            // штраф, руб.

    public LibraryCard(string reader, AccessLevel level)
    {
        Reader = reader; Level = level; History = new List<int>(); Fine = 0;
    }
}

struct DigitalLibrary
{
    public Work[] Works;
    public LibraryCard[] Cards;
    const int FinePerDay = 10;

    public DigitalLibrary(Work[] works, LibraryCard[] cards) { Works = works; Cards = cards; }

    public void Borrow(int c, int w)
    {
        if (Cards[c].Level < Works[w].Access)
        {
            Console.WriteLine($"{Cards[c].Reader}: нет доступа к \"{Works[w].Title}\" (нужен {Works[w].Access})");
            return;
        }
        Works[w].Borrowed++;
        Cards[c].History.Add(w);
        Console.WriteLine($"{Cards[c].Reader} взял(а) \"{Works[w].Title}\"");
    }

    public void Return(int c, int w, int daysLate, int stars)
    {
        Works[w].RatingSum += stars;
        Works[w].RatingCount++;
        if (daysLate > 0) Cards[c].Fine += daysLate * FinePerDay;
        Console.WriteLine($"{Cards[c].Reader} вернул(а) \"{Works[w].Title}\", оценка {stars}, просрочка {daysLate} дн.");
    }

    public void Search(SearchCriterion criterion, string text, SortOrder order)
    {
        List<Work> found = new List<Work>();
        foreach (Work w in Works)
        {
            bool match = criterion switch
            {
                SearchCriterion.Title => w.Title.Contains(text),
                SearchCriterion.Type => w.Type.ToString() == text,
                _ => Array.Exists(w.Authors, a => a.Name.Contains(text))
            };
            if (match) found.Add(w);
        }
        found.Sort((a, b) => order == SortOrder.Ascending ? a.Year.CompareTo(b.Year) : b.Year.CompareTo(a.Year));

        Console.WriteLine($"Поиск: {criterion} = \"{text}\", {order}");
        foreach (Work w in found)
        {
            Console.Write($"  {w.Title} ({w.Type}, {w.Year}) - ");
            foreach (Author a in w.Authors) Console.Write($"{a.Name} [{a.Contribution}] ");
            Console.WriteLine();
        }
    }

    // Рекомендация: непрочитанное произведение того же типа, что и последнее прочитанное, с лучшим рейтингом
    public void Recommend(int c)
    {
        LibraryCard card = Cards[c];
        if (card.History.Count == 0) return;
        WorkType lastType = Works[card.History[card.History.Count - 1]].Type;
        int best = -1;
        for (int i = 0; i < Works.Length; i++)
        {
            bool fits = Works[i].Type == lastType && !card.History.Contains(i) && Works[i].Access <= card.Level;
            if (fits && (best == -1 || Works[i].Rating > Works[best].Rating)) best = i;
        }
        Console.WriteLine($"Рекомендация ({card.Reader}): {(best == -1 ? "нет" : Works[best].Title)}");
    }

    public void Report()
    {
        Console.WriteLine("Популярность:");
        foreach (Work w in Works)
            Console.WriteLine($"  {w.Title}: выдач {w.Borrowed}, рейтинг {w.Rating:F1}");
        Console.WriteLine("Должники:");
        foreach (LibraryCard c in Cards)
            if (c.Fine > 0) Console.WriteLine($"  {c.Reader}: штраф {c.Fine} руб.");
    }
}

class Program
{
    static void Main()
    {
        Work[] works =
        {
            new Work("Пикник на обочине", WorkType.Book,
                new[] { new Author("А. Стругацкий", AuthorContribution.Author), new Author("Б. Стругацкий", AuthorContribution.CoAuthor) },
                1972, AccessLevel.Basic),
            new Work("Солярис", WorkType.Book,
                new[] { new Author("С. Лем", AuthorContribution.Author), new Author("Д. Брускин", AuthorContribution.Translator) },
                1961, AccessLevel.Basic),
            new Work("Дюна", WorkType.Book,
                new[] { new Author("Ф. Херберт", AuthorContribution.Author) }, 1965, AccessLevel.Premium),
            new Work("Трудно быть богом", WorkType.Audiobook,
                new[] { new Author("А. Стругацкий", AuthorContribution.Author) }, 1964, AccessLevel.Basic)
        };
        LibraryCard[] cards =
        {
            new LibraryCard("Анна", AccessLevel.Premium),
            new LibraryCard("Борис", AccessLevel.Basic)
        };
        DigitalLibrary lib = new DigitalLibrary(works, cards);

        lib.Borrow(0, 0);
        lib.Return(0, 0, 0, 5);
        lib.Borrow(1, 2);          // нет доступа
        lib.Borrow(1, 1);
        lib.Return(1, 1, 4, 4);    // просрочка 4 дня
        lib.Borrow(1, 3);
        lib.Return(1, 3, 0, 5);

        Console.WriteLine();
        lib.Search(SearchCriterion.Author, "Стругацкий", SortOrder.Descending);
        lib.Search(SearchCriterion.Type, "Book", SortOrder.Ascending);

        Console.WriteLine();
        lib.Recommend(0);
        lib.Recommend(1);

        Console.WriteLine();
        lib.Report();
    }
}