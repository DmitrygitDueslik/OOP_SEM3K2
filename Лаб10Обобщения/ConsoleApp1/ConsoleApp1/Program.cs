using System;
using System.Numerics;

internal class Adder<T> where T : INumber<T>
{
    public T Add(T a, T b)
    {
        return a + b;
    }
}

internal class IntAdder : Adder<int>
{
}

internal class DoubleAdder : Adder<double>
{
}

internal class StringAdder : Adder<string>
{
}

internal class Program
{
    private static void Main()
    {
        IntAdder intAdder = new IntAdder();
        Console.WriteLine($"5 + 3 = {intAdder.Add(5, 3)}");

        DoubleAdder doubleAdder = new DoubleAdder();
        Console.WriteLine($"2,5 + 1,25 = {doubleAdder.Add(2.5, 1.25)}");

        try
        {
            typeof(Adder<>).MakeGenericType(typeof(string));
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Ошибка: Adder<string> создать нельзя, string не число");
        }
    }
}