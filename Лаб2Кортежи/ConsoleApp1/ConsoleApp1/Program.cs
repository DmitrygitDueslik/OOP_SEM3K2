using System;
using System.Runtime.Serialization;

struct PStack
{
    public string Name;
    public int Age;
}
class Heap
{
    public string Name;
    public int Age;
}
class Program
{
    static void Main()
    {
        PStack p1= new PStack();
        p1.Name = "Dasha";
        p1.Age = 20;

        PStack p2 = p1;
        p2.Age = 99; 

        Console.WriteLine($"Стек: p1.Age = {p1.Age}"); // Выведет 20 
        Console.WriteLine($"Стек: p2.Age = {p2.Age}"); // Выведет 99

        Heap c1=new Heap();
        c1.Name = "Bob";
        c1.Age = 30;

        Heap c2 = c1;
        c2.Age = 99; 

        Console.WriteLine($"Куча: c1.Age = {c1.Age}"); //99
        Console.WriteLine($"Куча: c2.Age = {c2.Age}"); //99
    }
}