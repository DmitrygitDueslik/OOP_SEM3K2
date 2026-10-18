using System;

class Program
{
    static void Main()
    {
        int[] numbers = new int[5]; // Массив из 5 целых чисел (по умолчанию все 0)
        int[] values = { 1, 2, 3, 4, 5 }; // Инициализация 

        int[,] matrix = new int[3, 4]; // 3 строки, 4 столбца (все элементы 0 по умолчанию)
        int[,] grid = { { 1, 2 }, { 3, 4 } }; // Матрица 2x2

        int[][] jagged = new int[3][]; // Зубчатый массив
        jagged[0] = new int[] { 1, 2, 3 };
        jagged[1] = new int[] { 4, 5 };
        jagged[2] = new int[] { 6 }; // Добавил третью строку для полноты

        // --- Вывод одномерного массива numbers ---
        Console.WriteLine("Вывод массива numbers (все нули):");
        for (int i = 0; i < numbers.Length; i++)
        {
            Console.Write($"{numbers[i]} ");
        }
        Console.WriteLine("\n");

        // --- Вывод матрицы grid (2x2) ---
        Console.WriteLine("Вывод матрицы grid (2x2):");
        for (int i = 0; i < grid.GetLength(0); i++) // Строки
        {
            for (int j = 0; j < grid.GetLength(1); j++) // Столбцы
            {
                Console.Write($"{grid[i, j]} "); // Правильный синтаксис для multidimensional array
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        // --- Вывод матрицы matrix (3x4) ---
        Console.WriteLine("Вывод матрицы matrix (3x4, все нули):");
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write($"{matrix[i, j]} ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        // --- Вывод зубчатого массива jagged ---
        Console.WriteLine("Вывод зубчатого массива jagged:");
        for (int i = 0; i < jagged.Length; i++)
        {
            for (int j = 0; j < jagged[i].Length; j++)
            {
                Console.Write($"{jagged[i][j]} "); // Правильный синтаксис для jagged array
            }
            Console.WriteLine();
        }
    }
}