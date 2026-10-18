using System;
using System.Collections.Generic;

// —тудент сравниваетс€ Ђсам с собойї по средней оценке Ч IComparable<Student>
class Student : IComparable<Student>
{
    public string Name { get; set; }
    public int Age { get; set; }
    public double Grade { get; set; } // средн€€ оценка

    public Student(string name, int age, double grade)
    {
        Name = name;
        Age = age;
        Grade = grade;
    }

    // < 0 Ч текущий студент Ђменьшеї other, 0 Ч равны, > 0 Ч Ђбольшеї
    public int CompareTo(Student? other)
    {
        if (other == null) return 1; // любой объект больше null
        return Grade.CompareTo(other.Grade);
    }

    public override string ToString()
    {
        return $"{Name,-18} возраст: {Age}, средний балл: {Grade:F1}";
    }
}

// ¬нешний Ђсравнивательї Ч сортировка по имени, не мен€€ класс Student
class StudentComparer : IComparer<Student>
{
    public int Compare(Student? x, Student? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        return string.Compare(x.Name, y.Name, StringComparison.CurrentCulture);
    }
}

class Program
{
    static void Print(string title, Student[] students)
    {
        Console.WriteLine(title);
        foreach (Student s in students)
        {
            Console.WriteLine("  " + s);
        }
        Console.WriteLine();
    }

    static void Main()
    {
        Student[] students =
        {
            new Student("—мирнова ќльга", 20, 4.7),
            new Student("јбрамов ƒенис", 19, 3.9),
            new Student(" озлов »горь", 21, 4.2),
            new Student("¬асильева јнна", 18, 5.0),
            new Student("≈горов ѕавел", 20, 3.5)
        };

        Print("»сходный массив:", students);

        // Array.Sort без параметров использует IComparable<Student>.CompareTo
        Array.Sort(students);
        Print("—ортировка по среднему баллу (IComparable):", students);

        // Array.Sort с IComparer использует внешний компаратор
        Array.Sort(students, new StudentComparer());
        Print("—ортировка по имени (IComparer):", students);
    }
}
