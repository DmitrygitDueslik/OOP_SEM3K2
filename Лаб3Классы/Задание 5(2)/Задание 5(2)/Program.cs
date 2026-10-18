using System;

class Circle
{
    private double radius;

    public double Radius
    {
        get { return radius; }
        set { radius = value; }
    }

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

    // Статический метод: сравнивает площади двух окружностей
    public static string CompareArea(Circle c1, Circle c2)
    {
        if (c1.GetArea() > c2.GetArea())
            return "Первая окружность";
        else if (c2.GetArea() > c1.GetArea())
            return "Вторая окружность";
        else
            return "Окружности равны по площади";
    }
}

class Program
{
    static void Main()
    {
        Circle c1 = new Circle();
        c1.Radius = 5;

        Circle c2 = new Circle();
        c2.Radius = 8;

        Console.WriteLine($"Первая окружность: радиус = {c1.Radius}, площадь = {c1.GetArea():F2}");
        Console.WriteLine($"Вторая окружность: радиус = {c2.Radius}, площадь = {c2.GetArea():F2}");

        string result = Circle.CompareArea(c1, c2);
        Console.WriteLine($"\nБольшая по площади: {result}");
    }
}
