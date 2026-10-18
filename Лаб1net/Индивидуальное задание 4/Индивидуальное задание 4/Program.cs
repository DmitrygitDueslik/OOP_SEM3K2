using System;
class Program
{
    static void ChangeAll(int a,object b,ref int c)
    {
        Console.WriteLine($"Внутри метода ChangeAll:");
        a = a + 100;
        Console.WriteLine($"После изменения a: a = {a} ");
        b = 999;//Указывает на новый объект, исходный объект не изменяется
        //в main() b ссылается на исходный объект(массив), а не на новый объект 999
        Console.WriteLine($" b переприсвоен на новый box: b = {b} (на исходный объект не влияет)");
        c = c * 10;
        Console.WriteLine($" c внутри метода(ref): c = {c}");
    }

    static void MutateArrayContent(object b)
    {
        if(b is int[] arr)
        {
            arr[0] = 777;
        }
    }

    static void Main()
    {
        int x = 5;
        object y=new int[] { 1, 2, 3 };
        int z = 7;
        Console.WriteLine($"\nДо вызова метода ChangeAll: x = {x}, y =[{string.Join(", ", (int[])y)}], z = {z}");
        MutateArrayContent(y);
        Console.WriteLine($"\nПосле вызова метода MutateArrayContent(Меняли содержимое объекта: x = {x}, y =[{string.Join(", ", (int[])y)}], z = {z}");
        ChangeAll(x,y,ref z);

        Console.WriteLine("\nПосле вызова метода  ChangeAll):");
        Console.WriteLine($"x={x}(Не изменилось, т.к. передавалось по значению метод менял только копию)");
        Console.WriteLine($"y=[{string.Join(", ", (int[])y)}] Не изменилось этим вызовом-внутри метода "+
            "переприсвоили саму ссылку b, а не исходный объект)");
        Console.WriteLine($"z={z}(Изменилось, т.к. передавалось по ссылке)");
    }
}