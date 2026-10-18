using System;

// Определяем интерфейс IAnimal
// Интерфейс содержит только объявления методов без реализации
interface IAnimal
{
    // Метод для издания звука - только объявление, без тела
    void MakeSound();

    // Свойство - тоже только объявление
    string Name { get; set; }
}

// Класс Dog реализует интерфейс IAnimal
class Dog : IAnimal
{
    // Реализация свойства Name из интерфейса
    public string Name { get; set; }

    // Конструктор класса Dog
    public Dog(string name)
    {
        Name = name; // Сохраняем имя собаки
    }

    // Реализация метода MakeSound из интерфейса
    public void MakeSound()
    {
        // Для собаки звук - лай
        Console.WriteLine($"{Name} говорит: Гав-гав!");
    }
}

// Класс Cat реализует интерфейс IAnimal
class Cat : IAnimal
{
    // Реализация свойства Name из интерфейса
    public string Name { get; set; }

    // Конструктор класса Cat
    public Cat(string name)
    {
        Name = name; // Сохраняем имя кошки
    }

    // Реализация метода MakeSound из интерфейса
    public void MakeSound()
    {
        // Для кошки звук - мяуканье
        Console.WriteLine($"{Name} говорит: Мяу-мяу!");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Интерфейс IAnimal\n");

        // Создаем объекты классов, реализующих интерфейс IAnimal
        Dog dog = new Dog("Бобик");
        Cat cat = new Cat("Мурка");

        // Вызываем метод MakeSound напрямую через объекты
        Console.WriteLine("Вызов методов напрямую:");
        dog.MakeSound();
        cat.MakeSound();

        Console.WriteLine("\nРабота через интерфейс:");

        // Создаем массив интерфейсного типа
        // В этом массиве могут храниться любые объекты, реализующие IAnimal
        IAnimal[] animals = new IAnimal[] { dog, cat };

        // Проходим по массиву и вызываем метод MakeSound
        // Благодаря полиморфизму вызывается правильная реализация для каждого объекта
        foreach (IAnimal animal in animals)
        {
            animal.MakeSound();
        }
    }
}
