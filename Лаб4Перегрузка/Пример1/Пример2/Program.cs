using System;

public static class StringExtensions
{
    // Метод расширения: проверка на палиндром
    public static bool IsPalindrome(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return true;

        // Убираем пробелы и приводим к нижнему регистру
        string cleaned = str.Replace(" ", "").ToLower();

        // Сравниваем символы с начала и с конца
        for (int i = 0; i < cleaned.Length / 2; i++)
        {
            if (cleaned[i] != cleaned[cleaned.Length - 1 - i])
                return false;
        }
        return true;
    }

    // Метод расширения: переворот строки
    public static string Reverse(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        char[] chars = str.ToCharArray();     // превращаем строку в массив символов
        Array.Reverse(chars);                  // переворачиваем массив
        return new string(chars);               // создаем новую строку из массива
    }
}

class Program
{
    static void Main()
    {
        string word1 = "шалаш";
        string word2 = "hello";
        string sentence = "а роза упала на лапу азора";

        // Используем методы расширения
        Console.WriteLine($"Слово '{word1}' палиндром? {word1.IsPalindrome()}"); // True
        Console.WriteLine($"Слово '{word2}' палиндром? {word2.IsPalindrome()}"); // False
        Console.WriteLine($"Фраза '{sentence}' палиндром? {sentence.IsPalindrome()}"); // True

        Console.WriteLine($"\nИсходная строка: {word2}");
        Console.WriteLine($"Перевернутая: {word2.Reverse()}"); // "olleh"
    }
}
