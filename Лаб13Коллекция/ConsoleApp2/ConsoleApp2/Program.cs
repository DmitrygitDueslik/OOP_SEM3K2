using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main()
    {
        Dictionary<string, int> ages = new Dictionary<string, int>();
        HashSet<string> names = new HashSet<string>();

        ages.Add("Анна", 20);
        ages.Add("Иван", 19);
        names.Add("Анна");
        names.Add("Иван");
        Console.WriteLine("Add:");
        Console.WriteLine("  Словарь:   " + string.Join(", ", ages));
        Console.WriteLine("  Множество: " + string.Join(", ", names));

        Console.WriteLine("Повторный Add(\"Анна\"):");
        Console.WriteLine("  Множество вернуло: " + names.Add("Анна"));
        try
        {
            ages.Add("Анна", 25);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("  Словарь: ошибка, такой ключ уже есть");
        }

        Console.WriteLine($"Count: словарь {ages.Count}, множество {names.Count}");

        Console.WriteLine($"Есть Иван? словарь {ages.ContainsKey("Иван")}, множество {names.Contains("Иван")}");

        ages["Иван"] = 21;
        Console.WriteLine("Возраст Ивана: " + ages["Иван"]);

        ages.Remove("Анна");
        names.Remove("Анна");
        Console.WriteLine("После Remove(\"Анна\"):");
        Console.WriteLine("  Словарь:   " + string.Join(", ", ages));
        Console.WriteLine("  Множество: " + string.Join(", ", names));

        //---------------------------------
        HashSet<int> set = new HashSet<int> { 5, 1, 8, 3 };
        SortedSet<int> sortedSet = new SortedSet<int> { 5, 1, 8, 3 };

        Console.WriteLine("HashSet:   " + string.Join(" ", set));        // 5 1 8 3
        Console.WriteLine("SortedSet: " + string.Join(" ", sortedSet));  // 1 3 5 8

        SortedDictionary<int, string> rooms = new SortedDictionary<int, string>();
        rooms.Add(305, "Анна");
        rooms.Add(101, "Иван");
        rooms.Add(210, "Петр");

        foreach (var r in rooms)
            Console.WriteLine($"Комната {r.Key}: {r.Value}");

        SortedDictionary<string, int> ages2 = new SortedDictionary<string, int>();
        ages.Add("Петр", 25);
        ages.Add("Анна", 30);
        ages.Add("Иван", 19);

        Console.WriteLine("По ключу :");
        foreach (var a in ages2)
            Console.WriteLine($"  {a.Key} - {a.Value}");

        Console.WriteLine("По значению :");
        foreach (var a in ages2.OrderBy(x => x.Value))
            Console.WriteLine($"  {a.Key} - {a.Value}");
    }
}