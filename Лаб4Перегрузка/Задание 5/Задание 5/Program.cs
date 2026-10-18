using System;

public class Counter
{
    public int Value { get; set; }

    public Counter(int value = 0)
    {
        Value = value;
    }

    // Инкремент: ++counter / counter++
    public static Counter operator ++(Counter c)
    {
        return new Counter(c.Value + 1);
    }

    // Декремент: --counter / counter--
    public static Counter operator --(Counter c)
    {
        return new Counter(c.Value - 1);
    }

    // Прибавление числа: counter + number
    public static Counter operator +(Counter c, int number)
    {
        return new Counter(c.Value + number);
    }

    // Вычитание числа: counter - number
    public static Counter operator -(Counter c, int number)
    {
        return new Counter(c.Value - number);
    }

    // Сравнение счетчиков
    public static bool operator >(Counter a, Counter b)
    {
        return a.Value > b.Value;
    }

    public static bool operator <(Counter a, Counter b)
    {
        return a.Value < b.Value;
    }

    // true/false - позволяют использовать объект в условиях (if, while)
    public static bool operator true(Counter c)
    {
        return c.Value != 0;
    }

    public static bool operator false(Counter c)
    {
        return c.Value == 0;
    }

    public override string ToString()
    {
        return $"Counter({Value})";
    }
}

// Методы расширения для char
public static class CharExtensions
{
    public static bool IsDigit(this char c)
    {
        return char.IsDigit(c);
    }

    public static bool IsLetter(this char c)
    {
        return char.IsLetter(c);
    }

    public static char ToUpper(this char c)
    {
        return char.ToUpper(c);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Работа с классом Counter\n");

        Counter counter = new Counter(5);
        Console.WriteLine($"Начальное значение: {counter}");

        counter++;
        Console.WriteLine($"После counter++: {counter}");

        counter--;
        counter--;
        Console.WriteLine($"После двух counter--: {counter}");

        Counter increased = counter + 10;
        Console.WriteLine($"counter + 10 = {increased}");

        Counter decreased = counter - 3;
        Console.WriteLine($"counter - 3 = {decreased}");

        Counter c1 = new Counter(7);
        Counter c2 = new Counter(3);
        Console.WriteLine($"\nc1 = {c1}, c2 = {c2}");
        Console.WriteLine($"c1 > c2: {c1 > c2}");
        Console.WriteLine($"c1 < c2: {c1 < c2}");

        Counter zero = new Counter(0);
        Counter nonZero = new Counter(4);

        Console.WriteLine("\nПроверка через true/false:");
        if (zero)
            Console.WriteLine("zero: счетчик не равен нулю");
        else
            Console.WriteLine("zero: счетчик равен нулю");

        if (nonZero)
            Console.WriteLine("nonZero: счетчик не равен нулю");
        else
            Console.WriteLine("nonZero: счетчик равен нулю");

        Console.WriteLine("\nМетоды расширения для char");
        char ch1 = '7';
        char ch2 = 'a';
        char ch3 = '#';

        Console.WriteLine($"'{ch1}'.IsDigit() = {ch1.IsDigit()}");
        Console.WriteLine($"'{ch2}'.IsLetter() = {ch2.IsLetter()}");
        Console.WriteLine($"'{ch3}'.IsDigit() = {ch3.IsDigit()}, IsLetter() = {ch3.IsLetter()}");
        Console.WriteLine($"'{ch2}'.ToUpper() = {ch2.ToUpper()}");
    }
}
