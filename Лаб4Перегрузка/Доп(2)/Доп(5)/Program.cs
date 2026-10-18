using System;
using System.Collections.Generic;

public class HistoryList
{
    private List<int> items = new List<int>();
    private const int MaxHistory = 10;

    // История операций для undo/redo: '+' - было добавление, '-' - было удаление
    public Stack<(char op, int value)> UndoHistory { get; } = new Stack<(char, int)>();
    public Stack<(char op, int value)> RedoHistory { get; } = new Stack<(char, int)>();

    public int this[int index] => items[index];

    private void PushUndo(char op, int value)
    {
        UndoHistory.Push((op, value));
        if (UndoHistory.Count > MaxHistory)
        {
            var temp = new Stack<(char, int)>(UndoHistory);
            UndoHistory.Clear();
            while (temp.Count > 1)
                UndoHistory.Push(temp.Pop());
        }
        RedoHistory.Clear(); // новое действие "затирает" возможность повтора старых
    }

    // Внутренние операции - используются и операторами, и Undo/Redo
    public void AddInternal(int value, bool recordHistory)
    {
        items.Add(value);
        if (recordHistory) PushUndo('+', value);
    }

    public void RemoveLastInternal(bool recordHistory)
    {
        if (items.Count == 0) return;
        int removed = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);
        if (recordHistory) PushUndo('-', removed);
    }

    // Оператор + - добавление элемента
    public static HistoryList operator +(HistoryList list, int value)
    {
        list.AddInternal(value, true);
        return list;
    }

    // Унарный оператор - - удаление последнего элемента
    public static HistoryList operator -(HistoryList list)
    {
        list.RemoveLastInternal(true);
        return list;
    }

    public override string ToString()
    {
        return "[" + string.Join(", ", items) + "]";
    }
}

public static class HistoryListExtensions
{
    // Отмена последней операции
    public static void Undo(this HistoryList list)
    {
        if (list.UndoHistory.Count == 0)
        {
            Console.WriteLine("Нечего отменять");
            return;
        }

        var (op, value) = list.UndoHistory.Pop();

        if (op == '+')
            list.RemoveLastInternal(false); // отменяем добавление - удаляем элемент
        else
            list.AddInternal(value, false); // отменяем удаление - восстанавливаем элемент

        list.RedoHistory.Push((op, value));
    }

    // Повтор последней отменённой операции
    public static void Redo(this HistoryList list)
    {
        if (list.RedoHistory.Count == 0)
        {
            Console.WriteLine("Нечего повторять");
            return;
        }

        var (op, value) = list.RedoHistory.Pop();

        if (op == '+')
            list.AddInternal(value, false);
        else
            list.RemoveLastInternal(false);

        list.UndoHistory.Push((op, value));
    }
}

class Program
{
    static void Main()
    {
        HistoryList history = new HistoryList();

        history = history + 10;
        history = history + 20;
        history = history + 30;
        Console.WriteLine($"После трёх добавлений: {history}");

        history = -history; // удаляем последний элемент
        Console.WriteLine($"После удаления последнего: {history}");

        Console.WriteLine("\nОтменяем последнюю операцию (удаление 30 -> должно вернуться)");
        history.Undo();
        Console.WriteLine($"После Undo(): {history}");

        Console.WriteLine("\nОтменяем ещё раз (добавление 30 -> должно удалиться)");
        history.Undo();
        Console.WriteLine($"После Undo(): {history}");

        Console.WriteLine("\nПовторяем отменённую операцию (Redo)");
        history.Redo();
        Console.WriteLine($"После Redo(): {history}");

        Console.WriteLine($"\nЭлемент по индексу 0: {history[0]}");
    }
}
