using System;
using System.Collections.Generic;

internal class TextAnalyzer
{
    private string[] lines;
    private Dictionary<string, Predicate<string>> rules = new Dictionary<string, Predicate<string>>();

    public TextAnalyzer(string[] lines)
    {
        this.lines = lines;
    }

    // Добавить правило проверки строк
    public void AddRule(string name, Predicate<string> rule)
    {
        rules[name] = rule;
    }

    // Сколько строк удовлетворяет каждому правилу
    public void CountSatisfying()
    {
        foreach (KeyValuePair<string, Predicate<string>> rule in rules)
        {
            int count = Array.FindAll(lines, rule.Value).Length;
            Console.WriteLine($"  {rule.Key}: {count}");
        }
    }

    // Первая строка, удовлетворяющая правилу
    public string? FindFirstSatisfying(string ruleName)
    {
        return Array.Find(lines, rules[ruleName]);
    }

    // Преобразование всех строк
    public void Transform(Func<string, string> transformer)
    {
        lines = Array.ConvertAll(lines, s => transformer(s));
    }

    public void Print()
    {
        foreach (string s in lines) Console.WriteLine($"  {s}");
    }
}

internal class Program
{
    private static void Main()
    {
        string[] text = { "Анна", "казак", "Солнечный день", "шалаш", "Привет мир", "ok" };
        TextAnalyzer analyzer = new TextAnalyzer(text);

        analyzer.AddRule("Длина больше 10", s => s.Length > 10);
        analyzer.AddRule("Содержит 'а'", s => s.ToLower().Contains('а'));
        analyzer.AddRule("С заглавной буквы", s => s.Length > 0 && char.IsUpper(s[0]));
        analyzer.AddRule("Палиндром", s =>
        {
            string lower = s.ToLower();
            char[] arr = lower.ToCharArray();
            Array.Reverse(arr);
            return lower == new string(arr);
        });

        Console.WriteLine("Строки:");
        analyzer.Print();

        Console.WriteLine("\nКоличество строк по правилам:");
        analyzer.CountSatisfying();

        Console.WriteLine("\nПервая строка-палиндром: " + analyzer.FindFirstSatisfying("Палиндром"));
        Console.WriteLine("Первая длинная строка: " + analyzer.FindFirstSatisfying("Длина больше 10"));

        analyzer.Transform(s => s.ToUpper());
        Console.WriteLine("\nПосле Transform (верхний регистр):");
        analyzer.Print();

        analyzer.Transform(s => $"{s} ({s.Length})");
        Console.WriteLine("\nПосле Transform (добавлена длина):");
        analyzer.Print();
    }
}