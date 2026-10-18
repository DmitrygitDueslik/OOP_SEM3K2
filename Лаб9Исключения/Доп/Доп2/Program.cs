using System;
using System.Collections.Generic;
using System.Linq;

// Исключение, содержащее сразу все найденные ошибки
internal class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(List<string> errors)
        : base($"Данные не прошли проверку: ошибок - {errors.Count}")
    {
        Errors = errors;
    }
}

internal class User
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public int Age { get; set; }
    public string Phone { get; set; } = "";
}

internal class UserValidator
{
    // Проверяет ВСЕ поля и только в конце выбрасывает одно исключение со списком ошибок
    public void Validate(User user)
    {
        List<string> errors = new List<string>();

        // Имя
        if (string.IsNullOrWhiteSpace(user.Name))
            errors.Add("Имя: не заполнено");
        else if (!user.Name.All(c => char.IsLetter(c) || c == ' ' || c == '-'))
            errors.Add($"Имя: \"{user.Name}\" содержит недопустимые символы");

        // Email: одна @, есть точка после @
        int at = user.Email.IndexOf('@');
        if (at <= 0 || at != user.Email.LastIndexOf('@') || user.Email.IndexOf('.', at) < at + 2 || user.Email.EndsWith("."))
            errors.Add($"Email: \"{user.Email}\" имеет неверный формат");

        // Возраст
        if (user.Age < 14 || user.Age > 120)
            errors.Add($"Возраст: {user.Age} вне диапазона 14..120");

        // Телефон: +7XXXXXXXXXX или 8XXXXXXXXXX
        string digits = user.Phone.StartsWith("+7") ? "8" + user.Phone.Substring(2) : user.Phone;
        if (digits.Length != 11 || !digits.StartsWith("8") || !digits.All(char.IsDigit))
            errors.Add($"Телефон: \"{user.Phone}\" - ожидается +7XXXXXXXXXX или 8XXXXXXXXXX");

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }
}

internal class Program
{
    private static void Check(UserValidator validator, User user)
    {
        Console.WriteLine($"Проверка пользователя \"{user.Name}\":");
        try
        {
            validator.Validate(user);
            Console.WriteLine("  Данные корректны\n");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"  {ex.Message}");
            foreach (string error in ex.Errors)
            {
                Console.WriteLine($"   - {error}");
            }
            Console.WriteLine();
        }
    }

    private static void Main()
    {
        UserValidator validator = new UserValidator();

        Check(validator, new User { Name = "Анна Смирнова", Email = "anna@mail.ru", Age = 25, Phone = "+79161234567" });
        Check(validator, new User { Name = "R2-D2", Email = "robot@@galaxy", Age = 150, Phone = "12-34" });
        Check(validator, new User { Name = "Пётр", Email = "petr@yandex.ru", Age = 12, Phone = "89031112233" });
    }
}