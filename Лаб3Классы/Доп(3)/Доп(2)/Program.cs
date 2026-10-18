using System;

class Order
{
    public int orderId;
    public string customerName;
    public double amount;

    // 1. Конструктор без параметров
    public Order()
    {
        customerName = "Неизвестно";
        amount = 0;
        Console.WriteLine("Вызван конструктор без параметров");
    }

    // 2. Конструктор с customerName - вызывает первый через this()
    public Order(string customerName) : this()
    {
        this.customerName = customerName;
        Console.WriteLine("Вызван конструктор с customerName");
    }

    // 3. Конструктор с customerName и amount - вызывает второй через this()
    public Order(string customerName, double amount) : this(customerName)
    {
        this.amount = amount;
        Console.WriteLine("Вызван конструктор с customerName и amount");
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Заказ: клиент = {customerName}, сумма = {amount}\n");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("--- Order 1 (без параметров) ---");
        Order order1 = new Order();
        order1.PrintInfo();

        Console.WriteLine("--- Order 2 (customerName) ---");
        Order order2 = new Order("Иван Иванов");
        order2.PrintInfo();

        Console.WriteLine("--- Order 3 (customerName, amount) ---");
        Order order3 = new Order("Мария Петрова", 1500.50);
        order3.PrintInfo();
    }
}
