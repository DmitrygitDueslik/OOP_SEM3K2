using System;

public class FloatingPoint
{
    public double Value { get; set; }

    // Статическое свойство - погрешность сравнения, общая для всех объектов класса
    public static double Epsilon { get; set; } = 0.0001;

    public FloatingPoint(double value)
    {
        Value = value;
    }

    // Сравнение с учетом погрешности epsilon
    public static bool operator ==(FloatingPoint a, FloatingPoint b)
    {
        return Math.Abs(a.Value - b.Value) < Epsilon;
    }

    public static bool operator !=(FloatingPoint a, FloatingPoint b)
    {
        return !(a == b);
    }

    // Явное преобразование FloatingPoint -> int (с округлением)
    public static explicit operator int(FloatingPoint fp)
    {
        return (int)Math.Round(fp.Value);
    }

    // Неявное преобразование int -> FloatingPoint
    public static implicit operator FloatingPoint(int value)
    {
        return new FloatingPoint(value);
    }

    public override bool Equals(object obj)
    {
        return obj is FloatingPoint fp && this == fp;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

public static class FloatingPointExtensions
{
    // Проверка, является ли число целым с учетом погрешности
    public static bool IsWhole(this FloatingPoint fp)
    {
        return Math.Abs(fp.Value - Math.Round(fp.Value)) < FloatingPoint.Epsilon;
    }
}

class Program
{
    static void Main()
    {
        FloatingPoint a = new FloatingPoint(1.0000001);
        FloatingPoint b = new FloatingPoint(1.0000002);

        Console.WriteLine($"a = {a}, b = {b}, Epsilon = {FloatingPoint.Epsilon}");
        Console.WriteLine($"a == b: {a == b}");

        FloatingPoint.Epsilon = 0.00000001;
        Console.WriteLine($"\nУменьшили Epsilon до {FloatingPoint.Epsilon}");
        Console.WriteLine($"a == b: {a == b}");

        FloatingPoint c = new FloatingPoint(4.999999999);
        Console.WriteLine($"\nc = {c}");
        Console.WriteLine($"(int)c = {(int)c}");
        Console.WriteLine($"c.IsWhole() = {c.IsWhole()}");

        FloatingPoint d = 10; // неявное преобразование int -> FloatingPoint
        Console.WriteLine($"\nd = {d} (получено неявно из int)");
    }
}
