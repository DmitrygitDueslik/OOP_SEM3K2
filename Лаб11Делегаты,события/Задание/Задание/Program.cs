using System;

internal delegate double ArrayOperation(int[] numbers);

internal class MathHelper
{
    public static double Sum(int[] numbers)
    {
        double sum = 0;
        foreach (int n in numbers)
        {
            sum += n;
        }

        return sum;
    }

    public static double Average(int[] numbers)
    {
        return Sum(numbers) / numbers.Length;
    }

    public static double Max(int[] numbers)
    {
        int max = numbers[0];
        foreach (int n in numbers)
        {
            if (n > max) max = n;
        }
        return max;
    }

    public static double Min(int[] numbers)
    {
        int min = numbers[0];
        foreach (int n in numbers)
        {
            if (n < min) min = n;
        }

        return min;
    }
}

internal class ThresholdReachedEventArgs : EventArgs
{
    public int Threshold { get; }
    public DateTime TimeReached { get; }

    public ThresholdReachedEventArgs(int threshold)
    {
        Threshold = threshold;
        TimeReached = DateTime.Now;
    }
}

internal class Counter
{
    private int threshold;
    public int Value { get; private set; }

    public event EventHandler<ThresholdReachedEventArgs> ThresholdReached;

    public Counter(int threshold)
    {
        this.threshold = threshold;
    }

    public void Increment()
    {
        Value++;
        Console.WriteLine($"Счётчик: {Value}");
        if (Value == threshold)
        {
            // Генерируем событие (если есть подписчики)
            ThresholdReached?.Invoke(this, new ThresholdReachedEventArgs(threshold));
        }
    }
}

internal class Program
{
    // Метод выполняет любую операцию, переданную через делегат
    private static void Execute(string name, ArrayOperation operation, int[] numbers)
    {
        Console.WriteLine($"{name,-8}: {operation(numbers):0.##}");
    }

    // Обработчики события
    private static void ShowMessage(object? sender, ThresholdReachedEventArgs e)
    {
        Console.WriteLine($"  >>> Порог {e.Threshold} достигнут!");
    }

    private static void ShowTime(object? sender, ThresholdReachedEventArgs e)
    {
        Console.WriteLine($"  >>> Время достижения: {e.TimeReached:T}");
    }

    private static void Main()
    {
        Console.WriteLine("=== Задание 1. MathHelper и делегат ===\n");
        int[] numbers = { 4, 9, 2, 15, 7, 3 };
        Console.WriteLine("Массив: " + string.Join(", ", numbers));

        ArrayOperation op = MathHelper.Sum;
        Console.WriteLine($"Через переменную делегата (Sum): {op(numbers)}\n");

        ArrayOperation[] operations = { MathHelper.Sum, MathHelper.Average, MathHelper.Max, MathHelper.Min };
        string[] names = { "Сумма", "Среднее", "Максимум", "Минимум" };
        for (int i = 0; i < operations.Length; i++)
        {
            Execute(names[i], operations[i], numbers);
        }

        Console.WriteLine("\n=== Задание 2. Counter и событие ThresholdReached ===\n");
        Counter counter = new Counter(5);
        counter.ThresholdReached += ShowMessage; // подписка
        counter.ThresholdReached += ShowTime;

        for (int i = 0; i < 7; i++)
        {
            counter.Increment();
        }

        counter.ThresholdReached -= ShowTime;    // отписка
        Console.WriteLine("\nОбработчик ShowTime отписан. Новый счётчик с порогом 2:");
        Counter counter2 = new Counter(2);
        counter2.ThresholdReached += ShowMessage;
        counter2.Increment();
        counter2.Increment();
    }
}