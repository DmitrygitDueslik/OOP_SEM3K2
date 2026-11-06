using System;

internal class Student
{
    public string Name { get; set; } = "";
    public string Group { get; set; } = "";
    public string Faculty { get; set; } = "";

    public List<int> Grades { get; set; } = new List<int>();

    public double Average => Grades.Average();
}

internal class Teacher
{
    public string Name { get; set; } = "";
    public string Department { get; set; } = "";
}

internal class Program
{
    private static void Main()
    {
        List<Student> students = new List<Student>()
        {new Student { Name = "Иванов Иван",     Group = "ПИ-101", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5, 5 } },
            new Student { Name = "Петрова Анна",    Group = "ПИ-101", Faculty = "ФИТ", Grades = new List<int> { 4, 4, 3, 4 } },
            new Student { Name = "Сидоров Пётр",    Group = "ПИ-101", Faculty = "ФИТ", Grades = new List<int> { 5, 5, 4, 4 } },
            new Student { Name = "Козлова Мария",   Group = "ПИ-101", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 4, 3 } },
            new Student { Name = "Смирнов Алексей", Group = "ПИ-102", Faculty = "ФИТ", Grades = new List<int> { 5, 5, 5, 4 } },
            new Student { Name = "Новикова Елена",  Group = "ПИ-102", Faculty = "ФИТ", Grades = new List<int> { 4, 5, 4, 5 } },
            new Student { Name = "Морозов Денис",   Group = "ЭК-201", Faculty = "Экономика", Grades = new List<int> { 4, 3, 4, 4 } },
            new Student { Name = "Волкова Ольга",   Group = "ЭК-201", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5, 5 } },
            new Student { Name = "Лебедев Игорь",   Group = "ПИ-101", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 4, 4 } },
            new Student { Name = "Егорова Дарья",   Group = "ЭК-202", Faculty = "Экономика", Grades = new List<int> { 3, 3, 4, 3 } }
        };

        List<Teacher> teachers = new List<Teacher>
        {
            new Teacher { Name = "Белов Николай",    Department = "ФИТ" },
            new Teacher { Name = "Орлова Светлана",  Department = "ФИТ" },
            new Teacher { Name = "Фёдоров Андрей",   Department = "Экономика" },
            new Teacher { Name = "Зайцева Ирина",    Department = "ФИТ" }
        };

        Console.WriteLine("=== Задание 1. Группа ПИ-101, средний балл > 4.0 ===\n");

        var goodStudents = from s in students
                           where s.Group == "ПИ-101" && s.Average > 4.0
                           orderby s.Average descending
                           select s;

        foreach (Student s in goodStudents)
        {
            Console.WriteLine($"  {s.Name,-16} оценки: {string.Join(" ", s.Grades)}  средний: {s.Average:F2}");
        }

        Console.WriteLine("\n=== Задание 2. Все люди факультета ФИТ ===\n");

        string faculty = "ФИТ";

        var people = students
                        .Where(s => s.Faculty == faculty)
                        .Select(s => new { s.Name, Status = "Студент" })
                        .Concat(teachers
                         .Where(t => t.Department == faculty)
                           .Select(t => new { t.Name, Status = "Преподаватель" }))
                          .OrderBy(p => p.Name);

        foreach (var p in people)
        {
            Console.WriteLine($"  {p.Name,-16} {p.Status}");
        }
        Console.WriteLine($"\nВсего на факультете {faculty}: {people.Count()} человек");
    }
}