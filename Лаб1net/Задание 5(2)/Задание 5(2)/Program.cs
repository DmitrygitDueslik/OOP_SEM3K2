using System;
class Program
{
    static void Square(ref int number)
    {
        number = number * number;
    }

    static void Main()
    {
        Console.WriteLine("Задание 2.Возведение числа в квадрат через ref\n");
        int number = 5;
        Console.WriteLine($"Исходное число: {number}");
        Square(ref number);
        Console.WriteLine($"Число после вызова Square: {number}");
    }
}