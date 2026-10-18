using System;

// Базовый класс
class Vehicle
{
    public string Brand { get; set; }
    public double Speed { get; set; } // км/ч

    public Vehicle(string brand, double speed)
    {
        Brand = brand;
        Speed = speed;
    }

    // Время в пути в часах (виртуальный метод)
    public virtual double GetTravelTime(double distance, int hour)
    {
        return distance / Speed;
    }
}

class Car : Vehicle
{
    public Car(string brand, double speed) : base(brand, speed) { }

    public override double GetTravelTime(double distance, int hour)
    {
        double speed = Speed;
        // Час пик: скорость падает на 30%
        bool isRushHour = (hour >= 8 && hour < 10) || (hour >= 18 && hour < 20);
        if (isRushHour)
            speed *= 0.7;
        return distance / speed;
    }
}

class Bicycle : Vehicle
{
    public Bicycle(string brand, double speed) : base(brand, speed) { }

    public override double GetTravelTime(double distance, int hour)
    {
        // Скорость падает на 10% после каждых полных 10 км пути
        int tenKmSegments = (int)(distance / 10);
        double speed = Speed * Math.Pow(0.9, tenKmSegments);
        return distance / speed;
    }
}

class PublicTransport : Vehicle
{
    public PublicTransport(string brand, double speed) : base(brand, speed) { }

    public override double GetTravelTime(double distance, int hour)
    {
        if (hour < 6 || hour >= 23)
            throw new InvalidOperationException($"не ходит в {hour}:00 - время вне графика (6:00-23:00)");

        double waitingHours = 10.0 / 60.0; // 10 минут ожидания
        return distance / Speed + waitingHours;
    }
}

class Program
{
    static void Main()
    {
        Vehicle[] vehicles = new Vehicle[]
        {
            new Car("Toyota", 60),
            new Bicycle("Stels", 20),
            new PublicTransport("Автобус №12", 40)
        };

        double distance = 15; // км

        foreach (int hour in new[] { 9, 2 })
        {
            Console.WriteLine($"Расстояние: {distance} км, время суток: {hour}:00");
            foreach (Vehicle v in vehicles)
            {
                try
                {
                    double time = v.GetTravelTime(distance, hour);
                    Console.WriteLine($"  {v.Brand}: время в пути = {time:F2} ч");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"  {v.Brand}: {ex.Message}");
                }
            }
            Console.WriteLine();
        }
    }
}
