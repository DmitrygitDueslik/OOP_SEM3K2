using System;

class Student
{
    // Закрытые поля
    private string name;
    private int age;
    private double grade;

    // Конструктор по умолчанию
    public Student()
    {
        name = "Неизвестно";
        age = 0;
        grade = 0;
    }

    // Конструктор с параметрами
    public Student(string name, int age, double grade)
    {
        this.name = name;
        this.age = age;
        this.grade = grade;
    }

    // Свойства
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Age
    {
        get { return age; }
        set { age = value; }
    }

    public double Grade
    {
        get { return grade; }
        set { grade = value; }
    }

    // Метод для вывода информации
    public void PrintInfo()
    {
        Console.WriteLine($"Студент: {name}, возраст: {age}, оценка: {grade}");
    }
}

class Program
{
    static void Main()
    {
        // Создание через конструктор по умолчанию
        Student s1 = new Student();
        s1.PrintInfo();

        // Установка значений через свойства
        s1.Name = "Иван Петров";
        s1.Age = 20;
        s1.Grade = 4.5;
        s1.PrintInfo();

        // Создание через конструктор с параметрами
        Student s2 = new Student("Анна Сидорова", 19, 4.8);
        s2.PrintInfo();
    }
}
