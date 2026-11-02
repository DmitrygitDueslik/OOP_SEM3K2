using System;

internal class Program
{
    private static void Main()
    {
        int[] numbers = { 12, -7, 0, -20, 5, 16, -3, 8, -15, 20, -1, 4 };
        Console.WriteLine("Исходный массив:        " + string.Join(", ", numbers));

        int[] negatives = Array.FindAll(numbers, x => x < 0);
        Console.WriteLine("Отрицательные числа:    " + string.Join(", ", negatives));

        int[] absolute = Array.ConvertAll(negatives, x => Math.Abs(x));
        Console.WriteLine("Их модули:              " + string.Join(", ", absolute));

        int[] converted = Array.ConvertAll(numbers, x => x < 0 ? Math.Abs(x) : x);
        Console.WriteLine("Преобразованный массив: " + string.Join(", ", converted));

        Predicate<int> isPowerOfTwo = x =>
        {
            if (x <= 0) return false;
            while (x % 2 == 0)
            {
                x = x / 2;
            }
            return x == 1;
        };

        int[] test = { 1, 6, 8, 12, 64, 100 };
        foreach (int x in test)
        {
            Console.WriteLine($"{x,3} - степень двойки? {isPowerOfTwo(x)}");
        }

        Console.WriteLine();
        Console.WriteLine("Исходный массив: " + string.Join(", ", numbers));
        bool hasPower = Array.Exists(numbers, isPowerOfTwo);
        Console.WriteLine($"Есть степени двойки? {hasPower}");
        Console.WriteLine("Найденные степени двойки: " + string.Join(", ", Array.FindAll(numbers, isPowerOfTwo)));

        int[] odd = { 3, 5, 7, -8, 0 };
        Console.WriteLine($"\nМассив {string.Join(", ", odd)}: есть степени двойки? {Array.Exists(odd, isPowerOfTwo)}");
    }
}