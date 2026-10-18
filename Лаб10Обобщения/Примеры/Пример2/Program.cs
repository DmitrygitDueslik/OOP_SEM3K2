using System;
using System.Collections.Generic;

internal class Program
{
    // Обобщенный метод поиска элемента в массиве
    private static (int index, int count) FindElement<T>(T[] array, T value)
    {
        if (array == null || array.Length == 0)
        {
            return (-1, 0);
        }

        int firstIndex = -1;
        int count = 0;

        // Используем компаратор по умолчанию для типа T
        EqualityComparer<T> comparer = EqualityComparer<T>.Default;

        for (int i = 0; i < array.Length; i++)
        {
            if (comparer.Equals(array[i], value))
            {
                if (firstIndex == -1)
                {
                    firstIndex = i;
                }
                count++;
            }
        }

        return (firstIndex, count);
    }

    // Перегрузка для массивов с пользовательским компаратором
    private static (int index, int count) FindElement<T>(T[] array, T value, IEqualityComparer<T> comparer)
    {
        if (array == null || array.Length == 0)
        {
            return (-1, 0);
        }

        int firstIndex = -1;
        int count = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (comparer.Equals(array[i], value))
            {
                if (firstIndex == -1)
                {
                    firstIndex = i;
                }
                count++;
            }
        }

        return (firstIndex, count);
    }

    private static void Main()
    {
        // Работа с массивом целых чисел
        int[] numbers = { 5, 12, 8, 5, 15, 5, 7, 5 };
        Console.WriteLine($"Массив чисел: [{string.Join(", ", numbers)}]");

        var result = FindElement(numbers, 5);
        Console.WriteLine($"Поиск числа 5: индекс первого = {result.index}, количество = {result.count}");

        result = FindElement(numbers, 42);
        Console.WriteLine($"Поиск числа 42: индекс первого = {result.index}, количество = {result.count}");

        Console.WriteLine();

        // Работа с массивом строк
        string[] words = { "apple", "banana", "apple", "cherry", "apple", "date" };
        Console.WriteLine($"Массив строк: [{string.Join(", ", words)}]");

        var stringResult = FindElement(words, "apple");
        Console.WriteLine($"Поиск 'apple': индекс первого = {stringResult.index}, количество = {stringResult.count}");

        stringResult = FindElement(words, "grape");
        Console.WriteLine($"Поиск 'grape': индекс первого = {stringResult.index}, количество = {stringResult.count}");

        Console.WriteLine();

        // Работа с массивом объектов
        Person[] people = {
            new Person { Name = "Alice", Age = 25 },
            new Person { Name = "Bob", Age = 30 },
            new Person { Name = "Alice", Age = 25 },
            new Person { Name = "Charlie", Age = 35 }
        };

        Console.WriteLine("Массив людей:");
        foreach (var p in people)
        {
            Console.WriteLine($"  {p}");
        }

        Person searchPerson = new Person { Name = "Alice", Age = 25 };

        // Поиск по умолчанию (сравнение по ссылкам)
        var personResult = FindElement(people, searchPerson);
        Console.WriteLine($"\nПоиск {searchPerson} (сравнение по ссылкам):");
        Console.WriteLine($"  индекс первого = {personResult.index}, количество = {personResult.count}");

        // Поиск с пользовательским компаратором (по имени и возрасту)
        var personResultWithComparer = FindElement(people, searchPerson, new PersonComparer());
        Console.WriteLine($"\nПоиск {searchPerson} (сравнение по имени и возрасту):");
        Console.WriteLine($"  индекс первого = {personResultWithComparer.index}, количество = {personResultWithComparer.count}");
    }
}

// Класс Person
internal class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Age})";
    }
}

// Компаратор для Person, сравнивающий по имени и возрасту
internal class PersonComparer : IEqualityComparer<Person>
{
    public bool Equals(Person x, Person y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        return x.Name == y.Name && x.Age == y.Age;
    }

    public int GetHashCode(Person obj)
    {
        return (obj.Name?.GetHashCode() ?? 0) ^ obj.Age.GetHashCode();
    }
}