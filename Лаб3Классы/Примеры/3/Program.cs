using System;

class Counter
{
    // Статическое поле - принадлежит классу, а не объекту
    private static int objectCount = 0;

    // Обычные поля - свои для каждого объекта
    private int id;
    private string name;

    // Конструктор
    public Counter(string name)
    {
        this.name = name;

        // Увеличиваем статический счетчик
        objectCount++;

        // Присваиваем ID на основе счетчика
        this.id = objectCount;

        Console.WriteLine($"Создан объект {id}: {name}");
    }

    // Статический метод для получения счетчика
    public static int GetObjectCount()
    {
        return objectCount;
    }

    // Обычный метод
    public void PrintInfo()
    {
        Console.WriteLine($"Объект {id}: {name}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine($"Начальное количество объектов: {Counter.GetObjectCount()}\n");

        Counter c1 = new Counter("Первый");
        Counter c2 = new Counter("Второй");
        Counter c3 = new Counter("Третий");

        Console.WriteLine($"\nКоличество созданных объектов: {Counter.GetObjectCount()}");

        c1.PrintInfo();
        c2.PrintInfo();
        c3.PrintInfo();
    }
}
