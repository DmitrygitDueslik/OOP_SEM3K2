using System;
using System.Collections.Generic;

public abstract class Employee
{
    // защищенные поля (доступны в производных классах)
    protected string _name;
    protected int _id;
    protected double _baseSalary;

    // открытые свойства для доступа
    public string Name
    {
        get { return _name; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Имя не может быть пустым");
            _name = value;
        }
    }

    public int Id
    {
        get { return _id; }
        set { _id = value; }  // можно установить только внутри
    }

    // абстрактный метод - каждая должность считает зарплату по-своему
    public abstract double CalculateSalary();

    // виртуальный метод - может быть переопределен
    public virtual void DisplayInfo()
    {
        Console.WriteLine($"ID: {_id}, Имя: {_name}");
        Console.WriteLine($"Зарплата: {CalculateSalary()}");
    }
}

public class PermanentEmployee : Employee
{
    // свойство только для чтения
    public double MonthlySalary { get; private set; }

    public PermanentEmployee(string name, int id, double monthlySalary)
    {
        Name = name;
        Id = id;
        if (monthlySalary <= 0)
            throw new ArgumentException("Зарплата должна быть положительной");
        MonthlySalary = monthlySalary;
    }

    public override double CalculateSalary()
    {
        return MonthlySalary;
    }
}

public class HourlyEmployee : Employee
{
    private double _hourlyRate;
    private int _hoursWorked;

    public double HourlyRate
    {
        get { return _hourlyRate; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Ставка должна быть положительной");
            _hourlyRate = value;
        }
    }

    public int HoursWorked
    {
        get { return _hoursWorked; }
        set
        {
            if (value < 0 || value > 744)  // максимум часов в месяце
                throw new ArgumentException("Некорректное количество часов");
            _hoursWorked = value;
        }
    }

    public HourlyEmployee(string name, int id, double hourlyRate)
    {
        Name = name;
        Id = id;
        HourlyRate = hourlyRate;
        HoursWorked = 0;
    }

    public void AddHours(int hours)
    {
        if (hours > 0)
            HoursWorked += hours;
    }

    public override double CalculateSalary()
    {
        return _hourlyRate * _hoursWorked;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Ставка: {_hourlyRate:C}/час, Часов: {_hoursWorked}");
    }
}

public class Manager : Employee
{
    private double _fixedSalary;
    private double _bonus;

    public double Bonus
    {
        get { return _bonus; }
        set
        {
            if (value < 0)
                throw new ArgumentException("Бонус не может быть отрицательным");
            _bonus = value;
        }
    }

    public Manager(string name, int id, double fixedSalary)
    {
        Name = name;
        Id = id;
        if (fixedSalary <= 0)
            throw new ArgumentException("Зарплата должна быть положительной");
        _fixedSalary = fixedSalary;
        _bonus = 0;
    }

    public void SetBonus(double bonus)
    {
        Bonus = bonus;
    }

    public override double CalculateSalary()
    {
        return _fixedSalary + _bonus;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Оклад: {_fixedSalary}, Бонус: {_bonus}");
    }
}

class Program
{
    static void Main()
    {
        // создаем список сотрудников
        List<Employee> employees = new List<Employee>();

        PermanentEmployee perm1 = new PermanentEmployee("Иван Петров", 101, 50000);
        employees.Add(perm1);

        HourlyEmployee hourly1 = new HourlyEmployee("Мария Сидорова", 102, 300);
        hourly1.AddHours(160);  // отработала 160 часов
        employees.Add(hourly1);

        HourlyEmployee hourly2 = new HourlyEmployee("Петр Иванов", 103, 350);
        hourly2.AddHours(120);
        employees.Add(hourly2);

        Manager manager1 = new Manager("Анна Смирнова", 201, 80000);
        manager1.SetBonus(15000);
        employees.Add(manager1);

        Manager manager2 = new Manager("Олег Васильев", 202, 75000);
        manager2.SetBonus(20000);
        employees.Add(manager2);

        // выводим информацию о всех сотрудниках
        Console.WriteLine("=== ИНФОРМАЦИЯ О СОТРУДНИКАХ ===\n");

        double totalSalary = 0;
        foreach (Employee emp in employees)
        {
            emp.DisplayInfo();
            Console.WriteLine();
            totalSalary += emp.CalculateSalary();
        }

        Console.WriteLine($"ИТОГО: Фонд оплаты труда: {totalSalary}");
    }
}
