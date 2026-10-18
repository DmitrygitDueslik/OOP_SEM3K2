using System;
using System.Collections.Generic;

public class CircularChainException : Exception
{
    public CircularChainException(string message) : base(message)
    {
    }
}

// Цепочка связанных объектов: "элемент - следующий элемент"
public class ChainBuilder<T> where T : class
{
    private Dictionary<T, T?> next = new Dictionary<T, T?>();
    private T? head;  // первый элемент
    private T? tail;  // последний добавленный элемент

    public bool AllowCircular { get; set; }

    // Добавляет элемент в конец цепочки
    public void AddNext(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (tail != null && next[tail] != null)
        {
            throw new InvalidOperationException("Цепочка уже замкнута в цикл - добавлять элементы нельзя");
        }

        bool alreadyInChain = next.ContainsKey(item);
        if (alreadyInChain && !AllowCircular)
        {
            throw new CircularChainException($"Связь {tail} -> {item} создаст цикл, а AllowCircular = false");
        }

        if (head == null)
        {
            head = item;          // первый элемент
        }
        else
        {
            next[tail!] = item;   // связываем предыдущий последний с новым
        }

        if (!alreadyInChain)
        {
            next[item] = null;    // у нового элемента пока нет следующего
            tail = item;
        }
        // если элемент уже был в цепочке, связь замкнула цикл, tail не меняем
    }

    // Возвращает следующий элемент (null — конец цепочки)
    public T? GetNext(T item)
    {
        if (!next.TryGetValue(item, out T? result))
        {
            throw new KeyNotFoundException($"Элемент {item} отсутствует в цепочке");
        }
        return result;
    }

    // Есть ли цикл: идём от начала, пока не встретим уже посещённый элемент или конец
    public bool IsCircular()
    {
        HashSet<T> visited = new HashSet<T>();
        T? current = head;
        while (current != null)
        {
            if (!visited.Add(current)) return true; // встретили повторно
            current = next[current];
        }
        return false;
    }

    public override string ToString()
    {
        List<string> parts = new List<string>();
        HashSet<T> visited = new HashSet<T>();
        T? current = head;
        while (current != null)
        {
            if (!visited.Add(current))
            {
                parts.Add($"({current} ...цикл)");
                break;
            }
            parts.Add(current.ToString()!);
            current = next[current];
        }
        return parts.Count == 0 ? "(пусто)" : string.Join(" -> ", parts);
    }
}

// Пример класса для цепочки — остановки маршрута
public class Stop
{
    public string Name { get; }

    public Stop(string name)
    { Name = name; }

    public override string ToString() => Name;
}

internal class Program
{
    private static void Main()
    {
        Stop a = new Stop("Вокзал"), b = new Stop("Центр"), c = new Stop("Парк"), d = new Stop("Университет");

        Console.WriteLine("=== Линейный маршрут (AllowCircular = false) ===");
        ChainBuilder<Stop> route = new ChainBuilder<Stop> { AllowCircular = false };
        route.AddNext(a);
        route.AddNext(b);
        route.AddNext(c);
        Console.WriteLine($"Цепочка: {route}");
        Console.WriteLine($"После {b} идёт {route.GetNext(b)}");
        Console.WriteLine($"После {c} идёт {route.GetNext(c)?.ToString() ?? "конец маршрута"}");
        Console.WriteLine($"Есть цикл: {route.IsCircular()}");

        try
        {
            route.AddNext(a); // вернуться на вокзал — цикл
        }
        catch (CircularChainException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        try
        {
            route.GetNext(d);
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("\n=== Кольцевой маршрут (AllowCircular = true) ===");
        ChainBuilder<Stop> ring = new ChainBuilder<Stop> { AllowCircular = true };
        ring.AddNext(a);
        ring.AddNext(b);
        ring.AddNext(d);
        ring.AddNext(a); // замыкаем кольцо
        Console.WriteLine($"Цепочка: {ring}");
        Console.WriteLine($"После {d} идёт {ring.GetNext(d)}");
        Console.WriteLine($"Есть цикл: {ring.IsCircular()}");

        Console.Write("Два круга по кольцу: ");
        Stop? s = a;
        for (int i = 0; i < 6; i++)
        {
            Console.Write(s + (i < 5 ? " -> " : "\n"));
            s = ring.GetNext(s!);
        }

        try
        {
            ring.AddNext(c);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("\n=== Та же цепочка для строк ===");
        ChainBuilder<string> words = new ChainBuilder<string>();
        foreach (string w in new[] { "раз", "два", "три" }) words.AddNext(w);
        Console.WriteLine($"Цепочка: {words}, цикл: {words.IsCircular()}");
    }
}