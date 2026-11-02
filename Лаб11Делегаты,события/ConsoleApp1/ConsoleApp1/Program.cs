using System;

internal delegate double Operation(double a, double b);

internal class Calculator
{
    public static double Add(double a, double b)
    {
        return a + b;
    }

    public static double Subtract(double a, double b)
    {
        return a - b;
    }

    public static double Multiply(double a, double b)
    {
        return a * b;
    }

    public static double Divide(double a, double b)
    {
        return a / b;
    }
}

internal class Program
{
    private static void Run(Operation? op, double a, double b)
    {
        if (op == null) return;

        foreach (Operation method in op.GetInvocationList())
        {
            Console.WriteLine($"  {method.Method.Name}: {method(a, b)}");
        }
    }

    private static void Main()
    {
        double x = 12, y = 4;

        Operation? op = Calculator.Add;
        op += Calculator.Subtract;
        op += Calculator.Multiply;
        op += Calculator.Divide;

        Console.WriteLine("Работают 4 операции:");
        Run(op, x, y);

        op -= Calculator.Add;
        Console.WriteLine("\nРаботают 3 операции (убрали +):");
        Run(op, x, y);

        op -= Calculator.Subtract;
        Console.WriteLine("\nРаботают 2 операции (убрали -):");
        Run(op, x, y);

        Console.WriteLine($"\nПрямой вызов op(x, y) : {op?.Invoke(x, y)}");
    }
}