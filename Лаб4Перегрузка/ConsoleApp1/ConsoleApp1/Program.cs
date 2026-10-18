using System;

public class MyClass
{
    public int A;
    public int B;

    public MyClass(int a,int b)
    {
        A = a;

        B = b;
    }
    //Конструктор
}

public class MyClass2
{
    public int A2;
    public int B2;
    //по умолчанию 
}

public class MyClass3
{
    public int A3;
    public int B3;

    private MyClass3(int a,int b)
    {
        A3 = a;
        B3 = b;
    }
    public static MyClass3 Create(int a,int b)
    {
        return new MyClass3(a,b);
    }
}

class Program
{
    static void Main()
    {
        //конструторк
        MyClass obj1 = new MyClass(10, 20);
        Console.WriteLine($"MyClass: A={obj1.A}, B={obj1.B}");


        //инициализатор
        MyClass2 obj2 = new MyClass2
        {
            A2 = 10,
            B2 = 20
        };
        Console.WriteLine($"MyClass2: A2={obj2.A2}, B2={obj2.B2}");
        //фабричный(внутри без new)
        MyClass3 obj3 = MyClass3.Create(10, 20);
        Console.WriteLine($"MyClass3: A3={obj3.A3}, B3={obj3.B3}");

    }
}