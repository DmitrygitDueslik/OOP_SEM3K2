using System;

class Circle
{
    public double radius;

    public double GetDiameter()
    {
        return radius * 2;
    }

    public double GetCircumference()
    {
        return 2 * Math.PI * radius;
    }

    public double GetArea()
    {
        return Math.PI * radius * radius;
    }
}

class Program
{
    static void Main()
    {
        Circle circle = new Circle();
        circle.radius = 5;

        Console.WriteLine($"Радиус окружности: {circle.radius}");
        Console.WriteLine($"Диаметр: {circle.GetDiameter()}");
        Console.WriteLine($"Длина окружности: {circle.GetCircumference():F2}");
        Console.WriteLine($"Площадь: {circle.GetArea():F2}");
    }
}
