using System;
    class Program
{
    static  void Main()
    {
        Console.WriteLine("Упаковка и распаковка (Boxing/Unboxing)\n");

        int number = 42;
        Console.WriteLine($"Значимый тип int: number={number}");
        Console.WriteLine($"Хранится в стеке\n");

        object boxedNumber = number;
        Console.WriteLine($"После упаковки:boxedNumber={boxedNumber}");
        Console.WriteLine($"boxedNumber - это ссылочный тип,хранящийся в куче");
        Console.WriteLine($"Тип boxedNumber:{boxedNumber.GetType()}\n");


        number = 100;
        Console.WriteLine($"Изменили исходное number на 100");
        Console.WriteLine($"number={number}");
        Console.WriteLine($"boxedNumber={boxedNumber}(не изменилось , т.к. это копичя в куче\n");

        int unboxedNumber = (int)boxedNumber;
        Console.WriteLine($"После распаковки:unboxedNumber={unboxedNumber}");

        Console.WriteLine("\nОшибка при распаковке");
        object obj= 123;
        Console.WriteLine($"obj={obj},тип:{obj.GetType()}");
        try
        {
            double wrongUnbox = (double)obj;

        }
        catch(InvalidCastException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            Console.WriteLine($"Распаковать можно только в тот тип,который был упакован!");
        }
    }
}