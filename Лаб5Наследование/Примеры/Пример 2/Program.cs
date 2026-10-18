using System;

class Employee
{
    public string Name { get; set; }
    protected double baseSalary; // защищенное поле - доступно в наследниках

    public Employee(string name, double baseSalary)
    {
        Name = name;
        this.baseSalary = baseSalary;
    }

    // Виртуальный метод - может быть переопределен в наследниках
    public virtual double CalculateSalary()
    {
        return baseSalary;
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Сотрудник: {Name}, зарплата: {CalculateSalary()} руб.");
    }
}

class Manager : Employee
{
    private double bonus;

    public Manager(string name, double baseSalary, double bonus)
        : base(name, baseSalary)
    {
        this.bonus = bonus;
    }

    // Переопределение виртуального метода
    public override double CalculateSalary()
    {
        return baseSalary + bonus;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Менеджер: {Name}, оклад: {baseSalary}, бонус: {bonus}, всего: {CalculateSalary()} руб.");
    }
}

class Developer : Employee
{
    private int projectsCount;
    private double projectRate;

    public Developer(string name, double baseSalary, int projectsCount, double projectRate)
        : base(name, baseSalary)
    {
        this.projectsCount = projectsCount;
        this.projectRate = projectRate;
    }

    // Переопределение виртуального метода
    public override double CalculateSalary()
    {
        return baseSalary + (projectsCount * projectRate);
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Разработчик: {Name}, оклад: {baseSalary}, проекты: {projectsCount}, всего: {CalculateSalary()} руб.");
    }
}

class Program
{
    static void Main()
    {
        // Демонстрация полиморфизма
        Console.WriteLine("Полиморфизм\n");

        // Массив объектов базового класса, но содержащий объекты разных типов
        Employee[] employees = new Employee[]
        {
            new Employee("Иван Петров", 30000),
            new Manager("Анна Сидорова", 50000, 15000),
            new Developer("Петр Иванов", 40000, 3, 5000)
        };

        // Полиморфный вызов - для каждого объекта вызывается его собственная реализация
        foreach (Employee emp in employees)
        {
            emp.DisplayInfo();
        }

        Console.WriteLine("\nДемонстрация работы с конкретными типами");

        foreach (Employee emp in employees)
        {
            Console.WriteLine($"{emp.Name} заработал: {emp.CalculateSalary()} руб.");
        }
    }
}
