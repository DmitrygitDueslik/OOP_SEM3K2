using System;

// Обобщенный класс для хранения пары значений
public class Pair<T1, T2>
{
    private T1 _first;
    private T2 _second;

    // Конструктор
    public Pair(T1 first, T2 second)
    {
        _first = first;
        _second = second;
    }

    // Свойства
    public T1 First
    {
        get { return _first; }
        set { _first = value; }
    }

    public T2 Second
    {
        get { return _second; }
        set { _second = value; }
    }

    // Метод для вывода информации
    public void PrintInfo()
    {
        Console.WriteLine($"First: {_first} (тип: {typeof(T1)}), Second: {_second} (тип: {typeof(T2)})");
    }

    // Метод для сравнения пар
    public bool Equals(Pair<T1, T2> other)
    {
        if (other == null) return false;
        return _first.Equals(other._first) && _second.Equals(other._second);
    }

    // Переопределение ToString
    public override string ToString()
    {
        return $"({_first}, {_second})";
    }
}

internal class Program
{
    private static void Main()
    {
        // Создание пары с типами int и string
        Pair<int, string> person = new Pair<int, string>(25, "Иван");
        Console.WriteLine("Информация о паре:");
        person.PrintInfo();
        Console.WriteLine($"ToString: {person}");

        Console.WriteLine();

        // Создание пары с типами double и bool
        Pair<double, bool> measurements = new Pair<double, bool>(98.6, true);
        Console.WriteLine("Информация о паре:");
        measurements.PrintInfo();
        Console.WriteLine($"ToString: {measurements}");

        // Изменение значений
        measurements.First = 36.6;
        measurements.Second = false;
        Console.WriteLine($"\nПосле изменения: {measurements}");

        // Сравнение пар
        Pair<int, string> anotherPerson = new Pair<int, string>(25, "Иван");
        Console.WriteLine($"\nperson.Equals(anotherPerson): {person.Equals(anotherPerson)}");
    }
}