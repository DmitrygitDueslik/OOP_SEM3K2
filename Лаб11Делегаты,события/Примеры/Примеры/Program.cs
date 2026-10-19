using System;

// Объявляем делегат, который может ссылаться на методы,
// принимающие два целых числа и возвращающие целое число
// Это как бы "шаблон" для методов, которые мы сможем вызывать через этот делегат
internal delegate int MathOperation(int x, int y);

internal class Calculator
{
    // Метод сложения - подходит под сигнатуру делегата (принимает два int, возвращает int)
    public int Add(int a, int b)
    {
        Console.WriteLine($"Выполняется сложение {a} + {b}");
        return a + b;
    }

    // Метод вычитания - тоже подходит под сигнатуру делегата
    public int Subtract(int a, int b)
    {
        Console.WriteLine($"Выполняется вычитание {a} - {b}");
        return a - b;
    }

    // Метод умножения - тоже подходит
    public int Multiply(int a, int b)
    {
        Console.WriteLine($"Выполняется умножение {a} * {b}");
        return a * b;
    }
}

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Простой делегат\n");

        // Создаем экземпляр калькулятора
        Calculator calc = new Calculator();

        // Создаем переменную делегата и присваиваем ей метод Add
        // Теперь переменная operation хранит ссылку на метод calc.Add
        // Важно: метод не вызывается здесь, только сохраняется ссылка на него
        MathOperation operation = calc.Add;

        // Вызываем метод через делегат
        // Это равносильно вызову calc.Add(10, 5)
        Console.WriteLine("Вызов делегата, указывающего на Add:");
        int result = operation(10, 5);
        Console.WriteLine($"Результат: {result}\n");

        // Перенаправляем делегат на другой метод
        // Теперь operation хранит ссылку на метод calc.Subtract
        operation = calc.Subtract;
        Console.WriteLine("Вызов делегата, указывающего на Subtract:");
        result = operation(10, 5);
        Console.WriteLine($"Результат: {result}\n");

        // Снова перенаправляем на другой метод
        operation = calc.Multiply;
        Console.WriteLine("Вызов делегата, указывающего на Multiply:");
        result = operation(10, 5);
        Console.WriteLine($"Результат: {result}\n");

        // Демонстрация передачи делегата как параметра метода
        // Это главное преимущество делегатов - мы можем передавать методы как данные
        Console.WriteLine("Передача делегата в качестве параметра:");

        // Передаем разные методы в один и тот же метод ExecuteOperation
        ExecuteOperation(calc.Add, 15, 3);      // Будет выполнено сложение
        ExecuteOperation(calc.Subtract, 15, 3); // Будет выполнено вычитание
        ExecuteOperation(calc.Multiply, 15, 3); // Будет выполнено умножение
    }

    // Метод, принимающий делегат в качестве параметра
    // Это позволяет нам передавать ЛЮБОЙ метод, соответствующий сигнатуре делегата
    // Таким образом, мы можем изменять поведение метода ExecuteOperation,
    // не меняя сам метод, а просто передавая разные операции
    private static void ExecuteOperation(MathOperation op, int a, int b)
    {
        Console.Write($"Выполняем операцию с числами {a} и {b}: ");
        int result = op(a, b); // Вызываем метод через переданный делегат
        Console.WriteLine($"Результат операции: {result}");
    }
}