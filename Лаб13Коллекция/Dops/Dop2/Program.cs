using System;
using System.Collections.Generic;

internal class Order
{
    public int Id;
    public string Customer;
    public decimal Amount;

    public Order(int id, string customer, decimal amount)
    {
        Id = id;
        Customer = customer;
        Amount = amount;
    }

    public override string ToString() => $"№{Id} {Customer}, {Amount} руб.";
}

internal class OrderSystem
{
    private Queue<Order> pending = new Queue<Order>();             // ждут обработки
    private Dictionary<int, Order> byId = new Dictionary<int, Order>(); // быстрый поиск
    private Stack<Order> history = new Stack<Order>();             // обработанные
    private List<Order> all = new List<Order>();                   // все заказы
    private HashSet<int> usedIds = new HashSet<int>();             // занятые номера

    public void Add(Order order)
    {
        if (!usedIds.Add(order.Id)) // Add вернёт false, если номер уже был
        {
            Console.WriteLine($"Ошибка: номер {order.Id} уже использован");
            return;
        }
        pending.Enqueue(order);
        byId[order.Id] = order;
        all.Add(order);
        Console.WriteLine($"Добавлен заказ {order}");
    }

    public void ProcessNext()
    {
        if (pending.Count == 0)
        {
            Console.WriteLine("Очередь пуста");
            return;
        }
        Order order = pending.Dequeue();
        history.Push(order);
        Console.WriteLine($"Обработан заказ {order}");
    }

    public void UndoLast()
    {
        if (history.Count == 0)
        {
            Console.WriteLine("Нечего отменять");
            return;
        }
        Order order = history.Pop();
        pending.Enqueue(order); // возвращаем в очередь
        Console.WriteLine($"Отменена обработка заказа {order}, он снова в очереди");
    }

    public void Find(int id)
    {
        if (byId.TryGetValue(id, out Order? order))
            Console.WriteLine($"Найден заказ {order}");
        else
            Console.WriteLine($"Заказ №{id} не найден");
    }

    public void Print()
    {
        Console.WriteLine($"  Всего заказов: {all.Count}, ждут: {pending.Count}, обработано: {history.Count}");
        Console.WriteLine($"  В очереди: {string.Join("; ", pending)}");
    }
}

internal class Program
{
    private static void Main()
    {
        OrderSystem system = new OrderSystem();
        system.Add(new Order(101, "Иванов", 1500));
        system.Add(new Order(102, "Петрова", 820));
        system.Add(new Order(103, "Сидоров", 3200));
        system.Add(new Order(102, "Козлова", 500)); // дубликат номера
        system.Print();

        Console.WriteLine();
        system.ProcessNext();
        system.ProcessNext();
        system.Print();

        Console.WriteLine();
        system.UndoLast();
        system.Print();

        Console.WriteLine();
        system.Find(103);
        system.Find(999);
    }
}