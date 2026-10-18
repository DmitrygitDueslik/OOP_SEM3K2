using System;

class Program
{
    static void AssignNewValues(int byValue, ref int byRef)
    {
        byValue = 500; // меняем локальную копию 
        byRef = 500;   // меняем через ссылку видно снаружи

        Console.WriteLine($"Внутри метода: byValue = {byValue}, byRef = {byRef}");
    }

    static void Main()
    {
        Console.WriteLine("Часть 1. Обычный порядок передачи\n");

        int a = 1;
        int b = 2;

        Console.WriteLine($"До вызова: a = {a}, b = {b}");
        AssignNewValues(a, ref b);
        Console.WriteLine($"После AssignNewValues(a, ref b): a = {a}, b = {b}\n");

        Console.WriteLine("Часть 2. Меняем переменные местами (a и b меняются ролями)\n");

        int c = 1;
        int d = 2;

        Console.WriteLine($"До вызова: c = {c}, d = {d}");
        // теперь d стоит на месте параметра "по значению", c - на месте ref
        AssignNewValues(d, ref c);
        Console.WriteLine($"После AssignNewValues(d, ref c): c = {c}, d = {d}");
    }
}
default 