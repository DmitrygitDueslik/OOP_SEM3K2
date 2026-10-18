using System;

class Book
{
    // Поля класса
    public string title;
    public string author;
    public int year;

    // Метод для вывода информации
    public void DisplayInfo()
    {
        Console.WriteLine($"Книга: \"{title}\"");
        Console.WriteLine($"Автор: {author}");
        Console.WriteLine($"Год издания: {year}");
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        // Создание объекта (экземпляра класса)
        Book book1 = new Book();

        // Присваивание значений полям
        book1.title = "Война и мир";
        book1.author = "Лев Толстой";
        book1.year = 1869;

        // Вызов метода
        book1.DisplayInfo();

        // Создание второго объекта
        Book book2 = new Book();
        book2.title = "Преступление и наказание";
        book2.author = "Федор Достоевский";
        book2.year = 1866;

        book2.DisplayInfo();
    }
}
