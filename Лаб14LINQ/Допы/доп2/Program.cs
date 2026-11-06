using System;
using System.Collections.Generic;
using System.Linq;

internal class Employee
{
    public string Name { get; set; } = "";
    public string Department { get; set; } = "";
    public string Gender { get; set; } = "";
    public int Age { get; set; }
    public decimal Salary { get; set; }
}

internal class Program
{
    private static void Main()
    {
        List<Employee> employees = new List<Employee>
        {
            new Employee { Name = "Иванов",   Department = "Разработка", Gender = "М", Age = 28, Salary = 120000 },
            new Employee { Name = "Петрова",  Department = "Разработка", Gender = "Ж", Age = 31, Salary = 115000 },
            new Employee { Name = "Сидоров",  Department = "Разработка", Gender = "М", Age = 35, Salary = 150000 },
            new Employee { Name = "Козлова",  Department = "Разработка", Gender = "Ж", Age = 25, Salary = 90000 },
            new Employee { Name = "Морозов",  Department = "Бухгалтерия", Gender = "М", Age = 45, Salary = 80000 },
            new Employee { Name = "Волкова",  Department = "Бухгалтерия", Gender = "Ж", Age = 38, Salary = 75000 },
            new Employee { Name = "Лебедева", Department = "Бухгалтерия", Gender = "Ж", Age = 50, Salary = 85000 },
            new Employee { Name = "Орлов",    Department = "Продажи",   Gender = "М", Age = 29, Salary = 70000 },
            new Employee { Name = "Зайцева",  Department = "Продажи",   Gender = "Ж", Age = 27, Salary = 72000 },
            new Employee { Name = "Белов",    Department = "Продажи",   Gender = "М", Age = 33, Salary = 78000 }
        };

        // Двухуровневая группировка: сначала по отделу, внутри отдела - по полу
        var groups = employees
            .GroupBy(e => e.Department)
            .OrderBy(d => d.Key)
            .Select(d => new
            {
                Department = d.Key,
                ByGender = d.GroupBy(e => e.Gender)
                            .OrderBy(g => g.Key)
                            .Select(g => new
                            {
                                Gender = g.Key,
                                Count = g.Count(),
                                AvgAge = g.Average(e => e.Age),
                                AvgSalary = g.Average(e => e.Salary)
                            })
            });

        // Вывод в виде таблицы
        string line = new string('-', 62);
        Console.WriteLine(line);
        Console.WriteLine($"| {"Отдел",-12} | {"Пол",-3} | {"Кол-во",6} | {"Ср. возраст",11} | {"Ср. зарплата",13} |");
        Console.WriteLine(line);
        foreach (var dept in groups)
        {
            foreach (var g in dept.ByGender)
            {
                Console.WriteLine($"| {dept.Department,-12} | {g.Gender,-3} | {g.Count,6} | {g.AvgAge,11:F1} | {g.AvgSalary,13:F0} |");
            }
            Console.WriteLine(line);
        }
    }
}