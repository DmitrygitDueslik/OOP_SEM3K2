using System;

struct Product
{
    public string Name;
    public double Price;
    public int Quantity;
    public string Category;

    public Product(string name, double price, int quantity, string category)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        Category = category;
    }

    public double GetTotalCost()
    {
        return Price * Quantity;
    }
}

enum FileSizeUnit { Byte, KB, MB, GB, TB }

class Program
{
    // Переводит байты в подходящую единицу
    static (double, FileSizeUnit) ConvertSize(long bytes)
    {
        double size = bytes;
        FileSizeUnit unit = FileSizeUnit.Byte;

        while (size >= 1024 && unit != FileSizeUnit.TB)
        {
            size = size / 1024;
            unit++;
        }
        return (size, unit);
    }

    static void Main()
    {
        // Задание 1
        Product[] products = new Product[5];
        products[0] = new Product("Ноутбук", 65000, 4, "Электроника");
        products[1] = new Product("Телефон", 32000, 10, "Электроника");
        products[2] = new Product("Кофе", 890, 120, "Продукты");
        products[3] = new Product("Куртка", 7500, 15, "Одежда");
        products[4] = new Product("Книга", 1200, 30, "Книги");

        foreach (Product p in products)
        {
            Console.WriteLine($"{p.Name} ({p.Category}): {p.Price} руб. x {p.Quantity} шт. = {p.GetTotalCost()} руб.");
        }

        Product maxProduct = products[0];
        foreach (Product p in products)
        {
            if (p.Price > maxProduct.Price)
            {
                maxProduct = p;
            }
        }
        Console.WriteLine($"Самый дорогой продукт: {maxProduct.Name}, {maxProduct.Price} руб.");

        // Задание 2
        Console.WriteLine();
        long[] sizes = { 500, 1536, 5242880, 3221225472 };
        foreach (long bytes in sizes)
        {
            (double value, FileSizeUnit unit) = ConvertSize(bytes);
            Console.WriteLine($"{bytes} байт -> {value} {unit}");
        }
    }
}