using System;
using System.Collections.Generic;

// Конвейер: список шагов-лямбд, которые выполняются по очереди
internal class Pipeline<T>
{
    private List<Func<T, T>> steps = new List<Func<T, T>>();

    public void AddStep(Func<T, T> step) => steps.Add(step);

    public void InsertStep(int index, Func<T, T> step) => steps.Insert(index, step);

    public void RemoveStep(int index) => steps.RemoveAt(index);

    public int GetStepCount() => steps.Count;

    public void Clear() => steps.Clear();

    // Пропускаем значение через все шаги
    public T Execute(T input)
    {
        T value = input;
        foreach (Func<T, T> step in steps)
        {
            value = step(value);
        }
        return value;
    }

    // То же для каждого элемента массива
    public T[] ExecuteAll(T[] inputs)
    {
        return Array.ConvertAll(inputs, x => Execute(x));
    }
}

internal class Program
{
    private static void Main()
    {
        // ===== Числа =====
        Pipeline<int> numbers = new Pipeline<int>();
        numbers.AddStep(x => x + 1);   // +1
        numbers.AddStep(x => x * 2);   // *2
        numbers.AddStep(x => x * x);   // в квадрат

        Console.WriteLine($"Шагов: {numbers.GetStepCount()}");
        Console.WriteLine($"Execute(3) = {numbers.Execute(3)}   // ((3 + 1) * 2)^2");

        int[] input = { 0, 1, 2, 3 };
        Console.WriteLine($"ExecuteAll({string.Join(", ", input)}) = {string.Join(", ", numbers.ExecuteAll(input))}");

        numbers.InsertStep(0, x => -x); // в начало: сменить знак
        Console.WriteLine($"\nПосле InsertStep(0, x => -x): Execute(3) = {numbers.Execute(3)}   // ((-3 + 1) * 2)^2");

        numbers.RemoveStep(3);          // убрать возведение в квадрат
        Console.WriteLine($"После RemoveStep(3): шагов {numbers.GetStepCount()}, Execute(3) = {numbers.Execute(3)}");

        numbers.Clear();
        Console.WriteLine($"После Clear: шагов {numbers.GetStepCount()}, Execute(3) = {numbers.Execute(3)}");

        // ===== Строки =====
        Pipeline<string> text = new Pipeline<string>();
        text.AddStep(s => s.Trim());
        text.AddStep(s => s.ToLower());
        text.AddStep(s => s.Replace(" ", "_"));

        string[] names = { "  Hello World ", "C Sharp", " Лямбда Выражения" };
        Console.WriteLine("\nСтроки:");
        foreach (string s in text.ExecuteAll(names))
        {
            Console.WriteLine($"  [{s}]");
        }
    }
}