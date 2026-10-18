using System;
using System.Globalization;
public enum Currency
{
    RUB,
    USD,
    EUR
}

struct BankAccount
{
    public string AccountNumber; // номер счёта
    public string Owner;         // владелец
    public decimal Balance;      // баланс
    public Currency Currency;    // валюта

    public BankAccount(string accountNumber, string owner, decimal balance, Currency currency)
    {
        AccountNumber = accountNumber;
        Owner = owner;
        Balance = balance;
        Currency = currency;
    }
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine($"  Счёт {AccountNumber}: сумма пополнения должна быть положительной");
            return;
        }
        Balance += amount;
        Console.WriteLine($"  Счёт {AccountNumber}: пополнение на {amount} {Currency}, баланс {Balance} {Currency}");
    }

    // Снятие средств с проверкой достаточности; возвращает true при успехе
    public bool Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine($"  Счёт {AccountNumber}: сумма снятия должна быть положительной");
            return false;
        }
        if (amount > Balance)
        {
            Console.WriteLine($"  Счёт {AccountNumber}: недостаточно средств (баланс {Balance} {Currency}, запрошено {amount} {Currency})");
            return false;
        }
        Balance -= amount;
        Console.WriteLine($"  Счёт {AccountNumber}: снято {amount} {Currency}, баланс {Balance} {Currency}");
        return true;
    }

    // Баланс в рублях — чтобы сравнивать счета в разных валютах
    public decimal GetBalanceInRub()
    {
        decimal rate = Currency switch
        {
            Currency.USD => 90m,  // условные курсы
            Currency.EUR => 100m,
            _ => 1m
        };
        return Balance * rate;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"{AccountNumber} | {Owner,-16} | {Balance,10:F2} {Currency} | ~ {GetBalanceInRub(),10:F2} RUB");
    }

    }
public enum ProcessorBrand
{
    Intel,
    AMD,
    Apple
}

class Program
{
    static string GetProcessorName(ProcessorBrand brand, double frequencyGHz)
    {
        string model = brand switch
        {
            ProcessorBrand.Intel => "Intel Core i7",
            ProcessorBrand.AMD => "AMD Ryzen 7",
            ProcessorBrand.Apple => "Apple M3",
            _ => "Неизвестный процессор"
        };
        return $"{model} {frequencyGHz.ToString("0.0#", CultureInfo.InvariantCulture)}GHz";
    }

    static void Main()
    {
        // ===== Задание 1 =====
        Console.WriteLine("=== Задание 1. Структура BankAccount ===\n");

        BankAccount[] accounts =
        {
            new BankAccount("40817-001", "Иванов Иван", 150000m, Currency.RUB),
            new BankAccount("40817-002", "Петрова Анна", 4000m, Currency.USD),
            new BankAccount("40817-003", "Сидоров Пётр", 1800m, Currency.EUR),
            new BankAccount("40817-004", "Козлова Мария", 250000m, Currency.RUB)
        };

        Console.WriteLine("Операции со счетами:");
        // Элемент массива — переменная, поэтому метод меняет именно его
        accounts[0].Deposit(25000m);
        accounts[1].Withdraw(700m);
        accounts[2].Withdraw(5000m);   // недостаточно средств
        accounts[3].Withdraw(-100m);   // некорректная сумма

        Console.WriteLine("\nСписок счетов:");
        foreach (BankAccount acc in accounts)
        {
            acc.DisplayInfo();
        }

        // Поиск самого богатого счёта (с пересчётом в рубли)
        int richest = 0;
        for (int i = 1; i < accounts.Length; i++)
        {
            if (accounts[i].GetBalanceInRub() > accounts[richest].GetBalanceInRub())
            {
                richest = i;
            }
        }
        Console.WriteLine($"\nСамый богатый счёт: {accounts[richest].AccountNumber}, владелец {accounts[richest].Owner} " +
                          $"(~ {accounts[richest].GetBalanceInRub():F2} RUB)");

        // Демонстрация: структура копируется при присваивании
        BankAccount copy = accounts[0];
        copy.Deposit(1m);
        Console.WriteLine($"\nПосле пополнения копии: копия = {copy.Balance}, оригинал = {accounts[0].Balance}");

        // ===== Задание 2 =====
        Console.WriteLine("\n=== Задание 2. Перечисление ProcessorBrand ===\n");
        Console.WriteLine(GetProcessorName(ProcessorBrand.Intel, 3.6));
        Console.WriteLine(GetProcessorName(ProcessorBrand.AMD, 4.2));
        Console.WriteLine(GetProcessorName(ProcessorBrand.Apple, 4.05));

        Console.WriteLine("\nВсе бренды перечисления:");
        foreach (ProcessorBrand brand in Enum.GetValues(typeof(ProcessorBrand)))
        {
            Console.WriteLine($"  {(int)brand} = {brand}");
        }
    }
}
