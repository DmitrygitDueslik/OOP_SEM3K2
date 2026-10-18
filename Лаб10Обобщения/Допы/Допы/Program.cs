using System;
using System.Collections.Generic;
using System.Reflection;

public class CriterionException : Exception
{
    public string Criterion { get; }

    public CriterionException(string criterion, string message, Exception? inner = null)
        : base(message, inner)
    {
        Criterion = criterion;
    }
}

public class GenericComparator<T>
{
    // Результат: < 0 — a меньше b, 0 — равны, > 0 — a больше b
    public int Compare(T a, T b, string criterion)
    {
        // Критерий "value" — сравнение самих значений
        if (criterion.Equals("value", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                return Comparer<T>.Default.Compare(a, b);
            }
            catch (ArgumentException ex)
            {
                throw new CriterionException(criterion, $"Тип {typeof(T).Name} не поддерживает сравнение значений (нет IComparable)", ex);
            }
        }

        // Иначе критерий — имя свойства; ищем его через рефлексию (без учёта регистра)
        PropertyInfo? property = typeof(T).GetProperty(criterion,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (property == null)
        {
            throw new CriterionException(criterion, $"У типа {typeof(T).Name} нет свойства \"{criterion}\"");
        }

        object? valueA = property.GetValue(a);
        object? valueB = property.GetValue(b);

        if (valueA is IComparable comparableA)
        {
            // значения свойства одного типа, поэтому CompareTo безопасен
            return comparableA.CompareTo(valueB);
        }
        throw new CriterionException(criterion,
            $"Свойство {typeof(T).Name}.{property.Name} имеет тип {property.PropertyType.Name}, который нельзя сравнить");
    }

    // Удобная обёртка для вывода результата
    public string Describe(T a, T b, string criterion)
    {
        int r = Compare(a, b, criterion);
        string sign = r < 0 ? "<" : r > 0 ? ">" : "=";
        return $"{a} {sign} {b}  (по критерию \"{criterion}\")";
    }
}

public class Address
{
    public string City { get; set; } = "";

    public override string ToString() => City;
}

public class Employee
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public Address Address { get; set; } = new Address();

    public override string ToString() => $"{Name}({Age})";
}

internal class Program
{
    private static void Try<T>(GenericComparator<T> comparator, T a, T b, string criterion)
    {
        try
        {
            Console.WriteLine("  " + comparator.Describe(a, b, criterion));
        }
        catch (CriterionException ex)
        {
            Console.WriteLine($"  Ошибка критерия \"{ex.Criterion}\": {ex.Message}");
        }
    }

    private static void Main()
    {
        Console.WriteLine("Числа:");
        var ints = new GenericComparator<int>();
        Try(ints, 42, 17, "value");

        Console.WriteLine("\nСтроки:");
        var strings = new GenericComparator<string>();
        Try(strings, "яблоко", "киви", "value");   // по алфавиту
        Try(strings, "яблоко", "киви", "Length");  // по длине

        Console.WriteLine("\nДаты:");
        var dates = new GenericComparator<DateTime>();
        DateTime d1 = new DateTime(2024, 12, 31), d2 = new DateTime(2025, 1, 1);
        Console.WriteLine($"  d1 = {d1:d}, d2 = {d2:d}");
        Try(dates, d1, d2, "year");
        Try(dates, d1, d2, "Month");

        Console.WriteLine("\nСотрудники:");
        var employees = new GenericComparator<Employee>();
        Employee e1 = new Employee { Name = "Ольга", Age = 34, Salary = 120000, Address = new Address { City = "Москва" } };
        Employee e2 = new Employee { Name = "Андрей", Age = 29, Salary = 150000, Address = new Address { City = "Казань" } };
        Try(employees, e1, e2, "Age");
        Try(employees, e1, e2, "salary");
        Try(employees, e1, e2, "Name");
        Try(employees, e1, e2, "Height");   // свойства нет
        Try(employees, e1, e2, "Address");  // тип Address не реализует IComparable
        Try(employees, e1, e2, "value");    // Employee не реализует IComparable
    }
}