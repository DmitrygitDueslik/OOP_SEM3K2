using System;

// Комбинирует предикаты: каждый метод возвращает НОВЫЙ композер
internal class PredicateComposer<T>
{
    private Predicate<T> current;

    // Начальное условие - «всегда true»
    public PredicateComposer() : this(x => true) { }

    private PredicateComposer(Predicate<T> predicate)
    {
        current = predicate;
    }

    public PredicateComposer<T> And(Predicate<T> other)
    {
        Predicate<T> prev = current;
        return new PredicateComposer<T>(x => prev(x) && other(x));
    }

    public PredicateComposer<T> Or(Predicate<T> other)
    {
        Predicate<T> prev = current;
        return new PredicateComposer<T>(x => prev(x) || other(x));
    }

    public PredicateComposer<T> Not()
    {
        Predicate<T> prev = current;
        return new PredicateComposer<T>(x => !prev(x));
    }

    public Predicate<T> Build()
    {
        return current;
    }
}

internal class Program
{
    private static void Main()
    {
        int[] numbers = { -4, -1, 0, 3, 5, 6, 9, 12 };
        Console.WriteLine("Массив: " + string.Join(", ", numbers));

        // Пример из задания: НЕ (x > 5 ИЛИ x < 0)  =>  0 <= x <= 5
        Predicate<int> p1 = new PredicateComposer<int>()
            .And(x => x > 5)
            .Or(x => x < 0)
            .Not()
            .Build();
        Console.WriteLine("not (x > 5 or x < 0): " + string.Join(", ", Array.FindAll(numbers, p1)));

        // Чётные и положительные
        Predicate<int> p2 = new PredicateComposer<int>()
            .And(x => x % 2 == 0)
            .And(x => x > 0)
            .Build();
        Console.WriteLine("чётные и > 0:         " + string.Join(", ", Array.FindAll(numbers, p2)));

        // То же для строк
        string[] words = { "кот", "Москва", "дом", "Волга", "лес" };
        Predicate<string> p3 = new PredicateComposer<string>()
            .And(s => s.Length > 3)
            .Or(s => s.StartsWith("д"))
            .Build();
        Console.WriteLine("\nСлова: " + string.Join(", ", words));
        Console.WriteLine("длина > 3 или начинается с 'д': " + string.Join(", ", Array.FindAll(words, p3)));
    }
}