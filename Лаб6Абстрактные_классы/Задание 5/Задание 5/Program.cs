using System;
using System.Transactions;

public abstract class Transport
{
    private double _capacity;//грузоподьемность(инкапсул)
    public double Capacity
    {
        get { return _capacity; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Грузоподъемность долна быть положительной");
            _capacity = value;
        }
    }
    public double Speed { get; set; }
    public double DeliveryCost { get; set; }
    
    public double CurrentLoad { get; set; }//Текущий груз
    protected Transport(double capacity,double speed,double deliveryCost)
    {
        Capacity = capacity;
        Speed = speed;
        DeliveryCost = deliveryCost;
    }

    public void LoadCargo(double weight)
    {
        if (weight <= 0)
        {
            throw new ArgumentException("Вес груза должен быть положительным");
        }
        if (CurrentLoad + weight > Capacity)
        {
            throw new InvalidOperationException (
                                $"Превышена грузоподъемность: {CurrentLoad} т + {weight} т > {Capacity} т");
        }
        CurrentLoad += weight;
    }
    public void UnloadCargo()
    {
        CurrentLoad = 0;
    }

    // Абстрактный метод - у каждого вида транспорта своя формула расчёта времени
    public abstract double CalculateDeliveryTime(double distance);

    public void DisplayInfo(double distance)
    {
        Console.WriteLine(
            $"{GetType().Name}: груз {CurrentLoad:F0} т из {Capacity:F0} т, " +
            $"время доставки на {distance:F0} км = {CalculateDeliveryTime(distance):F2} ч, " +
            $"стоимость = {distance * DeliveryCost:F2} руб.");
    }
}

public class Truck : Transport
{
    public Truck(double capacity,double speed,double deliveryCost) : base(capacity, speed, deliveryCost) { }

    public override double CalculateDeliveryTime(double distance)
    {
        double loadRatio = CurrentLoad / Capacity;
        double effectiveSpeed = loadRatio > 0.8 ? Speed * 0.85 : Speed;
        return distance / effectiveSpeed;
    }

}

public class Ship : Transport
{
    private const double PortDelayHours = 3;

    public Ship(double capacity, double speed, double deliveryCost) :
        base(capacity, speed, deliveryCost)
    { }
    public override double CalculateDeliveryTime(double distance)
    {
        return distance / Speed + PortDelayHours;
    }
}

public class Airplane : Transport
{
    private const double CheckInHours = 1.5;

    public Airplane(double capacity,double speed,double deliveryCost)
        : base(capacity, speed, deliveryCost) { }

    public override double CalculateDeliveryTime(double distance)
    {
        return distance / Speed * CheckInHours;
    }
}


class Program
{
    static void Main()
    {
        Truck truck = new Truck(capacity: 10, speed: 80, deliveryCost: 25);
        Ship ship = new Ship(capacity: 5000, speed: 35, deliveryCost: 2);
        Airplane airplane = new Airplane(capacity: 20, speed: 800, deliveryCost: 150);

        truck.LoadCargo(9);      // 90% загрузки - скорость упадёт
        ship.LoadCargo(3000);
        airplane.LoadCargo(15);

        double distance = 520;

        Console.WriteLine($"Расчёт доставки на {distance} км:\n");
        truck.DisplayInfo(distance);
        ship.DisplayInfo(distance);
        airplane.DisplayInfo(distance);
        Console.WriteLine("\nПопытка перегрузить грузовик:");
        try
        {
            truck.LoadCargo(5); // 9+5=14 > 10 - исключение
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

    }
}