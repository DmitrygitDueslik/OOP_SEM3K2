using System;

internal class TemperatureOutOfRangeException : Exception
{
    public double Temperature { get; }
    public double Min { get; }
    public double Max { get; }

    public TemperatureOutOfRangeException() : base()
    {
    }

    public TemperatureOutOfRangeException(string message) : base(message)
    {
    }

    public TemperatureOutOfRangeException(string message, Exception inner) : base(message, inner)
    {
    }

    public TemperatureOutOfRangeException(double temperature, double min, double max)
        : base($"Температура {temperature} °C вне диапазона [{min}; {max}]")
    {
        Temperature = temperature;
        Min = min;
        Max = max;
    }
}

internal class Program
{
    private const double MinTemperature = -50;
    private const double MaxTemperature = 50;

    // Задание 1: чтение возраста; при ошибке преобразования выбрасываем исключение
    private static int ReadAge()
    {
        Console.WriteLine("Введите ваш возраст");
        string input = Console.ReadLine() ?? "";
        if (!int.TryParse(input, out int age))
        {
            throw new FormatException($"\"{input}\" не является целым числом");
        }
        if (age < 0 || age > 150)
        {
            throw new ArgumentOutOfRangeException(nameof(age), age, "Возраст должен быть от 0 до 150");
        }
        return age;
    }

    // Задание 2: проверка температуры
    private static void CheckTemperature(double temperature)
    {
        if (temperature < MinTemperature || temperature > MaxTemperature)
        {
            throw new TemperatureOutOfRangeException(temperature, MinTemperature, MaxTemperature);
        }
        Console.WriteLine($"  {temperature} °C - в допустимом диапазоне");
    }

    private static void Main()
    {
        Console.WriteLine("=== Задание 1. Ввод возраста ===\n");
        bool success = false;
        while (!success)
        {
            try
            {
                int age = ReadAge();
                Console.WriteLine($"Возраст принят: {age}");
                success = true;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Ошибка формата: {ex.Message}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Ошибка диапазона: введено {ex.ActualValue}, а возраст должен быть от 0 до 150");
            }
            finally
            {
                Console.WriteLine(success ? "Ввод завершён.\n" : "Повторите ввод.\n");
            }
        }

        Console.WriteLine("=== Задание 2. Проверка температуры ===\n");
        double[] measurements = { 23.5, -50, 50.1, -73.2, 36.6 };
        int errors = 0;
        foreach (double t in measurements)
        {
            try
            {
                CheckTemperature(t);
            }
            catch (TemperatureOutOfRangeException ex)
            {
                errors++;
                Console.WriteLine($"  Ошибка: {ex.Message}");
                double excess = ex.Temperature > ex.Max ? ex.Temperature - ex.Max : ex.Min - ex.Temperature;
                Console.WriteLine($"          превышение границы на {excess:F1} °C");
            }
        }
        Console.WriteLine($"\nПроверено измерений: {measurements.Length}, с ошибкой: {errors}");
    }
}