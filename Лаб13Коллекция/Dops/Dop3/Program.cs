using System;
using System.Collections.Generic;

internal class PhoneBook
{
    // фамилия -> список телефонов
    private Dictionary<string, List<string>> contacts = new Dictionary<string, List<string>>();

    // телефон -> фамилия (обратный словарь для поиска по номеру)
    private Dictionary<string, string> owners = new Dictionary<string, string>();

    // Добавить контакт или ещё один телефон к существующему контакту
    public void AddPhone(string surname, string phone)
    {
        if (owners.ContainsKey(phone))
        {
            Console.WriteLine($"Номер {phone} уже принадлежит {owners[phone]}");
            return;
        }
        if (!contacts.ContainsKey(surname))
            contacts[surname] = new List<string>();
        contacts[surname].Add(phone);
        owners[phone] = surname;
        Console.WriteLine($"{surname}: добавлен номер {phone}");
    }

    public void RemovePhone(string surname, string phone)
    {
        if (contacts.ContainsKey(surname) && contacts[surname].Remove(phone))
        {
            owners.Remove(phone);
            Console.WriteLine($"{surname}: удалён номер {phone}");
        }
        else
        {
            Console.WriteLine($"У {surname} нет номера {phone}");
        }
    }

    public void RemoveContact(string surname)
    {
        if (!contacts.ContainsKey(surname))
        {
            Console.WriteLine($"Контакт {surname} не найден");
            return;
        }
        foreach (string phone in contacts[surname])
            owners.Remove(phone);
        contacts.Remove(surname);
        Console.WriteLine($"Контакт {surname} удалён");
    }

    public void FindBySurname(string surname)
    {
        if (contacts.TryGetValue(surname, out List<string>? phones))
            Console.WriteLine($"Телефоны {surname}: {string.Join(", ", phones)}");
        else
            Console.WriteLine($"Контакт {surname} не найден");
    }

    public void FindByPhone(string phone)
    {
        if (owners.TryGetValue(phone, out string? surname))
            Console.WriteLine($"Номер {phone} принадлежит: {surname}");
        else
            Console.WriteLine($"Номер {phone} не найден");
    }

    public void Print()
    {
        Console.WriteLine("Телефонная книга:");
        foreach (KeyValuePair<string, List<string>> pair in contacts)
            Console.WriteLine($"  {pair.Key}: {string.Join(", ", pair.Value)}");
    }
}

internal class Program
{
    private static void Main()
    {
        PhoneBook book = new PhoneBook();
        book.AddPhone("Иванов", "+375291111111");
        book.AddPhone("Петров", "+375292222222");
        book.AddPhone("Иванов", "+375443333333");   // второй номер Иванова
        book.AddPhone("Сидоров", "+375292222222");  // номер уже занят
        book.Print();

        Console.WriteLine();
        book.FindBySurname("Иванов");
        book.FindByPhone("+375292222222");
        book.FindByPhone("+375000000000");

        Console.WriteLine();
        book.RemovePhone("Иванов", "+375291111111");
        book.RemoveContact("Петров");
        book.FindByPhone("+375292222222");
        book.Print();
    }
}