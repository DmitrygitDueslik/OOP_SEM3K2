using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main()
    {
        string text = "Коллекции в C# помогают хранить данные. Список хранит данные по порядку, " +
                      "словарь хранит данные по ключу, а множество хранит только уникальные данные. " +
                      "Очередь и стек тоже коллекции, и каждая коллекция нужна для своей задачи.";

        // 1. List<string> - все слова
        char[] separators = { ' ', '.', ',', '!', '?' };
        string[] parts = text.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries);
        List<string> words = new List<string>(parts);

        // 2. Dictionary - частота каждого слова
        Dictionary<string, int> frequency = new Dictionary<string, int>();
        foreach (string word in words)
        {
            if (frequency.ContainsKey(word))
                frequency[word]++;
            else
                frequency[word] = 1;
        }

        // 3. HashSet - уникальные слова
        HashSet<string> unique = new HashSet<string>(words);

        // 4. SortedDictionary - частоты в алфавитном порядке
        SortedDictionary<string, int> sorted = new SortedDictionary<string, int>(frequency);

        // 5. Queue - последние 10 слов
        Queue<string> history = new Queue<string>();
        foreach (string word in words)
        {
            history.Enqueue(word);
            if (history.Count > 10)
                history.Dequeue(); // убираем самое старое слово
        }

        Console.WriteLine($"Всего слов: {words.Count}");
        Console.WriteLine($"Уникальных слов: {unique.Count}");

        // Топ-10 по частоте: сортируем пары по убыванию количества, при равенстве - по алфавиту
        List<KeyValuePair<string, int>> top = new List<KeyValuePair<string, int>>(frequency);
        top.Sort((a, b) => a.Value != b.Value ? b.Value.CompareTo(a.Value) : a.Key.CompareTo(b.Key));
        Console.WriteLine("\nТоп-10 слов:");
        for (int i = 0; i < 10 && i < top.Count; i++)
        {
            Console.WriteLine($"  {i + 1,2}. {top[i].Key,-10} {top[i].Value}");
        }

        Console.WriteLine("\nSortedDictionary (первые 5 по алфавиту):");
        int n = 0;
        foreach (KeyValuePair<string, int> pair in sorted)
        {
            Console.WriteLine($"  {pair.Key}: {pair.Value}");
            n++;
            if (n == 5) break;
        }

        Console.WriteLine("\nПоследние 10 слов (Queue): " + string.Join(" ", history));
    }
}