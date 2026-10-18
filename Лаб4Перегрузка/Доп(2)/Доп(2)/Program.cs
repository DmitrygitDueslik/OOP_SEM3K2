using System;

public class CyclicArray
{
    private int[] items;

    public CyclicArray(int[] values)
    {
        items = (int[])values.Clone();
    }

    // Индексатор с поддержкой любых (в т.ч. отрицательных) индексов благодаря зацикленности
    public int this[int index]
    {
        get
        {
            int realIndex = ((index % items.Length) + items.Length) % items.Length;
            return items[realIndex];
        }
        set
        {
            int realIndex = ((index % items.Length) + items.Length) % items.Length;
            items[realIndex] = value;
        }
    }

    // Сдвиг всех элементов на одну позицию вправо (последний становится первым)
    public static CyclicArray operator ++(CyclicArray arr)
    {
        int n = arr.items.Length;
        int[] shifted = new int[n];
        shifted[0] = arr.items[n - 1];
        for (int i = 1; i < n; i++)
            shifted[i] = arr.items[i - 1];

        arr.items = shifted;
        return arr;
    }

    // Сдвиг всех элементов на одну позицию влево (первый становится последним)
    public static CyclicArray operator --(CyclicArray arr)
    {
        int n = arr.items.Length;
        int[] shifted = new int[n];
        for (int i = 0; i < n - 1; i++)
            shifted[i] = arr.items[i + 1];
        shifted[n - 1] = arr.items[0];

        arr.items = shifted;
        return arr;
    }

    public override string ToString()
    {
        return "[" + string.Join(", ", items) + "]";
    }
}

class Program
{
    static void Main()
    {
        CyclicArray arr = new CyclicArray(new int[] { 10, 20, 30, 40, 50 });

        Console.WriteLine($"Исходный массив: {arr}");
        Console.WriteLine($"arr[-1] = {arr[-1]}  (последний элемент)");
        Console.WriteLine($"arr[7]  = {arr[7]}   (зацикливание вперед)");

        arr++;
        Console.WriteLine($"\nПосле arr++ (сдвиг вправо): {arr}");

        arr--;
        arr--;
        Console.WriteLine($"После двух arr-- (сдвиг влево): {arr}");
    }
}
