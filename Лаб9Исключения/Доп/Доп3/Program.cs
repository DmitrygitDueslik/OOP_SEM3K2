using System;
using System.Collections.Generic;

//  Иерархия собственных исключений
internal class CalculatorException : Exception
{
    public CalculatorException(string message) : base(message)
    {
    }

    public CalculatorException(string message, Exception inner) : base(message, inner)
    {
    }
}

internal class CalcDivideByZeroException : CalculatorException
{
    public CalcDivideByZeroException() : base("Деление на ноль")
    {
    }
}

internal class CalcOverflowException : CalculatorException
{
    public CalcOverflowException(string operation) : base($"Переполнение при операции {operation}")
    {
    }
}

internal class NothingToUndoException : CalculatorException
{
    public NothingToUndoException() : base("История пуста - нечего отменять")
    {
    }
}

internal class BatchFailedException : CalculatorException
{
    public BatchFailedException(int step, Exception inner)
        : base($"Цепочка операций прервана на шаге {step}", inner) { }
}

//  Калькулятор с историей
internal class CalculatorWithHistory
{
    public double Value { get; private set; }                 // текущее значение
    private Stack<(string Op, double Before)> undo = new();   // стек для отмены
    public List<string> History { get; } = new List<string>(); // журнал для вывода

    public CalculatorWithHistory(double start)
    { Value = start; }

    // Выполняет одну операцию; при ошибке значение не меняется
    public void Apply(char op, double operand)
    {
        double before = Value;
        string text = $"{before} {op} {operand}";
        try
        {
            double result = op switch
            {
                '+' => before + operand,
                '-' => before - operand,
                '*' => before * operand,
                '/' => operand == 0 ? throw new CalcDivideByZeroException() : before / operand,
                _ => throw new CalculatorException($"Неизвестная операция '{op}'")
            };
            if (double.IsInfinity(result)) throw new CalcOverflowException(text);

            Value = result;                  // изменение состояния — только после всех проверок
            undo.Push((text, before));
            History.Add($"{text} = {Value}");
        }
        catch (CalculatorException ex)
        {
            Value = before;                  // откат (на случай, если состояние успело измениться)
            History.Add($"ОШИБКА: {text} -> {ex.Message}");
            throw;
        }
    }

    // Отмена последней успешной операции
    public void Undo()
    {
        if (undo.Count == 0) throw new NothingToUndoException();
        var (op, before) = undo.Pop();
        Value = before;
        History.Add($"ОТМЕНА: {op} (значение снова {Value})");
    }

    // Выполняет цепочку операций атомарно: при ошибке откатываются ВСЕ шаги цепочки
    public void ApplyBatch((char Op, double Operand)[] steps)
    {
        int done = 0;
        try
        {
            foreach (var step in steps)
            {
                Apply(step.Op, step.Operand);
                done++;
            }
        }
        catch (CalculatorException ex)
        {
            // вложенный try: откат сам по себе может завершиться ошибкой
            try
            {
                for (int i = 0; i < done; i++) Undo();
                History.Add($"ОТКАТ ЦЕПОЧКИ: отменено шагов - {done}");
            }
            catch (NothingToUndoException)
            {
                History.Add("ОТКАТ ЦЕПОЧКИ: не удалось отменить все шаги");
            }
            throw new BatchFailedException(done + 1, ex);
        }
    }
}

internal class Program
{
    private static void Main()
    {
        CalculatorWithHistory calc = new CalculatorWithHistory(10);

        void Run(string title, Action action)
        {
            Console.Write($"{title,-36}");
            try
            {
                action();
                Console.WriteLine($"OK, значение = {calc.Value}");
            }
            catch (BatchFailedException ex)
            {
                Console.WriteLine($"{ex.Message}: {ex.InnerException?.Message}; значение = {calc.Value}");
            }
            catch (CalculatorException ex)
            {
                Console.WriteLine($"ошибка: {ex.Message}; значение = {calc.Value}");
            }
        }

        Console.WriteLine($"Начальное значение: {calc.Value}\n");
        Run("10 + 5", () => calc.Apply('+', 5));
        Run("* 4", () => calc.Apply('*', 4));
        Run("/ 0", () => calc.Apply('/', 0));
        Run("% 3 (неизвестная операция)", () => calc.Apply('%', 3));
        Run("* 1e308 (переполнение)", () => calc.Apply('*', 1e308));
        Run("Отмена последней операции", () => calc.Undo());
        Run("Цепочка: -5, *2, +1", () => calc.ApplyBatch(new[] { ('-', 5.0), ('*', 2.0), ('+', 1.0) }));
        Run("Цепочка: +100, *3, /0, +1", () => calc.ApplyBatch(new[] { ('+', 100.0), ('*', 3.0), ('/', 0.0), ('+', 1.0) }));
        Run("Отмена", () => calc.Undo());
        Run("Отмена", () => calc.Undo());
        Run("Отмена", () => calc.Undo());
        Run("Отмена", () => calc.Undo());
        Run("Отмена (история пуста)", () => calc.Undo());

        Console.WriteLine("\nИстория операций:");
        for (int i = 0; i < calc.History.Count; i++)
        {
            Console.WriteLine($"{i + 1,3}. {calc.History[i]}");
        }
    }
}