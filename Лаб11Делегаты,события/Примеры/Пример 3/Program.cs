using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Стандартные делегаты Action, Func, Predicate\n");

        // Action - делегат, который не возвращает значение (void)
        // Action может иметь от 0 до 16 параметров
        Console.WriteLine("1. Action - методы, не возвращающие значение:");

        // Action без параметров
        Action sayHello = SayHello;
        sayHello(); // Вызываем делегат

        // Action с одним параметром (string)
        Action<string> printMessage = PrintMessage;
        printMessage("Это сообщение выведено через Action");

        // Action с двумя параметрами
        Action<int, int> printSum = (a, b) =>
            Console.WriteLine($"  Сумма {a} и {b} = {a + b}");
        printSum(10, 5);

        Console.WriteLine();

        // Func - делегат, который возвращает значение
        // Последний тип в угловых скобках - это тип возвращаемого значения
        Console.WriteLine("2. Func - методы, возвращающие значение:");

        // Func без параметров, возвращает int
        Func<int> getRandomNumber = GetRandomNumber;
        int random = getRandomNumber();
        Console.WriteLine($"  Случайное число: {random}");

        // Func с одним параметром, возвращает string
        Func<int, string> numberToString = n => $"Число {n}";
        string str = numberToString(42);
        Console.WriteLine($"  {str}");

        // Func с двумя параметрами, возвращает int
        Func<int, int, int> multiply = (x, y) => x * y;
        int product = multiply(6, 7);
        Console.WriteLine($"  Произведение 6 и 7 = {product}");

        // Func для сложения
        Func<int, int, int> add = (x, y) => x + y;
        Console.WriteLine($"  Сумма 15 и 25 = {add(15, 25)}");

        Console.WriteLine();

        // Predicate - делегат, возвращающий bool (обычно для проверки условий)
        // Эквивалентен Func<T, bool>
        Console.WriteLine("3. Predicate - проверка условий:");

        // Создаем список чисел
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        Console.WriteLine("  Список чисел: " + string.Join(", ", numbers));

        // Predicate для проверки, является ли число четным
        Predicate<int> isEven = n => n % 2 == 0;

        // Используем FindAll для поиска всех четных чисел
        List<int> evenNumbers = numbers.FindAll(isEven);
        Console.WriteLine("  Четные числа: " + string.Join(", ", evenNumbers));

        // Predicate для проверки, больше ли число 5
        Predicate<int> isGreaterThan5 = n => n > 5;
        List<int> greaterThan5 = numbers.FindAll(isGreaterThan5);
        Console.WriteLine("  Числа больше 5: " + string.Join(", ", greaterThan5));

        Console.WriteLine();

        // Демонстрация передачи стандартных делегатов в методы
        Console.WriteLine("4. Передача делегатов в методы:");

        ProcessNumbers(numbers, n => Console.Write($"{n * n} "), "Квадраты чисел");
        ProcessNumbers(numbers, n => Console.Write($"{n * 2} "), "Числа, умноженные на 2");

        // Фильтрация с передачей предиката
        List<int> filtered = FilterNumbers(numbers, n => n % 3 == 0);
        Console.WriteLine($"  Числа, кратные 3: {string.Join(", ", filtered)}");
    }

    // Обычные методы для демонстрации
    private static void SayHello()
    {
        Console.WriteLine("  Привет от метода SayHello!");
    }

    private static void PrintMessage(string msg)
    {
        Console.WriteLine($"  {msg}");
    }

    private static int GetRandomNumber()
    {
        Random rand = new Random();
        return rand.Next(1, 100);
    }

    // Метод, принимающий Action для обработки каждого элемента
    private static void ProcessNumbers(List<int> numbers, Action<int> processor, string title)
    {
        Console.Write($"  {title}: ");
        foreach (int n in numbers)
        {
            processor(n); // Вызываем переданный делегат для каждого числа
        }
        Console.WriteLine();
    }

    // Метод, принимающий Predicate для фильтрации
    private static List<int> FilterNumbers(List<int> numbers, Predicate<int> predicate)
    {
        List<int> result = new List<int>();
        foreach (int n in numbers)
        {
            if (predicate(n)) // Проверяем условие через делегат
            {
                result.Add(n);
            }
        }
        return result;
    }
}