using System;

// Класс A
class A
{
    public int ValueA { get; }

    public A(int valueA)
    {
        if (valueA < 1 || valueA > 100)
            throw new ArgumentException($"valueA должно быть от 1 до 100, получено {valueA}");
        ValueA = valueA;
    }
}

// Класс B наследует A
class B : A
{
    public int ValueB { get; }

    public B(int valueA, int valueB) : base(valueA)
    {
        if (valueB <= ValueA)
            throw new ArgumentException($"valueB должно быть больше valueA ({ValueA}), получено {valueB}");
        ValueB = valueB;
    }
}

// Класс C наследует B
class C : B
{
    public int ValueC { get; }

    public C(int valueA, int valueB, int valueC) : base(valueA, valueB)
    {
        int sum = ValueA + ValueB;
        int diff = Math.Abs(ValueA - ValueB);
        if (valueC >= sum || valueC <= diff)
            throw new ArgumentException(
                $"valueC должно быть меньше суммы ({sum}) и больше разности ({diff}), получено {valueC}");
        ValueC = valueC;
    }

    public override string ToString() => $"A={ValueA}, B={ValueB}, C={ValueC}";
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Создание корректного объекта:");
        try
        {
            C c1 = new C(10, 20, 25); // sum=30, diff=10, 10<25<30 - верно
            Console.WriteLine($"Успех: {c1}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("\nПопытки создать объекты с некорректными значениями:");

        try { A badA = new A(150); }
        catch (ArgumentException ex) { Console.WriteLine($"Ошибка A: {ex.Message}"); }

        try { B badB = new B(50, 30); }
        catch (ArgumentException ex) { Console.WriteLine($"Ошибка B: {ex.Message}"); }

        try { C badC = new C(10, 20, 35); } // sum=30, 35>=30 - некорректно
        catch (ArgumentException ex) { Console.WriteLine($"Ошибка C: {ex.Message}"); }
    }
}
