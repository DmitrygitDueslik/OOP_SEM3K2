using System;

public interface ITriple<T>
{
    void SetValues(T first, T second, T third);

    void GetValues(out T first, out T second, out T third);

    bool HasDuplicates();
}

public class Triple<T> : ITriple<T> where T : IEquatable<T>
{
    private T first = default!;
    private T second = default!;
    private T third = default!;
    private bool isSet; //были ли значения заданы

    private static void Log(string message)
    {
        Console.WriteLine($"   [LOG Triple<{typeof(T).Name}>] {message}");
    }

    public void SetValues(T first, T second, T third)
    {
        try
        {
            // для ссылочных типов (например, string) null считается некорректным значением
            if (first is null || second is null || third is null)
            {
                throw new ArgumentNullException(first is null ? nameof(first) : second is null ? nameof(second) : nameof(third),
                                                "Значения тройки не могут быть null");
            }
            this.first = first;
            this.second = second;
            this.third = third;
            isSet = true;
        }
        finally
        {
            Log($"SetValues({Show(first)}, {Show(second)}, {Show(third)}) завершён, данные заданы: {isSet}");
        }
    }

    public void GetValues(out T first, out T second, out T third)
    {
        try
        {
            if (!isSet)
            {
                throw new InvalidOperationException("Значения ещё не заданы - сначала вызовите SetValues");
            }
            first = this.first;
            second = this.second;
            third = this.third;
        }
        finally
        {
            Log("GetValues завершён");
        }
    }

    public bool HasDuplicates()
    {
        bool result = false;
        try
        {
            if (!isSet)
            {
                throw new InvalidOperationException("Нельзя проверить дубликаты: значения не заданы");
            }
            // Equals(T) из IEquatable<T> — без упаковки для значимых типов
            result = first.Equals(second) || first.Equals(third) || second.Equals(third);
            return result;
        }
        finally
        {
            Log($"HasDuplicates -> {result}");
        }
    }

    private static string Show(T value) => value is null ? "null" : value.ToString()!;
}

internal class Program
{
    private static void Demo<T>(string title, T a, T b, T c) where T : IEquatable<T>
    {
        Console.WriteLine($"\n=== {title} ===");
        ITriple<T> triple = new Triple<T>();
        try
        {
            triple.SetValues(a, b, c);
            triple.GetValues(out T x, out T y, out T z);
            Console.WriteLine($"Значения: {x}, {y}, {z}");
            Console.WriteLine($"Есть повторы: {(triple.HasDuplicates() ? "да" : "нет")}");
        }
        catch (ArgumentNullException ex)
        {
            // ex.Message дополняется служебным текстом на английском, поэтому выводим ParamName
            Console.WriteLine($"Ошибка: параметр {ex.ParamName} равен null - значения тройки не могут быть null");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private static void Main()
    {
        Demo("Triple<int>", 7, 3, 7);
        Demo("Triple<char>", 'a', 'b', 'c');
        Demo("Triple<string>", "C#", "Java", "Python");
        // null! — намеренно передаём null (знак ! отключает предупреждение компилятора)
        Demo<string>("Triple<string> с null", "C#", null!, "Python");

        Console.WriteLine("\n=== Чтение незаполненной тройки ===");
        Triple<double> empty = new Triple<double>();
        try
        {
            empty.GetValues(out double p, out double q, out double r);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}