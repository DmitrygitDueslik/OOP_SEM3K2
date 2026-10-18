using System;

class Program
{
    // 1. Передача типа значения (int) по значению
    static void ModifyValue(int x)
    {
        x = x * 2;
        Console.WriteLine($"Внутри ModifyValue (по значению): x = {x}");
    }

    // 2. Передача типа значения (int) по ссылке (ref)
    static void ModifyValueRef(ref int x)
    {
        x = x * 2;
        Console.WriteLine($"Внутри ModifyValueRef (ref): x = {x}");
    }

    // 3. Передача ссылочного типа (string) по значению
    static void ModifyString(string s)
    {
        s = s + " (изменено)";
        Console.WriteLine($"Внутри ModifyString (по значению): s = \"{s}\"");
    }

    // 4. Передача ссылочного типа (string) по ссылке (ref)
    static void ModifyStringRef(ref string s)
    {
        s = s + " (изменено)";
        Console.WriteLine($"Внутри ModifyStringRef (ref): s = \"{s}\"");
    }

    // 5. Метод с out параметром
    static void GetValues(out int min, out int max)
    {
        // out параметры ОБЯЗАНЫ быть присвоены до выхода из метода
        min = 1;
        max = 100;
        // Можно также выполнить какие-то вычисления
    }

    static void Main()
    {
        Console.WriteLine("Передача параметров\n");

        // Случай 1: int по значению
        int num = 5;
        Console.WriteLine($"До вызова ModifyValue: num = {num}");
        ModifyValue(num);
        Console.WriteLine($"После вызова ModifyValue: num = {num}\n");

        // Случай 2: int по ссылке (ref)
        int numRef = 5;
        Console.WriteLine($"До вызова ModifyValueRef: numRef = {numRef}");
        ModifyValueRef(ref numRef);
        Console.WriteLine($"После вызова ModifyValueRef: numRef = {numRef}\n");

        // Случай 3: string по значению
        string text = "Привет";
        Console.WriteLine($"До вызова ModifyString: text = \"{text}\"");
        ModifyString(text);
        Console.WriteLine($"После вызова ModifyString: text = \"{text}\"\n");

        // Случай 4: string по ссылке (ref)
        string textRef = "Привет";
        Console.WriteLine($"До вызова ModifyStringRef: textRef = \"{textRef}\"");
        ModifyStringRef(ref textRef);
        Console.WriteLine($"После вызова ModifyStringRef: textRef = \"{textRef}\"\n");

        // Случай 5: использование out
        GetValues(out int minimum, out int maximum);
        Console.WriteLine($"После вызова GetValues: min = {minimum}, max = {maximum}");
    }
}
