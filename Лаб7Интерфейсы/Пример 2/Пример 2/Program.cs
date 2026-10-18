using System;

// Интерфейс для обычного работника
interface IWorker
{
    // Метод для выполнения работы
    void Work();
}

// Интерфейс для менеджера
interface IManager
{
    // Метод для выполнения работы (то же имя, что и в IWorker)
    void Work();
}

// Класс Employee реализует оба интерфейса
class Employee : IWorker, IManager
{
    public string Name { get; set; }

    public Employee(string name)
    {
        Name = name;
    }

    // Неявная (обычная) реализация метода Work из интерфейса IWorker
    // Этот метод будет вызываться, когда объект используется как IWorker
    public void Work()
    {
        Console.WriteLine($"{Name} выполняет обычную работу");
    }

    // Явная реализация метода Work из интерфейса IManager
    // Обратите внимание: нет модификатора public, перед методом указано имя интерфейса
    // Этот метод можно вызвать только через ссылку типа IManager
    void IManager.Work()
    {
        Console.WriteLine($"{Name} управляет командой");
    }

    // Обычный метод класса
    public void DisplayInfo()
    {
        Console.WriteLine($"Сотрудник: {Name}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Явная реализация интерфейсов\n");

        // Создаем объект класса Employee
        Employee emp = new Employee("Иван Петров");
        emp.DisplayInfo(); // Вызов обычного метода класса

        Console.WriteLine("\nВызов метода Work через объект класса:");
        emp.Work(); // Вызывается неявная реализация (IWorker.Work)

        Console.WriteLine("\nВызов метода Work через интерфейс IWorker:");
        IWorker worker = emp; // Приводим к типу IWorker
        worker.Work(); // Вызывается неявная реализация (та же, что и через объект)

        Console.WriteLine("\nВызов метода Work через интерфейс IManager:");
        IManager manager = emp; // Приводим к типу IManager
        manager.Work(); // Вызывается явная реализация (IManager.Work)

        Console.WriteLine("\nПопытка вызвать явную реализацию через объект:");
        // emp.Work() - это вызовет неявную реализацию
        // Явную реализацию можно вызвать только через интерфейсный тип
    }
}
