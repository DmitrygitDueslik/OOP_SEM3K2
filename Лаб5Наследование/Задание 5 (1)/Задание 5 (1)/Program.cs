using System;

class Shape
{
    public string Color { get; set; }
    public Shape(string color)
    {
        Color = color;
    }

    public virtual double GetArea()
    {
        return 0;
    }

    public virtual double GetPerimeter()
    {
        return 0;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Фигура цвета {Color}: площадь = {GetArea():F2}, периметр = {GetPerimeter():F2}");
    }

}

class Circle : Shape
{
    public double Radius { get;set; }
    public Circle(string color,double radius):base(color)
    {
        Radius = radius;
    }

    public override double GetArea()
    {
        return Math.PI * Radius * Radius;
    }
    public override double GetPerimeter()
    {
        return 2 * Math.PI * Radius;
    }
}


class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(string color,double width,double height) : base(color)
    {
        Height = height;
        Width = width;
    }
    public override double GetArea()
    {
        return Height * Width;
    }
    public override double GetPerimeter()
    {
        return 2 * (Height + Width);
    }
    public bool IsSquare()
    {
        return Math.Abs(Width - Height) < 0.0001;
    }
}

class Triangle : Shape
{
    public double SideA { get; set; }
    public double SideB { get; set; }
    public double SideC { get; set; }

    public Triangle(string color,double a,double b,double c) : base(color)
    {
        SideA = a;
        SideB = b;
        SideC = c;
    }

    public override double GetPerimeter()
    {
        return SideC + SideB + SideA;
    }
    public override double GetArea()
    {
        double s = GetPerimeter() / 2;
        return Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
    }
    
    // Проверка: является ли треугольник прямоугольным (теорема Пифагора)
    public bool IsRightTriangle()
    {
        double[] sides = { SideA, SideB, SideC };
        Array.Sort(sides);
        double a = sides[0], b = sides[1], c = sides[2];
        return Math.Abs(a * a + b * b - c * c) < 0.0001;
    }

}


class Program
{
    static void Main()
    {
        Shape[] shapes = new Shape[]
        {
            new Circle("красный",5),
            new Rectangle("синий",4,4),
            new Triangle("зелёный",3,4,5)
        };

        foreach(Shape shape in shapes)
        {
            shape.DisplayInfo();
        }
        Rectangle rect = (Rectangle)shapes[1];
        Console.WriteLine($"\nПрямоугольник - квадрат: {rect.IsSquare()}");

        Triangle tri = (Triangle)shapes[2];
        Console.WriteLine($"Треугольник - прямоугольный: {tri.IsRightTriangle()}");

    }
}