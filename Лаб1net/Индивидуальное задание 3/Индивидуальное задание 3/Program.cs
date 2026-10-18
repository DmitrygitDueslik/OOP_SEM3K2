using System;
class Program
{
    static void Main()
    {
        int x = 5;
        object obj1 = x;
        object obj2 = obj1;
        Console.WriteLine($"x: {x}, \nobj1: {obj1}, \nobj2: {obj2}");
        Console.WriteLine("\nobj1 и obj2 ссылаются на один и тот же объект в памяти, который хранит значение 5.");

        x = 10;
        Console.WriteLine($"\nobj1: {obj1}, obj2: {obj2}"); // Output: obj1: 5, obj2: 5
        Console.WriteLine($"\nОни остались с теми же значениями, что и были при присвоении, так как int является типом значения, а не ссылочным типом.");
        int y=(int)obj1;

        Console.WriteLine($"Распаковали obj1 в y: y={y}");
        y = 99; // меняем y - это отдельная переменная в стеке
        Console.WriteLine("\nИзменили y на 99");
        Console.WriteLine($"y = {y}");
        Console.WriteLine($"obj1 = {obj1}  (не изменилось)");

    }
}