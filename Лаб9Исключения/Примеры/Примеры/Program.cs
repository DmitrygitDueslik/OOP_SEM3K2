using System;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Обработка исключений при делении\n");

        // Флаг для повторения ввода при ошибке
        bool success = false;

        // Цикл продолжается, пока пользователь не введет корректные данные
        while (!success)
        {
            try
            {
                // Блок try содержит код, который может вызвать исключение
                Console.Write("Введите первое число: ");
                string input1 = Console.ReadLine();
                int a = int.Parse(input1); // Может вызвать FormatException

                Console.Write("Введите второе число: ");
                string input2 = Console.ReadLine();
                int b = int.Parse(input2); // Может вызвать FormatException

                // Попытка деления
                int result = a / b; // Может вызвать DivideByZeroException

                // Если дошли до этой строки - исключений не было
                Console.WriteLine($"Результат деления: {a} / {b} = {result}");
                success = true; // Выходим из цикла
            }
            catch (DivideByZeroException ex)
            {
                // Обработка деления на ноль
                Console.WriteLine($"Ошибка: деление на ноль! {ex.Message}");
                Console.WriteLine("Попробуйте снова.\n");
            }
            catch (FormatException ex)
            {
                // Обработка неверного формата числа
                Console.WriteLine($"Ошибка: неверный формат числа! {ex.Message}");
                Console.WriteLine("Попробуйте снова.\n");
            }
            catch (Exception ex)
            {
                // Обработка любых других исключений
                Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
                Console.WriteLine("Попробуйте снова.\n");
            }
            finally
            {
                // Этот блок выполняется всегда, независимо от наличия исключения
                Console.WriteLine("Блок finally выполнен (очистка ресурсов)");
            }
        }

        Console.WriteLine("\nПрограмма завершена успешно.");
    }
}