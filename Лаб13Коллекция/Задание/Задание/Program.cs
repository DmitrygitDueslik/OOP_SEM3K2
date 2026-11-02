using System;
using System.Collections.Generic;

internal class Program
{
    // Обычные методы-задачи для очереди
    private static void SaveReport()
    {
        Console.WriteLine("  Отчёт сохранён");
    }

    private static void SendEmail()
    {
        Console.WriteLine("  Письмо отправлено");
    }

    private static void Main()
    {
        //  Задание 1
        Console.WriteLine("=== Задание 1. List<string> с городами ===\n");

        List<string> cities = new List<string>();
        cities.Add("Москва");
        cities.Add("Минск");
        cities.Add("Брест");
        cities.Add("Гродно");
        cities.Add("Омск");
        cities.Add("Пинск");
        cities.Add("Мурманск");
        cities.Add("Сочи");
        Console.WriteLine("Исходный список:       " + string.Join(", ", cities));

        cities.Sort();
        Console.WriteLine("После сортировки:      " + string.Join(", ", cities));

        List<string> mCities = cities.FindAll(city => city.StartsWith("М"));
        Console.WriteLine("Начинаются с \"М\":      " + string.Join(", ", mCities));

        int removed = cities.RemoveAll(city => city.Length < 5);
        Console.WriteLine($"Удалено коротких:      {removed}");
        Console.WriteLine("Итоговый список:       " + string.Join(", ", cities));

        //  Задание 2
        Console.WriteLine("\n=== Задание 2. Queue<Action> с задачами ===\n");

        Queue<Action> tasks = new Queue<Action>();
        tasks.Enqueue(() => Console.WriteLine("  Почта проверена"));
        tasks.Enqueue(SaveReport);
        tasks.Enqueue(() => Console.WriteLine("  Встреча проведена"));
        tasks.Enqueue(SendEmail);

        Console.WriteLine($"Задач в очереди: {tasks.Count}");
        int number = 1;
        while (tasks.Count > 0)
        {
            Action task = tasks.Dequeue();
            Console.WriteLine($"Задача {number}:");
            task();
            number++;
        }
        Console.WriteLine($"Задач в очереди: {tasks.Count}");
    }
}