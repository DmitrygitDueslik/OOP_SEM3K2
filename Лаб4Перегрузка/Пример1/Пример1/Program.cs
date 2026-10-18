using System;
using System.Collections.Generic;

public class IntSet
{
    private List<int> _items = new List<int>();

    // Добавление элемента (только уникальные)
    public void Add(int value)
    {
        if (!_items.Contains(value))
            _items.Add(value);
    }

    // Перегрузка + (объединение множеств)
    public static IntSet operator +(IntSet a, IntSet b)
    {
        IntSet result = new IntSet();

        // Добавляем все элементы из первого множества
        foreach (int item in a._items)
            result.Add(item);

        // Добавляем все элементы из второго множества
        foreach (int item in b._items)
            result.Add(item);

        return result;
    }

    // Перегрузка * (пересечение множеств)
    public static IntSet operator *(IntSet a, IntSet b)
    {
        IntSet result = new IntSet();

        foreach (int item in a._items)
        {
            if (b._items.Contains(item))
                result.Add(item);
        }

        return result;
    }

    // Вывод множества
    public override string ToString()
    {
        return "{" + string.Join(", ", _items) + "}";
    }
}

class Program
{
    static void Main()
    {
        // Создаем два множества
        IntSet set1 = new IntSet();
        set1.Add(1); set1.Add(2); set1.Add(3);

        IntSet set2 = new IntSet();
        set2.Add(2); set2.Add(3); set2.Add(4);

        Console.WriteLine($"Множество 1: {set1}"); // {1, 2, 3}
        Console.WriteLine($"Множество 2: {set2}"); // {2, 3, 4}

        // Используем перегруженные операторы
        IntSet union = set1 + set2;  // объединение
        IntSet intersection = set1 * set2;  // пересечение

        Console.WriteLine($"Объединение: {union}");       // {1, 2, 3, 4}
        Console.WriteLine($"Пересечение: {intersection}"); // {2, 3}
    }
}
