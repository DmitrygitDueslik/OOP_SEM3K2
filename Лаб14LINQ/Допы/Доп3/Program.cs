using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

internal class Program
{
    private static void Main()
    {
        string fileName = "text.txt";
        File.WriteAllText(fileName,
            "LINQ упрощает работу с данными. С LINQ код становится короче! " +
            "Можно фильтровать, сортировать и группировать данные. " +
            "Разве это не удобно? LINQ работает с массивами, списками и файлами.");
        string text = File.ReadAllText(fileName);

        // Слова: регулярное выражение \w+ находит последовательности букв и цифр
        var words = Regex.Matches(text.ToLower(), @"\w+").Select(m => m.Value).ToList();

        // Предложения: разбиваем по . ! ? (регулярное выражение внутри LINQ)
        var sentences = Regex.Split(text, @"(?<=[.!?])\s+")
                             .Where(s => s.Trim().Length > 0)
                             .ToList();

        Console.WriteLine($"Слов: {words.Count}, предложений: {sentences.Count}");

        // 1. Топ-10 слов по частоте
        Console.WriteLine("\nТоп-10 слов:");
        var top = words.GroupBy(w => w)
                       .OrderByDescending(g => g.Count()).ThenBy(g => g.Key, StringComparer.Ordinal)
                       .Take(10);
        foreach (var g in top)
            Console.WriteLine($"  {g.Key,-14} {g.Count()}");

        // 2. Самое длинное и самое короткое предложение (по числу слов)
        int WordCount(string s) => Regex.Matches(s, @"\w+").Count;
        string longest = sentences.OrderByDescending(WordCount).First();
        string shortest = sentences.OrderBy(WordCount).First();
        Console.WriteLine($"\nСамое длинное (слов: {WordCount(longest)}): {longest}");
        Console.WriteLine($"Самое короткое (слов: {WordCount(shortest)}): {shortest}");

        // 3. Статистика по знакам препинания
        Console.WriteLine("\nЗнаки препинания:");
        var punctuation = text.Where(char.IsPunctuation)
                              .GroupBy(c => c)
                              .OrderByDescending(g => g.Count());
        foreach (var g in punctuation)
            Console.WriteLine($"  '{g.Key}': {g.Count()}");

        // 4. Группировка предложений по количеству слов
        Console.WriteLine("\nПредложения по количеству слов:");
        foreach (var g in sentences.GroupBy(WordCount).OrderBy(g => g.Key))
            Console.WriteLine($"  слов в предложении {g.Key}: предложений {g.Count()}");

        File.Delete(fileName);
    }
}