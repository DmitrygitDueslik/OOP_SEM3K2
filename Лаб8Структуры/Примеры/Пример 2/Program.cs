using System;

// Перечисление дней недели
public enum DaysOfWeek
{
    Monday = 1,      // Понедельник - 1
    Tuesday = 2,     // Вторник - 2
    Wednesday = 3,   // Среда - 3
    Thursday = 4,    // Четверг - 4
    Friday = 5,      // Пятница - 5
    Saturday = 6,    // Суббота - 6
    Sunday = 7       // Воскресенье - 7
}

// Расширение для перечисления (методы расширения)
public static class DaysOfWeekExtensions
{
    public static bool IsWeekend(this DaysOfWeek day)
    {
        return day == DaysOfWeek.Saturday || day == DaysOfWeek.Sunday;
    }

    public static string GetRussianName(this DaysOfWeek day)
    {
        switch (day)
        {
            case DaysOfWeek.Monday: return "Понедельник";
            case DaysOfWeek.Tuesday: return "Вторник";
            case DaysOfWeek.Wednesday: return "Среда";
            case DaysOfWeek.Thursday: return "Четверг";
            case DaysOfWeek.Friday: return "Пятница";
            case DaysOfWeek.Saturday: return "Суббота";
            case DaysOfWeek.Sunday: return "Воскресенье";
            default: return "Неизвестный день";
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Работа с перечислением DaysOfWeek ===\n");

        // Перебор всех значений перечисления
        Console.WriteLine("Все дни недели:");
        foreach (DaysOfWeek day in Enum.GetValues(typeof(DaysOfWeek)))
        {
            int dayNumber = (int)day;
            string russianName = day.GetRussianName();
            string weekend = day.IsWeekend() ? "Выходной" : "Рабочий день";

            Console.WriteLine($"{dayNumber}. {russianName} ({day}) - {weekend}");
        }

        Console.WriteLine("\n=== Проверка конкретных дней ===\n");

        // Проверка конкретных дней
        CheckDay(DaysOfWeek.Monday);
        CheckDay(DaysOfWeek.Saturday);

        Console.WriteLine("\n=== Получение дня по номеру ===\n");

        // Получение дня по числовому значению
        int dayNumber1 = 3;
        DaysOfWeek dayFromNumber = (DaysOfWeek)dayNumber1;
        Console.WriteLine($"День с номером {dayNumber1}: {dayFromNumber.GetRussianName()}");

        // Парсинг строки в перечисление
        string dayString = "Friday";
        if (Enum.TryParse(dayString, out DaysOfWeek parsedDay))
        {
            Console.WriteLine($"День из строки '{dayString}': {parsedDay.GetRussianName()}");
        }

        Console.WriteLine("\n=== Информация о перечислении ===\n");

        // Получение информации о перечислении
        Type enumType = typeof(DaysOfWeek);
        Console.WriteLine($"Имя перечисления: {enumType.Name}");
        Console.WriteLine($"Базовый тип: {Enum.GetUnderlyingType(enumType)}");

        // Получение всех имен и значений
        string[] names = Enum.GetNames(enumType);
        Array values = Enum.GetValues(enumType);

        Console.WriteLine("\nДни недели (имя = значение):");
        for (int i = 0; i < names.Length; i++)
        {
            Console.WriteLine($"{names[i]} = {(int)values.GetValue(i)}");
        }
    }

    static void CheckDay(DaysOfWeek day)
    {
        string russianName = day.GetRussianName();
        string dayType = day.IsWeekend() ? "выходной" : "рабочий";
        Console.WriteLine($"{russianName} ({day}) - это {dayType} день");
    }
}
