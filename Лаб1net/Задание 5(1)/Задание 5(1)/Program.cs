using System;
class Program
{
    static void Main()
    {
        Console.WriteLine("\nУпаковка double и определнного типа");

        double pi = 3.14;
        object obj = pi; // Упаковка double в object

        Console.WriteLine($"Исходное значение: {pi}");
        Console.WriteLine($"Упакованное значение: {obj}");

        Console.WriteLine($"\nТип упакованного значения: {obj.GetType()}");

        Console.WriteLine($"\nТип через typeof(double): {typeof(double)}");


        //Сравнение типов
        Console.WriteLine($"\nСравнение типов: {obj.GetType() == typeof(double)}");


        double unboxed = (double)obj; // Распаковка обратно в double

        Console.WriteLine($"Распакованное значение: {unboxed}");

        //Если распоковать неправильный тип, то будет ошибка InvalidCastException

        try
        {
            int wrongUnboxed = (int)obj; // Попытка распаковки в неправильный тип
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine($"\nОшибка при распаковке: {ex.Message}");
        }
    }
}