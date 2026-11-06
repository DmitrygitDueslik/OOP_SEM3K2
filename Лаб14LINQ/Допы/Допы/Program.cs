using System;
using System.Collections.Generic;
using System.Linq;

internal class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}

internal class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

internal class Sale
{
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public int Quantity { get; set; }
    public int Month { get; set; }
}

internal class Program
{
    private static void Main()
    {
        List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Ноутбук",  Price = 60000 },
            new Product { Id = 2, Name = "Мышь",     Price = 800 },
            new Product { Id = 3, Name = "Монитор",  Price = 15000 },
            new Product { Id = 4, Name = "Флешка",   Price = 600 },
            new Product { Id = 5, Name = "Наушники", Price = 3000 },
            new Product { Id = 6, Name = "Принтер",  Price = 12000 },
            new Product { Id = 7, Name = "Веб-камера", Price = 2500 }
        };
        List<Customer> customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Иванов" },
            new Customer { Id = 2, Name = "Петрова" },
            new Customer { Id = 3, Name = "Сидоров" }
        };
        List<Sale> sales = new List<Sale>
        {
            new Sale { ProductId = 1, CustomerId = 1, Quantity = 1, Month = 1 },
            new Sale { ProductId = 2, CustomerId = 1, Quantity = 2, Month = 1 },
            new Sale { ProductId = 2, CustomerId = 2, Quantity = 1, Month = 1 },
            new Sale { ProductId = 4, CustomerId = 3, Quantity = 3, Month = 2 },
            new Sale { ProductId = 3, CustomerId = 2, Quantity = 1, Month = 2 },
            new Sale { ProductId = 2, CustomerId = 3, Quantity = 1, Month = 2 },
            new Sale { ProductId = 5, CustomerId = 2, Quantity = 2, Month = 3 },
            new Sale { ProductId = 4, CustomerId = 1, Quantity = 1, Month = 3 },
            new Sale { ProductId = 1, CustomerId = 3, Quantity = 1, Month = 3 },
            new Sale { ProductId = 5, CustomerId = 3, Quantity = 1, Month = 3 }
        };

        // Соединяем продажи с товарами и клиентами - дальше работаем с полными данными
        var details = from s in sales
                      join p in products on s.ProductId equals p.Id
                      join c in customers on s.CustomerId equals c.Id
                      select new { Product = p.Name, Customer = c.Name, s.Month, s.Quantity, Sum = p.Price * s.Quantity };

        // 1. Топ-5 товаров по количеству продаж (число сделок)
        Console.WriteLine("1. Топ-5 товаров по количеству продаж:");
        var top = details.GroupBy(d => d.Product)
                         .Select(g => new { Product = g.Key, Count = g.Count() })
                         .OrderByDescending(x => x.Count).ThenBy(x => x.Product)
                         .Take(5);
        foreach (var x in top)
            Console.WriteLine($"   {x.Product,-10} продаж: {x.Count}");

        // 2. Самый прибыльный клиент
        var best = details.GroupBy(d => d.Customer)
                          .Select(g => new { Customer = g.Key, Total = g.Sum(d => d.Sum) })
                          .OrderByDescending(x => x.Total)
                          .First();
        Console.WriteLine($"\n2. Самый прибыльный клиент: {best.Customer} ({best.Total} руб.)");

        // 3. Продажи по месяцам с нарастающим итогом
        Console.WriteLine("\n3. Продажи по месяцам:");
        decimal runningTotal = 0;
        foreach (var m in details.GroupBy(d => d.Month).OrderBy(g => g.Key))
        {
            decimal monthSum = m.Sum(d => d.Sum);
            runningTotal += monthSum;
            Console.WriteLine($"   Месяц {m.Key}: {monthSum,7} руб., нарастающий итог: {runningTotal,7} руб.");
        }

        // 4. Товары, которые ни разу не продавались
        var unsold = products.Where(p => !sales.Any(s => s.ProductId == p.Id)).Select(p => p.Name);
        Console.WriteLine($"\n4. Не продавались: {string.Join(", ", unsold)}");

        // 5. Средний чек каждого клиента
        Console.WriteLine("\n5. Средний чек клиентов:");
        foreach (var g in details.GroupBy(d => d.Customer).OrderBy(g => g.Key))
            Console.WriteLine($"   {g.Key,-8} {g.Average(d => d.Sum):F2} руб.");
    }
}