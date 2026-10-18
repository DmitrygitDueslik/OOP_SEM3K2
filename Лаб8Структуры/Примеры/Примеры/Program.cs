using System;

// Объявление структуры Student
public struct Student
{
    // Поля структуры
    public string Name;
    public int Age;
    public double AverageGrade;

    // Конструктор
    public Student(string name, int age, double averageGrade)
    {
        Name = name;
        Age = age;
        AverageGrade = averageGrade;
    }

    // Метод для вывода информации о студенте
    public void DisplayInfo()
    {
        Console.WriteLine($"Студент: {Name}, Возраст: {Age}, Средний балл: {AverageGrade:F2}");
    }

    // Метод для проверки, является ли студент отличником
    public bool IsExcellentStudent()
    {
        return AverageGrade >= 4.5;
    }
}

class Program
{
    static void Main()
    {
        // Создание массива студентов
        Student[] students = new Student[5];

        // Заполнение массива
        students[0] = new Student("Иванов Иван", 19, 4.8);
        students[1] = new Student("Петрова Анна", 20, 4.2);
        students[2] = new Student("Сидоров Петр", 18, 3.9);
        students[3] = new Student("Козлова Мария", 19, 5.0);
        students[4] = new Student("Смирнов Алексей", 20, 4.6);

        // Вывод всех студентов
        Console.WriteLine("Список всех студентов:");
        Console.WriteLine("------------------------");
        foreach (Student student in students)
        {
            student.DisplayInfo();
        }

        // Вывод отличников
        Console.WriteLine("\nСписок отличников (средний балл >= 4.5):");
        Console.WriteLine("------------------------");
        int count = 0;
        foreach (Student student in students)
        {
            if (student.IsExcellentStudent())
            {
                student.DisplayInfo();
                count++;
            }
        }

        if (count == 0)
        {
            Console.WriteLine("Отличников нет");
        }

        Console.WriteLine($"\nВсего студентов: {students.Length}, Отличников: {count}");
    }
}
