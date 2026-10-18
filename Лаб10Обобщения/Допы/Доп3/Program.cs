using System;

public enum SortAlgorithm
{ Bubble, Selection, Insertion, Quick }

// Сортировщик для любого сравнимого типа
public class Sorter<T> where T : IComparable<T>
{
    public long Comparisons { get; private set; }
    public long Swaps { get; private set; } // обмены (для вставок — сдвиги элементов)

    // Сортирует копию массива выбранным алгоритмом и возвращает её
    public T[] Sort(T[] array, SortAlgorithm algorithm)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        T[] a = (T[])array.Clone(); // исходный массив не меняем
        Comparisons = 0;
        Swaps = 0;

        switch (algorithm)
        {
            case SortAlgorithm.Bubble: BubbleSort(a); break;
            case SortAlgorithm.Selection: SelectionSort(a); break;
            case SortAlgorithm.Insertion: InsertionSort(a); break;
            case SortAlgorithm.Quick: QuickSort(a, 0, a.Length - 1); break;
            default: throw new ArgumentOutOfRangeException(nameof(algorithm));
        }
        return a;
    }

    private bool Greater(T x, T y)
    {
        Comparisons++;
        return x.CompareTo(y) > 0;
    }

    private void Swap(T[] a, int i, int j)
    {
        Swaps++;
        (a[i], a[j]) = (a[j], a[i]);
    }

    public void BubbleSort(T[] a)
    {
        for (int i = 0; i < a.Length - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < a.Length - 1 - i; j++)
            {
                if (Greater(a[j], a[j + 1]))
                {
                    Swap(a, j, j + 1);
                    swapped = true;
                }
            }
            if (!swapped) break; // массив уже упорядочен
        }
    }

    public void SelectionSort(T[] a)
    {
        for (int i = 0; i < a.Length - 1; i++)
        {
            int min = i;
            for (int j = i + 1; j < a.Length; j++)
            {
                if (Greater(a[min], a[j])) min = j;
            }
            if (min != i) Swap(a, i, min);
        }
    }

    public void InsertionSort(T[] a)
    {
        for (int i = 1; i < a.Length; i++)
        {
            T key = a[i];
            int j = i - 1;
            while (j >= 0 && Greater(a[j], key))
            {
                a[j + 1] = a[j]; // сдвиг вправо
                Swaps++;
                j--;
            }
            a[j + 1] = key;
        }
    }

    // Быстрая сортировка (разбиение Ломуто, опорный — последний элемент)
    public void QuickSort(T[] a, int low, int high)
    {
        if (low >= high) return;
        T pivot = a[high];
        int i = low - 1;
        for (int j = low; j < high; j++)
        {
            if (!Greater(a[j], pivot)) // a[j] <= pivot
            {
                i++;
                if (i != j) Swap(a, i, j);
            }
        }
        if (i + 1 != high) Swap(a, i + 1, high);
        QuickSort(a, low, i);
        QuickSort(a, i + 2, high);
    }
}

internal class Program
{
    private static void Test<T>(string title, T[] data) where T : IComparable<T>
    {
        Console.WriteLine($"=== {title}: [{string.Join("; ", data)}] ===");
        Sorter<T> sorter = new Sorter<T>();
        Console.WriteLine($"{"Алгоритм",-10} {"Сравнений",10} {"Обменов",8}   Результат");
        foreach (SortAlgorithm alg in Enum.GetValues<SortAlgorithm>())
        {
            T[] sorted = sorter.Sort(data, alg);
            Console.WriteLine($"{alg,-10} {sorter.Comparisons,10} {sorter.Swaps,8}   [{string.Join("; ", sorted)}]");
        }
        Console.WriteLine();
    }

    private static void Main()
    {
        Test("int, случайный порядок", new[] { 29, 3, 72, 15, 8, 41, 64, 1, 50, 36 });
        Test("int, уже отсортирован", new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
        Test("double, обратный порядок", new[] { 9.5, 7.25, 5.0, 3.75, 1.5 });
        Test("string", new[] { "груша", "яблоко", "апельсин", "банан", "киви" });
    }
}