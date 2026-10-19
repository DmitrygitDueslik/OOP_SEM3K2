using System;
using System.Collections.Generic;

// Клавиатура - издатель события KeyPressed
internal class Keyboard
{
    public event Action<string>? KeyPressed;

    public void Press(string key)
    {
        Console.WriteLine($"Нажата клавиша: {key}");
        KeyPressed?.Invoke(key);
    }
}

internal class Program
{
    private static Dictionary<string, int> counts = new Dictionary<string, int>();

    // Обработчик для цифр
    private static void DigitHandler(string key)
    {
        if (key.Length == 1 && char.IsDigit(key[0]))
            Console.WriteLine($"  Нажата цифра {key}");
    }

    // Обработчик для букв
    private static void LetterHandler(string key)
    {
        if (key.Length == 1 && char.IsLetter(key[0]))
            Console.WriteLine($"  Нажата буква {key}");
    }

    // Обработчик для специальных клавиш
    private static void SpecialHandler(string key)
    {
        if (key == "Enter" || key == "Escape" || key == "Space")
            Console.WriteLine($"  Специальная клавиша: {key}");
    }

    // Обработчик-счётчик
    private static void CountHandler(string key)
    {
        if (counts.ContainsKey(key))
            counts[key]++;
        else
            counts[key] = 1;
    }

    private static void Main()
    {
        Keyboard keyboard = new Keyboard();
        keyboard.KeyPressed += DigitHandler;
        keyboard.KeyPressed += LetterHandler;
        keyboard.KeyPressed += SpecialHandler;
        keyboard.KeyPressed += CountHandler;

        string[] keys = { "A", "5", "Enter", "A", "Space", "7" };
        foreach (string key in keys)
        {
            keyboard.Press(key);
        }

        Console.WriteLine("\nОтписываем обработчик букв...\n");
        keyboard.KeyPressed -= LetterHandler;
        keyboard.Press("B");
        keyboard.Press("Escape");

        Console.WriteLine("\nКоличество нажатий:");
        foreach (KeyValuePair<string, int> pair in counts)
        {
            Console.WriteLine($"  {pair.Key}: {pair.Value}");
        }
    }
}