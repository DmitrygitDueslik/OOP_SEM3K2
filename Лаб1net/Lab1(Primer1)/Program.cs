using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Типы значений (int)");
        int a = 10;
        int b = a;
        Console.WriteLine($"До изменения: a = {a}, b = {b}");
        b = 20;
        Console.WriteLine($"После изменения b: a= {a}, b = {b}");
        Console.WriteLine("Изменение b не повлияло на a,т.к это копия.");
        Console.WriteLine("\nТипы значений double)");
        double x = 3.14;
        double y = x;
        Console.WriteLine($"До изменения: x = {x}, y = {y}");
        y = 2.71;
        Console.WriteLine($"После изменения y: x = {x}, y = {y}");
        Console.WriteLine("Изменение y не повлияло на x,т.к это копия.");

        Console.WriteLine("\nСсылочные типы (string)");
        string s1 = "Hello";
        string s2 = s1;
        Console.WriteLine($"До изменения: s1 = \"{s1}\", s2 = \"{s2}\"");
        s2 = "World";
        Console.WriteLine($"После изменения s2: s1 = \"{s1}\", s2 = \"{s2}\"");
        Console.WriteLine("s1 не изменилась, т.к. строки неизменяемы, а s2 присвоена новая строка.");

        Console.WriteLine("\nДемонстрация ссылочной природы object");
        object obj1 = new object();
        object obj2 = obj1;
        Console.WriteLine($"obj1 и obj2 ссылаются на один объект: {ReferenceEquals(obj1, obj2)}");
    }
}