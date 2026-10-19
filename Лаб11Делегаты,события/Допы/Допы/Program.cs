using System;
using System.Collections.Generic;

// Делегат для арифметической операции
internal delegate double Operation(double a, double b);

// Статические методы операций
internal class MathOperations
{
    public static double Add(double a, double b) => a + b;

    public static double Subtract(double a, double b) => a - b;

    public static double Multiply(double a, double b) => a * b;

    public static double Divide(double a, double b) => a / b;
}

// Калькулятор с памятью и отменой
internal class Calculator
{
    public double Value { get; private set; }
    private Stack<double> history = new Stack<double>(); // предыдущие значения

    public Calculator(double start)
    {
        Value = start;
    }

    // Выбор операции по символу: возвращаем нужный метод как делегат
    public static Operation? GetOperation(char symbol)
    {
        switch (symbol)
        {
            case '+': return MathOperations.Add;
            case '-': return MathOperations.Subtract;
            case '*': return MathOperations.Multiply;
            case '/': return MathOperations.Divide;
            default: return null;
        }
    }

    public void Apply(char symbol, double number)
    {
        Operation? op = GetOperation(symbol);
        if (op == null)
        {
            Console.WriteLine($"Неизвестная операция '{symbol}'");
            return;
        }
        if (symbol == '/' && number == 0)
        {
            Console.WriteLine("Деление на ноль невозможно");
            return;
        }
        history.Push(Value);              // запоминаем значение до операции
        double before = Value;
        Value = op(Value, number);        // вызываем операцию через делегат
        Console.WriteLine($"{before} {symbol} {number} = {Value}");
    }

    public void Undo()
    {
        if (history.Count == 0)
        {
            Console.WriteLine("Отменять нечего");
            return;
        }
        Value = history.Pop();
        Console.WriteLine($"Отмена. Значение: {Value}");
    }
}

internal class Program
{
    private static void Main()
    {
        Calculator calc = new Calculator(10);
        Console.WriteLine($"Начальное значение: {calc.Value}\n");

        calc.Apply('+', 5);
        calc.Apply('*', 3);
        calc.Apply('-', 20);
        calc.Apply('/', 0);
        calc.Apply('%', 2);
        calc.Apply('/', 5);

        Console.WriteLine();
        calc.Undo();
        calc.Undo();
        calc.Apply('+', 100);
        calc.Undo();
        calc.Undo();
        calc.Undo();
        calc.Undo();

        Console.WriteLine($"\nИтоговое значение: {calc.Value}");
    }
}