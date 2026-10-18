using System;
using System.Collections.Generic;

// Три независимых «умения»
interface IFlyable
{
    void Fly();
}

interface ISwimmable
{
    void Swim();
}

interface IRunnable
{
    void Run();
}

// Общий базовый класс животных (только имя)
abstract class Animal
{
    public string Name { get; }
    protected Animal(string name) { Name = name; }
}

// Утка умеет всё
class Duck : Animal, IFlyable, ISwimmable, IRunnable
{
    public Duck(string name) : base(name) { }
    public void Fly() => Console.WriteLine($"  {Name} летит над озером");
    public void Swim() => Console.WriteLine($"  {Name} плавает у берега");
    public void Run() => Console.WriteLine($"  {Name} бежит вперевалку");
}

// Рыба умеет только плавать
class Fish : Animal, ISwimmable
{
    public Fish(string name) : base(name) { }
    public void Swim() => Console.WriteLine($"  {Name} плывёт в глубине");
}

// Собака бегает и плавает
class Dog : Animal, IRunnable, ISwimmable
{
    public Dog(string name) : base(name) { }
    public void Run() => Console.WriteLine($"  {Name} бежит за мячом");
    public void Swim() => Console.WriteLine($"  {Name} плывёт по-собачьи");
}

class Program
{
    static void Main()
    {
        List<Animal> animals = new List<Animal>
        {
            new Duck("Утка Дональд"),
            new Fish("Карась"),
            new Dog("Шарик"),
            new Duck("Утка Серая"),
            new Fish("Щука")
        };

        int flyers = 0, swimmers = 0, runners = 0;

        foreach (Animal animal in animals)
        {
            Console.WriteLine($"{animal.Name} ({animal.GetType().Name}):");

            // as возвращает null, если объект не реализует интерфейс
            // (знак ? после типа означает, что переменная может содержать null)
            IFlyable? flyable = animal as IFlyable;
            if (flyable != null)
            {
                flyable.Fly();
                flyers++;
            }

            ISwimmable? swimmable = animal as ISwimmable;
            if (swimmable != null)
            {
                swimmable.Swim();
                swimmers++;
            }

            IRunnable? runnable = animal as IRunnable;
            if (runnable != null)
            {
                runnable.Run();
                runners++;
            }
        }

        Console.WriteLine("\n=== Статистика ===");
        Console.WriteLine($"Всего животных: {animals.Count}");
        Console.WriteLine($"Умеют летать:   {flyers}");
        Console.WriteLine($"Умеют плавать:  {swimmers}");
        Console.WriteLine($"Умеют бегать:   {runners}");
    }
}
