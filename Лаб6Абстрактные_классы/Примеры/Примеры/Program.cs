using System;

public abstract class Shape
{
    public string Name { get; set; }

    public Shape(string name)
    {
        Name = name;
    }

    // абстрактные методы (без реализации)
    public abstract double GetArea();
    public abstract double GetPerimeter();

    // обычный метод (с реализацией)
    public void DisplayInfo()
    {
        Console.WriteLine($"Фигура: {Name}");
        Console.WriteLine($"Площадь: {GetArea():F2}");
        Console.WriteLine($"Периметр: {GetPerimeter():F2}");
        Console.WriteLine();
    }
}

public class Circle : Shape
{
    private double _radius;  // инкапсулированное поле

    public double Radius
    {
        get { return _radius; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Радиус должен быть положительным");
            _radius = value;
        }
    }

    public Circle(double radius) : base("Круг")
    {
        Radius = radius;
    }

    // обязательное переопределение абстрактных методов
    public override double GetArea()
    {
        return Math.PI * _radius * _radius;
    }

    public override double GetPerimeter()
    {
        return 2 * Math.PI * _radius;
    }
}

public class Rectangle : Shape
{
    private double _width;
    private double _height;

    public double Width
    {
        get { return _width; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Ширина должна быть положительной");
            _width = value;
        }
    }

    public double Height
    {
        get { return _height; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Высота должна быть положительной");
            _height = value;
        }
    }

    public Rectangle(double width, double height) : base("Прямоугольник")
    {
        Width = width;
        Height = height;
    }

    public override double GetArea()
    {
        return _width * _height;
    }

    public override double GetPerimeter()
    {
        return 2 * (_width + _height);
    }
}

public class Triangle : Shape
{
    private double _sideA;
    private double _sideB;
    private double _sideC;

    public Triangle(double a, double b, double c) : base("Треугольник")
    {
        // проверка существования треугольника
        if (a + b <= c || a + c <= b || b + c <= a)
            throw new ArgumentException("Треугольник с такими сторонами не существует");

        _sideA = a;
        _sideB = b;
        _sideC = c;
    }

    public override double GetArea()
    {
        // формула Герона
        double p = (_sideA + _sideB + _sideC) / 2;
        return Math.Sqrt(p * (p - _sideA) * (p - _sideB) * (p - _sideC));
    }

    public override double GetPerimeter()
    {
        return _sideA + _sideB + _sideC;
    }
}

class Program
{
    static void Main()
    {
        // нельзя создать объект абстрактного класса:
        // Shape shape = new Shape("Фигура"); // Ошибка компиляции

        // создаем массив различных фигур
        Shape[] shapes = new Shape[]
        {
            new Circle(5),
            new Rectangle(4, 6),
            new Triangle(3, 4, 5),
            new Circle(3.5),
            new Rectangle(7, 2)
        };

        // полиморфизм: вызываем методы базового класса,
        // а выполняются реализации производных классов
        foreach (Shape shape in shapes)
        {
            shape.DisplayInfo();
        }

        // демонстрация инкапсуляции
        Circle circle = new Circle(10);
        // circle._radius = -5; // ошибка: поле private
        circle.Radius = 15;  // работа через свойство с проверкой
        Console.WriteLine($"Радиус круга: {circle.Radius}");
        // circle.Radius = -3; // выбросит исключение
    }
}
