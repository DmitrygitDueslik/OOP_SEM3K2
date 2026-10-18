using System;

// Базовый класс
class Vehicle
{
    public string Brand { get; set; }
    public int Year { get; set; }

    public Vehicle(string brand, int year)
    {
        Brand = brand;
        Year = year;
    }

    public void Start()
    {
        Console.WriteLine($"{Brand} запущен");
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Транспортное средство: {Brand}, {Year} год");
    }
}

// Производный класс
class Car : Vehicle
{
    public int DoorsCount { get; set; }

    public Car(string brand, int year, int doorsCount)
        : base(brand, year) // вызов конструктора базового класса
    {
        DoorsCount = doorsCount;
    }

    public void Honk()
    {
        Console.WriteLine($"{Brand} сигналит: Бип-бип!");
    }

    // Скрытие метода DisplayInfo (не переопределение!)
    public new void DisplayInfo()
    {
        base.DisplayInfo(); // вызываем метод базового класса
        Console.WriteLine($"Количество дверей: {DoorsCount}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Создание объекта базового класса:");
        Vehicle vehicle = new Vehicle("Абстрактный транспорт", 2020);
        vehicle.Start();
        vehicle.DisplayInfo();

        Console.WriteLine("\nСоздание объекта производного класса:");
        Car car = new Car("Toyota", 2022, 4);
        car.Start(); // унаследованный метод
        car.Honk();  // собственный метод
        car.DisplayInfo(); // скрытый метод
    }
}
