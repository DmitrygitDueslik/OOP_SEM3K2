using System;

interface ICalculator
{
    double Add(double a, double b);
    double Subtract(double a, double b);
    double Multiply(double a, double b);
    double Divide(double a, double b);

}

interface IMemory
    {
        void Store(double value);
         // сохранить значение в память
        double Recall();          // извлечь значение из памяти

}

class SimpleCalculator : ICalculator, IMemory
{
    private double memory;

    public double Add(double a, double b)
    {
        return a + b;
    }
    public double Subtract(double a, double b)
    {
        return a - b;
    }

    public double Multiply(double a,double b)
    {
        return a * b;
    }


    public double Divide(double a,double b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Деление на ноль невозможно");
          
        }
        return a / b;
    }

    // ---- реализация IMemory ----
    public void Store(double value)
    {
        memory = value;
        Console.WriteLine($"  [M] В память сохранено: {memory}");
    }
    public double Recall()
    {
        Console.WriteLine($"  [M] Из памяти извлечено: {memory}");
        return memory;
    }

}
class Program
{
    static void Main()
    {
        SimpleCalculator calc = new SimpleCalculator();

        Console.WriteLine("=== Работа через интерфейс ICalculator ===");
        ICalculator calculator = calc;
        Console.WriteLine($"12 + 8 = {calculator.Add(12, 8)}");
        Console.WriteLine($"12 - 8 = {calculator.Subtract(12, 8)}");
        Console.WriteLine($"12 * 8 = {calculator.Multiply(12, 8)}");
        Console.WriteLine($"12 / 8 = {calculator.Divide(12, 8)}");

        try
        {
            Console.WriteLine($"12 / 0 = {calculator.Divide(12, 0)}");
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        // работа с памятью через интерфейс IMemory
        Console.WriteLine("\n=== Работа через интерфейс IMemory ===");
        IMemory memory = calc; // тот же самый объект, но «видимый» как IMemory

        // Вычисляем (2 + 3) * 4 с сохранением промежуточного результата
        double sum = calculator.Add(2, 3);
        Console.WriteLine($"2 + 3 = {sum}");
        memory.Store(sum);

        double result = calculator.Multiply(memory.Recall(), 4);
        Console.WriteLine($"(2 + 3) * 4 = {result}");

        // Демонстрация множественной реализации
        Console.WriteLine("\n=== Проверка реализуемых интерфейсов ===");
        object obj = calc;
        Console.WriteLine($"calc is ICalculator: {obj is ICalculator}");
        Console.WriteLine($"calc is IMemory:     {obj is IMemory}");
        Console.WriteLine($"calculator и memory ссылаются на один объект: {ReferenceEquals(calculator, memory)}");

    }
}