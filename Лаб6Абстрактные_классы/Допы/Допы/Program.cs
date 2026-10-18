using System;
using System.Collections.Generic;

// Абстрактный класс банковского счёта с защитой PIN-кодом
public abstract class BankAccount
{
    public string AccountNumber { get; }
    public string OwnerName { get; }
    public DateTime OpenDate { get; }

    private decimal _balance;                     // инкапсулированный баланс
    private readonly string _pinCode;              // инкапсулированный PIN
    private int _failedAttempts;
    private readonly List<string> _log = new List<string>(); // журнал операций (выписка)

    public decimal Balance => _balance;            // только чтение снаружи
    public bool IsLocked { get; private set; }

    protected BankAccount(string accountNumber, string ownerName, decimal balance, string pinCode, DateTime openDate)
    {
        AccountNumber = accountNumber;
        OwnerName = ownerName;
        _balance = balance;
        _pinCode = pinCode;
        OpenDate = openDate;
    }

    // Проверка PIN-кода с блокировкой после трёх неверных попыток
    protected bool VerifyPin(string pin)
    {
        if (IsLocked)
        {
            Console.WriteLine($"Счёт {AccountNumber} заблокирован - операции недоступны");
            return false;
        }
        if (pin == _pinCode)
        {
            _failedAttempts = 0;
            return true;
        }
        _failedAttempts++;
        Console.WriteLine($"Неверный PIN ({_failedAttempts}/3)");
        if (_failedAttempts >= 3)
        {
            IsLocked = true;
            Console.WriteLine($"Счёт {AccountNumber} заблокирован после трёх неверных попыток");
        }
        return false;
    }

    protected void ChangeBalance(decimal delta, string description)
    {
        _balance += delta;
        _log.Add($"{description}: {delta:+0.00;-0.00} руб., остаток {_balance:F2} руб.");
    }

    public abstract bool Deposit(decimal amount, string pin);
    public abstract bool Withdraw(decimal amount, string pin);

    public void PrintStatement()
    {
        Console.WriteLine($"Выписка по счёту {AccountNumber} ({OwnerName}):");
        foreach (var entry in _log)
            Console.WriteLine($"  {entry}");
    }
}

public class SavingsAccount : BankAccount
{
    public double InterestRate { get; } // процент в месяц

    public SavingsAccount(string accountNumber, string ownerName, decimal balance, string pin, DateTime openDate, double interestRate)
        : base(accountNumber, ownerName, balance, pin, openDate)
    {
        InterestRate = interestRate;
    }

    public override bool Deposit(decimal amount, string pin)
    {
        if (!VerifyPin(pin)) return false;
        ChangeBalance(amount, "Пополнение");
        return true;
    }

    public override bool Withdraw(decimal amount, string pin)
    {
        if (!VerifyPin(pin)) return false;
        if (amount > Balance)
        {
            Console.WriteLine("Недостаточно средств на сберегательном счёте");
            return false;
        }
        ChangeBalance(-amount, "Снятие");
        return true;
    }

    public void ApplyMonthlyInterest()
    {
        decimal interest = Balance * (decimal)(InterestRate / 100);
        ChangeBalance(interest, "Начисление процентов");
    }
}

public class CheckingAccount : BankAccount
{
    private const decimal OperationFee = 30m;

    public CheckingAccount(string accountNumber, string ownerName, decimal balance, string pin, DateTime openDate)
        : base(accountNumber, ownerName, balance, pin, openDate) { }

    public override bool Deposit(decimal amount, string pin)
    {
        if (!VerifyPin(pin)) return false;
        ChangeBalance(amount - OperationFee, "Пополнение (с комиссией)");
        return true;
    }

    public override bool Withdraw(decimal amount, string pin)
    {
        if (!VerifyPin(pin)) return false;
        ChangeBalance(-(amount + OperationFee), "Снятие (с комиссией)");
        return true;
    }
}

public class CreditAccount : BankAccount
{
    public decimal CreditLimit { get; }

    public CreditAccount(string accountNumber, string ownerName, decimal balance, string pin, DateTime openDate, decimal creditLimit)
        : base(accountNumber, ownerName, balance, pin, openDate)
    {
        CreditLimit = creditLimit;
    }

    public override bool Deposit(decimal amount, string pin)
    {
        if (!VerifyPin(pin)) return false;
        ChangeBalance(amount, "Погашение долга");
        return true;
    }

    public override bool Withdraw(decimal amount, string pin)
    {
        if (!VerifyPin(pin)) return false;
        if (Balance - amount < -CreditLimit)
        {
            Console.WriteLine("Превышен кредитный лимит");
            return false;
        }
        ChangeBalance(-amount, "Снятие в счёт кредита");
        return true;
    }
}

class Program
{
    static void Main()
    {
        var savings = new SavingsAccount("SA-100", "Дмитрий", 10000m, "1234", new DateTime(2026, 1, 10), interestRate: 2);
        var checking = new CheckingAccount("CA-200", "Ольга", 5000m, "4321", new DateTime(2026, 2, 1));
        var credit = new CreditAccount("CR-300", "Игорь", 0m, "5555", new DateTime(2026, 3, 5), creditLimit: 20000m);

        Console.WriteLine("Операции по сберегательному счёту:");
        savings.Deposit(2000m, "1234");
        savings.Withdraw(500m, "1234");
        savings.ApplyMonthlyInterest();
        savings.PrintStatement();

        Console.WriteLine("\nПопытки с неверным PIN на расчётном счёте:");
        checking.Withdraw(100m, "0000"); // 1
        checking.Withdraw(100m, "9999"); // 2
        checking.Withdraw(100m, "0001"); // 3 -> блокировка
        checking.Withdraw(100m, "4321"); // счёт уже заблокирован

        Console.WriteLine("\nОперации по кредитному счёту:");
        credit.Withdraw(15000m, "5555");
        credit.Withdraw(10000m, "5555"); // 15000+10000=25000 > лимита 20000
        credit.PrintStatement();
    }
}
