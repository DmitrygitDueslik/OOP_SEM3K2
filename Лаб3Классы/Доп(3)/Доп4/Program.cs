using System;

class Engine
{
    public int horsePower;
    public bool isRunning;

    public Engine(int horsePower)
    {
        this.horsePower = horsePower;
        this.isRunning = false;
    }

    public void Start()
    {
        isRunning = true;
        Console.WriteLine($"Двигатель ({horsePower} л.с.) запущен");
    }
}

class Car
{
    public string model;
    public Engine engine; // поле-объект другого класса

    public Car(string model, Engine engine)
    {
        this.model = model;
        this.engine = engine;
    }

    public void StartCar()
    {
        Console.WriteLine($"Запуск автомобиля {model}...");
        engine.Start(); // вызов метод объекта, хранящегося в поле engine
    }
}

class Program
{
    static void Main()
    {
        Engine engine = new Engine(150);
        Car car = new Car("Toyota Camry", engine);

        car.StartCar();

        Console.WriteLine($"\nСостояние двигателя: isRunning = {engine.isRunning}");
    }
}
